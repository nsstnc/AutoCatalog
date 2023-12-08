using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCatalog
{
    // класс описывающий подвеску и тормоза автомобиля
    internal class SuspensionAndBrakes
    {
        public string TypeOfFrontSuspension { get; }
        public string TypeOfBackSuspension { get; }
        public string FrontBrakes { get; }
        public string BackBrakes { get; }
        public SuspensionAndBrakes(string typeOfFrontSuspension, string typeOfBackSuspension,
            string frontBrakes, string backBrakes) 
        { 
            TypeOfFrontSuspension = typeOfFrontSuspension;
            TypeOfBackSuspension = typeOfBackSuspension;
            FrontBrakes = frontBrakes;
            BackBrakes = backBrakes;
        }
    }
}
