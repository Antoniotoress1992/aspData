using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Transactions;

using Infragistics.Web.UI.EditorControls;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;
using CRM.RuleEngine;

using Microsoft.Reporting.WebForms;
using CRM.Data.Entities;
using System.Text;
using CRM.Core;

namespace CRM.Web.Protected {
	public partial class LeadInvoice : System.Web.UI.Page {
		string[] unitDescriptions = { "percentage", "sales tax" };
		List<InvoiceView> invoiceReport = null;

		const int SALES_TAX_ID = 5;

		protected void Page_Load(object sender, EventArgs e) {
			// access master page from nested master page
			Protected.ClaimRuler masterPage = Master.Master as Protected.ClaimRuler;

			// check user permission
			masterPage.checkPermission();


			int invoiceID = 0;

			if (!Page.IsPostBack) {
				bindData();
			}

			if (int.TryParse(ViewState["InvoiceID"].ToString(), out invoiceID) && invoiceID > 0) {
				showToolbarButtons();
			}
		}

		protected void btnPrint_Click(object sender, EventArgs e) {
			int invoiceID = 0;
			int.TryParse(ViewState["InvoiceID"].ToString(), out invoiceID);

			string encryptedID = Core.SecurityManager.EncryptQueryString(ViewState["InvoiceID"].ToString());

			if (invoiceID > 0) {
				string js = string.Format("window.open('../Content/PrintInvoice.aspx?q={0}', 'invoice', 'status=0, toolbar=0, width=800, height=800');", encryptedID);

				ScriptManager.RegisterStartupScript(Page, typeof(Page), "Print_Invoice", js, true);
			}
		}

		protected void btnInvoiceLedger_Click(object sender, EventArgs e) {
			Response.Redirect("~/Protected/LeadInvoiceLedger.aspx");
		}

		protected void bindInvoiceDetails(int invoiceID) {
			List<InvoiceDetail> invoiceDetailLines = null;
			decimal? totalAmount = 0;

			invoiceDetailLines = InvoiceDetailManager.GetInvoiceDetails(invoiceID);

			gvInvoiceLines.DataSource = invoiceDetailLines.OrderBy(x => x.LineDate);

			gvInvoiceLines.DataBind();

			// total invoice lines
			if (invoiceDetailLines != null && invoiceDetailLines.Count > 0) {
				totalAmount = invoiceDetailLines.Where(x => x.isBillable == true).Sum(x => x.LineAmount);


				txtTotalAmount.Text = string.Format("{0:N2}", totalAmount);
			}
		}

		protected void clearFields() {

		}

		//protected void bindBillTo(Lead lead) {
		//	ListItem billToItem = null;
		//	string itemValue = null;			
		//	string claimantName = null;

		//	ddlBillTo.Items.Add(new ListItem("Select One", "0"));

		//	if (lead != null && lead.LeadPolicies != null && lead.LeadPolicies.Count > 0) {

		//		// add insurance company policy
		//		foreach (LeadPolicy policy in lead.LeadPolicies) {
		//			if (!string.IsNullOrEmpty(policy.InsuranceCompanyName)) {						
		//				itemValue = string.Format("{0}|{1}", policy.PolicyType, policy.CarrierID ?? 0);

		//				billToItem = new ListItem(policy.InsuranceCompanyName, itemValue);

		//				ddlBillTo.Items.Add(billToItem);
		//			}
		//		}

		//		// add client mailing address as option
		//		claimantName = string.Format("{0} {1}", lead.ClaimantFirstName ?? "", lead.ClaimantLastName ?? "");

		//		itemValue = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
		//					1,			// homeowners
		//					claimantName,
		//					lead.MailingAddress ?? "",
		//					lead.MailingCity ?? "",
		//					lead.MailingState ?? "",
		//					lead.MailingZip ?? ""
		//					);
		//		ddlBillTo.Items.Add(new ListItem("Policyholder - Mailing Address", itemValue));

		//		// add client loss address as option
		//		claimantName = string.Format("{0} {1}", lead.ClaimantFirstName ?? "", lead.ClaimantLastName ?? "");

		//		itemValue = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
		//					1,			// homeowners
		//					claimantName,
		//					(lead.LossAddress ?? "") + (lead.LossAddress2 ?? ""),
		//					lead.CityName ?? "",
		//					lead.StateName ?? "",
		//					lead.Zip ?? ""
		//					);
		//		ddlBillTo.Items.Add(new ListItem("Policyholder - Loss Address", itemValue));
		//	}
		//}

