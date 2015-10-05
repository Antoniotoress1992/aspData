using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.Web {
	public partial class AccessDenied : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

		}

		protected void btnContinue_Click(object sender, EventArgs e) {
			FormsAuthentication.SignOut();
			Session.Abandon();
			Response.Redirect("~/Login.aspx");
		}
	}
}