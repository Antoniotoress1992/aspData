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
    
    public partial class EarningCode
    {
        public int EarningCodeID { get; set; }
        public int ClientID { get; set; }
        public string Code { get; set; }
        public string CodeDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
