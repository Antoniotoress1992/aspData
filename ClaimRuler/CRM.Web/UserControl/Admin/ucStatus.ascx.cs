namespace CRM.Web.UserControl.Admin {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using CRM.Data.Account;
	using CRM.Data;
	using LinqKit;
	using CRM.Core;
	using System.Globalization;
	using System.Transactions;
    using CRM.Data.Entities;

	public partial class ucStatus : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;
		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;
			this.Page.Form.DefaultButton = this.btnSave.UniqueID;

			if (!IsPostBack) {
				DoBind();
			}

			if (Session["LeadIds"] == null)
				btnReturnToClaim.Visible = false;
		}

		private void DoBind() {
			int clientID = Core.SessionHelper.getClientId();
			List<StatusMaster> lstStatus = null;

			var predicate = PredicateBuilder.True<StatusMaster>();


			lstStatus = StatusManager.GetAll(clientID);

			gvStatus.DataSource = lstStatus;
			gvStatus.DataBind();


			// load reminder master
			CollectionManager.FillCollection(ddlReminder, "ReminderID", "Description", ReminderMasterManager.GetAll(clientID));

		}

		protected void gv_RowCommand(object sender, GridViewCommandEventArgs e) {
			StatusMaster leadStatus = null;
			int statusId = 0;

			if (e.CommandName.Equals("DoEdit")) {
				statusId = Convert.ToInt32(e.CommandArgument);

				leadStatus = StatusManager.GetStatusId(statusId);

				if (leadStatus != null) {
					hdId.Value = statusId.ToString();

					txtStatus.Text = leadStatus.StatusName;

					if (leadStatus.ReminderID != null) {
						ddlReminder.SelectedValue = leadStatus.ReminderID.ToString();
					}

					cbxIsCountable.Checked = leadStatus.isCountable ?? false;


					cbxIsCountAsOpen.Checked = leadStatus.isCountAsOpen ?? false;

					pnlGrid.Visible = false;
					pnlEdit.Visible = true;
				}

			}
			else if (e.CommandName.Equals("DoDelete")) {
				// In Case of delete 
				try {

					var status = StatusManager.GetStatusId(Convert.ToInt32(e.CommandArgument));
					status.Status = false;
					StatusManager.Save(status);
					btnCancel_Click(null, null);
					lblSave.Text = "Record Deleted Sucessfully.";
					lblSave.Visible = true;
				}
				catch (Exception ex) {
					lblError.Text = "Record Not Deleted.";
					lblError.Visible = true;

					Core.EmailHelper.emailError(ex);
				}
			}


		}

		protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			gvStatus.PageIndex = e.NewPageIndex;

			DoBind();
		}

		protected void btnReturnToClaim_Click(object sender, EventArgs e) {
			if (Session["LeadIds"] != null) {
				Response.Redirect("~/protected/newlead.aspx?id=" + Session["LeadIds"].ToString());
			}
		}

		protected void btnNew_Click(object sender, EventArgs e) {
			pnlEdit.Visible = true;
			pnlGrid.Visible = false;

			clearFields();
		}

		private void clearFields() {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
			txtStatus.Text = string.Empty;
			hdId.Value = "0";
			ddlReminder.SelectedIndex = -1;

			cbxIsCountAsOpen.Checked = false;
			cbxIsCountable.Checked = false;
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
			bool exists = false;

			int clientID = Core.SessionHelper.getClientId();

			try {
				using (TransactionScope scope = new TransactionScope()) {

					if (clientID > 0)
						exists = StatusManager.IsExist(txtStatus.Text.Trim(), Convert.ToInt32(hdId.Value), clientID);
					else
						exists = StatusManager.IsExist(txtStatus.Text.Trim(), Convert.ToInt32(hdId.Value));

					if (exists) {
						lblMessage.Text = "Status already exists.";
						lblMessage.Visible = true;
						txtStatus.Focus();
						return;
					}
					StatusMaster status = StatusManager.GetStatusId(Convert.ToInt32(hdId.Value));
					status.StatusName = txtStatus.Text;
					status.Status = true;

					if (clientID > 0)
						status.clientID = clientID;

					if (Convert.ToInt32(ddlReminder.SelectedValue) > 0)
						status.ReminderID = Convert.ToInt32(ddlReminder.SelectedValue);
					else
						status.ReminderID = null;

					status.isCountable = cbxIsCountable.Checked;
					status.isCountAsOpen = cbxIsCountAsOpen.Checked;

					StatusManager.Save(status);
					lblSave.Text = hdId.Value == "0" ? "Record Saved Sucessfully." : "Record Updated Sucessfully.";

					lblSave.Visible = true;
					scope.Complete();
				}
			}
			catch (Exception ex) {
				lblError.Visible = true;
				lblError.Text = "Record Not Saved !!!";

				Core.EmailHelper.emailError(ex);

			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			clearFields();

			pnlEdit.Visible = false;
			pnlGrid.Visible = true;

			DoBind();
		}
	}
}