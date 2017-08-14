using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping_Website.Models
{
    public class DB_Connection
    {
        static string connectionString = "mongodb://localhost:27017";
        static MongoClient client = new MongoClient(connectionString);
        //Create DB
        static IMongoDatabase db = client.GetDatabase("ShoppingDB");

        public static void CreateProductCollection()
        {

            // Create Collection
            var collection_product = db.GetCollection<ProductModel>("Products");

            ProductModel product = new ProductModel
            {
                Name = "Fresh Linen Small",
                Category = "Candles",
                Description = "Freshen up any space with this amazing candle. Scents of fresh crisp linen combine with a touch of soft musk and ocean breeze for a clean, fresh fragrance.",
                Price = 7.95,
                Quantity = 10,
                ImageURL = "FreshLinenSmall.jpg"
            };
            collection_product.InsertOne(product);

            product = new ProductModel
            {
                Name = "Hello Panda",
                Category = "Notebook",
                Description = "An adorable panda greets you from the cover of this darling cloth covered notebook. A fun lime green elastic band keeps your thoughts and notes secure.",
                Price = 9.95,
                Quantity = 5,
                ImageURL = "HelloPanda.jpg"
            };
            collection_product.InsertOne(product);

            product = new ProductModel
            {
                Name = "Hummingbird Boxed",
                Category = "Stationery",
                Description = "A majestic hummingbird is accented with purple glitter on this sweet note card that is sure to brighten anyone day!",
                Price = 10.95,
                Quantity = 15,
                ImageURL = "HummingbirdBoxed.jpg"
            };
            collection_product.InsertOne(product);

            product = new ProductModel
            {
                Name = "Easter Joy Dessert Plates",
                Category = "Easter Gifts",
                Description = "Host your Easter gathering with these cute paper plates that feature a bunny soaring above a mountain of Easter eggs. Perfect for appetizers or dessert.",
                Price = 4.95,
                Quantity = 2,
                ImageURL = "EasterJoyDessertPlates.jpg"
            };
            collection_product.InsertOne(product);

            product = new ProductModel
            {
                Name = "Dream Bubble Bath",
                Category = "Bath & Body",
                Description = "Surround yourself in luxury with this indulging bubble bath. The delicious white tea and honeysuckle scents leave you feeling clean and fresh.",
                Price = 49.00,
                Quantity = 2,
                ImageURL = "DreamBubbleBath.jpg"
            };
            collection_product.InsertOne(product);

            product = new ProductModel
            {
                Name = "Taylor Swift Lollipop Birthday Card",
                Category = "Greeting Cards",
                Description = "Wish her the sweetest birthday with this fun card from our Taylor Swift collection. A lollipop is created out of felt and has a fun pink and orange swirl that is sure to stand out at any celebration.",
                Price = 7.95,
                Quantity = 33,
                ImageURL = "TaylorSwiftLollipop.jpg"
            };
            collection_product.InsertOne(product);

            product = new ProductModel
            {
                Name = "Handmade Cardigan & Bowtie Birthday Card",
                Category = "Greeting Cards",
                Description = "Hand them this handsome birthday card. Real fabric creates a gray cardigan, button-down shirt and bow tie on this well-dressed, hip card.",
                Price = 8.95,
                Quantity = 55,
                ImageURL = "HandmadeCardigan.jpg"
            };
            collection_product.InsertOne(product);
        }

        public static void CreateUserCollection()
        {
            // Users
            // Create Collection
            var collection_user = db.GetCollection<UserModel>("Users");
            UserModel user = new UserModel
            {
                Firstname = "Sara",
                Lastname = "Smith",
                Username = "ssmith",
                Phone = 6127319831,
                Email = "sara.smith@gmail.com",
                Address = "123 Main St.",
                City = "Boston",
                State = "MA",
                ZipCode = 02215,
                Password = "12345",
                UserType = "customer"
            };
            collection_user.InsertOne(user);

            user = new UserModel
            {
                Firstname = "David",
                Lastname = "Dao",
                Username = "ddao",
                Phone = 6127319831,
                Email = "david.dao@gmail.com",
                Address = "254 Main St.",
                City = "Allston",
                State = "MA",
                ZipCode = 02134,
                Password = "12345",
                UserType = "customer"
            };
            collection_user.InsertOne(user);

            user = new UserModel
            {
                Firstname = "Sahar",
                Lastname = "Amini",
                Username = "samini",
                Phone = 6127319833,
                Email = "samini@bu.edu",
                Address = "808 Commonwealth Ave.",
                City = "Boston",
                State = "MA",
                ZipCode = 02215,
                Password = "12345",
                UserType = "admin"
            };
            collection_user.InsertOne(user);
        }

        public static void CreateOrderCollection()
        {

            // Order
            // Create Collection
            var collection_order = db.GetCollection<OrderModel>("Orders");
        }
    }
}