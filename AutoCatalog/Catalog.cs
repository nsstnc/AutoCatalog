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
        public List<Car> cars { get; set; }

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
        public void RemoveCar(int index) 
        {
            this.cars.RemoveAt(index);
        }

        public List<Car> GetCars()
        {
            return this.cars;
        }
    }
}
