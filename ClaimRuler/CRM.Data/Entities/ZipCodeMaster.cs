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
    
    public partial class ZipCodeMaster
    {
        public ZipCodeMaster()
        {
            this.Carrier = new HashSet<Carrier>();
            this.Mortgagee = new HashSet<Mortgagee>();
        }
    
        public int ZipCodeID { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> StateID { get; set; }
        public string ZipCode { get; set; }
    
        public virtual ICollection<Carrier> Carrier { get; set; }
        public virtual ICollection<Mortgagee> Mortgagee { get; set; }
    }
}
