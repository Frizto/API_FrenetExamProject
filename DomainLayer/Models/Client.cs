using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Models;

public partial class Client
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }
    // Navigation Properties
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    public string? AspNetUserId { get; set; }
}
