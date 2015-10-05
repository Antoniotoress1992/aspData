using System;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class ClientRolesManager : System.Web.UI.Page {
		List<SecRoleModuleManager.secRoleModuleGet> roleModules = null;
		int clientID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			clientID = Core.SessionHelper.getClientId();

			if (!Page.IsPostBack) {				
				doBind();
			}
		}

		protected void doBind() {
			List<SecRole> roles = SecRoleManager.GetRolesManagedByClient(clientID);
						
			gvClientRoles.DataSource = roles;

			gvClientRoles.DataBind();
		}

		private void bindRoleActions(int roleID) {
			List<Data.Entities.Action> actions = null;
			List<RoleAction> roleActions = null;

			clientID = SessionHelper.getClientId();

			// bind actions
			using (ActionManager repository = new ActionManager()) {
				actions = repository.GetActions();

				roleActions = repository.GetRoleActions(clientID, roleID);
			}

			// show all actions
			CollectionManager.Fillchk(cblRoleActions, "ActionID", "ActionName", actions);

			if (roleActions != null && roleActions.Count > 0) {

				foreach (RoleAction roleAction in roleActions) {
					ListItem item = cblRoleActions.Items.FindByValue(roleAction.ActionID.ToString());
					if (item != null)
						item.Selected = true;
				}
			}

		}

		private void bindRoleModules(int roleID) {
			// get all modules client can edit
			roleModules = SecRoleModuleManager.getRoleModule(clientID, roleID);
		
			// get those main menu items
			List<SecRoleModuleManager.secRoleModuleGet> parentModules = (from x in roleModules where (x.ParentId == null || x.ParentId == 0) orderby x.ModuleName select x).ToList();

			// show them on grid
			gvModules.DataSource = parentModules;
			gvModules.DataBind();
		}
		
		protected void gvModules_RowDataBound(object sender, GridViewRowEventArgs e) {
			int roleID = Convert.ToInt32(ViewState["RoleID"]);
			GridView gvModulePermission = null;
			SecRoleModuleManager.secRoleModuleGet parentModule = null;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				parentModule = e.Row.DataItem as SecRoleModuleManager.secRoleModuleGet;

				List<SecRoleModuleManager.secRoleModuleGet> childrenModules = roleModules.Where(x => (x.ParentId == parentModule.ModuleId)).ToList();

				gvModulePermission = e.Row.FindControl("gvModulePermission") as GridView;

				gvModulePermission.DataSource = childrenModules;
				gvModulePermission.DataBind();
			}
		}


		protected void gvClientRoles_RowCommand(object sender, GridViewCommandEventArgs e) {
			
			int roleID = Convert.ToInt32(e.CommandArgument);
			SecRole role = null;

			if (e.CommandName == "DoEdit") {
				role = SecRoleManager.GetByRoleId(roleID);

				if (role != null) {
					ViewState["RoleID"] = roleID.ToString();

					showEditPanel();

					lbtnSelectAll.Visible = true;

					txtRoleDescription.Text = role.RoleDescription;
					txtRoleName.Text = role.RoleName;

					if (role.ClientID == null) {
						// prevent system wide roles from being edited by user
						txtRoleDescription.Enabled = false;
						txtRoleName.Enabled = false;
					}
					else {
						txtRoleDescription.Enabled = true;
						txtRoleName.Enabled = true;
					}

					bindRoleModules(roleID);

					bindRoleActions(roleID);
				}

			}
			else if (e.CommandName == "DoDelete") {
				role = SecRoleManager.GetByRoleId(roleID);

				if (role != null) {
					role.Status = false;

					SecRoleManager.Save(role);

					bindRoleModules(roleID);
				}

			}
		}


		protected void btnCancel_Click(object sender, EventArgs e) {
			pnlEditPanel.Visible = false;
			pnlGridPanel.Visible = true;

			doBind();
		}


		protected void btnSave_Click(object sender, EventArgs e) {
			int roleID = Convert.ToInt32(ViewState["RoleID"]);

			SecRole role = null;

			Page.Validate("Role");
			if (!Page.IsValid)
				return;


			clientID = Core.SessionHelper.getClientId();

			if (roleID == 0) {
				role = new SecRole();
				role.ClientID = clientID;
				role.Status = true;
			}
			else {
				role = SecRoleManager.GetByRoleId(roleID);
			}

			if (role != null) {
				role.RoleDescription = txtRoleDescription.Text;

				role.RoleName = txtRoleName.Text;

				try {
					using (TransactionScope scope = new TransactionScope()) {
						role = SecRoleManager.Save(role);

						SecRoleModuleManager.deleteRoleModules(clientID, roleID);

						saveRoleModules(clientID, roleID);

						saveRoleActions(clientID, roleID);

						// complete transaction
						scope.Complete();
					}					

					// return user to grid
					showGridPanel();

					doBind();
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}
		}


		protected void btnNewRole_Click(object sender, EventArgs e) {
			ViewState["RoleID"] = "0";

			txtRoleDescription.Text = string.Empty;

			txtRoleName.Text = string.Empty;

			txtRoleName.Enabled = true;

			txtRoleDescription.Enabled = true;

			showEditPanel();

			lbtnSelectAll.Visible = false;

			// clear grid
			gvModules.DataSource = null;
			gvModules.DataBind();
		}

		private void saveRoleActions(int clientID, int roleID) {
			RoleAction roleAction = null;

			using (ActionManager repository = new ActionManager()) {
				repository.DeleteAll(clientID, roleID);

				foreach (ListItem item in cblRoleActions.Items) {
					if (item.Selected) {
						roleAction = new RoleAction();
						roleAction.ClientID = clientID;
						roleAction.RoleID = roleID;
						roleAction.ActionID = Convert.ToInt32(item.Value);

						repository.Save(roleAction);
					}
				}
			}
		}

		private void saveRoleModules(int clientID, int roleID) {
			SecRoleModule roleModule = null;

			foreach (GridViewRow row in gvModules.Rows) {
				if (row.RowType == DataControlRowType.DataRow) {
					CheckBox cbxModule = row.FindControl("cbxModule") as CheckBox;

					if (cbxModule.Checked) {
						roleModule = new SecRoleModule();

						roleModule.ClientID = clientID;
						roleModule.RoleID = roleID;

						roleModule.ModuleID = (int)gvModules.DataKeys[row.RowIndex].Value;
						roleModule.Status = 1;

						roleModule.ViewPermission = cbxModule.Checked;
						roleModule.AddPermssion = true;
						roleModule.DeletePermission = false;
						roleModule.EditPermission = true;
						
						SecRoleModuleManager.SaveRoleModule(roleModule);

						GridView gvModulePermission = row.FindControl("gvModulePermission") as GridView;

						// save
						saveChildrenModules(roleModule, gvModulePermission);
					}
				}
			}
		}

        private void saveChildrenModules(SecRoleModule parentModule, GridView gvModulePermission)
        {
			SecRoleModule roleModule = null;

			foreach (GridViewRow row in gvModulePermission.Rows) {
				if (row.RowType == DataControlRowType.DataRow) {
					CheckBox cbxViewPermission = row.FindControl("cbxViewPermission") as CheckBox;
					CheckBox cbxEditPermission = row.FindControl("cbxEditPermission") as CheckBox;
					CheckBox cbxAddPermission = row.FindControl("cbxAddPermission") as CheckBox;
					CheckBox cbxDeletePermission = row.FindControl("cbxDeletePermission") as CheckBox;

					if (cbxViewPermission.Checked || cbxEditPermission.Checked || cbxEditPermission.Checked || cbxDeletePermission.Checked) {						
						// save child
						roleModule = new SecRoleModule();

						roleModule.ClientID = parentModule.ClientID;
						roleModule.RoleID = parentModule.RoleID;

						roleModule.ModuleID = (int)gvModulePermission.DataKeys[row.RowIndex].Value;
						roleModule.Status = 1;

						roleModule.ViewPermission = cbxViewPermission.Checked;
						roleModule.AddPermssion = cbxAddPermission != null ? cbxAddPermission.Checked : false;
						roleModule.DeletePermission = cbxDeletePermission != null ? cbxDeletePermission.Checked : false; ;
						roleModule.EditPermission = cbxEditPermission != null ? cbxEditPermission.Checked : false;

						SecRoleModuleManager.SaveRoleModule(roleModule);
					}
				}
			}

		}

		private void processTreeView(TreeNodeCollection parentNode) {
			SecRoleModule roleModule = null;

			if (parentNode != null && parentNode != null && parentNode.Count > 0) {
				foreach (TreeNode node in parentNode) {
					if (node.Checked) {
						roleModule = new SecRoleModule();
						roleModule.ModuleID = Convert.ToInt32(node.Value);
						roleModule.RoleID = Convert.ToInt32(ViewState["RoleID"]);
						roleModule.Status = 1;

						roleModule.ViewPermission = true;
						roleModule.AddPermssion = false;
						roleModule.DeletePermission = false;
						roleModule.EditPermission = false;

						SecRoleModuleManager.SaveRoleModule(roleModule);
					}
					processTreeView(node.ChildNodes);
				}
			}
		}

		private void showEditPanel() {
			pnlGridPanel.Visible = false;
			pnlEditPanel.Visible = true;

			SetFocus(txtRoleName);
		}

		private void showGridPanel() {
			pnlGridPanel.Visible = true;
			pnlEditPanel.Visible = false;
		}

		protected void lbtnSelectAll_Click(object sender, EventArgs e) {
			foreach (GridViewRow row in gvModules.Rows) {
				// process top level grid
				if (row.RowType == DataControlRowType.DataRow) {
					CheckBox cbxModule = row.FindControl("cbxModule") as CheckBox;

					// check module
					cbxModule.Checked = true;

					// process inner grid
					GridView gvModulePermission = row.FindControl("gvModulePermission") as GridView;

					foreach (GridViewRow innerRow in gvModulePermission.Rows) {
						if (row.RowType == DataControlRowType.DataRow) {
							CheckBox cbxViewPermission = innerRow.FindControl("cbxViewPermission") as CheckBox;
							CheckBox cbxEditPermission = innerRow.FindControl("cbxEditPermission") as CheckBox;
							CheckBox cbxAddPermission = innerRow.FindControl("cbxAddPermission") as CheckBox;
							CheckBox cbxDeletePermission = innerRow.FindControl("cbxDeletePermission") as CheckBox;

							cbxViewPermission.Checked = true;
							cbxEditPermission.Checked = true;
							cbxAddPermission.Checked = true;
							cbxDeletePermission.Checked = true;
						}
					}
				}
				
			}
		}


	}
}