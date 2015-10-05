using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using LinqKit;
using System.Linq.Expressions;
using CRM.Core;

using Infragistics.WebUI.WebSchedule;
using Infragistics.WebUI.Shared;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucSchedule : System.Web.UI.UserControl {
		int clientID = 0;
		int roleID = 0;
		int userID = 0;

		private string TypeView {
			get { return ViewState["TypeView"] as string; }
			set { ViewState["TypeView"] = value; }
		}

		private string ActiveResourceKey {
			get { return ViewState["ActiveResourceKey"] as string; }
			set { ViewState["ActiveResourceKey"] = value; }
		}

		protected void Page_Load(object sender, EventArgs e) {
			roleID = SessionHelper.getUserRoleId();
			clientID = SessionHelper.getClientId();
			userID = SessionHelper.getUserId();

			if (!Page.IsPostBack)
				bindData();
		}

		private void bindData() {
			switch (this.dataMode) {
				case schedulerDataMode.Lead:
					bindTasksForLead();
					break;

				default:
					switch (roleID) {

						case (int)UserRole.Client:
						case (int)UserRole.SiteAdministrator:

							bindResourcesForClient();

							bindTasksForClient();
							break;

						case (int)UserRole.User:
						case (int)UserRole.Adjuster:

							bindTasksForUser();
							break;

						default:
							break;
					}
					break;
			}
		}

		private void bindResourcesForClient() {
			//int luserID = 0;

            List<CRM.Data.Entities.UserStaff> staffMembers = SecUserManager.GetStaff(clientID);

			if (staffMembers != null && staffMembers.Count > 0) {
				ActiveResourceKey = staffMembers[0].UserId.ToString();

				foreach (UserStaff user in staffMembers) {
					Resource resource = new Resource();
					resource.DataKey = user.UserId.ToString();
					resource.Name = user.StaffName;
					resource.EmailAddress = user.EmailAddress;
					WebScheduleInfo1.VisibleResources.Add(resource);
				}

				//WebScheduleInfo1.VisibleResources[0] == unassigned
				if (ActiveResourceKey != null) {
					Resource activeResource = WebScheduleInfo1.VisibleResources.GetResourceFromKey(ActiveResourceKey);
					WebScheduleInfo1.ActiveResourceName = activeResource.Name;
				}
				else {
					WebScheduleInfo1.ActiveResourceName = WebScheduleInfo1.VisibleResources[1].Name;

					ActiveResourceKey = WebScheduleInfo1.VisibleResources[1].DataKey.ToString();
				}

				//luserID = Convert.ToInt32(WebScheduleInfo1.VisibleResources[1].DataKey);

				//DateTime fromDate = SchedulerManager.GetScheduleFromDate(WebScheduleInfo1);

				//DateTime toDate = SchedulerManager.GetScheduleEndDate(WebScheduleInfo1, this.TypeView);
			}

		}

		private void bindTasksForClient() {
			DateTime fromDate = SchedulerManager.GetScheduleFromDate(WebScheduleInfo1);
			DateTime toDate = SchedulerManager.GetScheduleEndDate(WebDayView1);

			List<LeadTask> appointments = null;

			// active resouce
			int userID = Convert.ToInt32(ActiveResourceKey);

			// get taks and show in scheduler as appointment
			Expression<Func<Task, bool>> predicate = PredicateBuilder.True<CRM.Data.Entities.Task>();
			predicate = predicate.And(LeadTask => LeadTask.start_date >= fromDate && LeadTask.end_date <= toDate);
			predicate = predicate.And(LeadTask => LeadTask.creator_id == clientID);
			appointments = TasksManager.GetLeadTask(predicate, fromDate, toDate);

			SchedulerManager.BindAppointment(WebScheduleInfo1, appointments);
		}

		private void bindTasksForLead() {
			int leadID = SessionHelper.getLeadId();
			Resource resource = null;

			if (leadID > 0) {
				resource = new Resource();
				resource.DataKey = leadID.ToString();
				resource.Name = SessionHelper.getClaimantName();				
				WebScheduleInfo1.VisibleResources.Add(resource);

				WebScheduleInfo1.ActiveResourceName = WebScheduleInfo1.VisibleResources[1].Name;

				ActiveResourceKey = WebScheduleInfo1.VisibleResources[1].DataKey.ToString();

				DateTime fromDate = SchedulerManager.GetScheduleFromDate(WebScheduleInfo1);
				DateTime toDate = SchedulerManager.GetScheduleEndDate(WebDayView1);

				List<LeadTask> appointments = null;

				// get tasks and show them in scheduler as appointments
				Expression<Func<Task, bool>> predicate = PredicateBuilder.True<CRM.Data.Entities.Task>();
				predicate = predicate.And(LeadTask => LeadTask.start_date >= fromDate && LeadTask.end_date <= toDate);
				predicate = predicate.And(LeadTask => LeadTask.lead_id == leadID);
				appointments = TasksManager.GetLeadTask(predicate, fromDate, toDate);

				// set resourcekey to lead id
				if (appointments != null && appointments.Count > 0) {
					appointments.ForEach(x => x.resourceKey = leadID);
				}

				SchedulerManager.BindAppointment(WebScheduleInfo1, appointments);
				
			}
		}

		private void bindTasksForUser() {
			DateTime fromDate = SchedulerManager.GetScheduleFromDate(WebScheduleInfo1);
			DateTime toDate = SchedulerManager.GetScheduleEndDate(WebDayView1);

			List<LeadTask> appointments = null;

			// get taks and show in scheduler as appointment
			Expression<Func<Task, bool>> predicate = PredicateBuilder.True<CRM.Data.Entities.Task>();
			predicate = predicate.And(LeadTask => LeadTask.start_date >= fromDate && LeadTask.end_date <= toDate);
			predicate = predicate.And(LeadTask => LeadTask.owner_id == userID);
			appointments = TasksManager.GetLeadTask(predicate, fromDate, toDate);

			SchedulerManager.BindAppointment(WebScheduleInfo1, appointments);
		}

		protected void changeCalendarView_Click(object sender, EventArgs e) {
			if (sender == btn1Day)
				this.WebDayView1.VisibleDays = 1;
			else if (sender == btn5Days)
				this.WebDayView1.VisibleDays = 5;

			bindData();
		}

		public enum schedulerDataMode { Lead, User }
		public schedulerDataMode dataMode {
			get {
				return (schedulerDataMode)ViewState["schedule_dataMode"];
			}
			set { ViewState["schedule_dataMode"] = value; }
		}

		public bool showResizeFullButton {
			get {
				return ViewState["btnResizeFull"] == null ? true : (bool)ViewState["btnResizeFull"];
			}
			set {
				ViewState["btnResizeFull"] = value;
				this.btnResizeFull.Visible = value;

			}
		}

		// client/admin changed to another user tab
		protected void WebScheduleInfo1_ActiveResourceChanged(object sender, ActiveResourceChangedEventArgs e) {
			List<LeadTask> tasks = null;

			DateTime fromDate = SchedulerManager.GetScheduleFromDate(WebScheduleInfo1);
			DateTime toDate = SchedulerManager.GetScheduleEndDate(WebDayView1);
			int userID = Convert.ToInt32(e.ResourceDataKey);

			// save it in viewstate
			ActiveResourceKey = e.ResourceDataKey;

			Expression<Func<Task, bool>> predicate = PredicateBuilder.True<CRM.Data.Entities.Task>();
			predicate = predicate.And(LeadTask => LeadTask.start_date >= fromDate && LeadTask.end_date <= toDate);
			predicate = predicate.And(LeadTask => LeadTask.owner_id == userID);
			tasks = TasksManager.GetLeadTask(predicate, fromDate, toDate);

			SchedulerManager.BindAppointment(WebScheduleInfo1, tasks);
			//SchedulerManager.BindUserAppointments(WebScheduleInfo1, userID, fromDate, toDate);
		}

		// user changes date on schedule
		protected void WebScheduleInfo1_ActiveDayChanged(object sender, ActiveDayChangedEventArgs e) {
			bindData();
			//Resource activeUserResource = null;
			//int luserID = 0;

			//DateTime fromDate = e.NewDate.Value;//SchedulerManager.GetScheduleFromDate(WebScheduleInfo1);
			//DateTime toDate = SchedulerManager.GetScheduleEndDate(WebDayView1);


			//switch (roleID) {
			//	case (int)UserRole.Client:
			//	case (int)UserRole.SiteAdministrator:
			//		activeUserResource = ((WebScheduleInfo)sender).ActiveResource;
			//		luserID = Convert.ToInt32(activeUserResource.DataKey);

			//		// save it in viewstate
			//		ActiveResourceKey = activeUserResource.DataKey.ToString();

			//		bindTasksForClient();
			//		break;

			//	default:
			//		bindTasksForUser();
			//		break;
			//}



			////Expression<Func<Task, bool>> predicate = PredicateBuilder.True<CRM.Data.Task>();
			////predicate = predicate.And(LeadTask => LeadTask.start_date >= fromDate && LeadTask.end_date <= toDate);
			////predicate = predicate.And(LeadTask => LeadTask.owner_id == luserID);
			////tasks = TasksManager.GetLeadTask(predicate, fromDate, toDate);

			////SchedulerManager.BindAppointment(WebScheduleInfo1, tasks);
		}

	}
}