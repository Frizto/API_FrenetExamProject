namespace ApplicationLayer.DTOs.User;
public record TokenResultDTO(bool Flag, 
    string? Token = null, 
    string? Message = "") : ServiceResponse(Flag, Message, DateTime.UtcNow);
