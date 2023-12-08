using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    // класс описываюший кузов
    internal class Body
    {
        public string Name { get; }
        public int CountOfDoors { get; }


        public Body(string name, int count_of_doors)
        {
            Name = name;
            CountOfDoors = count_of_doors;
        }
    }
}
