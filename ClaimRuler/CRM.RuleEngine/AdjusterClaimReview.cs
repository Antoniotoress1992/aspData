using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Linq;

using CRM.Data;
using CRM.Data.Account;

using CRM.Repository;
using CRM.Data.Entities;

namespace CRM.RuleEngine {
	public class AdjusterClaimReview : RuleBase {
		//string[] properties = { "AdjusterID" };
		List<BusinessRule> rules = null;
		string[] values = null;

		public AdjusterClaimReview() {

		}

		public RuleException TestRule(int clientID, Claim claim) {
			string claimLimit = null;
			bool isRuleMet = false;
			int numberOfClaims = 0;
			int numberOfClaimsLimit = 0;

			// skip if no adjuster assigned to claim
			if ((claim.AdjusterID ?? 0) == 0)
				return null;

			// get number of claims assigned to adjuster
			numberOfClaims = ClaimsManager.getAdjusterClaimCount((int)claim.AdjusterID);

			// get business rules for client/rule type id
			using (BusinessRuleManager repository = new BusinessRuleManager()) {
				rules = repository.GetBusinessRules(clientID, Globals.RuleType.AdjusterClaimReview);
			}

			if (rules != null && rules.Count > 0) {
				// build value array
				values = new string[] { claim.AdjusterID.ToString() };

				foreach (BusinessRule rule in rules) {
					XElement ruleXML = XElement.Parse(rule.RuleXML);

					XElement adjusterCondition = base.GetElement(ruleXML, "AdjusterID", claim.AdjusterID.ToString());

					claimLimit = base.GetElementValue(ruleXML, "Claim");

					if (adjusterCondition != null && !string.IsNullOrEmpty(claimLimit)) {
						if (int.TryParse(claimLimit, out numberOfClaimsLimit) && numberOfClaimsLimit > 0) {

							isRuleMet = (numberOfClaims <= numberOfClaimsLimit);
						}
					}


					if (isRuleMet) {
						// add exception to queue
						ruleException = new RuleException();

						ruleException.BusinessRuleID = rule.BusinessRuleID;

						ruleException.ClientID = clientID;

						ruleException.ObjectID = claim.ClaimID;

						ruleException.ObjectTypeID = (int)Globals.ObjectType.Claim;

						break;
					}
				}
			}

			return this.ruleException;
		}
	}
}
