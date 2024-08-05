namespace ApplicationLayer.CQRS.Shipment.Queries;
public class ReadShipmentQuery
{
    private string shipmentId;

    public ReadShipmentQuery(string shipmentId)
    {
        this.shipmentId = shipmentId;
    }

    public string? Id { get; set; }
}
