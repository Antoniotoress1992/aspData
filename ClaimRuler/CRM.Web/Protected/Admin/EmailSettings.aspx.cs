using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;
using CRM.Web.Utilities;

namespace CRM.Web.Protected.Admin {
	public partial class EmailSettings : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			int clientID = SessionHelper.getClientId();

			Form.DefaultButton = btnSave.UniqueID;

			// set location where client can upload pictures for signature
			txtEmailSignature.UploadedFilesDirectory = Server.MapPath("~/clientLogo/" + clientID.ToString());

			if (!Page.IsPostBack) {
				FillForm();
			}

		}

		protected void FillForm() {			
			int userID = 0;

			if (Session["UserId"] != null && int.TryParse(Session["UserId"].ToString(), out userID) && userID > 0) {
				//Client client = ClientManager.Get(clientID);
                CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(userID);

				if (user != null) {
					txtEmailHost.Text = user.emailHost ?? "";

					txtEmail.Text = user.Email ?? "";

					txtHostPostNumber.Text = user.emailHostPort ?? "";

					txtEmailSignature.Text = user.emailSignature ?? "";

					cbxSSL.Checked = user.isSSL ?? true;

					if (!string.IsNullOrEmpty(user.emailPassword)) {
						txtEmailPassword.Attributes.Add("value", "pass");						
					}
				}
			}

		}

		protected void btnSave_Click(object sender, EventArgs e) {		
			int userID = 0;

			if (Session["UserId"] != null && int.TryParse(Session["UserId"].ToString(), out userID) && userID > 0) {
                CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(userID);

				if (user != null) {
					user.emailHost = txtEmailHost.Text.Trim();

					user.emailHostPort = txtHostPostNumber.Text.Trim();

					user.emailSignature = txtEmailSignature.Text.Trim();

					user.Email = txtEmail.Text.Trim();

					user.isSSL = cbxSSL.Checked;

					if (!string.IsNullOrEmpty(txtEmailPassword.Text) && txtEmailPassword.Text != "pass")
						user.emailPassword = SecurityManager.Encrypt(txtEmailPassword.Text);

					lblMessage.Visible = true;

					try {
						SecUserManager.Save(user);

						lblMessage.Text = "Changes saved successfully.";
					}
					catch (Exception ex) {
						lblMessage.Text = "Error saving changes.";
					}
				}
			}

		}

		protected void btnCancel_Click(object sender, EventArgs e) {
		}
	}
}