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
    
    public partial class AdjusterHandleClaimType
    {
        public int ID { get; set; }
        public Nullable<int> AdjusterID { get; set; }
        public Nullable<int> PolicyTypeID { get; set; }
    
        public virtual AdjusterMaster AdjusterMaster { get; set; }
        public virtual LeadPolicyType LeadPolicyType { get; set; }
    }
}