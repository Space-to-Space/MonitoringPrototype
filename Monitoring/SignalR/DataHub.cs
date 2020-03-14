using Microsoft.AspNetCore.SignalR;

namespace Monitoring.SignalR
{
    public class DataHub : Hub
    {
        /// <summary>
        /// Sends new data to every client
        /// </summary>
        public async void Send()
        {
            // Call the addNewMessageToPage method to update clients.
            await Clients.All.SendCoreAsync("receive", new object[] { "löppt" });
        }
    }
}
