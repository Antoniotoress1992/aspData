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
    
    public partial class LeadPolicyDamageType
    {
        public int ID { get; set; }
        public int PolicyID { get; set; }
        public int TypeOfDamageId { get; set; }
    
        public virtual LeadPolicy LeadPolicy { get; set; }
        public virtual TypeOfDamageMaster TypeOfDamageMaster { get; set; }
    }
}
