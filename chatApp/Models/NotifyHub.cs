using DotNetOpenAuth.OpenId.Provider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace chatApp.Models
{
     [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   // [Authorize]
    public class NotifyHub  : Hub<InotifyHub>
    {



        /*public async Task BroadcastAsync(Messages message, string connectionId)
        {
            await Clients.User(connectionId).BroadcastChartData(message);
        }
        public string GetConnectionId() => Context.ConnectionId;
        */
        //this is used for frontend invoke use some fucntionality by invoking from frontend and taking function of backend...


        public void Send(string userId, Messages message)
        {
            Clients.User(userId).send(message);
        }
    }

    

    public interface InotifyHub
    {
        Task BroadcastChartData(Messages data);
        void send(Messages message);
    }

   
}
