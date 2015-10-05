using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;

namespace CRM.Web.UserControl.Admin {
	public partial class ucCarrierChannel : System.Web.UI.UserControl {
		int carrierID {
			get { return Session["CarrierID"] != null ? Convert.ToInt32(Session["CarrierID"]) : 0; }

		}
		protected void Page_Load(object sender, EventArgs e) {
			BindData();
		}

		private void BindData() {
			//if (carrierID > 0) {
			//	gvChannels.DataSource = CarrierChannelManager.GetChannels(carrierID);
			//	gvChannels.DataBind();
			//}
		}
	}
}