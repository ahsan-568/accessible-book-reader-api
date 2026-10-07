using System;
using chatApp.Data;
using chatApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(chatApp.Areas.Identity.IdentityHostingStartup))]
namespace chatApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {

        public void Configure(IWebHostBuilder builder)
        {

            builder.ConfigureServices((context, services) => {
                services.AddDbContext<chatAppContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("Connection")));

                services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<chatAppContext>();
            });
        }
    }

}