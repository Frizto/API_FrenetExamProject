﻿using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Queries;
using ApplicationLayer.DTOs.User;
using InfrastructureLayer.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Handlers.User;
sealed class ReadUserHandler(AppDbContext appDbContext) : IQueryHandler<ReadUserQuery, ReadUserDTO>
{
    private readonly ReadUserDTO Dummy = new(false, null, null, null, null, null, DateTime.UtcNow);
    public async Task<ReadUserDTO> Handle(ReadUserQuery query, CancellationToken cancellationToken)
    {

        var dbUser = await appDbContext.Clients
            .Where(u => u.Id == Int32.Parse(query.Id!))
            .Select(u => new ReadUserDTO(true, u.Id.ToString(), u.Name, u.Email, u.Phone, "User Found!", DateTime.UtcNow))
            .FirstOrDefaultAsync(cancellationToken) ?? Dummy;
        return dbUser;
    }

    public async Task<IEnumerable<ReadUserDTO>> HandleListAsync(ReadUserQuery query, CancellationToken cancellationToken)
    {
        List<ReadUserDTO> users = [];
        if (query.Id is not null)
        {
            var user = await Handle(query, cancellationToken);
            users.Add(user);
            return users;
        }

        users = await appDbContext.Clients
                    .Select(u => new ReadUserDTO(true, u.Id.ToString(), u.Name, u.Email, u.Phone, "User Found!", DateTime.UtcNow))
                    .ToListAsync(cancellationToken);

        if (users.Count == 0)
        {
            users.Add(Dummy);
        }

        return users;
    }
}
