namespace ApplicationLayer.DTOs.User;
public record TokenResponseDTO(string? Token) : ServiceResponse(true, string.Empty, DateTime.UtcNow);
