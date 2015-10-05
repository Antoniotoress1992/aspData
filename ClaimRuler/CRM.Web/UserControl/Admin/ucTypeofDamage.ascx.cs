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
	public partial class ucTypeofDamage : System.Web.UI.UserControl {
			int clientID = 0;
			protected Protected.ClaimRuler masterPage = null;

		protected void Page_Load(object sender, EventArgs e) {
			clientID = Core.SessionHelper.getClientId();
			
			masterPage = this.Page.Master as Protected.ClaimRuler;

			if (!IsPostBack) {


			
				DoBind();
			}
		}
		private void DoBind() {

			List<TypeOfDamageMaster> lst = TypeofDamageManager.GetAll(clientID);
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
				Label lblTypeOfDamage = (Label)e.Item.FindControl("lblTypeOfDamage");
				txtTypeofDamage.Text = lblTypeOfDamage.Text;

				Label lblSort = (Label)e.Item.FindControl("lblSort");
				txtSort.Text = lblSort.Text;

			}
			else if (e.CommandName.Equals("DoDelete")) {
				// In Case of delete 
				try {
					//Label lblTypeOfDamage = (Label)e.Item.FindControl("lblTypeOfDamage");
					//var list = CheckPrimaryValue.checkTypeofDamage(lblTypeOfDamage.Text);
					//if (list.ToList().Count > 0) {
					//	lblMessage.Text = "Type of Damage is used so you can't delete !!!";
					//	lblMessage.Visible = true;
					//	return;
					//}

					var dt = TypeofDamageManager.GetbyTypeOfDamageId(Convert.ToInt32(e.CommandArgument));
					
					dt.Status = false;
					
					TypeofDamageManager.Save(dt);
					DoBind();

					lblSave.Text = "Record Deleted Successfully.";
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

			Page.Validate("TypeofDamage");
			if (!Page.IsValid)
				return;

			try {

				bool exists = TypeofDamageManager.IsExist(txtTypeofDamage.Text.Trim(), Convert.ToInt32(hdId.Value));
				if (exists) {
					lblMessage.Text = "Type Of Damage already exists.";
					lblMessage.Visible = true;
					txtTypeofDamage.Focus();
					return;
				}
				TypeOfDamageMaster obj = TypeofDamageManager.GetbyTypeOfDamageId(Convert.ToInt32(hdId.Value));
				obj.TypeOfDamage = txtTypeofDamage.Text;
				
				if (!string.IsNullOrEmpty(txtSort.Text))
					obj.Sort = (byte)Convert.ToInt32(txtSort.Text);

				obj.Status = true;

				if (clientID > 0)
					obj.ClientId = clientID;

				TypeofDamageManager.Save(obj);
				saveMsg = hdId.Value == "0" ? "Record Saved Successfully." : "Record Updated Successfully.";
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
			txtTypeofDamage.Text = string.Empty;
			hdId.Value = "0";
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
			txtTypeofDamage.Text = string.Empty;
			hdId.Value = "0";
			DoBind();
		}

		protected void cbxHidden_CheckedChanged(object sender, EventArgs e) {
			ListViewItem lvItem = ((CheckBox)sender).NamingContainer as ListViewItem;
			int idx = lvItem.DataItemIndex;
			int TypeofDamageId = (int)lvData.DataKeys[idx].Value;

			TypeOfDamageMaster damageType = TypeofDamageManager.GetbyTypeOfDamageId(TypeofDamageId);

			if (damageType != null) {
				damageType.IsHidden = ((CheckBox)sender).Checked;

				TypeofDamageManager.Save(damageType);
				DoBind();
			}
		}
	}
}