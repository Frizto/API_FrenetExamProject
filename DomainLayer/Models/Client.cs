using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class Client
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? AspNetUserId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
