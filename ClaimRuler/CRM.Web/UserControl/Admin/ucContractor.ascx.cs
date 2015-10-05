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
	public partial class ucContractor : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;

		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;

			this.Page.Form.DefaultButton = btnSave.UniqueID;

			if (!IsPostBack) {			
				DoBind();
			}
			if (Session["LeadIds"] == null)

				btnReturnToClaim.Visible = false;
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

			loadContractors();
		}

		protected void btnNew_Click(object sender, EventArgs e) {
			pnlGrid.Visible = false;

			pnlEdit.Visible = true;

			// clear variables
			txtAddress.Text = "";
			txtAddress2.Text = "";
			txtContractor.Text = "";
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

		protected void btnReturnToClaim_Click(object sender, EventArgs e) {
			if (Session["LeadIds"] != null) {
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

			try {
				saveContractor();

				lblSave.Text = "Contractor information saved sucessfully.";

				lblSave.Visible = true;

				loadContractors();
			}

			catch (Exception ex) {
				lblError.Visible = true;
				lblError.Text = "Unable to save contractor information!";
			}
		}

		private void DoBind() {

			loadContractors();

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


		protected void gvContractor_RowCommand(object sender, GridViewCommandEventArgs e) {
			int contractorID = 0;

			if (e.CommandName == "DoEdit") {
				hdId.Value = e.CommandArgument.ToString();
				contractorID = Convert.ToInt32(e.CommandArgument.ToString());

				ContractorMaster contractor = ContractorManager.Get(contractorID);

				txtContractor.Text = contractor.ContractorName;
				txtAddress.Text = contractor.Address1;
				txtAddress2.Text = contractor.Address2;
				txtFedID.Text = contractor.FederalTaxID;

				if (contractor.StateID != null) {
					ddlState.SelectedValue = contractor.StateID.ToString();

					CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll((int)contractor.StateID));
				}

				if (contractor.CityID != null) {
					ddlCity.SelectedValue = contractor.CityID.ToString();

					CollectionManager.FillCollection(ddlLossZip, "ZipCodeID", "ZipCode", ZipCode.getByCityID((int)contractor.CityID));
				}

				if (!string.IsNullOrEmpty(contractor.ZipCode))
					ddlLossZip.SelectedValue = contractor.ZipCode;

				txtEmail.Text = contractor.Email;

				txtPhone.Text = contractor.Phone;

				pnlEdit.Visible = true;

				pnlGrid.Visible = false;


			}
			else if (e.CommandName == "DoDelete") {
				contractorID = Convert.ToInt32(e.CommandArgument.ToString());

				ContractorMaster contractor = ContractorManager.Get(contractorID);

				if (contractor != null) {
					contractor.Status = false;

					ContractorManager.Save(contractor);
				}
			}

			loadContractors();
		}
		
		protected void gv_onSorting(object sender, GridViewSortEventArgs e) {
			GridView gridView = sender as GridView;

			IQueryable<ContractorMaster> contractors = null;
			int clientID = Core.SessionHelper.getClientId();

		
			contractors = ContractorManager.GetAll(clientID);
			
			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			gridView.DataSource = contractors.orderByExtension(e.SortExpression, descending);

			gridView.DataBind();

		}

		protected void loadContractors() {
			IQueryable<ContractorMaster> contractors = null;
			int clientID = Core.SessionHelper.getClientId();

			contractors = ContractorManager.GetAll(clientID);			

			gvContractor.DataSource = contractors.ToList();
			gvContractor.DataBind();
		}

		protected void saveContractor() {
			// 2013-08-07 tortega
			int clientID = Core.SessionHelper.getClientId();


			using (TransactionScope scope = new TransactionScope()) {

				bool exists = ContractorManager.IsExist(txtContractor.Text.Trim(), Convert.ToInt32(hdId.Value));
				if (exists) {
					lblMessage.Text = "Contractor name already exists.";
					lblMessage.Visible = true;
					txtContractor.Focus();
					return;
				}
				ContractorMaster contractor = ContractorManager.Get(Convert.ToInt32(hdId.Value));
				contractor.ContractorName = txtContractor.Text;
				contractor.BusinessName = txtBusinessName.Text;
				contractor.FederalTaxID = txtFedID.Text.Trim();
				contractor.Address1 = txtAddress.Text.Trim();
				contractor.Address2 = txtAddress2.Text.Trim();
				contractor.StateID = Convert.ToInt32(ddlState.SelectedValue);
				contractor.CityID = Convert.ToInt32(ddlCity.SelectedValue);
				contractor.ZipCode = ddlLossZip.SelectedValue;
				contractor.FederalTaxID = txtFedID.Text.Trim();
				contractor.Email = txtEmail.Text;
				contractor.Phone = txtPhone.Text;

				contractor.Status = true;

				// 2013-08-07 tortega
				if (clientID > 0)
					contractor.ClientId = clientID;

				ContractorManager.Save(contractor);

				scope.Complete();

			}

		}

		protected void gvContractor_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			GridView gridView = sender as GridView;

			int clientID = Core.SessionHelper.getClientId();

			IQueryable<ContractorMaster> contractors = null;
			
			contractors = ContractorManager.GetAll(clientID);

			bool descending = false;

			string lastSorExpression = ViewState["lastSorExpression"] as string;

			if (lastSorExpression == null) {
				lastSorExpression = "ContractorName";
				descending = false;
			}
			else
				descending = !(bool)ViewState[lastSorExpression];

			ViewState[lastSorExpression] = descending;

			gridView.PageIndex = e.NewPageIndex;

			gridView.DataSource = contractors.orderByExtension(lastSorExpression, descending);

			gridView.DataBind();
		}


	}
}