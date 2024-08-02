using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InfrastructureLayer.DataAccess;
public class AppUser : IdentityUser
{
    // Base Properties
    [Required]
    [RegularExpression(@"^[a-zA-Z\s]{8,40}$", ErrorMessage = "Name can only contain letters and spaces.")]
    public string? Name { get; set; }
    [Required]
    public string? UserType { get; set; }
}
