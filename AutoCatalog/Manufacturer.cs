using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    // класс производителя - содержит информацию о производителе


    internal class Manufacturer
    {
        public string? Name { get; }
        public int? YearOfFoundation { get; }
        public string? Country { get; }


        public Manufacturer(string? name, int? year_of_foundation, string? country)
        {
            Name = name;
            YearOfFoundation = year_of_foundation;
            Country = country;
        }
    }
}
