using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace MyTurn.Web.Hubs
{
    public class MyTurnHub : Hub
    {
        private const string GroupName = "Sample";
        public void Subscribe()
        {            
            Groups.Add(Context.ConnectionId, GroupName);
            Clients.All.received(new DateTime());
        }

        public void Unsubscribe()
        {
            Groups.Remove(Context.ConnectionId, GroupName);
        }
    }
}