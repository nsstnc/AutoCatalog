using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    abstract class Container<T>
    {
        public abstract List<T> items { get; set; }
        public abstract void AddItem(T item);
        public abstract void RemoveItem(int index);
        public abstract List<T> Get();
    }
    
}
