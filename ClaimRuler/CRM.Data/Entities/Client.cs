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
    
    public partial class Client
    {
        public Client()
        {
            this.AdjusterMaster = new HashSet<AdjusterMaster>();
            this.BusinessRule = new HashSet<BusinessRule>();
            this.SecUser1 = new HashSet<SecUser>();
            this.Leads = new HashSet<Leads>();
            this.RuleException = new HashSet<RuleException>();
            this.SecRoleInvoiceApprovalPermission = new HashSet<SecRoleInvoiceApprovalPermission>();
        }
    
        public int ClientId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CityId { get; set; }
        public string ZipCode { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string PrimaryPhoneNo { get; set; }
        public string PrimaryEmailId { get; set; }
        public string SecondaryPhoneNo { get; set; }
        public string SecondaryEmailId { get; set; }
        public string EmailHost { get; set; }
        public string EmailHostPort { get; set; }
        public Nullable<int> Active { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> InsertDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public Nullable<int> maxUsers { get; set; }
        public string ClientName { get; set; }
        public Nullable<decimal> FeePerContract { get; set; }
        public string FederalIDNo { get; set; }
        public Nullable<bool> isTrial { get; set; }
        public Nullable<int> MaxLeads { get; set; }
        public Nullable<bool> isShowTasks { get; set; }
        public Nullable<int> InactivityDays { get; set; }
        public string imapHost { get; set; }
        public Nullable<int> imapHostPort { get; set; }
        public Nullable<bool> imapHostUseSSL { get; set; }
        public Nullable<int> InvoiceSettingID { get; set; }
        public Nullable<decimal> InvoiceContingencyFee { get; set; }
        public Nullable<int> NextClaimNumber { get; set; }
        public Nullable<int> ClientTypeID { get; set; }
        public Nullable<int> InvoicePaymentTerms { get; set; }
    
        public virtual ICollection<AdjusterMaster> AdjusterMaster { get; set; }
        public virtual ICollection<BusinessRule> BusinessRule { get; set; }
        public virtual CityMaster CityMaster { get; set; }
        public virtual SecUser SecUser { get; set; }
        public virtual StateMaster StateMaster { get; set; }
        public virtual ICollection<SecUser> SecUser1 { get; set; }
        public virtual ICollection<Leads> Leads { get; set; }
        public virtual ICollection<RuleException> RuleException { get; set; }
        public virtual ICollection<SecRoleInvoiceApprovalPermission> SecRoleInvoiceApprovalPermission { get; set; }
    }
}
