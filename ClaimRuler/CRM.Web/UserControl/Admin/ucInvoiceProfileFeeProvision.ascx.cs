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
	public partial class ucInvoiceProfileFeeProvision : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {

		}

		public void bindProvisions(int profileID) {
			ViewState["profileID"] = profileID.ToString();

			gvFeeProvisions.DataSource = CarrierInvoiceProfileFeeProvisionManager.GetAll(profileID);

			gvFeeProvisions.DataBind();
		}
		protected void lbtnNewProvision_Click(object sender, EventArgs e) {
			ViewState["ID"] = "0";

			showProvisionPanel();

			clearProvisionFields();			
		}

		protected void gvFeeProvisions_RowCommand(object sender, GridViewCommandEventArgs e) {
			int profileID = Convert.ToInt32(ViewState["profileID"]);

			int id = Convert.ToInt32(e.CommandArgument);
			
			CarrierInvoiceProfileFeeProvision provision = null;

			if (e.CommandName == "DoEdit") {
				provision = CarrierInvoiceProfileFeeProvisionManager.Get(id);

				if (provision != null) {
					ViewState["ID"] = id.ToString();

					txtProvisionAmount.Value = provision.ProvisionAmount;

					txtProvisionName.Text = provision.ProvisionText;

					showProvisionPanel();
				}
			}
			else if (e.CommandName == "DoDelete") {
				CarrierInvoiceProfileFeeProvisionManager.Delete(id);

				bindProvisions(profileID);
			}
		}


		protected void btnProvisionSave_Click(object sender, EventArgs e) {
			int id = Convert.ToInt32(ViewState["ID"]);
			
			int profileID = Convert.ToInt32(ViewState["profileID"]);

			CarrierInvoiceProfileFeeProvision provision = null;

			lblProvisionMessage.Text = string.Empty;

			if (id == 0) {
				// new 
				provision = new CarrierInvoiceProfileFeeProvision();

				provision.CarrierInvoiceProfileID = profileID;
			}
			else {
				provision = CarrierInvoiceProfileFeeProvisionManager.Get(id);
			}

			if (provision != null) {				
				provision.ProvisionAmount = Convert.ToDecimal(txtProvisionAmount.Value == null ? "0" : txtProvisionAmount.Value);

				provision.ProvisionText = txtProvisionName.Text;

				try {
					CarrierInvoiceProfileFeeProvisionManager.Save(provision);

					showProvisionsGrid();

					bindProvisions(profileID);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);

					lblProvisionMessage.Text = "Unable to save provision data.";

					lblProvisionMessage.Visible = true;
				}
			}
		}

		protected void btnProvisionCancel_Click(object sender, EventArgs e) {
			int profileID = Convert.ToInt32(ViewState["profileID"]);

			showProvisionsGrid();

			bindProvisions(profileID);
		}

		private void clearProvisionFields() {
			txtProvisionName.Text = string.Empty;
			txtProvisionAmount.Text = string.Empty;
			lblProvisionMessage.Text = string.Empty;
		}

		private void showProvisionPanel() {
			pnlFeeProvision.Visible = true;
			gvFeeProvisions.Visible = false;

			lbtnNewProvision.Visible = false;
		}

		private void showProvisionsGrid() {
			pnlFeeProvision.Visible = false;
			
			gvFeeProvisions.Visible = true;

			lbtnNewProvision.Visible = true;
		}
	}
}