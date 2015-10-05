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
    
    public partial class vw_Lead_Search
    {
        public Nullable<int> ClientID { get; set; }
        public int LeadId { get; set; }
        public Nullable<System.DateTime> OriginalLeadDate { get; set; }
        public string InsuredName { get; set; }
        public string ClaimantFirstName { get; set; }
        public string ClaimantLastName { get; set; }
        public string BusinessName { get; set; }
        public string MailingAddress { get; set; }
        public Nullable<int> Status { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string Zip { get; set; }
        public string LossAddress { get; set; }
        public string LossAddress2 { get; set; }
        public Nullable<System.DateTime> SiteSurveyDate { get; set; }
        public Nullable<System.DateTime> LastActivityDate { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MailingZip { get; set; }
        public string LeadSourceName { get; set; }
        public Nullable<System.DateTime> LossDate { get; set; }
        public string AdjusterClaimNumber { get; set; }
        public Nullable<int> SeverityNumber { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public string ClaimWorkflowType { get; set; }
        public string CauseOfLoss { get; set; }
        public int ClaimStatusID { get; set; }
        public int ClaimSubStatusID { get; set; }
        public Nullable<int> policyID { get; set; }
        public string PolicyNumber { get; set; }
        public Nullable<int> policyTypeID { get; set; }
        public string Coverage { get; set; }
        public string InsuranceCompanyName { get; set; }
        public Nullable<int> CarrierID { get; set; }
        public string StatusName { get; set; }
        public string SubStatusName { get; set; }
        public string AdjusterName { get; set; }
        public Nullable<int> AdjusterId { get; set; }
        public Nullable<int> ContractorId { get; set; }
        public string ContractorName { get; set; }
        public Nullable<int> AppraiserId { get; set; }
        public string AppraiserName { get; set; }
        public string UmpireName { get; set; }
        public Nullable<int> ProducerId { get; set; }
        public string ProducerName { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public Nullable<int> ClaimID { get; set; }
        public string ProgressDescription { get; set; }
        public string LocationName { get; set; }
        public string InsurerClaimNumber { get; set; }
    }
}