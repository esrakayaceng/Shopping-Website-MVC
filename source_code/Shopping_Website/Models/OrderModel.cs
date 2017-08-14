using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shopping_Website.Models
{
    public class OrderModel
    {
        //using MongoDB.Bson;
        public ObjectId _id { get; set; } //MongoDb uses this field as identity.

        [Required]
        public string CustomerID { get; set; }
        [Required]
        public List<string> ProductIDList { get; set; }
        [Required]
        public List<int> NumberofItems { get; set; }
        [Required]
        public string OrderDate { get; set; }
        [Required]
        public bool OrderStatus { get; set; }
        [Required]
        public double TotalPrice { get; set; }

    }
}