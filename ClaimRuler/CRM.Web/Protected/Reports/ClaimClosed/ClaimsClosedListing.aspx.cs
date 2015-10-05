using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;

using Microsoft.Reporting.WebForms;

namespace CRM.Web.Reports.ClaimClosed {
	public partial class ClaimsClosedListing : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			if (!Page.IsPostBack) {
				bindReport();
			}
			
		}

		private void bindReport() {
			int clientID = Core.SessionHelper.getClientId();
            List<CRM.Data.Entities.vw_ClaimsClosedListing> reportData = null;

			using (RerportManager report = new RerportManager()) {
				reportData = report.claimsClosedListing(clientID);
			}


			if (reportData != null) {
				reportViewer.Reset();
				reportViewer.ProcessingMode = ProcessingMode.Local;

				reportViewer.LocalReport.DataSources.Clear();

				reportViewer.LocalReport.EnableExternalImages = true;

				ReportDataSource reportDataSource = new ReportDataSource();
				reportDataSource.Name = "DataSet1";
				reportDataSource.Value = reportData;

				reportViewer.LocalReport.DataSources.Add(reportDataSource);

				reportViewer.LocalReport.ReportPath = Server.MapPath("~/Protected/Reports/ClaimClosed/ClaimsClosedListing.rdlc");
			}
		}
	}
}