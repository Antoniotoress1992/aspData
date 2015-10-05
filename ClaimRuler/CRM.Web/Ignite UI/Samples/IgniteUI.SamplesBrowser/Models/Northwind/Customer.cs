using IgniteUI.SamplesBrowser.Models.Repositories;
using System.Collections.Generic;

namespace IgniteUI.SamplesBrowser.Models.Northwind
{
    public class Customer
    {
        public string ID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}