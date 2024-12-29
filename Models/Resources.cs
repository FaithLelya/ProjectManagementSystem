using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementSystem.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public string Supplier { get; set; }
        //any other additional fields
    }
}