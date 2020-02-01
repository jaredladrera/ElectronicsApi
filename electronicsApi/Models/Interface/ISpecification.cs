using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ElectronicsAPI.Models.Interface;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsAPI.Models.Interface
{
    public interface ISpecification
    {
        int deviceId { get; set; }
        int ram { get; set; }
        string color { get; set; }
        string hdd { get; set; }
        string operatingSystem { get; set; }
        string processor { get; set; }

    }
}
