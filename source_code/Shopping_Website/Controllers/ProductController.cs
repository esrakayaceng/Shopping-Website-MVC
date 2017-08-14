using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Shopping_Website.Models;
using MongoDB.Bson;

namespace Shopping_Website.Controllers
{
    public class ProductController : Controller
    {
        static List<string> bag;
        List<ProductModel> shopBag;

        static string connectionString = "mongodb://localhost:27017";
        static MongoClient client = new MongoClient(connectionString);
        //Get DB
        static IMongoDatabase db = client.GetDatabase("ShoppingDB");
        //Get Collection
        static IMongoCollection<ProductModel> collection = db.GetCollection<ProductModel>("Products");

        // GET: Product/Index
        public ActionResult Index()
        {
            // find all products
            var list = collection.Find(_ => true).ToList();
            ViewData["Products"] = list;

            return View();
        }

        // Product/GreetingCards
        public ActionResult GreetingCards()
        {
            var list = collection.AsQueryable().Where(b => b.Category.Equals("Greeting Cards")).ToList();
            ViewData["GreetingCards"] = list;

            return View();
        }

        // Product/Notebook
        public ActionResult Notebook()
        {
            var list = collection.AsQueryable().Where(b => b.Category.Equals("Notebook")).ToList();
            ViewData["Notebook"] = list;
            return View();
        }

        // Product/Stationery
        public ActionResult Stationery()
        {
            var list = collection.AsQueryable().Where(b => b.Category.Equals("Stationery")).ToList();
            ViewData["Stationery"] = list;
            return View();
        }

        // Product/EasterGifts
        public ActionResult EasterGifts()
        {
            var list = collection.AsQueryable().Where(b => b.Category.Equals("Easter Gifts")).ToList();
            ViewData["EasterGifts"] = list;
            return View();
        }

        // Product/BathBody
        public ActionResult BathBody()
        {
            var list = collection.AsQueryable().Where(b => b.Category.Equals("Bath & Body")).ToList();
            ViewData["BathBody"] = list;
            return View();
        }

        // Product/Candles
        public ActionResult Candles()
        {
            var list = collection.AsQueryable().Where(b => b.Category.Equals("Candles")).ToList();
            ViewData["Candles"] = list;
            return View();
        }

        // Product/Product Detail
        [HttpGet]
        public ActionResult ProductDetail()
        {
            // Request.Params
            var id = Request.QueryString["id"];
            var list = collection.AsQueryable().Where(b => b._id.Equals(ObjectId.Parse(id))).ToList();
            ViewData["Detail"] = list;
            return View();
        }

        // Get Product/Remove from Bag
        public ActionResult Remove()
        {
            var id = Request.QueryString["id1"];
            bag.Remove(id);
            return RedirectToAction("ShoppingBag", "Product");
        }

        // Product/ShoppingBag
        public ActionResult ShoppingBag()
        {
            // when user submits his order in Customer/SubmitOrder
            if (Session["ShoppingBag"] == null) bag = null;

            if (Request.QueryString["id"] != null || bag != null )
            {
                // if the bag already exists no need to create new one
                if (bag == null) bag = new List<string>();
                if (shopBag == null) shopBag = new List<ProductModel>();
                // GET - this is check becuase od Product/Remove from bag
                if (Request.QueryString["id"] != null)
                {
                    var id = Request.QueryString["id"];
                    // add all id
                    bag.Add(id);
                }
                // get all products which are added to the shopping bag - AddRange -> add list to another list
                foreach (string b in bag) { shopBag.AddRange(collection.AsQueryable().Where(x => x._id.Equals(ObjectId.Parse(b))).ToList()); }
                ViewData["ShoppingBag"] = shopBag;
            }
            Session["ShoppingBag"] = bag;
            return View();
        }

        // Product/Checkout
        public ActionResult Checkout()
        {
            Session["ShoppingBag"] = bag;
            return RedirectToAction("Index", "Login");
        }
    }
}