using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CRM.Data;
using CRM.Data.Account;

using Infragistics.Web.UI.EditorControls;
using Infragistics.Web.UI.ListControls;
using CRM.Data.Entities;

namespace CRM.Web.UserControl {
	public partial class ucClaimPropertyLimits : System.Web.UI.UserControl {


		List<vw_ClaimLimit> limits = null;
		protected void Page_Load(object sender, EventArgs e) {
            
		}
		
        //public void bindData(int claimID) {
           
        //    limits = ClaimLimitManager.GetAll(claimID, LimitType.LIMIT_TYPE_PROPERTY);

        //    if (limits != null && limits.Count == 0) {
        //        //limits = primePropertyLimits();
        //    }
        //    lblNoRecordFound.Visible = false;
        //    if (limits.Count > 0)
        //    {

        //        if (limits[0].IsStatic != false)
        //        {

        //            gvLimits.DataSource = limits;
        //            gvLimits.DataBind();

        //            gvLimits2.DataSource = null;
        //            gvLimits2.DataBind();
        //        }
        //        else
        //        {
        //            gvLimits.DataSource = null;
        //            gvLimits.DataBind();
        //            gvLimits2.DataSource = limits;
        //            gvLimits2.DataBind();
        //        }
        //    }
        //    else
        //    {
        //        lblNoRecordFound.Visible = true;
        //    }

        //}

        public void bindData(int policyID)
        {
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


                //gvLimits3.DataSource = results;
                //gvLimits3.DataBind();

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
                        lblNoRecordFound.Visible = false;
                        //disableNewRow();
                       // gvLimits2.DataSource = null;
                       // gvLimits2.DataBind();
                    }
                    else
                    {

                        gvLimits.DataSource = null;
                        gvLimits.DataBind();
                       // gvLimits2.DataSource = limits;
                       // gvLimits2.DataBind();
                    }

                }
                else
                {
                    lblNoRecordFound.Visible = true;
                }
                if (limits != null && limits.Count == 0 && policyID == 0)
                {

                    //limits = primePropertyLimits();
                    gvLimits.DataSource = limits;
                    gvLimits.DataBind();

                   // gvLimits2.DataSource = null;
                   // gvLimits2.DataBind();
                    lblNoRecordFound.Visible = false;
                }

