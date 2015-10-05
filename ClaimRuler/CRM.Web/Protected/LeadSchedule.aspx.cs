using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;

using CRM.Web.Utilities;
using CRM.Data.Entities;

namespace CRM.Web.Protected {
	public partial class LeadSchedule : System.Web.UI.Page {
	
		protected void Page_Load(object sender, EventArgs e) {			
			int leadID = 0;
			
			// check user permission
			//Master.checkPermission();

			if (!Page.IsPostBack) {
				leadID = Core.SessionHelper.getLeadId();

				if (leadID > 0) {
					Leads lead = LeadsManager.GetByLeadId(leadID);



					lblClaimant.Text = lead.ClaimantFirstName + " " + lead.ClaimantLastName;

					bindTasks();
				}
			}
		}
		
		protected void btnExportTask_click(object sender, EventArgs e) {
			Server.Transfer("~/Protected/Admin/TaskExport.aspx");
		}

        
		
		protected void bindTasks() {
		}
	}
}