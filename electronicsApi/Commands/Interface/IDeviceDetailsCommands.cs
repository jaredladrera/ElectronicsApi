using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsAPI.Models;

namespace ElectronicsAPI.Commands.Interface
{
    public interface IDeviceDetailsCommands
    {
        int PutLatestVersionAll(int id, DeviceDetails latest);
        bool DeleteById(int id);
        bool Post(DeviceDetails deviceDetails);
        DeviceDetails Create(DeviceDetails deviceDetails);
    }
}
