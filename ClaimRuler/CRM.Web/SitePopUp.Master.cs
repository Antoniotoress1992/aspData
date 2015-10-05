using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace CRM.Web {
	public partial class SitePopUp : System.Web.UI.MasterPage {
		protected void Page_Load(object sender, EventArgs e) {

		}
		protected void Page_Init(object sender, EventArgs e) {
			if (Session["UserId"] == null) {
				Session.Abandon();

				FormsAuthentication.SignOut();


				Response.Redirect("~/Login.aspx");
			}
		}
	}
}