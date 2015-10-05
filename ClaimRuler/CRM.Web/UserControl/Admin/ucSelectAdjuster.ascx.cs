using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data.Account;
using CRM.Web.Utilities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucSelectAdjuster : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {		
		}

		public string requiredValidation {
			get { return ViewState["requiredValidation"].ToString(); }
			set {
				ViewState["requiredValidation"] = value;
				rfvAdjuster.Visible = value.ToLower().Equals("true");
			}
		}

		public void bindData(int clientID) {
			CollectionManager.FillCollection(ddlAdjuster, "AdjusterId", "AdjusterName", AdjusterManager.GetAll(clientID));
		}
		public string SelectedValue {
			get { return ddlAdjuster.SelectedValue; }
			set { ddlAdjuster.SelectedValue = value; }
		}

		public int SelectedIndex {
			get { return ddlAdjuster.SelectedIndex; }
			set { ddlAdjuster.SelectedIndex = value; }
		}
	}
}