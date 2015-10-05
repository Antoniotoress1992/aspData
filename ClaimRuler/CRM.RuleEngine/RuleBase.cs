using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using CRM.Data;
using CRM.Repository;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.RuleEngine {
	public class RuleBase : IDisposable {
		public RuleException ruleException = null;

		private bool disposed = false;

		public int clientID { get; set; }
		public int claimID { get; set; }

		public XElement GetElement(XElement xmlRule, string elementName) {
			return (from XElement r in xmlRule.Descendants("condition")
				   where r.Element("property").Value == elementName
				   select r
					).FirstOrDefault();
		}

		public XElement GetElement(XElement xmlRule, string propertyName, string value) {
			return (from XElement r in xmlRule.Descendants("condition")
				   where r.Element("property").Value == propertyName && r.Element("value").Value == value
				   select r
				   ).FirstOrDefault();
		}


		public string GetElementValue(XElement xmlRule, string elementName) {
			return (from XElement r in xmlRule.Descendants("condition")
				   where r.Element("property").Value == elementName
				   select r.Element("value").Value
					).FirstOrDefault();
		}


		public XElement CreateCondition(string propertyName, string value) {
			XElement condition = new XElement("condition");
			XElement property = new XElement("property", propertyName);
			condition.Add(property);

			XElement propertyValue = new XElement("value", value);
			condition.Add(propertyValue);

			return condition;
		}


		public int GetConditionCount(XElement xmlRule) {
			return (
					from XElement r in xmlRule.Descendants("condition")
					select r
				   ).Count();
		}

		public virtual bool TestRule(XElement xmlRule, string[] properties, string[] values) {
			bool isRuleMet = false;

			int conditionsToMeet = GetConditionCount(xmlRule);

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

		public void AddException(RuleException ruleException) {
			using (RuleExceptionManager repository = new RuleExceptionManager()) {
				repository.Save(ruleException);
			}
		}

		public bool ExceptionExists(int clientID, int businessRuleID, int objectID, int objectTypeID) {
			bool exists = false;

			using (RuleExceptionManager repository = new RuleExceptionManager()) {
				exists = repository.CheckForException(clientID, businessRuleID, objectID, objectTypeID);
			}

			return exists;
		}

		#region ===== memory management =====

		public void Dispose() {
			// Perform any object clean up here.

			// If you are inheriting from another class that
			// also implements IDisposable, don't forget to
			// call base.Dispose() as well.
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing) {
			if (!disposed) {
				if (disposing) {
					ruleException = null;
				}

				disposed = true;
			}
		}
		#endregion
	}
}
