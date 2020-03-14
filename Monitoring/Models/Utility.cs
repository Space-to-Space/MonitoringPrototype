using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitoring.Models
{
    public static class Utility
    {
        public enum Sensor
        {
            Undefined = -1,
            Volume = 0,
            Thermal = 1,
            Force = 2
        }
    }
}
