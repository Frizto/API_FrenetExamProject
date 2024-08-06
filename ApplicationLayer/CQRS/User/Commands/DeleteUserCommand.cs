using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.CQRS.User.Commands;

public class DeleteUserCommand
{
    [Required(ErrorMessage = "User Guid is Required")]
    public string? Guid { get; set; }
}