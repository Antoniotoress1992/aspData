using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucLeadPolicyContact : System.Web.UI.UserControl {
		int clientID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			clientID = Core.SessionHelper.getClientId();

			if (!Page.IsPostBack)
				bindContactTypes();
		}

		private void bindContactTypes() {
			CollectionManager.FillCollection(ddlContactType, "ID", "Description", LeadContactTypeManager.GetAll(clientID));
		}

		public void bindData(int leadID, int policyTypeID) {
			ViewState["policyTypeID"] = policyTypeID.ToString();

			gvContacts.DataSource = LeadContactManager.GetContactByLeadID(leadID, policyTypeID);
			gvContacts.DataBind();
		}
		protected void btnCancel_Click(object sender, EventArgs e) {
			showContactGrid();
		}

		protected void btnSaveContact_Click(object sender, EventArgs e) {
			Page.Validate("Contact");
			if (!Page.IsValid)
				return;

			int leadID = Convert.ToInt32(Session["LeadIds"]);
			int policyTypeID = Convert.ToInt32(ViewState["policyTypeID"]);

			int contactID = 0;
			int contactTypeID = Convert.ToInt32(ddlContactType.SelectedValue);

			LeadContact contact = null;
			
			contactID = Convert.ToInt32(ViewState["ContactID"]);
			if (contactID == 0) {
				contact = new LeadContact();

			}
			else
				contact = LeadContactManager.Get(contactID);

			if (contact != null) {
				contact.ID = contactID;
				contact.LeadID = leadID;
				contact.ContactName = txtContactName.Text;

				contact.isActive = true;
				contact.Mobile = txtContactPhone.Text;
				contact.Email = txtContactEmail.Text;
				contact.PolicyTypeID = policyTypeID;

				if (ddlContactType.SelectedIndex > 0)
					contact.ContactTypeID = contactTypeID;
				else
					contact.ContactTypeID = null;

				LeadContactManager.Save(contact);

				showContactGrid();

				// refresh screen
				bindData(leadID, policyTypeID);				
			}

		}

		private void clearContactFields() {
			txtContactEmail.Text = string.Empty;
			txtContactName.Text = string.Empty;
			txtContactPhone.Text = string.Empty;

			ddlContactType.SelectedIndex = -1;
		}
		protected void lbtnNewContact_Click(object sender, EventArgs e) {
			ViewState["ContactID"] = "0";

			clearContactFields();

			showContactPanel();
		}

		protected void gvContacts_RowCommand(object sender, GridViewCommandEventArgs e) {
			LeadContact contact = null;
			int contactID = Convert.ToInt32(e.CommandArgument);
			
			int leadID = Convert.ToInt32(Session["LeadIds"]);
			
			int policyTypeID = Convert.ToInt32(ViewState["policyTypeID"]);

			if (e.CommandName == "DoEdit") {
				contact = LeadContactManager.Get(contactID);
				if (contact != null) {
					showContactPanel();

					txtContactName.Text = contact.ContactName;

					txtContactEmail.Text = contact.Email;
					txtContactPhone.Text = contact.Mobile;

					try {
						ddlContactType.SelectedValue = contact.ContactTypeID.ToString();
					}
					catch {
					}

					ViewState["ContactID"] = contact.ID;

				}
			}
			if (e.CommandName == "DoRemove") {
				contactID = Convert.ToInt32(e.CommandArgument);
				try {
					contact = LeadContactManager.Get(contactID);
					contact.isActive = false;
					LeadContactManager.Save(contact);

					bindData(leadID, policyTypeID);	
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);

					lblContactMessage.Text = "Record Not Deleted.";
					lblContactMessage.Visible = true;
				}
			}

		}

		private void showContactGrid() {
			pnlContacGrid.Visible = true;
			pnlContactEdit.Visible = false;
		}

		private void showContactPanel() {
			pnlContacGrid.Visible = false;
			pnlContactEdit.Visible = true;
		}
	}
}