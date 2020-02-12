using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using ElectronicsAPI.Commands.Interface;
using MongoDB.Driver;
using ElectronicsAPI.Models;

namespace ElectronicsAPI.Commands.Interface
{
    [ExcludeFromCodeCoverage]
    public class SpecsCommands: ISpecsCommands
    {
        private readonly IMongoCollection<DeviceDetails> _device;
        private readonly IMongoCollection<Specification> _specification;

        public SpecsCommands(IMongoCollection<DeviceDetails> device, IMongoCollection<Specification> specification)
        {
            this._device = device;
            this._specification = specification;
        }

        virtual public int UpdateLatest(int id, Specification devlatest)
        {
            if (id < 1)
                throw new InvalidOperationException("No matching found");
            if (devlatest == null)
                throw new ArgumentNullException(nameof(devlatest), "Request can't be null");

            var deviceInfo = _specification.Find(d => d.deviceId == id).ToList();

            if (deviceInfo.Count() < 0)
                throw new InvalidOperationException("No matching record found");

            FilterDefinition<Specification> filter = null;
            UpdateDefinitionBuilder<Specification> update = null;
            UpdateDefinition<Specification> updateDef = null;
            UpdateResult result = null;
            var updateCount = 0;

            foreach (var deviceInfos in deviceInfo)
            {
                filter = Builders<Specification>.Filter.Eq(nameof(deviceInfos.deviceId), deviceInfos.deviceId);
                update = Builders<Specification>.Update;

                updateDef = update.Set(nameof(deviceInfos.ram), devlatest.ram)
                            .Set(nameof(deviceInfos.color), devlatest.color)
                            .Set(nameof(deviceInfos.hdd), devlatest.hdd)
                            .Set(nameof(deviceInfos.operatingSystem), devlatest.operatingSystem)
                            .Set(nameof(deviceInfos.processor), devlatest.processor);
                result = _specification.UpdateOne(filter, updateDef);

                if (result.IsAcknowledged)
                    updateCount++;
            }

            return updateCount;
        }
        // ito ang bagong command
        // dito tayo nag tapos update  function hindi pa ayos ang interface nito an then inject mo na sa controller
        // tangalin ang current command na nasa controller and ienject ang bago then test
        // sudan ang devicedetailscommand para hindi mag ka problema katulad ng nakaraan 500 internal server error


        public bool DeleteById(int deviceId)
        {
            if (deviceId < 1)
                return false;

            var specs = _specification.Find(m => m.deviceId == deviceId).ToList();

            if (specs.Count < 1)
                return false;

            var filter = Builders<Specification>.Filter.Eq("deviceId", deviceId);
            var result = _specification.DeleteOne(filter);

            if(!result.IsAcknowledged)
                return false;

            return true;
        }

        public bool Post(Specification specs)
        {
            if (specs.deviceId < 1)
                throw new InvalidOperationException("Device Id must be greaterthan 0");
            if (specs == null)
                throw new InvalidOperationException("the body can be null");

            var __specs = _specification.Find(m => m.deviceId == specs.deviceId).ToList();

            if (__specs.Count() > 0)
                throw new InvalidOperationException("Already exist");

            _specification.InsertOne(specs);

            return true;
        }

        public bool DeletingById(string id)
        {
            if (id == "")
                return false;

            var specs = _specification.Find(m => m.Id == id).ToList();

            if (specs.Count < 1)
                return false;

            var filter = Builders<Specification>.Filter.Eq("Id", id);
            var result = _specification.DeleteOne(filter);

            if (!result.IsAcknowledged)
                return false;

            return true;
        }

    }
}
