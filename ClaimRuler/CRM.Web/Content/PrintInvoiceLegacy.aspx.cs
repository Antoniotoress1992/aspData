using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;

using Microsoft.Reporting.WebForms;
using CRM.Data.Entities;

namespace CRM.Web.Content {
	public partial class PrintInvoiceLegacy : System.Web.UI.Page {

		List<InvoiceView> invoice = null;
		string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

		protected void Page_Load(object sender, EventArgs e) {
			int invoiceID = Request.Params["id"] == null ? 0 : Convert.ToInt32(Request.Params["id"]);

			invoice = LeadInvoiceManager.GetInvoiceForReport(invoiceID);

			if (invoice != null) {
				reportViewer.Reset();
				reportViewer.LocalReport.DataSources.Clear();

				reportViewer.LocalReport.EnableExternalImages = true;

				ReportDataSource reportDataSource = new ReportDataSource();
				reportDataSource.Name = "DataSet1";
				reportDataSource.Value = invoice;

				reportViewer.LocalReport.DataSources.Add(reportDataSource);

				reportViewer.LocalReport.ReportPath = appPath + "/Content/Invoice.rdlc";

				this.reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subReportsEventHandler);

				string pdfPath = string.Format("{0}/Temp/{1}.pdf", appPath, Guid.NewGuid());

				Core.ReportHelper.savePDFFromLocalReport(reportViewer.LocalReport, pdfPath);

				Core.ReportHelper.renderToBrowser(pdfPath);
			}
		}
		protected void subReportsEventHandler(object sender, SubreportProcessingEventArgs e) {
			e.DataSources.Add(new ReportDataSource("DataSet1", invoice[0].legacyInvoiceLines));
		}
	}

}