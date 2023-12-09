using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Automation;

namespace AutoCatalog
{   
    // класс-контейнер для производителей
    internal class Manufactures
    {
        private List<Manufacturer> manufacturers { get; }

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
        public bool RemoveManufacture(Manufacturer manufacturer)
        {
            bool contain = this.manufacturers.Contains(manufacturer);
            if (contain) this.manufacturers.Remove(manufacturer);
            return contain;
        }

        public List<Manufacturer> GetManufacturers()
        {
            return this.manufacturers;
        }
    }
}