                //gvLimits3.DataSource = null;
                //gvLimits3.DataBind();

            }




        }
        protected void gvLimits_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PolicyLimit limit = null;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                WebDropDown ddlSettlementType = e.Row.FindControl("ddlSettlementType") as WebDropDown;
                if (e.Row.DataItem != null)
                {
                    limit = e.Row.DataItem as PolicyLimit;
                    try
                    {
                        ddlSettlementType.SelectedValue = limit.ApplyTo;
                    }
                    catch (Exception ex)
                    {
                        ddlSettlementType.SelectedItemIndex = 1;
                    }
                }
            }
        }
		
		private List<vw_ClaimLimit> primePropertyLimits() {
			List<Limit> limits = null;
			List<vw_ClaimLimit> propertyLimits = null;

			limits = LimitManager.GetAll(LimitType.LIMIT_TYPE_PROPERTY);

			propertyLimits = (from x in limits
						   select new vw_ClaimLimit {
							   LimitID = x.LimitID,
							   LimitLetter = x.LimitLetter,
							   LimitType = x.LimitType,
							   LimitDescription = x.LimitDescription
						   }).ToList<vw_ClaimLimit>();

			return propertyLimits;

		}

		public void saveLimits(int claimID) {
			int claimLimitID = 0;
			int limitID = 0;
			ClaimLimit limit = null;

			foreach (GridViewRow row in gvLimits.Rows) {
				if (row.RowType == DataControlRowType.DataRow) {
					try {
						WebNumericEditor txtLossAmountACV = row.FindControl("txtLossAmountACV") as WebNumericEditor;
						WebNumericEditor txtLossAmountRCV = row.FindControl("txtLossAmountRCV") as WebNumericEditor;
						WebTextEditor txtDepreciation = row.FindControl("txtDepreciation") as WebTextEditor;
                        WebTextEditor txtNonRecoverableDepreciation = row.FindControl("txtNonRecoverableDepreciation") as WebTextEditor;
						WebTextEditor txtOverageAmount = row.FindControl("txtOverageAmount") as WebTextEditor;

						claimLimitID = (int)gvLimits.DataKeys[row.RowIndex].Values[0];
						limitID = (int)gvLimits.DataKeys[row.RowIndex].Values[1];

						if (claimLimitID == 0) {
							// new 
							limit = new ClaimLimit();
							limit.ClaimID = claimID;
							limit.LimitID = limitID;
						}
						else
							limit = ClaimLimitManager.Get(claimLimitID);

						limit.LossAmountACV = txtLossAmountACV.Value == null ? 0 : Convert.ToDecimal(txtLossAmountACV.Value);
						limit.LossAmountRCV = txtLossAmountRCV.Value == null ? 0 : Convert.ToDecimal(txtLossAmountRCV.Value);
						limit.Depreciation = txtDepreciation.Value == null ? 0 : Convert.ToDecimal(txtDepreciation.Value);
                        limit.NonRecoverableDepreciation = txtNonRecoverableDepreciation.Value == null ? 0 : Convert.ToDecimal(txtNonRecoverableDepreciation.Value);
						limit.OverageAmount = txtOverageAmount.Value == null ? 0 : Convert.ToDecimal(txtOverageAmount.Value);
				
						ClaimLimitManager.Save(limit);
					}
					catch (Exception ex) {
						Core.EmailHelper.emailError(ex);
					}
				}
       	}

            foreach (GridViewRow row in gvLimits2.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    try
                    {
                        WebNumericEditor txtLossAmountACV = row.FindControl("txtLossAmountACV") as WebNumericEditor;
                        WebNumericEditor txtLossAmountRCV = row.FindControl("txtLossAmountRCV") as WebNumericEditor;
                        WebTextEditor txtDepreciation = row.FindControl("txtDepreciation") as WebTextEditor;
                        WebTextEditor txtNonRecoverableDepreciation = row.FindControl("txtNonRecoverableDepreciation") as WebTextEditor;
                        WebTextEditor txtOverageAmount = row.FindControl("txtOverageAmount") as WebTextEditor;

                        claimLimitID = (int)gvLimits2.DataKeys[row.RowIndex].Values[0];
                        limitID = (int)gvLimits2.DataKeys[row.RowIndex].Values[1];

                        if (claimLimitID == 0)
                        {
                            // new 
                            limit = new ClaimLimit();
                            limit.ClaimID = claimID;
                            limit.LimitID = limitID;
                        }
                        else
                            limit = ClaimLimitManager.Get(claimLimitID);

                        limit.LossAmountACV = txtLossAmountACV.Value == null ? 0 : Convert.ToDecimal(txtLossAmountACV.Value);
                        limit.LossAmountRCV = txtLossAmountRCV.Value == null ? 0 : Convert.ToDecimal(txtLossAmountRCV.Value);
                        limit.Depreciation = txtDepreciation.Value == null ? 0 : Convert.ToDecimal(txtDepreciation.Value);
                        limit.NonRecoverableDepreciation = txtNonRecoverableDepreciation.Value == null ? 0 : Convert.ToDecimal(txtNonRecoverableDepreciation.Value);
                        limit.OverageAmount = txtOverageAmount.Value == null ? 0 : Convert.ToDecimal(txtOverageAmount.Value);

                        ClaimLimitManager.Save(limit);
                    }
                    catch (Exception ex)
                    {
                        Core.EmailHelper.emailError(ex);
                    }
                }
            }	


		}

        //public void FillLossOfUse(Claim objClaim )
        //{
        //    if (objClaim.LeadPolicy.Leads.LossOfUseAmount!=null)
        //    {
        //    txtLossAmount.Text =Convert.ToString(objClaim.LeadPolicy.Leads.LossOfUseAmount);
        //    }
        //    if (objClaim.LeadPolicy.Leads.LossOfUseReserve != null)
        //    {
        //        txtLossReserve.Text = Convert.ToString(objClaim.LeadPolicy.Leads.LossOfUseReserve);
        //    }
        //}

        //public string getLossAmount()
        //{
        //    string amount=txtLossAmount.Text;
        //    return amount;
           
        //}
        //public string getLossReserve()
        //{
        //    string amount = txtLossReserve.Text;
        //    return amount;
        //}

		//protected void gvLimits_RowDataBound(object sender, GridViewRowEventArgs e) {
		//	//decimal totalAmount = 0;
		//	//decimal propertyTotalLossAmountACV = 0;
		//	//decimal propertyTotalLossAmountRCV = 0;
		//	//decimal propertyTotalDepreciation = 0;
		//	//decimal propertyTotalDeductible = 0;

		//	//if (e.Row.RowType == DataControlRowType.Footer) {
		//	//	// total loss amount acv
		//	//	totalAmount = limits.Sum(x => x.LossAmountACV ?? 0);
		//	//	WebTextEditor lblTotalLossAmountACV = e.Row.FindControl("lblTotalLossAmountACV") as WebTextEditor;
		//	//	lblTotalLossAmountACV.Text = totalAmount.ToString("N2");

		//	//	// total loss amount rcv
		//	//	totalAmount = limits.Sum(x => x.LossAmountRCV ?? 0);
		//	//	WebTextEditor lblTotalLossAmountRCV = e.Row.FindControl("lblTotalLossAmountRCV") as WebTextEditor;
		//	//	lblTotalLossAmountACV.Text = totalAmount.ToString("N2");
		//	//}
		//}
	}
}