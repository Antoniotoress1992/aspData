using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;
using CRM.Repository;
using CRM.RuleEngine;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Rules {
	public partial class ucRule8 : ucRuleBase, IRule {

		public string title {
			set { lblTitle.Text = value; }
		}

		protected override void Page_Load(object sender, EventArgs e) {

		}

		public void bindData(BusinessRule businessRule) {
			int clientID = SessionHelper.getClientId();
			List<Carrier> carriers = null;
			List<ExpenseType> expenseTypes = null;
		
			// load carriers
			carriers = CarrierManager.GetCarriers(clientID).ToList();
			CollectionManager.FillCollection(ddlCarrier, "CarrierID", "CarrierName", carriers);

			// load expenses
			using (ExpenseTypeManager repository = new ExpenseTypeManager()) {
				expenseTypes = repository.GetAll(clientID).ToList();
			}
			CollectionManager.FillCollection(ddlExpenseType, "ExpenseTypeID", "ExpenseName", expenseTypes);

			ViewState["businessRuleID"] = businessRule.BusinessRuleID.ToString();

			clearFields();

			if (businessRule.BusinessRuleID == 0) {
				// for new rules only.
				cbxActive.Checked = true;
			}
			else {
				// edit
				XElement ruleXML = XElement.Parse(businessRule.RuleXML);
				cbxActive.Checked = businessRule.IsActive;
                cbxEmailAdjuster.Checked = businessRule.EmailAdjuster;
                cbxEmailSupervisor.Checked = businessRule.EmailSupervisor;

				txtDescription.Text = businessRule.Description;

				using (RuleHelper ruleHelper = new RuleHelper()) {
					XElement conditionCarrier = ruleHelper.GetElement(ruleXML, "CarrierID");
					setValue(ddlCarrier, conditionCarrier.Element("value").Value);

					XElement conditionAdjuster = ruleHelper.GetElement(ruleXML, "ExpenseTypeID");
					setValue(ddlExpenseType, conditionAdjuster.Element("value").Value);
				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			BusinessRule rule = null;
			int businessRuleID = 0;

			Page.Validate("rule");
			if (!Page.IsValid)
				return;

			businessRuleID = Convert.ToInt32(ViewState["businessRuleID"]);
			lblMessage.Text = "";
			lblMessage.Visible = false;

			try {
				using (BusinessRuleManager repository = new BusinessRuleManager()) {

					if (businessRuleID == 0) {
						rule = new BusinessRule();
						rule.RuleID = (int)Globals.RuleType.SpecificExpenseTypePerCarrier;
						rule.ClientID = SessionHelper.getClientId();
					}
					else {
						rule = repository.GetBusinessRule(businessRuleID);
					}

					if (rule != null) {
						rule.IsActive = cbxActive.Checked;
                        rule.EmailAdjuster = cbxEmailAdjuster.Checked;
                        rule.EmailSupervisor = cbxEmailSupervisor.Checked;
						rule.Description = txtDescription.Text;

						rule.RuleXML = buildRule().ToString();

						rule = repository.Save(rule);

						ViewState["businessRuleID"] = rule.BusinessRuleID.ToString();

						lblMessage.Text = "Rule saved successfully.";
						lblMessage.CssClass = "ok";
						lblMessage.Visible = true;
					}
				}
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
				lblMessage.Text = "Rule not saved.";
				lblMessage.CssClass = "error";
				lblMessage.Visible = true;
			}

		}
		
		public XDocument buildRule() {
			//XDocument ruleXml = new XDocument(new XDeclaration("1.0", "UTF-8", "yes")); //create xml doc
			//XElement ruleNode = new XElement("rule");
			XDocument ruleXml = base.buildRootNode();

			XAttribute xaType = new XAttribute("type", "equal");
			XElement definitionNode = new XElement("definition", xaType);

			using (RuleHelper ruleHelper = new RuleHelper()) {
				// carrier condition
				XElement conditionCarrier = ruleHelper.CreateCondition("CarrierID", ddlCarrier.SelectedValue);

				// expense condition
				XElement conditionExpense = ruleHelper.CreateCondition("ExpenseTypeID", ddlExpenseType.SelectedValue);


				// add conditions to rule definition
				definitionNode.Add(conditionCarrier);
				definitionNode.Add(conditionExpense);
			}

			//ruleXml.Add(ruleNode);
			//ruleNode.Add(definitionNode);
			ruleXml.Element("rule").Add(definitionNode);

			return ruleXml;
		}

		public void clearFields() {
			txtDescription.Text = string.Empty;

			ddlCarrier.SelectedIndex = -1;
			ddlExpenseType.SelectedIndex = -1;

			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
		}
	}
}