		protected void bindData() {
			int claimID = 0;
			int clientID = 0;
			int invoiceID = 0;
			int leadID = 0;
			int policyID = 0;
			string dencryptedID = null;

			Invoice invoice = null;
			InvoiceDetail invoiceDetail = null;
			List<InvoiceDetail> invoiceDetails = null;

			// get id for current lead
			claimID = Core.SessionHelper.getClaimID();

			// get id for current lead
			leadID = Core.SessionHelper.getLeadId();

			// get client id
			clientID = Core.SessionHelper.getClientId();

			// get current policy
			policyID = Core.SessionHelper.getPolicyID();

			// check for new invoice or edit invoice
			if (Request.Params["q"] != null) {
				dencryptedID = Core.SecurityManager.DecryptQueryString(Request.Params["q"].ToString());
				ViewState["InvoiceID"] = dencryptedID;
			}
			else {
				ViewState["InvoiceID"] = "0";
			}


			int.TryParse(ViewState["InvoiceID"].ToString(), out invoiceID);

			// get lead/claim
			Leads lead = LeadsManager.GetByLeadId(leadID);

			// get policy type
			lblPolicyType.Text = LeadPolicyManager.GetPolicyTypeDescription(policyID);

			// get policy number
			lblPolicyNumber.Text = LeadPolicyManager.GetPolicyNumber(policyID);

			// get insurer claim number
			lblInsurerClaimNumber.Text = ClaimsManager.getInsurerClaimNumber(claimID);

			// get policy information
			if (lead != null) {
				lblClient.Text = string.Format("<b>{0} {1}<br/>{2}<br/>{3}<br/>{4}, {5} {6}</b>",
						lead.ClaimantFirstName ?? "",		//0
						lead.ClaimantLastName ?? "",		//1
						lead.LossAddress ?? "",			//2
						lead.LossAddress2 ?? "",			//3
						lead.CityName ?? "",			//4
						lead.StateName ?? "",			//5
						lead.Zip ?? ""					//6
						);

			}



			if (invoiceID == 0) {
				// new invoice		
				txtInvoiceDate.Text = DateTime.Now.ToShortDateString();

				invoiceDetail = new InvoiceDetail();
				invoiceDetail.LineDate = DateTime.Now;

				invoiceDetails = new List<InvoiceDetail>();
				invoiceDetails.Add(invoiceDetail);

				gvInvoiceLines.DataSource = invoiceDetails;

				gvInvoiceLines.DataBind();


				// hide empty row
				gvInvoiceLines.Rows[0].Visible = false;

				// hide print button
				//btnPrint.Visible = false;

				// hiden invoice number
				//pnlInvoiceNumber.Visible = false;

			}
			else {
				// edit invoice
				invoice = InvoiceManager.Get(invoiceID);

				// show print button
				//btnPrint.Visible = true;

				if (invoice != null && invoice.InvoiceDetail != null) {
					txtInvoiceDate.Text = string.Format("{0:MM/dd/yyyy}", invoice.InvoiceDate);
					txtDueDate.Text = string.Format("{0:MM/dd/yyyy}", invoice.DueDate);

					txtBillTo.Text = invoice.BillToName;
					txtBillToAddress1.Text = invoice.BillToAddress1;
					txtBillToAddress2.Text = invoice.BillToAddress2;
					txtBillToAddress3.Text = invoice.BillToAddress3;

					// reference
					txtReferenceNumber.Text = (invoice.InvoiceNumber ?? 0).ToString();

					// show total
					txtTotalAmount.Text = string.Format("{0:N2}", invoice.TotalAmount);


					// sort line items by date
					gvInvoiceLines.DataSource = invoice.InvoiceDetail.OrderBy(x => x.LineDate);

					gvInvoiceLines.DataBind();

					// show invoice numebr
					//pnlInvoiceNumber.Visible = true;


					txtInvoiceNumber.Text = string.Format("{0:N0}", invoice.InvoiceNumber ?? 0);
				}
			}


			bindInvoiceServices();
		}

