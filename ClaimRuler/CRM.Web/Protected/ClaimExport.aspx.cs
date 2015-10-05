using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.Protected {
	public partial class ClaimExport : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			// access master page from nested master page
			Protected.ClaimRuler masterPage = Master.Master as Protected.ClaimRuler;

			// check user permission
			masterPage.checkPermission();
			
			if (!Page.IsPostBack) {
				bindData();
			}
		}

		private void bindData() {
			contractGrid.DataBind();
		}

		private void addReportToClaimDocument(string reportPath, int claimID) {
			string claimDocumentPath = null;
			string destinationFilePath = null;

			// get application path
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			ClaimDocument claimDocument = new ClaimDocument();
			claimDocument.ClaimID = claimID;
			claimDocument.IsPrint = true;
			claimDocument.Description = "Claim Report";
			claimDocument.DocumentDate = DateTime.Now;
			claimDocument.DocumentName = "Claim_Report.pdf";

			// report category
			claimDocument.DocumentCategoryID = 8;			

			claimDocument = ClaimDocumentManager.Save(claimDocument);

			claimDocumentPath = string.Format("{0}/ClaimDocuments/{1}/{2}", appPath, claimID, claimDocument.ClaimDocumentID);

			if (!Directory.Exists(claimDocumentPath))
				Directory.CreateDirectory(claimDocumentPath);

			destinationFilePath = claimDocumentPath + "/Claim_Report.pdf";

			System.IO.File.Copy(reportPath, destinationFilePath, true);

			// delete temp file
			//File.Delete(reportPath);
		}

		protected void btnExport_Click(object sender, EventArgs e) {
			ExportParameter exportParameter = new ExportParameter();

			int claimID = SessionHelper.getClaimID();
			int leadID = SessionHelper.getLeadId();

			//string filepath = Server.MapPath("~/Temp");
			string finalReportPath = null;

			exportParameter.isClaimLogo = cbxClaimLog.Checked;
			exportParameter.isCoverage = cbxCoverage.Checked;
			exportParameter.isDemographics = cbxDemographics.Checked;
			exportParameter.isDocuments = cbxDocuments.Checked;
			exportParameter.isPhotos = cbxPhotos.Checked;
			

			try {
				finalReportPath = ExportLeadHelper.exportLead(exportParameter, claimID);

				//generateSharedDocumentPage(claimID, leadID);

				emailDocumentLink(finalReportPath, txtEmailTo.Text.Trim(), claimID);

				addReportToClaimDocument(finalReportPath, claimID);

				lblMessage.Text = "Claim report has been printed and emailed successfully.";
				lblMessage.CssClass = "ok";
			}
			catch (Exception ex) {
				lblMessage.Text = ex.Message;
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}

			
		}

		

		protected void emailDocumentLink(string finalReportPath, string emailTo, int claimID) {
			int userID = Core.SessionHelper.getUserId();
			Claim claim = null;
			string password = null;
			int port = 0;
			string[] recipients = null;
			string subject = null;

			Leads lead = null;
			string encryptedQuery = null;
			StringBuilder emailBody = null;

			string itsgHost = ConfigurationManager.AppSettings["smtpHost"].ToString();
			string itsgHostPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
			string itsgEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
			string itsgEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();

			CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(userID);

			if (user != null && !string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.emailHost) && !string.IsNullOrEmpty(user.emailHostPort)) {
				claim = ClaimsManager.Get(claimID);

				if (claim != null) {
					lead = claim.LeadPolicy.Leads;
					
					if (string.IsNullOrEmpty(emailTo))
						recipients = new string[] { user.Email };
					else
						recipients = emailTo.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

					encryptedQuery = Core.SecurityManager.EncryptQueryString(claimID.ToString());
					
					emailBody = new StringBuilder();

					// .containerBox
					emailBody.Append("<div style=\"margin:auto;width:600px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");

					// header image
					emailBody.Append("<div style=\"padding:1px;\"><img src=\"http://app.claimruler.com/images/email_header_small.jpg\"></image></div>");
				
					// .paneContentInner
					emailBody.Append("<div style=\"margin: 20px;\">");

					subject = string.Format("{0} {1} wants to share {2} Adjuster Claim File with you", user.FirstName, user.LastName, lead.insuredName);

					emailBody.Append("<table cellpadding=\"0\" cellspacing=\"0\" style=\"border-left:1px #b9e1fb solid;border-right:1px #b9e1fb solid;border-bottom:1px #b9e1fb solid;border-top:1px #b9e1fb solid;border-radius:0px 0px 4px 4px\" border=\"0\" align=\"center\">");
					emailBody.Append("<tbody><tr><td colspan=\"3\" height=\"36\"></td></tr>");
					emailBody.Append("<tr><td width=\"36\"></td>");
					emailBody.Append("<td width=\"454\" style=\"font-size:14px;color:#444444;font-family:'Open Sans','Lucida Grande','Segoe UI',Arial,Verdana,'Lucida Sans Unicode',Tahoma,'Sans Serif';border-collapse:collapse\" align=\"left\" valign=\"top\">");


					emailBody.Append(string.Format("{0} {1} invited you to a Claim Ruler Software shared file called \"{2}\"",user.FirstName, user.LastName, lead.insuredName));

					string css = "border-radius:3px;border-left:1px #18639a solid;font-size:16px;border-bottom:1px #0f568b solid;padding:14px 7px 14px 7px;border-top:1px #2270ab solid;display:block;max-width:210px;font-family:'Open Sans','lucida grande','Segoe UI',arial,verdana,'lucida sans unicode',tahoma,sans-serif;text-align:center;background-image:-webkit-gradient(linear,0% 0%,0% 100%,from(rgb(55,163,235)),to(rgb(33,129,207)));width:210px;text-decoration:none;color:white;border-right:1px #18639a solid;font-weight:600;margin:0px auto 0px auto;background-color:#33a0e8";

					// user app siteURL for report.pdf
					emailBody.Append(string.Format("<br/><br/><center><a style=\"{0}\" href=\"{1}/Temp/{2}\">View Report</a></center>",
								css, ConfigurationManager.AppSettings["siteURL"], Path.GetFileName(finalReportPath)));

					emailBody.Append(string.Format("<br/><br/><center><a style=\"{0}\" href=\"{1}/Content/Public.aspx?q={2}\">View Files</a></center></td>",
								css, ConfigurationManager.AppSettings["siteURL"], encryptedQuery));

					emailBody.Append("<td width='36'></td></tr><tr><td colspan='3' height='36'></td></tr></tbody></table>");

					emailBody.Append("</div>");	// paneContentInner
					emailBody.Append("</div>");	// containerBox
		
					//int.TryParse(user.emailHostPort, out port);
					int.TryParse(itsgHostPort, out port);

					password = SecurityManager.Decrypt(user.emailPassword);				

					Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, itsgHost, port, itsgEmail, itsgEmailPassword);
				}
			}
			else {
				throw new Exception("Unable to export claim. Please verify user email settings."); 
			}
		}

		protected void btnPrintOnly_Click(object sender, EventArgs e) {
			ExportParameter exportParameter = new ExportParameter();

			int claimID = SessionHelper.getClaimID();
			int leadID = SessionHelper.getLeadId();

			//string filepath = Server.MapPath("~/Temp");
			string finalReportPath = null;

			exportParameter.isClaimLogo = cbxClaimLog.Checked;
			exportParameter.isCoverage = cbxCoverage.Checked;
			exportParameter.isDemographics = cbxDemographics.Checked;
			exportParameter.isDocuments = cbxDocuments.Checked;
			exportParameter.isPhotos = cbxPhotos.Checked;


			try {
				finalReportPath = ExportLeadHelper.exportLead(exportParameter, claimID);
				
				addReportToClaimDocument(finalReportPath, claimID);

				string siteURL = ConfigurationManager.AppSettings["siteURL"].ToString();

				string path = siteURL + "/Temp/" + Path.GetFileName(finalReportPath);

				string js = string.Format("window.open('{0}', '_blank','height=800,width=800,titlebar=yes,top=25,left=25, scrollbars=yes,status=no,resizable=yes');", path);

				ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "report", js, true);
				
			}
			catch (Exception ex) {
				lblMessage.Text = ex.Message;
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}
		}

		//private void generateDocumentList(TextWriter file, int leadID, int claimID) {
		//	string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

		//	List<LeadsDocument> documents = null;
		//	List<ClaimDocument> claimDocuments = null;
		//	List<string> pdfs = new List<string>();		

		//	documents = LeadsUploadManager.getLeadsDocumentForExportByLeadID(leadID);
		//	claimDocuments = ClaimDocumentManager.GetAll(claimID);

			

		//	// lead documents
		//	if (documents != null && documents.Count > 0) {

		//		List<string> leadPDFs = (from x in documents
		//							where x.DocumentName.Contains(".pdf")
		//							select string.Format("{0}/LeadsDocument/{1}/{2}/{3}", appPath, x.LeadId, x.LeadDocumentId, x.DocumentName)
		//					  ).ToList();

		//		file.WriteLine("<tr style=\"background-color:White;\">");

		//		foreach (string pdf in leadPDFs) {
		//			file.WriteLine("<td style=\"width:32px;\" align=\"center\">");
		//			pdfs.Add(pdf);
		//		}
		//	}

		//	// claim documents
		//	if (claimDocuments != null && claimDocuments.Count > 0) {

		//		List<string> claimPDFs = (from x in claimDocuments
		//							 where x.DocumentName.Contains(".pdf")
		//							 select string.Format("{0}/ClaimDocuments/{1}/{2}/{3}", appPath, x.ClaimID, x.ClaimDocumentID, x.DocumentName)
		//					  ).ToList();

		//		foreach (string pdf in claimPDFs) {
		//			pdfs.Add(pdf);
		//		}
		//	}

		//}

		
		//private string generateSharedDocumentPage(int claimID, int leadID) {
		//	string siteURL = ConfigurationManager.AppSettings["siteURL"].ToString();
		//	string url = null;
		//	string filename = null;
			
		//	filename = Guid.NewGuid().ToString() + ".html";

		//	TextWriter file = new StreamWriter(Server.MapPath("~/Public/") + filename);
		//	file.WriteLine("<html>");
		//	file.WriteLine("<meta http-equiv='Content-Type' content='text/html; charset=utf-8'>");
		//	file.WriteLine("<head>");
		//	file.WriteLine("<title>Claim Ruler - Industrial Strength Property Claim Management Software</title>");
		//	file.WriteLine(string.Format("<link type=\"text/css\" rel=\"stylesheet\" href=\"{0}/Css/ClaimRuler.css\"></link>",siteURL));
		//	file.WriteLine(string.Format("<link rel=\"shortcut icon\" href=\"/favicon.ico\"><link rel=\"icon\" href=\"{0}/favicon.ico\" type=\"image/x-icon\">", siteURL));
		//	file.WriteLine("</head>");
		//	file.WriteLine("<body>");
		//	file.WriteLine("<div class=\"page-title\">Shared Documents</div>");
		//	file.WriteLine(string.Format("<h2>{0}</h2>", SessionHelper.getClaimantName()));

		//	file.WriteLine("<table class=\"gridView\" rules=\"all\" id=\"content1_gvDocument\" style=\"height:100%;width:100%;border-collapse:collapse;\" align=\"Center\" border=\"1\" cellpadding=\"2\" cellspacing=\"0\">");
		//	file.WriteLine("<tbody>");
		//	file.WriteLine("<tr><th scope=\"col\">&nbsp;</th><th scope=\"col\">Document Name</th><th scope=\"col\">Description</th></tr>");

		//	file.WriteLine("<tr style=\"background-color:White;\">");

		//	//generateDocumentList(file, claimID, leadID);

		//	file.WriteLine("</table>");


		//	file.WriteLine("</body>");
		//	file.WriteLine("</html>");

		//	file.Flush();
		//	file.Close();

		//	return filename;
		//}

		
	}
}