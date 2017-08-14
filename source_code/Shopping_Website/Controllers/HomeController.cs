using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// MongoDB
using MongoDB.Bson;
using MongoDB.Driver;

// Model
using Shopping_Website.Models;

namespace Shopping_Website.Controllers
{
    public class HomeController : Controller
    { 

        public ActionResult Index()
        {
            // Install-Package mongocsharpdriver for MongoDB
            
            string connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);

            //check if database exists
            var cursor = client.ListDatabases();
            List<string> list = new List<string>();
            // add database name only to the list
            cursor.ForEachAsync(db => list.Add(db["name"].ToString()));
            // check if list has this database name
            if (!list.Contains("ShoppingDB"))
            {
                // create collection = tables 
                DB_Connection.CreateProductCollection();
                DB_Connection.CreateUserCollection();
                DB_Connection.CreateOrderCollection();
            }

            return View();
        }
       
        public ActionResult Contact()
        {
          
            return View();
        }

    }
}