
namespace CRM.Web {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.Security;

	public partial class Site : System.Web.UI.MasterPage {
		protected void Page_Load(object sender, EventArgs e) {							
		}

		protected void Page_Init(object sender, EventArgs e) {
			if (Session["UserId"] == null) {
				Session.Abandon();

				FormsAuthentication.SignOut();
							
				
				Response.Redirect("~/Login.aspx");
			}
		}

		public void showFooter(bool isVisible) {
			ucFooter1.Visible = isVisible;
		}
	}
}