using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.Shipment.Commands;
using ApplicationLayer.DTOs;
using InfrastructureLayer.DataAccess;
using InfrastructureLayer.Loggers;
using NLog;

namespace InfrastructureLayer.Handlers.Shipment;
public sealed class UpdateShipmentHandler(AppDbContext appDbContext) : ICommandHandler<UpdateShipmentCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(UpdateShipmentCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // 0a. Find the shipment data
            var shipmentData = await appDbContext.Shipments
                .FindAsync(command.Id)
                ?? throw new Exception("Shipment not found");

            // 0b. Check if the User is the owner of the shipment
            if (shipmentData.ClientId != command.ClientId)
            {
                throw new Exception("You are not allowed to Update this Shipment");
            }

            // 1. Update the shipment data
            shipmentData.Origin = command.Origin!;
            shipmentData.Destination = command.Destination!;
            shipmentData.Status = command.Status.ToString();

            // 2. Save the changes
            var resultDb = await appDbContext.SaveChangesAsync(cancellationToken);
            if (resultDb == 0)
            {
                throw new Exception("Failed when saving shipment update to Db");
            }

            // 3. Commit the transaction
            await transaction.CommitAsync(cancellationToken);

            // 4. Create the log
            if (appDbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                using (ScopeContext.PushProperty("TransactionId", Guid.NewGuid().ToString()))
                {
                    ScopeContext.PushProperty("EntityId", shipmentData.Id);
                    CustomLoggers.UpdateLogger.Info("Shipment Updated Successfully");
                }
            }

            return new ServiceResponse(true, "Shipment Updated Successfully", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            // If an unexpected error occurs, roll back the transaction
            transaction.Rollback();
            if (appDbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                CustomLoggers.ErrorLogger.Error(ex, ex.Message);
            }
            return new ServiceResponse(false, ex.Message, DateTime.UtcNow);
        }
    }
}
