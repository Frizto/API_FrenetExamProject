namespace ApplicationLayer.DTOs.Shipment;
public record ReadShipmentDTO(
    bool Flag,
    string? Id = null,
    string? ClientId = null,
    string? Origin = null,
    string? Destination = null,
    string? Status = null,
    string? Message = null) : ServiceResponse(Flag, Message, DateTime.UtcNow);
