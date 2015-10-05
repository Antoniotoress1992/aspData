using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Web.Utilities;
using CRM.Core;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	[ValidationProperty("SelectedValue")]
	public partial class ucReminderInterval : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {
			if (!Page.IsPostBack) {
				bindData();
			}
			
			
		}

		private void bindData() {
			int clientID = SessionHelper.getClientId();

            List<CRM.Data.Entities.TaskReminderMaster> taskReminderMasters = null;

			taskReminderMasters = TaskReminderMasterManager.GetAll(clientID);

			ddlReminderInterval.DataSource = taskReminderMasters;
			ddlReminderInterval.DataBind();
		}

		
		public string SelectedValue {
			get { return ddlReminderInterval.SelectedValue; }
			set { ddlReminderInterval.SelectedValue = value; }
		}

		public int SelectedIndex {
			get { return ddlReminderInterval.SelectedIndex; }
			set { ddlReminderInterval.SelectedIndex = value; }
		}
	}
}