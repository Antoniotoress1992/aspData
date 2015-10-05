using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;

using CRM.Repository;

using System.Transactions;
using CRM.Data.Entities;

namespace CRM.Web.UserControl {
	public partial class ucEvent : System.Web.UI.UserControl {

		protected void Page_Load(object sender, EventArgs e) {
            
		}


		public void bindData(int taskID) {
			int clientID = SessionHelper.getClientId();
			int userID = SessionHelper.getUserId();

			List<UserStaff> owners = null;

			// load owners for this client
			owners = SecUserManager.GetStaff(clientID);
			CollectionManager.FillCollection(ddlOwner, "UserId", "StaffName", owners);

			// check user can assign user to event
			bool isAssignUsersToActivities = Core.PermissionHelper.checkAction((int)Globals.Actions.AssignUsersToActivities);


			if (isAssignUsersToActivities) {
				ddlOwner.Enabled = true;
			}
			else {
				ddlOwner.Enabled = false;
				ddlOwner.SelectedValue = userID.ToString();
			}

			fillForm(taskID);
		}

		private void bindInvitees(int taskID) {
			List<InviteeView> invitees = null;

			using (InviteeManager repository = new InviteeManager()) {
				invitees = repository.GetAll(taskID);
			}

			gvInvitees.DataSource = invitees;
			gvInvitees.DataBind();
		}

		protected void lbtnClearRecurrence_Click(object sender, EventArgs e) {
			txtRecurrenceEndDate.Text = string.Empty;
			txtRecurrenceStartDate.Text = string.Empty;
			ddlRecurringRepeatFrequency.SelectedIndex = 0;

			if (ViewState["RecurringID"] != null)
				RecurrenceManager.Delete(Convert.ToInt32(ViewState["RecurringID"]));
		}


		protected void lbtnClearReminder_Click(object sender, EventArgs e) {
			ddlReminderWhen.SelectedIndex = 0;
			ddlReminderRepeat.SelectedIndex = 0;
			ddlReminderAlert.SelectedIndex = 0;

			// delete reminder
			if (ViewState["ReminderID"] != null)
				ReminderManager.Delete(Convert.ToInt32(ViewState["ReminderID"]));
		}

		private void fillForm(int taskID) {
			Reminder reminder = null;
			Recurrence recurrence = null;
			Task task = null;




			if (taskID > 0) {
				// edit existing event
				task = TasksManager.Get(taskID);

				if (task != null) {
					txtSubject.Text = task.text;
					txtDescription.Text = task.details;
					txtEventDateTimeStart.Value = task.start_date;
					txtEventDateTimeEnd.Value = task.end_date;
					ddlOwner.SelectedValue = (task.owner_id ?? 0).ToString();
					ddlReminderWhen.SelectedValue = (task.ReminderInterval ?? 0).ToString();

					hf_taskID.Value = taskID.ToString();

					if (task.Reminder != null && task.Reminder.Count > 0) {
						reminder = task.Reminder.FirstOrDefault();
						if (reminder != null) {
							ViewState["ReminderID"] = reminder.ReminderID.ToString();
							ddlReminderRepeat.SelectedValue = (reminder.ReminderWhen ?? 0).ToString();
							ddlReminderAlert.SelectedValue = (reminder.AlertTypeID ?? 1).ToString();		// email
						}
					}

					if (task.Recurrence != null) {

						ViewState["RecurringID"] = recurrence.RecurringID.ToString();
						txtRecurrenceStartDate.Value = recurrence.DateStart;
						txtRecurrenceEndDate.Value = recurrence.DateEnd;
						ddlRecurringRepeatFrequency.SelectedValue = (recurrence.RepeatFrequencyID ?? 0).ToString();

					}
					// show invitees
					pnlInvitees.Visible = true;

					bindInvitees(taskID);
				}
			}



		}

		private void btnSave_Click(object sender, EventArgs e) {
			saveEvent();
		}

		private void btnSaveNew_Click(object sender, EventArgs e) {
			saveAndNew();
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			Response.Redirect("~/protected/Tasks.aspx");
		}

		private void clearFields() {
			txtEventDateTimeStart.Value = null;
			txtEventDateTimeEnd.Value = null;
			txtSubject.Text = string.Empty;
			txtDescription.Text = string.Empty;
			ddlOwner.SelectedIndex = 0;

			ddlReminderWhen.SelectedIndex = 0;
			ddlReminderRepeat.SelectedIndex = 0;
			ddlReminderAlert.SelectedIndex = 0;

			txtRecurrenceEndDate.Value = null;
			txtRecurrenceStartDate.Value = null;
			ddlRecurringRepeatFrequency.SelectedIndex = 0;

		}

		[System.Web.Services.WebMethod]
		static public void inviteeAddContact(int taskID, int? contactID, int? userID, int? leadID) {
			Invitee invitee = null;

			if (taskID > 0) {
				invitee = new Invitee();
				invitee.TaskID = taskID;
				invitee.ContactID = contactID;
				invitee.UserID = userID;
				invitee.LeadID = leadID;

				using (InviteeManager repository = new InviteeManager()) {
					repository.Save(invitee);
				}
			}
		}

		private void saveRecurrence(Recurrence recurrence) {
			recurrence.IsActive = true;

			recurrence.DateEnd = txtRecurrenceEndDate.Date;

			recurrence.DateStart = txtRecurrenceStartDate.Date;

			recurrence.RepeatFrequencyID = ddlRecurringRepeatFrequency.SelectedIndex > 0 ? (int?)Convert.ToInt32(ddlRecurringRepeatFrequency.SelectedValue) : null;

			recurrence = RecurrenceManager.Save(recurrence);
		}

