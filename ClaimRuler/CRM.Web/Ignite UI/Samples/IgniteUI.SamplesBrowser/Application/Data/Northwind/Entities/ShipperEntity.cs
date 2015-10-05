using System;
using System.Collections.Generic;

namespace IgniteUI.SamplesBrowser.Application.Data
{
    public class ShipperEntity
    {
        public ShipperEntity()
        {
            this.Orders = new List<OrderEntity>();
        }

        public int ShipperID { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public virtual ICollection<OrderEntity> Orders { get; set; }
    }
}
