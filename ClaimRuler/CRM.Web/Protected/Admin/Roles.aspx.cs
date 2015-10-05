
namespace CRM.Web.Protected.Admin {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;

	public partial class Roles : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			// make sure this is Admin
			if (!Core.PermissionHelper.isAdmin()) {
				Response.Redirect("~/AccessDenied.aspx");
			}
		}
	}
}