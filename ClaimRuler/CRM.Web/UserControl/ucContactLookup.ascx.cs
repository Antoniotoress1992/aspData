using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.Web.UserControl {
	public partial class ucContactLookup : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {

		}
		public void bindData() {
			gvContacts.DataSourceID = "edsContacts";
			gvContacts.DataBind();
		}
	}
}