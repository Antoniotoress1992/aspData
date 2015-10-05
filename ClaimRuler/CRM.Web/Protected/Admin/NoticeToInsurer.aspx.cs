using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class NoticeToInsurer : System.Web.UI.Page {
		string templateURL = "~/Protected/Letters/Notice_To_Insurer.doc";

		protected void Page_Load(object sender, EventArgs e) {
			int leadID = Convert.ToInt32(Session["LeadIds"]);

			if (leadID > 0) {
				autoFillLetter(leadID);				
			}
		}

		protected void autoFillLetter(int leadID) {
			Leads lead = null;
			
			string letterTemplatePath = Server.MapPath(templateURL);

			lead = LeadsManager.Get(leadID);

			if (lead != null) {
				string finalDocumentaPath = Server.MapPath(string.Format("~/Temp/Notice_To_Insurer_{0}.doc", lead.LeadId));

				Core.MergeDocumentHelper.autoFillDocument(letterTemplatePath, finalDocumentaPath, lead, true);

				ReportHelper.renderToBrowser(finalDocumentaPath);
			}

		}

	}
}