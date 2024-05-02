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
