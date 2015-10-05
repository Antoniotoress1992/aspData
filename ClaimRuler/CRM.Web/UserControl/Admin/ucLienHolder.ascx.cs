using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;

using CRM.Data;
using CRM.Core;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucLienHolder : System.Web.UI.UserControl {
		int clientID = 0;
		protected Protected.ClaimRuler masterPage = null;

		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;

			if (!IsPostBack) {
			
				DoBind();
			}

		}

		protected void DoBind() {
			List<StateMaster> states = State.GetAll();
			List<CountryMaster> contries = CountryMasterManager.GetAll();

			CollectionManager.FillCollection(ddlState, "StateId", "StateName", states);

			CollectionManager.FillCollection(ddlCountry, "CountryID", "CountryName", contries);

			bindLienholders();
		}

		protected void bindLienholders() {
			clientID = Core.SessionHelper.getClientId();

			List<Mortgagee> mortgagees = MortgageeManager.GetAll(clientID).ToList();

			gvMortgagee.DataSource = mortgagees;
			gvMortgagee.DataBind();
		}

		protected void btnNew_Click(object sender, EventArgs e) {
			hf_id.Value = "0";

			pnlEdit.Visible = true;
			pnlList.Visible = false;

			clearFields();

		}

		protected void clearFields() {
			txtName.Text = string.Empty;
			txtAddress.Text = string.Empty;
			txtAddress.Text = string.Empty;
			txtAddress2.Text = string.Empty;
			txtPhone.Text = string.Empty;
			txtFax.Text = string.Empty;
			txtEmail.Text = string.Empty;
			txtContactName.Text = string.Empty;
			
			ddlCity.SelectedIndex = -1;
			ddlLossZip.SelectedIndex = -1;
			ddlState.SelectedIndex = -1;


		}

		protected void gv_RowCommand(object sender, GridViewCommandEventArgs e) {
			Mortgagee mortgagee = null;

			if (e.CommandName == "DoEdit") {
				hf_id.Value = e.CommandArgument.ToString();

				mortgagee = MortgageeManager.Get(Convert.ToInt32(e.CommandArgument));

				if (mortgagee != null) {
					pnlEdit.Visible = true;
					pnlList.Visible = false;

					hf_id.Value = mortgagee.MortgageeID.ToString();

					CollectionManager.FillCollection(ddlState, "StateId", "StateName", State.GetAll());
					CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll(mortgagee.StateID ?? 0));
					CollectionManager.FillCollection(ddlLossZip, "ZipCodeID", "ZipCode", ZipCode.getByCityID(mortgagee.CityID ?? 0));

					txtName.Text = mortgagee.MortageeName;
					txtAddress.Text = mortgagee.AddressLine1;
					txtAddress2.Text = mortgagee.AddressLine2;
					txtFax.Text = mortgagee.Fax;
					txtPhone.Text = mortgagee.Phone;

					txtEmail.Text = mortgagee.Email;
					txtContactName.Text = mortgagee.ContactName;


					if (mortgagee.StateID != null)
						ddlState.SelectedValue = mortgagee.StateID.ToString();

					if (mortgagee.CityID != null)
						ddlCity.SelectedValue = mortgagee.CityID.ToString();

					if (mortgagee.ZipCodeID != null)
						ddlLossZip.SelectedValue = mortgagee.ZipCodeID.ToString();

					this.ddlCountry.SelectedValue = (mortgagee.CountryID ?? 0).ToString();
				}
			}

			if (e.CommandName == "DoDelete") {
				mortgagee = MortgageeManager.Get(Convert.ToInt32(e.CommandArgument));
				if (mortgagee != null) {
					mortgagee.Status = false;

					MortgageeManager.Save(mortgagee);

					bindLienholders();
				}
			}

		}

		protected void gv_onSorting(object sender, GridViewSortEventArgs e) {

			IQueryable<Mortgagee> clients = MortgageeManager.GetAll(clientID);
			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			gvMortgagee.DataSource = clients.orderByExtension(e.SortExpression, descending);

			gvMortgagee.DataBind();

		}

		protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e) {
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

		protected void btnSave_Click(object sender, EventArgs e) {					
			lblMessage.Text = string.Empty;

			try {
				if (saveLienholder()) {

					lblMessage.Text = "Lienholder information saved successfully.";

					lblMessage.CssClass = "ok";

					bindLienholders();
				}
			}
			catch (Exception ex) {
				lblMessage.CssClass = "error";
				lblMessage.Text = "Unable to save Lienholder information!";

				Core.EmailHelper.emailError(ex);
			}
		}

		protected void btnClose_Click(object sender, EventArgs e) {
			lblMessage.CssClass = string.Empty;
			lblMessage.Text = string.Empty;


			this.hf_id.Value = "0";

			pnlEdit.Visible = false;
			pnlList.Visible = true;

			bindLienholders();
		}

		protected bool saveLienholder() {
			int id = Convert.ToInt32(hf_id.Value);
			clientID = Core.SessionHelper.getClientId();

		
			Mortgagee mortgagee = null;

			// 2013-08-07 tortega		
			using (TransactionScope scope = new TransactionScope()) {

				

				if (id > 0) {
					mortgagee = MortgageeManager.Get(id);
				}
				else {
					bool exists = MortgageeManager.IsExist(txtName.Text.Trim(), clientID);
					if (exists) {
						lblMessage.Text = "Mortgagee name already exists.";
						lblMessage.CssClass = "error";
						txtName.Focus();
						return false;
					}

					mortgagee = new Mortgagee();
					mortgagee.Status = true;
				}

				mortgagee.ClientID = clientID;
				mortgagee.MortageeName = txtName.Text.Trim();
				mortgagee.AddressLine1 = txtAddress.Text.Trim();
				mortgagee.AddressLine2 = txtAddress2.Text.Trim();

				mortgagee.CityID = Convert.ToInt32(ddlCity.SelectedValue);
				mortgagee.StateID = Convert.ToInt32(this.ddlState.SelectedValue);
				mortgagee.ZipCodeID = Convert.ToInt32(this.ddlLossZip.SelectedValue);

				mortgagee.Phone = txtPhone.Text.Trim();
				mortgagee.Fax = txtFax.Text.Trim();
				mortgagee.Email = txtEmail.Text;

				mortgagee.ContactName = txtContactName.Text;

				if (ddlCountry.SelectedIndex > 0)
					mortgagee.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);

				MortgageeManager.Save(mortgagee);

				scope.Complete();

			}
			return true;
		}
	}
}