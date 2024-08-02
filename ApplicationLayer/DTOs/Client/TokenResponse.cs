namespace ApplicationLayer.DTOs.Client;
public record TokenResponse(string? Token) : ServiceResponse(true, null);
