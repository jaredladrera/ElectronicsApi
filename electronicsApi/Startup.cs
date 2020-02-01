using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ElectronicsAPI.Models;
using ElectronicsAPI.Services;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using ElectronicsAPI.Controllers;
using ElectronicsAPI.Services.Interface;
using ElectronicsAPI.Commands.Interface;
using ElectronicsAPI.Commands;

namespace ElectronicsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var dbSettings = new DatabaseSettings();
            dbSettings.ConnectionString = Configuration.GetSection("DatabaseSettings:ConnectionString").Value;
            dbSettings.DatabaseName = Configuration.GetSection("DatabaseSettings:DatabaseName").Value;

            services.Configure<DatabaseSettings>(options =>
            {
                options.ConnectionString = dbSettings.ConnectionString;
                options.DatabaseName = dbSettings.DatabaseName;
            });

            //Mongo Database
            var client = new MongoClient(dbSettings.ConnectionString);
            var mongoDB = client.GetDatabase(dbSettings.DatabaseName);
            services.AddSingleton(mongoDB);
            var deviceDetailsCollection = mongoDB.GetCollection<DeviceDetails>("electronicsDetails");
            var deviceSpecsCollection = mongoDB.GetCollection<Specification>("specification");

            services.AddSingleton<IDeviceDetailsService>(new DeviceDetailsService(deviceDetailsCollection));
            services.AddSingleton<IDeviceDetailsCommands>(new DeviceDetailsCommands(deviceDetailsCollection));


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
