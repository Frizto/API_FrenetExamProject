using DomainLayer.Enums;

namespace ApplicationLayer.DTOs.User;
public record  ReadUserDTO(
    string? Id, 
    string? Name, 
    string? Email, 
    string? Phone) : ServiceResponse(true, string.Empty, DateTime.UtcNow);
