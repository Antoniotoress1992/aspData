using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.Web.Protected {
	public partial class ClaimRulerClaim : System.Web.UI.MasterPage {
		protected void Page_Load(object sender, EventArgs e) {

		}
		public void btnReturnToClient_Click(object sender, EventArgs e) {
			int leadID = Core.SessionHelper.getLeadId();
			if (leadID > 0) {
				Response.Redirect("~/Protected/NewLead.aspx?q=" + Core.SecurityManager.EncryptQueryString(leadID.ToString()));
			}
		}

		public void btnReturnToClaim_Click(object sender, EventArgs e) {
			Response.Redirect("~/Protected/ClaimEdit.aspx");
		}

		public void btnReturnToPolicy_Click(object sender, EventArgs e) {
			Response.Redirect("~/Protected/LeadPolicyEdit.aspx");
		}


		public bool enableBackToClaimButton {
			set {
				this.btnReturnToClaim.Visible = value;
			}
		}

		public void showTopMenu(bool isVisible) {
			claimTopMenu.Visible = isVisible;
		}
		
	}
}