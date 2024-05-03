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

        public void AddManufacturer(Manufacturer manufacturer)
        {
            manufacturer.Id = db.Manufacturers.Count();
            db.Manufacturers.Add(manufacturer);
            db.SaveChanges();
        }

        public void AddCar(Car car)
        {
            car.Id = db.Cars.Count();
            db.Cars.Add(car);
            db.SaveChanges();
        }
        public void AddTransmission(Transmission transmission)
        {
            transmission.Id = db.Transmissions.Count();
            db.Transmissions.Add(transmission);
            db.SaveChanges();
        }
        public void AddSuspensionAndBrakes(SuspensionAndBrake suspension)
        {
            suspension.Id = db.SuspensionAndBrakes.Count();
            db.SuspensionAndBrakes.Add(suspension);
            db.SaveChanges();
        }

        public void AddConfiguration(Configuration config)
        {
            config.Id = db.Configurations.Count();
            db.Configurations.Add(config);
            db.SaveChanges();
        }


        public void AddEngine(Engine engine)
        {
            engine.Id = db.Engines.Count();
            db.Engines.Add(engine);
            db.SaveChanges();
        }

        public void DeleteManifacturer(ManufacturerTemplate manufacturer)
        {

            db.Manufacturers.Where(p => p.Name == manufacturer.Name &&
                p.YearOfFoundation == manufacturer.YearOfFoundation &&
                p.Country == manufacturer.Country).ExecuteDelete();

            db.SaveChanges();
        }

        public void UpdateManufacturer(ManufacturerTemplate old, ManufacturerTemplate new_)
        {
            db.Manufacturers.Where(p => p.Name == old.Name &&
                p.YearOfFoundation == old.YearOfFoundation &&
                p.Country == old.Country)
                .ExecuteUpdate(s => 
                            s.SetProperty(u => u.Name, u => new_.Name)
                            .SetProperty(u => u.YearOfFoundation, u => new_.YearOfFoundation)
                            .SetProperty(u => u.Country, u => new_.Country));


            db.SaveChanges();
        }

        public void DeleteCar(CarTemplate car)
        {

            List<CarTemplate> cars = this.GetAllCars();

            // получаем автомобиль по id
            var item = db.Cars.FirstOrDefault(p => p.Id == car.Id);

            if (item != null) {

                var itemConfiguration = db.Configurations.FirstOrDefault(p => p.Id ==  item.ConfigurationId);
                if (itemConfiguration != null)
                {
                    // удаляем зависимые элементы конфигурации
                    db.Engines.Where(p => p.Id == itemConfiguration.EngineId).ExecuteDelete();
                    db.Transmissions.Where(p => p.Id == itemConfiguration.TransmissionId).ExecuteDelete();
                    db.SuspensionAndBrakes.Where(p => p.Id == itemConfiguration.SuspensionAndBrakesId).ExecuteDelete();
                    // удаляем конфигурацию
                    db.Configurations.Remove(itemConfiguration);
                }

                // удаляем автомобиль
                db.Cars.Remove(item);
            };

            
            db.SaveChanges();
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
                               Id = car.Id,
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
