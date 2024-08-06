using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.CQRS.Shipment.Commands;
public class UpdateShipmentCommand
{
    [Required]
    public string? Guid { get; set; }
    [Required]
    public int ClientId { get; set; }
    [Required]
    public string? Origin { get; set; }
    [Required]
    public string? Destination { get; set; }
    [Required]
    public ShipmentStatusEnum Status { get; set; }
}