using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SignalRDemoApp.Hubs
{
    public class ReceiveDataHub : Hub
    {        
        public Task NotifyConnection()
        {
             return Clients.All.SendAsync("TestBrodcasting", $"Testing a Basic HUB at {DateTime.Now.ToLocalTime()}");
        }
    }
}
