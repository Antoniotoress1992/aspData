using System;
using System.Collections.Generic;

namespace IgniteUI.SamplesBrowser.Application.Data
{
    public class CategoryEntity
    {
        public CategoryEntity()
        {
            this.Products = new List<ProductEntity>();
        }

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public virtual ICollection<ProductEntity> Products { get; set; }
    }
}
