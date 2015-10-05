using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.Web.Protected {
	public partial class LeadPolicyEdit : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			// check permissions
			lbtnSave.Visible = Core.PermissionHelper.checkEditPermission("UsersLeads.aspx");
			
			// set default button
			Page.Form.DefaultButton = lbtnSave.UniqueID;

			if (!Page.IsPostBack) {
				policyEditForm.bindData();
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			policyEditForm.saveForm();
		}

		

		//protected void btnBackToPolicy_Click(object sender, EventArgs e) {
		//	int leadID = Core.SessionHelper.getLeadId();
			

		//	string url = string.Format("~/Protected/NewLead.aspx?q={0}", Core.SecurityManager.EncryptQueryString(leadID.ToString()));

		//	Response.Redirect(url);
		//}
	}
}