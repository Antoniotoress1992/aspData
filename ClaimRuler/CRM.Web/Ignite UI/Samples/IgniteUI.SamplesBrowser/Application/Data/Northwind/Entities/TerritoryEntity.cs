using System;
using System.Collections.Generic;

namespace IgniteUI.SamplesBrowser.Application.Data
{
    public class TerritoryEntity
    {
        public TerritoryEntity()
        {
            this.Employees = new List<EmployeeEntity>();
        }

        public string TerritoryID { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionID { get; set; }
        public virtual RegionEntity Region { get; set; }
        public virtual ICollection<EmployeeEntity> Employees { get; set; }
    }
}
