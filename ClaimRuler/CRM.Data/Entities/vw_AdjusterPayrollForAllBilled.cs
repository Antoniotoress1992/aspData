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
    
    public partial class vw_AdjusterPayrollForAllBilled
    {
        public string AdjusterName { get; set; }
        public Nullable<System.DateTime> ServiceDate { get; set; }
        public Nullable<int> InvoiceNumber { get; set; }
        public string InsuredName { get; set; }
        public string AdjusterClaimNumber { get; set; }
        public Nullable<decimal> ServiceFee { get; set; }
        public decimal Non_Payable { get; set; }
        public decimal Payable { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string BranchCode { get; set; }
        public Nullable<System.DateTime> ExpenseDate { get; set; }
        public long RowID { get; set; }
        public Nullable<int> ClientID { get; set; }
        public int AdjusterId { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<bool> isBillable { get; set; }
    }
}
