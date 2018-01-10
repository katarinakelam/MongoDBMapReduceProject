using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoDBMapReduceProject.Models
{
    public class Comment
    {
        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}