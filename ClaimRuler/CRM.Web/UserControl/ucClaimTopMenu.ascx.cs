using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Infragistics.Web.UI.NavigationControls;
using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.UserControl {
	public partial class ucClaimTopMenu : System.Web.UI.UserControl {
		int clientID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			bindMenu();
		}

		protected void bindMenu() {
			clientID = Core.SessionHelper.getClientId();
			DataMenuItem letterSubMenuItem = null;

			List<ClientLetterTemplate> letterTemplates = LetterTemplateManager.GetAll(clientID);

			// get letters menu
			DataMenuItem letterMenu = topMenu.Items[2].Items[0];

			if (letterTemplates != null && letterTemplates.Count > 0) {
				letterMenu.Items.Clear();

				foreach (ClientLetterTemplate letterTemplate in letterTemplates) {
					letterSubMenuItem = new DataMenuItem();
					letterSubMenuItem.ImageUrl = "~/Images/letter.png";
					letterSubMenuItem.Key = "letter";

					letterSubMenuItem.Text = letterTemplate.Description;
					letterSubMenuItem.Value = letterTemplate.TemplateID.ToString();

					letterMenu.Items.Add(letterSubMenuItem);

				}
			}

		}
		protected void WebDataMenu1_ItemClick(object sender, Infragistics.Web.UI.NavigationControls.DataMenuItemEventArgs e) {
			switch (e.Item.Value) {
				case "1":
					break;

				case "2":
					generatePhotoReport();
					break;

				default:
					if (e.Item.Key == "letter") {
						// mail merge letter
						mergeLetter(Convert.ToInt32(e.Item.Value));
					}
					break;
			}
		}

		protected void mergeLetter(int templateID) {
			string finalDocumentPath = null;
			string clientFileName = null;
			int claimID = 0;
			ClientLetterTemplate letterTemplate = null;

			clientID = Core.SessionHelper.getClientId();

			claimID = Core.SessionHelper.getClaimID();
							
			try {
				letterTemplate = LetterTemplateManager.Get(templateID);

				finalDocumentPath = Core.MergeDocumentHelper.mergeLetterTemplate(clientID, letterTemplate, claimID);

				Core.MergeDocumentHelper.addLetterToDocumentList(claimID, finalDocumentPath, letterTemplate.Description);

				clientFileName = Core.SessionHelper.getClaimantName() + ".doc";
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
			}

			if (!string.IsNullOrEmpty(finalDocumentPath) && !string.IsNullOrEmpty(clientFileName)) {
				Core.ReportHelper.renderToBrowser(finalDocumentPath, clientFileName);

				Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
			}

		}

		protected void generatePhotoReport() {
			bool isSuccess = true;
			string ErrorMessage = string.Empty;

			
			int claimID = Core.SessionHelper.getClaimID();

			if (claimID > 0) {

				try {
					string filename1 = CRM.Web.CreatePDF.CreateAndGetPDF(claimID, Request.PhysicalApplicationPath + "PDF\\", out ErrorMessage);


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
					OpenNewWindow(claimID);
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

	}
}