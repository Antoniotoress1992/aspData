using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using Infragistics.Web.UI.EditorControls;
using Infragistics.Web.UI.ListControls;
using CRM.Data.Entities;
using CRM.Repository;
using System.Transactions;
using CRM.Core;

namespace CRM.Web.UserControl.Admin {
	public partial class ucPolicyPropertyLimits : System.Web.UI.UserControl {
		


		protected void Page_Load(object sender, EventArgs e) 
        {
            //if(!IsPostBack)
            //{
            //    disableNewRow();
            //}
            
		}

		public void bindData(int policyID) {
			List<PolicyLimit> limits = null;
            
            if (HttpContext.Current.Session["Limit"] != null && HttpContext.Current.Session["PolicyLimit"] != null && HttpContext.Current.Session["tblCasulityPolicylimit"] != null && HttpContext.Current.Session["tblAllPolicylimit"] != null)
            {
                DataTable Limit = HttpContext.Current.Session["Limit"] as DataTable;
                DataTable PolicyLimit = HttpContext.Current.Session["PolicyLimit"] as DataTable;

                var results = from limit in Limit.AsEnumerable()
                              join policylimit in PolicyLimit.AsEnumerable() on (int)limit["LimitID"] equals (int)policylimit["LimitID"]
                              select new
                              {
                                  LimitLetter = (string)limit["LimitLetter"],
                                  LimitDescription = (string)limit["LimitDescription"],
                                  LimitID = (int)limit["LimitID"],
                                  LimitAmount = (decimal)policylimit["LimitAmount"],
                                  LimitDeductible = (decimal)policylimit["LimitDeductible"],
                                  CATDeductible = policylimit["CATDeductible"],
                                  ConInsuranceLimit = (decimal)policylimit["ConInsuranceLimit"],
                                  ApplyTo = (string)policylimit["ApplyTo"],
                                  ITV = (decimal)policylimit["ITV"],
                                  Reserve = (decimal)policylimit["Reserve"],                                  
                              };


                gvLimits3.DataSource = results;
                gvLimits3.DataBind();

                if (PolicyLimit.Rows.Count <= 0)
                {
                    lblNoRecordFound.Visible = true;
                }
                else
                {
                    lblNoRecordFound.Visible = false;
                }


            }
            else
            {
                limits = PolicyLimitManager.GetAll(policyID, LimitType.LIMIT_TYPE_PROPERTY);
                bool isStatic = PolicyLimitManager.LimitIsStatic(policyID);

                //gvLimits.DataSource = limits;
                //gvLimits.DataBind();

                if (limits.Count > 0)
                {

                    if (isStatic)
                    {
                        
                        gvLimits.DataSource = limits;
                        gvLimits.DataBind();
                        disableNewRow();
                        gvLimits2.DataSource = null;
                        gvLimits2.DataBind();
                    }
                    else
                    {
                       
                        gvLimits.DataSource = null;
                        gvLimits.DataBind();
                        gvLimits2.DataSource = limits;
                        gvLimits2.DataBind();
                    }
                   
                }
                else
                {
                    lblNoRecordFound.Visible = true;
                }
                if (limits != null && limits.Count == 0 && policyID == 0)
                {
                   
                    limits = primePropertyLimits();
                    gvLimits.DataSource = limits;
                    gvLimits.DataBind();

                    gvLimits2.DataSource = null;
                    gvLimits2.DataBind();
                    lblNoRecordFound.Visible = false;
                }                

                gvLimits3.DataSource = null;
                gvLimits3.DataBind();

            }


            

		}

		
		private List<PolicyLimit> primePropertyLimits() {
			List<PolicyLimit> propertyLimits = null;
			List<Limit> limits = null;

			limits = LimitManager.GetAll(LimitType.LIMIT_TYPE_PROPERTY);

			propertyLimits = (from x in limits
						 select new PolicyLimit {
							 LimitID = x.LimitID,							
							 Limit = x
						 }).ToList<PolicyLimit>();

			return propertyLimits;
		}

