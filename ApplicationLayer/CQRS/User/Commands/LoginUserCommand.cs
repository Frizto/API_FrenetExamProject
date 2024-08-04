using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.CQRS.User.Commands;
public class LoginUserCommand
{
    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required, MinLength(8), MaxLength(40)]
    public string? Password { get; set; }
}