		protected void bindInvoiceServices() {
			int clientID = Core.SessionHelper.getClientId();
			string isBillable = null;
			string unitDescription = null;
			GridViewRow footerRow = this.gvInvoiceLines.FooterRow;

			List<InvoiceServiceType> invoiceServices = InvoiceServiceManager.GetAll(clientID).ToList();

			DropDownList cbx = footerRow.FindControl("cbxServiceDescription") as DropDownList;

			if (cbx != null && invoiceServices != null && invoiceServices.Count > 0) {
				// clear all items
				cbx.Items.Clear();

				// add select options
				cbx.Items.Add(new ListItem("Select service", "0"));

				foreach (InvoiceServiceType invoiceService in invoiceServices) {
					if (invoiceService.InvoiceServiceUnit != null) {
						isBillable = ((bool)invoiceService.InvoiceServiceUnit.IsActive) ? "1" : "0";
						unitDescription = invoiceService.InvoiceServiceUnit.UnitDescription;
					}
					else {
						isBillable = "0";
						unitDescription = invoiceService.ServiceDescription;
					}


					ListItem item = new ListItem(invoiceService.ServiceDescription.Trim(),
												string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
												invoiceService.ServiceTypeID,
												invoiceService.ServiceRate ?? 0,
												unitDescription,
												isBillable,
												invoiceService.ServicePercentage ?? 0,
												invoiceService.DefaultQty ?? 0
												));
					cbx.Items.Add(item);
				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			Page.Validate("invoice");
			if (!Page.IsValid)
				return;

			saveInvoice();
		}

		//protected void bindInvoiceLines() {
		//     int invoiceID = 0;
		//     List<LeadInvoiceDetail> invoiceDetailLines = null;

		//     invoiceID = Convert.ToInt32(hf_invoiceID.Value);

		//     invoiceDetailLines = LeadInvoiceDetailManager.GetInvoiceDetails(invoiceID);

		//     gvInvoiceLines.DataSource = invoiceDetailLines;

		//     gvInvoiceLines.DataBind();
		//}

		private void calculateLineTotal() {
			int invoiceID = 0;
			decimal qty = 0;
			decimal rate = 0;
			decimal totalAmount = 0;
			decimal taxableTotal = 0;
			string unitDescription = null;
			List<InvoiceDetail> invoiceDetails = null;

			// get service unit type
			Label lblUnitDescription = gvInvoiceLines.FooterRow.FindControl("lblUnitDescription") as Label;
			if (lblUnitDescription != null)
				unitDescription = lblUnitDescription.Text;

			// get quantity
			WebNumericEditor txtQty = gvInvoiceLines.FooterRow.FindControl("txtQty") as WebNumericEditor;
			if (txtQty != null)
				qty = txtQty.Value == null ? 0 : Convert.ToDecimal(txtQty.Value);

			// rate
			WebNumericEditor txtRate = gvInvoiceLines.FooterRow.FindControl("txtRate") as WebNumericEditor;

			// remove any % symbol
			if (txtRate != null)
				rate = txtRate.Value == null ? 0 : Convert.ToDecimal(txtRate.Value);

			if (!string.IsNullOrEmpty(unitDescription) && unitDescriptions.Contains(unitDescription.ToLower()))
				rate /= 100;	// make it a percent for "percentage, sales tax"

			totalAmount = rate * qty;

			if (!string.IsNullOrEmpty(unitDescription) && unitDescription.ToLower().Equals("sales tax")) {
				int.TryParse(ViewState["InvoiceID"].ToString(), out invoiceID);

				invoiceDetails = InvoiceDetailManager.GetInvoiceDetails(invoiceID);

				taxableTotal = (decimal)invoiceDetails.Where(x => x.isBillable == true &&
													x.InvoiceServiceType.InvoiceServiceUnit.UnitID != SALES_TAX_ID)
													.Sum(x => x.LineAmount);

				totalAmount = taxableTotal * rate;
			}


			TextBox txtLineAmount = gvInvoiceLines.FooterRow.FindControl("txtLineAmount") as TextBox;
			if (txtLineAmount != null) {
				txtLineAmount.Text = totalAmount.ToString("N2");
			}
		}

		
		// We come here each time user selected invoice service
		protected void cbxServiceDescription_selectedIndexChanged(object sender, EventArgs e) {
			DropDownList cbx = sender as DropDownList;
			string billable = null;
			string[] values = null;
			decimal rate = 0;
			decimal percentage = 0;
			decimal qty = 0;
			decimal lineTotal = 0;
			string unitDescription = null;


			values = cbx.SelectedValue.Split(new char[] { '|' });

			if (values.Length >= 4) {
				// get rate
				decimal.TryParse(values[1], out rate);

				// unit description, i.e., Hrs
				unitDescription = values[2];

				// is billable
				billable = values[3];

				// percentage
				decimal.TryParse(values[4], out percentage);

				// default qty
				decimal.TryParse(values[5], out qty);


				CheckBox cbxBillable = gvInvoiceLines.FooterRow.FindControl("cbxBillable") as CheckBox;
				if (cbxBillable != null)
					cbxBillable.Checked = billable.Equals("1");

				Label lblUnitDescription = gvInvoiceLines.FooterRow.FindControl("lblUnitDescription") as Label;
				if (lblUnitDescription != null)
					lblUnitDescription.Text = unitDescription;

				// default quantity
				WebNumericEditor txtQty = gvInvoiceLines.FooterRow.FindControl("txtQty") as WebNumericEditor;
				txtQty.Value = qty;

				if (unitDescription.ToLower() == "sales tax") {
					if (txtQty != null) {
						txtQty.Text = "1";
					}
				}

				// determine if percentage rate instead if rate fee
				if (percentage > 0)
					rate = (percentage / 100);

				lineTotal = qty * rate;

				// show rate to use
				WebNumericEditor txtRate = gvInvoiceLines.FooterRow.FindControl("txtRate") as WebNumericEditor;
				if (txtRate != null && !string.IsNullOrEmpty(unitDescription)) {
					txtRate.Value = rate;
				}

				TextBox txtLineAmount = gvInvoiceLines.FooterRow.FindControl("txtLineAmount") as TextBox;
				txtLineAmount.Text = lineTotal.ToString("N2");

				calculateLineTotal();
			}

		}

		protected void ddlBillTo_SelectedIndexChanged(object sender, EventArgs e) {
			DropDownList ddl = sender as DropDownList;
			string[] billToValues = null;
			Carrier carrier = null;
			int carrierID = 0;


			if (ddl.SelectedIndex > 0) {
				try {
					billToValues = ddl.SelectedValue.Split(new char[] { '|' });

					carrierID = Convert.ToInt32(billToValues[1]);

					carrier = CarrierManager.Get(carrierID);

					if (carrier != null) {
						txtBillTo.Text = carrier.CarrierName;
						txtBillToAddress1.Text = carrier.AddressLine1;
						txtBillToAddress2.Text = carrier.AddressLine2;
						txtBillToAddress3.Text = string.Format("{0}, {1} {2}",
							carrier.CityMaster == null ? "" : carrier.CityMaster.CityName,
							carrier.StateMaster == null ? "" : carrier.StateMaster.StateName,
							carrier.ZipCode ?? "");

					}

				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}

			}
		}

		protected void deleteInvoiceComment(int invoiceLineID) {
			LeadCommentManager.DeleteLeadCommentByReferenceId(invoiceLineID);
		}
		/// <summary>
		/// add item to invoice
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ibtnAdd_Click(object sender, ImageClickEventArgs e) {
			Page.Validate("invoice");
			if (!Page.IsValid)
				return;

			Page.Validate("invoiceLine");
			if (!Page.IsValid)
				return;


			saveInvoice();

		}

		/// <summary>
		/// Gathers information from gridview footer
		/// </summary>
		/// <returns></returns>
		protected InvoiceDetail getInvoiceDetailLine() {
			decimal qty = 0;
			decimal rate = 0;
			decimal totalAmount = 0;
			string[] values = null;
			int serviceTypeID = 0;
			DateTime date = DateTime.MaxValue;

			InvoiceDetail invoiceDetailLine = new InvoiceDetail();

			// service date
			WebDatePicker txtDate = gvInvoiceLines.FooterRow.FindControl("txtDate") as WebDatePicker;
			if (txtDate != null && !string.IsNullOrEmpty(txtDate.Text))
				invoiceDetailLine.LineDate = Convert.ToDateTime(txtDate.Text);


			// service description
			DropDownList cbx = gvInvoiceLines.FooterRow.FindControl("cbxServiceDescription") as DropDownList;

			if (cbx != null) {
				values = cbx.SelectedValue.Split(new char[] { '|' });
				if (values.Length >= 3) {
					int.TryParse(values[0], out serviceTypeID);

					invoiceDetailLine.ServiceTypeID = serviceTypeID;

					invoiceDetailLine.LineDescription = cbx.SelectedItem.Text.Trim();
				}
			}

			// quantity
			WebNumericEditor txtQty = gvInvoiceLines.FooterRow.FindControl("txtQty") as WebNumericEditor;
			if (txtQty != null) {
				qty = txtQty.Value == null ? 0 : Convert.ToDecimal(txtQty.Value);

				invoiceDetailLine.Qty = qty;
			}

			// unit description
			//Label lblUnitDescription = gvInvoiceLines.FooterRow.FindControl("lblUnitDescription") as Label;
			//if (lblUnitDescription != null)
			//	invoiceDetailLine.UnitDescription = lblUnitDescription.Text;

			// rate
			WebNumericEditor txtRate = gvInvoiceLines.FooterRow.FindControl("txtRate") as WebNumericEditor;
			if (txtRate != null) {
				rate = txtRate.Value == null ? 0 : Convert.ToDecimal(txtRate.Value);

				invoiceDetailLine.Rate = rate;
			}

			// total amount
			// quantity
			TextBox txtLineAmount = gvInvoiceLines.FooterRow.FindControl("txtLineAmount") as TextBox;

			decimal.TryParse(txtLineAmount.Text.Trim().Replace(",", ""), out totalAmount);
			invoiceDetailLine.LineAmount = totalAmount;
			invoiceDetailLine.Total = totalAmount;

			//if (invoiceDetailLine.UnitDescription != null && invoiceDetailLine.UnitDescription.ToLower() == "percentage")
			//     totalAmount = (rate / 100 ) * qty;
			//else
			//     totalAmount = rate * qty;



			// comments
			WebTextEditor txtComments = gvInvoiceLines.FooterRow.FindControl("txtComments") as WebTextEditor;
			if (txtComments != null)
				invoiceDetailLine.Comments = txtComments.Text.Trim();

			// is billable
			CheckBox cbxBillable = gvInvoiceLines.FooterRow.FindControl("cbxBillable") as CheckBox;
			if (cbxBillable != null)
				invoiceDetailLine.isBillable = cbxBillable.Checked;

			return invoiceDetailLine;
		}

		protected void gvInvoiceLines_DataBound(object sender, EventArgs e) {
			bindInvoiceServices();
		}

		protected void gvInvoiceLines_OnRowDataBound(object sender, GridViewRowEventArgs e) {
			if (e.Row.RowType == DataControlRowType.DataRow) {
			}
			else if (e.Row.RowType == DataControlRowType.Footer) {
			}
		}

		// process gridview commands
		protected void gvInvoiceLines_RowCommand(object sender, GridViewCommandEventArgs e) {
			int invoiceID = 0;
			int invoiceLineID = 0;
			InvoiceDetail invoiceLine = null;
			TextBox txtLineAmount;

			switch (e.CommandName) {


				case "DoEdit":
					invoiceLineID = Convert.ToInt32(e.CommandArgument);

					invoiceLine = InvoiceDetailManager.Get(invoiceLineID);

					if (invoiceLine != null) {
						ViewState["InvoiceLineID"] = invoiceLineID.ToString();


						// date
						WebDatePicker txtDate = gvInvoiceLines.FooterRow.FindControl("txtDate") as WebDatePicker;
						if (txtDate != null && invoiceLine.LineDate != null)
							txtDate.Text = string.Format("{0:MM/dd/yyyy}", invoiceLine.LineDate);

						// service description
						DropDownList cbx = gvInvoiceLines.FooterRow.FindControl("cbxServiceDescription") as DropDownList;
						if (cbx != null && invoiceLine.ServiceTypeID != null) {
							ListItem item = cbx.Items.FindByText(invoiceLine.LineDescription.Trim());
							if (item != null)
								cbx.SelectedIndex = cbx.Items.IndexOf(item);
						}
						else {
							ListItem item = new ListItem(invoiceLine.LineDescription, invoiceLine.LineDescription);
							cbx.Items.Add(item);
							cbx.Text = invoiceLine.LineDescription;
						}

						// quantity
						WebNumericEditor txtQty = gvInvoiceLines.FooterRow.FindControl("txtQty") as WebNumericEditor;
						if (txtQty != null && invoiceLine.Qty != null)
							txtQty.Text = invoiceLine.Qty.ToString();

						// rate
						WebNumericEditor txtRate = gvInvoiceLines.FooterRow.FindControl("txtRate") as WebNumericEditor;
						if (txtRate != null)
							txtRate.Value = invoiceLine.Rate;

						//// service unit
						//Label lblUnitDescription = gvInvoiceLines.FooterRow.FindControl("lblUnitDescription") as Label;
						//if (lblUnitDescription != null && invoiceLine.InvoiceServiceType != null && invoiceLine.InvoiceServiceType.InvoiceServiceUnit != null)
						//	lblUnitDescription.Text = invoiceLine.InvoiceServiceType.InvoiceServiceUnit.UnitDescription;

						// total amount
						txtLineAmount = gvInvoiceLines.FooterRow.FindControl("txtLineAmount") as TextBox;
						if (txtLineAmount != null)
							txtLineAmount.Text = string.Format("{0:N2}", invoiceLine.LineAmount ?? 0);

						//billable
						CheckBox cbxBillable = gvInvoiceLines.FooterRow.FindControl("cbxBillable") as CheckBox;
						if (cbxBillable != null && invoiceLine.isBillable != null)
							cbxBillable.Checked = invoiceLine.isBillable ?? false;

						// comments
						WebTextEditor txtComments = gvInvoiceLines.FooterRow.FindControl("txtComments") as WebTextEditor;
						if (txtComments != null && invoiceLine.Comments != null)
							txtComments.Text = invoiceLine.Comments;

						// show cancel icon
						ImageButton ibtnCancel = gvInvoiceLines.FooterRow.FindControl("ibtnCancel") as ImageButton;
						ibtnCancel.Visible = true;
					}
					break;

				case "DoDelete":
					invoiceLineID = Convert.ToInt32(e.CommandArgument);
					if (invoiceLineID > 0) {
						using (TransactionScope scope = new TransactionScope()) {
							try {

								InvoiceDetailManager.Delete(invoiceLineID);

								deleteInvoiceComment(invoiceLineID);

								invoiceID = Convert.ToInt32(ViewState["InvoiceID"].ToString());

								bindInvoiceDetails(invoiceID);

								// complete transaction
								scope.Complete();

							}
							catch (Exception ex) {
								lblMessage.Text = "Error while deleting invoice detail line.";
								lblMessage.CssClass = "error";
								Core.EmailHelper.emailError(ex);
							}
						}
					}

					break;

			}
		}

		protected bool isEditMode() {
			bool isVisible = false;

			isVisible = (ViewState["InvoiceLineID"] != null && !ViewState["InvoiceLineID"].ToString().Equals("0"));

			return isVisible;
		}

		private void saveInvoice() {
			int clientID = 0;			
			int invoiceID = 0;
			int InvoiceLineID = 0;
			Invoice invoice = null;
			InvoiceDetail invoiceDetailLine = null;
			InvoiceDetail invoiceDetail = null;

			int nextInvoiceNumber = 0;
			int policyID = 0;
			decimal taxAmount = 0;

			
			// get invoice id 
			invoiceID = Convert.ToInt32(ViewState["InvoiceID"].ToString());

			clientID = Core.SessionHelper.getClientId();

			// current policy being edited
			policyID = Session["policyID"] == null ? 0 : Convert.ToInt32(Session["policyID"]);
			//policy = LeadPolicyManager.GetByID(policyID);

			if (invoiceID == 0) {
				invoice = new Invoice();

				// get id for current lead
				invoice.ClaimID = Core.SessionHelper.getClaimID();

				invoice.IsVoid = false;

				// assign client
				//invoice.ClientID = clientID;

				// hide print button
				//btnPrint.Visible = false;
			}
			else {
				invoice = InvoiceManager.Get(invoiceID);

				// show print button
				//btnPrint.Visible = true;

			}


			//invoice.PolicyID = policy.PolicyType;

			invoice.InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
			invoice.DueDate = Convert.ToDateTime(txtDueDate.Text);
			invoice.BillToName = txtBillTo.Text.Trim();
			invoice.BillToAddress1 = txtBillToAddress1.Text.Trim();
			invoice.BillToAddress2 = txtBillToAddress2.Text.Trim();
			invoice.BillToAddress3 = txtBillToAddress3.Text.Trim();

			//invoice.AdjusterID = policy.AdjusterID;

			//invoice.AdjusterInvoiceNumber = txtReferenceNumber.Text.Trim();

			try {
				using (TransactionScope scope = new TransactionScope()) {
					if (invoiceID == 0) {
						// assign next invoice number to new invoice
						nextInvoiceNumber = InvoiceManager.GetNextInvoiceNumber(clientID);

						invoice.InvoiceNumber = nextInvoiceNumber;
					}

					invoiceID = InvoiceManager.Save(invoice);

					// save newly generated invoice id
					ViewState["InvoiceID"] = invoiceID.ToString();

					// check for add/edit invoice detail line
					InvoiceLineID = ViewState["InvoiceLineID"] == null ? 0 : Convert.ToInt32(ViewState["InvoiceLineID"]);

					if (InvoiceLineID > 0)
						invoiceDetail = InvoiceDetailManager.Get(InvoiceLineID);
					else
						invoiceDetail = new InvoiceDetail();


					// get detail line from gridview footer
					if (invoiceDetail != null) {
						invoiceDetailLine = getInvoiceDetailLine();

						if (invoiceDetailLine.LineDate != null && !string.IsNullOrEmpty(invoiceDetailLine.LineDescription) && invoiceDetailLine.Qty > 0 &&
							invoiceDetailLine.Rate > 0) {

							// update fields
							invoiceDetail.InvoiceID = invoiceID;
							invoiceDetail.InvoiceLineID = InvoiceLineID;
							invoiceDetail.ServiceTypeID = invoiceDetailLine.ServiceTypeID;
							invoiceDetail.Comments = invoiceDetailLine.Comments;
							invoiceDetail.isBillable = invoiceDetailLine.isBillable;
							invoiceDetail.LineAmount = invoiceDetailLine.LineAmount;
							invoiceDetail.LineDate = invoiceDetailLine.LineDate;
							invoiceDetail.LineDescription = invoiceDetailLine.LineDescription;
							invoiceDetail.Qty = invoiceDetailLine.Qty;
							invoiceDetail.Rate = invoiceDetailLine.Rate;
							invoiceDetail.UnitDescription = invoiceDetailLine.UnitDescription;
							invoiceDetail.Total = invoiceDetailLine.Total;

							// save invoice detail
							InvoiceDetailManager.Save(invoiceDetail);

							// clear
							ViewState["InvoiceLineID"] = "0";

							// update invoice total after adding 
							invoice = InvoiceManager.Get(invoiceID);

							invoice.TotalAmount = invoice.InvoiceDetail.Where(x => x.isBillable == true).Sum(x => x.LineAmount);

							taxAmount = (invoice.TotalAmount ?? 0) * (invoice.TaxRate / 100);

							//invoice.TotalAmount = invoice.TotalAmount + taxAmount;

							InvoiceManager.Save(invoice);
							
							// update comment
							updateInvoiceComment(invoice, invoiceDetail);
						}
					}

					// 2014-05-02 apply rule
					using (SpecificExpenseTypePerCarrier ruleEngine = new SpecificExpenseTypePerCarrier()) {
						RuleException ruleException = ruleEngine.TestRule(clientID, invoice);

						if (ruleException != null) {
							ruleException.UserID = Core.SessionHelper.getUserId();
							ruleEngine.AddException(ruleException);
                            CheckSendMail(ruleException);
						}
					}

					// complete transaction
					scope.Complete();
				}

				// refresh invoice detail lines
				bindInvoiceDetails(invoiceID);

				// refresh invoice number on UI
				txtInvoiceNumber.Text = (invoice.InvoiceNumber ?? 0).ToString();

				clearFields();

				showToolbarButtons();
                lblMessage.Text = "Invoice save successfully.";
                lblMessage.CssClass = "ok";
			}
			catch (Exception ex) {
				lblMessage.Text = "Error while saving invoice.";
				lblMessage.CssClass = "error";
				Core.EmailHelper.emailError(ex);
			}
		}

		protected void updateInvoiceComment(Invoice invoice, InvoiceDetail invoiceDetail) {
			ClaimComment comment = null;

			// retrieve comment by reference
			//comment = ClaimCommentManager.GetLeadCommentByReferenceId(invoiceDetail.InvoiceLineID);

			if (comment == null)
				comment = new ClaimComment();

			comment.UserId = Core.SessionHelper.getUserId();
			comment.CommentDate = DateTime.Now;
			comment.ClaimID = invoice.ClaimID;
			//comment.ReferenceID = invoiceDetail.InvoiceLineID;
			comment.IsActive = true;	// active
			comment.CommentText = string.Format("<div>Invoice # {0} - {1:MM-dd-yyyy} for {2} Qty:{3:N2} Rate:{4:N2} Item Total:{5:N2}</div><div>{6}</div>",
				invoice.InvoiceNumber,
				invoiceDetail.LineDate,
				invoiceDetail.LineDescription ?? "",
				invoiceDetail.Qty ?? 0,
				invoiceDetail.Rate ?? 0,
				invoiceDetail.LineAmount ?? 0,
				invoiceDetail.Comments ?? ""
				);

			ClaimCommentManager.Save(comment);
		}

		private void showToolbarButtons() {
			btnPrint.Visible = true;

			lbtnEmail.Visible = true;
		}

		protected void txtRate_TextChanged(object sender, EventArgs e) {
			calculateLineTotal();
		}

		protected void txtQty_TextChanged(object sender, EventArgs e) {
			calculateLineTotal();
		}

		protected void ibtnCancel_Click(object sender, ImageClickEventArgs e) {
			int invoiceID = 0;

			int.TryParse(ViewState["InvoiceID"].ToString(), out invoiceID);

			ViewState["InvoiceLineID"] = "0";

			// refresh invoice detail lines
			bindInvoiceDetails(invoiceID);

			clearFields();
		}

		protected void btnEmailInvoice_Click(object sender, EventArgs e) {
			string reportPath = null;
			int invoiceID = 0;
			Invoice invoice = null;
			int port = 0;
			string password = null;
			CRM.Data.Entities.SecUser user = null;
			string subject = "Invoice";
			int userID = Core.SessionHelper.getUserId();
			string bodyText = "Invoiced is attached.";

			//Page.Validate("emailInvoice");
			//if (!Page.IsValid)
			//	return;

			try {
				invoiceID = Convert.ToInt32(ViewState["InvoiceID"].ToString());

				reportPath = generateInvoicePDF(invoiceID);

				user = SecUserManager.GetByUserId(userID);

				if (user != null && !string.IsNullOrEmpty(reportPath)) {
					invoice = InvoiceManager.Get(invoiceID);

					string[] recipients = txtEmailTo.Text.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

					string[] attachments = new string[] { reportPath };

					port = Convert.ToInt32(user.emailHostPort);

					password = Core.SecurityManager.Decrypt(user.emailPassword);

                    //new email functionality by chetu
                    string crsupportEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
                    string crsupportEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
                    Core.EmailHelper.sendEmail(crsupportEmail, recipients, null, subject, bodyText, attachments, crsupportEmail, crsupportEmailPassword);
					///////////////////
                    //Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, bodyText, attachments, user.emailHost, port, user.Email, password);

					// add copy of invoice to documet list
					updateDocumentLog(reportPath, invoice.ClaimID);

					lblMessage.Text = "Invoice mailed successfully.";
					lblMessage.CssClass = "ok";
				}
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
				lblMessage.Text = "Unable to email invoice.";
				lblMessage.CssClass = "error";
			}
		}

		protected string generateInvoicePDF(int invoiceID) {
			string reportPath = null;

			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			invoiceReport = InvoiceManager.GetInvoiceForReport(invoiceID);

			if (invoiceReport != null) {
				reportViewer.Reset();

				reportViewer.LocalReport.DataSources.Clear();

				reportViewer.LocalReport.EnableExternalImages = true;

				ReportDataSource reportDataSource = new ReportDataSource();

				reportDataSource.Name = "DataSet1";

				reportDataSource.Value = invoiceReport;

				reportViewer.LocalReport.DataSources.Add(reportDataSource);

				reportViewer.LocalReport.ReportPath = Server.MapPath("~/Content/Invoice.rdlc");

				reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subReportsEventHandler);

				reportPath = string.Format("{0}/Temp/Invoice_{1}.pdf", appPath, invoiceID);

				Core.ReportHelper.savePDFFromLocalReport(reportViewer.LocalReport, reportPath);
			}

			return reportPath;
		}

