using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitoring.Models
{
    public class Device
    {
        public string SerialPort;
        public Utility.Sensor SensorType;
        public bool Started;
    }
}
