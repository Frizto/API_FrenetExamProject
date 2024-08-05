using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.Shipment.Commands;
using ApplicationLayer.DTOs;
using InfrastructureLayer.DataAccess;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace InfrastructureLayer.Handlers.Shipment;
sealed class CreateShipmentHandler(AppDbContext appDbContext) : ICommandHandler<CreateShipmentCommand, ServiceResponse>
{
    // 0. Create logger instances
    private static readonly Logger CreateLogger = LogManager.GetLogger("CreateLogger");
    private static readonly Logger ErrorLogger = LogManager.GetLogger("ErrorLogger");
    public async Task<ServiceResponse> Handle(CreateShipmentCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // 1. Create the AspNetUser for credentials.
            var shipmentData = new DomainLayer.Models.Shipment
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = command.ClientId,
                Origin = command.Origin!,
                CreationDate = DateTime.UtcNow,
                Destination = command.Destination!,
                Status = command.Status.ToString()
            };

            var createShipmentResult = await appDbContext.Shipments
                .AddAsync(shipmentData, cancellationToken);
            if (createShipmentResult.State != EntityState.Added)
            {
                throw new Exception("Failed when creating the Shipment Order");
            }

            // 2a. Save the changes.
            var resultDb = await appDbContext.SaveChangesAsync(cancellationToken);
            if (resultDb == 0)
            {
                throw new Exception("Failed when saving shipment order creation to Db");
            }

            // 3b. If all operations succeed, commit the transaction.
            await transaction.CommitAsync(cancellationToken);

            // 4. Create the Log.
            using (ScopeContext.PushProperty("TransactionId", Guid.NewGuid().ToString()))
            {
                ScopeContext.PushProperty("EntityId", shipmentData.Id);
                CreateLogger.Info("Shipment Order Created Successfully");
            }

            return new ServiceResponse(true, "Shipment Order Created Successfully", DateTime.UtcNow);

        }
        catch (Exception ex)
        {
            // If an unexpected error occurs, roll back the transaction.
            transaction.Rollback();
            ErrorLogger.Error(ex, ex.Message);
            return new ServiceResponse(false, ex.Message, DateTime.UtcNow);
        }
    }
}
