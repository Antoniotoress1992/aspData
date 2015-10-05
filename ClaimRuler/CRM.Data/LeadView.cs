using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Data.Entities
{
	public class LeadView {
		public string AdjusterName { get; set; }

		public string AppraiserName { get; set; }
		
		public string BusinessName { get; set; }

		public int ClaimID {get; set; }

		public string ClaimantFirstName { get; set; }
		public string ClaimantLastName { get; set; }
		
		public string Coverage { get; set; }
		public string ClaimNumber { get; set; }

		public string ClaimWorkflowType { get; set; }

		public DateTime? ContractDate { get; set; }

		public string CityName { get; set; }
		public string ContractorName { get; set; }

		public string Email { get; set; }

		public string EventName { get; set; }
		
		public string EventType { get; set; }
		public string InsuranceCompanyName { get; set; }

		public string InsuredName { get; set; }

		public int LeadId { get; set; }

		public string LeadSourceName { get; set; }
		

		public DateTime?[] LossDates { get; set; }
		public DateTime? LossDate { get; set; }

		public string LossAddress { get; set; }

		public string MailingCity { get; set; }
		public string MailingState { get; set; }
		public string MailingZip { get; set; }

		public DateTime? OriginalLeadDate { get; set; }

		public DateTime? LastActivityDate { get; set; }
		public string PolicyType { get; set; }
		public int policyID { get; set; }

		public string ProducerName { get; set; }

		public int Severity { get; set; }
		public string SortClaimNumber { get; set; }
		public string StatusName { get; set; }
		public string StatusCodes { get; set; }
        public int status { get; set; }

		public string StateName { get; set; }

		public DateTime? SiteSurveyDate { get; set; }
		public string SubStatusName { get; set; }

		public string TypeofDamageText { get; set; }
		public string TypeOfProperty { get; set; }

		public string UmpireName { get; set; }

		public string UserName { get; set; }
        public string TypeOfLoss { get; set; }
		public string Zip { get; set; }

        public string ProgressDescription { get; set; }

        public string LocationName { get; set; }

        public string InsurerClaimNumber { get; set; }

	}
}
