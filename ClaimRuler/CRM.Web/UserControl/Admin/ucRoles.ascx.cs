
namespace CRM.Web.UserControl.Admin {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using CRM.Data.Account;
	using CRM.Data;
	using System.Transactions;
    using CRM.Data.Entities;

	public partial class ucRoles : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;
		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;

			if (!IsPostBack) {

				DoBind();
			}			
		}

		private void DoBind() {
			gvData.DataSource = SecRoleManager.GetAll();
			gvData.DataBind();
		}

		protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e) {
			//lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Visible = false;
			if (e.CommandName.Equals("DoEdit")) {
				pnlEdit.Enabled = true;
				pnlList.Enabled = false;
				pnlEdit.Visible = true;
				lbnNew.Visible = false;
				SecRole role = SecRoleManager.GetByRoleId(Convert.ToInt32(e.CommandArgument));
				hdId.Value = role.RoleId.ToString();
				txtRoleName.Text = role.RoleName;
				txtRoleDescription.Text = role.RoleDescription;
				ddlStatus.SelectedValue = role.Status == true ? "1" : "0";

				//cbxIsclient.Checked = role.isClient ?? false;

				ddlStatus.Focus();
				btnSave.Enabled = true;
			}
			else if (e.CommandName.Equals("DoDelete")) {
				try {
					using (TransactionScope scope = new TransactionScope()) {
						pnlList.Enabled = true;
						pnlEdit.Enabled = false;

						SecRole role = SecRoleManager.GetByRoleId(Convert.ToInt32(e.CommandArgument));
						role.Status = false;
						SecRoleManager.Save(role);
						scope.Complete();
					}
					DoBind();
					//
					lblSave.Visible = true;
					lblSave.Text = "Role has been deleted.";
				}
				catch (Exception ex) {
					lblError.Visible = true;
					lblError.Text = "Role was not deleted!";
				}
			}
		}

		private void ResetForm() {
			txtRoleName.Text = string.Empty;
			txtRoleDescription.Text = string.Empty;
			ddlStatus.SelectedIndex = 0;
			hdId.Value = "0";
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Visible = false;
			DoBind();

		}

		protected void lbnNew_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Visible = false;
			pnlEdit.Enabled = true;
			pnlList.Enabled = false;
			hdId.Value = "0";
			pnlEdit.Visible = true;
			ddlStatus.Focus();

			//cbxIsclient.Checked = false;

			lbnNew.Visible = false;
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Visible = false;
			try {
				using (TransactionScope scope = new TransactionScope()) {
					SecRole role = SecRoleManager.GetByRoleId(Convert.ToInt32(hdId.Value));
					role.RoleName = txtRoleName.Text;
					role.RoleDescription = txtRoleDescription.Text;
					role.Status = ddlStatus.SelectedValue == "1" ? true : false;

					//role.isClient = cbxIsclient.Checked;

					SecRoleManager.Save(role);
					btnCancel_Click(null, null);
					lblSave.Visible = true;
					lblSave.Text = "Record Saved Successfully.";
					//Label1.Text = "Record Saved";
					scope.Complete();
				}
			}
			catch (Exception ex) {
				lblError.Visible = true;
				lblError.Text = "Record Not Saved !!!";
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			pnlEdit.Enabled = false;
			pnlList.Enabled = true;
			pnlEdit.Visible = false;
			lbnNew.Visible = true;
			ResetForm();
		}
		protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			lblError.Text = string.Empty;
			gvData.PageIndex = e.NewPageIndex;
			DoBind();
		}

		protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e) {
			if (e.Row.RowType == DataControlRowType.DataRow) {
				ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
				if (imgDelete != null) {
					if (imgDelete.Enabled == false)
						imgDelete.Style.Add(HtmlTextWriterStyle.Cursor, "default");
				}
			}
		}

	}
}