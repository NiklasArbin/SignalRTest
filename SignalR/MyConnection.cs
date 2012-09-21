using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SignalR;

namespace SignalRTest.SignalR
{
    public class MyConnection : PersistentConnection
    {
        protected override Task OnReceivedAsync(IRequest request, string connectionId, string data)
        {
            // Broadcast data to all clients
            string serverData = String.Format("Handled by server: {0}", data);
            return Connection.Broadcast(serverData);
        }
        protected override Task OnConnectedAsync(IRequest request, string connectionId)
        {
            Groups.Send("allClients", String.Format("New connection from client: {0}", connectionId));
            Groups.Add(connectionId, "allClients");
            return Connection.Send(connectionId, String.Format("You are now connected with Id: {0}", connectionId));
        }
        protected override Task OnDisconnectAsync(string connectionId)
        {
            Groups.Remove(connectionId, "allClients");
            return Connection.Broadcast(String.Format("Connection {0} has been disconnected", connectionId));
        }
    }
}