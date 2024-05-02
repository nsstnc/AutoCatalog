using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    internal class DataService
    {
        private readonly CatalogAppContext db;

        public DataService(CatalogAppContext dbContext)
        {
            db = dbContext;
        }



        public List<ManufacturerTemplate> GetAllManufactures()
        {

            var manufactures = from manufacturer in db.Manufacturers
                                   select new ManufacturerTemplate
                                   {
                                       Name = manufacturer.Name,
                                       YearOfFoundation = manufacturer.YearOfFoundation,
                                       Country = manufacturer.Country,
                                   };

                
            
            return manufactures.ToList();
        }




        public List<CarTemplate> GetAllCars()
        {
            
                var cars = from car in db.Cars
                           join manufacturer in db.Manufacturers on car.ManufacturerId equals manufacturer.Id
                           join configuration in db.Configurations on car.ConfigurationId equals configuration.Id
                           join transmission in db.Transmissions on configuration.TransmissionId equals transmission.Id
                           join engine in db.Engines on configuration.EngineId equals engine.Id
                           join suspensionAndBrakes in db.SuspensionAndBrakes on configuration.SuspensionAndBrakesId equals suspensionAndBrakes.Id
                           select new CarTemplate
                           {
                               Name = car.Name,
                               Generation = car.Generation,
                               Manufacturer = manufacturer.Name,
                               YearOfFoundation = manufacturer.YearOfFoundation,
                               Country = manufacturer.Country,
                               Year = car.Year,
                               Configuration = configuration.Name,
                               TypeOfEngine = engine.TypeOfEngine,
                               CylinderArrangement = engine.CylinderArrangement,
                               Power = engine.Power,
                               Volume = engine.Volume,
                               MaxTorque = engine.MaxTorque,
                               NumberOfCylinders = engine.NumberOfCylinders,
                               TypeOfBoost = engine.TypeOfBoost,
                               FuelGrade = engine.FuelGrade,
                               EnginePowerSupplySystem = engine.EnginePowerSupplySystem,
                               TransmissionType = transmission.Type,
                               NumberOfGears = transmission.NumberOfGears,
                               TypeOfFrontSuspension = suspensionAndBrakes.TypeOfFrontSuspension,
                               TypeOfBackSuspension = suspensionAndBrakes.TypeOfBackSuspension,
                               FrontBrakes = suspensionAndBrakes.FrontBrakes,
                               BackBrakes = suspensionAndBrakes.BackBrakes,
                               TypeOfDrive = configuration.TypeOfDrive,
                               OverClocking = configuration.OverClocking,
                               Clearance = configuration.Clearance,
                               CurbWeight = configuration.CurbWeight,
                               FullWeight = configuration.FullWeight,
                               FuelTankVolume = configuration.FuelTankVolume,
                               NumberOfSeats = configuration.NumberOfSeats,
                               Body = car.Body,
                               Category = car.Category

                           };

                
            return cars.ToList();
        }
    }
}
