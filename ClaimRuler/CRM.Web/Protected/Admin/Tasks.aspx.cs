using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;

using CRM.Web.Utilities;

namespace CRM.Web.Protected.Admin {
	public partial class Tasks : System.Web.UI.Page {
		/// <summary>
		/// This is a dummy page 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
	
		protected void Page_Load(object sender, EventArgs e) {

			Response.Redirect("~/Protected/Tasks.aspx");
		}

	
	}
}