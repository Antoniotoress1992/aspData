using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;

using CRM.Data.Account;
using CRM.Data;
using LinqKit;
using CRM.Core;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class SubLimitOfLiability : System.Web.UI.Page {
		List<SecRoleModuleManager.secRoleModuleGet> resRolePermission = new List<SecRoleModuleManager.secRoleModuleGet>();
		int clientID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			clientID = Core.SessionHelper.getClientId();

			Form.DefaultButton = btnSave.UniqueID;

			txtSublimit.Focus();

			if (!IsPostBack) {
				DoBind();
			}
		}

		private void DoBind() {
			List<SubLimitOfLiabilityMaster> limits = null;

			limits = SubLimitOfLiabilityManager.GetAll(clientID);

			gvSublimit.DataSource = limits;
			gvSublimit.DataBind();

		}

		protected void btnSave_Click(object sender, EventArgs e) {
			SubLimitOfLiabilityMaster sublimit = null;

			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;

			int clientID = Core.SessionHelper.getClientId();
			
			int id = Convert.ToInt32(hdId.Value);

			try {
				using (TransactionScope scope = new TransactionScope()) {

					bool exists = SubLimitOfLiabilityManager.IsExist(txtSublimit.Text.Trim(), clientID);
					if (exists) {
						lblMessage.Text = "Sub-Limit of Liability already exists.";
						lblMessage.Visible = true;
						txtSublimit.Focus();
						return;
					}
					if (id == 0)
						sublimit = new SubLimitOfLiabilityMaster();
					else
						sublimit = SubLimitOfLiabilityManager.Get(id);

					sublimit.Description = txtSublimit.Text;
					sublimit.IsActive = true;

					if (clientID > 0)
						sublimit.ClientId = clientID;

					SubLimitOfLiabilityManager.Save(sublimit);

					lblMessage.Text = hdId.Value == "0" ? "Record Saved Successfully." : "Record Updated Successfully.";
					lblMessage.CssClass = "ok";
					scope.Complete();
				}
				clearFields();
			}
			catch (Exception ex) {
				lblMessage.Text = "Record not saved!";

				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}
		}

		private void clearFields() {
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;

			txtSublimit.Text = string.Empty;

			hdId.Value = "0";

			DoBind();
		}

		protected void gv_RowCommand(object sender, GridViewCommandEventArgs e) {
			SubLimitOfLiabilityMaster sublimit = null;
			int id = Convert.ToInt32(e.CommandArgument);

			if (e.CommandName.Equals("DoEdit")) {				
				sublimit = SubLimitOfLiabilityManager.Get(id);

				if (sublimit != null) {
					hdId.Value = id.ToString();

					txtSublimit.Text = sublimit.Description;					
				}

			}
			else if (e.CommandName.Equals("DoDelete")) {
				// In Case of delete 
				try {
					sublimit = SubLimitOfLiabilityManager.Get(id);
					if (sublimit != null) {
						sublimit.IsActive = false;

						SubLimitOfLiabilityManager.Save(sublimit);

						lblMessage.Text = "Record Deleted Successfully.";
						lblMessage.Visible = true;
						lblMessage.CssClass = "ok";

						DoBind();
					}
				}
				catch (Exception ex) {
					lblMessage.Text = "Record Not Deleted.";
					lblMessage.Visible = true;
					lblMessage.CssClass = "error";

					Core.EmailHelper.emailError(ex);
				}
			}


		}

		protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			gvSublimit.PageIndex = e.NewPageIndex;

			DoBind();
		}
	}
}