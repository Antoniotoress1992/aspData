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
using CRM.Core;
using CRM.Data.Entities;

namespace CRM.Web.Protected {
	public partial class MergeTemplateLetter : System.Web.UI.Page {
		int clientID = 0;
		int leadID = 0;
		int claimID = 0;

		string appPath = null;

		protected void Page_Load(object sender, EventArgs e) {
			// access master page from nested master page
			Protected.ClaimRuler masterPage = Master.Master as Protected.ClaimRuler;

			// check user permission
			//masterPage.checkPermission();


			clientID = Core.SessionHelper.getClientId();
			leadID = Core.SessionHelper.getLeadId(); 
			claimID = Core.SessionHelper.getClaimID();

			appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			if (!Page.IsPostBack) {
				bindData();
			}
		}

		protected void bindData() {
			List<ClientLetterTemplate> templates = LetterTemplateManager.GetAll(clientID);

			gvLetter.DataSource = templates;
			gvLetter.DataBind();
		}

		
		//protected void addLetterToDocumentList(int claimID, string documentPath, string documentDescription) {
		//	string directoryPath = null;

		//	ClaimDocument claimDocument = new ClaimDocument();
		//	string destinationPath = null;

		//	if (!File.Exists(documentPath))
		//		return;

		//	string ext = System.IO.Path.GetExtension(documentPath);

		//	claimDocument.ClaimID = claimID;
		//	claimDocument.Description = documentDescription;
		//	claimDocument.DocumentName = Utilities.ReportHelper.sanatizeFileName(documentDescription) + ext;

		//	claimDocument.DocumentDate = DateTime.Now;

		//	claimDocument = ClaimDocumentManager.Save(claimDocument);

		//	if (claimDocument.ClaimDocumentID > 0) {
		//		directoryPath = string.Format("{0}/ClaimDocuments/{1}/{2}", appPath, claimID, claimDocument.ClaimDocumentID);

		//		if (!Directory.Exists(directoryPath))
		//			Directory.CreateDirectory(directoryPath);

		//		destinationPath = directoryPath + "/" + claimDocument.DocumentName;

		//		File.Copy(documentPath, destinationPath);
		//	}
		//}


		protected void gvLetter_RowCommand(object sender, GridViewCommandEventArgs e) {
			int templateID = 0;
			string finalDocumentPath = null;
			string clientFileName = null;
			ClientLetterTemplate letterTemplate = null;

			if (e.CommandName == "DoPrint") {
				templateID = Convert.ToInt32(e.CommandArgument);

				try {
					letterTemplate = LetterTemplateManager.Get(templateID);

					finalDocumentPath = Core.MergeDocumentHelper.mergeLetterTemplate(clientID, letterTemplate, this.claimID);

					Core.MergeDocumentHelper.addLetterToDocumentList(this.claimID, finalDocumentPath, letterTemplate.Description);

					clientFileName = SessionHelper.getClaimantName() + ".doc";					
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}

				if (!string.IsNullOrEmpty(finalDocumentPath) && !string.IsNullOrEmpty(clientFileName))
					Core.ReportHelper.renderToBrowser(finalDocumentPath, clientFileName);
			}
		}

	}
}