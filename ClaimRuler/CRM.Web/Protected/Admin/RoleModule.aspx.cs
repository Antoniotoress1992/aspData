using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.Web.Protected.Admin {
	public partial class RoleModule : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			// make sure this is Admin
			if (!Core.PermissionHelper.isAdmin()) {
				Response.Redirect("~/AccessDenied.aspx");
			}
		}
	}
}