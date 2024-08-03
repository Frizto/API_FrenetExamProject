using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.CQRS.Client.Commands;
public class CreateClientCommand
{
    // Base Properties
    [Required]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name is too short.")]
    public string? Name { get; set; }
    [Required, EmailAddress]
    public string? Email { get; set; }
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,40}$", ErrorMessage = "Password must be between 8 and 40 characters and contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character.")]
    public required string Password { get; set; }
    [Required, Compare(nameof(Password))]
    public required string ConfirmPassword { get; set; }
}
