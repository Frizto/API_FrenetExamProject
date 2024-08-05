using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.CQRS.Shipment.Queries;
public class ShipmentPricingQuery
{
    [Display(Name = "A Valid Shipment Guid")]
    public string? ShipmentId { get; set; }

    [Required, Display(Name = "From Postal Code")]
    public string? FromPostalCode { get; set; }

    [Required, Display(Name = "To Postal Code")]
    public string? ToPostalCode { get; set; }

    [Required, Display(Name = "Height (in cm)")]
    public double Height { get; set; }

    [Required, Display(Name = "Width (in cm)")]
    public double Width { get; set; }

    [Required, Display(Name = "Length (in cm)")]
    public double Length { get; set; }

    [Required, Display(Name = "Weight (in kg)")]
    public double Weight { get; set; }
}
