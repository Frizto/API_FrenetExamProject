namespace ApplicationLayer.CQRS.Shipment.Queries;
public class ReadShipmentQuery(string? id)
{
    public string? Id { get; set; } = id;
}
