using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

using LinqKit;
using System.Linq.Expressions;

using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;
using CRM.Data.Entities;

namespace CRM.RuleEngine {
	public class ClaimAssignmentReview : RuleBase {
		
		public List<Claim> TestRule(BusinessRule rule) {
			//List<BusinessRule> rules = null;
			List<Claim> claims = null;
			int lapsedTime = 0;
			DateTime now = DateTime.Now;

			//// get business rules for client/rule type id
			//using (BusinessRuleManager repository = new BusinessRuleManager()) {
			//	rules = repository.GetBusinessRules(Globals.RuleType.ClaimAssingmentReview);
			//}


			XElement ruleXML = XElement.Parse(rule.RuleXML);

			// get lapse time units: number of hours or days
			int.TryParse(GetElementValue(ruleXML, "LapseTime"), out lapsedTime);

			// get lapse time type: hours or days
			string lapseTimeType = GetElementValue(ruleXML, "LapseTimeType");

			Expression<Func<Claim, bool>> predicate = null;

			// query filters
			predicate = PredicateBuilder.True<CRM.Data.Entities.Claim>();
			//predicate = predicate.And(x => x.LeadPolicy.Lead.ClientID == clientID);			// claims for this client only
			predicate = predicate.And(x => x.IsActive == true);							// active claims
			predicate = predicate.And(x => x.LastStatusUpdate != null);
			predicate = predicate.And(x => x.ProgressStatusID == (int)Globals.ClaimProgressStatus.ClaimAssignedNotAcceptedYet);

			// determine time lapsed
			if (lapseTimeType == "1") {		// hours
				predicate = predicate.And(x => EntityFunctions.DiffHours(x.LastProgressChanged, now) > lapsedTime);
			}
			else if (lapseTimeType == "2") {	// days
				predicate = predicate.And(x => EntityFunctions.DiffDays(x.LastProgressChanged, now) > lapsedTime);
			}

			using (ClaimManager repository = new ClaimManager()) {
				claims = repository.SearchClaim(predicate);				
			}



			return claims;
		}
	}
}
