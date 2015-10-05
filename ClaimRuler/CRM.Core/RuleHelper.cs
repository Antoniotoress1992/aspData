using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using CRM.Data;
using CRM.Repository;

namespace CRM.Core {
	static public class RuleHelper {

		static public XElement getElement(XElement xmlRule, string elementName) {
			return (from XElement r in xmlRule.Descendants("condition")
				   where r.Element("property").Value == elementName
				   select r
					).FirstOrDefault();
		}

		static public XElement createCondition(string propertyName, string value) {
			XElement condition = new XElement("condition");
			XElement property = new XElement("property", propertyName);
			condition.Add(property);

			XElement propertyValue = new XElement("value", value);
			condition.Add(propertyValue);

			return condition;
		}

		static int getConditionCount(XElement xmlRule) {
			return (
					from XElement r in xmlRule.Descendants("condition")
					select r
				   ).Count();
		}

		static bool testRule(XElement xmlRule, string[] properties, string[] values) {
			bool isRuleMet = false;

			int conditionsToMeet = getConditionCount(xmlRule);

			if (conditionsToMeet > 0) {
				int conditionsMet = (
								from XElement r in xmlRule.Descendants("condition")
								where properties.Contains(r.Element("property").Value) && values.Contains(r.Element("value").Value)
								select r
							).Count();

				isRuleMet = conditionsToMeet.Equals(conditionsMet);
			}

			return isRuleMet;
		}


		/// <summary>
		/// Rule ID 8
		/// </summary>
		/// <param name="claimID"></param>
		/// <param name="expenseID"></param>
		/// <returns></returns>
		static public bool specificExpenseTypePerCarrier(int clientID, int claimID, int expenseID) {
			int carrierID = 0;
			bool isRuleMet = false;
			List<BusinessRule> rules = null;
			string[] properties = {"CarrierID", "ExpenseTypeID"};
			string[] values = null;

			// get carrier associated with claim/policy
			using (ClaimManager repository = new ClaimManager()) {
				carrierID = repository.GetCarrier(claimID);
			}

			if (carrierID > 0 && rules != null && rules.Count > 0) {
				// build value array
				values = new string[] { carrierID.ToString(), expenseID.ToString()};

				foreach (BusinessRule rule in rules) {
					XElement ruleXML = XElement.Parse(rule.RuleXML);

					isRuleMet = testRule(ruleXML, properties, values);

					if (isRuleMet) {
						// add exception to queue
					}
				}
			}

			return isRuleMet;
		}

		
	}
}