		public void saveLimits(int policyID) {
			int policyLimitID = 0;
			int limitID = 0;
            //int claimLimitID = 0;
            int myClaimID = 0;
            int myClaimLimitID = 0;
            int myLimitId = 0; //anything prefaced with "my" is new; OC
            myClaimID = SessionHelper.getClaimID(); //Convert.ToInt32(Session["ClaimID"]);
			PolicyLimit limit = null;
            PolicyLimit myPolicyLimit = null;
            Limit myLimit = null;
           // ClaimLimit myClaimLimit = null;
            //added new; OC 9/11/2014: put in place to add new row to the policy limits grid. took the old 3 column one out.
            //crete the footer controls available for use here :OC 9/12/14
            WebTextEditor txtMyCoverage = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyCoverage");
            WebTextEditor txtMyDescription = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyDescription");
            WebNumericEditor txtMyLimit = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyLimit");
            WebNumericEditor txtMyDeductible = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyDeductible");
            WebTextEditor txtMyCATDeductible = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyCATDeductible");
            WebDropDown ddlMySettlementType = (WebDropDown)gvLimits.FooterRow.FindControl("ddlMySettlementType");
            WebPercentEditor txtMyCoInsuranceLimit = (WebPercentEditor)gvLimits.FooterRow.FindControl("txtMyCoInsuranceLimit");
            WebPercentEditor txtMyITV = (WebPercentEditor)gvLimits.FooterRow.FindControl("txtMyITV");
            WebNumericEditor txtMyReserve = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyReserve");
            WebTextEditor txtMyWHDeductible = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyWHDeductible");
            WebNumericEditor txtMyLossAmountACV = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyLossAmountACV");
            WebNumericEditor txtMyLossAmountRCV = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyLossAmountRCV");
            WebNumericEditor txtMyOverage = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyOverage");

            if (txtMyCoverage.Text != "")//TODO: change this condition to something more stable (checkbox or something)
            {
                //LIMIT TABLE STUFF
                myLimit = new Limit();
                myLimit.LimitLetter = txtMyCoverage.Text;
                myLimit.LimitType = 1;//need to change to user input and create text boxes
                myLimit.LimitDescription = txtMyDescription.Text;
                myLimit.IsStatic = false;
                try
                {
                    LimitManager.Save(myLimit);
                }
                catch (Exception ex)
                {
                    Core.EmailHelper.emailError(ex);
                }
                
                var a = LimitManager.GetLatest(); //need to get the lastly created limit id from the insert we just did above
                myLimitId = Convert.ToInt32(a.LimitID);
                
                //CLAIM LIMIT STUFF
                //myClaimLimit = new ClaimLimit();
                //myClaimLimit.LimitID = myLimitId;
                //myClaimLimit.ClaimID = myClaimID;
                //myClaimLimit.PolicyID = policyID;
                //myClaimLimit.LossAmountACV = txtMyLossAmountACV.Value == null ? 0 : Convert.ToDecimal(txtMyLossAmountACV.Text);
                //myClaimLimit.LossAmountRCV = txtMyLossAmountRCV.Value == null ? 0 : Convert.ToDecimal(txtMyLossAmountRCV.Text);
                //myClaimLimit.OverageAmount = txtMyOverage.Value == null ? 0 : Convert.ToDecimal(txtMyOverage.Text);
                //try
                //{
                //    ClaimLimitManager.Save(myClaimLimit);
                //}
                //catch (Exception ex)
                //{

                //}
                //var b = ClaimLimitManager.GetLatest();
                //myClaimLimitID = Convert.ToInt32(b.ClaimLimitID);
                
                //POLICY LIMIT STUFF
                myPolicyLimit = new PolicyLimit();
                myPolicyLimit.PolicyID = policyID;
                myPolicyLimit.LimitID = myLimitId;
                myPolicyLimit.ClaimLimitID = myClaimLimitID;
                myPolicyLimit.LimitAmount = txtMyLimit.Value == null ? 0 : Convert.ToDecimal(txtMyLimit.Text); //= Convert.ToDecimal(txtMyLimit.Text);
                myPolicyLimit.LimitDeductible = txtMyDeductible.Value == null ? 0 : Convert.ToDecimal(txtMyDeductible.Text);
                myPolicyLimit.CATDeductible = txtMyCATDeductible.Text;
                if(ddlMySettlementType.SelectedItemIndex > 0)
                {
                    myPolicyLimit.ApplyTo = ddlMySettlementType.SelectedItem.Text;
                }
                else
                {
                    myPolicyLimit.ApplyTo = null;
                }
                
                myPolicyLimit.ConInsuranceLimit = Convert.ToDecimal(txtMyCoInsuranceLimit.Value);
                myPolicyLimit.ITV = Convert.ToDecimal(txtMyITV.Value);
                myPolicyLimit.Reserve = Convert.ToDecimal(txtMyReserve.Value);
                myPolicyLimit.WindHailDeductible = txtMyWHDeductible.Text;
                myPolicyLimit.LossAmountACV = txtMyLossAmountACV.Value == null ? 0 : Convert.ToDecimal(txtMyLossAmountACV.Text);
                myPolicyLimit.LossAmountRCV = txtMyLossAmountRCV.Value == null ? 0 : Convert.ToDecimal(txtMyLossAmountRCV.Text);
                myPolicyLimit.OverageAmount = txtMyOverage.Value == null ? 0 : Convert.ToDecimal(txtMyOverage.Text);
                
                try
                {
                    PolicyLimitManager.Save(myPolicyLimit);
                }
                catch (Exception ex)
                {
                    
                }
               
            }

            else //run the regular stuff it was doing before
            {

                //original code
                foreach (GridViewRow row in gvLimits.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        //Label txtLimitLetter = row.FindControl("txtLimitLetter") as Label;
                        //Label txtDescription = row.FindControl("txtDescription") as Label;
                        WebNumericEditor txtLimit = row.FindControl("txtLimit") as WebNumericEditor;
                        WebNumericEditor txtDeductible = row.FindControl("txtDeductible") as WebNumericEditor;
                        WebTextEditor txtWHDeductible = row.FindControl("txtWHDeductible") as WebTextEditor;//new
                        WebTextEditor txtCATDeductible = row.FindControl("txtCATDeductible") as WebTextEditor;
                        WebTextEditor txtSettlementType = row.FindControl("txtSettlementType") as WebTextEditor;
                        WebNumericEditor txtCoInsuranceLimit = row.FindControl("txtCoInsuranceLimit") as WebNumericEditor;
                        WebDropDown ddlSettlementType = row.FindControl("ddlSettlementType") as WebDropDown;
                        WebPercentEditor txtITV = row.FindControl("txtITV") as WebPercentEditor;//new
                        WebNumericEditor txtReserve = row.FindControl("txtReserve") as WebNumericEditor;//new
                        WebNumericEditor txtLossAmountACV = row.FindControl("txtLossAmountACV") as WebNumericEditor;//new
                        WebNumericEditor txtLossAmountRCV = row.FindControl("txtLossAmountRCV") as WebNumericEditor;//new
                        WebNumericEditor txtOverage = row.FindControl("txtOverage") as WebNumericEditor;//new
                        policyLimitID = (int)gvLimits.DataKeys[row.RowIndex].Values[0];
                        limitID = (int)gvLimits.DataKeys[row.RowIndex].Values[1];
                        
                        //claimLimitID = (int)gvLimits.DataKeys[row.RowIndex].Values[2];
                        //claimLimitID = (int)gvLimits.DataKeys[row.RowIndex].Values[2];
                        //if ((txtLossAmountACV.Text != "" ||
                        //    txtLossAmountRCV.Text != "" ||
                        //    txtOverage.Text != "" )&&
                        //   ( txtOverage.Text != "0.00" ||
                        //    txtLossAmountRCV.Text != "0.00" ||
                        //    txtLossAmountACV.Text != "0.00"))
                        //{
                        //CLAIM LIMIT STUFF
                            //myClaimLimit = new ClaimLimit();
                            //myClaimLimit.ClaimLimitID = claimLimitID;
                            //myClaimLimit.LimitID = limitID;
                            //myClaimLimit.ClaimID = myClaimID;
                            //myClaimLimit.PolicyID = policyID;
                            //myClaimLimit.PolicyLimitID = policyLimitID;
                            //myClaimLimit.LossAmountACV = txtLossAmountACV.Value == null ? 0 : Convert.ToDecimal(txtLossAmountACV.Text);
                            //myClaimLimit.LossAmountRCV = txtLossAmountRCV.Value == null ? 0 : Convert.ToDecimal(txtLossAmountRCV.Text);
                            //myClaimLimit.OverageAmount = txtOverage.Value == null ? 0 : Convert.ToDecimal(txtOverage.Text);
                            //try
                            //{
                            //    ClaimLimitManager.Save(myClaimLimit);
                            //}
                            //catch (Exception ex)
                            //{

                            //}
                            ////    var b = ClaimLimitManager.GetLatest2(policyLimitID);
                            ////    myClaimLimitID = Convert.ToInt32(b.ClaimLimitID);
                            ////}
                        if (policyLimitID == 0)
                            limit = new PolicyLimit();
                        else
                            limit = PolicyLimitManager.Get(policyLimitID);

                        limit.PolicyLimitID = policyLimitID;
                        limit.LimitID = limitID;
                        //limit.ClaimLimitID = claimLimitID;
                        limit.PolicyID = policyID;
                        limit.LimitAmount = txtLimit.Value == null ? 0 : Convert.ToDecimal(txtLimit.Value);
                        limit.LimitDeductible = txtDeductible.Value == null ? 0 : Convert.ToDecimal(txtDeductible.Value);
                        limit.CATDeductible = txtCATDeductible.Text;
                        limit.WindHailDeductible = txtWHDeductible.Text;
                        limit.ConInsuranceLimit = txtCoInsuranceLimit.Value == null ? 0 : Convert.ToDecimal(txtCoInsuranceLimit.Value);
                        limit.ITV = Convert.ToDecimal(txtITV.Value);
                        limit.Reserve = Convert.ToDecimal(txtReserve.Value);
                        limit.LossAmountACV = txtLossAmountACV.Value == null ? 0 : Convert.ToDecimal(txtLossAmountACV.Text);
                        limit.LossAmountRCV = txtLossAmountRCV.Value == null ? 0 : Convert.ToDecimal(txtLossAmountRCV.Text);
                        limit.OverageAmount = txtOverage.Value == null ? 0 : Convert.ToDecimal(txtOverage.Text);
                        if (ddlSettlementType.SelectedItemIndex > 0)
                        {
                            //limit.SettlementType = ddlSettlementType.SelectedValue;
                            limit.ApplyTo = ddlSettlementType.SelectedValue;
                        }
                        else
                        {
                            limit.ApplyTo = null;
                        }

                        try
                        {
                            PolicyLimitManager.Save(limit);
                        }
                        catch (Exception ex)
                        {
                            Core.EmailHelper.emailError(ex);
                        }
                    }
                }
            }
            cbAddNewPolicy.Checked = false;
            disableNewRow();
        }

