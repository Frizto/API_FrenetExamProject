using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.CQRS.User.Queries;
public class RefreshUserTokenQuery
{
    [Required]
    public string? Token { get; set; }
}
