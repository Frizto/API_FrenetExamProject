using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.Shipment.Commands;
using ApplicationLayer.CQRS.Shipment.Queries;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Shipment;
using DomainLayer.Enums;
using InfrastructureLayer.DataAccess;
using InfrastructureLayer.Handlers.Shipment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace xUnitTestLayer.Handlers
{
    public class ShipmentPricingHandlerTests
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _dbContext;
        private readonly ICommandHandler<CreateShipmentCommand, ServiceResponse> _createHandler;
        private readonly IQueryHandler<ReadShipmentQuery, ReadShipmentDTO> _shipHandler;
        private readonly IQueryHandler<ShipmentPricingQuery, ShipmentPricingDTO> _pricingHandler;

        public ShipmentPricingHandlerTests()
        {
            _httpClient = new HttpClient();
            var _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

            _dbContext = new AppDbContext(_options);
            _createHandler = new CreateShipmentHandler(_dbContext);
            _shipHandler = new ReadShipmentHandler(_dbContext);
            _pricingHandler = new ShipmentPricingHandler(_httpClient, _shipHandler);
        }

        [Fact]
        public async void GetPricing_ReturnsFromShipmentId()
        {
            // 1. Lets add a new shipment to the database
            // Arrange
            var newShipment = new CreateShipmentCommand
            {
                ClientId = 1,
                Origin = "São Paulo",
                Destination = "Curitiba",
                Status = ShipmentStatusEnum.Processing
            };

            // Act
            var createResult = _createHandler.Handle(newShipment, CancellationToken.None);

            // Assert
            Assert.True(createResult.Result.Flag);

            // 2. Now lets get the pricing from the shipmentId
            var shipmentIdObj = new ShipmentPricingQuery
            {
                ShipmentId = _dbContext.Shipments
                .Where(x => x.Origin == "São Paulo"
                && x.Destination == "Curitiba"
                && x.ClientId == newShipment.ClientId).FirstOrDefault()?.Id
            };

            // Act
            var result = await _pricingHandler.HandleListAsync(shipmentIdObj, CancellationToken.None);
            var company = result.FirstOrDefault(x => x.Company.Name == "Correios");
            var type = company.Name;
            var price = double.Parse(company.Price) / 100;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(company);
            Assert.True(price == 23.76);
            Assert.Equal("PAC", type);
        }
    }
}
