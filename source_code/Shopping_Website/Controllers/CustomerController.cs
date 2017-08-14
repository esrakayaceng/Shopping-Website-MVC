using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Shopping_Website.Models;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace Shopping_Website.Controllers
{
    public class CustomerController : Controller
    {

        static string connectionString = "mongodb://localhost:27017";
        static MongoClient client = new MongoClient(connectionString);
        //Get DB
        static IMongoDatabase db = client.GetDatabase("ShoppingDB");
        //Get Collection
        static IMongoCollection<UserModel> collection = db.GetCollection<UserModel>("Users");
        static IMongoCollection<ProductModel> collection_product = db.GetCollection<ProductModel>("Products");
        static IMongoCollection<OrderModel> collection_order = db.GetCollection<OrderModel>("Orders");

        // GET: Customer
        public ActionResult Index()
        {
            var list = collection.AsQueryable().Where(b => b._id.Equals(ObjectId.Parse(Session["UserID"].ToString()))).ToList();
            ViewData["CustomerInfo"] = list;
            return View();
        }

        // Get Customer/Logout
        public ActionResult Logout()
        {
            Session.Remove("UserID");
            return RedirectToAction("Index", "Login");
        }

        // Get Customer/Checkout
        public ActionResult Checkout()
        {
            // Get product and number of it that a user orders
            Dictionary<List<ProductModel>, int> productList = new Dictionary<List<ProductModel>, int>();
            // check if session is empty or not
            if (Session["ShoppingBag"] as List<string> != null)
            {
                var idList = Session["ShoppingBag"] as IEnumerable<string>;
                // count number of times a product's id appears 
                Dictionary<string, int> count = idList.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
                // return Product properties and b.value is number of Product that user wants to buy
                foreach (var b in count) {
                    productList.Add(collection_product.AsQueryable().Where(x => x._id.Equals(ObjectId.Parse(b.Key))).ToList(), b.Value);
                }
                // The list contains <Product, Number of Product that user wants to buy>
                List<DataDictionary> list = productList.Select(p => new DataDictionary { ListofProduct = p.Key, Number = p.Value }).ToList();

                ViewData["Checkout"] = list;

               
                // calculate total price
                IEnumerable<double> totalPrice = list.Select(x => x.ListofProduct[0].Price * x.Number); 
                ViewData["TotalPrice"] = totalPrice.Sum();
                Session["TotalPrice"] = totalPrice.Sum();
            }

            return View();
        }

        // Customer/Remove - remove button in checkout
        public ActionResult Remove()
        {
            // Session["ShoppingBag"] = bag; all of products' id
            var id = Request.QueryString["id"];
            if (Session["ShoppingBag"] != null)
            {
                var SessionList = Session["ShoppingBag"] as List<string>;
                // remove only one match not all
                SessionList.Remove(id);
                Session["ShoppingBag"] = SessionList;
            }
            return RedirectToAction("Checkout", "Customer");
        }
        // Customer/SubmitOrder
        public ActionResult SubmitOrder()
        {
            var idList = Session["ShoppingBag"] as IEnumerable<string>;
            // count number of times a product's id appears 
            Dictionary<string, int> count = idList.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

            // reduce quantity for each product
            foreach (var product in count)
            {
                // find product
                var filter = Builders<ProductModel>.Filter.Eq("_id", ObjectId.Parse(product.Key));
                // get current quantity
                var p = collection_product.AsQueryable().Where(x => x._id.Equals(ObjectId.Parse(product.Key))).ToList();
                int quantity = p[0].Quantity;
                // check if there is enough amount in the system
                if (quantity >= product.Value)
                {
                    // update
                    var updateQuantity = Builders<ProductModel>.Update.Set("Quantity", (quantity - product.Value));
                    collection_product.FindOneAndUpdate(filter, updateQuantity);
                }
                else{
                    TempData["message"] = "There is only " + quantity.ToString() +" "+ p[0].Name +" in stock";
                    return RedirectToAction("Checkout", "Customer");
                }

            }
            // add order to Order table
            OrderModel order = new OrderModel
            {
                CustomerID = Session["UserID"].ToString(),
                // conver Dictionary list to List
                ProductIDList = count.Keys.Select(k => k).ToList(),
                NumberofItems = count.Values.Select(v => v).ToList(),
                OrderDate = DateTime.Now.ToShortDateString(),
                OrderStatus = false,
                TotalPrice = (double)Session["TotalPrice"]
            };
            collection_order.InsertOne(order);

            Session.Remove("ShoppingBag");

            return RedirectToAction("Index", "Customer");
        }

        // Customer/EditAccount
        [HttpGet]
        public ActionResult EditAccount()
        {
            var id = Session["UserID"].ToString();
            // find the user
            var user = collection.AsQueryable().Where(x => x._id.Equals(ObjectId.Parse(id))).ToList();
            ViewData["UserInfo"] = user;
            return View();
        }

        [HttpPost]
        public ActionResult EditAccount(FormCollection form)
        {
            var id = Session["UserID"].ToString();
            var filter = Builders<UserModel>.Filter.Eq("_id", ObjectId.Parse(id));
            var updateUsername = Builders<UserModel>.Update.Set("Username", form["username"]);
            var updatePassword = Builders<UserModel>.Update.Set("Password", form["password"]);
            var updateEmail = Builders<UserModel>.Update.Set("Email", form["email"]);
            var updateAddress = Builders<UserModel>.Update.Set("Address", form["address"]);
            var updateCity = Builders<UserModel>.Update.Set("City", form["city"]);
            var updateState = Builders<UserModel>.Update.Set("State", form["state"]);
            var updateZipCode = Builders<UserModel>.Update.Set("ZipCode", form["zipCode"]);
            var updatePhone = Builders<UserModel>.Update.Set("Phone", form["phone"]);
            var combinedUpdates = Builders<UserModel>.Update.Combine(updateUsername, updatePassword,updateEmail,updateAddress,updateCity,updateState,updateZipCode,updatePhone);
            collection.FindOneAndUpdate(filter, combinedUpdates);

            TempData["message"] = "You have successfully updated your account";

            return RedirectToAction("Index", "Customer");
        }

        // Customer/Order
        [HttpGet]
        public ActionResult OrderStatus()
        {
            var id = Session["UserID"].ToString();
            // find the order base on customer id
            var orders = collection_order.AsQueryable().Where(x => x.CustomerID.Equals(ObjectId.Parse(id))).ToList();
            // sort by latest order date
            ViewData["Orders"] = orders.OrderByDescending(x=>x.OrderDate);
            return View();
        }

        // Customer/OrderDetail
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
            foreach (var o in product_id_list) {
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


    }
}