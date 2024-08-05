using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.CQRS.Shipment.Queries;
public class ShipmentPricingQuery
{
    [Required]
    public string? FromPostalCode { get; set; }
    [Required]
    public string? ToPostalCode { get; set; }
    [Required]
    public double Height { get; set; }
    [Required]
    public double Width { get; set; }
    [Required]
    public double Length { get; set; }
    [Required]
    public double Weight { get; set; }
}
