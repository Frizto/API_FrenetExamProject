using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.DTOs;
using ApplicationLayer.Extensions;
using InfrastructureLayer.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace InfrastructureLayer.Handlers.User;
sealed class DeleteUserHandler(UserManager<AppUser> userManager,
    AppDbContext appDbContext) : ICommandHandler<DeleteUserCommand, ServiceResponse>
{

    private static readonly Logger DeleteLogger = LogManager.GetLogger("DeleteLogger");
    private static readonly Logger ErrorLogger = LogManager.GetLogger("ErrorLogger");

    public async Task<ServiceResponse> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // 1. Find the user by Id.
            var user = await userManager.FindByIdAsync(command.Id) 
                ?? throw new Exception("User not found");

            // 2. Delete the user.
            var deleteResult = await userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                throw new Exception("Failed when deleting the user");
            }

            // 3. Delete the client associated with the user.
            var client = await appDbContext.Clients.FirstOrDefaultAsync(c => c.AspNetUserId == command.Id, cancellationToken);
            if (client != null)
            {
                appDbContext.Clients.Remove(client);
            }

            // 4. Save the changes.
            var resultDb = await appDbContext.SaveChangesAsync(cancellationToken);
            if (resultDb == 0)
            {
                throw new Exception("Failed when saving user deletion to Db");
            }

            // 5. If all operations succeed, commit the transaction.
            transaction.Commit();

            // 6. Create the Log.
            using (ScopeContext.PushProperty("TransactionId", Guid.NewGuid().ToString()))
            {
                ScopeContext.PushProperty("EntityId", user.Id);
                DeleteLogger.Info("User Deleted Successfully");
            }

            return new ServiceResponse(true, "User Deleted Successfully", DateTime.UtcNow);
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
