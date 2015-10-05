using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Core;
using CRM.Data.Account;
using CRM.Data;
using CRM.Core;
using CRM.Web.Utilities;
using System.Transactions;

using Infragistics.WebUI.WebDataInput;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {


	public partial class ucNewUser : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;
		int roleID = 0;
		int clientID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			bool isAdmin = false;

			roleID = Core.SessionHelper.getUserRoleId();

			clientID = Core.SessionHelper.getClientId();

			masterPage = this.Page.Master as Protected.ClaimRuler;
			this.Page.Form.DefaultButton = this.btnSave.UniqueID;

			isAdmin = (Session["UserName"] != null && Session["UserName"].ToString().ToLower() == "admin");

			if (!IsPostBack) {

				FillForm();


				if (isAdmin) {
					bindClients();
				}
			}
			

			// tortega - show clients 
			if (isAdmin) {
				tr_client.Visible = true;
			}

		}

		private void bindRole() 
        {
			List<SecRole> roles = null;

			if (roleID == (int)UserRole.Administrator)
				roles = SecRoleManager.GetAll();
			else
				roles = SecRoleManager.GetRolesManagedByClient(clientID);

			CollectionManager.FillCollection(ddlRole, "RoleId", "RoleName", roles);
		}

		private void bindClients() {

			CollectionManager.FillCollection(ddlClient, "ClientId", "BusinessName", ClientManager.GetAll());
		}


		private void FillForm() {
			string photoPath = null;
			int userId = Session["UID"] != null ? Convert.ToInt32(Session["UID"]) : 0;
			int roleID = SessionHelper.getUserRoleId();
			
			if (userId > 0) {
				hfUserId.Value = userId.ToString();
				lblheading.Text = "Edit User Details";
				chkPassword.Enabled = true;
				btnResetPassword.Visible = true;
			}
			else {
				lblheading.Text = "New User";
				chkPassword.Visible = false;
				btnResetPassword.Visible = false;
			}


			bindRole();

			if (userId > 0) {
				
				// update user
                CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(Convert.ToInt32(userId));

				if (user.UserId > 0 && user.UserName != null) {
					txtFirstName.Text = user.FirstName;
					txtLastName.Text = user.LastName;
					txtEmail.Text = user.Email;
					txtUserName.Text = user.UserName;
					txtPassWord.Text = SecurityManager.Decrypt(user.Password);
					ViewState["Password"] = txtPassWord.Text;
					txtPassWord.Attributes.Add("value", "12345");
					txtPassWord.Enabled = false;
					txtRePassWord.Text = SecurityManager.Decrypt(user.Password);
					txtRePassWord.Attributes.Add("value", "12345");
					txtRePassWord.Enabled = false;

					cbxViewAllClaims.Checked = user.isViewAllClaims ?? false;

					// disable role DDL when client is editing his own client id
					if (roleID == (int)UserRole.Client && user.RoleId == (int)UserRole.Client) {
						ddlRole.Enabled = false;
						ddlRole.Items.Add(new ListItem("Client", user.RoleId.ToString()));
					}
					else
						ddlRole.Enabled = true;

					ddlRole.SelectedValue = user.RoleId.ToString();

					if (user.Status.ToString() == "True") {
						ddlStatus.SelectedValue = "1";
					}
					else {
						ddlStatus.SelectedValue = "0";
					}

					if (user.ClientID != null) {
						ddlClient.SelectedValue = user.ClientID.ToString();
					}

					userPhoto.ImageUrl = Core.Common.getUserPhotoURL(user.UserId);

					//txtEmailSignature.Text = user.emailSignature;					
				}
				if (user.RoleId == 1) {
                    CRM.Data.Entities.SecUser loginUser = SecUserManager.GetByUserId(Convert.ToInt32(Session["UserId"]));


					ddlStatus.Enabled = false;
					txtUserName.Enabled = false;

				}
			}

		}

		protected void btnSave_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblMessage.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Visible = false;
			lblMessage.Visible = false;

			bool isnew = false;
			bool isError = false;
			CRM.Data.Entities.SecUser user = null;

			Page.Validate("register");
			if (!Page.IsValid)
				return;

			// 2013-08-06 tortega
			int clientID = Core.SessionHelper.getClientId();
			int roleID = Core.SessionHelper.getUserRoleId();

			try {
				using (TransactionScope scope = new TransactionScope()) {
					if (hfUserId.Value == "0") {
						isnew = true;
						
						user = new CRM.Data.Entities.SecUser();

						user.ClientID = clientID;
					}
					else {
						user = SecUserManager.GetByUserId(Convert.ToInt32(hfUserId.Value));
					}

					// ** Add User  ** //
					if (isnew) {
						bool userExist = SecUserManager.IsUserNameExist(txtUserName.Text.Trim());
						//bool emailExist = SecUserManager.IsEmailExist(txtEmail.Text.Trim());

						// 2013-08-06 tortega
						if (clientID > 0 && ClientManager.UsersLimitReached(clientID)) {
							throw new Exception("Maximum number of users reached.");
						}

						if (userExist) {
							throw new Exception("User Name already exists.");
						}						
					}

					user.FirstName = txtFirstName.Text.Trim();
					user.LastName = txtLastName.Text.Trim();
					user.Email = txtEmail.Text;
					user.UserName = txtUserName.Text.Trim();
					//user.Password = SecurityManager.Encrypt(txtPassWord.Text);

					user.RoleId = Convert.ToInt32(ddlRole.SelectedValue);
					user.Status = ddlStatus.SelectedValue == "1" ? true : false;
					user.Blocked = false;

					user.isViewAllClaims = cbxViewAllClaims.Checked;

					if (chkPassword.Checked == true || isnew) 
                    {
						user.Password = SecurityManager.Encrypt(txtPassWord.Text);
					}

					user.UserName = txtUserName.Text.Trim();
					user.FirstName = txtFirstName.Text.Trim();
					user.LastName = txtLastName.Text.Trim();
					user.Email = txtEmail.Text;
					user.Status = ddlStatus.SelectedValue == "1" ? true : false;
					user.Blocked = user.Blocked;
					user.RoleId = Convert.ToInt32(ddlRole.SelectedValue);

					user = SecUserManager.Save(user);

					//clearControls();
					scope.Complete();

					if (isnew)
						lblSave.Text = "User account has been created. User credentials sent via email.";
					else
						lblSave.Text = "User account saved.";

					lblSave.Visible = true;
					btnResetPassword.Visible = true;
				}

			}
			catch (Exception ex) {
				isError = true;
				lblError.Text = ex.Message;
				lblError.Visible = true;

				Core.EmailHelper.emailError(ex);
			}
			finally {
				if (isnew && isError == false) {
					// email user account info
					Core.EmailHelper.emailUserCredentials(user);
				}				
			}
		}



		private void clearControls() {

			txtFirstName.Text = string.Empty;
			txtLastName.Text = string.Empty;
			txtEmail.Text = string.Empty;
			txtUserName.Text = string.Empty;
			txtPassWord.Text = string.Empty;
			txtRePassWord.Text = string.Empty;

			ddlRole.SelectedIndex = 0;

			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			hfUserId.Value = "0";
		}


		protected void btnCancel_Click(object sender, EventArgs e) {
			Session["UID"] = null;
			Response.Redirect("~/protected/admin/UserList.aspx");
		}

		protected void btnResetPassword_Click(object sender, EventArgs e) {
			// random password
			string password = Guid.NewGuid().ToString().Substring(0, 8);

			try {
                CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(Convert.ToInt32(hfUserId.Value));

				// encrypt password
				user.Password = Core.SecurityManager.Encrypt(password);

				user = SecUserManager.Save(user);

				Core.EmailHelper.emailUserPasswordReset(user);

				lblSave.Text = "User password has been reset. New password sent to user via email.";
				
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);

				lblSave.Text = "Unable to reset password.";
				lblSave.Visible = true;
			}
		}

		//private void emailUserCredentials(SecUser user) {
		//	string crsupportEmail = ConfigurationManager.AppSettings["userID"].ToString();
		//	string crsupportEmailPassword = ConfigurationManager.AppSettings["Password"].ToString();

		//	if (user != null && !string.IsNullOrEmpty(user.Email)) {
		//		string[] recipients = new string[] { user.Email };

				
		//		string password = SecurityManager.Decrypt(user.Password);
				

		//		string subject = "ClaimRuler Access Account";

		//		string appURL = ConfigurationManager.AppSettings["appURL"].ToString();

		//		string bodyText = "<p>Congratulations, " + user.FirstName + "!</p>" +
		//						"<p>You have been granted access to ClaimRuler.</p>" +
		//						"<p>User name: <b>" + user.UserName + "</b></p>" +
		//						"<p>Password: <b>" + password + "</b></p>" +
		//						string.Format("<a target='_blank' href='{0}/login.aspx'>Claim Ruler - Industrial Strength Property Claim Management Software</a>", appURL);

		//		Core.EmailHelper.sendEmail(crsupportEmail, recipients, null, subject, bodyText, null, crsupportEmail, crsupportEmailPassword);
		//	}
		//}
	}
}