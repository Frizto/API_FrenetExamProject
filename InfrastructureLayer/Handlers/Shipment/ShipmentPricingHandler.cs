using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.Shipment.Queries;
using ApplicationLayer.DTOs.Shipment;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace InfrastructureLayer.Handlers.Shipment;
public sealed class ShipmentPricingHandler(HttpClient httpClient,
    IQueryHandler<ReadShipmentQuery, ReadShipmentDTO> readHandler) : IQueryHandler<ShipmentPricingQuery, ShipmentPricingDTO>
{
    private static Dictionary<string, string> Ceps = new()
    {
        { "aracaju", "49010000" },
        { "belem", "66010000" },
        { "belo horizonte", "30140071" },
        { "boa vista", "69301000" },
        { "brasilia", "70040902" },
        { "campo grande", "79002000" },
        { "cuiaba", "78010000" },
        { "curitiba", "80010000" },
        { "florianopolis", "88010000" },
        { "fortaleza", "60110000" },
        { "goiania", "74003010" },
        { "joao pessoa", "58010000" },
        { "macapa", "68900073" },
        { "maceio", "57010000" },
        { "manaus", "69010000" },
        { "natal", "59010000" },
        { "palmas", "77001002" },
        { "porto alegre", "90010000" },
        { "porto velho", "76801000" },
        { "recife", "50010000" },
        { "rio branco", "69900000" },
        { "rio de janeiro", "20010000" },
        { "salvador", "40010000" },
        { "sao luis", "65010000" },
        { "sao paulo", "01001000" },
        { "teresina", "64001000" },
        { "vitoria", "29010000" }
    };

    public async Task<ShipmentPricingDTO> Handle(ShipmentPricingQuery command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ShipmentPricingDTO>> HandleListAsync(ShipmentPricingQuery query, CancellationToken cancellationToken)
    {
        var requestUri = "https://www.melhorenvio.com.br/api/v2/me/shipment/calculate";
        var requestBody = new
        {
            from = new { postal_code = query.FromPostalCode },
            to = new { postal_code = query.ToPostalCode },
            package = new { height = query.Height, width = query.Width, length = query.Length, weight = query.Weight }
        };

        ShipmentPricingQuery _query = query;
        if (query.ShipmentId is not null)
        {
            var shipment = await readHandler.Handle(new ReadShipmentQuery(query.ShipmentId), cancellationToken);
            var result = GetPostalCodes(shipment.Origin, shipment.Destination);
            if (result is not null)
            {
                requestBody = new
                {
                    from = new { postal_code = result.Value.Key },
                    to = new { postal_code = result.Value.Value },
                    package = new { height = 4.0, width = 12.0, length = 17.0, weight = 0.8 }
                };
            }
        }

        var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("Authorization", Environment.GetEnvironmentVariable("ASPNETCORE_FRENETEXAM_EXTERNALTOKEN_DEV"));

        var response = await httpClient.PostAsync(requestUri, jsonContent);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ShipmentPricingDTO>>(responseContent);
        }
        else
        {
            throw new Exception($"Error retrieving shipment pricing: {response.ReasonPhrase}");
        }
    }

    public KeyValuePair<string, string>? GetPostalCodes(string cityOrigin, string cityDestination)
    {
        string? postalCodeOrigin = null;
        string? postalCodeDestination = null;

        var normalizedCityOrigin = NormalizeString(cityOrigin);
        var normalizedCityDestination = NormalizeString(cityDestination);

        if (Ceps.TryGetValue(normalizedCityOrigin, out var postalCode1))
        {
            postalCodeOrigin = postalCode1;
        }

        if (Ceps.TryGetValue(normalizedCityDestination, out var postalCode2))
        {
            postalCodeDestination = postalCode2;
        }

        if (postalCodeOrigin != null && postalCodeDestination != null)
        {
            return new KeyValuePair<string, string>(postalCodeOrigin, postalCodeDestination);
        }

        return null;
    }

    private string NormalizeString(string input)
    {
        var normalizedString = input.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower();
    }
}

