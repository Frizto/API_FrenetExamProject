using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.CQRS.User.Commands;
public class CreateUserCommand
{
    [Required]
    [RegularExpression(@"^[a-zA-Z\s]{8,40}$", ErrorMessage = "Name can only contain letters and spaces.")]
    public string? Name { get; set; }
    [Required, EmailAddress(ErrorMessage = "Email is Required")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Phone is Required"), Phone(ErrorMessage = "Phone is not valid")]
    public string? Phone { get; set; }
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,40}$", ErrorMessage = "Password must be at least 8 characters and contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character.")]
    public string? Password { get; set; }
    [Required, Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; }
    [Required(ErrorMessage = "User Role is Required")]
    public AppUserTypeEnum UserRole { get; set; } = AppUserTypeEnum.Client;
}
