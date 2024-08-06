using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.Shipment.Queries;
using ApplicationLayer.DTOs.Shipment;
using InfrastructureLayer.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Handlers.Shipment;
public sealed class ReadShipmentHandler(AppDbContext appDbContext) : IQueryHandler<ReadShipmentQuery, ReadShipmentDTO>
{
    public async Task<ReadShipmentDTO> Handle(ReadShipmentQuery query, CancellationToken cancellationToken)
    {
        var dbShipment = await appDbContext.Shipments
            .Where(s => s.Id == query.Guid!)
            .Select(s => new ReadShipmentDTO(true, s.Id!.ToString(), s.ClientId.ToString(), s.Origin, s.Destination, s.Status, "Shipment Found!"))
            .FirstOrDefaultAsync(cancellationToken) ?? new ReadShipmentDTO(false);
        return dbShipment;
    }

    public async Task<IEnumerable<ReadShipmentDTO>> HandleListAsync(ReadShipmentQuery query, CancellationToken cancellationToken)
    {
        List<ReadShipmentDTO> shipments = [];
        if (query.Guid is not null)
        {
            var shipment = await Handle(query, cancellationToken);
            shipments.Add(shipment);
            return shipments;
        }

        shipments = await appDbContext.Shipments
            .Select(s => new ReadShipmentDTO(true, s.Id!.ToString(), s.ClientId.ToString(), s.Origin, s.Destination, s.Status, "Shipment Found!"))
            .ToListAsync(cancellationToken);

        if (shipments.Count == 0)
        {
            shipments.Add(new ReadShipmentDTO(false));
        }

        return shipments;
    }
}

