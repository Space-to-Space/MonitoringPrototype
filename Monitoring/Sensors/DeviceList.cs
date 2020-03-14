using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Monitoring.Models;
using Monitoring.SignalR;

namespace Monitoring.Sensors
{
    public static class DeviceList
    {
        public static List<Device> Devices = new List<Device>();

        public static bool IsDeviceKnown(string port)
        {
            return Devices.Exists(e => e.SerialPort == port);
        }

        public static Device Add(string port, Utility.Sensor sensorType)
        {
            var device = new Device
            {
                SensorType = sensorType,
                SerialPort = port
            };

            Devices.Add(device);

            return device;
        }

        public static bool AreAllPortsStarted()
        {
            return !Devices.Exists(e => !e.Started);
        }
    }
}
