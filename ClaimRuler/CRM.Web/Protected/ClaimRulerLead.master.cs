using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Infragistics.Web.UI.NavigationControls;

using CRM.Data;

namespace CRM.Web.Protected {
	public partial class ClaimRulerLead : ClaimRulerLeadBase {
		public int LeadID {
			get { return Core.SessionHelper.getLeadId(); }
		}

		protected void Page_Load(object sender, EventArgs e) {

		}

		protected void generatePhotoReport() {
			bool isSuccess = true;
			string ErrorMessage = string.Empty;

			// 2013-03-12 tortega
			Page.Validate();
			if (!Page.IsValid)
				return;

			int LeadId = Core.SessionHelper.getLeadId();

			if (LeadId > 0) {

				try {						
					string filename1 = CRM.Web.CreatePDF.CreateAndGetPDF(LeadId, Request.PhysicalApplicationPath + "PDF\\", out ErrorMessage);


					//LeadReportGenerateLog objLeadReportGenerateLog = new LeadReportGenerateLog();
					//objLeadReportGenerateLog.LeadId = Convert.ToInt32(ViewState["LeadIds"].ToString());
					//objLeadReportGenerateLog.GenerateDate = DateTime.Now;
					//objLeadReportGenerateLog.Generatedby = Convert.ToInt32(Session["UserId"]);
					//LeadReportLogManager.Save(objLeadReportGenerateLog);

					// 2013-03-21 tortega - moved out of try/catch - causes exception
					//OpenNewWindow(LeadId);
				}
				catch (Exception ex) {					
					isSuccess = false;
				}

				// show report when success
				if (isSuccess)
					OpenNewWindow(LeadId);
			}
		}

		public void OpenNewWindow(int LeadId) {
			string url = Request.PhysicalApplicationPath + "PDF\\" + LeadId + ".pdf";
			if (System.IO.File.Exists(url)) {

				string FileName = LeadId + ".pdf";
				Response.ContentType = "Application/pdf";
				Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + " ");
				Response.TransmitFile(url);
				Response.End();
			}
		}

		protected void webTaskPanel_ItemClick(object sender, ExplorerBarItemClickEventArgs e) {
			switch (e.Item.Value) {
				case "1":			// return to claim edit
					int leadID = Core.SessionHelper.getLeadId();
					if (leadID > 0) {
						Response.Redirect("~/Protected/NewLead.aspx?id=" + leadID.ToString());
					}
					break;

				case "2":
					generatePhotoReport();
					break;

				default:
					break;
			}
		}

		

	}
}