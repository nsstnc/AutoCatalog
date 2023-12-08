using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    // класс описывающий поколение
    internal class Generation
    {
        public string Name { get; }
        public int YearOfProductionStart { get; }
        public int? YearOfProductionEnd { get; }

        public Generation(string name, int year_of_production_start, int? year_of_production_end) 
        {
            Name = name;
            YearOfProductionEnd = year_of_production_end;
            YearOfProductionStart = year_of_production_start;
        }
    }
}
