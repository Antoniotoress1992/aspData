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
    
    public partial class Invitee
    {
        public int InviteeID { get; set; }
        public Nullable<int> TaskID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> ContactID { get; set; }
        public Nullable<int> LeadID { get; set; }
    
        public virtual Contact Contact { get; set; }
        public virtual Leads Leads { get; set; }
        public virtual SecUser SecUser { get; set; }
        public virtual Task Task { get; set; }
    }
}
