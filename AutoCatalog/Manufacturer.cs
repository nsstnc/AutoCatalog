using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    // класс производителя - содержит информацию о производителе


    internal class Manufacturer
    {
        public string classification = "Производитель:\n    ";
        public string? Name { get; }
        public int? YearOfFoundation { get; }
        public string? Country { get; }


        public Manufacturer(string? name, int? yearOfFoundation, string? country)
        {
            Name = name;
            YearOfFoundation = yearOfFoundation;
            Country = country;
        }
    }


    // класс-контейнер для производителей
    internal class Manufactures
    {
        public List<Manufacturer> manufacturers { get; set; }

        public Manufactures() { manufacturers = new List<Manufacturer>(); }
        // добавление производителя в список производителей
        public bool AddManufacture(Manufacturer manufacturer)
        {
            bool contain = false;
            // для каждого элемента в списке получаем свойства и сравниваем с текущим производителем, если все свойства совпадают, то такой производитель уже есть
            foreach (Manufacturer item in manufacturers)
            {
                contain = item.GetType().GetProperties().All(s => s.GetValue(item).ToString().ToLower() == s.GetValue(manufacturer).ToString().ToLower());
            }

            if (!contain) this.manufacturers.Add(manufacturer);
            // возвращаем результат операции
            return !contain;
        }
        // удаление производителя из списка производителей
        public void RemoveManufacture(int index)
        {
            manufacturers.RemoveAt(index);
        }

        public List<Manufacturer> GetManufacturers()
        {
            return this.manufacturers;
        }
    }


}
