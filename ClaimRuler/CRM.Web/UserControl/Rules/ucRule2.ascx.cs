﻿using System;
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
using System.Configuration;
using System.Text;
using CRM.Web.Protected.Admin;

using System.Web.Security;

using LinqKit;

using CRM.Web.UserControl.Admin;

using System.Data.SqlClient;

using System.Data;





namespace CRM.Web.UserControl.Rules
{
    public partial class ucRule2 : ucRuleBase, IRule
    {
        List<Carrier> carriers = null;
        int ddlCarrierId = 0;
        string connectionString = ConfigurationManager.ConnectionStrings["ClaimRuler"].ConnectionString;
        public string title
        {
            set { lblTitle.Text = value; }
        }

        protected override void Page_Load(object sender, EventArgs e)

        {
            int clientID = SessionHelper.getClientId();
        

        }

        public void bindData(BusinessRule businessRule)
        {
            int clientID = SessionHelper.getClientId();

            ViewState["businessRuleID"] = businessRule.BusinessRuleID.ToString();

            clearFields();

            if (businessRule.BusinessRuleID == 0)
            {
                // for new rules only.
                cbxActive.Checked = true;
                carriers = CarrierManager.GetCarriers(clientID).ToList();
                CollectionManager.FillCollection(ddlCarrier, "CarrierID", "CarrierName", carriers);
            }
            else
            {
                // edit
                XElement ruleXML = XElement.Parse(businessRule.RuleXML);
                cbxActive.Checked = businessRule.IsActive;
                cbxEmailAdjuster.Checked = businessRule.EmailAdjuster;
                cbxEmailSupervisor.Checked = businessRule.EmailSupervisor;

                txtDescription.Text = businessRule.Description;
                
                carriers = CarrierManager.GetCarriers(clientID).ToList();
                CollectionManager.FillCollection(ddlCarrier, "CarrierID", "CarrierName", carriers);

                string str_query = @"SELECT * FROM BusinessRule WHERE BusinessRuleID = @BusinessRuleId 
                                       ";
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(str_query, conn))
                {
                    cmd.Parameters.AddWithValue("@BusinessRuleId", businessRule.BusinessRuleID);

                    
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        BusinessRule businessRuleObj = new BusinessRule();

                        businessRuleObj.BusinessRuleID = Convert.ToInt32(reader["BusinessRuleID"]);
                        if (reader["CarrierId"] == DBNull.Value)
                        {
                            ddlCarrierId = 0;
                        }
                        else {
                            ddlCarrierId = Convert.ToInt32(reader["CarrierId"]);
                        }
                       
                       
                    }

                    conn.Close();
                }
                ddlCarrier.SelectedValue = Convert.ToString(ddlCarrierId);
                
                using (RuleHelper ruleHelper = new RuleHelper())
                {
                    XElement conditionLapseTime = ruleHelper.GetElement(ruleXML, "LapseTime");
                    txtDayHours.Value = conditionLapseTime.Element("value").Value;

                    XElement conditionLapseTimeType = ruleHelper.GetElement(ruleXML, "LapseTimeType");
                    setValue(this.ddlhourday, conditionLapseTimeType.Element("value").Value);
                }
            }
        }
       
        protected void btnSave_Click(object sender, EventArgs e)
        {
            BusinessRule rule = null;
            int businessRuleID = 0;

            Page.Validate("rule");
            if (!Page.IsValid)
                return;

            businessRuleID = Convert.ToInt32(ViewState["businessRuleID"]);
            lblMessage.Text = "";
            lblMessage.Visible = false;

            try
            {
                using (BusinessRuleManager repository = new BusinessRuleManager())
                {

                    if (businessRuleID == 0)
                    {
                        rule = new BusinessRule();
                        BusinessRules businessRulesObj = new BusinessRules();
                        rule.RuleID = Convert.ToInt32(Session["ruleId"]);
                        rule.ClientID = SessionHelper.getClientId();
                        rule.CarrierId = Convert.ToInt32(this.ddlCarrier.SelectedValue);
                    }
                    else
                    {
                        rule = repository.GetBusinessRule(businessRuleID);
                    }

                    if (rule != null)
                    {
                        rule.IsActive = cbxActive.Checked;
                        rule.EmailAdjuster = cbxEmailAdjuster.Checked;
                        rule.EmailSupervisor = cbxEmailSupervisor.Checked;
                        rule.CarrierId = Convert.ToInt32(this.ddlCarrier.SelectedValue);
                        rule.Description = txtDescription.Text;
                        ViewState["businessRuleID"] = rule.BusinessRuleID.ToString();
                        rule.RuleXML = buildRule().ToString();
                        rule = repository.Save(rule);
                        string str_query = @"update BusinessRule set 
                                            CarrierId = @CarrierId
                                            
                                    where
                                        
                                        BusinessRuleID=@BusinessRuleID      
                                ";

                        using (SqlConnection conn = new SqlConnection(connectionString))
                        using (SqlCommand cmd = new SqlCommand(str_query, conn))
                        {

                            cmd.Parameters.AddWithValue("@BusinessRuleID ", rule.BusinessRuleID);
                            cmd.Parameters.AddWithValue("@CarrierId", rule.CarrierId);

                            conn.Open();

                            cmd.ExecuteNonQuery();
                            conn.Close();
                            conn.Dispose();

                            conn.Close();
                        }
                       

                        lblMessage.Text = "Rule saved successfully.";
                        // notifyUser(txtDescription.Text, SessionHelper.getClientId());
                        lblMessage.CssClass = "ok";
                        lblMessage.Visible = true;
                    }
                }

                Login login = new Login();
                login.setGlobalSession();
                login.formatException();
                login.setRulexception();
            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
                lblMessage.Text = "Rule not saved.";
                lblMessage.CssClass = "error";
                lblMessage.Visible = true;
            }

        }

        public XDocument buildRule()
        {
            XDocument ruleXml = base.buildRootNode();

            XAttribute xaType = new XAttribute("type", "equal");
            XElement definitionNode = new XElement("definition", xaType);

            using (RuleHelper ruleHelper = new RuleHelper())
            {

                // hours/days unit
                XElement conditionLapseTime = ruleHelper.CreateCondition("LapseTime", txtDayHours.Text);

                // hour/day selection
                XElement conditionLapseTimeType = ruleHelper.CreateCondition("LapseTimeType", this.ddlhourday.SelectedValue);


                // add conditions to rule definition
                definitionNode.Add(conditionLapseTime);
                definitionNode.Add(conditionLapseTimeType);

                //ruleXml.Add(ruleNode);
                ruleXml.Element("rule").Add(definitionNode);
            }


            return ruleXml;
        }

        public void clearFields()
        {
            txtDescription.Text = string.Empty;

            txtDayHours.Value = 0;

            lblMessage.Text = string.Empty;
            lblMessage.Visible = false;
        }



    }
}