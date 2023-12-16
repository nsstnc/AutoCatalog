using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    abstract class Container
    {
        public abstract List<ContainerItem> items { get; set; }
        public abstract void AddItem();
        public abstract void RemoveItem();
        public abstract List<ContainerItem> Get();
    }

}
