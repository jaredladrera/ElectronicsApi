using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using ElectronicsAPI.Models;
using System.Diagnostics.CodeAnalysis;
using ElectronicsAPI.Services.Interface;

namespace ElectronicsAPI.Services
{

    [ExcludeFromCodeCoverage]
    public class DeviceDetailsService : IDeviceDetailsService
    {
        private readonly IMongoCollection<DeviceDetails> _device;
        private readonly IMongoCollection<Specification> _specs;

        public DeviceDetailsService(IMongoCollection<DeviceDetails> device, IMongoCollection<Specification> specs)
        {
            _device = device;
            _specs = specs;
        }

        virtual public object GetAll()
        {
            var deviceInfos = (from deviceInfo in _device.Find(a => true).ToList() select new { 
                deviceID = deviceInfo.Id,
                controlNumber = deviceInfo.controlNumber,
                brand = deviceInfo.brand,
                model = deviceInfo.model,
                isHighEnd = deviceInfo.isHighEnd,
                releaseDate = deviceInfo.releaseDate
            });

            return deviceInfos;
        }

        virtual public object GetAllInformation()
        {

            var deviceInformation = (from deviceinfo in _device.Find(a => true).ToList() join specs in _specs.Find(m => true).ToList() on deviceinfo.Id equals specs.deviceId
                                     select new { 
                                           controlNumber = deviceinfo.controlNumber,
                                           brand = deviceinfo.brand,
                                           model = deviceinfo.model,
                                           isHighEnd = deviceinfo.isHighEnd,
                                           releaseDate = deviceinfo.releaseDate,
                                           ram = specs.ram,
                                           color = specs.color,
                                           hdd = specs.hdd,
                                           operatingSystem = specs.operatingSystem,
                                           processor = specs.processor
                                     });

            return deviceInformation;
        }

        virtual public bool GetByControlNumber(int id, out object deviceInfo)
        {
            if(id <= 0)
            {
                deviceInfo = null;
                return false;
            }

            object deviceInfos = null;

            try
            {
                deviceInfos = (from __deviceinfo in _device.Find(a => a.Id == id).ToList() select new { 
                    controlNumber = __deviceinfo.controlNumber,
                    brand = __deviceinfo.brand,
                    model = __deviceinfo.model,
                    isHighEnd = __deviceinfo.isHighEnd,
                    releaseDate = __deviceinfo.releaseDate
                });
            } catch
            {
                deviceInfo = null;
                return false;
            }

            if((deviceInfos as IEnumerable<object>).Count() <= 0)
            {
                deviceInfo = null;
                return false;
            }

            deviceInfo = (deviceInfos as IEnumerable<object>).First();
            return true;

        }

        virtual public bool GetByBrand(string brand, out object deviceinfo)
        {
            if(brand == "")
            {
                deviceinfo = null;
                return false;
            }
            object deviceInfos = null;

            try
            {
                deviceInfos = (from __deviceinfo in _device.Find(a => a.brand == brand).ToList() select new {
                    controlNumber = __deviceinfo.controlNumber,
                    brand = __deviceinfo.brand,
                    model = __deviceinfo.model,
                    isHighEnd = __deviceinfo.isHighEnd,
                    releaseDate = __deviceinfo.releaseDate
                });
            } 
            catch
            {
                deviceinfo = null;
                return false;
            }

            if((deviceInfos as IEnumerable<object>).Count() <= 0)
            {
                deviceinfo = null;
                return false;
            }

            deviceinfo = (deviceInfos as IEnumerable<object>).First();
            return true;
        }

        public DeviceDetails getById(int id) =>
           _device.Find<DeviceDetails>(gadget => gadget.Id == id).FirstOrDefault();
    }
}
