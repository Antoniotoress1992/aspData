using IgniteUI.SamplesBrowser.Models.Repositories;
using System;

namespace IgniteUI.SamplesBrowser.Models.Northwind
{
    public class Product
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<short> UnitsInStock { get; set; }
        public Nullable<short> UnitsOnOrder { get; set; }
        public Nullable<short> ReorderLevel { get; set; }
        public string SupplierName { get; set; }
        public string CategoryName { get; set; }
        public int Rating { get; set; }
        public bool Discontinued { get; set; }
        public string CategoryImageUrl { get; set; }
    }
}