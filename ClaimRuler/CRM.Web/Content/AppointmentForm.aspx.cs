using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;
using CRM.Web.Utilities;
using CRM.Data.Entities;

namespace CRM.Web.Content {
	public partial class AppointmentForm : System.Web.UI.Page {
		int clientID = 0;
		int roleID = 0;
		int userID = 0;

		int TaskID {
			get {
				int id = 0;
				int.TryParse(Request.Params["id"], out id);
				return id;
			}
		}
	
		bool isLead {
			get { return Request.Params["o"] != null; }
		}

		private void bindData() {
			List<UserStaff> users = null;

			// load tasks priorties
			ddlPriority.DataSource = TasksManager.GetPriorities(clientID);
			ddlPriority.DataBind();

			// load users
			users = SecUserManager.GetStaff(clientID);
			CollectionManager.FillCollection(ddlUsers, "UserId", "StaffName", users);
			
			// select user
			ddlUsers.SelectedValue = userID.ToString();

			if (roleID == (int)UserRole.Client || roleID == (int)UserRole.SiteAdministrator) {
				ddlUsers.Enabled = true;
			}
			else {
				// disable dropdown for user
				ddlUsers.Enabled = false;
			}
			

			// load appointment data			
			if (TaskID > 0) {
				fillApppointmentForm(TaskID);
			}
		}

		protected void btnDelete_Click(object sender, EventArgs e) {

			if (TaskID > 0) {
				TasksManager.Delete(TaskID);
			}
			closeWindow();
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			string end_date = null;
			int reminderInterval = 0;
			int leadID = 0;
			string start_date = null;
			Task task = null;
			int userID = 0;

			Page.Validate();
			if (!Page.IsValid)
				return;

			if (TaskID == 0) {
				task = new Task();
			}
			else {
				task = TasksManager.Get(TaskID);
			}

			leadID = SessionHelper.getLeadId();

			// set reminder flag
			if (ucReminderInterval.SelectedIndex > 0) {
				reminderInterval = Convert.ToInt32(ucReminderInterval.SelectedValue);

				task.ReminderInterval = reminderInterval;
			}

			task.IsReminder = (reminderInterval > 0);

			task.creator_id = clientID;

			task.text = txtSubject.Text.Trim();
			task.details = txtDetails.Text.Trim();

			end_date = string.Format("{0} {1}", endDate.Text, endTime.Text);
			start_date = string.Format("{0} {1}", startDate.Text, startTime.Text);

			task.start_date = Convert.ToDateTime(start_date);
			task.end_date = Convert.ToDateTime(end_date);

			task.isAllDay = cbxAllDayEvent.Checked;

			task.status_id = (int)Globals.Task_Status.Active;

			// assign owner
			//if (roleID == (int)UserRole.Client || roleID == (int)UserRole.SiteAdministrator) {
				userID = Convert.ToInt32(ddlUsers.SelectedValue);
				if (userID > 0)
					task.owner_id = userID;	
			//}
			//else
				//task.owner_id = userID;

			if (this.isLead && leadID > 0)
				task.lead_id = leadID;

			//task.policy_type = Convert.ToInt32(ucPolicyType.SelectedValue);
			
            if (ddlPriority.SelectedIndex > 0)
				task.PriorityID = Convert.ToInt32(ddlPriority.SelectedValue);

			TasksManager.Save(task);

			//string js = "<script>window.close();window.opener.location.reload();</script>";
			closeWindow();
		}

		private void closeWindow() {
			string js = "<script>window.close();window.opener.location.reload();</script>";
			//string js = "<script>window.close();window.opener.document.getElementById('btnSubmit').click();</script>";
			ScriptManager.RegisterStartupScript(Page, typeof(Page), "aptkey", js, false);
		}

		private void fillApppointmentForm(int taskID) {
			LeadTask task = TasksManager.GetLeadTask(taskID);

			if (task != null) {
				txtDetails.Text = task.details;
				txtSubject.Text = task.text;

				startDate.Text = ((DateTime)task.start_date).ToShortDateString();
				endDate.Text = ((DateTime)task.end_date).ToShortDateString();

				startTime.Text = ((DateTime)task.start_date).ToString("hh:mm tt");
				endTime.Text = ((DateTime)task.end_date).ToString("hh:mm tt");

				cbxAllDayEvent.Checked = task.isAllDay;

				ddlPriority.SelectedValue = task.priorityID.ToString();

				if (task.owner_id != null)
					ddlUsers.SelectedValue = task.owner_id.ToString();

				//ucPolicyType.SelectedValue = task.policy_type.ToString();

				ucReminderInterval.SelectedValue = task.reminderInterval.ToString();
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			clientID = SessionHelper.getClientId();

			roleID = SessionHelper.getUserRoleId();

			userID = SessionHelper.getUserId();

			if (!Page.IsPostBack) {
				bindData();
			}
		}
	}
}