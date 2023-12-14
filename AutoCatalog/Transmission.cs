using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    internal class Transmission
    {
       
        public string Type {  get; }
        public int NumberOfGears { get; }
        public Transmission(string type, int numberOfGears,
            string classification = "Трансмиссия:\n   ")
        {
        
            Type = type;
            NumberOfGears = numberOfGears;
        }   
    }
}
