using System;
using System.Collections.Generic;

namespace InfrastructureLayer.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? AddressId { get; set; }

    public virtual Address? Address { get; set; }
}
