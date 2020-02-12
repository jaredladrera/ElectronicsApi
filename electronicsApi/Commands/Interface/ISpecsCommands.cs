using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsAPI.Models;

namespace ElectronicsAPI.Commands.Interface
{
    public interface ISpecsCommands
    {
        int UpdateLatest(int id, Specification devlatest);
        bool DeleteById(int deviceId);
        bool Post(Specification specs);
        bool DeletingById(string id);
    }
}
