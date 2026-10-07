using chatApp.Data;
using chatApp.Models;
using chatApp.Repository;
using DotNetOpenAuth.OpenId;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static chatApp.Models.MakingHttpRequest;

namespace chatApp
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


       

        public void ConfigureServices(IServiceCollection services)
        {

            
            /*  services.AddAuthentication(x =>
              {
                  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              })
              .AddJwtBearer(x =>
              {
                  x.RequireHttpsMetadata = false;
                  x.SaveToken = true;
                  x.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5D9BB31C-36B9-4E2E-9130-52529C0ACCFE")),
                      ValidateIssuer = false,
                      ValidateAudience = false
                  };
                  x.Events = new JwtBearerEvents
                  {
                      OnMessageReceived = context =>
                      {
                          var accessToken = context.Request.Query["access_token"];

                          // If the request is for our hub...
                          var path = context.HttpContext.Request.Path;
                          if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/notify")))
                          {
                              // Read the token out of the query string
                              context.Token = accessToken;
                          }
                          return Task.CompletedTask;
                      }
                  };
              });*/




            services
                      .AddAuthentication(options =>
                      {
                          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                          options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                      })
                      .AddJwtBearer(cfg =>
                      {
                          cfg.RequireHttpsMetadata = false;
                          cfg.SaveToken = true;
                          cfg.TokenValidationParameters = new TokenValidationParameters
                          {
                              ValidIssuer = "backend",
                              ValidAudience = "frontend",
                              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5D9BB31C-36B9-4E2E-9130-52529C0ACCFE")),
                              ClockSkew = TimeSpan.Zero // remove delay of token when expire
                          };
                          cfg.Events = new JwtBearerEvents
                          {   OnMessageReceived = context =>
                                {
                                    var accessToken = context.Request.Query["access_token"];

                                    // If the request is for our hub...
                                    var path = context.HttpContext.Request.Path;
                                    if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/notify")))
                                    {
                                        // Read the token out of the query string
                                        context.Token = accessToken;
                                    }
                                    return Task.CompletedTask;
                                },

                              OnAuthenticationFailed = context =>
                              {
                                  var te = context.Exception;
                                  return Task.CompletedTask;
                              }
                          };
                      });



            /*
                        services.AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                     .AddJwtBearer(options =>
                    {
                     options.SaveToken = true;
                      options.RequireHttpsMetadata = false;
                      options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = "frontend",
                        ValidIssuer = "backend",
                        ClockSkew = TimeSpan.Zero,// It forces tokens to expire exactly at token expiration time instead of 5 minutes later
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5D9BB31C-36B9-4E2E-9130-52529C0ACCFE"))
                    };
                }); 
            */



            /*   services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
           {

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = Configuration["jwt:Issuer"],
                   ValidAudience = Configuration["jwt:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
               };

               options.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
                   {
                       if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                       {
                           context.Response.Headers.Add("Token-Expired", "true");
                       }
                       return Task.CompletedTask;
                   }
               };


           });*/



            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });



            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // Add EF services to the services container.




           


            services.AddMvc();
            //services.AddEntityFrameworkSqlServer()
            //   .AddDbContext<chatAppContext>(options =>
            //      options.UseSqlServer(Configuration["Connection"]));

            services.AddSingleton<IUserIdProvider, NameUserIdprovider>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers();

            services.AddHttpClient<IserviceTTS, serviceTTS>(c =>
            {
                c.BaseAddress = new Uri("https://api.cle.org.pk/v1/synth");
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()

                    .SetIsOriginAllowed(_ => true)


                    );
            });
            
            services.AddRazorPages();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

         
            app.UseCors(x => x
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

            app.UseHttpsRedirection();
           
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();


            app.UseStaticFiles();
           // app.UseStaticFiles(new StaticFileOptions()
           // {
           //     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
           //     RequestPath = new PathString("/Resources")
           // });

            // app.UseSignalR(routes=>   
            // {
            //    routes.MapHub<NotifyHub>("/Notify");
            // });

            // app.MapHub<NotifyHub>("/Notify");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<NotifyHub>("/Notify");
                endpoints.MapRazorPages();
            });
        }
    }
}
