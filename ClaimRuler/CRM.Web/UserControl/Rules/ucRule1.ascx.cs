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
	public partial class ucRule1 : ucRuleBase {

		protected override void Page_Load(object sender, EventArgs e) {

		}

		public void bindData(int businessRuleID) {
			int clientID = SessionHelper.getClientId();

			IQueryable<AdjusterMaster> adjusters = null;

			adjusters = AdjusterManager.GetAll(clientID);

			CollectionManager.FillCollection(ddlAdjuster, "AdjusterId", "adjusterName", adjusters);

			ViewState["businessRuleID"] = businessRuleID.ToString();

			
			if (businessRuleID == 0) {
				// for new rules only.
				cbxActive.Checked = true;
			}
			else {

			}
		}
		
		public void bindData(BusinessRule businessRule) {
			int clientID = SessionHelper.getClientId();			

			IQueryable<AdjusterMaster> adjusters = null;

			adjusters = AdjusterManager.GetAll(clientID);

            ddlAdjuster.DataSource = adjusters.ToList();
            ddlAdjuster.DataValueField = "AdjusterId";
            ddlAdjuster.DataTextField = "adjusterName";
            ddlAdjuster.DataBind();
            ddlAdjuster.Items.Insert(0, new ListItem("--- Select ---", "0"));

			//CollectionManager.FillCollection(ddlAdjuster, "AdjusterId", "adjusterName", adjusters);

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
					XElement invoiceCondition = ruleHelper.GetElement(ruleXML, "Claim");
					txtNumberOfClaims.Text = invoiceCondition.Element("value").Value;

					XElement adjusterCondition = ruleHelper.GetElement(ruleXML, "AdjusterID");
					ddlAdjuster.SelectedValue = adjusterCondition.Element("value").Value;
				}

				//.Where(x => x.Element("property").Value == "Invoice").FirstOrDefault();
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
						rule.RuleID = (int)Globals.RuleType.AdjusterClaimReview;
						rule.ClientID = SessionHelper.getClientId();
					}
					else {
						rule = repository.GetBusinessRule(businessRuleID);
					}

					if (rule != null) {
						rule.UpdateDate = DateTime.Now;

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

		
		private XDocument buildRule() {
			XDocument ruleXml = base.buildRootNode();

			XAttribute xaType = new XAttribute("type", "equal");
			XElement definitionNode = new XElement("definition", xaType);

			// invoice condition
			XElement conditionClaim = new XElement("condition");
			XElement propertyClaim = new XElement("property", "Claim");
			conditionClaim.Add(propertyClaim);

			// invoice condition value
			XElement claimValue = new XElement("value", txtNumberOfClaims.Text);
			conditionClaim.Add(claimValue);

			// adjuster condition
			XElement conditionAdjuster = new XElement("condition");
			//XAttribute xaAdjuster = new XAttribute("name", "AdjusterID");
			XElement propertyAdjuster = new XElement("property", "AdjusterID");
			conditionAdjuster.Add(propertyAdjuster);

			// adjuster condition value
			XElement adjusterValue = new XElement("value", ddlAdjuster.SelectedValue);
			conditionAdjuster.Add(adjusterValue);

			// add conditions to rule definition
			definitionNode.Add(conditionClaim);
			definitionNode.Add(conditionAdjuster);

			//ruleXml.Add(ruleNode);
			ruleXml.Element("rule").Add(definitionNode);

			return ruleXml;
		}

		private void clearFields() {
			txtDescription.Text = string.Empty;

			txtNumberOfClaims.Value = 0;

			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
		}
	}
}