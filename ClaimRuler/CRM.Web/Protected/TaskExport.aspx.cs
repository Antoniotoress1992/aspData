using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;


namespace CRM.Web.Protected {
	public partial class TaskExport : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			
			// check user permission
			Master.checkPermission();


			if (!Page.IsPostBack) {
				bindTasks();
			}
		}

		protected void bindTasks() {
			int userID = 0;
			int clientID = 0;

			List<LeadTask> tasks = null;

			int roleID = Core.SessionHelper.getUserRoleId();

			switch (roleID) {
				case (int)UserRole.Administrator:
					tasks = TasksManager.GetAll();
					break;

				case (int)UserRole.Client:
				case (int)UserRole.SiteAdministrator:
					clientID = Core.SessionHelper.getClientId();

					tasks = TasksManager.GetLeadTaskByClientId(clientID);
					break;

				default:
					userID = Core.SessionHelper.getUserId();

					tasks = TasksManager.GetLeadTaskByUserID(userID);
					break;
			}

			
			gvTasks.DataSource = tasks;

			gvTasks.DataBind();
		}

		protected void btnExport_click(object sender, EventArgs e) {
			int userID = 0;
			List<LeadTask> tasks = null;
            CRM.Data.Entities.SecUser user = null;

			userID = Core.SessionHelper.getUserId();

			if (userID > 0) {
				tasks = getSelectedTasks();

				user = SecUserManager.GetByUserId(userID);

				if (user != null && user.Email != null && user.emailPassword != null) {
					try {
						Core.GCalendarHelper.export(user.Email, Core.SecurityManager.Decrypt(user.emailPassword), tasks);

						lblMessage.Text = "Export completed.";
						lblMessage.CssClass = "ok";
					}
					catch (Exception ex) {
						lblMessage.CssClass = "error";
						lblMessage.Text = "Export failed.";
					}
				}
			}

			//help.ExportToGoogleCalendar("https://www.google.com/calendar/feeds/default/private/full", tasks);
		}

		protected void btnClose_click(object sender, EventArgs e) {
			Response.Redirect("~/Protected/Tasks.aspx");
		}

		private List<LeadTask> getSelectedTasks() {
			List<LeadTask> tasks = new List<LeadTask>();
			LeadTask task = null;
			DateTime date = DateTime.MinValue;

			foreach (GridViewRow row in gvTasks.Rows) {
				if (row.RowType == DataControlRowType.DataRow) {
					CheckBox cbxExport = row.FindControl("cbxExport") as CheckBox;

					if (cbxExport != null && cbxExport.Checked) {
						task = new LeadTask();
						Label start_date = row.FindControl("start_date") as Label;

						DateTime.TryParse(start_date.Text, out date);
						task.start_date = date;

						Label end_date = row.FindControl("end_date") as Label;

						DateTime.TryParse(end_date.Text, out date);
						task.end_date = date;

						Label title = row.FindControl("event") as Label;
						task.text = title.Text;

						Label details = row.FindControl("details") as Label;
						task.details = details.Text;

						tasks.Add(task);
					}
				}
			}
			return tasks;
		}
	}
}