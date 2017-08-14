using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shopping_Website.Models
{
    public class ProductModel
    {
        //using MongoDB.Bson;
        public ObjectId _id { get; set; } //MongoDb uses this field as identity.

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string ImageURL { get; set; }

       
    }
}