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
    
    public partial class ProgressStatus
    {
        public int ProgressStatusID { get; set; }
        public string ProgressDescription { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> SortOrder { get; set; }
    }
}