using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;

using Microsoft.Reporting.WebForms;

using iTextSharp.text;
using iTextSharp.text.pdf;
using CRM.Data.Entities;

namespace CRM.Web.Content {
	public partial class PrintInvoice : System.Web.UI.Page {
		List<InvoiceView> invoice = null;
		string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

		protected void Page_Load(object sender, EventArgs e) {

			if (!Page.IsPostBack) {
				bindReport();
			}

		}

		private void bindReport() {
			int invoiceID = 0;
			string decryptedUID = Core.SecurityManager.DecryptQueryString(Request.Params["q"].ToString());

			int.TryParse(decryptedUID, out invoiceID);

			invoice = InvoiceManager.GetInvoiceForReport(invoiceID);

			if (invoice != null) {
				reportViewer.Reset();
				reportViewer.LocalReport.DataSources.Clear();

				reportViewer.LocalReport.EnableExternalImages = true;

				ReportDataSource reportDataSource = new ReportDataSource();
				reportDataSource.Name = "DataSet1";
				reportDataSource.Value = invoice;

				reportViewer.LocalReport.DataSources.Add(reportDataSource);

				reportViewer.LocalReport.ReportPath = Server.MapPath("~/Content/Invoice.rdlc");

				this.reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subReportsEventHandler);

				string pdfPath = string.Format("{0}/Temp/{1}.pdf", appPath, Guid.NewGuid());

				Core.ReportHelper.savePDFFromLocalReport(reportViewer.LocalReport, pdfPath);

				pdfPath = addCompanyLogo(pdfPath);

				Core.ReportHelper.renderToBrowser(pdfPath);
			}
		}

		private string addCompanyLogo(string pdfPath) {
			string outputPDFPath = null;
			int clientID = Core.SessionHelper.getClientId();

			string logoPath = string.Format("{0}/ClientLogo/{1}.jpg", ConfigurationManager.AppSettings["appPath"].ToString(), clientID);
			if (File.Exists(logoPath)) {
				outputPDFPath = string.Format("{0}/Temp/{1}.pdf", appPath, Guid.NewGuid());

	
				using (var inputPdfStream = new FileStream(pdfPath, FileMode.Open))

				using (var outputPdfStream = new FileStream(outputPDFPath, FileMode.Create)) {
					PdfReader reader = new PdfReader(inputPdfStream);

					PdfStamper stamper = new PdfStamper(reader, outputPdfStream);

					PdfContentByte pdfContentByte = stamper.GetOverContent(1);

					//var image = iTextSharp.text.Image.GetInstance(inputImageStream);
					var image = iTextSharp.text.Image.GetInstance(logoPath);
					image.ScaleToFit(100f, 100f);

					PdfDocument doc = pdfContentByte.PdfDocument;

					image.SetAbsolutePosition(40f, doc.PageSize.Height - 150f);
					pdfContentByte.AddImage(image);
					stamper.Close();
				}
			}
			else {
				outputPDFPath = pdfPath;
			}

			return outputPDFPath;
		}
		//private string addCompanyLogo(string pdfPath) {
		//	string outputPDFPath = null;
		//	int clientID = Core.SessionHelper.getClientId();

		//	string logoPath = string.Format("{0}/ClientLogo/{1}.jpg", ConfigurationManager.AppSettings["appPath"].ToString(), clientID);
		//	if (File.Exists(logoPath)) {
		//		outputPDFPath = string.Format("{0}/Temp/{1}.pdf", appPath, Guid.NewGuid());

		//		using (var inputPdfStream = new FileStream(pdfPath, FileMode.Open))
		//		using (var inputImageStream = new FileStream(logoPath, FileMode.Open))
		//		using (var outputPdfStream = new FileStream(outputPDFPath, FileMode.Create)) {
		//			PdfReader reader = new PdfReader(inputPdfStream);
		//			PdfStamper stamper = new PdfStamper(reader, outputPdfStream);
		//			PdfContentByte pdfContentByte = stamper.GetOverContent(1);
		//			var image = iTextSharp.text.Image.GetInstance(inputImageStream);
		//			image.ScaleToFit(100f, 100f);

		//			PdfDocument doc = pdfContentByte.PdfDocument;

		//			image.SetAbsolutePosition(40f, doc.PageSize.Height - 150f);
		//			pdfContentByte.AddImage(image);
		//			stamper.Close();
		//		}
		//	}
		//	else {
		//		outputPDFPath = pdfPath;
		//	}

		//	return outputPDFPath;
		//}

		protected void subReportsEventHandler(object sender, SubreportProcessingEventArgs e) {
			e.DataSources.Add(new ReportDataSource("DataSet1", invoice[0].invoiceLines));
		}
	}
}