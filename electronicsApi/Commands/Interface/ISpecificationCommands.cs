using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsAPI.Commands;
using ElectronicsAPI.Models;

namespace ElectronicsAPI.Commands.Interface
{
    public interface ISpecificationCommands
    {
        bool Post(DeviceDetails devDet, Specification spex);
        bool PostTest(DeviceDetails device);

    }
}
