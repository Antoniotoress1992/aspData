using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CRM.Data;
using CRM.Data.Account;
using Infragistics.Web.UI.EditorControls;
using CRM.Data.Entities;
using System.Data;
using CRM.Repository;
using System.Transactions;

namespace CRM.Web.UserControl.Admin {
	public partial class ucPolicyCasualtyLimits : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {

		}

		public void bindData(int policyID) {
			List<PolicyLimit> limits = null;


            if (HttpContext.Current.Session["Limit"] != null && HttpContext.Current.Session["PolicyLimit"] != null && HttpContext.Current.Session["tblCasulityPolicylimit"] != null && HttpContext.Current.Session["tblAllPolicylimit"] != null)
            {
                DataTable Limit = HttpContext.Current.Session["Limit"] as DataTable;
                DataTable PolicyLimit = HttpContext.Current.Session["tblCasulityPolicylimit"] as DataTable;

                var results = from limit in Limit.AsEnumerable()
                              join policylimit in PolicyLimit.AsEnumerable() on (int)limit["LimitID"] equals (int)policylimit["LimitID"]
                              select new
                              {
                                  LimitLetter = (string)limit["LimitLetter"],
                                  LimitDescription = (string)limit["LimitDescription"],
                                  LimitAmount = (decimal)policylimit["LimitAmount"],
                                  LimitID = (int)limit["LimitID"],
                                  //LimitDeductible = (decimal)policylimit["LimitDeductible"],
                                  //ApplyTo = (string)policylimit["ApplyTo"],
                                  //ITV = (decimal)policylimit["ITV"],
                                  //Reserve = (decimal)policylimit["Reserve"],
                              };


                gvLimits3.DataSource = results;
                gvLimits3.DataBind();

                if (!results.Any())
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
                limits = PolicyLimitManager.GetAll(policyID, LimitType.LIMIT_TYPE_CASUALTY);
                bool isStatic = PolicyLimitManager.LimitIsStatic(policyID);


                //gvLimits.DataSource = limits;
                //gvLimits.DataBind();

                lblNoRecordFound.Visible = false;
                if (limits.Count > 0)
                {

                    if (isStatic)
                    {

                        gvLimits.DataSource = limits;
                        gvLimits.DataBind();

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

                if (isStatic == true || policyID == 0)
                {
                    if (limits != null && limits.Count == 0)
                    {
                        limits = primeCasualtyLimits();
                        gvLimits.DataSource = limits;
                        gvLimits.DataBind();

                        gvLimits2.DataSource = null;
                        gvLimits2.DataBind();
                        lblNoRecordFound.Visible = false;
                    }
                }

                gvLimits3.DataSource = null;
                gvLimits3.DataBind();

            }

		}

		private List<PolicyLimit> primeCasualtyLimits() {
			List<PolicyLimit> casualtyLimits = null;
			List<Limit> limits = null;

			limits = LimitManager.GetAll(LimitType.LIMIT_TYPE_CASUALTY);

			casualtyLimits = (from x in limits
						   select new PolicyLimit {
							   LimitID = x.LimitID,
							   Limit = x
						   }).ToList<PolicyLimit>();

			return casualtyLimits;
		}


		

		public void saveLimits(int policyID) {
			int policyLimitID = 0;
			int limitID = 0;
			PolicyLimit limit = null;

			foreach (GridViewRow row in gvLimits.Rows) {
				if (row.RowType == DataControlRowType.DataRow) {
					WebNumericEditor txtLimit = row.FindControl("txtLimit") as WebNumericEditor;
					

					policyLimitID = (int)gvLimits.DataKeys[row.RowIndex].Values[0];
					limitID = (int)gvLimits.DataKeys[row.RowIndex].Values[1];

					if (policyLimitID == 0)
						limit = new PolicyLimit();
					else
						limit = PolicyLimitManager.Get(policyLimitID);

					limit.PolicyLimitID = policyLimitID;
					limit.LimitID = limitID;

					limit.PolicyID = policyID;
					limit.LimitAmount = txtLimit.Value == null ? 0 : Convert.ToDecimal(txtLimit.Value);
				
					try {
						PolicyLimitManager.Save(limit);
					}
					catch (Exception ex) {
						Core.EmailHelper.emailError(ex);
					}
				}
			}
		}


        protected void gvLimits3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int limitID = Convert.ToInt32(e.CommandArgument);

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


            bindData(0);
        }

        protected void gvLimits2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int limitID = Convert.ToInt32(e.CommandArgument);
            int policyId = Convert.ToInt32(Session["policyID"].ToString());
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
            Response.Redirect(Request.RawUrl);
            bindData(policyId);

        }

	}
}