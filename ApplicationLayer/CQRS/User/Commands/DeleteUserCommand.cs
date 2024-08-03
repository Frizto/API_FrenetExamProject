using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.CQRS.User.Commands;

public class DeleteUserCommand
{
    [Required(ErrorMessage = "User Id is Required")]
    public string? Id { get; set; }
}