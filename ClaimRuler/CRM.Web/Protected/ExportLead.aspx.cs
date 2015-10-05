using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;

using CRM.Web.Utilities;
using CRM.Data.Entities;

namespace CRM.Web.Protected {
	public partial class ExportLead : System.Web.UI.Page {


		protected void Page_Load(object sender, EventArgs e) {
			// access master page from nested master page
			Protected.ClaimRuler masterPage = Master.Master as Protected.ClaimRuler;

			// check user permission
			masterPage.checkPermission();

			if (!Page.IsPostBack) {
				bindData();
			}
		}

		protected void bindData() {
			int leadID = SessionHelper.getLeadId();
			Leads lead = null;

			// get claimant name
			lead = LeadsManager.Get(leadID);
			if (lead != null) {
				if (lead.LeadPolicy != null && lead.LeadPolicy.Count > 0) {
					Core.CollectionManager.FillCollection(ddlPolicyType, "policyTypeID", "policyTypeDescription", LeadPolicyTypeManager.GetCurrentPolicy(lead), false);
				}
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			int leadID = Core.SessionHelper.getLeadId();
			if (leadID > 0) {
				Response.Redirect("~/Protected/NewLead.aspx?id=" + leadID.ToString());
			}
		}

		protected void btnExport_Click(object sender, EventArgs e) {
			ExportParameter exportParameter = new ExportParameter();

			int leadID = Convert.ToInt32(Session["LeadIds"]);

			//string filepath = Server.MapPath("~/Temp");
			string finalReportPath = null;

			exportParameter.isClaimLogo = cbxClaimLog.Checked;
			exportParameter.isCoverage = cbxCoverage.Checked;
			exportParameter.isDemographics = cbxDemographics.Checked;
			exportParameter.isDocuments = cbxDocuments.Checked;
			exportParameter.isPhotos = cbxPhotos.Checked;


			exportParameter.policyTypeID = Convert.ToInt32(ddlPolicyType.SelectedValue);

			try {
				finalReportPath = ExportLeadHelper.exportLead(exportParameter, leadID);

				emailDocumentLink(finalReportPath, txtEmailTo.Text.Trim());

				lblMessage.Text = "Claim has been successfully exported.";
				lblMessage.CssClass = "ok";
			}
			catch (Exception ex) {
				lblMessage.Text = "Error while exporting claim.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}

			//renderToClient(finalReportPath);
		}


		protected void emailDocumentLink(string finalReportPath, string emailTo) {
			int userID = Core.SessionHelper.getUserId();

			string bodyText = null;
			int leadID = Convert.ToInt32(Session["LeadIds"]);
			string password = null;
			int port = 0;
			string[] recipients = null;
			string subject = null;

			Leads lead = null;
			string encryptedQuery = null;
			System.Text.StringBuilder html = new System.Text.StringBuilder();

			CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(userID);
			if (user != null && user.Email != null && user.emailHost != null && user.emailHostPort != null) {
				if (string.IsNullOrEmpty(emailTo))
					recipients = new string[] { user.Email };
				else
					recipients = emailTo.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

				lead = LeadsManager.GetByLeadId(leadID);

				if (lead != null) {
					encryptedQuery = Core.SecurityManager.EncryptQueryString(leadID.ToString());


					subject = string.Format("{0} {1} wants to share {2} {3} Public Adjuster Claim File with you",
										user.FirstName, user.LastName, lead.ClaimantFirstName, lead.ClaimantLastName);

					html.Append("<table cellpadding=\"0\" cellspacing=\"0\" style=\"border-left:1px #b9e1fb solid;border-right:1px #b9e1fb solid;border-bottom:1px #b9e1fb solid;border-top:1px #b9e1fb solid;border-radius:0px 0px 4px 4px\" border=\"0\" align=\"center\">");
					html.Append("<tbody><tr><td colspan=\"3\" height=\"36\"></td></tr>");
					html.Append("<tr><td width=\"36\"></td>");
					html.Append("<td width=\"454\" style=\"font-size:14px;color:#444444;font-family:'Open Sans','Lucida Grande','Segoe UI',Arial,Verdana,'Lucida Sans Unicode',Tahoma,'Sans Serif';border-collapse:collapse\" align=\"left\" valign=\"top\">");

					
					html.Append(string.Format("{0} {1} invited you to a Claim Ruler Software shared file called \"{2} {3}.\"",
										user.FirstName, user.LastName, lead.ClaimantFirstName, lead.ClaimantLastName));

					string css = "border-radius:3px;border-left:1px #18639a solid;font-size:16px;border-bottom:1px #0f568b solid;padding:14px 7px 14px 7px;border-top:1px #2270ab solid;display:block;max-width:210px;font-family:'Open Sans','lucida grande','Segoe UI',arial,verdana,'lucida sans unicode',tahoma,sans-serif;text-align:center;background-image:-webkit-gradient(linear,0% 0%,0% 100%,from(rgb(55,163,235)),to(rgb(33,129,207)));width:210px;text-decoration:none;color:white;border-right:1px #18639a solid;font-weight:600;margin:0px auto 0px auto;background-color:#33a0e8";
				

					html.Append(string.Format("<br/><br/><center><a style=\"{0}\" href=\"{1}/Temp/{2}.pdf\">View Report</a></center>",
								css, ConfigurationManager.AppSettings["appURL"], leadID));

					html.Append(string.Format("<br/><br/><center><a style=\"{0}\" href=\"{1}/Content/SharedDocuments.aspx?q={2}\">View Files</a></center></td>",
								css, ConfigurationManager.AppSettings["appURL"], encryptedQuery));

					html.Append("<td width='36'></td></tr><tr><td colspan='3' height='36'></td></tr></tbody></table>");

					port = Convert.ToInt32(user.emailHostPort);

					password = SecurityManager.Decrypt(user.emailPassword);

					bodyText = html.ToString();

					Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, bodyText, null, user.emailHost, port, user.Email, password, user.isSSL ?? false);
				}
			}

		}

		protected void renderToClient(string finalReportPath) {
			if (File.Exists(finalReportPath)) {
				// filename shown to client
				string FileName = "ExportReport.pdf";

				Response.ContentType = "Application/pdf";
				Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + " ");
				Response.TransmitFile(finalReportPath);
				Response.End();
			}
		}
	}
}