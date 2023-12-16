using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    // класс контейнер - каталог, содержащий в себе все автомобили
    internal class Catalog : Container<Car>
    {
        // конструктор класса
        public Catalog() { items = new List<Car>(); }

        public override List<Car> items { get; set; }
        // добавление элемента в контейнер
        public override void AddItem(Car item)
        {
            bool contain = false;
            // для каждого элемента в списке получаем свойства и сравниваем с текущим элементов, если все свойства совпадают, то такой объект уже есть
            foreach (Car included_item in items)
            {
                contain = item.GetType().GetProperties().All(s => s.GetValue(item).ToString().ToLower() == s.GetValue(included_item).ToString().ToLower());
            }

            if (!contain) this.items.Add(item);

        }
        // удаление элемента из контейнера по индексу
        public override void RemoveItem(int index) { items.RemoveAt(index); }
        public override List<Car> Get(){ return items; }

    }
}
