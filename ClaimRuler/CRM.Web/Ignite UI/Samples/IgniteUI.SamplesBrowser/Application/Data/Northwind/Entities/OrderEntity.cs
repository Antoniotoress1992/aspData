using System;
using System.Collections.Generic;

namespace IgniteUI.SamplesBrowser.Application.Data
{
    public class OrderEntity
    {
        public OrderEntity()
        {
            this.OrderDetails = new List<OrderDetailEntity>();
        }

        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<System.DateTime> RequiredDate { get; set; }
        public Nullable<System.DateTime> ShippedDate { get; set; }
        public Nullable<int> ShipVia { get; set; }
        public Nullable<decimal> Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public virtual CustomerEntity Customer { get; set; }
        public virtual EmployeeEntity Employee { get; set; }
        public virtual ICollection<OrderDetailEntity> OrderDetails { get; set; }
        public virtual ShipperEntity Shipper { get; set; }
    }
}
