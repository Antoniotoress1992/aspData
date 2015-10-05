using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Web.Protected {
	public class ClaimRulerLeadBase : System.Web.UI.MasterPage {
		public void btnReturnToClaim_Click(object sender, EventArgs e) {
			int leadID = Core.SessionHelper.getLeadId();
			if (leadID > 0) {
				Response.Redirect("~/Protected/NewLead.aspx?q=" + Core.SecurityManager.EncryptQueryString(leadID.ToString()));
			}
		}

	}

	
}