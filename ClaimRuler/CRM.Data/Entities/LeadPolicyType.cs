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
    
    public partial class LeadPolicyType
    {
        public LeadPolicyType()
        {
            this.AdjusterHandleClaimType = new HashSet<AdjusterHandleClaimType>();
            this.LeadPolicy = new HashSet<LeadPolicy>();
        }
    
        public int LeadPolicyTypeID { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<AdjusterHandleClaimType> AdjusterHandleClaimType { get; set; }
        public virtual ICollection<LeadPolicy> LeadPolicy { get; set; }
    }
}