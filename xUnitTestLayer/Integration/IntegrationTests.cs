using Newtonsoft.Json;
using System.Text;

namespace xUnitTestLayer.Integration
{
    public class IntegrationTests
    {
        private readonly HttpClient _httpClient;

        public IntegrationTests()
        {
            _httpClient = new HttpClient();
        }

        [Fact]
        public async Task TestExternalApiIntegration()
        {
            // Arrange
            var requestBody = new
            {
                from = new { postal_code = 7307045 },
                to = new { postal_code = 80010000 },
                package = new { height = 4.0, width = 4.0, length = 4.0, weight = 4.0 }
            };
            
            var apiUrl = "https://www.melhorenvio.com.br/api/v2/me/shipment/calculate";

            // Act
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", Environment.GetEnvironmentVariable("ASPNETCORE_FRENETEXAM_EXTERNALTOKEN_DEV"));

            var response = await _httpClient.PostAsync(apiUrl, jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);   
        }
    }
}

