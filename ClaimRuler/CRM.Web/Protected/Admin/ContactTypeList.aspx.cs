using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Web;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class ContactTypeList : System.Web.UI.Page {
		int clientID = 0;
		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			clientID = Core.SessionHelper.getClientId();

			if (!IsPostBack) {			
				DoBind();
			}			
		}

		protected void btnNew_Click(object sender, EventArgs e) {
			showContactTypePanel();

			ViewState["ID"] = "0";

			clearFields();
		}

		private void clearFields() {
			txtContactType.Text = string.Empty;
		}

		protected void DoBind() {
			gvContactType.DataSource = LeadContactTypeManager.GetAll(clientID);

			gvContactType.DataBind();
		}
		protected void gvContactType_RowCommand(object sender, GridViewCommandEventArgs e) {
			LeadContactType contactType = null;

			int contactTypeID = Convert.ToInt32(e.CommandArgument);

			if (e.CommandName == "DoEdit") {
				contactType = LeadContactTypeManager.Get(contactTypeID);

				if (contactType != null) {
					txtContactType.Text = contactType.Description;

					showContactTypePanel();
				}
			}
			else if (e.CommandName == "DoDelete") {
				contactType = LeadContactTypeManager.Get(contactTypeID);
				if (contactType != null) {
					contactType.isActive = false;

					LeadContactTypeManager.Save(contactType);

					DoBind();
				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			Page.Validate("ContactType");
			if (!Page.IsValid)
				return;

			int contactTypeID = Convert.ToInt32(ViewState["ID"]);
			
			LeadContactType contactType = null;

			if (contactTypeID == 0) {
				contactType = new LeadContactType();

				contactType.isActive = true;

				contactType.ClientID = clientID;
			}
			else
				contactType = LeadContactTypeManager.Get(contactTypeID);

			if (contactType != null) {
				contactType.Description = txtContactType.Text;

				contactType = LeadContactTypeManager.Save(contactType);

				showContactTypeGrid();

				DoBind();
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			showContactTypeGrid();

			DoBind();
		}

		private void showContactTypePanel() {
			pnlContactTypeEdit.Visible = true;
			pnlContactTypeGrid.Visible = false;
		}

		private void showContactTypeGrid() {
			pnlContactTypeEdit.Visible = false;
			pnlContactTypeGrid.Visible = true;
		}
	}
}