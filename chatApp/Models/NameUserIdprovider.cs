using chatApp.Controllers;
using DotNetOpenAuth.OpenId.Provider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Security.Principal;

namespace chatApp.Models
{
    public class NameUserIdprovider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
           var identity = connection.User.Identity as ClaimsIdentity;
            string SenderId = identity.FindFirst("UserID").Value;
            return SenderId;
            //return connection.User?.Identity?.Name;
            // return connection.User?.Identity?.Claims[2];
        }
    }
}