		protected void subReportsEventHandler(object sender, SubreportProcessingEventArgs e) {
			e.DataSources.Add(new ReportDataSource("DataSet1", invoiceReport[0].invoiceLines));
		}

		private void updateDocumentLog(string reportPath, int claimID) {
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();
			string documentPath = null;

			ClaimDocument objLeadDoc = new ClaimDocument();

			string ActualFileName = Path.GetFileName(reportPath);

			string FileNameWithoutExt = Path.GetFileNameWithoutExtension(reportPath);

			objLeadDoc.ClaimID = claimID;

			objLeadDoc.Description = "Invoice";

			objLeadDoc.DocumentName = ActualFileName;
			objLeadDoc.IsPrint = false;

			objLeadDoc.DocumentDate = DateTime.Now;

			//objLeadDoc.PolicyTypeID = policyTypeID;

			objLeadDoc = ClaimDocumentManager.Save(objLeadDoc);

			if (objLeadDoc.ClaimDocumentID > 0) {
				//documentPath = appPath + "/ClaimDocuments/" + claimID + "/" + objLeadDoc.ClaimDocumentID;

				documentPath = string.Format("{0}/ClaimDocuments/{1}/{2}", appPath, claimID, objLeadDoc.ClaimDocumentID);
				if (!Directory.Exists(documentPath)) {
					Directory.CreateDirectory(documentPath);
				}

				File.Copy(reportPath, documentPath + "/" + ActualFileName, true);
			}
		}

