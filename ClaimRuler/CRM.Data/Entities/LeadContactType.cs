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
    
    public partial class LeadContactType
    {
        public LeadContactType()
        {
            this.Contact = new HashSet<Contact>();
            this.LeadContact = new HashSet<LeadContact>();
        }
    
        public int ID { get; set; }
        public string Description { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<int> ClientID { get; set; }
    
        public virtual ICollection<Contact> Contact { get; set; }
        public virtual ICollection<LeadContact> LeadContact { get; set; }
    }
}
