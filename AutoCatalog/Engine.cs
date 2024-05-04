using System;
using System.Collections.Generic;

namespace AutoCatalog;

public partial class Engine
{
    public long? Id { get; set; }

    public string? CylinderArrangement { get; set; }

    public string? TypeOfEngine { get; set; }

    public long? Power { get; set; }

    public long? Volume { get; set; }

    public long? MaxTorque { get; set; }

    public long? NumberOfCylinders { get; set; }

    public string? TypeOfBoost { get; set; }

    public string? FuelGrade { get; set; }

    public string? EnginePowerSupplySystem { get; set; }

    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();
}
