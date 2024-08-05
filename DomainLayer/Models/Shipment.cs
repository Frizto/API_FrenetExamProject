using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class Shipment
{
    public string? Id { get; set; }

    public int ClientId { get; set; }

    public string Origin { get; set; } = null!;

    public string Destination { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;
}
