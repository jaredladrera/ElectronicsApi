using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsAPI.Services;

namespace ElectronicsAPI.Services.Interface
{
    public interface ISpecificationService
    {
        object GetAll();
        bool GetById(string id, out object specsInfo);
        object GetAllInformation();
        bool GetAllById(int controlNumber, out object deviceInformation);
    }
}
