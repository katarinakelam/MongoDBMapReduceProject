using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DLL;
using MongoDBMapReduceProject.Models;
using System.Drawing;
using System.Web.Mvc;
using DLL.DLLModels;

namespace MongoDBMapReduceProject.Controllers
{
    public class HomeController : Controller
    {
        private DBQuery database = new DBQuery();

        public ActionResult Index(string comment = "")
        {
            var results = database.GetNews(10);
            List<Article> articles = new List<Article>();
            foreach (var result in results)
            {
                articles.Add(MapDatabaseResultToArticle(result));
            }

            comment = "";
            return View(articles);
        }

        [HttpPost]
        public ActionResult Index(string comment, string id)
        {
            if (comment != "")
            {
                database.SubmitComment(comment, int.Parse(id));
            }
            comment = "";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddNews()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNews([Bind(Include = "Headline, Body, Author")]Article article, HttpPostedFileBase upload)
        {
            var news = new QueryResult { Headline = article.Headline, Body = article.Body, Author = article.Author };
            if (upload != null && upload.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    news.Image = reader.ReadBytes(upload.ContentLength);
                }
            }
            database.Add(news);
            return RedirectToAction("Index", "Home");
        }

        public Article MapDatabaseResultToArticle(QueryResult result)
        {
            Article article = new Article();
            article.Id = result.Id;
            article.Author = result.Author;
            article.Headline = result.Headline;
            article.Body = result.Body;

            if (result.Image != null)
            {
                article.Image = result.Image;
            }

            if (result.Comments != null)
            {
                article.Comments = new List<Comment>();
                foreach (var comment in result.Comments)
                {
                    Comment com = new Comment();
                    com.Text = comment.Text;
                    com.Timestamp = comment.Timestamp;
                    article.Comments.Add(com);
                }
            }
            return article;
        }
    }
}
