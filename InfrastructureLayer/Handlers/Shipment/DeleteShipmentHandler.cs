using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.Shipment.Commands;
using ApplicationLayer.DTOs;
using InfrastructureLayer.DataAccess;
using InfrastructureLayer.Loggers;
using NLog;

namespace InfrastructureLayer.Handlers.Shipment;
public sealed class DeleteShipmentHandler(AppDbContext appDbContext) : ICommandHandler<DeleteShipmentCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(DeleteShipmentCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // 1a. Find the shipment data.
            var shipmentData = await appDbContext.Shipments
                .FindAsync(command.Id)
                ?? throw new Exception("Shipment not found");

            // 1b. Check if the User is the owner of the shipment
            if (shipmentData.ClientId != command.ClientId)
            {
                throw new Exception("You are not allowed to Delete this Shipment");
            }

            // 2. Remove the shipment data.
            appDbContext.Shipments.Remove(shipmentData);

            var resultDb = await appDbContext.SaveChangesAsync(cancellationToken);
            if (resultDb == 0)
            {
                throw new Exception("Failed when deleting shipment from Db");
            }

            // 3. If all operations succeed, commit the transaction.
            await transaction.CommitAsync(cancellationToken);

            // 4. Create the Log.
            if (appDbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                using (ScopeContext.PushProperty("TransactionId", Guid.NewGuid().ToString()))
                {
                    ScopeContext.PushProperty("EntityId", shipmentData.Id);
                    CustomLoggers.DeleteLogger.Info("Shipment Deleted Successfully");
                }
            }

            return new ServiceResponse(true, "Shipment Deleted Successfully", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            if (appDbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                CustomLoggers.ErrorLogger.Error(ex, ex.Message);
            }
            return new ServiceResponse(false, ex.Message, DateTime.UtcNow);
        }
    }
}
