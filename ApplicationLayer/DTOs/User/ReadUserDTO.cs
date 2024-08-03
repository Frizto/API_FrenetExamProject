using DomainLayer.Enums;

namespace ApplicationLayer.DTOs.User;
public record  ReadUserDTO(
    bool Flag,
    string? Id, 
    string? Name, 
    string? Email, 
    string? Phone,
    string? Message,
    DateTime DateTime);
