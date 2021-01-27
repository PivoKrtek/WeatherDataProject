using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDataLibrary
{
    public class OpenDoor
    {
        public DateTime DoorOpens { get; set; }
        public DateTime DoorCloses { get; set; }
        public TimeSpan OpenTime { get; set; }
        public double TempBeforeOpening { get; set; }
        public double TempAfterOpening { get; set; }
        public double TempBeforeClosing { get; set; }
        public double TempAfterClosing { get; set; }

    }
}
