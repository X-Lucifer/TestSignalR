using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestSignalrServer.Models;

namespace TestSignalrServer
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(x =>
            {
                x.AddPolicy("signalr", v =>
                {
                    v.SetIsOriginAllowed(i => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });
            var redis = _config["RedisConfig"];
            services.AddSignalR().AddStackExchangeRedis(redis);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("signalr");
            app.UseRouting();
            app.UseEndpoints(x =>
            {
                x.MapHub<ChatHub>("/realtime/chat");
                x.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"signalr is running...");
                });
            });
        }
    }
}
