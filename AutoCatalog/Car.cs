using System;
using System.Collections.Generic;

namespace AutoCatalog;

public partial class Car
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Generation { get; set; }

    public long? ManufacturerId { get; set; }

    public long? Year { get; set; }

    public long? ConfigurationId { get; set; }

    public string? Body { get; set; }

    public string? Category { get; set; }

    public virtual Configuration? Configuration { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }
}

public class CarTemplate
{
    public string? Name { get; set; }
    public string? Generation { get; set; }
    public string? Manufacturer { get; set; }
    public long? YearOfFoundation { get; set; }

    public string? Country { get; set; }
    public long? Year { get; set; }
    public string? Configuration { get; set; }
    public string? TypeOfEngine { get; set; }
    public string? CylinderArrangement { get; set; }
    public long? Power { get; set; }

    public long? Volume { get; set; }

    public long? MaxTorque { get; set; }

    public long? NumberOfCylinders { get; set; }

    public string? TypeOfBoost { get; set; }

    public string? FuelGrade { get; set; }

    public string? EnginePowerSupplySystem { get; set; }

    public string? TransmissionType { get; set; }

    public long? NumberOfGears { get; set; }

    public string? TypeOfFrontSuspension { get; set; }

    public string? TypeOfBackSuspension { get; set; }

    public string? FrontBrakes { get; set; }

    public string? BackBrakes { get; set; }






    public string? Body { get; set; }

    public string? Category { get; set; }

    public string? TypeOfDrive { get; set; }

    public decimal? OverClocking { get; set; }

    public long? Clearance { get; set; }

    public long? CurbWeight { get; set; }

    public long? FullWeight { get; set; }

    public long? FuelTankVolume { get; set; }

    public long? NumberOfSeats { get; set; }

                               
}