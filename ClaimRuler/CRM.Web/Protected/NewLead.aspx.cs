using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;

namespace CRM.Web.Protected {
	public partial class NewLead : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			string view = null;

			if (!Page.IsPostBack) {

				// check user permission
				//Master.checkPermission();


				// clear previous lead id that user open to work with
				Session["ClaimantFirstName"] = null;

				Session["ClaimantLastName"] = null;
				Session["InsuredName"] = null;

				Session["LeadIds"] = null;

				if (Core.PermissionHelper.checkViewPermission("AllUsersLeads.aspx") && !Core.PermissionHelper.checkEditPermission("AllUsersLeads.aspx"))
					view = "viewOnly";
				else
					view = "";

				// check lead id was passed on query string(encrypted)
				if (Request.Params["q"] != null) {
					string id = Core.SecurityManager.DecryptQueryString(Request.Params["q"].ToString());

					this.Context.Items.Add("selectedleadid", id);
					//this.Context.Items.Add("view", "");
					this.Context.Items.Add("view", view);
				}

				// legacy support - check lead id was passed on query string
				if (Request.Params["id"] != null) {
					this.Context.Items.Add("selectedleadid", Request.Params["id"].ToString());
					//this.Context.Items.Add("view", "");
					this.Context.Items.Add("view", view);
				}
			}
		}
		
		
	}
}