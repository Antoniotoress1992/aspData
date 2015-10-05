using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LinqKit;
using System.Linq.Expressions;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;


using Infragistics.WebUI.WebSchedule;
using Infragistics.WebUI.Shared;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	// Activities for Client role
	public partial class Activities : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			// check user permission
			//Master.checkPermission("Tasks.aspx");

			if (!Page.IsPostBack) {
				bindData();
			}
			bindEvents();
		}

		private void bindData() {
			int clientID = SessionHelper.getClientId();

			List<UserStaff> staffMembers = SecUserManager.GetStaff(clientID);

			CollectionManager.FillCollection(ddlUsers, "UserId", "StaffName", staffMembers);

		}



		protected void btnExportTask_click(object sender, EventArgs e) {
			Server.Transfer("~/Protected/Admin/TaskExport.aspx");
		}

		protected void btnLogCall_Click(object sender, EventArgs e) {

		}

		public void bindEvents() {
			int clientID = SessionHelper.getClientId();
			int DaysinMonth = 0;
			DateTime fromDate = DateTime.Now;
			DateTime toDate = DateTime.Now; 
			int userID = 0;

			if (ddlUsers.SelectedIndex > 0) {
				userID = Convert.ToInt32(ddlUsers.SelectedValue);

				if (WebDayView1.Visible) {
					fromDate = SchedulerManager.GetScheduleFromDate(WebScheduleInfo1);
					toDate = SchedulerManager.GetScheduleEndDate(WebDayView1);
				}
				else if (WebMonthView1.Visible) {
					fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
					DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
					toDate = fromDate.AddMonths(1).AddTicks(-1); 
				}

				List<LeadTask> appointments = null;

				// get events from all users and show on scheduler 
				Expression<Func<Task, bool>> predicate = PredicateBuilder.True<CRM.Data.Entities.Task>();
				predicate = predicate.And(LeadTask => LeadTask.creator_id == clientID);
				predicate = predicate.And(LeadTask => LeadTask.TaskType == 2);
				predicate = predicate.And(LeadTask => LeadTask.owner_id == userID);

				predicate = predicate.And(LeadTask => LeadTask.start_date >= fromDate && LeadTask.start_date <= toDate);


				appointments = TasksManager.GetEvents(predicate);


				Resource resource = new Resource();

				resource.DataKey = userID.ToString();

				resource.Name = ddlUsers.SelectedItem.Text;

				WebScheduleInfo1.Activities.Clear();
				WebScheduleInfo1.VisibleResources.Clear();

				WebScheduleInfo1.VisibleResources.Add(resource);

				SchedulerManager.BindAppointment(WebScheduleInfo1, appointments);
			}
		}
		
		protected void gvTasks_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e) {

			vw_Task task = null;


			if (e.Row.DataItem != null) {
				task = (vw_Task)e.Row.DataItem;

				if (task.TaskStatusName == "Expired") {
					e.Row.CssClass = "redstar";
				}

				HyperLink hlnkLead = e.Row.Items.FindItemByKey("Subject").FindControl("hlnkLead") as HyperLink;

				if (hlnkLead != null && task.LeadID != null) {
					hlnkLead.Text = task.InsuredName;

					hlnkLead.NavigateUrl = "~/Protected/NewLead.aspx?q=" + Core.SecurityManager.EncryptQueryString(task.LeadID.ToString());
				}
			}

		}

		protected void gvTasks_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e) {
			string[] taskID_TaskType = null;
			string encrypedID = null;
			int taskID = 0;

			if (e.CommandName == "DoEdit") {
				taskID_TaskType = e.CommandArgument.ToString().Split('|');

				encrypedID = SecurityManager.EncryptQueryString(taskID_TaskType[0]);

				if (taskID_TaskType[1] == "1")
					Response.Redirect("~/protected/TaskEdit.aspx?q=" + encrypedID);
				else if (taskID_TaskType[1] == "2")
					Response.Redirect("~/protected/EventEdit.aspx?q=" + encrypedID);
			}
			else if (e.CommandName == "DoDelete") {
				taskID = Convert.ToInt32(e.CommandArgument);
				TasksManager.Delete(taskID);
                Response.Redirect(Request.RawUrl); 
			}

		}

		protected void WebScheduleInfo1_ActiveDayChanged(object sender, ActiveDayChangedEventArgs e) {
			bindEvents();
		}

		protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e) {
			bindEvents();

		}

		protected void ibtn5Days_Click(object sender, ImageClickEventArgs e) {
			td_tasks.Visible = true;

			// hide dayview calendar
			pnlDayView.Visible = true;

			// show monthview calendar
			pnlMonthView.Visible = false;

			WebDayView1.VisibleDays = 5;

			bindEvents();
		}
				
		protected void ibtn1Day_Click(object sender, ImageClickEventArgs e) {
			td_tasks.Visible = true;

			// hide dayview calendar
			pnlDayView.Visible = true;

			// show monthview calendar
			pnlMonthView.Visible = false;

			WebDayView1.VisibleDays = 1;

			bindEvents();
		}

		protected void ibtnMonth_Click(object sender, ImageClickEventArgs e) {
			// hide TD where tasks are shown
			td_tasks.Visible = false;

			// hide dayview calendar
			pnlDayView.Visible = false;

			// show monthview calendar
			pnlMonthView.Visible = true;

			bindEvents();
		}
	}
}