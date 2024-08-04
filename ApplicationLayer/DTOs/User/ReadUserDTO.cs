using DomainLayer.Enums;

namespace ApplicationLayer.DTOs.User;
public record  ReadUserDTO(
    bool Flag,
    string? Id = null, 
    string? Name = null, 
    string? Email = null, 
    string? Phone = null,
    string? Message = null) : ServiceResponse(Flag, Message, DateTime.UtcNow);
