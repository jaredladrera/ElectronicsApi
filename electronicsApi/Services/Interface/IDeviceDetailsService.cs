using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsAPI.Services.Interface
{
    public interface IDeviceDetailsService
    {
        object GetAll();
        bool GetByControlNumber(int id, out object deviceInfo);
        bool GetByBrand(string brand, out object deviceinfo);
    }
}
           