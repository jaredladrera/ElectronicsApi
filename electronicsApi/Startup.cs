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

            // naka base dito na nagmula sa databasesettings.cs ang mga kukunin mong data mula sa appsetngs.json 
            // parang nagiging blueprint o prototype/clone sya na pag basasehan {get;set;}
            var dbSettings = new DatabaseSettings();
            dbSettings.ConnectionString = Configuration.GetSection("DatabaseSettings:ConnectionString").Value;
            dbSettings.DatabaseName = Configuration.GetSection("DatabaseSettings:DatabaseName").Value;

            services.Configure<DatabaseSettings>(options =>
            {
                options.ConnectionString = dbSettings.ConnectionString;
                options.DatabaseName = dbSettings.DatabaseName;
            });

            //Mongo Database
            // sa parteng to kinukuha ang connectioString at DatabaseName na naka declare sa appsettings.json
            // upang makuha din ang collection na laman ng database at maipasa sa service at commands function
            var client = new MongoClient(dbSettings.ConnectionString);
            var mongoDB = client.GetDatabase(dbSettings.DatabaseName);
            services.AddSingleton(mongoDB);

            // this is injecting your Model in the database Collections for base data type
            // sa parte naman to ay ang  pag papasa ng base datatype mula sa model papunta sa collections
            // lahat ng papasok na data mula mula sa user ay tatangapin lamang kung ito ay tutugma sa models na ginawa mo
            // at pag ka tapos saka lang ipapasok ng models ang data papunta sa collection o sa database
            var deviceDetailsCollection = mongoDB.GetCollection<DeviceDetails>("electronicsDetails");
            var deviceSpecsCollection = mongoDB.GetCollection<Specification>("specification");

            // mahala to para malaman ng service or commands ang mga gagamitin mong collection na ng galing sa database
            // sa part na to ang service at commands ay papasahan ng collection parameters ito yung nakikita mong parameters sa baba
            // at saparte ding ito ang parameter na pinasa hindi lang models ito ay collection mula sa mongoDB na nakabase ay ang ginawa mong collection sa Models
            services.AddSingleton<IDeviceDetailsService>(new DeviceDetailsService(deviceDetailsCollection, deviceSpecsCollection));
            services.AddSingleton<IDeviceDetailsCommands>(new DeviceDetailsCommands(deviceDetailsCollection));
            services.AddSingleton<ISpecsCommands>(new SpecsCommands(deviceDetailsCollection, deviceSpecsCollection));
            services.AddSingleton<ISpecificationService>(new SpecificationService(deviceSpecsCollection, deviceDetailsCollection));

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
