using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;
using CRM.Core;

namespace CRM.Web.UserControl.Admin {
	public partial class ucCarrierTask : System.Web.UI.UserControl {
		private int carrierID {
			get {
				return Session["CarrierID"] != null ? Convert.ToInt32(Session["CarrierID"]) : 0;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack)
            {
                BindPriority();
            }

		}

		public void bindData() {
			gvTasks.DataSource = CarrierTaskManager.GetAll(carrierID);
			gvTasks.DataBind();
		}

		protected void btnNewTask_Click(object sender, EventArgs e) {
			ViewState["TaskID"] = "0";

			showEditPanel();

			clearFields();

		}

		protected void btnSave_Click(object sender, EventArgs e) {
			Page.Validate("appointment");
			if (!Page.IsValid)
				return;

			int reminderInterval = 0;
			int taskID = Convert.ToInt32(ViewState["TaskID"]);
			Task task = null;

			if (taskID == 0) {
				task = new Task();
				task.CarrierID = carrierID;
				task.owner_id = Core.SessionHelper.getUserId();
			}
			else {
				task = TasksManager.Get(taskID);
			}

			if (task != null) {
				// set reminder flag
				reminderInterval = Convert.ToInt32(ucReminderInterval.SelectedValue);
				task.ReminderInterval = reminderInterval;

				task.IsReminder = (reminderInterval > 0);

				task.creator_id = Core.SessionHelper.getClientId();

				task.text = txtSubject.Text.Trim();
				task.details = txtDetails.Text.Trim();

				string end_date = string.Format("{0} {1}", endDate.Text, endTime.Text);
				string start_date = string.Format("{0} {1}", startDate.Text, startTime.Text);

				task.start_date = Convert.ToDateTime(start_date);
				task.end_date = Convert.ToDateTime(end_date);

				task.isAllDay = cbxAllDayEvent.Checked;

				task.status_id = (int)Globals.Task_Status.Active;


				task.owner_id = Core.SessionHelper.getUserId();

				if (ddlPriority.SelectedIndex > 0)
					task.PriorityID = Convert.ToInt32(ddlPriority.SelectedValue);

				TasksManager.Save(task);

				showGridPanel();

				bindData();
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			showGridPanel();
		}

		private void clearFields() {
			txtDetails.Text = string.Empty;
			txtSubject.Text = string.Empty;
			startDate.Value = null;
			endDate.Value = null;
			startTime.Text = null;
			endTime.Value = null;
			ddlPriority.SelectedIndex = -1;
			ucReminderInterval.SelectedIndex = -1;
		}

		private void fillForm(int taskID) {
			Task task = TasksManager.Get(taskID);

			if (task != null) {
				txtDetails.Text = task.details;
				txtSubject.Text = task.text;

				startDate.Text = ((DateTime)task.start_date).ToShortDateString();
				endDate.Text = ((DateTime)task.end_date).ToShortDateString();

				startTime.Text = ((DateTime)task.start_date).ToString("hh:mm tt");
				endTime.Text = ((DateTime)task.end_date).ToString("hh:mm tt");

				if (task.PriorityID != null)
					ddlPriority.SelectedValue = task.PriorityID.ToString();

				if (task.ReminderInterval != null)
					ucReminderInterval.SelectedValue = task.ReminderInterval.ToString();
			}
		}

		protected void gvTasks_RowCommand(object sender, GridViewCommandEventArgs e) {
			int taskID = Convert.ToInt32(e.CommandArgument);

			if (e.CommandName == "DoEdit") {
				ViewState["TaskID"] = taskID.ToString();

				showEditPanel();

				fillForm(taskID);
			}
			else if (e.CommandName == "DoDelete") {
				TasksManager.Delete(taskID);

				showGridPanel();

				bindData();
			}
		}

		protected void IsTimeValid(object sender, ServerValidateEventArgs args) {
			DateTime stime = DateTime.MinValue;

			if (DateTime.TryParse(args.Value, out stime))
				args.IsValid = true;
			else
				args.IsValid = false;
		}

		private void showEditPanel() {
			pnlEdit.Visible = true;
			pnlGrid.Visible = false;
		}

		private void showGridPanel() {
			pnlEdit.Visible = false;
			pnlGrid.Visible = true;
		}

        protected void BindPriority()
        {

            List<TaskPriority> priority = CarrierTaskManager.GetAllPriority();

            CollectionManager.FillCollection(ddlPriority, "PriorityID", "PriorityName", priority);
        }

	}
}