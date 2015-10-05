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

namespace CRM.Web.Content {
	public partial class ReminderPopUp : System.Web.UI.Page {

		const string REMINDER_KEY = "vw_reminders";

		protected void Page_Load(object sender, EventArgs e) {
			//Master.showFooter(false);

		
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

						// clear adcanced notice popup. Reminder will popup on exact date/time due.
						task.ReminderInterval = 0;

						TasksManager.Save(task);
					}
				}
			}

			closeWindow();
		}

		[System.Web.Services.WebMethod]
		public static string checkForReminders() {
			string hasReminder = "0";

			try {
				int userID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
				List<vw_Reminder> reminders = SchedulerManager.GetUpcomingReminders(userID);

				if (reminders != null && reminders.Count > 0) {
					hasReminder = "1";
					HttpContext.Current.Session["vw_reminders"] = reminders;
				}
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
			}

			return hasReminder;
		}

		[System.Web.Services.WebMethod]
		public static void clearReminderFromSession() {
			if (HttpContext.Current.Session[REMINDER_KEY] != null) 
				HttpContext.Current.Session.Remove(REMINDER_KEY);
		}
		
		private void closeWindow() {
			Session.Remove(REMINDER_KEY);
			string js = "closeReminderDialog();";

			Page.ClientScript.RegisterStartupScript(this.GetType(), "rk", js, true);
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
			int reminderInterval = 0;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				Label lblDueIn = e.Row.FindControl("lblDueIn") as Label;

				//TimeSpan difference = ((DateTime)reminder.start_date) - DateTime.Now;

				if (reminder.DueIn < 0) {
					// overdue, make it color red
					reminderInterval = Math.Abs((int)reminder.DueIn);

					lblDueIn.Text = Common.convertIntervalToString(reminderInterval) + " Overdue";
					e.Row.CssClass = "redstar";

					PnlAlarmOverDue.Visible = true;
					pnlAlarm.Visible = false;
				}
				else {
					// due now
					//TimeSpan interval = DateTime.Now - (DateTime)reminder.start_date;

					//intervalMinutes = Math.Abs((int)interval.TotalMinutes);

					lblDueIn.Text = Common.convertIntervalToString((int)reminder.DueIn);

					
				}
			}
		}
	}
}