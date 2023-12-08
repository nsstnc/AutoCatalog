using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{   
    // класс-контейнер для производителей
    internal class Manufactures
    {
        private List<Manufacturer> manufacturers { get; }

        // добавление производителя в список производителей
        public bool AddManufacture(Manufacturer manufacturer)
        {
            bool contain = this.manufacturers.Contains(manufacturer);
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
