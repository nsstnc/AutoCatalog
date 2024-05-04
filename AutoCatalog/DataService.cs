using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

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
            if (transmission.Id != null) 
            {
                // обновляем трансмиссию
                db.Transmissions.Where(p => p.Id == transmission.Id)
                    .ExecuteUpdate(s =>
                            s.SetProperty(u => u.Type, u => transmission.Type)
                            .SetProperty(u => u.NumberOfGears, u => transmission.NumberOfGears)
                            );
            }
            else
            {
                transmission.Id = db.Transmissions.Count();
                db.Transmissions.Add(transmission);
            }
            
            db.SaveChanges();
        }
        public void AddSuspensionAndBrakes(SuspensionAndBrake suspension)
        {
            if (suspension.Id != null)
            {
                // обновляем подвеску
                db.SuspensionAndBrakes.Where(p => p.Id == suspension.Id)
                    .ExecuteUpdate(s =>
                            s.SetProperty(u => u.TypeOfFrontSuspension, u => suspension.TypeOfFrontSuspension)
                            .SetProperty(u => u.TypeOfBackSuspension, u => suspension.TypeOfBackSuspension)
                            .SetProperty(u => u.FrontBrakes, u => suspension.FrontBrakes)
                            .SetProperty(u => u.BackBrakes, u => suspension.BackBrakes)
                            );
            }
            else
            {
                suspension.Id = db.SuspensionAndBrakes.Count();
                db.SuspensionAndBrakes.Add(suspension);
            }
            
            db.SaveChanges();
        }

        public void AddConfiguration(Configuration config)
        {
            if (config.Id != null) 
            {
                db.Configurations.Where(p => p.Id == config.Id)
                    .ExecuteUpdate(s =>
                            s.SetProperty(u => u.Name, u => config.Name)
                            .SetProperty(u => u.TypeOfDrive, u => config.TypeOfDrive)
                            .SetProperty(u => u.OverClocking, u => config.OverClocking)
                            .SetProperty(u => u.Clearance, u => config.Clearance)
                            .SetProperty(u => u.CurbWeight, u => config.CurbWeight)
                            .SetProperty(u => u.FullWeight, u => config.FullWeight)
                            .SetProperty(u => u.FuelTankVolume, u => config.FuelTankVolume)
                            .SetProperty(u => u.NumberOfSeats, u => config.NumberOfSeats)
                            );
            }
            else 
            {
                config.Id = db.Configurations.Count();
                db.Configurations.Add(config);
            }
            
            db.SaveChanges();
        }


        public void AddEngine(Engine engine)
        {
            if (engine.Id != null)
            {
                // обновляем двигатель
                db.Engines.Where(p => p.Id == engine.Id)
                    .ExecuteUpdate(s =>
                            s.SetProperty(u => u.CylinderArrangement, u => engine.CylinderArrangement)
                            .SetProperty(u => u.TypeOfEngine, u => engine.TypeOfEngine)
                            .SetProperty(u => u.Power, u => engine.Power)
                            .SetProperty(u => u.Volume, u => engine.Volume)
                            .SetProperty(u => u.MaxTorque, u => engine.MaxTorque)
                            .SetProperty(u => u.NumberOfCylinders, u => engine.NumberOfCylinders)
                            .SetProperty(u => u.TypeOfBoost, u => engine.TypeOfBoost)
                            .SetProperty(u => u.FuelGrade, u => engine.FuelGrade)
                            .SetProperty(u => u.EnginePowerSupplySystem, u => engine.EnginePowerSupplySystem)
                            );
            }
            else
            {
                engine.Id = db.Engines.Count();
                db.Engines.Add(engine);
            }
            
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


        private long? GetCarId(Car car)
        {
            // получаем автомобиль по полям из списка
            long? id = db.Cars.FirstOrDefault(p =>
                    p.Name == car.Name &&
                    p.Generation == car.Generation &&
                    p.Year == car.Year &&
                    p.ConfigurationId == car.ConfigurationId &&
                    p.Body == car.Body &&
                    p.Category == car.Category).Id;

            return id;
        }



        public void DeleteCar(CarTemplate car)
        {
            // список всех автомобилей
            List<CarTemplate> cars = this.GetAllCars();

            // получаем автомобиль по полям из списка
            long? id = cars.FirstOrDefault(p =>
                    p.Id == car.Id &&
                    p.Name == car.Name &&
                    p.Generation == car.Generation &&
                    p.Manufacturer == car.Manufacturer &&
                    p.YearOfFoundation == car.YearOfFoundation &&
                    p.Country == car.Country &&
                    p.Year == car.Year &&
                    p.Configuration == car.Configuration &&
                    p.TypeOfEngine == car.TypeOfEngine &&
                    p.CylinderArrangement == car.CylinderArrangement &&
                    p.Power == car.Power &&
                    p.Volume == car.Volume &&
                    p.MaxTorque == car.MaxTorque &&
                    p.NumberOfCylinders == car.NumberOfCylinders &&
                    p.TypeOfBoost == car.TypeOfBoost &&
                    p.FuelGrade == car.FuelGrade &&
                    p.EnginePowerSupplySystem == car.EnginePowerSupplySystem &&
                    p.TransmissionType == car.TransmissionType &&
                    p.NumberOfGears == car.NumberOfGears &&
                    p.TypeOfFrontSuspension == car.TypeOfFrontSuspension &&
                    p.TypeOfBackSuspension == car.TypeOfBackSuspension &&
                    p.FrontBrakes == car.FrontBrakes &&
                    p.BackBrakes == car.BackBrakes &&
                    p.Body == car.Body &&
                    p.Category == car.Category &&
                    p.TypeOfDrive == car.TypeOfDrive &&
                    p.OverClocking == car.OverClocking &&
                    p.Clearance == car.Clearance &&
                    p.CurbWeight == car.CurbWeight &&
                    p.FullWeight == car.FullWeight &&
                    p.FuelTankVolume == car.FuelTankVolume &&
                    p.NumberOfSeats == car.NumberOfSeats).Id;


            var item = db.Cars.FirstOrDefault(p => p.Id == id);

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

        public void UpdateCar(Car new_, Engine new_engine, Transmission new_transmission, SuspensionAndBrake new_suspension, Configuration new_configuration)
        {
            /*
            Configuration confItem = new_configuration;

            if (confItem != null)
            {
                // обновляем двигатель
                db.Engines.Where(p => p.Id == confItem.EngineId)
                    .ExecuteUpdate(s =>
                            s.SetProperty(u => u.CylinderArrangement, u => new_engine.CylinderArrangement)
                            .SetProperty(u => u.TypeOfEngine, u => new_engine.TypeOfEngine)
                            .SetProperty(u => u.Power, u => new_engine.Power)
                            .SetProperty(u => u.Volume, u => new_engine.Volume)
                            .SetProperty(u => u.MaxTorque, u => new_engine.MaxTorque)
                            .SetProperty(u => u.NumberOfCylinders, u => new_engine.NumberOfCylinders)
                            .SetProperty(u => u.TypeOfBoost, u => new_engine.TypeOfBoost)
                            .SetProperty(u => u.FuelGrade, u => new_engine.FuelGrade)
                            .SetProperty(u => u.EnginePowerSupplySystem, u => new_engine.EnginePowerSupplySystem)
                            );
                // обновляем трансмиссию
                db.Transmissions.Where(p => p.Id == confItem.TransmissionId)
                    .ExecuteUpdate(s =>
                            s.SetProperty(u => u.Type, u => new_transmission.Type)
                            .SetProperty(u => u.NumberOfGears, u => new_transmission.NumberOfGears)
                            );

                // обновляем подвеску
                db.SuspensionAndBrakes.Where(p => p.Id == confItem.SuspensionAndBrakesId)
                    .ExecuteUpdate(s =>
                            s.SetProperty(u => u.TypeOfFrontSuspension, u => new_suspension.TypeOfFrontSuspension)
                            .SetProperty(u => u.TypeOfBackSuspension, u => new_suspension.TypeOfBackSuspension)
                            .SetProperty(u => u.FrontBrakes, u => new_suspension.FrontBrakes)
                            .SetProperty(u => u.BackBrakes, u => new_suspension.BackBrakes)
                            );

                // обновляем конфигурацию
                db.Configurations.Where(p => p.Id == confItem.Id)
                    .ExecuteUpdate(s =>
                            s.SetProperty(u => u.Name, u => new_configuration.Name)
                            .SetProperty(u => u.TypeOfDrive, u => new_configuration.TypeOfDrive)
                            .SetProperty(u => u.OverClocking, u => new_configuration.OverClocking)
                            .SetProperty(u => u.Clearance, u => new_configuration.Clearance)
                            .SetProperty(u => u.CurbWeight, u => new_configuration.CurbWeight)
                            .SetProperty(u => u.FullWeight, u => new_configuration.FullWeight)
                            .SetProperty(u => u.FuelTankVolume, u => new_configuration.FuelTankVolume)
                            .SetProperty(u => u.NumberOfSeats, u => new_configuration.NumberOfSeats)
                            );


            }
            */

            // обновляем автомобиль

            db.Cars.Where(p => p.Id == this.GetCarId(new_))
                .ExecuteUpdate(s =>
                            s.SetProperty(u => u.Name, u => new_.Name)
                            .SetProperty(u => u.ManufacturerId, u => new_.ManufacturerId)
                            .SetProperty(u => u.Generation, u => new_.Generation)
                            .SetProperty(u => u.Year, u => new_.Year)
                            .SetProperty(u => u.Body, u => new_.Body)
                            .SetProperty(u => u.Category, u => new_.Category)
                            );
            
            


            db.SaveChanges();
        }

        public long GetManufacturerById(ManufacturerTemplate manufacturer)
        {
            return db.Manufacturers.FirstOrDefault(p => p.Name == manufacturer.Name &&
                    p.YearOfFoundation == manufacturer.YearOfFoundation &&
                    p.Country == manufacturer.Country).Id;
        }


        public Configuration GetConfigByCar(CarTemplate car)
        {
            // получаем автомобиль по id
            var carItem = db.Cars.FirstOrDefault(p => p.Id == car.Id);
            if (carItem != null)
            {
                return db.Configurations.FirstOrDefault(p => p.Id == carItem.ConfigurationId);
            }
            else return null;

        }

        public Manufacturer GetManufacturerByCar(CarTemplate car)
        {
            // получаем автомобиль по id
            var carItem = db.Cars.FirstOrDefault(p => p.Id == car.Id);
            if (carItem != null)
            {
                return db.Manufacturers.FirstOrDefault(p => p.Id == carItem.ManufacturerId);
            }
            else return null;

        }


        public Engine GetEngineByConfig(Configuration config)
        {
            // получаем конфиг по id
            var confItem = db.Configurations.FirstOrDefault(p => p.Id == config.Id);
            if (confItem != null)
            {
                return db.Engines.FirstOrDefault(p => p.Id == config.EngineId);
            }
            else return null;
        }
        public Transmission GetTransmissionByConfig(Configuration config)
        {
            // получаем конфиг по id
            var confItem = db.Configurations.FirstOrDefault(p => p.Id == config.Id);
            if (confItem != null)
            {
                return db.Transmissions.FirstOrDefault(p => p.Id == config.TransmissionId);
            }
            else return null;
        }

        public SuspensionAndBrake GetSuspensionByConfig(Configuration config)
        {
            // получаем конфиг по id
            var confItem = db.Configurations.FirstOrDefault(p => p.Id == config.Id);
            if (confItem != null)
            {
                return db.SuspensionAndBrakes.FirstOrDefault(p => p.Id == config.SuspensionAndBrakesId);
            }
            else return null;
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
