using ElectronicsAPI.Models;
using MongoDB.Driver;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ElectronicsAPI.Commands.Interface;


namespace ElectronicsAPI.Commands
{
    [ExcludeFromCodeCoverage]
    public class DeviceDetailsCommands : IDeviceDetailsCommands
    {
        private readonly IMongoCollection<DeviceDetails> _device;

        public DeviceDetailsCommands(IMongoCollection<DeviceDetails> device)
        {
            this._device = device;
        }

        public int PutLatestVersionAll(int id, DeviceDetails latest)
        {
            if (id == null)
                throw new InvalidOperationException("No matching record found");
            if (latest == null)
                throw new ArgumentNullException(nameof(latest), "Request can not be null");

            var deviceInfos = _device.Find(d => d.controlNumber == id).ToList();

            if (deviceInfos.Count < 1)
                throw new InvalidOperationException("No matching record found");

            //research about this
            FilterDefinition<DeviceDetails> filter = null;
            UpdateDefinitionBuilder<DeviceDetails> update = null;
            UpdateDefinition<DeviceDetails> updateDef = null;
            UpdateResult result = null;
            var updateCount = 0;

            foreach (var deviceInfo in deviceInfos)
            {
                //this is for updating the data from the database
                filter = Builders<DeviceDetails>.Filter.Eq(nameof(deviceInfo.controlNumber), deviceInfo.controlNumber);
                update = Builders<DeviceDetails>.Update;
                updateDef = update.Set(nameof(deviceInfo.brand), deviceInfo.brand)
                    .Set(nameof(deviceInfo.controlNumber), deviceInfo.controlNumber)
                    .Set(nameof(deviceInfo.model), deviceInfo.model)
                    .Set(nameof(deviceInfo.isHighEnd), deviceInfo.isHighEnd)
                    .Set(nameof(deviceInfo.releaseDate), deviceInfo.releaseDate);

                result = _device.UpdateOne(filter, updateDef);

                if (result.IsAcknowledged)
                    updateCount++;

            }

            return updateCount;

        } // end of function

        public bool DeleteById(int id)
        {
            if (id < 1)
                return false;

            var dev = _device.Find(m => m.Id == id).ToList();

            if (dev.Count < 1)
                return false;

            var filter = Builders<DeviceDetails>.Filter.Eq("Id", id);
            var result = _device.DeleteOne(filter);

            if (!result.IsAcknowledged)
                return false;

            return true;
        }

        public bool Post(DeviceDetails deviceDetails)
        {
            if(deviceDetails.Id < 1)
                throw new InvalidOperationException("ID must be greater than 0");
            if (deviceDetails == null)
                throw new InvalidOperationException("Body can not be null");

            var __device = _device.Find(m => m.Id == deviceDetails.Id).ToList();

            if (__device.Count > 0)
                throw new InvalidOperationException("Already Exist");

            _device.InsertOne(deviceDetails);

            return true;

        }

    }
}
