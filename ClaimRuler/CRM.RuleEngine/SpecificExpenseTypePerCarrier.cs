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
	public class SpecificExpenseTypePerCarrier : RuleBase {
		string[] properties = { "CarrierID", "ExpenseTypeID" };
		List<BusinessRule> rules = null;
		string[] values = null;
		

		public RuleException TestRule(int clientID, Claim claim, int expenseTypeID) {
			int carrierID = 0;
			bool isRuleMet = false;
			
			
			// get carrier associated with claim/policy
			using (ClaimManager repository = new ClaimManager()) {
				carrierID = repository.GetCarrier(claim.ClaimID);
			}

			// get business rules for client/rule type id
			using (BusinessRuleManager repository = new BusinessRuleManager()) {
				rules = repository.GetBusinessRules(clientID, Globals.RuleType.SpecificExpenseTypePerCarrier);
			}

			if (carrierID > 0 && expenseTypeID > 0 && rules != null && rules.Count > 0) {
				// build value array
				values = new string[] { carrierID.ToString(), expenseTypeID.ToString() };

				foreach (BusinessRule rule in rules) {
					XElement ruleXML = XElement.Parse(rule.RuleXML);

					isRuleMet = base.TestRule(ruleXML, properties, values);

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

			return ruleException;
		}

		//public RuleException TestRule(int clientID, Invoice invoice, int expenseTypeID) {
		//	int carrierID = 0;
		//	bool isRuleMet = false;
		//	List<BusinessRule> rules = null;
		//	string[] properties = { "CarrierID", "ExpenseTypeID" };
		//	string[] values = null;
		//	RuleException ruleException = null;			

		//	// get carrier associated with claim/policy
		//	using (ClaimManager repository = new ClaimManager()) {
		//		carrierID = repository.GetCarrier(invoice.ClaimID);
		//	}

		//	// get business rules for client/rule type id
		//	using (BusinessRuleManager repository = new BusinessRuleManager()) {
		//		rules = repository.GetBusinessRules(clientID, Globals.RuleType.SpecificExpenseTypePerCarrier);
		//	}

		//	if (carrierID > 0 && expenseTypeID > 0 && rules != null && rules.Count > 0) {
		//		// build value array
		//		values = new string[] { carrierID.ToString(), expenseTypeID.ToString() };

		//		foreach (BusinessRule rule in rules) {
		//			XElement ruleXML = XElement.Parse(rule.RuleXML);

		//			isRuleMet = base.TestRule(ruleXML, properties, values);

		//			if (isRuleMet) {
		//				// add exception to queue
		//				ruleException = new RuleException();

		//				ruleException.BusinessRuleID = rule.BusinessRuleID;

		//				ruleException.ClientID = clientID;

		//				ruleException.ObjectID = invoice.InvoiceID;

		//				ruleException.ObjectTypeID = (int)Globals.ObjectType.Invoice;

		//				break;
		//			}
		//		}
		//	}

		//	return ruleException;
		//}

		public RuleException TestRule(int clientID, Invoice invoice) {
			int carrierID = 0;
			bool isRuleMet = false;
			List<int> expenseTypeIDCollection = null;

			// get carrier associated with claim/policy
			using (ClaimManager repository = new ClaimManager()) {
				carrierID = repository.GetCarrier(invoice.ClaimID);
			}

			// get business rules for client of type "SpecificExpenseTypePerCarrier"
			using (BusinessRuleManager repository = new BusinessRuleManager()) {
				rules = repository.GetBusinessRules(clientID, Globals.RuleType.SpecificExpenseTypePerCarrier);
			}

			expenseTypeIDCollection = InvoiceDetailManager.GetInvoiceExpenseTypeIDCollection(invoice.InvoiceID);

			if (carrierID > 0 && rules != null && rules.Count > 0 && expenseTypeIDCollection != null && expenseTypeIDCollection.Count > 0) {

				foreach (int expenseTypeID in expenseTypeIDCollection) {
					// build value array
					values = new string[] { carrierID.ToString(), expenseTypeID.ToString() };

					foreach (BusinessRule rule in rules) {
						XElement ruleXML = XElement.Parse(rule.RuleXML);

						isRuleMet = base.TestRule(ruleXML, properties, values);

						if (isRuleMet) {
							// add exception to queue
							ruleException = new RuleException();

							ruleException.BusinessRuleID = rule.BusinessRuleID;

							ruleException.ClientID = clientID;

							ruleException.ObjectID = invoice.InvoiceID;

							ruleException.ObjectTypeID = (int)Globals.ObjectType.Invoice;

							break;
						}
					}
				}
			}

			return ruleException;
		}		
	}
}
