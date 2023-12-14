using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    internal class Transmission
    {
        public string classification = "Трансмиссия:\n    ";
        public string Name { get; }
        public string Type {  get; }
        public int NumberOfGears { get; }
        public Transmission(string name, string type, int numberOfGears)
        {
            Name = name;
            Type = type;
            NumberOfGears = numberOfGears;
        }   
    }
}