		protected void gvLimits_RowDataBound(object sender, GridViewRowEventArgs e) {
			PolicyLimit limit = null;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				WebDropDown ddlSettlementType = e.Row.FindControl("ddlSettlementType") as WebDropDown;
				if (e.Row.DataItem != null) {
					limit = e.Row.DataItem as PolicyLimit;
					try {
						ddlSettlementType.SelectedValue = limit.ApplyTo;
					}
					catch (Exception ex) {
						ddlSettlementType.SelectedItemIndex = 1;
					}
				}
			}
		}

        protected void gvLimits3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int limitID =Convert.ToInt32(e.CommandArgument);

            DataTable tbllimitGet = HttpContext.Current.Session["Limit"] as DataTable;
            DataTable tblPolicylimitGet = HttpContext.Current.Session["PolicyLimit"] as DataTable;
            DataTable tblCasulityPolicylimitGet = HttpContext.Current.Session["tblCasulityPolicylimit"] as DataTable;
            DataTable tblAllPolicylimitGet = HttpContext.Current.Session["tblAllPolicylimit"] as DataTable;

            string limitFilter = "LimitID=" + limitID;
            DataRow[] limitRow = tbllimitGet.Select(limitFilter);

            DataRow[] policyLimitRow = tblPolicylimitGet.Select(limitFilter);

            DataRow[] casulityPolicyLimitRow = tblCasulityPolicylimitGet.Select(limitFilter);

            DataRow[] allPolicyLimitRow = tblAllPolicylimitGet.Select(limitFilter);

            foreach (DataRow row in limitRow)
            {
                row.Delete();
            }
            tbllimitGet.AcceptChanges();
            foreach (DataRow row in policyLimitRow)
            {
                row.Delete();
            }
            tblPolicylimitGet.AcceptChanges();
            foreach (DataRow row in casulityPolicyLimitRow)
            {
                row.Delete();
            }
            tblCasulityPolicylimitGet.AcceptChanges();
            foreach (DataRow row in allPolicyLimitRow)
            {                
                row.Delete();
            }
            tblAllPolicylimitGet.AcceptChanges();

            

            if (tblAllPolicylimitGet.Rows.Count <= 0)
            {                
               
                Response.Redirect(Request.RawUrl);
            }
           // bindData(0);


        }

        protected void gvLimits2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int limitID = Convert.ToInt32(e.CommandArgument);
            int policyId =Convert.ToInt32(Session["policyID"].ToString());
            ClaimManager objClaimManager = new ClaimManager();


            using (TransactionScope scope = new TransactionScope())
            {
                List<Claim> lstClaim = objClaimManager.GetPolicyClaim(policyId);
                foreach (var claim in lstClaim)
                {
                    int claimId = claim.ClaimID;
                    ClaimLimitManager.EditModeDeleteClaimLimit(limitID, claimId);
                }
                PolicyLimitManager.EditModeDeletePolicyLimit(limitID);
                LimitManager.EditModeDeleteLimit(limitID);
                scope.Complete();
            }

            bindData(policyId);
            Response.Redirect(Request.RawUrl);
        }

        protected void cbAddNewPolicy_CheckedChanged(object sender, EventArgs e)
        {
            // WebTextEditor txtMyCoverage = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyCoverage");
            //WebTextEditor txtMyDescription = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyDescription");
            //WebNumericEditor txtMyLimit = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyLimit");
            //WebNumericEditor txtMyDeductible = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyDeductible");
            //WebTextEditor txtMyCATDeductible = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyCATDeductible");
            //WebDropDown ddlMySettlementType = (WebDropDown)gvLimits.FooterRow.FindControl("ddlMySettlementType");
            //WebPercentEditor txtMyCoInsuranceLimit = (WebPercentEditor)gvLimits.FooterRow.FindControl("txtMyCoInsuranceLimit");
            //WebPercentEditor txtMyITV = (WebPercentEditor)gvLimits.FooterRow.FindControl("txtMyITV");
            //WebPercentEditor txtMyReserve = (WebPercentEditor)gvLimits.FooterRow.FindControl("txtMyReserve");
            //WebTextEditor txtMyWHDeductible = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyWHDeductible");

            if (cbAddNewPolicy.Checked == true)
            {
                enableNewRow();
                gvLimits.FooterRow.BackColor = System.Drawing.Color.PowderBlue;
                //gvLimits.DataBind();
            }
            else
            {
                disableNewRow();
                gvLimits.FooterRow.BackColor = System.Drawing.Color.White;
            }
        }
        public void enableNewRow()
        {
            WebTextEditor txtMyCoverage = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyCoverage");
            WebTextEditor txtMyDescription = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyDescription");
            WebNumericEditor txtMyLimit = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyLimit");
            WebNumericEditor txtMyDeductible = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyDeductible");
            WebTextEditor txtMyCATDeductible = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyCATDeductible");
            WebDropDown ddlMySettlementType = (WebDropDown)gvLimits.FooterRow.FindControl("ddlMySettlementType");
            WebPercentEditor txtMyCoInsuranceLimit = (WebPercentEditor)gvLimits.FooterRow.FindControl("txtMyCoInsuranceLimit");
            WebPercentEditor txtMyITV = (WebPercentEditor)gvLimits.FooterRow.FindControl("txtMyITV");
            WebPercentEditor txtMyReserve = (WebPercentEditor)gvLimits.FooterRow.FindControl("txtMyReserve");
            WebTextEditor txtMyWHDeductible = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyWHDeductible");
            WebNumericEditor txtMyLossAmountACV = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyLossAmountACV");
            WebNumericEditor txtMyLossAmountRCV = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyLossAmountRCV");
            WebNumericEditor txtMyOverage = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyOverage");
            txtMyCoverage.Enabled = true;
            txtMyDescription.Enabled = true;
            txtMyLimit.Enabled = true;
            txtMyDeductible.Enabled = true;
            txtMyCATDeductible.Enabled = true;
            ddlMySettlementType.Enabled = true;
            txtMyCoInsuranceLimit.Enabled = true;
            txtMyITV.Enabled = true;
            txtMyReserve.Enabled = true;
            txtMyWHDeductible.Enabled = true;
            txtMyLossAmountACV.Enabled = true;
            txtMyLossAmountRCV.Enabled = true;
            txtMyOverage.Enabled = true;
        }

        public void disableNewRow()
        {
            WebTextEditor txtMyCoverage = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyCoverage");
            WebTextEditor txtMyDescription = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyDescription");
            WebNumericEditor txtMyLimit = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyLimit");
            WebNumericEditor txtMyDeductible = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyDeductible");
            WebTextEditor txtMyCATDeductible = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyCATDeductible");
            WebDropDown ddlMySettlementType = (WebDropDown)gvLimits.FooterRow.FindControl("ddlMySettlementType");
            WebPercentEditor txtMyCoInsuranceLimit = (WebPercentEditor)gvLimits.FooterRow.FindControl("txtMyCoInsuranceLimit");
            WebPercentEditor txtMyITV = (WebPercentEditor)gvLimits.FooterRow.FindControl("txtMyITV");
            WebPercentEditor txtMyReserve = (WebPercentEditor)gvLimits.FooterRow.FindControl("txtMyReserve");
            WebTextEditor txtMyWHDeductible = (WebTextEditor)gvLimits.FooterRow.FindControl("txtMyWHDeductible");
            WebNumericEditor txtMyLossAmountACV = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyLossAmountACV");
            WebNumericEditor txtMyLossAmountRCV = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyLossAmountRCV");
            WebNumericEditor txtMyOverage = (WebNumericEditor)gvLimits.FooterRow.FindControl("txtMyOverage");

            txtMyCoverage.Enabled = false;
            txtMyDescription.Enabled = false;
            txtMyLimit.Enabled = false;
            txtMyDeductible.Enabled = false;
            txtMyCATDeductible.Enabled = false;
            ddlMySettlementType.Enabled = false;
            txtMyCoInsuranceLimit.Enabled = false;
            txtMyITV.Enabled = false;
            txtMyReserve.Enabled = false;
            txtMyWHDeductible.Enabled = false;
            txtMyLossAmountACV.Enabled = false;
            txtMyLossAmountRCV.Enabled = false;
            txtMyOverage.Enabled = false;

        }
	}
}