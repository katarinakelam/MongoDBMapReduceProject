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
        public int Id { get; set; }

        public string Author { get; set; }

        public string Headline { get; set; }

        public string Body { get; set; }

        public byte[] Image { get; set; }

        public List<QueryComment> Comments { get; set; }
    }
}
