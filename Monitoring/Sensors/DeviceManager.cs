using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Monitoring.Models;
using Monitoring.SignalR;

namespace Monitoring.Sensors
{
    public class DeviceManager
    {
        private readonly IHubContext<DataHub> _hubContext;

        private const string _unwantedPort = "/dev/ttyAMA0";

        public bool IsRunning { get; set; }

        public DeviceManager(IHubContext<DataHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async void LookForNewDevicesAsync()
        {
            IsRunning = true;

            await Task.Run(() =>
            {
                while (true)
                {
                    var foundDevices = SerialPort.GetPortNames()
                        .Where(port => port != _unwantedPort && !DeviceList.IsDeviceKnown(port)).ToList();

                    if (!foundDevices.Any())
                        continue;

                    foundDevices.ForEach(Console.WriteLine);

                    AddNewDevices(foundDevices);

                    Thread.Sleep(5000);
                }
            });
        }

        /// <summary>
        /// Adds found devices
        /// </summary>
        public void AddNewDevices(List<string> ports)
        {
            ports.ForEach(AddNewDevice);
        }

        /// <summary>
        /// Adds new device
        /// </summary>
        public void AddNewDevice(string port)
        {
            //var serialPort = new SerialPort(port, Keys.BaudRate, Parity.Mark);
            //try
            //{
            //    serialPort.Open();
            //}
            //catch (Exception)
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.Error.WriteLine("Failed to bind new device");
            //    Console.ForegroundColor = ConsoleColor.White;
            //    return;
            //}
            var sensorType = Utility.Sensor.Undefined;

            //while (sensorType == Utility.Sensor.Undefined)
            //{
            //    try
            //    {
            //        var result = serialPort.ReadLine();

            //        if (!int.TryParse(result, out var sensorResult))
            //            continue;

            //        if (Enum.IsDefined(typeof(Utility.Sensor), sensorResult))
            //            sensorType = (Utility.Sensor)sensorResult;
            //    }
            //    catch (Exception)
            //    {
            //        Console.ForegroundColor = ConsoleColor.Red;
            //        Console.Error.WriteLine("Can't receive. Error occured while reading.");
            //        Console.ForegroundColor = ConsoleColor.White;
            //    }
            //}

            //Console.WriteLine("Sensor type " + sensorType);

            //for (int i = 0; i < 10; i++)
            //{
            //    serialPort.WriteLine("ASDASDSADASDASDADSDAS");
            //}
            
            //serialPort.Close();


            var dev = DeviceList.Add(port, sensorType);
            

            StartListening(dev);
        }

        /// <summary>
        /// Starts listening to the port
        /// </summary>
        public async void StartListening(Device device)
        {
            await Task.Run(async () =>
            {
                Console.WriteLine("INFO > Started listening");

                var dataList = new List<object>();

                var port = new SerialPort(device.SerialPort, Keys.BaudRate);

                device.Started = true;

                while (!DeviceList.AreAllPortsStarted())
                {
                    Thread.Sleep(500);
                }

                try
                {
                    port.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                for (var i = 0; i < 50000; i++)
                {
                    try
                    {
                        dataList.Add(port.ReadLine());

                        if (dataList.Count != 20) 
                            continue;

                        await DataManager.SendData(_hubContext, dataList);
                        dataList = new List<object>();
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Port stopped receiving. Error occured.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                }

                port.Close();

                device.Started = false;
            });
        }
    }
}
