namespace ApplicationLayer.CQRS.Shipment.Queries;
public class ShipmentPricingQuery
{
    public string? ShipmentId { get; set; }

    public string? FromPostalCode { get; set; }

    public string? ToPostalCode { get; set; }

    public double Height { get; set; }

    public double Width { get; set; }

    public double Length { get; set; }

    public double Weight { get; set; }
}
