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
    
    public partial class ClaimLimit
    {
        public ClaimLimit()
        {
            this.PolicyLimit = new HashSet<PolicyLimit>();
        }
    
        public int ClaimLimitID { get; set; }
        public int ClaimID { get; set; }
        public int LimitID { get; set; }
        public Nullable<decimal> LossAmountACV { get; set; }
        public Nullable<decimal> LossAmountRCV { get; set; }
        public Nullable<decimal> Depreciation { get; set; }
        public Nullable<decimal> OverageAmount { get; set; }
        public Nullable<decimal> NonRecoverableDepreciation { get; set; }
        public string ApplyTo { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> PolicyID { get; set; }
        public Nullable<int> PolicyLimitID { get; set; }
    
        public virtual Claim Claim { get; set; }
        public virtual Limit Limit { get; set; }
        public virtual ClaimLimit ClaimLimit1 { get; set; }
        public virtual ClaimLimit ClaimLimit2 { get; set; }
        public virtual ICollection<PolicyLimit> PolicyLimit { get; set; }
    }
}
