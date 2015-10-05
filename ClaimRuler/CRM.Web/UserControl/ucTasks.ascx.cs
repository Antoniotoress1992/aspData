using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Core;
using CRM.Data.Entities;

namespace CRM.Web.UserControl {
	public partial class ucTasks : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;
		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;
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

			if (e.CommandName == "DoEdit") {
				taskID_TaskType = e.CommandArgument.ToString().Split('|');

				encrypedID = SecurityManager.EncryptQueryString(taskID_TaskType[0]);

				if (taskID_TaskType[1] == "1")
					Response.Redirect("~/protected/TaskEdit.aspx?q=" + encrypedID);
				else if (taskID_TaskType[1] == "2")
					Response.Redirect("~/protected/EventEdit.aspx?q=" + encrypedID);
			}
			else if (e.CommandName == "DoDelete") {

			}

		}

	}
}