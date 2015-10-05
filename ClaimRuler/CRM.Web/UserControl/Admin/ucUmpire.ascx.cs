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
	public partial class ucUmpire : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;

		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;
			this.Page.Form.DefaultButton = this.btnSave.UniqueID;

			if (!IsPostBack) {			
				DoBind();
			}
			if (Session["LeadIds"] == null)
				btnReturnToClaim.Visible = false;
		}
		
		protected void btnReturnToClaim_Click(object sender, EventArgs e) {
			if (Session["LeadIds"] != null) {
				//this.Context.Items.Add("selectedleadid", Session["LeadIds"].ToString());
				//this.Context.Items.Add("view", "");
				//Server.Transfer("~/protected/admin/newlead.aspx");
				Response.Redirect("~/protected/newlead.aspx?id=" + Session["LeadIds"].ToString());
			}
		}
		
		protected void btnCancel_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;


			hdId.Value = "0";

			pnlEdit.Visible = false;
			pnlGrid.Visible = true;

			loadUmpires();
		}

		protected void btnNew_Click(object sender, EventArgs e) {
			pnlGrid.Visible = false;

			pnlEdit.Visible = true;

			// clear variables
			txtAddress.Text = "";
			txtAddress2.Text = "";
			txtName.Text = "";
			txtBusinessName.Text = "";
			txtEmail.Text = "";
			txtPhone.Text = "";
			txtFedID.Text = "";

			ddlState.Items.Clear();
			ddlCity.Items.Clear();
			ddlLossZip.Items.Clear();

			List<StateMaster> states = State.GetAll();

			CollectionManager.FillCollection(ddlState, "StateId", "StateName", states);
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;

			try {
				saveUmpire();

				lblSave.Text = "Umpire information saved successfully.";

				lblSave.Visible = true;

				loadUmpires();
			}

			catch (Exception ex) {
				lblError.Visible = true;
				lblError.Text = "Unable to save umpire information!";
			}
		}

		private void DoBind() {

			loadUmpires();

			List<StateMaster> states = State.GetAll();

			CollectionManager.FillCollection(ddlState, "StateId", "StateName", states);
		}

		protected void ddlState_SelectedIndexChanged(object sender, EventArgs e) {
			if (ddlState.SelectedIndex > 0) {
				CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll(Convert.ToInt32(ddlState.SelectedValue)));
			}
			else {
				ddlCity.Items.Clear();
			}
		}

		protected void dllCity_SelectedIndexChanged(object sender, EventArgs e) {
			if (ddlCity.SelectedIndex > 0) {
				CollectionManager.FillCollection(ddlLossZip, "ZipCodeID", "ZipCode", ZipCode.getByCityID(Convert.ToInt32(ddlCity.SelectedValue)));
			}
			else {
				ddlLossZip.Items.Clear();
			}

		}

		protected void gv_onSorting(object sender, GridViewSortEventArgs e) {
			GridView gridView = sender as GridView;

			IQueryable<UmpireMaster> umpires = null;
			int clientID = Core.SessionHelper.getClientId();

			if (clientID > 0)
				umpires = UmpireManager.GetAll(clientID);
			else
				umpires = UmpireManager.GetAll();

			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			gridView.DataSource = umpires.orderByExtension(e.SortExpression, descending);

			gridView.DataBind();

		}

		protected void gvUmpire_RowCommand(object sender, GridViewCommandEventArgs e) {
			int umpireID = 0;

			if (e.CommandName == "DoEdit") {
				hdId.Value = e.CommandArgument.ToString();
				umpireID = Convert.ToInt32(e.CommandArgument.ToString());

				UmpireMaster umpire = UmpireManager.Get(umpireID);

				txtName.Text = umpire.UmpireName;
				txtAddress.Text = umpire.Address1;
				txtAddress2.Text = umpire.Address2;
				txtFedID.Text = umpire.FederalTaxID;

				if (umpire.StateID != null) {
					ddlState.SelectedValue = umpire.StateID.ToString();

					CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll((int)umpire.StateID));
				}

				if (umpire.CityID != null) {
					ddlCity.SelectedValue = umpire.CityID.ToString();

					CollectionManager.FillCollection(ddlLossZip, "ZipCodeID", "ZipCode", ZipCode.getByCityID((int)umpire.CityID));
				}

				if (!string.IsNullOrEmpty(umpire.ZipCode))
					ddlLossZip.SelectedValue = umpire.ZipCode;

				txtEmail.Text = umpire.Email;

				txtPhone.Text = umpire.Phone;

				pnlEdit.Visible = true;

				pnlGrid.Visible = false;


			}
			else if (e.CommandName == "DoDelete") {
				umpireID = Convert.ToInt32(e.CommandArgument.ToString());

				AppraiserMaster appraiser = AppraiserManager.Get(umpireID);

				if (appraiser != null) {
					appraiser.Status = false;

					AppraiserManager.Save(appraiser);
				}
			}

			loadUmpires();
		}

		protected void loadUmpires() {
			IQueryable<UmpireMaster> umpires = null;
			int clientID = Core.SessionHelper.getClientId();

			if (clientID > 0)
				umpires = UmpireManager.GetAll(clientID);
			else
				umpires = UmpireManager.GetAll();

			gvUmpire.DataSource = umpires.ToList();
			gvUmpire.DataBind();

		}

		protected void saveUmpire() {
			// 2013-08-07 tortega
			int clientID = Core.SessionHelper.getClientId();


			using (TransactionScope scope = new TransactionScope()) {

				bool exists = UmpireManager.IsExist(txtName.Text.Trim(), Convert.ToInt32(hdId.Value));
				if (exists) {
					lblMessage.Text = "Umpire name already exists.";
					lblMessage.Visible = true;
					txtName.Focus();
					return;
				}
				UmpireMaster umpire = UmpireManager.Get(Convert.ToInt32(hdId.Value));
				umpire.UmpireName = txtName.Text;
				umpire.BusinessName = txtBusinessName.Text;
				umpire.FederalTaxID = txtFedID.Text.Trim();
				umpire.Address1 = txtAddress.Text.Trim();
				umpire.Address2 = txtAddress2.Text.Trim();
				umpire.StateID = Convert.ToInt32(ddlState.SelectedValue);
				umpire.CityID = Convert.ToInt32(ddlCity.SelectedValue);
				umpire.ZipCode = ddlLossZip.SelectedValue;
				umpire.FederalTaxID = txtFedID.Text.Trim();
				umpire.Email = txtEmail.Text;
				umpire.Phone = txtPhone.Text;

				umpire.Status = true;

				// 2013-08-07 tortega
				if (clientID > 0)
					umpire.ClientId = clientID;

				UmpireManager.Save(umpire);

				scope.Complete();

			}

		}
	}
}