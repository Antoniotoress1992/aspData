using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CRM.Core;

namespace CRM.Web.Protected.Admin {
	public partial class ProofofLoss : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

			string letetrTemplatePath = Server.MapPath("~/Protected/Letters/Proof_of_Loss.xls");

			ReportHelper.renderToBrowser(letetrTemplatePath);
		}
	}
}