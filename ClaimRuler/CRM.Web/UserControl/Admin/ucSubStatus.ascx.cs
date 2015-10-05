
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
	public partial class ucSubStatus : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				DoBind();
			}
			if (Session["LeadIds"] == null)
				btnReturnToClaim.Visible = false;
		}

		private void DoBind() {
			int clientID = Core.SessionHelper.getClientId();
			List<SubStatusMaster> lstStatus = null;

			//var predicate = PredicateBuilder.True<SubStatusMaster>();

			lstStatus = SubStatusManager.GetAll(clientID);


			gvSubStatus.DataSource = lstStatus;
			gvSubStatus.DataBind();

		}

		protected void btnReturnToClaim_Click(object sender, EventArgs e) {
			if (Session["LeadIds"] != null) {
				//this.Context.Items.Add("selectedleadid", Session["LeadIds"].ToString());
				//this.Context.Items.Add("view", "");
				//Server.Transfer("~/protected/admin/newlead.aspx");
				Response.Redirect("~/protected/newlead.aspx?id=" + Session["LeadIds"].ToString());
			}
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

					bool exists = SubStatusManager.IsExist(txtSubStatus.Text.Trim(), clientID);
					if (exists) {
						lblMessage.Text = "Sub Status name already exists.";
						lblMessage.Visible = true;
						txtSubStatus.Focus();
						return;
					}
					SubStatusMaster status = SubStatusManager.GetSubStatusId(Convert.ToInt32(hdId.Value));
					status.SubStatusName = txtSubStatus.Text;
					status.Status = true;

					// 2013-09-20
					status.clientID = clientID;

					SubStatusManager.Save(status);
					lblSave.Text = hdId.Value == "0" ? "Record Saved Successfully." : "Record Updated Successfully.";

					lblSave.Visible = true;
					scope.Complete();
				}
				clearFields();
			}
			catch (Exception ex) {
				lblError.Visible = true;
				lblError.Text = "Record Not Saved!";

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
			txtSubStatus.Text = string.Empty;
			hdId.Value = "0";
			DoBind();
		}

		protected void gv_RowCommand(object sender, GridViewCommandEventArgs e) {
			SubStatusMaster leadStatus = null;
			int statusId = 0;

			if (e.CommandName.Equals("DoEdit")) {
				statusId = Convert.ToInt32(e.CommandArgument);

				leadStatus = SubStatusManager.GetSubStatusId(statusId);

				if (leadStatus != null) {
					hdId.Value = statusId.ToString();

					txtSubStatus.Text = leadStatus.SubStatusName;
				}

			}
			else if (e.CommandName.Equals("DoDelete")) {
				// In Case of delete 
				try {

					var status = SubStatusManager.GetSubStatusId(Convert.ToInt32(e.CommandArgument));
					status.Status = false;

					SubStatusManager.Save(status);

					lblSave.Text = "Record Deleted Successfully.";
					lblSave.Visible = true;
				}
				catch (Exception ex) {
					lblError.Text = "Record Not Deleted.";
					lblError.Visible = true;

					Core.EmailHelper.emailError(ex);
				}
			}


		}

		protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			gvSubStatus.PageIndex = e.NewPageIndex;

			DoBind();
		}
	}
}