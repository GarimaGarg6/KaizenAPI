using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace KaizenAPI.Models
{
    public class CMClass
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Class_Id")]
        [Required(ErrorMessage ="Class Id is required")]
        public string Class_Id { get; set; }

        [BsonElement("Class")]
        public int Class { get; set; }

        [BsonElement("Sections")]
        public string Sections { get; set; }
    }
}