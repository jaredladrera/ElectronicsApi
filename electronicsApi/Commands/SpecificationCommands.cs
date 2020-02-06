using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using ElectronicsAPI.Models;
using ElectronicsAPI.Commands.Interface;

namespace ElectronicsAPI.Commands
{
    [ExcludeFromCodeCoverage]
    public class SpecificationCommands : ISpecificationCommands
    {
        private readonly IMongoCollection<Specification> _specs;
        private readonly IMongoCollection<DeviceDetails> _device;

        public SpecificationCommands(IMongoCollection<Specification> specs, IMongoCollection<DeviceDetails> device)
        {
            this._specs = specs;
            this._device = device;
        }

        public bool Post(DeviceDetails devDet, Specification spex)
        {
            if (devDet.Id < 1 || spex.deviceId < 1)
                throw new InvalidOperationException("Id must be greater than 0");
            if (devDet == null || spex == null)
                throw new InvalidOperationException("the body can be null");

            var __device = _device.Find(m => m.Id == devDet.Id).ToList();
            var __spec = _specs.Find(s => s.deviceId == spex.deviceId).ToList();

            if (__device.Count() > 0 || __spec.Count() > 0)
                throw new InvalidOperationException("Already Exist");

 

            _device.InsertOne(devDet);
           // _specs.InsertOne(spex);
           
            
            return true;
        }

        
        public bool PostTest(DeviceDetails device)
        {
            if(device.Id < 1)
                throw new InvalidOperationException("ID must be greater than 0");
            if (device == null)
                throw new InvalidOperationException("the body can't be null");


            var __device = _device.Find(m => m.Id == device.Id).ToList();

            if (__device.Count() > 0)
                throw new InvalidOperationException("Already Exist");
            _device.InsertOne(device);

            return true;
        }


    }
}
