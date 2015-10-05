using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Transactions;

using Infragistics.Web.UI.EditorControls;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;

using Microsoft.Reporting.WebForms;

namespace CRM.Web.Protected {
	public partial class InvoiceEditPopUp : System.Web.UI.Page {
	
		protected void Page_Load(object sender, EventArgs e) {
			string dencryptedID = null;
			int invoiceID = 0;

			if (Request.Params["q"] != null) {
				dencryptedID = Core.SecurityManager.DecryptQueryString(Request.Params["q"].ToString());
				int.TryParse(dencryptedID, out invoiceID);
			}
		

			if (!Page.IsPostBack) {
				invoiceEdit.bindData(invoiceID);
			}
		}
	}
}