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
    
    public partial class ClaimComment
    {
        public int CommentID { get; set; }
        public int ClaimID { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CommentDate { get; set; }
        public string ActivityType { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string InternalComments { get; set; }
    
        public virtual Claim Claim { get; set; }
        public virtual SecUser SecUser { get; set; }
    }
}
