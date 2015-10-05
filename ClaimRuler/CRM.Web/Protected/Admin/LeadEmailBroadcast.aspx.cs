using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;


namespace CRM.Web.Protected.Admin {
	public partial class LeadEmailBroadcast : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			if (!Page.IsPostBack) {				
			}

		}


		protected void btnSend_Click(object sender, EventArgs e) {			
			string[] attachments = null;
			int clientID = 0;
			string[] recipient = null;
			List<string> emails = null;
			CRM.Data.Entities.SecUser user = null;
			string decryptedPassword = null;

			string fromEmail = null;
			string subject = txtEmailSubject.Text.Trim();
			string bodyText = txtEmailText.Text.Trim();

			user = SecUserManager.GetByUserId(SessionHelper.getUserId());

			if (user == null) {
				return;
			}

			int roleID = SessionHelper.getUserRoleId();

			if (roleID == (int)UserRole.Client || roleID == (int)UserRole.SiteAdministrator) {
				clientID = SessionHelper.getClientId();

				emails = LeadsManager.GetLeadEmails(clientID);
			}
			else
				emails = LeadsManager.GetLeadEmails();

			fromEmail = user.Email;
			txtEmailText.Text += "\n\n" + user.emailSignature ?? "";

			decryptedPassword = Core.SecurityManager.Decrypt(user.emailPassword);
			
			if (fileUpload.HasFile && fileUpload.PostedFile.ContentLength > 0 ) {						
				attachments = new string[] { Path.GetFullPath(fileUpload.PostedFile.FileName) };								
			}

			if (emails != null && emails.Count > 0) {
				try {
					foreach (string email in emails) {
						recipient = new string[] { email };

						Core.EmailHelper.sendEmail(fromEmail, recipient, null, subject, bodyText, attachments, user.Email, decryptedPassword);						
					}
					lblMessage.Text = "Email broadcast complete.";
					lblMessage.CssClass = "ok";
				}
				catch (Exception ex) {
					lblMessage.Text = "Email broadcast failed.";
					lblMessage.CssClass = "error";

				}
			}
		}
		
	}
}