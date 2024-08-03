using System;
using System.Collections.Generic;

namespace InfrastructureLayer.Models;

public partial class Nlog
{
    public int Id { get; set; }

    public string? MachineName { get; set; }

    public DateTime Logged { get; set; }

    public string Level { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? Logger { get; set; }

    public string? Properties { get; set; }

    public string? Exception { get; set; }
}
