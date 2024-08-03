using System;
using System.Collections.Generic;

namespace InfrastructureLayer.Models;

public partial class NlogDelete
{
    public int Id { get; set; }

    public int OriginalId { get; set; }

    public string? MachineName { get; set; }

    public DateTime Logged { get; set; }

    public string Level { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? Logger { get; set; }

    public string? Properties { get; set; }

    public string? Exception { get; set; }

    public DateTime OperationTime { get; set; }

    public string TransactionId { get; set; } = null!;
}
