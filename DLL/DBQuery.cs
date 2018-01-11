using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.DLLModels;

namespace DLL
{
    public class DBQuery
    {
        protected static IMongoClient mongoClient;
        protected static IMongoDatabase database;

        public DBQuery()
        {
            mongoClient = new MongoClient("mongodb://192.168.56.12:27017");
            database = mongoClient.GetDatabase("novabaza");
        }

        public void SubmitComment(string commentText, int articleId)
        {
            var comment = new BsonDocument
            {
                { "timestamp", BsonDateTime.Create(DateTime.Now)},
                {"text", commentText}
            };

            var collection = database.GetCollection<BsonDocument>("articles");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", articleId);
            var update = Builders<BsonDocument>.Update
                .Push("comments", comment);
            collection.UpdateOneAsync(filter, update);
        }

        public List<QueryResult> GetNews(int count)
        {
            var collection = database.GetCollection<BsonDocument>("articles");
            var filter = new BsonDocument();
            var sort = Builders<BsonDocument>.Sort.Descending("_id");
            var sortedCollection = collection.Find(filter).Sort(sort).Limit(count).ToList();
            var results = new List<QueryResult>();

            foreach (var item in sortedCollection)
            {
                results.Add(MapFromDatabaseToQueryResult(item));
            }

            return results;
        }

        public void Add(QueryResult article)
        {
            var collection = database.GetCollection<BsonDocument>("articles");
            var Id = (int)collection.Count(new BsonDocument());

            var news = new BsonDocument
            {
                {"_id", BsonInt32.Create(Id+1) },
                {"headline", BsonString.Create(article.Headline) },
                {"body", BsonString.Create(article.Body) },
                {"author", BsonString.Create(article.Author) },
                {"comments", new BsonArray() },
                {"image", new BsonBinaryData(article.Image) }
            };

            collection.InsertOne(news);
        }

        public QueryResult MapFromDatabaseToQueryResult(BsonDocument item)
        {
            QueryResult result = new QueryResult();
            BsonValue val;

            item.TryGetValue("_id", out val);
            result.Id = val.AsInt32;

            item.TryGetValue("headline", out val);
            result.Headline = val.AsString;

            item.TryGetValue("body", out val);
            result.Body = val.AsString;

            item.TryGetValue("author", out val);
            result.Author = val.AsString;

            if (item.TryGetValue("image", out val))
            {
                result.Image = val.AsByteArray;
            }

            result.Comments = new List<QueryComment>();

            if (item.TryGetValue("comments", out val))
            {
                var commentList = val.AsBsonArray;
                foreach (var comment in commentList)
                {
                    QueryComment com = new QueryComment();
                    BsonValue temp;
                    var commentAsDocument = comment.AsBsonDocument;
                    commentAsDocument.TryGetValue("timestamp", out temp);
                    com.Timestamp = temp.ToUniversalTime();
                    commentAsDocument.TryGetValue("text", out temp);
                    com.Text = temp.AsString;

                    result.Comments.Add(com);
                }
            }

            return result;
        }
    }
}

