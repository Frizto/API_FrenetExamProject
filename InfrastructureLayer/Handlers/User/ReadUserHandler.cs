using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Queries;
using ApplicationLayer.DTOs.User;
using InfrastructureLayer.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Handlers.User;
sealed class ReadUserHandler(AppDbContext appDbContext) : IQueryHandler<ReadUserQuery, ReadUserDTO>
{
    public async Task<ReadUserDTO> Handle(ReadUserQuery query, CancellationToken cancellationToken)
    {
        var dbUser = await appDbContext.Clients
            .Where(u => u.Id == Int32.Parse(query.Id))
            .Select(u => new ReadUserDTO(u.Id.ToString(), u.Name, u.Email, u.Phone))
            .FirstOrDefaultAsync(cancellationToken) ?? new ReadUserDTO(null, null, null, null);
        return dbUser;
    }

    public async Task<IEnumerable<ReadUserDTO>> HandleListAsync(ReadUserQuery query, CancellationToken cancellationToken)
    {
        List<ReadUserDTO> users = [];
        if (query.Id is not null)
        {
            var adminUser = await Handle(query, cancellationToken);
            users.Add(adminUser);
            return users;
        }

        try
        {
            users = await appDbContext.Clients
                        .Select(u => new ReadUserDTO(u.Id.ToString(), u.Name, u.Email, u.Phone))
                        .ToListAsync(cancellationToken);

            if (users.Count == 0)
            {
                users.Add(new ReadUserDTO(null, null, null, null));
            }

            return users;
        }
        catch (Exception ex)
        {

            return [new(null, null, null, null)];
        }
    }
}
