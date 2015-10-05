using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data.Account;
using CRM.Data;
using CRM.Web.Utilities;

namespace CRM.Web.Protected.Admin {
	public partial class DashboardStatistics : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			if (!Page.IsPostBack) {
				loadData();
			}
		}

		protected void loadData() {
			int clientID = SessionHelper.getClientId();
			int openCount = 0;
			int closeCount = 0;

			if (clientID > 0) {
				openCount = LeadsManager.GetOpenLeadCount(clientID);

				closeCount = LeadsManager.GetCloseLeadCount(clientID);

				lblOpenLeadCount.Text = string.Format("{0:n0}", openCount);

				lblCloseLeadCount.Text = string.Format("{0:n0}", closeCount);
			}
		}
	}
}