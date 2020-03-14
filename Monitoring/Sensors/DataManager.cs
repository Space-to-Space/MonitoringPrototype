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
    public static class DataManager
    {
        public static Task SendData(IHubContext<DataHub> dataHub, List<object> data)
        {
            return Task.Run(() => {dataHub.Clients.All.SendAsync("receive", data); });
        }
    }
}
