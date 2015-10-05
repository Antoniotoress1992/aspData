using IgniteUI.SamplesBrowser.Models.Repositories;
using System;
using System.Collections.Generic;

namespace IgniteUI.SamplesBrowser.Models.Northwind
{
    public class Employee 
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string TitleOfCourtesy { get; set; }
        public string BirthDate { get; set; }
        public string HireDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string HomePhone { get; set; }
        public string Extension { get; set; }
        public string Notes { get; set; }
        public string Supervisor { get; set; }
        public string ImageUrl { get; set; }
        public virtual IEnumerable<Employee> Employees { get; set; }
    }
}