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
	public partial class ucAppraiser : System.Web.UI.UserControl {
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

			loadAppraisers();
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

			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
		}

		protected void btnReturnToClaim_Click(object sender, EventArgs e) {
			if (Session["LeadIds"] != null) {
				Response.Redirect("~/protected/newlead.aspx?q=" + Core.SecurityManager.EncryptQueryString(Session["LeadIds"].ToString()));
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
				saveAppraiser();

				lblSave.Text = "Appraiser information saved sucessfully.";

				lblSave.Visible = true;

				loadAppraisers();
			}

			catch (Exception ex) {
				lblError.Visible = true;
				lblError.Text = "Unable to save appraiser information!";

				Core.EmailHelper.emailError(ex);
			}
		}

		private void DoBind() {

			loadAppraisers();

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

		protected void gvAppraisers_RowCommand(object sender, GridViewCommandEventArgs e) {
			int appraiserID = 0;

			if (e.CommandName == "DoEdit") {
				hdId.Value = e.CommandArgument.ToString();
				appraiserID = Convert.ToInt32(e.CommandArgument.ToString());

				AppraiserMaster contractor = AppraiserManager.Get(appraiserID);

				txtName.Text = contractor.AppraiserName;
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
				appraiserID = Convert.ToInt32(e.CommandArgument.ToString());

				AppraiserMaster appraiser = AppraiserManager.Get(appraiserID);

				if (appraiser != null) {
					appraiser.Status = false;

					AppraiserManager.Save(appraiser);
				}
			}

			loadAppraisers();
		}

		protected void gv_onSorting(object sender, GridViewSortEventArgs e) {
			GridView gridView = sender as GridView;

			IQueryable<AppraiserMaster> appraisers = null;
			int clientID = Core.SessionHelper.getClientId();
			
			appraisers = AppraiserManager.GetAll(clientID);
			
			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			ViewState["lastSorExpression"] = e.SortExpression;

			gridView.DataSource = appraisers.orderByExtension(e.SortExpression, descending);

			gridView.DataBind();

		}

		protected void loadAppraisers() {
			IQueryable<AppraiserMaster> appraisers = null;
			int clientID = Core.SessionHelper.getClientId();


			appraisers = AppraiserManager.GetAll(clientID);



			gvAppraisers.DataSource = appraisers.ToList();
			gvAppraisers.DataBind();

		}

		protected void saveAppraiser() {
			// 2013-08-07 tortega
			int clientID = Core.SessionHelper.getClientId();


			using (TransactionScope scope = new TransactionScope()) {

				bool exists = AppraiserManager.IsExist(txtName.Text.Trim(), Convert.ToInt32(hdId.Value));
				if (exists) {
					lblMessage.Text = "Appraiser name already exists.";
					lblMessage.Visible = true;
					txtName.Focus();
					return;
				}
				AppraiserMaster appraiser = AppraiserManager.Get(Convert.ToInt32(hdId.Value));
				appraiser.AppraiserName = txtName.Text;
				appraiser.BusinessName = txtBusinessName.Text;
				appraiser.FederalTaxID = txtFedID.Text.Trim();
				appraiser.Address1 = txtAddress.Text.Trim();
				appraiser.Address2 = txtAddress2.Text.Trim();
				appraiser.StateID = Convert.ToInt32(ddlState.SelectedValue);
				appraiser.CityID = Convert.ToInt32(ddlCity.SelectedValue);
				appraiser.ZipCode = ddlLossZip.SelectedValue;
				appraiser.FederalTaxID = txtFedID.Text.Trim();
				appraiser.Email = txtEmail.Text;
				appraiser.Phone = txtPhone.Text;

				appraiser.Status = true;

				// 2013-08-07 tortega
				if (clientID > 0)
					appraiser.ClientId = clientID;

				AppraiserManager.Save(appraiser);

				scope.Complete();

			}

		}

		

		protected void gvAppraisers_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			GridView gridView = sender as GridView;

			int clientID = Core.SessionHelper.getClientId();

			IQueryable<AppraiserMaster> appraisers = AppraiserManager.GetAll(clientID);

			bool descending = false;

			string lastSorExpression = ViewState["lastSorExpression"] as string;

			if (lastSorExpression == null) {
				lastSorExpression = "AppraiserName";				
				descending = false;				
			}
			else
				descending = !(bool)ViewState[lastSorExpression];

			ViewState[lastSorExpression] = descending;

			gvAppraisers.PageIndex = e.NewPageIndex;

			gridView.DataSource = appraisers.orderByExtension(lastSorExpression, descending);

			gridView.DataBind();

			
		}
	}
}