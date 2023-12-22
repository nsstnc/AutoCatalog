using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoCatalog
{
    // класс производителя - содержит информацию о производителе
    internal class Manufacturer
    {
        public string Name { get; }
        public int YearOfFoundation { get; }
        public string Country { get; }

        public Manufacturer(string name, int yearOfFoundation, string country)
        {
            Name = name;
            YearOfFoundation = yearOfFoundation;
            Country = country;
        }

   
    }


    // класс-контейнер для производителей
    internal class Manufactures : Container<Manufacturer>
    {
        // конструктор класса
        public Manufactures() { items = new List<Manufacturer>(); }

        public override List<Manufacturer> items { get; set; }
        // добавление элемента в контейнер
        public override void AddItem(Manufacturer item)
        {
            bool contain = false;
            // для каждого элемента в списке получаем свойства и сравниваем с текущим элементов, если все свойства совпадают, то такой объект уже есть
            foreach (Manufacturer included_item in items)
            {
                contain = item.GetType().GetProperties().All(s => s.GetValue(item).ToString().ToLower() == s.GetValue(included_item).ToString().ToLower());
            }

            if (!contain) this.items.Add(item);

        }
        // удаление элемента из контейнера по индексу
        public override void RemoveItem(int index) { items.RemoveAt(index); }
        // замена элемента в контейнере
        public override void ChangeItem(int index, Manufacturer item) { items[index] = item; }
        public override List<Manufacturer> Get() { return items; }

    }
}
