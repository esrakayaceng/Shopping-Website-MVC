using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Shopping_Website.Models;
using MongoDB.Driver.Builders;

namespace Shopping_Website.Controllers
{
    public class LoginController : Controller
    {
        static string connectionString = "mongodb://localhost:27017";
        static MongoClient client = new MongoClient(connectionString);
        //Get DB
        static IMongoDatabase db = client.GetDatabase("ShoppingDB");
        //Get Collection
        static IMongoCollection<UserModel> collection = db.GetCollection<UserModel>("Users");

        // GET: Login/Index
        [HttpGet]
        public ActionResult Index()
        {
            // check if user logins before
            if (Session["UserID"] != null)
            {
                var id = Session["UserID"];
                // get user
                var c = collection.AsQueryable().Where(b => b._id.Equals(id)).ToList();
                if(c[0].UserType.Equals("customer"))
                    return RedirectToAction("Index", "Customer");
                else
                    return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Index(FormCollection form) 
        {
            var list = collection.AsQueryable().Where(b => b.Username.Equals(form["username"]) && b.Password.Equals(form["password"])).ToList();
            foreach (var b in list)
            {
                Session["UserID"] = b._id;
                if (b.UserType == "customer") return RedirectToAction("Index", "Customer");
                else return RedirectToAction("Index", "Admin");
            }
            TempData["message"] = "We were unable to find that username and password in our system.";
            return View();
        }

        // GET: Login/Registration
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        // POST: Login/Registration
        [HttpPost]
        public ActionResult Registration(FormCollection form)
        {
            UserModel user = new UserModel
            {
                Firstname = form["firstname"],
                Lastname = form["lastname"],
                Username = form["username"],
                Phone = long.Parse(form["phone"]),
                Email = form["email"],
                Address = form["address"],
                City = form["city"],
                State = form["state"],
                ZipCode = int.Parse(form["zipcode"]),
                Password = form["password"],
                UserType = "customer"
            };
            collection.InsertOne(user);
            TempData["message"] = "Account is created successfully";
            //Response.Redirect("/Login/Index");
            return RedirectToAction("Index");
        }
    }
}