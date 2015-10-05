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
	public partial class ucOtherSource : System.Web.UI.UserControl {
		List<SecRoleModuleManager.secRoleModuleGet> resRolePermission = new List<SecRoleModuleManager.secRoleModuleGet>();
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {


				string str = HttpContext.Current.Request.Url.AbsolutePath.ToLower();
				string pagename = str.Substring(str.LastIndexOf("/") + 1);
				if (Session["rolePermission"] != null) {
					resRolePermission = (List<SecRoleModuleManager.secRoleModuleGet>)Session["rolePermission"];
					resRolePermission = resRolePermission.Where(x => (x.Url.ToLower() == pagename)).ToList();
					if (resRolePermission.Count > 0) {
						hfDeletePermission.Value = resRolePermission[0].DeletePermission == null ? "0" : resRolePermission[0].DeletePermission == true ? "1" : "0";
						hfNewPermission.Value = resRolePermission[0].AddPermssion == null ? "0" : resRolePermission[0].AddPermssion == true ? "1" : "0";
						hfEditPermission.Value = resRolePermission[0].EditPermission == null ? "0" : resRolePermission[0].EditPermission == true ? "1" : "0";
						hfViewPermission.Value = resRolePermission[0].ViewPermssion == null ? "0" : resRolePermission[0].ViewPermssion == true ? "1" : "0";
					}
				}
				else {
					Response.Redirect("~/Protected/Admin/ServiceNotAvailable.aspx");
				}
				DoBind();
			}
			if (Session["LeadIds"] == null)
				btnReturnToClaim.Visible = false;
		}
		protected void btnReturnToClaim_Click(object sender, EventArgs e) {
			if (Session["LeadIds"] != null) {
				this.Context.Items.Add("selectedleadid", Session["LeadIds"].ToString());
				this.Context.Items.Add("view", "");
				Server.Transfer("~/protected/admin/newlead.aspx");
			}
		}
		private void DoBind() {

			List<OtherSourceMaster> lst = OtherSourceManager.GetAll();
			if (lst.Count > 0) {
				dvData.Visible = true;
				PagerRow.Visible = true;
				lvData.DataSource = lst;
				lvData.DataBind();
			}
			else {
				dvData.Visible = false;
				PagerRow.Visible = false;
			}
		}

		protected void lvData_ItemCommand(object sender, ListViewCommandEventArgs e) {
			lblError.Text = string.Empty;
			lblSave.Text = string.Empty;
			lblMessage.Text = string.Empty;
			lblMessage.Visible = false;
			lblError.Visible = false;
			lblSave.Visible = false;
			if (e.CommandName.Equals("DoEdit")) {
				int Id = Convert.ToInt32(e.CommandArgument);
				hdId.Value = Id.ToString();
				Label lblOtherSource = (Label)e.Item.FindControl("lblOtherSource");
				txtOtherSource.Text = lblOtherSource.Text;

			}
			else if (e.CommandName.Equals("DoDelete")) {

				try {
					var list = CheckPrimaryValue.CheckPrimaryValueExists(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToInt32(e.CommandArgument));
					if (list.ToList().Count > 0) {
						lblMessage.Text = "Other Source is used so you can't delete !!!";
						lblMessage.Visible = true;
						return;
					}
					var dt = OtherSourceManager.GetbyOtherSourceId(Convert.ToInt32(e.CommandArgument));
					dt.Status = false;
					OtherSourceManager.Save(dt);
					DoBind();
					lblSave.Text = "Record Deleted Sucessfully.";
					lblSave.Visible = true;
				}
				catch (Exception ex) {
					lblError.Text = "Record Not Deleted.";
					lblError.Visible = true;
				}
			}
		}


		protected void lvData_PreRender(object sender, EventArgs e) {
			DoBind();

		}
		string saveMsg = string.Empty;
		protected void btnSave_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
			try {

				bool exists = OtherSourceManager.IsExist(txtOtherSource.Text.Trim(), Convert.ToInt32(hdId.Value));
				if (exists) {
					lblMessage.Text = "Other Source allready exists.";
					lblMessage.Visible = true;
					txtOtherSource.Focus();
					return;
				}
				OtherSourceMaster obj = OtherSourceManager.GetbyOtherSourceId(Convert.ToInt32(hdId.Value));
				obj.OtherSource = txtOtherSource.Text;
				obj.Status = true;
				OtherSourceManager.Save(obj);
				saveMsg = hdId.Value == "0" ? "Record Saved Sucessfully." : "Record Updated Sucessfully.";
				btnCancel_Click(null, null);
				lblSave.Text = saveMsg;
				lblSave.Visible = true;

			}
			catch (Exception ex) {
				lblError.Visible = true;
				lblError.Text = "Record Not Saved !!!";
			}
		}

		private void clearControls() {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
			txtOtherSource.Text = string.Empty;
			hdId.Value = "0";
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
			txtOtherSource.Text = string.Empty;
			hdId.Value = "0";
			DoBind();
		}
	}
}