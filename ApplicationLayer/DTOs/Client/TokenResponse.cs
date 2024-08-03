namespace ApplicationLayer.DTOs.Client;
public record TokenResponse(string? Token) : ServiceResponse(true, string.Empty, DateTime.UtcNow);
