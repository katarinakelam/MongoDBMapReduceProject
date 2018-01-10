using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DLLModels
{
    public class QueryResult
    {
        [BsonElement("_id")]
        public int Id { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("headline")]
        public string Headline { get; set; }

        [BsonElement("body")]
        public string Body { get; set; }

        [BsonElement("image")]
        public byte[] Image { get; set; }

        [BsonElement("comments")]
        public List<QueryComment> Comments { get; set; }
    }
}
