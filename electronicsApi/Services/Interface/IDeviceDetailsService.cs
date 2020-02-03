using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsAPI.Models;

namespace ElectronicsAPI.Services.Interface
{
    public interface IDeviceDetailsService
    {
        object GetAll();
        bool GetByControlNumber(int id, out object deviceInfo);
        bool GetByBrand(string brand, out object deviceinfo);
        DeviceDetails getById(int Id);
    }
}
           