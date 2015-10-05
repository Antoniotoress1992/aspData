using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Web.Utilities;
using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucCarrierContact : System.Web.UI.UserControl {
		private int carrierID {
			get {
				return Session["CarrierID"] != null ? Convert.ToInt32(Session["CarrierID"]) : 0;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
		}

		public void bindData(int lcarrierID) {
			List<CarrierContact> contacts = CarrierContactManager.GetAll(lcarrierID);

			showContactGrid();

			gvContacts.DataSource = contacts;
			gvContacts.DataBind();
		}
		private void bindContactTypes() {
			int clientID = Core.SessionHelper.getClientId();

            List<CRM.Data.Entities.LeadContactType> leadContactTypes =LeadContactTypeManager.GetAll(clientID);

			CollectionManager.FillCollection(ddlContactType, "ID", "Description", leadContactTypes);
		}

		protected void bindStates() {
			List<StateMaster> states = State.GetAll();

			CollectionManager.FillCollection(ddlState, "StateId", "StateName", states);
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			Page.Validate("Contact");
			if (!Page.IsValid)
				return;

			saveContact();
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			showContactGrid();

			bindData(carrierID);
		}

		private void clearContactFields() {
			this.txtFirstName.Text = string.Empty;
			this.txtLastName.Text = string.Empty;
			this.txtContactEmail.Text = string.Empty;
			this.txtContactPhone.Text = string.Empty;
			this.txtMobilePhone.Text = string.Empty;
			this.txtContactTile.Text = string.Empty;
			this.txtDepartmentName.Text = string.Empty;
			txtContactFax.Text = string.Empty;

			ddlContactType.SelectedIndex = -1;
			ddlCity.SelectedIndex = -1;
			ddlLossZip.SelectedIndex = -1;

			txtCompanyName.Text = string.Empty;
			txtAddress1.Text = string.Empty;
			txtAddress2.Text = string.Empty;
			cbxPrimary.Checked = false;
		}

		protected void gvContacts_RowCommand(object sender, GridViewCommandEventArgs e) {
			int clientID = SessionHelper.getClientId();
			Contact contact = null;			
			int id = string.IsNullOrEmpty(e.CommandArgument.ToString()) ? 0 : Convert.ToInt32(e.CommandArgument);
			int stateID = 0;
			int cityID = 0;
			int zipCodeID =  0;

			lblMessage.Text = string.Empty;
			lblMessage.CssClass = string.Empty;

			if (e.CommandName == "DoEdit") {
				ViewState["ContactID"] = id.ToString();

				contact = ContactManager.Get(id);

				if (contact != null) {
					showContactPanel();

					bindContactTypes();

					bindStates();

					if (contact.UserID == null) {
						btnShowCreateAccount.Visible = true;
					}
					else {
						btnShowCreateAccount.Visible = false;

						// show current user account for contact
						pnlUserAccount.Visible = true;

						txtUserName.Text = contact.SecUser.UserName;
						txtUserName.Enabled = false;
						
						ddlClientRoles.Enabled = false;

						// bind client roles
						CollectionManager.FillCollection(ddlClientRoles, "RoleId", "RoleName", SecRoleManager.GetRolesManagedByClient(clientID));

						try {
							ddlClientRoles.SelectedValue = (contact.SecUser.RoleId).ToString();
						}
						catch { }
						
						// hide create account button
						this.btnCreateCarrierAccount.Visible = false;
					}

					this.txtFirstName.Text = contact.FirstName;
					this.txtLastName.Text = contact.LastName;
					this.txtContactEmail.Text = contact.Email;
					this.txtContactPhone.Text = contact.Phone;
					this.txtMobilePhone.Text = contact.Mobile;
					this.txtContactTile.Text = contact.ContactTitle;
					this.txtDepartmentName.Text = contact.DepartmentName;
					txtContactFax.Text = contact.Fax;
					txtDepartmentName.Text = contact.DepartmentName;
					
					txtAddress1.Text = contact.Address1;
					txtAddress2.Text = contact.Address2;

					cbxPrimary.Checked = contact.IsPrimary ?? false;
					txtContactFax.Text = contact.Fax;

					stateID = contact.StateID ?? 0;

					if (stateID > 0) {
						ddlState.SelectedValue = stateID.ToString();

						CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll(stateID));
					}

					cityID = contact.CityID ?? 0;

					if (cityID > 0) {
						try {
							ddlCity.SelectedValue = cityID.ToString();
						}
						catch {
						}

						CollectionManager.FillCollection(ddlLossZip, "ZipCodeID", "ZipCode", ZipCode.getByCityID(cityID));
					}
					zipCodeID = contact.ZipCodeID ?? 0;

					if (zipCodeID > 0) {
						try {
							ddlLossZip.SelectedValue = zipCodeID.ToString();
						}
						catch {
						}
					}

					

					if (contact.CategoryID != null) {
						try {
							ddlContactType.SelectedValue = contact.CategoryID.ToString();
						}
						catch (Exception ex) {
							Core.EmailHelper.emailError(ex);
						}
					}					
				}
			}

			if (e.CommandName == "DoRemove") {
				CarrierContactManager.Delete(id);

				bindData(carrierID);
			}

		}

		protected void saveContact() {
			bool isNewContact = false;
			int contactID = Convert.ToInt32(ViewState["ContactID"]);

			lblMessage.Text = string.Empty;
			lblMessage.CssClass = string.Empty;

			Contact contact = null;
			CarrierContact carrierContact = null;

			if (contactID == 0) {
				// new contact
				contact = new Contact();
				
				contact.ClientID = Core.SessionHelper.getClientId();

				contact.CarrierID = carrierID;

				contact.CompanyName = CarrierManager.GetName(this.carrierID);
				
				isNewContact = true;
			}
			else
				contact = ContactManager.Get(contactID);

			contact.FirstName = txtFirstName.Text;
			contact.LastName = txtLastName.Text;
			
			contact.ContactName = txtFirstName.Text + " " + txtLastName.Text;

			contact.Email = txtContactEmail.Text;
			contact.Phone = txtContactPhone.Text;
			contact.Mobile = txtMobilePhone.Text;
			contact.ContactTitle = txtContactTile.Text;
			contact.DepartmentName = txtDepartmentName.Text;
			contact.IsPrimary = cbxPrimary.Checked;
			contact.Fax = txtContactFax.Text;
			
			contact.Address1 = txtAddress1.Text;
			contact.Address2 = txtAddress2.Text;

			if (ddlState.SelectedIndex > 0)
				contact.StateID = Convert.ToInt32(ddlState.SelectedValue);

			if (ddlCity.SelectedIndex > 0)
				contact.CityID = Convert.ToInt32(ddlCity.SelectedValue);
				
			if (ddlLossZip.SelectedIndex > 0)
				contact.ZipCodeID = Convert.ToInt32(ddlLossZip.SelectedValue);

			if (ddlContactType.SelectedIndex > 0)
				contact.CategoryID = Convert.ToInt32(ddlContactType.SelectedValue);
		
			try {
				if (isNewContact) {
					using (TransactionScope scope = new TransactionScope()) {
						// save contact to contact table
						contact = ContactManager.Save(contact);

						carrierContact = new CarrierContact();

						carrierContact.CarrierID = carrierID;
						carrierContact.ContactID = contact.ContactID;

						// save contact reference for carrier
						carrierContact = CarrierContactManager.Save(carrierContact);						

						// complete transaction
						scope.Complete();
					}

					ViewState["ContactID"] = contact.ContactID;
				}
				else {
					// save contact to contact table
					contact = ContactManager.Save(contact);
				}

				lblMessage.Text = "Contact saved successfully.";
				lblMessage.CssClass = "ok";

				btnShowCreateAccount.Visible = true;

			}
			catch (Exception ex) {
				lblMessage.Text = "Unable to save contact.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}
			


		}

		private void showContactGrid() {
			pnlContactEdit.Visible = false;
			pnlContactGrid.Visible = true;
		}

		private void showContactPanel() {
			pnlContactEdit.Visible = true;
			pnlContactGrid.Visible = false;
		}

		protected void lbtnNewContact_Click(object sender, EventArgs e) {
			ViewState["ContactID"] = "0";

			showContactPanel();

			clearContactFields();

			bindContactTypes();

			bindStates();
		}

		protected void ddlState_SelectedIndexChanged(object sender, EventArgs e) {
			if (ddlState.SelectedIndex > 0) {
				CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll(Convert.ToInt32(ddlState.SelectedValue)));
			}
			else {
				ddlCity.Items.Clear();
			}
		}

		protected void dllCity_SelectedIndexChanged(object sender, EventArgs e) {
			if (ddlCity.SelectedIndex > 0) {
				CollectionManager.FillCollection(ddlLossZip, "ZipCodeID", "ZipCode", ZipCode.getByCityID(Convert.ToInt32(ddlCity.SelectedValue)));
			}
			else {
				ddlLossZip.Items.Clear();
			}

		}

		#region Create user account
		protected void btnCreateCarrierAccount_Click(object sender, EventArgs e) {
			int clientID = 0;
			int contactID = 0;
			Contact contact = null;
			string newUserName = null;
			string password = null;
            CRM.Data.Entities.SecUser newUserAccount = null;
            CRM.Data.Entities.SecUser user = null;

			lblMessage.Text = string.Empty;
			lblMessage.CssClass = string.Empty;

			Page.Validate("Account");
			if (!Page.IsValid)
				return;

			clientID = SessionHelper.getClientId();

			newUserName = txtUserName.Text.Trim();

			// check username is not taken
			if (SecUserManager.IsUserNameExist(newUserName)) {
				lblMessage.Text = "User Name " + newUserName + " is already taken.";
				lblMessage.CssClass = "error";
				return;
			}

			// initialize user object
            user = new CRM.Data.Entities.SecUser();

			user.UserName = newUserName;
			user.FirstName = txtFirstName.Text;
			user.LastName = txtLastName.Text;
			user.Status = true;
			user.isSSL = true;

			// assign client for this user
			user.ClientID = clientID;

			// attach carrier to user
			user.CarrierID = this.carrierID;

			user.CreatedBy = Core.SessionHelper.getUserId();
			user.CreatedOn = DateTime.Now;
			user.Email = txtContactEmail.Text.Trim();

			// assign selected role
			user.RoleId = Convert.ToInt32(ddlClientRoles.SelectedValue);

			// random password
			password = Guid.NewGuid().ToString().Substring(0, 8);

			// encrypt password
			user.Password = Core.SecurityManager.Encrypt(password);

			try {
				using (TransactionScope scope = new TransactionScope()) {
					// add new user account
					newUserAccount = SecUserManager.Save(user);

					// associate new user account with contact
					contactID = Convert.ToInt32(ViewState["ContactID"]);

					contact = ContactManager.Get(contactID);
					
					contact.UserID = newUserAccount.UserId;
					
					ContactManager.Save(contact);

					scope.Complete();										
				}

				// email adjuster about newly created account				
				Core.EmailHelper.emailUserCredentials(newUserAccount);

				lblMessage.Text = string.Format("User Account '{0}' was created and credentials were emailed to the user.", newUserName);
				lblMessage.CssClass = "ok";

				// hide create account button
				this.btnShowCreateAccount.Visible = false;
				this.btnCreateCarrierAccount.Visible = false;
			}
			catch (Exception ex) {
				lblMessage.Text = "Unable to create user account.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}

		}


		protected void btnShowCreateAccount_Click(object sender, EventArgs e) {
			int clientID = SessionHelper.getClientId();

			pnlUserAccount.Visible = true;

			txtUserName.Text = string.Format("{0}.{1}", txtFirstName.Text.Trim().ToLower(), txtLastName.Text.Trim().ToLower());

			// bind client roles
			CollectionManager.FillCollection(ddlClientRoles, "RoleId", "RoleName", SecRoleManager.GetRolesManagedByClient(clientID));
		}
		#endregion
	}
}