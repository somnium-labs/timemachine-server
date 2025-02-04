﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TimeMachineServer;

namespace TimemachineServer
{
    public class Startup
    {
        private readonly string policyName = "electronDev";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                builder =>
                {
                    builder
                    .WithOrigins("http://localhost:8080")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            UniverseManager.Instance.Initialize();
            AssetManager.Instance.AddAsset("JP225", "NIKKEI225");
            AssetManager.Instance.AddAsset("KOSPI", "KOSPI");

            // Task.Run(() =>
            // {
            //     var tempVolatilityUpdate = new TempVolatilityUpdate();
            //     tempVolatilityUpdate.Run();
            // });

            app.UseCors(policyName);
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
