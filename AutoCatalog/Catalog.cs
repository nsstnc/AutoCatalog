using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    // класс контейнер - каталог, содержащий в себе все автомобили
    internal class Catalog
    {   
        private List<Car> cars { get; }

        public Catalog() { cars = new List<Car>(); }

        // добавление автомобиля в каталог
        public bool AddCar(Car car)
        {
            bool contain = false;
            // для каждого элемента в списке получаем свойства и сравниваем с текущим автомобилем, если все свойства совпадают, то такой автомобиль уже есть
            foreach (Car item in cars)
            {
                contain = item.GetType().GetProperties().All(s => s.GetValue(item).ToString().ToLower() == s.GetValue(car).ToString().ToLower());
            }

            if (!contain) this.cars.Add(car);
            // возвращаем результат операции
            return !contain;
        }
        // удаление автомобиля из каталога
        public bool RemoveCar(Car car) 
        {
            bool contain = this.cars.Contains(car);
            if (contain) this.cars.Remove(car);
            return contain;
        }

        public List<Car> GetCars()
        {
            return this.cars;
        }
    }
}
