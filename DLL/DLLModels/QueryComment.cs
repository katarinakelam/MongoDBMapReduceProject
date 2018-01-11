using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DLLModels
{
    public class QueryComment
    {
        public DateTime Timestamp { get; set; }

        public string Text { get; set; }
    }
}
