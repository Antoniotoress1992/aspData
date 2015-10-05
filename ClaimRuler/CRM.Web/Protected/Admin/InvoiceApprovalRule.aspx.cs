using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class InvoiceApprovalRule : System.Web.UI.Page {
		List<SecRoleModuleManager.secRoleModuleGet> resRolePermission = new List<SecRoleModuleManager.secRoleModuleGet>();
		int clientID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			clientID = Core.SessionHelper.getClientId();
			Master.checkPermission();

			if (!IsPostBack) {							
				bindData();
			}			
		}

		private void bindPermissions(int roleID) {
			List<SecRoleInvoiceApprovalPermission> rules = null;

			rules = InvoiceApprovalRuleManager.GetAll(roleID);			

			gvInvoiceApprovalRules.DataSource = rules;
			gvInvoiceApprovalRules.DataBind();		
		}

		protected void bindData() {
			List<SecRole> roles = SecRoleManager.GetRolesManagedByClient(clientID);

			CollectionManager.FillCollection(ddlRole, "RoleId", "RoleName", roles);
		}

		protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e) {
			int roleID = Convert.ToInt32(ddlRole.SelectedValue);

			if (roleID > 0) {
				showGridPanel();

				bindPermissions(roleID);
			} else {
				pnlGrid.Visible = false;
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			int id = Convert.ToInt32(ViewState["ID"]);						

			SecRoleInvoiceApprovalPermission rule = null;

			if (id == 0) {
				rule = new SecRoleInvoiceApprovalPermission();

				rule.RoleID = Convert.ToInt32(ddlRole.SelectedValue);

				rule.ClientID = clientID;
			}
			else {
				rule = InvoiceApprovalRuleManager.Get(id);
			}

			if (rule != null) {
				rule.AmountFrom = txtAmountFrom.Value == null ? 0 : Convert.ToDecimal(txtAmountFrom.Value);

				rule.AmountTo = txtAmountTo.Value == null ? 0 : Convert.ToDecimal(txtAmountTo.Value);

				InvoiceApprovalRuleManager.Save(rule);

				showGridPanel();

				bindPermissions(Convert.ToInt32(ddlRole.SelectedValue));
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			int roleID = Convert.ToInt32(ddlRole.SelectedValue);

			showGridPanel();

			bindPermissions(roleID);

		}

		protected void gvInvoiceApprovalRules_RowCommand(object sender, GridViewCommandEventArgs e) {
			int id = Convert.ToInt32(e.CommandArgument);
			SecRoleInvoiceApprovalPermission rule = null;

			if (e.CommandName == "DoEdit") {
				rule = InvoiceApprovalRuleManager.Get(id);							

				if (rule != null) {
					showEditPanel();

					ViewState["ID"] = id.ToString();

					txtAmountFrom.Text = rule.AmountFrom.ToString("N2");

					txtAmountTo.Text = rule.AmountTo.ToString("N2");

					SetFocus(txtAmountFrom);
				}
			}
			else if (e.CommandName == "DoDelete") {
				InvoiceApprovalRuleManager.Delete(id);

				showGridPanel();

				bindPermissions(Convert.ToInt32(ddlRole.SelectedValue));
			}
		}

		private void showEditPanel() {
			pnlEditRule.Visible = true;
			pnlGrid.Visible = false;
		}

		private void showGridPanel() {
			pnlEditRule.Visible = false;
			pnlGrid.Visible = true;
		}

		protected void lbtnNew_Click(object sender, EventArgs e) {
			ViewState["ID"] = "0";

			txtAmountFrom.Text = string.Empty;

			txtAmountTo.Text = string.Empty;

			showEditPanel();
		}
	}
}