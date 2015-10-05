using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucReminderPopUp : System.Web.UI.UserControl {
		const string REMINDER_KEY = "vw_reminders";

		protected void Page_Load(object sender, EventArgs e) {			
			bindData();
		}

		private void bindData() {
			if (Session[REMINDER_KEY] != null) {
				List<vw_Reminder> reminders = (List<vw_Reminder>)Session[REMINDER_KEY];

				if (reminders != null && reminders.Count > 0) {
					gvReminder.DataSource = reminders;
					gvReminder.DataBind();
				}
				else {
					closeWindow();
				}
			}
			else {
				closeWindow();
			}
		}

		public void btnSnooze_Click(object sender, EventArgs e) {
			int taskID = 0;
			int interval = 0;
			Task task = null;

			interval = Convert.ToInt32(ucReminderInterval.SelectedValue);

			foreach (GridViewRow row in gvReminder.Rows) {
				if (row.RowType == DataControlRowType.DataRow) {
					taskID = (int)gvReminder.DataKeys[row.RowIndex].Value;

					task = TasksManager.Get(taskID);
					if (task != null) {
						task.start_date = DateTime.Now.AddMinutes(interval);

						task.ReminderInterval = 0;

						TasksManager.Save(task);
					}
				}
			}

			closeWindow();
		}

		private void closeWindow() {
			Session.Remove(REMINDER_KEY);

			Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "rk", "window.close();", true);
		}

		protected void btnDismissAll_Click(object sender, EventArgs e) {
			int taskID = 0;

			foreach (GridViewRow row in gvReminder.Rows) {
				if (row.RowType == DataControlRowType.DataRow) {
					taskID = (int)gvReminder.DataKeys[row.RowIndex].Value;

					dismissReminder(taskID);
				}
			}

			closeWindow();
		}

		private void dismissReminder(int taskID) {
			Task task = null;

			task = TasksManager.Get(taskID);

			if (task != null) {
				task.status_id = (int)Globals.Task_Status.Dismissed;

				TasksManager.Save(task);

				deleteReminderInMemory(taskID);

				bindData();
			}
		}

		private void deleteReminderInMemory(int taskID) {
			List<vw_Reminder> reminders = null;

			if (Session[REMINDER_KEY] != null) {
				reminders = (List<vw_Reminder>)Session[REMINDER_KEY];

				reminders = reminders.Where(x => x.id != taskID).ToList();

				Session[REMINDER_KEY] = reminders;
			}
		}

		protected void gv_RowCommand(object sender, GridViewCommandEventArgs e) {
			int taskID = Convert.ToInt32(e.CommandArgument);

			if (e.CommandName == "DoEdit") {
			}
			else if (e.CommandName == "DoDismiss") {
				dismissReminder(taskID);

				deleteReminderInMemory(taskID);

				bindData();
			}
		}

		protected void gvReminder_RowDataBound(object sender, GridViewRowEventArgs e) {
			vw_Reminder reminder = e.Row.DataItem as vw_Reminder;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				Label lblDueIn = e.Row.FindControl("lblDueIn") as Label;

				TimeSpan difference = ((DateTime)reminder.start_date) - DateTime.Now;

				if (difference.TotalMinutes < 0) {
					// overdue
					lblDueIn.Text = Common.calculateOverdueTime(difference);
				}
				else {
					lblDueIn.Text = Common.convertIntervalToString(reminder.ReminderInterval ?? 0);
				}
			}
		}
	}
}