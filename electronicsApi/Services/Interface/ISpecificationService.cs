using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsAPI.Services;
using ElectronicsAPI.Models;

namespace ElectronicsAPI.Services.Interface
{
    public interface ISpecificationService
    {
        object GetAll();
        bool GetById(string id, out object specsInfo);
        object GetAllInformation();
        bool GetAllById(int controlNumber, out object deviceInformation);
        bool GetByDeviceId(int deviceId, out object specsInfo);
        Specification gettingById(string id);
        int GetTest(int id);

    }
}
