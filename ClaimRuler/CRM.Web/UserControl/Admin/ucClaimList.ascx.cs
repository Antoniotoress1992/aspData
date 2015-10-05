using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucClaimList : System.Web.UI.UserControl {
		

		protected void Page_Load(object sender, EventArgs e) {

		}

		public void bindData(int policyID) {
			List<Claim> claims = null;

			claims = ClaimsManager.GetAll(policyID);

			gvClaims.DataSource = claims;
			gvClaims.DataBind();
		}
		protected void gvClaims_RowCommand(object sender, GridViewCommandEventArgs e) {
			string[] ids = e.CommandArgument.ToString().Split(new char[] {'|'});

			int claimID = Convert.ToInt32(ids[0]);
			int policyID = Convert.ToInt32(ids[1]);

			Claim claim = null;

			if (e.CommandName == "DoEdit") {				
				Session["ClaimID"] = claimID;
				Session["policyID"] = policyID;

				Response.Redirect("~/Protected/ClaimEdit.aspx");
			}
			else if (e.CommandName == "DoDelete") {
				try {
					claim = ClaimsManager.Get(claimID);

					if (claim != null) {
						// make claim as deleted
						claim.IsActive = false;

						ClaimsManager.Save(claim);

						// refresh claim list
						bindData(claim.PolicyID);
					}
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}
		}		
	}
}