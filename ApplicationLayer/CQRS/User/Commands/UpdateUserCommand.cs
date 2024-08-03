using DomainLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.CQRS.User.Commands;
public class UpdateUserCommand
{
    [Required]
    public string? Id { get; set; }
    [RegularExpression(@"^[a-zA-Z\s]{8,40}$", ErrorMessage = "Name can only contain letters and spaces.")]
    public string? Name { get; set; }
    [EmailAddress(ErrorMessage = "Email is Required")]
    public string? Email { get; set; }
    [Phone(ErrorMessage = "Phone is not valid")]
    public string? Phone { get; set; }
    [Required(ErrorMessage = "User Role is Required")]
    public AppUserTypeEnum UserRole { get; set; } = AppUserTypeEnum.Client;
}