		private void saveReminder(Reminder reminder) {
			int minutes = 0;

			reminder.IsActive = true;

			minutes = -1 * Convert.ToInt32(ddlReminderWhen.SelectedValue);

			reminder.ReminderDate = txtEventDateTimeStart.Date.AddMinutes(minutes);

			reminder.AlertTypeID = ddlReminderAlert.SelectedIndex > 0 ? (int?)Convert.ToInt32(ddlReminderAlert.SelectedValue) : null;

			reminder = ReminderManager.Save(reminder);
		}

		public void saveEvent() {
			Task task = null;
			Recurrence recurrence = null;
			Reminder reminder = null;
			string success = "Event saved successfully.";
			string error = "Event was not saved.";

			int taskID = Convert.ToInt32(hf_taskID.Value);

			int clientID = SessionHelper.getClientId();

			if (taskID > 0) {
				// edit
				task = TasksManager.Get(taskID);
			}
			else {
				// new
				task = new Task();
				task.creator_id = clientID;
				task.TaskType = 2;	// save task as an event
				task.status_id = 1;
			}

			if (task != null) {
				try {
					task.text = txtSubject.Text.Trim();
					task.details = txtDescription.Text.Trim();
					task.start_date = txtEventDateTimeStart.Date;
					task.end_date = txtEventDateTimeEnd.Date;
					task.owner_id = Convert.ToInt32(ddlOwner.SelectedValue);
					task.ReminderInterval = Convert.ToInt32(ddlReminderWhen.SelectedValue);

					using (TransactionScope scope = new TransactionScope()) {
						taskID = TasksManager.Save(task);

						hf_taskID.Value = taskID.ToString();

						// reminder
						if (ddlReminderWhen.SelectedIndex > 0 && task.Reminder != null && task.Reminder.Count == 0) {
							// new reminder
							reminder = new Reminder();
							reminder.TaskID = taskID;

							saveReminder(reminder);
						}
						else if (ddlReminderWhen.SelectedIndex > 0 && task.Reminder != null && task.Reminder.Count == 1) {
							// edit existing reminder
							reminder = task.Reminder.FirstOrDefault();

							saveReminder(reminder);
						}
						else if (ddlReminderWhen.SelectedIndex == 0 && task.Reminder != null && task.Reminder.Count == 1 && ViewState["ReminderID"] != null) {
							// delete reminder
							ReminderManager.Delete(Convert.ToInt32(ViewState["ReminderID"]));
						}

						// recurring
						if (!string.IsNullOrEmpty(txtRecurrenceStartDate.Text) && task.Recurrence == null) {
							// new recurrence
							recurrence = new Recurrence();
							recurrence.TaskID = taskID;

							saveRecurrence(recurrence);
						}
						else if (!string.IsNullOrEmpty(txtRecurrenceStartDate.Text) && task.Recurrence != null) {
							// edit recurrence
							recurrence =(Recurrence) task.Recurrence;

							saveRecurrence(recurrence);
						}
						else if (string.IsNullOrEmpty(txtRecurrenceStartDate.Text) && task.Recurrence != null && ViewState["RecurringID"] != null) {
							// delete recurrence
							RecurrenceManager.Delete(Convert.ToInt32(ViewState["RecurringID"]));
						}

						// commit changes to DB
						scope.Complete();
					}

					// allow user to add invitees
					pnlInvitees.Visible = true;

					if (this.Page is CRM.Web.Protected.EventEdit)
						((CRM.Web.Protected.EventEdit)this.Page).setErrorMessage(success, "ok");
					else if (this.Page is CRM.Web.Protected.EventEditPopUp)
						((CRM.Web.Protected.EventEditPopUp)this.Page).setErrorMessage(success, "ok");

					//lblMessage.Text = "Event saved successfully.";
					//lblMessage.CssClass = "ok";
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
					if (this.Page is CRM.Web.Protected.EventEdit)
						((CRM.Web.Protected.EventEdit)this.Page).setErrorMessage(error, "error");
					else if (this.Page is CRM.Web.Protected.EventEditPopUp)
						((CRM.Web.Protected.EventEditPopUp)this.Page).setErrorMessage(error, "error");

					//lblMessage.Text = "Event was not saved.";
					//lblMessage.CssClass = "error";
				}
			}
		}

		public void saveAndNew() {
			saveEvent();

			clearFields();
		}
		protected void btnRefreshInvietees_Click(object sender, EventArgs e) {
			int taskID = Convert.ToInt32(hf_taskID.Value);

			bindInvitees(taskID);
		}


		protected void gvInvitees_RowCommand(object sender, GridViewCommandEventArgs e) {
			int inviteeID = 0;
			int taskID = Convert.ToInt32(hf_taskID.Value);

			if (e.CommandName == "DoDelete") {
				inviteeID = Convert.ToInt32(e.CommandArgument);
				try {
					using (InviteeManager repository = new InviteeManager()) {
						repository.Delete(inviteeID);
					}

					bindInvitees(taskID);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}
		}

		protected void lbtnUninviteeAll_Click(object sender, EventArgs e) {
			int taskID = Convert.ToInt32(hf_taskID.Value);

			using (InviteeManager repository = new InviteeManager()) {
				repository.DeleteAll(taskID);
			}
			bindInvitees(taskID);
		}

	}
}