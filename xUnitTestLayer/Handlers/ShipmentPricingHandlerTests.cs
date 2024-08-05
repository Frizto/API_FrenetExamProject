using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.Shipment.Queries;
using ApplicationLayer.DTOs.Shipment;
using InfrastructureLayer.Handlers.Shipment;
using Moq;

namespace xUnitTestLayer.Handlers
{
    public class ShipmentPricingHandlerTests
    {
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly Mock<IQueryHandler<ReadShipmentQuery, ReadShipmentDTO>> _mockReadHandler;
        private readonly ShipmentPricingHandler _handler;

        public ShipmentPricingHandlerTests()
        {
            _mockHttpClient = new Mock<HttpClient>();
            _mockReadHandler = new Mock<IQueryHandler<ReadShipmentQuery, ReadShipmentDTO>>();
            _handler = new ShipmentPricingHandler(_mockHttpClient.Object, _mockReadHandler.Object);
        }

        [Fact]
        public void GetPostalCodes_ReturnsCorrectPostalCodes()
        {
            // Arrange
            var cityOrigin = "São Paulo";
            var cityDestination = "Rio de Janeiro";

            // Act
            var result = _handler.GetPostalCodes(cityOrigin, cityDestination);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("01001000", result?.Key);
            Assert.Equal("20010000", result?.Value);
        }
    }
}
