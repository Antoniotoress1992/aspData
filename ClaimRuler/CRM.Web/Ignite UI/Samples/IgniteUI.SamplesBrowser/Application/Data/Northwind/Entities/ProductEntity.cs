using System;
using System.Collections.Generic;

namespace IgniteUI.SamplesBrowser.Application.Data
{
    public class ProductEntity
    {
        public ProductEntity()
        {
            this.OrderDetails = new List<OrderDetailEntity>();
        }

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<short> UnitsInStock { get; set; }
        public Nullable<short> UnitsOnOrder { get; set; }
        public Nullable<short> ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public virtual CategoryEntity Category { get; set; }
        public virtual ICollection<OrderDetailEntity> OrderDetails { get; set; }
        public virtual SupplierEntity Supplier { get; set; }
    }
}