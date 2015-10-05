using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class GenerateCarrierInvoice : System.Web.UI.Page {
		int clientID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			clientID = Core.SessionHelper.getClientId();
		}

		private void bindCarrierInvoiceProfiles() {
			int carrierID = Convert.ToInt32(ddlCarrier.SelectedValue);
			List<CarrierInvoiceProfile> profiles = null;

			if (carrierID > 0) {
				profiles = CarrierInvoiceProfileManager.GetAll(carrierID);

				Core.CollectionManager.FillCollection(ddlInvoiceProfile, "CarrierInvoiceProfileID", "ProfileName", profiles);
			}
		}

		private void bindPolicyForCarrier(int carrierID) {
			// load policy for carrier
			List<CRM.Data.Entities.LeadPolicy> policies = null;
			policies = CarrierManager.GetPoliciesReadyForInvoice(carrierID);

			gvCarrierPolicy.DataSource = policies;
			gvCarrierPolicy.DataBind();

			// show tool bar if policies found
			if (policies.Count > 0)
				pnlToolbar.Visible = true;
		}

		private void bindPoliciesReadyForInvoice() {
			int carrierID = Convert.ToInt32(ddlCarrier.SelectedValue);

			if (carrierID > 0) {
				bindPolicyForCarrier(carrierID);
			}
		}

		// generate invoices
		protected void btnGenerate_Click(object sender, EventArgs e) {
			Carrier carrier = null;
			CarrierInvoiceProfile profile = null;

			Invoice invoice = null;
			int carrierID = Convert.ToInt32(ddlCarrier.SelectedValue);
			decimal invoiceAmount = 0;
			int policyID = 0;

			int profileID = Convert.ToInt32(this.ddlInvoiceProfile.SelectedValue);

			if (carrierID == 0 || profileID == 0)
				return;

			try {
				profile = CarrierInvoiceProfileManager.Get(profileID);

				carrier = CarrierManager.Get(carrierID);
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);

				showErrorMessage();

				return;
			}

			try {
				using (TransactionScope scope = new TransactionScope()) {
					foreach (GridViewRow row in gvCarrierPolicy.Rows) {
						if (row.RowType == DataControlRowType.DataRow) {

							CheckBox cbxSelect = row.FindControl("cbxSelect") as CheckBox;

							if (cbxSelect.Checked) {
								invoice = new Invoice();
								//invoice.ClientID = clientID;
								//invoice.CarrierID = carrierID;
								invoice.InvoiceDate = DateTime.Now;
								
								invoice.InvoiceTypeID = profile.InvoiceType;
								
								invoice.CarrierInvoiceProfileID = profileID;
								
								invoice.IsVoid = false;

								invoice.BillToAddress1 = carrier.AddressLine1;
								invoice.BillToAddress2 = carrier.AddressLine2;
								invoice.BillToAddress3 = string.Empty;
								invoice.BillToName = carrier.CarrierName;

								policyID = (int)gvCarrierPolicy.DataKeys[row.RowIndex].Values[0];

								//invoice.PolicyID = policyID;

								//invoice.LeadId = (int)gvCarrierPolicy.DataKeys[row.RowIndex].Values[1];

								//invoice.PolicyTypeID = (int)gvCarrierPolicy.DataKeys[row.RowIndex].Values[2];

								Label lblInvoiceAmount = row.FindControl("lblInvoiceAmount") as Label;

								decimal.TryParse(lblInvoiceAmount.Text, out invoiceAmount);

								invoice.TotalAmount = invoiceAmount;

								invoice.InvoiceNumber = InvoiceManager.GetNextInvoiceNumber(clientID);

								int invoiceID = InvoiceManager.Save(invoice);

								InvoiceDetail invoiceDetail = new InvoiceDetail();
								invoiceDetail.InvoiceID = invoiceID;
								invoiceDetail.isBillable = true;
								invoiceDetail.Qty = 1;
								invoiceDetail.Rate = invoiceAmount;
								invoiceDetail.LineAmount = invoiceAmount;
								invoiceDetail.LineDate = DateTime.Now;
								invoiceDetail.LineDescription = "As per contract";

								InvoiceDetailManager.Save(invoiceDetail);

								// flag policy as invoiced
                                CRM.Data.Entities.LeadPolicy policy = LeadPolicyManager.Get(policyID);

								if (policy != null) {
									policy.IsInvoiced = true;

									LeadPolicyManager.Save(policy);
								}
							}
						}
					} // foreach

					// complete transaction
					scope.Complete();

					lblMessage.Text = "Invoice(s) have been generated.";
					lblMessage.CssClass = "ok";
				}
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);

				showErrorMessage();
			}
			finally {
				// refresh those policies to be invoiced
				bindPoliciesReadyForInvoice();
			}
		}

		private void clearProfileChoices() {
			ddlInvoiceProfile.Items.Clear();
			ddlInvoiceProfile.SelectedIndex = -1;
		}

		private void clearPolicyChoices() {
			gvCarrierPolicy.DataSource = null;
			gvCarrierPolicy.DataBind();
		}

		protected void ddlInvoiceMode_SelectedIndexChanged(object sender, EventArgs e) {
			if (ddlInvoiceMode.SelectedValue.Equals("2"))
				showManualPanel();
		}

		protected void ddlCarrier_SelectedIndexChanged(object sender, EventArgs e) {
			clearProfileChoices();

			clearPolicyChoices();

			bindCarrierInvoiceProfiles();
		}

		// user chose profile here
		protected void ddlInvoiceProfile_SelectedIndexChanged(object sender, EventArgs e) {
			bindPoliciesReadyForInvoice();
		}

		protected void gvCarrierPolicy_RowDataBound(object sender, GridViewRowEventArgs e) {
            CRM.Data.Entities.LeadPolicy policy = null;
			decimal invoiceAmount = 0;

			int profileID = Convert.ToInt32(ddlInvoiceProfile.SelectedValue);

			if (e.Row.RowType == DataControlRowType.DataRow) {
                policy = e.Row.DataItem as CRM.Data.Entities.LeadPolicy;

				Label lblPolicyHolder = e.Row.FindControl("lblPolicyHolder") as Label;
				lblPolicyHolder.Text = LeadsManager.GetLeadName((int)policy.LeadId);

				invoiceAmount = calculateInvoiceAmount(policy);
				Label lblInvoiceAmount = e.Row.FindControl("lblInvoiceAmount") as Label;

				lblInvoiceAmount.Text = string.Format("{0:N2}", invoiceAmount);
			}
		}

		// user cleared selection
		protected void lbtnClear_Click(object sender, EventArgs e) {
			ddlInvoiceMode.SelectedIndex = -1;

			pnlManualInvoice.Visible = false;

			clearPolicyChoices();

			clearProfileChoices();
		}

		// manual invoice process
		private void showManualPanel() {
			pnlManualInvoice.Visible = true;

			List<Carrier> carriers = CarrierManager.GetCarriers(clientID).ToList();

			CollectionManager.FillCollection(ddlCarrier, "CarrierID", "CarrierName", carriers);
		}

		private void showErrorMessage() {
			lblMessage.Text = "Invoice(s) not generated.";
			lblMessage.CssClass = "error";
		}

        private decimal calculateInvoiceAmount(CRM.Data.Entities.LeadPolicy policy)
        {

			string[] applicableStates = null;
			string claimState = null;
			CRM.Data.Entities.CarrierInvoiceProfile profile = null;
			decimal invoiceAmount = 0;
			decimal netClaimPayable = 0;

			int profileID = Convert.ToInt32(ddlInvoiceProfile.SelectedValue);

			profile = CarrierInvoiceProfileManager.GetProfileForInvoicing(profileID, policy.ClaimDesignationID);

			if (profile != null) {

				// check applicable state
				if (!string.IsNullOrEmpty(profile.CoverageArea)) {
					applicableStates = profile.CoverageArea.Split(new char[] { ',' });

					if (policy.Leads != null && policy.Leads.StateId != null && !applicableStates.Contains("all")) {
						claimState = policy.Leads.StateId.ToString();

						if (!applicableStates.Contains(claimState))
							return 0;
					}

				}

				// determine which amount to use as base to calculate invoice amount
				if (profile.InvoiceType != null && profile.InvoiceType == 1)
					netClaimPayable = (decimal)policy.NetClaimPayable;
				else if (profile.InvoiceType != null && profile.InvoiceType == 2)
					netClaimPayable = (decimal)policy.GrossLossPayable;

				// apply schedule fee
				if (profile != null && profile.CarrierInvoiceProfileFeeSchedule != null && profile.CarrierInvoiceProfileFeeSchedule.Count() > 0) {

					foreach (CarrierInvoiceProfileFeeSchedule schedule in profile.CarrierInvoiceProfileFeeSchedule) {

						if (netClaimPayable >= schedule.RangeAmountFrom && netClaimPayable <= schedule.RangeAmountTo) {
							if (schedule.FlatFee > 0)
								invoiceAmount = schedule.FlatFee;
							else if (schedule.MinimumFee > 0)
								invoiceAmount = schedule.MinimumFee;
							else if (schedule.PercentFee > 0)
								invoiceAmount = netClaimPayable * schedule.PercentFee;
						}
					}
				}

				// apply pricing provisions
				if (profile != null && profile.CarrierInvoiceProfileFeeProvision != null && profile.CarrierInvoiceProfileFeeProvision.Count() > 0) {
					foreach (CarrierInvoiceProfileFeeProvision feeProvision in profile.CarrierInvoiceProfileFeeProvision) {
						if (feeProvision.ProvisionAmount > 0)
							invoiceAmount += feeProvision.ProvisionAmount;
					}
				}
			}

			return invoiceAmount;
		}

		protected void btnRefresh_Click(object sender, EventArgs e) {
			bindPoliciesReadyForInvoice();
		}


	}
}