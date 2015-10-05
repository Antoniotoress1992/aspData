using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data.Account;

namespace CRM.Web.UserControl.Admin {
	public partial class ucCarrierInvoicProfileType : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {
			
		}
		

		//public string requiredValidation {
		//	get { return ViewState["requiredValidation"].ToString(); }
		//	set {
		//		ViewState["requiredValidation"] = value;
		//		rfvInvoiceProfile.Visible = value.ToLower().Equals("true");
		//	}
		//}

		
		public string SelectedValue {
			get { return ddlInvoicProfileType.SelectedValue; }
			set { ddlInvoicProfileType.SelectedValue = value; }
		}

		public int SelectedIndex {
			get { return ddlInvoicProfileType.SelectedIndex; }
			set { ddlInvoicProfileType.SelectedIndex = value; }
		}

		protected void ddlInvoiceProfile_SelectedIndexChanged(object sender, EventArgs e) {

		}
	}
}