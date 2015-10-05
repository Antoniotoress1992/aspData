using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.Web.Protected {
	public partial class LeadInvoiceLedger : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			// access master page from nested master page
			Protected.ClaimRuler masterPage = Master.Master as Protected.ClaimRuler;

			// check user permission
			masterPage.checkPermission();

			if (!Page.IsPostBack) {
				legacyInvoices.bindData();
			}

		}

		

	}
}