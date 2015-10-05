using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data.Account;
using CRM.Data;
using CRM.Repository;

using LinqKit;
using CRM.Core;
using System.Globalization;
using System.Transactions;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucServiceList : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;
		string[] unitDescriptions = { "percentage", "sales tax" };

		protected void Page_Load(object sender, EventArgs e) {

		}

		public void bindData() {
			int clientID = Core.SessionHelper.getClientId();

			loadServices();
		}

		protected void Page_Init(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			clearFields();

			pnlEdit.Visible = false;
			pnlGrid.Visible = true;

			loadServices();
		}

		protected void btnNew_Click(object sender, EventArgs e) {
			pnlGrid.Visible = false;

			pnlEdit.Visible = true;

			// clear variables
			clearFields();

		}

		protected void btnSave_Click(object sender, EventArgs e) {

			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;

			int clientID = Core.SessionHelper.getClientId();
			int id = 0;
			int unitID = 0;

			InvoiceServiceType service = null;

			Page.Validate("service");
			if (!Page.IsValid)
				return;

			try {
				int.TryParse(hfId.Value, out id);

				using (InvoiceServiceTypeManager repository = new InvoiceServiceTypeManager()) {
					if (id == 0) {
						service = new InvoiceServiceType();
						service.ClientID = clientID;
						service.isActive = true;
						service.IsTaxable = false;
					}
					else {
						service = repository.Get(id);
					}

					service.MinimumFee = txtMinimumFee.Value == null ? 0 : Convert.ToDecimal(txtMinimumFee.Value);
					service.IsTaxable = cbxIsTaxable.Checked;
					service.DefaultQty = txtDefaultQty.Value == null ? 0 : Convert.ToDecimal(txtDefaultQty.Value);

					service.ServiceRate = txtRate.Value == null ? 0 : Convert.ToDecimal(txtRate.Value);
					service.ServicePercentage = txtPercentage.Value == null ? 0 : Convert.ToDecimal(txtPercentage.Value);
					service.ServiceDescription = txtServiceDescription.Text.Trim();

					service.EarningCode = txtEarningCode.Text.Trim();

					//int.TryParse(ddlUnit.SelectedValue, out unitID);

					//if (ddlUnit.SelectedIndex > 0)
					//	service.UnitID = unitID;
					//else
					//	service.UnitID = null;


					repository.Save(service);
				}



				lblMessage.Text = "Service type saved successfully.";
				lblMessage.CssClass = "ok";

				pnlEdit.Visible = false;
				pnlGrid.Visible = true;

				loadServices();
			}

			catch (Exception ex) {
				lblMessage.Text = "Unable to save service type information!";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}
		}

		//private void bindEarningCodes() {
		//	int clientID = Core.SessionHelper.getClientId();

		//	List<EarningCode> earningCodes = null;

		//	using (EarningCodeManager repository = new EarningCodeManager()) {
		//		earningCodes = repository.GetAll(clientID);
		//	}

		//	CollectionManager.FillCollection(ddlEarningCode, "EarningCodeID", "Code", earningCodes);
		//}
		private void clearFields() {

			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;


			hfId.Value = "0";

			txtRate.Text = "";

			txtServiceDescription.Text = "";
			txtEarningCode.Text = "";
			//ddlUnit.SelectedIndex = -1;


			txtDefaultQty.Value = 0;
			txtMinimumFee.Value = 0;
		}

		//private void EnableUnitTextBox(string unitType) {
		//	if (unitDescriptions.Contains(unitType.ToLower())) {
		//		lblRatePrompt.Text = "Percentage (%)";
		//	}
		//	else {
		//		lblRatePrompt.Text = "Rate ($)";
		//	}

		//}

		//protected void ddlUnit_SelectedIndexChanged(object sender, EventArgs e) {
		//	EnableUnitTextBox(ddlUnit.SelectedItem.Text);			
		//}



		protected void gvServices_RowCommand(object sender, GridViewCommandEventArgs e) {
			int id = 0;
			InvoiceServiceType service = null;

			clearFields();

			if (e.CommandName == "DoEdit") {
				hfId.Value = e.CommandArgument.ToString();
				id = Convert.ToInt32(e.CommandArgument.ToString());

				lblMessage.Text = string.Empty;

				using (InvoiceServiceTypeManager repository = new InvoiceServiceTypeManager()) {
					service = repository.Get(id);


					if (service != null) {
						pnlEdit.Visible = true;

						pnlGrid.Visible = false;

						//bindEarningCodes();

						txtServiceDescription.Text = service.ServiceDescription;

						txtRate.Value = service.ServiceRate ?? 0;
						txtPercentage.Value = service.ServicePercentage ?? 0;
						txtDefaultQty.Value = service.DefaultQty ?? 0;
						txtMinimumFee.Value = service.MinimumFee ?? 0;
						txtEarningCode.Text = service.EarningCode;

						//if (service.UnitID != null) {
						//	ddlUnit.SelectedValue = service.UnitID.ToString();

						//	EnableUnitTextBox(service.InvoiceServiceUnit.UnitDescription);
						//}



						lblMessage.Text = "";
					}
				}

			}
			else if (e.CommandName == "DoDelete") {
				id = Convert.ToInt32(e.CommandArgument.ToString());

				using (InvoiceServiceTypeManager repository = new InvoiceServiceTypeManager()) {
					service = repository.Get(id);

					if (service != null) {
						service.isActive = false;

						repository.Save(service);
					}
				}
			}

			loadServices();
		}

		protected void gvServices_RowDataBound(object sender, GridViewRowEventArgs e) {

			if (e.Row.RowType == DataControlRowType.DataRow) {

				InvoiceServiceType service = e.Row.DataItem as InvoiceServiceType;

				Label lblServiceRate = e.Row.FindControl("lblServiceRate") as Label;

				//if (lblServiceRate != null && service != null && service.InvoiceServiceUnit != null) {

				//	if (unitDescriptions.Contains(service.InvoiceServiceUnit.UnitDescription.ToLower()))
				//		lblServiceRate.Text = string.Format("{0:N2} %", service.ServiceRate);
				//	else
				//		lblServiceRate.Text = string.Format("$ {0:N2}", service.ServiceRate);						
				//}
			}
		}

		protected void gv_onSorting(object sender, GridViewSortEventArgs e) {
			GridView gridView = sender as GridView;
			bool descending = false;

			IQueryable<InvoiceServiceType> services = null;
			int clientID = Core.SessionHelper.getClientId();

			using (InvoiceServiceTypeManager repository = new InvoiceServiceTypeManager()) {
				services = repository.GetAll(clientID);

				gridView.DataSource = services.orderByExtension(e.SortExpression, descending);

				gridView.DataBind();
			}

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;



		}

		protected void loadServices() {
			List<InvoiceServiceType> services = null;

			int clientID = Core.SessionHelper.getClientId();

			using (InvoiceServiceTypeManager repository = new InvoiceServiceTypeManager()) {
				services = repository.GetAll(clientID).ToList();
			}

			gvServices.DataSource = services;
			gvServices.DataBind();

		}

	}
}