		public List<BillToView> getBillToCollection(int claimID) {
			string claimantName = null;
			Claim claim = null;
			Carrier carrier = null;
			Leads lead = null;
			List<BillToView> billingCollection = new List<BillToView>();
			BillToView billCarrier = null;
			BillToView billClaimantMailingAddress = null;
			BillToView billClaimantLossAddress = null;

			claim = ClaimsManager.Get(claimID);

			if (claim != null) {

				// add insurance company 
				if (claim.LeadPolicy.Carrier != null) {
					carrier = claim.LeadPolicy.Carrier;
					billCarrier = new BillToView();

					billCarrier.billTo = "Carrier";
					billCarrier.billingName = carrier.CarrierName;
					billCarrier.mailingAddress = string.Format("{0} {1}", carrier.AddressLine1 ?? "", carrier.AddressLine2 ?? "");
					billCarrier.mailingCity = carrier.CityMaster == null ? "" : carrier.CityMaster.CityName;
					billCarrier.mailingState = carrier.StateMaster == null ? "" : carrier.StateMaster.StateName;
					billCarrier.mailingZip = carrier.ZipCode;

					billingCollection.Add(billCarrier);

				}

				// add claimant mailing address				
				lead = claim.LeadPolicy.Leads;
				claimantName = string.Format("{0} {1}", lead.ClaimantFirstName ?? "", lead.ClaimantLastName ?? "");

				billClaimantMailingAddress = new BillToView();
				billClaimantMailingAddress.billTo = "Policyholder - Mailing Address";
				billClaimantMailingAddress.billingName = claimantName;
				billClaimantMailingAddress.mailingAddress = lead.MailingAddress ?? "";
				billClaimantMailingAddress.mailingCity = lead.MailingCity ?? "";
				billClaimantMailingAddress.mailingState = lead.MailingState ?? "";
				billClaimantMailingAddress.mailingZip = lead.MailingZip ?? "";
				billingCollection.Add(billClaimantMailingAddress);

				// add claimant loss address
				billClaimantLossAddress = new BillToView();
				billClaimantLossAddress.billTo = "Policyholder - Loss Address";
				billClaimantLossAddress.billingName = claimantName;
				billClaimantLossAddress.mailingAddress = string.Format("{0} {1}", lead.LossAddress ?? "", lead.LossAddress2 ?? "");
				billClaimantLossAddress.mailingCity = lead.CityName ?? "";
				billClaimantLossAddress.mailingState = lead.StateName ?? "";
				billClaimantLossAddress.mailingZip = lead.Zip ?? "";
				billingCollection.Add(billClaimantLossAddress);


			}
			return billingCollection;
		}

