using System;
using System.Collections.Generic;

namespace AutoCatalog;

public partial class Configuration
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public long? EngineId { get; set; }

    public long? TransmissionId { get; set; }

    public string? TypeOfDrive { get; set; }

    public decimal? OverClocking { get; set; }

    public long? Clearance { get; set; }

    public long? CurbWeight { get; set; }

    public long? FullWeight { get; set; }

    public long? FuelTankVolume { get; set; }

    public long? NumberOfSeats { get; set; }

    public long? SuspensionAndBrakesId { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual Engine? Engine { get; set; }

    public virtual SuspensionAndBrake? SuspensionAndBrakes { get; set; }

    public virtual Transmission? Transmission { get; set; }
}
