using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Data.Account;
using CRM.Data;
using LinqKit;
using CRM.Core;
using System.Globalization;
using System.Transactions;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucLeadSource : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;
		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;
			
			this.Page.Form.DefaultButton = btnSave.UniqueID;

			if (!IsPostBack) {

				DoBind();
			}
		
		}
		protected void btnReturnToClaim_Click(object sender, EventArgs e) {
			if (Session["LeadIds"] != null) {				
				Response.Redirect("~/protected/newlead.aspx?id=" + Session["LeadIds"].ToString());
			}
		}

		private void DoBind() {
			List<LeadSourceMaster> lstLeadSource = null;
			// 2013-10-25 tortega
			int clientID = Core.SessionHelper.getClientId();
			int roleID = Core.SessionHelper.getUserRoleId();

			// 2013-10-25 tortega
			if (roleID == (int)UserRole.Administrator)
				lstLeadSource = LeadSourceManager.GetAll();
			else
				lstLeadSource = LeadSourceManager.GetAll(clientID);

			gvSources.DataSource = lstLeadSource;
			gvSources.DataBind();
		}

		protected void gv_RowCommand(object sender, GridViewCommandEventArgs e) {
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			lblError.Visible = false;
			lblSave.Visible = false;
			if (e.CommandName.Equals("DoEdit")) {
				int leadSourceId = Convert.ToInt32(e.CommandArgument);
				hdId.Value = leadSourceId.ToString();

				LeadSourceMaster leadSource = LeadSourceManager.GetLeadSourceId(leadSourceId);
				if (leadSource != null)
					txtLeadSource.Text = leadSource.LeadSourceName;

			}
			else if (e.CommandName.Equals("DoDelete")) {
				// In Case of delete 
				try {
				
					var lSource = LeadSourceManager.GetLeadSourceId(Convert.ToInt32(e.CommandArgument));
					lSource.Status = false;
					LeadSourceManager.Save(lSource);
					btnCancel_Click(null, null);
					lblSave.Text = "Lead Source deleted successfully.";
					lblSave.Visible = true;
				}
				catch (Exception ex) {
					lblError.Text = "Lead Source not deleted!";
					lblError.Visible = true;

					Core.EmailHelper.emailError(ex);
				}
			}
		}

		protected void gv_onSorting(object sender, GridViewSortEventArgs e) {
			List<LeadSourceMaster> sources = null;
			int clientID = Core.SessionHelper.getClientId();

			sources = LeadSourceManager.GetAll(clientID);
		
			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			gvSources.DataSource = sources.AsQueryable().orderByExtension(e.SortExpression, descending);

			gvSources.DataBind();

		}

		
		protected void btnSave_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;

			int clientID = Core.SessionHelper.getClientId();

			try {
				using (TransactionScope scope = new TransactionScope()) {

					bool exists = LeadSourceManager.IsExist(txtLeadSource.Text.Trim(), Convert.ToInt32(hdId.Value));
					if (exists) {
						lblMessage.Text = "Lead Source already exists!";
						lblMessage.Visible = true;
						txtLeadSource.Focus();
						return;
					}

					LeadSourceMaster lSource = LeadSourceManager.GetLeadSourceId(Convert.ToInt32(hdId.Value));
					lSource.LeadSourceName = txtLeadSource.Text;
					lSource.ClientId = clientID;
					lSource.Status = true;

					LeadSourceManager.Save(lSource);

					lblSave.Text = hdId.Value == "0" ? "Lead Source saved successfully." : "Lead Source updated successfully.";
					btnCancel_Click(null, null);
					lblSave.Visible = true;
					scope.Complete();
				}
				clearFields();
			}
			catch (Exception ex) {
				lblError.Visible = true;
				lblError.Text = "Lead Source not saved!";

				Core.EmailHelper.emailError(ex);
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			clearFields();
		}

		private void clearFields() {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
			txtLeadSource.Text = string.Empty;
			hdId.Value = "0";

			DoBind();
		}
	}
}