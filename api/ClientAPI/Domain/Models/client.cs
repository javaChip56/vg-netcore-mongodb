using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClientAPI.Domain.Models
{
    public class Client
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 
        public int ClientId { get; set; } 
        public string ClientName { get; set; } 
        public string ClientNo { get; set; } 
        public DateTime? BirthDate { get; set; } 
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } 
        public string UpdatedBy { get; set; } 
        public DateTime? UpdatedDate { get; set; }
    }
}