using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.Web.Protected.Admin {
	public partial class InvoiceServicesExpenses : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			// check user permission
			Master.checkPermission();

			if (!Page.IsPostBack) {
				invoiceServices.bindData();

				invoiceExpenses.bindData();
			}
		}
	}
}