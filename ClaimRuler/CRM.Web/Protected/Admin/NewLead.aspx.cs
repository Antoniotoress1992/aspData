using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;

namespace CRM.Web.Protected.Admin {
	public partial class NewLead : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			string url = "~/Protected/NewLead.aspx";

			// check lead id was passed on query string
			if (Request.Params["id"] != null) {
				url = "~/Protected/NewLead.aspx?q=" + Core.SecurityManager.EncryptQueryString(Request.Params["id"].ToString());
			}			
			Response.Redirect(url);
		}
		
	}
}