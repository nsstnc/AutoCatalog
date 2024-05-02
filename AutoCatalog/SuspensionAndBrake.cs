using System;
using System.Collections.Generic;

namespace AutoCatalog;

public partial class SuspensionAndBrake
{
    public long Id { get; set; }

    public string? TypeOfFrontSuspension { get; set; }

    public string? TypeOfBackSuspension { get; set; }

    public string? FrontBrakes { get; set; }

    public string? BackBrakes { get; set; }

    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();
}
