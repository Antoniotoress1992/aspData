//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRM.Data.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class CountryMaster
    {
        public CountryMaster()
        {
            this.Carrier = new HashSet<Carrier>();
            this.CarrierLocation = new HashSet<CarrierLocation>();
        }
    
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string CreatedMachineIP { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string UpdatedMachineIP { get; set; }
    
        public virtual ICollection<Carrier> Carrier { get; set; }
        public virtual ICollection<CarrierLocation> CarrierLocation { get; set; }
    }
}