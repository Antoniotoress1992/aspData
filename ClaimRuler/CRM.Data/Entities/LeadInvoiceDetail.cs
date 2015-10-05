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
    
    public partial class LeadInvoiceDetail
    {
        public int InvoiceLineID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public string LineDescription { get; set; }
        public Nullable<decimal> LineAmount { get; set; }
        public Nullable<int> LineItemNo { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<bool> isBillable { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public string UnitDescription { get; set; }
        public Nullable<System.DateTime> LineDate { get; set; }
        public Nullable<int> ServiceTypeID { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string Comments { get; set; }
    
        public virtual InvoiceServiceType InvoiceServiceType { get; set; }
        public virtual LeadInvoice LeadInvoice { get; set; }
    }
}
