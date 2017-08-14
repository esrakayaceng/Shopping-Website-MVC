using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Shopping_Website.Models;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace Shopping_Website.Controllers
{
    public class AdminController : Controller
    {
        static string connectionString = "mongodb://localhost:27017";
        static MongoClient client = new MongoClient(connectionString);
        //Get DB
        static IMongoDatabase db = client.GetDatabase("ShoppingDB");
        //Get Collection
        static IMongoCollection<UserModel> collection = db.GetCollection<UserModel>("Users");
        static IMongoCollection<ProductModel> collection_product = db.GetCollection<ProductModel>("Products");
        static IMongoCollection<OrderModel> collection_order = db.GetCollection<OrderModel>("Orders");

        // GET: Admin
        public ActionResult Index()
        {
            var list = collection.AsQueryable().Where(b => b._id.Equals(ObjectId.Parse(Session["UserID"].ToString()))).ToList();
            ViewData["AdminInfo"] = list;
            return View();
        }

        // Get Admin/Logout
        public ActionResult Logout()
        {
            Session.Remove("UserID");
            return RedirectToAction("Index", "Login");
        }

        // Get Admin/Customers
        public ActionResult Customers()
        {
            // find all customers
            var list = collection.Find(_ => _.UserType.Equals("customer")).ToList();
            ViewData["Customers"] = list;
            return View();
        }

        // Get Admin/Products
        public ActionResult Products()
        {
            return View();
        }

        // Admin/CustomerDetail
        public ActionResult CustomerDetail()
        {
            var id = Request.QueryString["id"];
            var list = collection.AsQueryable().Where(b => b._id.Equals(ObjectId.Parse(id))).ToList();
            ViewData["Info"] = list;
            return View();
        }

        // Admin/DeleteCustomer
        public ActionResult DeleteCustomer()
        {
            var id = Request.QueryString["id"];
            // delete customer orders
            collection_order.DeleteMany(d =>d.CustomerID.Equals(ObjectId.Parse(id)));
            // delete customer
            collection.DeleteOne(b => b._id.Equals(ObjectId.Parse(id)));
            TempData["message"] = "The customer and customer's orders deleted from the system.";
            return RedirectToAction("Customers", "Admin");
        }

        // Admin/DeleteProduct
        public ActionResult DeleteProduct()
        {
            var id = Request.QueryString["id"];
            // delete product
            collection_product.DeleteOne(b => b._id.Equals(ObjectId.Parse(id)));
            TempData["message"] = "The product deleted from the system.";
            return RedirectToAction("Products", "Admin");
        }

        // Admin/EditCustomer
        [HttpGet]
        public ActionResult EditCustomer()
        {
            var id = Request.QueryString["id"];
            // find the customer
            var customer = collection.AsQueryable().Where(x => x._id.Equals(ObjectId.Parse(id))).ToList();
            ViewData["CustomerInfo"] = customer;
            return View();
        }
        // Admin/EditCustomer
        [HttpPost]
        public ActionResult EditCustomer(FormCollection form)
        {
            var id = Request.Form["CustomerID"];
            var filter = Builders<UserModel>.Filter.Eq("_id", ObjectId.Parse(id));
            var updateFirstname = Builders<UserModel>.Update.Set("Firstname", form["fname"]);
            var updateLastname = Builders<UserModel>.Update.Set("Lastname", form["lname"]);
            var updateUserType = Builders<UserModel>.Update.Set("UserType", form["type"]);
            var updateUsername = Builders<UserModel>.Update.Set("Username", form["username"]);
            var updatePassword = Builders<UserModel>.Update.Set("Password", form["password"]);
            var updateEmail = Builders<UserModel>.Update.Set("Email", form["email"]);
            var updateAddress = Builders<UserModel>.Update.Set("Address", form["address"]);
            var updateCity = Builders<UserModel>.Update.Set("City", form["city"]);
            var updateState = Builders<UserModel>.Update.Set("State", form["state"]);
            var updateZipCode = Builders<UserModel>.Update.Set("ZipCode", form["zipCode"]);
            var updatePhone = Builders<UserModel>.Update.Set("Phone", form["phone"]);
            var combinedUpdates = Builders<UserModel>.Update.Combine(updateFirstname, updateLastname, updateUserType, updateUsername, updatePassword, updateEmail, updateAddress, updateCity, updateState, updateZipCode, updatePhone);
            collection.FindOneAndUpdate(filter, combinedUpdates);

            TempData["message"] = "You have successfully updated the account";

            return RedirectToAction("Customers", "Admin");
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(FormCollection form)
        {
            // System.IO.File.Copy(Request.Form["pic"], "~/Content/images/");
            ProductModel product = new ProductModel
            {
                Name = Request.Form["pname"],
                Category = Request.Form["category"],
                Description = Request.Form["description"],
                // convert string to double
                Price = Double.Parse(Request.Form["price"]),
                Quantity = int.Parse(Request.Form["quantity"]),
                ImageURL = Request.Form["pic"]
            };
            collection_product.InsertOne(product);

            TempData["message"] = "You have successfully added a new product to the system";

            return RedirectToAction("Products", "Admin");
        }

        // Admin/GreetingCards
        public ActionResult GreetingCards()
        {
            var list = collection_product.AsQueryable().Where(b => b.Category.Equals("Greeting Cards")).ToList();
            ViewData["GreetingCards"] = list;

            return View();
        }

        // Admin/Notebook
        public ActionResult Notebook()
        {
            var list = collection_product.AsQueryable().Where(b => b.Category.Equals("Notebook")).ToList();
            ViewData["Notebook"] = list;
            return View();
        }

        // Admin/Stationery
        public ActionResult Stationery()
        {
            var list = collection_product.AsQueryable().Where(b => b.Category.Equals("Stationery")).ToList();
            ViewData["Stationery"] = list;
            return View();
        }

        // Admin/EasterGifts
        public ActionResult EasterGifts()
        {
            var list = collection_product.AsQueryable().Where(b => b.Category.Equals("Easter Gifts")).ToList();
            ViewData["EasterGifts"] = list;
            return View();
        }

        // Admin/BathBody
        public ActionResult BathBody()
        {
            var list = collection_product.AsQueryable().Where(b => b.Category.Equals("Bath & Body")).ToList();
            ViewData["BathBody"] = list;
            return View();
        }

        // Admin/Candles
        public ActionResult Candles()
        {
            var list = collection_product.AsQueryable().Where(b => b.Category.Equals("Candles")).ToList();
            ViewData["Candles"] = list;
            return View();
        }

        // Admin/Product Detail
        [HttpGet]
        public ActionResult ProductDetail()
        {
            // Request.Params
            var id = Request.QueryString["id"];
            var list = collection_product.AsQueryable().Where(b => b._id.Equals(ObjectId.Parse(id))).ToList();
            ViewData["Detail"] = list;
            return View();
        }

        // Admin/EditProduct
        [HttpGet]
        public ActionResult EditProduct()
        {
            var id = Request.QueryString["id"];
            // find the product
            var product = collection_product.AsQueryable().Where(x => x._id.Equals(ObjectId.Parse(id))).ToList();
            ViewData["ProductInfo"] = product;
            return View();
        }

        // Admin/EditProduct
        [HttpPost]
        public ActionResult EditProduct(FormCollection form)
        {
            var id = Request.Form["ProductID"];

            // get the previous image
            if(form["image"] == "") {
                var product = collection_product.AsQueryable().Where(x => x._id.Equals(ObjectId.Parse(id))).ToList();
                form["image"] = product[0].ImageURL;
            }

            var filter = Builders<ProductModel>.Filter.Eq("_id", ObjectId.Parse(id));
            var updateName = Builders<ProductModel>.Update.Set("Name", form["name"]);
            var updateCategory = Builders<ProductModel>.Update.Set("Category", form["category"]);
            var updatePrice = Builders<ProductModel>.Update.Set("Price", form["price"]);
            var updateDescription = Builders<ProductModel>.Update.Set("Description", form["description"]);
            var updateQuantity = Builders<ProductModel>.Update.Set("Quantity", form["quantity"]);
            var updateImage = Builders<ProductModel>.Update.Set("ImageURL", form["image"]);
            var combinedUpdates = Builders<ProductModel>.Update.Combine(updateName, updateCategory, updatePrice, updateDescription, updateQuantity, updateImage);
            collection_product.FindOneAndUpdate(filter, combinedUpdates);

            TempData["message"] = "You have successfully updated the product";

            return RedirectToAction("Products", "Admin");
        }

        // Admin/Orders
        [HttpGet]
        public ActionResult Orders()
        {
            List<UserModel> customers = new List<UserModel>();
            List<OrderModel> orders = new List<OrderModel>();
            List<string> customerIDList = new List<string>();
           
            // find all orders
            var list = collection_order.Find(_ => true).ToList();
            // get all customers' id
            customerIDList.AddRange(list.Select(x => x.CustomerID));
            // remove duplicate customers' id
            foreach (var id in customerIDList.Distinct())
            {
                // get list of all customers' info
                customers.AddRange(collection.AsQueryable().Where(x => x._id.Equals(ObjectId.Parse(id))).ToList());
                // get list of a customer orders
                orders.AddRange(collection_order.AsQueryable().Where(x => x.CustomerID.Equals(ObjectId.Parse(id))).ToList());
                //dic.Add(customers, orders);
            }

            ViewData["Customers"] = customers;
            ViewData["Orders"] = orders;
            return View();
        }

        // Admin/OrderDetail
        public ActionResult OrderDetail()
        {
            List<ProductModel> list = new List<ProductModel>();
            Dictionary<List<ProductModel>, List<int>> dic = new Dictionary<List<ProductModel>, List<int>>();

            var id = Request.QueryString["id"];
            // find the order base order id
            var orders = collection_order.AsQueryable().Where(x => x._id.Equals(ObjectId.Parse(id))).ToList();
            // get ProductIDList
            var product_id_list = orders[0].ProductIDList;
            // get each product
            foreach (var o in product_id_list)
            {
                var product = collection_product.AsQueryable().Where(x => x._id.Equals(ObjectId.Parse(o)));
                list.AddRange(product);
            }
            // get number of items which customer wants
            var number_list = orders[0].NumberofItems;
            // dic<list of products, list of number of products that customer orders>
            dic.Add(list, number_list);
            ViewData["ListofProduct"] = dic;
            return View();
        }

        // Admin/Ready
        [HttpGet]
        public ActionResult Ready()
        {
            // order id
            var id = Request.QueryString["id"];
            var filter = Builders<OrderModel>.Filter.Eq("_id", ObjectId.Parse(id));
            var updateStatus = Builders<OrderModel>.Update.Set("OrderStatus", true);
            collection_order.FindOneAndUpdate(filter, updateStatus);
            return RedirectToAction("Orders", "Admin");
        }

        // Admin/NotReady
        [HttpGet]
        public ActionResult NotReady()
        {
            // order id
            var id = Request.QueryString["id"];
            var filter = Builders<OrderModel>.Filter.Eq("_id", ObjectId.Parse(id));
            var updateStatus = Builders<OrderModel>.Update.Set("OrderStatus", false);
            collection_order.FindOneAndUpdate(filter, updateStatus);
            return RedirectToAction("Orders", "Admin");
        }

        // Admin/Search
        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        // Admin/SearchCustomers
        [HttpGet]
        public ActionResult SearchCustomers()
        {
            return View();
        }

        // Admin/SearchCustomers
        [HttpPost]
        public ActionResult SearchCustomers(FormCollection form)
        {

            if (Request.Form["lname"] != "")
            {
                var filter = Builders<UserModel>.Filter.Regex("Lastname", new BsonRegularExpression(Request.Form["lname"], "i"));
                filter = filter & Builders<UserModel>.Filter.Eq(x => x.UserType, "customer");
                var customer = collection.Find(filter);
                ViewData["Data"] = customer.ToList();
                return View();
            }
            else if (Request.Form["email"] != "")
            {
                var filter = Builders<UserModel>.Filter.Regex("Email", new BsonRegularExpression(Request.Form["email"], "i"));
                filter = filter & Builders<UserModel>.Filter.Eq(x => x.UserType, "customer");
                var customer = collection.Find(filter);
                ViewData["Data"] = customer.ToList();
                return View();
            }
            else if(Request.Form["phonenumber"]!= "")
            {
                var customer = collection.AsQueryable().Where(x => x.Phone.Equals(Request.Form["phonenumber"]) && x.UserType.Equals("customer"));
                ViewData["Data"] = customer.ToList();
                return View();
            }
            else if (Request.Form["state"] != "")
            {
                var customer = collection.AsQueryable().Where(x => x.State.Equals(Request.Form["state"]) && x.UserType.Equals("customer"));
                ViewData["Data"] = customer.ToList();
                return View();
            }
            else if (Request.Form["zipcode"] != "")
            {
                var customer = collection.AsQueryable().Where(x => x.ZipCode.Equals(Request.Form["zipcode"]) && x.UserType.Equals("customer"));
                ViewData["Data"] = customer.ToList();
                return View();
            }
       
            TempData["message"] = "The customer that you are looking for is not in the system";
            return View();
        }

        // Admin/SearchProducts
        [HttpGet]
        public ActionResult SearchProducts()
        {
            return View();
        }

        // Admin/SearchProducts
        [HttpPost]
        public ActionResult SearchProducts(FormCollection form)
        {
            if (Request.Form["name"] != "")
            {
                var filter = Builders<ProductModel>.Filter.Regex("Name", new BsonRegularExpression(Request.Form["name"], "i"));
                var product = collection_product.Find(filter);
                ViewData["Data"] = product.ToList();
                return View();
            }
            else if (Request.Form["category"] != "" && Request.Form["price"] != "")
            {
                var product = collection_product.AsQueryable().Where(x => x.Category.Equals(Request.Form["category"]) && x.Price <= double.Parse(Request.Form["price"]));
                ViewData["Data"] = product.ToList();
                return View();
            }
            else if (Request.Form["category"] != "" && Request.Form["quantity"] != "")
            {
                var product = collection_product.AsQueryable().Where(x => x.Category.Equals(Request.Form["category"]) && x.Quantity <= int.Parse(Request.Form["quantity"]));
                ViewData["Data"] = product.ToList();
                return View();
            }
            else if (Request.Form["category"] != "")
            {
                var product = collection_product.AsQueryable().Where(x => x.Category.Equals(Request.Form["category"]));
                ViewData["Data"] = product.ToList();
                return View();
            }
            else if (Request.Form["price"] != "")
            {
                var product = collection_product.AsQueryable().Where(x => x.Price <= double.Parse(Request.Form["price"]));
                ViewData["Data"] = product.ToList();
                return View();
            }
            else if (Request.Form["quantity"] != "")
            {
                var product = collection_product.AsQueryable().Where(x => x.Quantity <= int.Parse(Request.Form["quantity"]));
                ViewData["Data"] = product.ToList();
                return View();
            }

            TempData["message"] = "The product that you are looking for is not in the system";
            return View();
        }

        // Admin/SearchOrders
        [HttpGet]
        public ActionResult SearchOrders()
        {
            return View();
        }

        // Admin/SearchOrders
        [HttpPost]
        public ActionResult SearchOrders(FormCollection form)
        {
            List<UserModel> customers = new List<UserModel>();
            List<OrderModel> orders = new List<OrderModel>();
            List<OrderModel> order = new List<OrderModel>();
            List<string> customerIDList = new List<string>();
            List<string> allDates = new List<string>();
            

            if (Request.Form["dateFrom"] != "" || Request.Form["dateTo"] != "")
            {
                // if user did not enter date from
                if (Request.Form["dateFrom"] == "") { Request.Form["dateFrom"] = DateTime.Now.AddMonths(-1).ToShortDateString(); }
                // if user did not enter date to
                if (Request.Form["dateTo"] == "") { Request.Form["dateTo"] = DateTime.Now.ToShortDateString(); }

                // change format of data from YYYY-MM-DD to MM/DD/YYYY
                DateTime startingDate = DateTime.Parse(String.Format("{0:M/d/yyyy}", Request.Form["dateFrom"]));
                DateTime endingDate = DateTime.Parse(String.Format("{0:M/d/yyyy}", Request.Form["dateTo"]));

                // get all dates between two dates
                for (DateTime date = startingDate; date <= endingDate; date = date.AddDays(1))
                   allDates.Add(date.ToShortDateString());
                foreach (var date in allDates) {
                    //get all orders for those dates
                    order.AddRange(collection_order.AsQueryable().Where(x => x.OrderDate.Equals(date)).ToList());
                }
                // get all customers' id
                customerIDList.AddRange(order.Select(x => x.CustomerID));
                // remove duplicate customers' id
                foreach (var id in customerIDList.Distinct())
                {
                    // get list of all customers' info
                    customers.AddRange(collection.AsQueryable().Where(x => x._id.Equals(ObjectId.Parse(id))).ToList());
                    // get list of a customer orders for those dates
                    foreach (var date in allDates)
                    {
                        orders.AddRange(collection_order.AsQueryable().Where(x => x.CustomerID.Equals(ObjectId.Parse(id)) && x.OrderDate.Equals(date)).ToList());
                    }
                    
                }

                ViewData["Customers"] = customers;
                ViewData["Orders"] = orders;

                return View();
            }

            else if (Request.Form["status"] != "")
            {
                bool b = Request.Form["status"].Equals("true") ? true : false;
                order = collection_order.AsQueryable().Where(x => x.OrderStatus == b).ToList();
                // get all customers' id
                customerIDList.AddRange(order.Select(x => x.CustomerID));
                // remove duplicate customers' id
                foreach (var id in customerIDList.Distinct())
                {
                    // get list of all customers' info
                    customers.AddRange(collection.AsQueryable().Where(x => x._id.Equals(ObjectId.Parse(id))).ToList());
                    // get list of a customer orders with the status
                    orders.AddRange(collection_order.AsQueryable().Where(x => x.CustomerID.Equals(ObjectId.Parse(id))&& x.OrderStatus == b).ToList());
                }
                ViewData["Customers"] = customers;
                ViewData["Orders"] = orders;

                return View();
            }

            TempData["message"] = "The customer that you are looking for is not in the system";
            return View();
        }

    }
}