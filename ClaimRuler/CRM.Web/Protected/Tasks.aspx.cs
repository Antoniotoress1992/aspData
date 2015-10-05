using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CRM.Core;
using CRM.Data;
using CRM.Data.Account;

using CRM.Web.Utilities;

namespace CRM.Web.Protected {
	public partial class Tasks : System.Web.UI.Page {



		protected void Page_Load(object sender, EventArgs e) {
			int roleID = 0;
			string url = null;

			// check user permission
			Master.checkPermission();

			roleID = SessionHelper.getUserRoleId();

			if (roleID == (int)UserRole.Client)
				url = "~/Protected/Admin/Activities.aspx";
			else
				url = "~/Protected/Activities.aspx";

			Response.Redirect(url);

			
		}
		
	}
}