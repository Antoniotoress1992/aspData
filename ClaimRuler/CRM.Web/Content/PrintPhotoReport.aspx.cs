using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.Content {
	public partial class PrintPhotoReport : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			GenerateReport();
		}

		protected void GenerateReport() {
			int LeadId = 0;
			string ErrorMessage;

			if (Session["LeadIds"] != null && Convert.ToInt32(Session["LeadIds"]) > 0) {
				try {

					LeadId = Convert.ToInt32(Session["LeadIds"]);

					string filename1 = CreatePDF.CreateAndGetPDF(LeadId, Request.PhysicalApplicationPath + "PDF\\", out ErrorMessage);


					LeadReportGenerateLog objLeadReportGenerateLog = new LeadReportGenerateLog();
					objLeadReportGenerateLog.LeadId = LeadId;
					objLeadReportGenerateLog.GenerateDate = DateTime.Now;
					objLeadReportGenerateLog.Generatedby = Convert.ToInt32(Session["UserId"]);
					LeadReportLogManager.Save(objLeadReportGenerateLog);

				}
				catch (Exception ex) {					
				}

				OpenNewWindow(LeadId);
			}
		}

		public void OpenNewWindow(int LeadId) {
			string url = Request.PhysicalApplicationPath + "PDF/" + LeadId + ".pdf";
			if (File.Exists(url)) {

				string FileName = LeadId + ".pdf";
				Response.ContentType = "Application/pdf";
				Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + " ");
				Response.TransmitFile(url);
				Response.End();
			}
		}
	
	}
}