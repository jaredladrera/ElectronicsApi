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
    public class SpecificationService : ISpecificationService
    {
        private readonly IMongoCollection<Specification> _specs;
        private readonly IMongoCollection<DeviceDetails> _devDetails;

        public SpecificationService(IMongoCollection<Specification> specs, IMongoCollection<DeviceDetails> devDetails)
        {
            _specs = specs;
            _devDetails = devDetails;
        }

        virtual public object GetAll()
        {
            var specsInfos = (from specsdetails in _specs.Find(a => true).ToList()
                              select new
                              {
                                  deviceId = specsdetails.deviceId,
                                  ram = specsdetails.ram,
                                  color = specsdetails.color,
                                  hdd = specsdetails.hdd,
                                  operatingSystem = specsdetails.operatingSystem,
                                  processor = specsdetails.processor
                              });
            return specsInfos;
        }

        virtual public bool GetById(string id, out object specsInfo)
        {
            if (id == "")
            {
                specsInfo = null;
                return false;
            }
            object specsInfos = null;

            try
            {
                specsInfos = (from __specsInfo in _specs.Find(a => a.Id == id).ToList() select new {
                    deviceId = __specsInfo.deviceId,
                    ram = __specsInfo.ram,
                    color = __specsInfo.color,
                    hdd = __specsInfo.hdd,
                    operatingSystem = __specsInfo.operatingSystem,
                    processor = __specsInfo.processor
                });
            } catch
            {
                specsInfo = null;
                return false;
            }

            if((specsInfos as IEnumerable<object>).Count() <= 0)
            {
                specsInfo = null;
                return false;
            }

            specsInfo = (specsInfos as IEnumerable<object>).First();
            return true;
               
        }



        virtual public bool GetByDeviceId(int deviceId, out object specsInfo)
        {
            if (deviceId < 1)
            {
                specsInfo = null;
                return false;
            }
            object specsInfos = null;

            try
            {
                specsInfos = (from __specsInfo in _specs.Find(a => a.deviceId == deviceId).ToList()
                              select new
                              {
                                  deviceId = __specsInfo.deviceId,
                                  ram = __specsInfo.ram,
                                  color = __specsInfo.color,
                                  hdd = __specsInfo.hdd,
                                  operatingSystem = __specsInfo.operatingSystem,
                                  processor = __specsInfo.processor
                              });
            }
            catch
            {
                specsInfo = null;
                return false;
            }

            if ((specsInfos as IEnumerable<object>).Count() <= 0)
            {
                specsInfo = null;
                return false;
            }

            specsInfo = (specsInfos as IEnumerable<object>).First();
            return true;

        }


        virtual public int GetTest(int id)
        {
            return id;
        }

        virtual public object GetAllInformation()
        {
            var deviceinfos = (from device in _devDetails.Find(a => true).ToList()
                               join specs in _specs.Find(m => true).ToList() on device.Id equals specs.deviceId select new {
                                        Id = device.Id,
                                        controlNumber = device.controlNumber,
                                        brand = device.brand,
                                        model = device.model,
                                        isHighEnd = device.isHighEnd,
                                        releaseDate = device.releaseDate,
                                        deviceId = specs.deviceId,
                                        ram = specs.ram,
                                        color = specs.color,
                                        hdd = specs.hdd,
                                        operatingSystem = specs.operatingSystem,
                                        processor = specs.processor
                               });
            return deviceinfos;
        }

        virtual public bool GetAllById(int controlNumber, out object deviceInformation)
        {
            if(controlNumber <= 0)
            {
                deviceInformation = null;
                return false;
            }

            object deviceInfos = null;

            try
            {
                deviceInfos = (from __deviceInfo in _devDetails.Find(a => a.controlNumber == controlNumber).ToList() join specs
                               in _specs.Find(s => true).ToList() on __deviceInfo.Id equals specs.deviceId into tmpSpecs from specs in tmpSpecs.DefaultIfEmpty()
                               select new {
                                   Id = __deviceInfo.Id,
                                   controlNumber = __deviceInfo.controlNumber,
                                   brand = __deviceInfo.brand,
                                   model = __deviceInfo.model,
                                   isHighEnd = __deviceInfo.isHighEnd,
                                   releaseDate = __deviceInfo.releaseDate,
                                   deviceId = specs.deviceId,
                                   ram = specs.ram,
                                   color = specs.color,
                                   hdd = specs.hdd,
                                   operatingSystem = specs.operatingSystem,
                                   processor = specs.processor
                               });
            } catch
            {
                deviceInformation = null;
                return false;
            }

            if((deviceInfos as IEnumerable<object>).Count() <= 0)
            {
                deviceInformation = null;
                return false;
            }

            deviceInformation = (deviceInfos as IEnumerable<object>).First();
            return true;
        }

        public Specification gettingById(string id) =>
            _specs.Find<Specification>(sp => sp.Id == id).FirstOrDefault();
    }
}
