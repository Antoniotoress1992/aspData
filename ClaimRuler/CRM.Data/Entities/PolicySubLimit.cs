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
    
    public partial class PolicySubLimit
    {
        public PolicySubLimit()
        {
            this.ClaimSubLimit = new HashSet<ClaimSubLimit>();
        }
    
        public int PolicySublimitID { get; set; }
        public int PolicyID { get; set; }
        public int LimitType { get; set; }
        public string SublimitDescription { get; set; }
        public Nullable<decimal> Sublimit { get; set; }
    
        public virtual ICollection<ClaimSubLimit> ClaimSubLimit { get; set; }
        public virtual LeadPolicy LeadPolicy { get; set; }
    }
}
