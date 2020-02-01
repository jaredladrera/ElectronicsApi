using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsAPI.Models.Interface
{
    public interface IDeviceDetails
    {
        int Id { get; set; }
        int controlNumber { get; set; }
        string brand { get; set; }
        string model { get; set; }
        bool isHighEnd { get; set; }
        DateTime? releaseDate { get; set; }
    }
}
