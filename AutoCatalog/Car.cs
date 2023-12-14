using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    // класс автомобиля - содержит информацию об автомобиле
    internal class Car
    {
        public string Name { get; }
        public string Generation { get; }
        public Manufacturer Manufacturer { get;}
        public int Year { get; }
        public Configuration Configuration { get; }
        public string Body { get; }
        public string Category { get; }


        public Car(string name, string generation, Manufacturer manufacturer, int year, Configuration configuration, string body, string category) 
        {
            Name = name;
            Generation = generation;
            Manufacturer = manufacturer;
            Year = year;
            Configuration = configuration;
            Body = body;
            Category = category;
        }
    }
}
