using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ElectronicsAPI.Models.Interface;
using AutoMapper;


namespace ElectronicsAPI.Models
{
    public class DeviceDetails : IDeviceDetails
    {
        [BsonId]
        public BsonObjectId InternalID { get; set; }
        public int Id { get; set; }
        public int controlNumber { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public bool isHighEnd { get; set; }

        [BsonDateTimeOptions]
        public DateTime? releaseDate { get; set; }
    }
}
