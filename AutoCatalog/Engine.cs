using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    // класс описывающий двигатель
    internal class Engine
    {
        public string Name { get; }
        public string TypeOfEngine { get; }
        public string CylinderArrangement { get; }
        public int Power { get; }
        public int Volume { get; }
        public int MaxTorque { get; }
        public int NumberOfCylinders { get; }
        public string TypeOfBoost { get; }
        public string FuelGrade { get; }
        public string EnginePowerSupplySystem { get; }

        public Engine(string name, string typeOfEngine, string cylinderArrangement, int power, int volume, int maxTorque, int numberOfCylinders,
            string typeOfBoost, string fuelGrade, string enginePowerSupplySystem)
        {
            Name = name;
            TypeOfEngine = typeOfEngine;
            CylinderArrangement = cylinderArrangement;
            Power = power;
            Volume = volume;
            MaxTorque = maxTorque;
            NumberOfCylinders = numberOfCylinders;
            TypeOfBoost = typeOfBoost;
            FuelGrade = fuelGrade;
            EnginePowerSupplySystem = enginePowerSupplySystem;
        }
    }
}
