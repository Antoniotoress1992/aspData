using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;

using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class ContractAdjustingServices : System.Web.UI.Page {
		string templateURL = "~/Protected/Letters/Contract_for_Adjusting_Services.doc";

		protected void Page_Load(object sender, EventArgs e) {
			int leadID = Convert.ToInt32(Session["LeadIds"]);

			if (leadID > 0)
				autoFillLetter(leadID);			
		}

		protected void autoFillLetter(int leadID) {
			Leads lead = null;

			string letterTemplatePath = Server.MapPath(templateURL);

			lead = LeadsManager.Get(leadID);

			if (lead != null) {
				string finalDocumentaPath = Server.MapPath(string.Format("~/Temp/Contract_for_Adjusting_Services{0}.doc", lead.LeadId));

				Core.MergeDocumentHelper.autoFillDocument(letterTemplatePath, finalDocumentaPath, lead, true);

				ReportHelper.renderToBrowser(finalDocumentaPath);
			}

		}
		
	}
}