        #region send reg flag mail

        public static void CheckSendMail(RuleException ruleExp)
        {
            if (ruleExp != null)
            {
                string adjusterEmail = string.Empty;
                string supervisorEmail = string.Empty;
                bool sendAdjuster = false;
                bool sendSupervisor = false;
                string recipient = string.Empty;
                int claimId = 0;

                BusinessRuleManager objRuleManager = new BusinessRuleManager();
                BusinessRule objRule = new BusinessRule();
                CRM.Data.Entities.Claim objClaim = new CRM.Data.Entities.Claim();
                CRM.Data.Entities.SecUser objSecUser = new Data.Entities.SecUser();
                AdjusterMaster adjustermaster = new AdjusterMaster();

                int businessRuleID = ruleExp.BusinessRuleID ?? 0;
                objRule = objRuleManager.GetBusinessRule(businessRuleID);
                if (objRule != null)
                {
                    claimId = ruleExp.ObjectID ?? 0;

                    objClaim = objRuleManager.GetClaim(claimId);
                    adjustermaster = objRuleManager.GetAdjuster(objClaim.AdjusterID ?? 0);
                    objSecUser = objRuleManager.GetSupervisor(objClaim.SupervisorID ?? 0);
                    if (objSecUser != null)
                    {
                        adjusterEmail = adjustermaster.email;
                        supervisorEmail = objSecUser.Email;

                        sendAdjuster = objRule.EmailAdjuster;
                        sendSupervisor = objRule.EmailSupervisor;

                        if (sendAdjuster == true && sendSupervisor == true)
                        {
                            recipient = adjusterEmail + "," + supervisorEmail;
                            notifyUser(objRule.Description, claimId, recipient);
                        }
                        else if (sendAdjuster == false && sendSupervisor == true)
                        {
                            recipient = supervisorEmail;
                            notifyUser(objRule.Description, claimId, recipient);
                        }
                        else if (sendAdjuster == true && sendSupervisor == false)
                        {

                            recipient = adjusterEmail;
                            notifyUser(objRule.Description, claimId, recipient);
                        }
                    }
                }
            }

        }

        public static void notifyUser(string description, int claimid, string recipient)
        {

            StringBuilder emailBody = new StringBuilder();
            string password = null;
            string[] recipients = null;
            string smtpHost = null;
            int smtpPort = 0;
            //string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
            string subject = "Red-Flag Alert: " + description + " Claim # " + claimid;
            CRM.Data.Entities.SecUser user = null;

            // get logged in user
            int userID = SessionHelper.getUserId();
            // get logged in user email info
            user = SecUserManager.GetByUserId(userID);
            // load email credentials
            smtpHost = user.emailHost;
            int.TryParse(user.emailHostPort, out smtpPort);

            // recipients
            recipients = new string[] { recipient };

            // build email body
            // .containerBox
            emailBody.Append("<div>");
            emailBody.Append("<div>");
            emailBody.Append("Claim Ruler Red Flag Alert.<br><br>");
            emailBody.Append("Please correct the following issue as soon as possible:  ");
            emailBody.Append(description + "with claim # " + claimid);
            emailBody.Append("</div>");	// paneContentInner
            emailBody.Append("</div>");	// containerBox

            password = Core.SecurityManager.Decrypt(user.emailPassword);

            Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, true);//user.isSSL ??

        }





        #endregion



	}
}