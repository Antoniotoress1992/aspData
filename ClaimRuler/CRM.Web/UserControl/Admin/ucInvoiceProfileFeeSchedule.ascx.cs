using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucInvoiceProfileFeeSchedule : System.Web.UI.UserControl {		
		protected void Page_Load(object sender, EventArgs e) {

		}

		public void bindFeeSchedule(int profileID) {
			ViewState["profileID"] = profileID.ToString();

			gvInvoiceFees.DataSource = CarrierInvoiceProfileFeeScheduleManager.GetAll(profileID);

			gvInvoiceFees.DataBind();
		}

		protected void gvInvoiceFees_RowCommand(object sender, GridViewCommandEventArgs e) {
			CarrierInvoiceProfileFeeSchedule fee = null;
			int id = Convert.ToInt32(e.CommandArgument);

			if (e.CommandName == "DoEdit") {
				ViewState["ID"] = e.CommandArgument.ToString();

				fee = CarrierInvoiceProfileFeeScheduleManager.Get(id);

				if (fee != null) {
					try {
						showFeePanel();

						txtAmountFrom.Text = fee.RangeAmountFrom.ToString("n2");
						txtAmountTo.Text = fee.RangeAmountTo.ToString("n2");
						txtFlatFee.Text = fee.FlatFee.ToString("n2");
						if (fee.PercentFee > 0)
							txtPercentFee.Text = (fee.PercentFee * 100).ToString("n2");

						txtMinimumAmount.Text = fee.MinimumFee.ToString("n2");

                        txtFlatCatPercent.Text = (fee.FlatCatPercent * 100).ToString();
                        txtFlatCatFee.Text = fee.FlatCatFee.ToString();
					}
					catch (Exception ex) {
						Core.EmailHelper.emailError(ex);
					}
				}

			}
			else if (e.CommandName == "DoDelete") {
				CarrierInvoiceProfileFeeScheduleManager.Delete(id);

				int profileID = Convert.ToInt32(ViewState["profileID"]);

				bindFeeSchedule(profileID);
			}
		}

		protected void blnkNewInvoiceFee_Click(object sender, EventArgs e) {
			ViewState["ID"] = "0";

			showFeePanel();

			clearFeeFields();
		}

		private void showFeeScheduleGrid() {
			pnlInvoiceFee.Visible = false;
			gvInvoiceFees.Visible = true;
		}

		private void clearFeeFields() {
			txtAmountFrom.Text = string.Empty;
			txtAmountTo.Text = string.Empty;
			txtFlatFee.Text = string.Empty;
			txtMinimumAmount.Text = string.Empty;
			txtPercentFee.Text = string.Empty;
            txtFlatCatPercent.Text = string.Empty;
            txtFlatCatFee.Text = string.Empty;
		}

		private void showFeePanel() {
			pnlInvoiceFee.Visible = true;
			gvInvoiceFees.Visible = false;
		}

		
		protected void btnSave_Click(object sender, EventArgs e) {
			CarrierInvoiceProfileFeeSchedule fee = null;

			int profileID = Convert.ToInt32(ViewState["profileID"]);

			// record id
			int id = Convert.ToInt32(ViewState["ID"]);

			if (id == 0) {
				fee = new CarrierInvoiceProfileFeeSchedule();

				fee.CarrierInvoiceProfileID = profileID;
			}
			else
				fee = CarrierInvoiceProfileFeeScheduleManager.Get(id);

			if (fee != null) {
				try {
					fee.FlatFee = Convert.ToDecimal(txtFlatFee.Value == null ? "0" : txtFlatFee.Value);

					fee.MinimumFee = Convert.ToDecimal(txtMinimumAmount.Value == null ? "0" : txtMinimumAmount.Value);

					fee.PercentFee = Convert.ToDecimal(txtPercentFee.Value == null ? "0" : txtPercentFee.Value);

					fee.RangeAmountFrom = Convert.ToDecimal(txtAmountFrom.Value == null ? "0" : txtAmountFrom.Value);

					fee.RangeAmountTo = Convert.ToDecimal(txtAmountTo.Value == null ? "0" : txtAmountTo.Value);

                    fee.FlatCatPercent = Convert.ToDecimal(txtFlatCatPercent.Value == null ? "0" : txtFlatCatPercent.Value);

                    fee.FlatCatFee = Convert.ToDecimal(txtFlatCatFee.Value == null ? "0" : txtFlatCatFee.Value);

					CarrierInvoiceProfileFeeScheduleManager.Save(fee);

					showFeeScheduleGrid();

					bindFeeSchedule(profileID);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			int carrierID = Convert.ToInt32(ViewState["profileID"]);

			showFeeScheduleGrid();

			bindFeeSchedule(carrierID);
		}
	}
}