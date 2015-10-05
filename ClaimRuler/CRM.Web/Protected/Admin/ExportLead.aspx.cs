using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;

using CRM.Web.Utilities;

namespace CRM.Web.Protected.Admin {
	public partial class ExportLead : System.Web.UI.Page {


		protected void Page_Load(object sender, EventArgs e) {
			Response.Redirect("~/Protected/ExportLead.aspx");
		}

		
	}
}