﻿using System;
using System.Collections.Generic;
using InfrastructureLayer.TestsModels;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.DataAccess;

public class LogsDbContext(DbContextOptions<LogsDbContext> options) : DbContext(options)
{
    public virtual DbSet<Nlog> Nlogs { get; set; }

    public virtual DbSet<NlogCreate> NlogCreates { get; set; }

    public virtual DbSet<NlogDelete> NlogDeletes { get; set; }

    public virtual DbSet<NlogUpdate> NlogUpdates { get; set; }
}
