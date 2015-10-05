using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;

namespace CRM.Web.UserControl.Admin {
	public partial class ucPolicyList1 : System.Web.UI.UserControl {
		// solution found at: http://www.dotnettwitter.com/2011/06/hierarchical-gridview-in-aspnet.html
		protected Protected.ClaimRuler masterPage = null;

		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;
		}

		protected void gvPolicyList_RowCommand(object sender, GridViewCommandEventArgs e) {
			int id = 0;

			switch (e.CommandName) {
				case "DoEdit":
				case "DoView":
					Session["policyID"] = e.CommandArgument;

					Response.Redirect("~/Protected/LeadPolicyEdit.aspx");					
					break;

				case "DoDelete":
					id = Convert.ToInt32(e.CommandArgument);
					try {
						Data.Entities.LeadPolicy policy = LeadPolicyManager.Get(id);
						if (policy != null) {
							policy.IsActive = false;

							LeadPolicyManager.Save(policy);

							// refresh policy list
							gvPolicyList.DataBind();
						}
					}
					catch (Exception ex) {
						Core.EmailHelper.emailError(ex);
					}
					break;

				
				default:
					break;
			}

		}


		protected void gvPolicyList_RowDataBound(object sender, GridViewRowEventArgs e) {
			Data.Entities.LeadPolicy policy = null;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				if (e.Row.DataItem != null) {
					policy = e.Row.DataItem as Data.Entities.LeadPolicy;

					ucClaimList claimList = e.Row.FindControl("claimList") as ucClaimList;

					claimList.bindData(policy.Id);

					//GridView gvClaims = e.Row.FindControl("gvClaims") as GridView;

					//gvClaims.DataSource = policy.Claims;

					//gvClaims.DataBind();
				}
			}
		}

		


	}
}