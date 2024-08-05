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
        { "Aracaju", "49000-000" },
        { "Belem", "66000-000" },
        { "Belo Horizonte", "30000-000" },
        { "Boa Vista", "69300-000" },
        { "Brasilia", "70000-000" },
        { "Campo Grande", "79000-000" },
        { "Cuiaba", "78000-000" },
        { "Curitiba", "80000-000" },
        { "Florianopolis", "88000-000" },
        { "Fortaleza", "60000-000" },
        { "Goiania", "74000-000" },
        { "Joao Pessoa", "58000-000" },
        { "Macapa", "68900-000" },
        { "Maceio", "57000-000" },
        { "Manaus", "69000-000" },
        { "Natal", "59000-000" },
        { "Palmas", "77000-000" },
        { "Porto Alegre", "90000-000" },
        { "Porto Velho", "76800-000" },
        { "Recife", "50000-000" },
        { "Rio Branco", "69900-000" },
        { "Rio de Janeiro", "20000-000" },
        { "Salvador", "40000-000" },
        { "Sao Luis", "65000-000" },
        { "Sao Paulo", "01000-000" },
        { "Teresina", "64000-000" },
        { "Vitoria", "29000-000" }
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

        requestBody = new
        {
            from = new { postal_code = query.FromPostalCode },
            to = new { postal_code = query.ToPostalCode },
            package = new { height = query.Height, width = query.Width, length = query.Length, weight = query.Weight }
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("Authorization", Environment.GetEnvironmentVariable("ASPNETCORE_FRENETEXAM_TOKEN_DEV"));

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

