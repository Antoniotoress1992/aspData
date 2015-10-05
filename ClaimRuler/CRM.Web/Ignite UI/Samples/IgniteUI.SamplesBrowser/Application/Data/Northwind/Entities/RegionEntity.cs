using System;
using System.Collections.Generic;

namespace IgniteUI.SamplesBrowser.Application.Data
{
    public class RegionEntity
    {
        public RegionEntity()
        {
            this.Territories = new List<TerritoryEntity>();
        }

        public int RegionID { get; set; }
        public string RegionDescription { get; set; }
        public virtual ICollection<TerritoryEntity> Territories { get; set; }
    }
}
