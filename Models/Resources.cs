using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementSystem.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal CostPerunit { get; set; }
    }
}