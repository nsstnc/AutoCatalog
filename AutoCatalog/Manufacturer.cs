using System;
using System.Collections.Generic;

namespace AutoCatalog;

public partial class Manufacturer
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public long? YearOfFoundation { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}

public class ManufacturerTemplate 
{
    public string? Name { get; set; }

    public long? YearOfFoundation { get; set; }

    public string? Country { get; set; }
}
