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
        [BsonElement("clientId")]
        public int ClientId { get; set; } 
        [BsonElement("clientName")]
        public string ClientName { get; set; } 
        [BsonElement("clientNo")]
        public string ClientNo { get; set; } 
        [BsonElement("birthDate")]
        public DateTime? BirthDate { get; set; } 
        [BsonElement("createdBy")]
        public string CreatedBy { get; set; }
        [BsonElement("createdDate")]
        public DateTime? CreatedDate { get; set; } 
        [BsonElement("updatedBy")]
        public string UpdatedBy { get; set; } 
        [BsonElement("updatedDate")]
        public DateTime? UpdatedDate { get; set; }
    }
}