using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Web.Utilities;

namespace CRM.Web.Protected.Admin {
	public partial class ShowHelp : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			string path = "../../Content/Claim_Ruler_Instructions.pdf";

			string js = string.Format("<script type='text/javascript'>openHelpWindow('{0}');</script>", path);

			
			Page.ClientScript.RegisterStartupScript(typeof(Page), "help", js);

			//ReportHelper.renderToBrowser(path);
		}
	}
}