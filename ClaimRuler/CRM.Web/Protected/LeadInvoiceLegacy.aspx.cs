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

using Microsoft.Reporting.WebForms;
using CRM.Data.Entities;


namespace CRM.Web.Protected {
	public partial class LeadInvoiceLegacy : System.Web.UI.Page {

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

			if (invoiceID > 0) {
				string js = string.Format("window.open('../Content/PrintInvoiceLegacy.aspx?id={0}', 'invoice', 'status=0, toolbar=0, width=800, height=800');", invoiceID);

				ScriptManager.RegisterStartupScript(Page, typeof(Page), "Print_Invoice", js, true);
			}
		}

		protected void btnInvoiceLedger_Click(object sender, EventArgs e) {
			Response.Redirect("~/Protected/LeadInvoiceLedger.aspx");
		}

		protected void bindInvoiceDetails(int invoiceID) {
			List<LeadInvoiceDetail> invoiceDetailLines = null;
			decimal? totalAmount = 0;

			invoiceDetailLines = LeadInvoiceDetailManager.GetInvoiceDetails(invoiceID);

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

		protected void bindBillTo(CRM.Data.Entities.Leads lead) {
			ListItem billToItem = null;
			string itemValue = null;
			string claimantName = null;

			ddlBillTo.Items.Add(new ListItem("Select One", "0"));

			if (lead != null && lead.LeadPolicy != null && lead.LeadPolicy.Count > 0) {

				// add insurance company policy
				foreach (CRM.Data.Entities.LeadPolicy policy in lead.LeadPolicy) {
					if (!string.IsNullOrEmpty(policy.InsuranceCompanyName)) {
						itemValue = string.Format("{0}|{1}", policy.PolicyType, policy.CarrierID ?? 0);

						billToItem = new ListItem(policy.InsuranceCompanyName, itemValue);

						ddlBillTo.Items.Add(billToItem);
					}
				}

				// add client mailing address as option
				claimantName = string.Format("{0} {1}", lead.ClaimantFirstName ?? "", lead.ClaimantLastName ?? "");

				itemValue = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
							1,			// homeowners
							claimantName,
							lead.MailingAddress ?? "",
							lead.MailingCity ?? "",
							lead.MailingState ?? "",
							lead.MailingZip ?? ""
							);
				ddlBillTo.Items.Add(new ListItem("Policyholder - Mailing Address", itemValue));

				// add client loss address as option
				claimantName = string.Format("{0} {1}", lead.ClaimantFirstName ?? "", lead.ClaimantLastName ?? "");

				itemValue = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
							1,			// homeowners
							claimantName,
							(lead.LossAddress ?? "") + (lead.LossAddress2 ?? ""),
							lead.CityName ?? "",
							lead.StateName ?? "",
							lead.Zip ?? ""
							);
				ddlBillTo.Items.Add(new ListItem("Policyholder - Loss Address", itemValue));
			}
		}

		protected void bindData() {
			int invoiceID = 0;
			int policyID = 0;

			CRM.Data.Entities.LeadInvoice invoice = null;
			List<LeadInvoiceDetail> invoiceDetails = null;

			// get id for current lead
			int leadID = Core.SessionHelper.getLeadId();

			// get client id
			int clientID = Core.SessionHelper.getClientId();

			// get current policy
			policyID = Core.SessionHelper.getPolicyID();

			// check for new invoice or edit invoice
			ViewState["InvoiceID"] = Request.Params["id"] == null ? "0" : Request.Params["id"].ToString();

			int.TryParse(ViewState["InvoiceID"].ToString(), out invoiceID);

			// get lead/claim
			Leads lead = LeadsManager.Get(leadID);

			if (lead != null) {
				bindBillTo(lead);

				lblClient.Text = string.Format("<b>{0} {1}<br/>{2}<br/>{3}<br/>{4}, {5} {6}</b>",
						lead.ClaimantFirstName ?? "",		//0
						lead.ClaimantLastName ?? "",		//1
						lead.LossAddress ?? "",			//2
						lead.LossAddress2 ?? "",			//3
						lead.CityName ?? "",	//4
						lead.StateName ?? "",	// 5
						lead.Zip ?? ""					//6
						);

			}


			if (invoiceID == 0) {
				// new invoice		
				txtInvoiceDate.Text = DateTime.Now.ToShortDateString();

				invoiceDetails = new List<LeadInvoiceDetail>();
				invoiceDetails.Add(new LeadInvoiceDetail());

				gvInvoiceLines.DataSource = invoiceDetails;

				gvInvoiceLines.DataBind();


				// hide empty row
				gvInvoiceLines.Rows[0].Visible = false;

				// hide print button
				//btnPrint.Visible = false;

				// hiden invoice number
				pnlInvoiceNumber.Visible = false;

			}
			else {
				// edit invoice
				invoice = LeadInvoiceManager.Get(invoiceID);

				// show print button
				//btnPrint.Visible = true;

				if (invoice != null && invoice.LeadInvoiceDetail != null) {
					txtInvoiceDate.Text = string.Format("{0:MM/dd/yyyy}", invoice.InvoiceDate);
					txtDueDate.Text = string.Format("{0:MM/dd/yyyy}", invoice.DueDate);

					txtBillTo.Text = invoice.BillToName;
					txtBillToAddress1.Text = invoice.BillToAddress1;
					txtBillToAddress2.Text = invoice.BillToAddress2;
					txtBillToAddress3.Text = invoice.BillToAddress3;

					// reference
					txtReferenceNumber.Text = invoice.AdjusterInvoiceNumber;

					// show total
					txtTotalAmount.Text = string.Format("{0:N2}", invoice.TotalAmount);


					// sort line items by date
					gvInvoiceLines.DataSource = invoice.LeadInvoiceDetail.OrderBy(x => x.LineDate);

					gvInvoiceLines.DataBind();

					// show invoice numebr
					pnlInvoiceNumber.Visible = true;


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
						unitDescription = "";
					}


					ListItem item = new ListItem(invoiceService.ServiceDescription.Trim(),
												string.Format("{0}|{1}|{2}|{3}", invoiceService.ServiceTypeID,
												invoiceService.ServiceRate,
												unitDescription,
												isBillable));
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
			List<LeadInvoiceDetail> invoiceDetails = null;

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

				invoiceDetails = LeadInvoiceDetailManager.GetInvoiceDetails(invoiceID);

				taxableTotal = (decimal)invoiceDetails.Where(x => x.isBillable == true &&
													x.InvoiceServiceType.InvoiceServiceUnit.UnitID != SALES_TAX_ID)
													.Sum(x => x.LineAmount);

				totalAmount = taxableTotal * rate;
			}


			TextBox txtLineAmount = gvInvoiceLines.FooterRow.FindControl("txtLineAmount") as TextBox;
			if (txtLineAmount != null)
				txtLineAmount.Text = totalAmount.ToString("N2");
		}

		// We come here each time user selected invoice service
		protected void cbxServiceDescription_selectedIndexChanged(object sender, EventArgs e) {
			DropDownList cbx = sender as DropDownList;
			string billable = null;
			string[] values = null;
			decimal rate = 0;
			string unitDescription = null;


			values = cbx.SelectedValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

			if (values.Length >= 4) {
				// get rate
				decimal.TryParse(values[1], out rate);

				// unit description, i.e., Hrs
				unitDescription = values[2];

				// is billable
				billable = values[3];

				WebNumericEditor txtRate = gvInvoiceLines.FooterRow.FindControl("txtRate") as WebNumericEditor;
				if (txtRate != null && !string.IsNullOrEmpty(unitDescription)) {
					txtRate.Text = rate.ToString("N2");
				}

				CheckBox cbxBillable = gvInvoiceLines.FooterRow.FindControl("cbxBillable") as CheckBox;
				if (cbxBillable != null)
					cbxBillable.Checked = billable.Equals("1");

				Label lblUnitDescription = gvInvoiceLines.FooterRow.FindControl("lblUnitDescription") as Label;
				if (lblUnitDescription != null)
					lblUnitDescription.Text = unitDescription;

				if (unitDescription.ToLower() == "sales tax") {
					WebNumericEditor txtQty = gvInvoiceLines.FooterRow.FindControl("txtQty") as WebNumericEditor;
					if (txtQty != null) {
						txtQty.Text = "1";
					}
				}

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
		protected LeadInvoiceDetail getInvoiceDetailLine() {
			decimal qty = 0;
			decimal rate = 0;
			decimal totalAmount = 0;
			string[] values = null;
			int serviceTypeID = 0;
			DateTime date = DateTime.MaxValue;

			LeadInvoiceDetail invoiceDetailLine = new LeadInvoiceDetail();

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
			Label lblUnitDescription = gvInvoiceLines.FooterRow.FindControl("lblUnitDescription") as Label;
			if (lblUnitDescription != null)
				invoiceDetailLine.UnitDescription = lblUnitDescription.Text;

			// rate
			WebNumericEditor txtRate = gvInvoiceLines.FooterRow.FindControl("txtRate") as WebNumericEditor;
			if (txtRate != null) {
				rate = txtRate.Value == null ? 0 : Convert.ToDecimal(txtRate.Value);

				invoiceDetailLine.Rate = rate;
			}

			// total amount
			// quantity
			TextBox txtLineAmount = gvInvoiceLines.FooterRow.FindControl("txtLineAmount") as TextBox;
			if (txtLineAmount != null) {
				decimal.TryParse(txtLineAmount.Text.Trim().Replace(",", ""), out totalAmount);
				invoiceDetailLine.LineAmount = totalAmount;
			}
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
			CRM.Data.Entities.LeadInvoiceDetail invoiceLine = null;
			TextBox txtLineAmount;

			switch (e.CommandName) {


				case "DoEdit":
					invoiceLineID = Convert.ToInt32(e.CommandArgument);

					invoiceLine = LeadInvoiceDetailManager.Get(invoiceLineID);

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
						if (txtRate != null && invoiceLine.InvoiceServiceType != null && invoiceLine.InvoiceServiceType.ServiceRate != null)
							txtRate.Text = invoiceLine.InvoiceServiceType.ServiceRate.ToString();

						// service unit
						Label lblUnitDescription = gvInvoiceLines.FooterRow.FindControl("lblUnitDescription") as Label;
						if (lblUnitDescription != null && invoiceLine.InvoiceServiceType != null && invoiceLine.InvoiceServiceType.InvoiceServiceUnit != null)
							lblUnitDescription.Text = invoiceLine.InvoiceServiceType.InvoiceServiceUnit.UnitDescription;

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

								LeadInvoiceDetailManager.Delete(invoiceLineID);

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
			string[] billToValues = null;
			int clientID = 0;
			int invoiceID = 0;
			int InvoiceLineID = 0;
			Data.Entities.LeadInvoice invoice = null;
			CRM.Data.Entities.LeadPolicy policy = null;
			LeadInvoiceDetail invoiceDetailLine = null;
			LeadInvoiceDetail invoiceDetail = null;

			int nextInvoiceNumber = 0;
			int policyID = 0;
			decimal taxAmount = 0;

			billToValues = ddlBillTo.SelectedValue.Split(new char[] { '|' });

			// get invoice id 
			invoiceID = Convert.ToInt32(ViewState["InvoiceID"].ToString());

			clientID = Core.SessionHelper.getClientId();

			// current policy being edited
			policyID = Session["policyID"] == null ? 0 : Convert.ToInt32(Session["policyID"]);
			policy = LeadPolicyManager.GetByID(policyID);



			if (invoiceID == 0) {
				invoice = new Data.Entities.LeadInvoice();

				// get id for current lead
				invoice.LeadId = Core.SessionHelper.getLeadId();

				invoice.isVoid = false;

				// assign client
				invoice.ClientID = clientID;

				// hide print button
				//btnPrint.Visible = false;
			}
			else {
				invoice = LeadInvoiceManager.Get(invoiceID);

				// show print button
				//btnPrint.Visible = true;

			}


			invoice.PolicyID = policy.PolicyType;

			invoice.InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
			invoice.DueDate = Convert.ToDateTime(txtDueDate.Text);
			invoice.BillToName = txtBillTo.Text.Trim();
			invoice.BillToAddress1 = txtBillToAddress1.Text.Trim();
			invoice.BillToAddress2 = txtBillToAddress2.Text.Trim();
			invoice.BillToAddress3 = txtBillToAddress3.Text.Trim();

			invoice.AdjusterID = policy.AdjusterID;

			invoice.AdjusterInvoiceNumber = txtReferenceNumber.Text.Trim();

			try {
				using (TransactionScope scope = new TransactionScope()) {
					if (invoiceID == 0) {
						// assign next invoice number to new invoice
						nextInvoiceNumber = LeadInvoiceManager.GetNextInvoiceNumber(clientID);

						invoice.InvoiceNumber = nextInvoiceNumber;
					}

					invoiceID = LeadInvoiceManager.Save(invoice);

					// save newly generated invoice id
					ViewState["InvoiceID"] = invoiceID.ToString();

					// check for add/edit invoice detail line
					InvoiceLineID = ViewState["InvoiceLineID"] == null ? 0 : Convert.ToInt32(ViewState["InvoiceLineID"]);

					if (InvoiceLineID > 0)
						invoiceDetail = LeadInvoiceDetailManager.Get(InvoiceLineID);
					else
						invoiceDetail = new LeadInvoiceDetail();


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
							LeadInvoiceDetailManager.Save(invoiceDetail);

							// clear
							ViewState["InvoiceLineID"] = "0";

							// update invoice total after adding 
							invoice = LeadInvoiceManager.Get(invoiceID);

							invoice.TotalAmount = invoice.LeadInvoiceDetail.Where(x => x.isBillable == true).Sum(x => x.LineAmount);

							taxAmount = (invoice.TotalAmount ?? 0) * ((invoice.TaxRate ?? 0) / 100);

							//invoice.TotalAmount = invoice.TotalAmount + taxAmount;

							LeadInvoiceManager.Save(invoice);

							// update comment
							updateInvoiceComment(invoice, invoiceDetail);
						}
					}
					// complete transaction
					scope.Complete();
				}

				// refresh invoice detail lines
				bindInvoiceDetails(invoiceID);

				clearFields();

				showToolbarButtons();
			}
			catch (Exception ex) {
				lblMessage.Text = "Error while saving invoice.";
				lblMessage.CssClass = "error";
				Core.EmailHelper.emailError(ex);
			}
		}

		protected void updateInvoiceComment(Data.Entities.LeadInvoice invoice, LeadInvoiceDetail invoiceDetail) {
			LeadComment comment = null;

			// retrieve comment by reference
			comment = LeadCommentManager.GetLeadCommentByReferenceId(invoiceDetail.InvoiceLineID);

			if (comment == null)
				comment = new LeadComment();

			comment.InsertBy = Core.SessionHelper.getUserId();
			comment.InsertDate = DateTime.Now;
			comment.LeadId = invoice.LeadId;
			comment.PolicyType = invoice.PolicyTypeID;
			comment.ReferenceID = invoiceDetail.InvoiceLineID;
			comment.Status = 1;	// active
			comment.UserId = Core.SessionHelper.getUserId();
			comment.CommentText = string.Format("<div>Invoice # {0} - {1:MM-dd-yyyy} for {2} Qty:{3:N2} Rate:{4:N2} Item Total:{5:N2}</div><div>{6}</div>",
				invoice.AdjusterInvoiceNumber,
				invoiceDetail.LineDate,
				invoiceDetail.LineDescription ?? "",
				invoiceDetail.Qty ?? 0,
				invoiceDetail.Rate ?? 0,
				invoiceDetail.LineAmount ?? 0,
				invoiceDetail.Comments ?? ""
				);

			LeadCommentManager.Save(comment);
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
			Data.Entities.LeadInvoice invoice = null;
			int port = 0;
			string password = null;
			CRM.Data.Entities.SecUser user = null;
			string subject = "Invoice";
			int userID = Core.SessionHelper.getUserId();
			string bodyText = "Invoiced is attached.";

			Page.Validate("emailInvoice");
			if (!Page.IsValid)
				return;

			try {
				invoiceID = Convert.ToInt32(ViewState["InvoiceID"].ToString());

				reportPath = generateInvoicePDF(invoiceID);

				user = SecUserManager.GetByUserId(userID);

				if (user != null && !string.IsNullOrEmpty(reportPath)) {
					invoice = LeadInvoiceManager.Get(invoiceID);

					string[] recipient = new string[] { txtEmailTo.Text };

					string[] attachments = new string[] { reportPath };

					port = Convert.ToInt32(user.emailHostPort);

					password = Core.SecurityManager.Decrypt(user.emailPassword);

					Core.EmailHelper.sendEmail(user.Email, recipient, null, subject, bodyText, attachments, user.emailHost, port, user.Email, password);

					updateDocumentLog(reportPath, (int)invoice.PolicyTypeID);

					lblMessage.Text = "Invoice mailed successfully.";
					lblMessage.CssClass = "ok";
				}
			}
			catch (Exception ex) {
				lblMessage.Text = "Unable to email invoice.";
				lblMessage.CssClass = "error";
			}
		}

		protected string generateInvoicePDF(int invoiceID) {
			string reportPath = null;

			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			invoiceReport = LeadInvoiceManager.GetInvoiceForReport(invoiceID);

			if (invoiceReport != null) {
				reportViewer.Reset();

				reportViewer.LocalReport.DataSources.Clear();

				reportViewer.LocalReport.EnableExternalImages = true;

				ReportDataSource reportDataSource = new ReportDataSource();

				reportDataSource.Name = "DataSet1";

				reportDataSource.Value = invoiceReport;

				reportViewer.LocalReport.DataSources.Add(reportDataSource);

				reportViewer.LocalReport.ReportPath = appPath + "/Content/Invoice.rdlc";

				reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subReportsEventHandler);

				reportPath = string.Format("{0}/Temp/Invoice_{1}.pdf", appPath, invoiceID);

				Core.ReportHelper.savePDFFromLocalReport(reportViewer.LocalReport, reportPath);
			}

			return reportPath;
		}

		protected void subReportsEventHandler(object sender, SubreportProcessingEventArgs e) {
			e.DataSources.Add(new ReportDataSource("DataSet1", invoiceReport[0].invoiceLines));
		}

		private void updateDocumentLog(string reportPath, int policyTypeID) {
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();
			string documentPath = null;

			int leadID = Core.SessionHelper.getLeadId();

			LeadsDocument objLeadDoc = new LeadsDocument();

			string ActualFileName = Path.GetFileName(reportPath);

			string FileNameWithoutExt = Path.GetFileNameWithoutExtension(reportPath);

			objLeadDoc.LeadId = leadID;

			objLeadDoc.Description = "Invoice";

			objLeadDoc.DocumentName = ActualFileName;
			objLeadDoc.Status = 1;
			objLeadDoc.IsPrint = false;

			objLeadDoc.InsertDate = DateTime.Now;

			objLeadDoc.PolicyTypeID = policyTypeID;

			objLeadDoc = LeadsUploadManager.SaveDocument(objLeadDoc);

			if (objLeadDoc.LeadDocumentId > 0) {
				documentPath = appPath + "/LeadsDocument/" + leadID + "/" + objLeadDoc.LeadDocumentId;

				if (!Directory.Exists(documentPath)) {
					Directory.CreateDirectory(documentPath);
				}

				File.Copy(reportPath, documentPath + "/" + ActualFileName, true);
			}
		}
	}
}
