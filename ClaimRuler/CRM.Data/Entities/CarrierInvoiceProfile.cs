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
    
    public partial class CarrierInvoiceProfile
    {
        public CarrierInvoiceProfile()
        {
            this.CarrierInvoiceProfileFeeItemized = new HashSet<CarrierInvoiceProfileFeeItemized>();
            this.CarrierInvoiceProfileFeeProvision = new HashSet<CarrierInvoiceProfileFeeProvision>();
            this.CarrierInvoiceProfileFeeSchedule = new HashSet<CarrierInvoiceProfileFeeSchedule>();
            this.Invoice = new HashSet<Invoice>();
            this.LeadInvoice = new HashSet<LeadInvoice>();
        }
    
        public int CarrierInvoiceProfileID { get; set; }
        public int CarrierID { get; set; }
        public Nullable<int> InvoiceFeeProfileID { get; set; }
        public string ProfileName { get; set; }
        public Nullable<System.DateTime> EffiectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public string CoverageArea { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CarrierInvoiceProfileTypeID { get; set; }
        public Nullable<int> InvoiceType { get; set; }
        public string AccountingContact { get; set; }
        public string AccountingContactEmail { get; set; }
        public Nullable<decimal> FirmDiscountPercentage { get; set; }
        public Nullable<decimal> FlatCatPercent { get; set; }
        public Nullable<decimal> FlatCatFee { get; set; }
        public Nullable<bool> BillTNE { get; set; }
    
        public virtual Carrier Carrier { get; set; }
        public virtual CarrierInvoiceType CarrierInvoiceType { get; set; }
        public virtual InvoiceFeeProfile InvoiceFeeProfile { get; set; }
        public virtual ICollection<CarrierInvoiceProfileFeeItemized> CarrierInvoiceProfileFeeItemized { get; set; }
        public virtual ICollection<CarrierInvoiceProfileFeeProvision> CarrierInvoiceProfileFeeProvision { get; set; }
        public virtual ICollection<CarrierInvoiceProfileFeeSchedule> CarrierInvoiceProfileFeeSchedule { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<LeadInvoice> LeadInvoice { get; set; }
    }
}
