using System;
using System.Collections.Generic;

namespace IgniteUI.SamplesBrowser.Application.Data
{
    public class EmployeeEntity
    {
        public EmployeeEntity()
        {
            this.Employees = new List<EmployeeEntity>();
            this.Orders = new List<OrderEntity>();
            this.Territories = new List<TerritoryEntity>();
        }

        public int EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string TitleOfCourtesy { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public Nullable<System.DateTime> HireDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string HomePhone { get; set; }
        public string Extension { get; set; }
        public string Notes { get; set; }
        public Nullable<int> ReportsTo { get; set; }
        public virtual ICollection<EmployeeEntity> Employees { get; set; }
        public virtual EmployeeEntity Employee { get; set; }
        public virtual ICollection<OrderEntity> Orders { get; set; }
        public virtual ICollection<TerritoryEntity> Territories { get; set; }
    }
}
