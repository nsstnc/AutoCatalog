using System;
using System.Collections.Generic;

namespace AutoCatalog;

public partial class Transmission
{
    public long? Id { get; set; }

    public string? Type { get; set; }

    public long? NumberOfGears { get; set; }

    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();
}
