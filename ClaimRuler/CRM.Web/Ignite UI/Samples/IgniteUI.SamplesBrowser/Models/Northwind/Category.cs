using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IgniteUI.SamplesBrowser.Models.Northwind
{
    public class Category
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int ProductCount { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}