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

namespace CRM.Web.Protected.Admin {
	public partial class MasterStatusReminder : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			Page.Form.DefaultButton = btnSave.UniqueID;

			if (!Page.IsPostBack) {
				bindList();
			}
		}

		protected void btnCancel_click(object sender, EventArgs e) {
			pnlEdit.Visible = false;
			pnlList.Visible = true;

			bindList();
		}

		protected void btnNew_Click(object sender, EventArgs e) {
			clearFields();

			pnlList.Visible = false;
			pnlEdit.Visible = true;
		}

		protected void btnSave_click(object sender, EventArgs e) {
			ReminderMaster reminder = null;
			
			Page.Validate("reminder");

			if (!Page.IsValid)
				return;

			reminder = new ReminderMaster();

			reminder.ReminderID = Convert.ToInt32(hf_reminderID.Value);
			
			reminder.Description = txtDescription.Text;

			reminder.DurationType = Convert.ToInt32(ddlType.SelectedValue);
			
			reminder.Duration = Convert.ToInt32(txtDuration.Text);
			
			reminder.isActive = true;

			if (SessionHelper.getClientId() > 0)
				reminder.clientID = SessionHelper.getClientId();

			ReminderMasterManager.Save(reminder);

			clearFields();

			pnlList.Visible = true;
			pnlEdit.Visible = false;

			bindList();
		}

		private void clearFields() {
			txtDescription.Text = "";
			txtDuration.Text = "";
			ddlType.SelectedIndex = -1;
			hf_reminderID.Value = "0";
			lblMessage.Text = string.Empty;
		}

		protected void bindList() {
			if (SessionHelper.getClientId() > 0)
				gvReminder.DataSource = ReminderMasterManager.GetAll(SessionHelper.getClientId());
			else
				gvReminder.DataSource = ReminderMasterManager.GetAll();

			gvReminder.DataBind();
		}

		protected void gvReminder_RowCommand(object sender, GridViewCommandEventArgs e) {
			int reminderID = 0;

			if (e.CommandName == "DoEdit") {
				hf_reminderID.Value = e.CommandArgument.ToString();
				reminderID = Convert.ToInt32(e.CommandArgument.ToString());

				ReminderMaster reminder = ReminderMasterManager.Get(reminderID);

				txtDescription.Text = reminder.Description;

				txtDuration.Text = reminder.Duration.ToString();

				ddlType.SelectedValue = reminder.DurationType.ToString();

				pnlEdit.Visible = true;
				pnlList.Visible = false;
			}

			if (e.CommandName == "DoDelete") {
				hf_reminderID.Value = e.CommandArgument.ToString();
				reminderID = Convert.ToInt32(e.CommandArgument.ToString());

				ReminderMaster reminder = ReminderMasterManager.Get(reminderID);

				if (reminder != null) {
					reminder.isActive = false;

					ReminderMasterManager.Save(reminder);
				}
			}

			bindList();
		}
	}
}