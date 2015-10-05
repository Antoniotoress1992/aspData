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
	public partial class ucSecondaryProducer : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;
		protected void Page_Load(object sender, EventArgs e) {
			this.Page.Form.DefaultButton = btnSave.UniqueID;
			
			masterPage = this.Page.Master as Protected.ClaimRuler;

			if (!IsPostBack) {
				
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
			int clientID = 0;
			List<SecondaryProducerMaster> lst = null;
			int roleID = Core.SessionHelper.getUserRoleId();

			if (roleID == (int)UserRole.Client || roleID == (int)UserRole.SiteAdministrator) {
				clientID = Core.SessionHelper.getClientId();

				lst = SecondaryProducerManager.GetAll(clientID);
			}
			else
				lst = SecondaryProducerManager.GetAll();

			if (lst.Count > 0) {
				lvData.DataSource = lst;
				lvData.DataBind();
				dvData.Visible = true;
				PagerRow.Visible = true;
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
				int id = Convert.ToInt32(e.CommandArgument);
				hdId.Value = id.ToString();
				Label lblName = (Label)e.Item.FindControl("lblName");
				txtName.Text = lblName.Text;

			}
			else if (e.CommandName.Equals("DoDelete")) {
				// In Case of delete 
				try {
					var list = CheckPrimaryValue.CheckPrimaryValueExists(0, 0, 0, 0, Convert.ToInt32(e.CommandArgument), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
					if (list.ToList().Count > 0) {
						lblMessage.Text = "Secondary Producer is used so you can't delete !!!";
						lblMessage.Visible = true;
						return;
					}
					var secondaryProducer = SecondaryProducerManager.GetSecondaryProducerId(Convert.ToInt32(e.CommandArgument));
					secondaryProducer.Status = false;
					SecondaryProducerManager.Save(secondaryProducer);
					btnCancel_Click(null, null);
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
			Page.Validate("producer");
			if (!Page.IsValid)
				return;

			int clientID = Core.SessionHelper.getClientId();

			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
			try {
				using (TransactionScope scope = new TransactionScope()) {

					bool exists = SecondaryProducerManager.IsExist(txtName.Text.Trim(), Convert.ToInt32(hdId.Value));
					if (exists) {
						lblMessage.Text = "Secondary Producer Name already exists.";
						lblMessage.Visible = true;
						txtName.Focus();
						return;
					}
					SecondaryProducerMaster producer = SecondaryProducerManager.GetSecondaryProducerId(Convert.ToInt32(hdId.Value));
					producer.SecondaryProduceName = txtName.Text;
					producer.Status = true;

					// tortega 2013-08-09
					if (clientID > 0)
						producer.ClientID = clientID;

					SecondaryProducerManager.Save(producer);
					saveMsg = hdId.Value == "0" ? "Record Saved Sucessfully." : "Record Updated Sucessfully.";
					btnCancel_Click(null, null);
					lblSave.Text = saveMsg;
					lblSave.Visible = true;
					scope.Complete();
				}
			}
			catch (Exception ex) {
				lblError.Visible = true;
				lblError.Text = "Record Not Saved !!!";
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
			txtName.Text = string.Empty;
			hdId.Value = "0";

			DoBind();
		}
	}
}