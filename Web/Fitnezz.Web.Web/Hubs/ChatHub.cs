using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Fitnezz.Web.Web.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId() => this.Context.ConnectionId;
    }
}
