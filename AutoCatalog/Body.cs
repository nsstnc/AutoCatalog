using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    // класс описываюший кузов
    internal class Body
    {
        public string classification = "Кузов:\n    ";
        public string Name { get; }
        public int CountOfDoors { get; }


        public Body(string name, int countOfDoors)
        {
            Name = name;
            CountOfDoors = countOfDoors;
        }
    }


    // класс-контейнер для кузовов
    internal class Bodies 
    {
        public List<Body> bodies { get; set; }

        public Bodies() { bodies = new List<Body>(); }
        // добавление производителя в список производителей
        public bool AddBody(Body body)
        {
            bool contain = false;
            // для каждого элемента в списке получаем свойства и сравниваем с текущим производителем, если все свойства совпадают, то такой производитель уже есть
            foreach (Body item in bodies)
            {
                contain = item.GetType().GetProperties().All(s => s.GetValue(item).ToString().ToLower() == s.GetValue(body).ToString().ToLower());
            }

            if (!contain) this.bodies.Add(body);
            // возвращаем результат операции
            return !contain;
        }
        // удаление производителя из списка производителей
        public void RemoveBody(int index)
        {
            this.bodies.RemoveAt(index);
        }

        public List<Body> GetBodies()
        {
            return this.bodies;
        }
    }



}
