using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CRM.Web.Utilities;
using CRM.Data;
using CRM.Data.Account;
using CRM.Core;

using Atom.Core;
using Atom.Core.Collections;

using Infragistics.WebUI.WebHtmlEditor;
using CRM.Data.Entities;

namespace CRM.Web.Protected {
	public partial class LeadImportEmail : System.Web.UI.Page {
		int clientID = 0;
		int userID = 0;
		int leadID = 0;
		int claimID = 0;

		protected void Page_Load(object sender, EventArgs e) {

			// access master page from nested master page
			Protected.ClaimRuler masterPage = Master.Master as Protected.ClaimRuler;

			// check user permission
			masterPage.checkPermission();

			clientID = SessionHelper.getClientId();
			userID = SessionHelper.getUserId();
			leadID = SessionHelper.getLeadId();
			claimID = SessionHelper.getClaimID();

			if (!Page.IsPostBack) {
				bindData();
			}
		}

		protected void btnImportEmail_Click(object sender, EventArgs e) {

			ClaimComment comment = null;
			DateTime date = DateTime.MinValue;

			try {
				foreach (GridViewRow row in gvMails.Rows) {
					if (row.RowType == DataControlRowType.DataRow) {
						CheckBox cbxImport = row.FindControl("cbxImport") as CheckBox;

						if (cbxImport != null && cbxImport.Checked) {
							Label receivedDate = row.FindControl("ReceivedDate") as Label;

							comment = new ClaimComment();

							comment.ClaimID = this.claimID;
							comment.IsActive = true;

							if (receivedDate != null && !string.IsNullOrEmpty(receivedDate.Text)) {
								DateTime.TryParse(receivedDate.Text, out date);
								comment.CommentDate = date;
							}

							comment.UserId = this.userID;

							WebHtmlEditor txtContents = row.FindControl("txtContents") as WebHtmlEditor;
							if (txtContents != null && !string.IsNullOrEmpty(txtContents.Text)) {
								comment.CommentText = txtContents.Text.Trim();
								
							}

							ClaimCommentManager.Save(comment);
						}
					}
				}
				lblMessage.Text = "Selected emails have been imported successfully.";
				lblMessage.CssClass = "ok";
			}
			catch (Exception ex) {
				lblMessage.Text = "Email import failure.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}
		}

		protected void bindData() {
			List<Email> emails = null;
			Client client = null;

			//default imap host
			string imapHost = "imap.gmail.com"; ;
			int port = 993;
			bool useSSL = true;


			// get current user						
            CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(userID);

			// get client associated with user
			client = ClientManager.Get(clientID);

			if (user != null && client != null && user.Email != null && user.emailPassword != null) {
				// show email where we are trying to import
				lblEmail.Text = string.Format("{0}/export", user.Email);

				string inboxPath = Server.MapPath("~/inbox");

				if (!string.IsNullOrEmpty(client.imapHost)) {
					imapHost = client.imapHost;
					port = client.imapHostPort ?? 993;
					useSSL = client.imapHostUseSSL ?? true;
				}

				try {
					emails = GCalendarHelper.getGMailEmail(imapHost, port, useSSL, user.Email, SecurityManager.Decrypt(user.emailPassword));
				}
				catch (Exception ex) {
					lblMessage.Text = "Unable to read emails. Please verify IMAP settings.";
					lblMessage.CssClass = "error";
				}
			}

			gvMails.DataSource = emails;
			gvMails.DataBind();
		}



		protected string formatText(string bodytext) {
			string[] lines = bodytext.Split('\n');
			StringBuilder sb = new StringBuilder();

			if (lines.Length > 0) {
				foreach (string line in lines) {
					sb.Append("<div>" + line.Replace("\r", "") + "<div/>");
				}
			}

			return sb.ToString();
		}

		protected string removeSpecialCharacters(string bodytext) {
			return bodytext.Replace("<", "").Replace("@", "").Replace(">", "");
		}
	}
}