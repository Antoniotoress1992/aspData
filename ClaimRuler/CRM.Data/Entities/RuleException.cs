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
    
    public partial class RuleException
    {
        public int RuleExceptionID { get; set; }
        public Nullable<int> BusinessRuleID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public System.DateTime ExceptionDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> ObjectID { get; set; }
        public Nullable<int> ObjectTypeID { get; set; }
        public Nullable<int> UserID { get; set; }
    
        public virtual BusinessRule BusinessRule { get; set; }
        public virtual Client Client { get; set; }
        public virtual RuleObject RuleObject { get; set; }
        public virtual SecUser SecUser { get; set; }
    }
}