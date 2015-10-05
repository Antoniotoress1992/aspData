
namespace CRM.Web.UserControl.Admin {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;

	using CRM.Data;
	using CRM.Data.Account;
	using CRM.Core;
	using System.Linq.Expressions;
	using System.Transactions;
    using CRM.Data.Entities;
	public partial class ucRoleModule : System.Web.UI.UserControl {
		List<SecRoleModuleManager.secRoleModuleGet> resRolePermission = new List<SecRoleModuleManager.secRoleModuleGet>();
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				bindDDL();
				
			}			
		}
		private void bindDDL() {
			// 2013-03-08
			//CollectionManager.FillCollection(ddlRole, "RoleId", "RoleName", SecRoleManager.GetAll().Where(x => (x.RoleId > 2) && x.Status==true));
			CollectionManager.FillCollection(ddlRole, "RoleId", "RoleName", SecRoleManager.GetSystemRoles());

		}


		List<SecRoleModuleManager.secRoleModuleGet> resRoleModule = new List<SecRoleModuleManager.secRoleModuleGet>();
		protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e) {

			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblError.Visible = false;
			if (ddlRole.SelectedIndex > 0) {
				bindGrid(Convert.ToInt32(ddlRole.SelectedValue));
				btnSave.Visible = true;
				pnlList.Visible = true;
			}
			else {
				btnSave.Visible = false;
				pnlList.Visible = false;
			}
		}
		private void bindGrid(int roleId) {
			resRoleModule = SecRoleModuleManager.getRoleModule(roleId);

			grdModule.DataSource = resRoleModule.Where(x => (x.ParentId == 0 || x.ParentId == null)).ToList();
			grdModule.DataBind();
		}
		protected void grdModule_RowDataBound(object sender, GridViewRowEventArgs e) {
			if (e.Row.RowType == DataControlRowType.DataRow) {

				GridView gvSubModule = (GridView)e.Row.FindControl("gvSubModule");
				HiddenField hfModuleId = (HiddenField)e.Row.FindControl("hfModuleId");
				HiddenField hfParentId = (HiddenField)e.Row.FindControl("hfParentId");
				HiddenField hfModuleType = (HiddenField)e.Row.FindControl("hfModuleType");

				List<SecRoleModuleManager.secRoleModuleGet> data = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(hfModuleId.Value))).ToList();//x.ParentId == Convert.ToInt32(hfParentId.Value) &&
				if (data.Count > 0) {
					gvSubModule.DataSource = data;
					gvSubModule.DataBind();


					CheckBox chkAllAdd = e.Row.FindControl("chkAllAdd") as CheckBox;
					CheckBox chkAllEdit = e.Row.FindControl("chkAllEdit") as CheckBox;
					CheckBox chkAllDel = e.Row.FindControl("chkAllDel") as CheckBox;
					CheckBox chkAllView = e.Row.FindControl("chkAllView") as CheckBox;
					if (chkAllAdd != null)
						chkAllAdd.Attributes.Add("onclick", "javascript:return SelectAllAdd(" + gvSubModule.ClientID.ToString() + ",this," + gvSubModule.Rows.Count + "," + e.Row.RowIndex.ToString() + ")");
					if (chkAllEdit != null)
						chkAllEdit.Attributes.Add("onclick", "javascript:return SelectAllEdit(" + gvSubModule.ClientID.ToString() + ",this," + gvSubModule.Rows.Count + "," + e.Row.RowIndex.ToString() + ")");
					if (chkAllDel != null)
						chkAllDel.Attributes.Add("onclick", "javascript:return SelectAllDelete(" + gvSubModule.ClientID.ToString() + ",this," + gvSubModule.Rows.Count + "," + e.Row.RowIndex.ToString() + ")");
					if (chkAllView != null)
						chkAllView.Attributes.Add("onclick", "javascript:return SelectAllView(" + gvSubModule.ClientID.ToString() + ",this," + gvSubModule.Rows.Count + "," + e.Row.RowIndex.ToString() + ")");
				}
			}
		}

		protected void grdSubModule_RowDataBound(object sender, GridViewRowEventArgs e) {
			if (e.Row.RowType == DataControlRowType.DataRow) {

				GridView gvSubModule1 = (GridView)e.Row.FindControl("gvSubModule1");
				HiddenField hfModuleId = (HiddenField)e.Row.FindControl("hfModuleIdChild");


				List<SecRoleModuleManager.secRoleModuleGet> data = resRoleModule.Where(x => (x.ParentId == Convert.ToInt32(hfModuleId.Value))).ToList();
				if (data.Count > 0) {
					gvSubModule1.DataSource = data;
					gvSubModule1.DataBind();



					CheckBox chkAllAdd = e.Row.FindControl("chkAdd") as CheckBox;
					CheckBox chkAllEdit = e.Row.FindControl("chkEdit") as CheckBox;
					CheckBox chkAllDel = e.Row.FindControl("chkDel") as CheckBox;
					CheckBox chkAllView = e.Row.FindControl("chkView") as CheckBox;
					if (chkAllAdd != null)
						chkAllAdd.Attributes.Add("onclick", "javascript:return SelectAllAdd(" + gvSubModule1.ClientID.ToString() + ",this," + gvSubModule1.Rows.Count + "," + e.Row.RowIndex.ToString() + ")");
					if (chkAllEdit != null)
						chkAllEdit.Attributes.Add("onclick", "javascript:return SelectAllEdit(" + gvSubModule1.ClientID.ToString() + ",this," + gvSubModule1.Rows.Count + "," + e.Row.RowIndex.ToString() + ")");
					if (chkAllDel != null)
						chkAllDel.Attributes.Add("onclick", "javascript:return SelectAllDelete(" + gvSubModule1.ClientID.ToString() + ",this," + gvSubModule1.Rows.Count + "," + e.Row.RowIndex.ToString() + ")");
					if (chkAllView != null)
						chkAllView.Attributes.Add("onclick", "javascript:return SelectAllView(" + gvSubModule1.ClientID.ToString() + ",this," + gvSubModule1.Rows.Count + "," + e.Row.RowIndex.ToString() + ")");
				}
			}
		}

		List<SecRoleModuleManager.secRoleModuleGet> refreshRolePermission = new List<SecRoleModuleManager.secRoleModuleGet>();
		protected void btnSave_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblError.Visible = false;
			try {
				using (TransactionScope scope = new TransactionScope()) {
					SecRoleModuleManager.UpdateSecRoleModuleStatus(Convert.ToInt32(ddlRole.SelectedValue));

					foreach (GridViewRow row in grdModule.Rows) {
						int check = 0;
						int checkParent = 0;
						GridView gv = (GridView)row.FindControl("gvSubModule");
						HiddenField hfRoleModuleId = (HiddenField)row.FindControl("hfRoleModuleId");
						HiddenField hfModuleId = (HiddenField)row.FindControl("hfModuleId");

						if (hfRoleModuleId.Value == string.Empty)
							hfRoleModuleId.Value = "0";

						CheckBox chkAllAdd = (CheckBox)row.FindControl("chkAllAdd");
						CheckBox chkAllEdit = (CheckBox)row.FindControl("chkAllEdit");
						CheckBox chkAllDel = (CheckBox)row.FindControl("chkAllDel");
						CheckBox chkAllView = (CheckBox)row.FindControl("chkAllView");

						SecRoleModule roleModuleMainGrid = SecRoleModuleManager.GetByRoleModuleId(Convert.ToInt32(hfRoleModuleId.Value));
						if (chkAllView.Checked || chkAllAdd.Checked || chkAllEdit.Checked || chkAllEdit.Checked) {
							var secRoleModule = new CRM.Data.Entities.SecRoleModule {
								ModuleID = Convert.ToInt32(hfModuleId.Value),
								RoleID = Convert.ToInt32(ddlRole.SelectedValue),
								AddPermssion = chkAllAdd.Checked == true ? true : false,
								EditPermission = chkAllEdit.Checked == true ? true : false,
								DeletePermission = chkAllDel.Checked == true ? true : false,
								ViewPermission = chkAllView.Checked == true ? true : false,
								Status = 1,
								CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name),
								CreatedOn = DateTime.Now,
								UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name),
								UpdatedOn = DateTime.Now,
								CreatedMachineIP = Request.ServerVariables["remote_addr"].ToString(),
							};
							SecRoleModuleManager.SaveRoleModule(secRoleModule);
							checkParent = 1;
						}


						foreach (GridViewRow subGridRow in gv.Rows) {
							CheckBox chkAdd = (CheckBox)subGridRow.FindControl("chkAdd");
							CheckBox chkEdit = (CheckBox)subGridRow.FindControl("chkEdit");
							CheckBox chkDelete = (CheckBox)subGridRow.FindControl("chkDel");
							CheckBox chkView = (CheckBox)subGridRow.FindControl("chkView");
							HiddenField hfModuleIdChild = (HiddenField)subGridRow.FindControl("hfModuleIdChild");
							SecRoleModule roleModule = SecRoleModuleManager.GetByRoleModuleId(Convert.ToInt32(hfRoleModuleId.Value));
							if (chkAdd.Checked || chkEdit.Checked || chkDelete.Checked || chkView.Checked) {
								var secRoleModuleChild = new CRM.Data.Entities.SecRoleModule {
									ModuleID = Convert.ToInt32(hfModuleIdChild.Value),
									RoleID = Convert.ToInt32(ddlRole.SelectedValue),
									AddPermssion = chkAdd.Checked == true ? true : false,
									EditPermission = chkEdit.Checked == true ? true : false,
									DeletePermission = chkDelete.Checked == true ? true : false,
									ViewPermission = chkView.Checked == true ? true : false,
									Status = 1,
									CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name),
									CreatedOn = DateTime.Now,
									UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name),
									UpdatedOn = DateTime.Now,
									CreatedMachineIP = Request.ServerVariables["remote_addr"].ToString(),

								};
								SecRoleModuleManager.SaveRoleModule(secRoleModuleChild);
								check = 1;
								checkParent = 0;
							}

							GridView gvchild = (GridView)subGridRow.FindControl("gvSubModule1");
							if (gvchild.Rows.Count > 0) {
								foreach (GridViewRow child in gvchild.Rows) {
									CheckBox chkAdd1 = (CheckBox)child.FindControl("chkAdd");
									CheckBox chkEdit1 = (CheckBox)child.FindControl("chkEdit");
									CheckBox chkDelete1 = (CheckBox)child.FindControl("chkDel");
									CheckBox chkView1 = (CheckBox)child.FindControl("chkView");
									HiddenField hfModuleIdChild1 = (HiddenField)child.FindControl("hfModuleIdChild1");
									SecRoleModule roleModule1 = SecRoleModuleManager.GetByRoleModuleId(Convert.ToInt32(hfRoleModuleId.Value));
									if (chkAdd1.Checked || chkEdit1.Checked || chkDelete1.Checked || chkView1.Checked) {
										var secRoleModuleChild = new CRM.Data.Entities.SecRoleModule {
											ModuleID = Convert.ToInt32(hfModuleIdChild1.Value),
											RoleID = Convert.ToInt32(ddlRole.SelectedValue),
											AddPermssion = chkAdd.Checked == true ? true : false,
											EditPermission = chkEdit.Checked == true ? true : false,
											DeletePermission = chkDelete.Checked == true ? true : false,
											ViewPermission = chkView.Checked == true ? true : false,
											Status = 1,
											CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name),
											CreatedOn = DateTime.Now,
											UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name),
											UpdatedOn = DateTime.Now,
											CreatedMachineIP = Request.ServerVariables["remote_addr"].ToString(),

										};
										SecRoleModuleManager.SaveRoleModule(secRoleModuleChild);
										check = 1;
										checkParent = 0;
									}
								}
							}


						}

					}

					CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(Convert.ToInt32(HttpContext.Current.User.Identity.Name));
					if (user.SecRole.RoleId == Convert.ToInt32(ddlRole.SelectedValue)) {
						Session["rolePermission"] = refreshRolePermission = SecRoleModuleManager.getRolePermission(user.SecRole.RoleId).ToList();
					}
					bindGrid(Convert.ToInt32(ddlRole.SelectedValue));
					lblSave.Text = "Record Saved Sucessfully.";
					lblSave.Visible = true;
					scope.Complete();
				}
			}
			catch (Exception ex) {
				lblError.Text = "Record Not Saved Sucessfully.";
				lblError.Visible = true;
			}
		}
	}
}