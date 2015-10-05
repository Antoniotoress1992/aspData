using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.UserControl {
	public partial class ucClaimContacts : System.Web.UI.UserControl {
		int clientID = 0;

		private int claimID {
			get {
				return Session["ClaimID"] != null ? Convert.ToInt32(Session["ClaimID"]) : 0;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			clientID = Core.SessionHelper.getClientId();

			lbtnNewContact.Visible = CRM.Core.PermissionHelper.checkAddPermission("UsersLeads.aspx");
			
		}

		public void bindData(int claimID) {
			clientID = Core.SessionHelper.getClientId();

			List<ClaimContact> contacts = ClaimContactManager.GetAll(claimID);

			showContactGrid();

			gvContacts.DataSource = contacts;
			gvContacts.DataBind();
		}

		private void bindContactTypes() {
			List<LeadContactType> contactTypes = null;

			// get contacts for this client
			contactTypes = LeadContactTypeManager.GetAll(clientID);

			Core.CollectionManager.FillCollection(ddlContactType, "ID", "Description", contactTypes);
		}
		protected void bindStates() {
			List<StateMaster> states = State.GetAll();

			CollectionManager.FillCollection(ddlState, "StateId", "StateName", states);
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			showContactGrid();

			bindData(this.claimID);
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			Page.Validate("Contact");
			if (!Page.IsValid)
				return;

			saveContact();
		}

		protected void clearContactFields() {
			txtAddress1.Text = "";
			txtAddress2.Text = "";
			txtCompanyName.Text = "";
			this.txtContactEmail.Text = "";
			txtFirstName.Text = "";
			txtLastName.Text = "";
			this.txtMobilePhone.Text = "";
			this.txtContactPhone.Text = "";
			txtContactFax.Text = "";
			ddlState.SelectedIndex = -1;
			ddlCity.Items.Clear();
			ddlLossZip.Items.Clear();
			ddlContactType.SelectedIndex = -1;
			txtContactTile.Text = string.Empty;
			txtDepartmentName.Text = string.Empty;

			cbxPrimary.Checked = false;

			ddlContactType.SelectedIndex = -1;
			ddlCity.SelectedIndex = -1;
			ddlLossZip.SelectedIndex = -1;
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


		protected void lbtnNewContact_Click(object sender, EventArgs e) {
			ViewState["ContactID"] = "0";

			showContactPanel();

			bindContactTypes();

			clearContactFields();

			bindStates();
		}

		protected void gvContacts_RowCommand(object sender, GridViewCommandEventArgs e) {
			Contact contact = null;
			int id = 0;
			
			if (e.CommandName == "DoEdit") {
				id = Convert.ToInt32(e.CommandArgument);

				ViewState["ContactID"] = id.ToString();

				contact = ContactManager.Get(id);

				if (contact != null) {
					showContactPanel();

					bindContactTypes();

					bindStates();

					fillForm(contact);			
				}
			}
			else if (e.CommandName == "DoRemove") {
				id = Convert.ToInt32(e.CommandArgument);

				ClaimContactManager.Delete(id);

				bindData(this.claimID);
			}
		}

		protected void fillForm(CRM.Data.Entities.Contact contact) {
			int stateID = 0;
			int cityID = 0;
			int zipCodeID = 0;

			this.txtFirstName.Text = contact.FirstName;
			this.txtLastName.Text = contact.LastName;
			this.txtContactEmail.Text = contact.Email;
			this.txtContactPhone.Text = contact.Phone;
			this.txtMobilePhone.Text = contact.Mobile;
			this.txtContactTile.Text = contact.ContactTitle;
			this.txtDepartmentName.Text = contact.DepartmentName;
			this.txtCompanyName.Text = contact.CompanyName;

			cbxPrimary.Checked = contact.IsPrimary ?? false;
			txtContactFax.Text = contact.Fax;

			txtAddress1.Text = contact.Address1;
			txtAddress2.Text = contact.Address2;

			ddlContactType.SelectedValue = (contact.CategoryID ?? 0).ToString();

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
		protected void saveContact() {
			bool isNewContact = false;
			int contactID = Convert.ToInt32(ViewState["ContactID"]);

			Contact contact = null;
			ClaimContact claimContact = null;
			lblMessage.Text = string.Empty;

			if (contactID == 0) {
				// new contact
				contact = new Contact();

				contact.ClientID = Core.SessionHelper.getClientId();

				isNewContact = true;
			}
			else
				contact = ContactManager.Get(contactID);

			contact.FirstName = txtFirstName.Text;
			contact.LastName = txtLastName.Text;
			contact.ContactName = txtFirstName.Text + " " + txtLastName.Text;

			contact.Phone = txtContactPhone.Text;
			contact.Email = txtContactEmail.Text;
			contact.Phone = txtContactPhone.Text;
			contact.Mobile = txtMobilePhone.Text;
			contact.ContactTitle = txtContactTile.Text;
			contact.DepartmentName = txtDepartmentName.Text;
			contact.IsPrimary = cbxPrimary.Checked;
			contact.Fax = txtContactFax.Text;
			contact.CompanyName = txtCompanyName.Text.Trim();

			if (ddlContactType.SelectedIndex > 0)
				contact.CategoryID = Convert.ToInt32(ddlContactType.SelectedValue);
			else
				contact.CategoryID = null;

			contact.Address1 = txtAddress1.Text;
			contact.Address2 = txtAddress2.Text;

			if (ddlState.SelectedIndex > 0)
				contact.StateID = Convert.ToInt32(ddlState.SelectedValue);

			if (ddlCity.SelectedIndex > 0)
				contact.CityID = Convert.ToInt32(ddlCity.SelectedValue);

			if (ddlLossZip.SelectedIndex > 0)
				contact.ZipCodeID = Convert.ToInt32(ddlLossZip.SelectedValue);

			

			try {
				if (isNewContact) {
					using (TransactionScope scope = new TransactionScope()) {
						// save contact to contact table
						contact = ContactManager.Save(contact);

						claimContact = new ClaimContact();

						claimContact.ClaimID = this.claimID;
						claimContact.ContactID = contact.ContactID;

						// save contact reference for claim
						claimContact = ClaimContactManager.Save(claimContact);

						// complete transaction
						scope.Complete();
					}
				}
				else {
					// save contact to contact table
					contact = ContactManager.Save(contact);
				}

				lblMessage.Text = "Contact saved successfully.";
				lblMessage.CssClass = "ok";

			}
			catch (Exception ex) {
				lblMessage.Text = "Contact not saved.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}
			finally {
				bindData(this.claimID);
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

		protected void lbtnImportContact_Click(object sender, EventArgs e) {
			pnlImportContact.Visible = true;
			pnlContactGrid.Visible = false;

			// bind contacts
			wdgContacts.DataSourceID = "edsContacts";
			wdgContacts.DataBind();
		}

		protected void btnCloseImportContact_Click(object sender, EventArgs e) {
			pnlImportContact.Visible = false;
			pnlContactGrid.Visible = true;
			
			// refresh contacts
			bindData(this.claimID);
		}

		protected void wdgContacts_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e) {
			ClaimContact claimContact = null;

			int contactID = 0;
			int claimID = SessionHelper.getClaimID();

			if (e.CommandName == "DoImport") {
				try {
					contactID = Convert.ToInt32(e.CommandArgument);
					claimContact = new ClaimContact();
					claimContact.ClaimID = claimID;
					claimContact.ContactID = contactID;

					ClaimContactManager.Save(claimContact);

					lblImportContactMessage.Text = "Contact was imported successfully.";
					lblImportContactMessage.CssClass = "ok";
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
					lblImportContactMessage.Text = "Contact was not imported.";
					lblImportContactMessage.CssClass = "error";
				}
			}
		}

		
	}
}