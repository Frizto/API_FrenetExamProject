using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.CQRS.Shipment.Commands;
public class DeleteShipmentCommand
{
    [Required]
    public string? Guid { get; set; }
    [Required]
    public int ClientId { get; set; }
}
