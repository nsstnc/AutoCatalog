using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    // класс описывающий комплектацию
    internal class Configuration
    {
        public string Name { get; }
        public Engine Engine { get; }
        public Transmission Transmission { get; }
        public SuspensionAndBrakes SuspensionAndBrakes { get; }
        public string TypeOfDrive { get; }
        public decimal? Overclocking { get; }
        public int? Clearance { get; }
        public int CurbWeight { get; }
        public int FullWeight { get; }
        public int FuelTankVolume {  get; }
        public int NumberOfSeats { get; }

        public Configuration(string name, Engine engine, Transmission transmission, SuspensionAndBrakes suspensionAndBrakes,
            string typeOfDrive, decimal? overClocking, int? clearance, int curbWeight, int fullWeight, int fuelTankVolume, int numberOfSeats)
        
        {
            Name = name;
            Engine = engine;
            Transmission = transmission;
            SuspensionAndBrakes = suspensionAndBrakes;
            TypeOfDrive = typeOfDrive;
            Overclocking = overClocking;
            Clearance = clearance;
            CurbWeight = curbWeight;
            FullWeight = fullWeight;
            FuelTankVolume = fuelTankVolume;
            NumberOfSeats = numberOfSeats;
        }  
    }
}
