﻿using System;
using System.Collections.Generic;

namespace DomainLayer.Models;

public partial class Address
{
    public int Id { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ZipCode { get; set; }

    public int? ClientId { get; set; }

    public virtual Client? Client { get; set; }
}
