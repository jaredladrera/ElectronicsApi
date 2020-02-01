using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ElectronicsAPI.Models.Interface;

namespace ElectronicsAPI.Models
{
    public class Specification : ISpecification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int deviceId { get; set; }
        public int ram { get; set; }
        public string color { get; set; }
        public string hdd { get; set; }
        public string operatingSystem { get; set; }
        public string processor { get; set; }
    }
}
