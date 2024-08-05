using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.Shipment.Queries;
using Newtonsoft.Json;
using System.Text;

namespace InfrastructureLayer.Handlers.Shipment;
public sealed class ShipmentPricingHandler(HttpClient httpClient) : IQueryHandler<ShipmentPricingQuery, ShipmentPricingDTO>
{
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
}

