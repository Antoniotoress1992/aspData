using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using LinqKit;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;

using Microsoft.Reporting.WebForms;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class CarrierInvoiceApproval : System.Web.UI.Page {
		List<InvoiceView> invoiceReport = null;
		int clientID = 0;
		int roleID = 0;
		int userID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			clientID = Core.SessionHelper.getClientId();

			roleID = Core.SessionHelper.getUserRoleId();

			userID = Core.SessionHelper.getUserId();

			if (!Page.IsPostBack) {
				doBind();
			}
		}

		protected void doBind() {
			List<SecRoleInvoiceApprovalPermission> invoiceApprovalRules = null;
			IQueryable<Invoice> invoices = InvoiceManager.GetInvoicesForApproval(clientID);

			// load approval lmits for role
			invoiceApprovalRules = InvoiceApprovalRuleManager.GetAll(roleID);
			ViewState["InvoiceApprovalRules"] = invoiceApprovalRules;

			gvInvoice.DataSource = invoices.ToList();
			gvInvoice.DataBind();


		}


		protected void btnEmailInvoice_Click(object sender, EventArgs e) {
			int invoiceID = 0;
			string reportPath = null;
			Invoice invoice = null;
			CRM.Data.Entities.SecUser user = null;
			string[] recipient = null;
			string subject = "Invoice";
			string bodyText = "Attached is invoice.";
			string[] attachments = null;
			string password = null;
			int port = 0;

			user = SecUserManager.GetByUserId(userID);

			foreach (GridViewRow row in gvInvoice.Rows) {
				if (row.RowType == DataControlRowType.DataRow) {
					CheckBox cbxApproved = row.FindControl("cbxApproved") as CheckBox;

					if (cbxApproved.Checked) {
						invoiceID = (int)gvInvoice.DataKeys[row.RowIndex].Value;

						reportPath = generateInvoicePDF(invoiceID);

						if (!string.IsNullOrEmpty(reportPath)) {
							invoice = InvoiceManager.Get(invoiceID);

							recipient = new string[] { invoice.CarrierInvoiceProfile.AccountingContactEmail };

							attachments = new string[] { reportPath };

							port = Convert.ToInt32(user.emailHostPort);

							password = Core.SecurityManager.Decrypt(user.emailPassword);

							Core.EmailHelper.sendEmail(user.Email, recipient, null, subject, bodyText, attachments, user.emailHost, port, user.Email, password);
						}
					}
				}
			}
		}


		protected void gvInvoice_Sorting(object sender, GridViewSortEventArgs e) {
			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			ViewState["lastSortExpression"] = e.SortExpression;
			ViewState["lastSortDirection"] = descending;

			IQueryable<Invoice> invoices = InvoiceManager.GetInvoicesForApproval(clientID);

			gvInvoice.DataSource = invoices.orderByExtension(e.SortExpression, descending);
			gvInvoice.DataBind();
		}

		protected void gvInvoice_RowDataBound(object sender, GridViewRowEventArgs e) {
			Invoice invoice = null;
			List<SecRoleInvoiceApprovalPermission> invoiceApprovalRules = null;
			bool isAllowed = false;
			decimal lossAmount = 0;
			decimal grossLossPayable = 0;
			decimal netLossPayable = 0;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				// get invoice
				invoice = e.Row.DataItem as Data.Entities.Invoice;

				// get approval rules
				invoiceApprovalRules = (List<SecRoleInvoiceApprovalPermission>)ViewState["InvoiceApprovalRules"];

				if (invoice != null && invoice.Claim != null && invoiceApprovalRules != null && invoiceApprovalRules.Count > 0) {
					grossLossPayable = invoice.Claim.GrossLossPayable ?? 0;

					netLossPayable = invoice.Claim.NetClaimPayable ?? 0;

					// determine highest loss amount
					if (netLossPayable > lossAmount)
						lossAmount = netLossPayable;

					if (grossLossPayable > lossAmount)
						lossAmount = grossLossPayable;

					// verify invoice amount falls in amount range for this role
					foreach (SecRoleInvoiceApprovalPermission invoiceApprovalRule in invoiceApprovalRules) {
						if (lossAmount >= invoiceApprovalRule.AmountFrom && lossAmount <= invoiceApprovalRule.AmountTo) {
							isAllowed = true;
							break;
						}
					}

					CheckBox cbxApproved = e.Row.FindControl("cbxApproved") as CheckBox;

					cbxApproved.Enabled = isAllowed;
				}
			}
		}

		protected void gvInvoice_RowCommand(object sender, GridViewCommandEventArgs e) {
			int invoiceID = Convert.ToInt32(e.CommandArgument);
			Invoice invoice = null;
			Claim claim = null;

			if (e.CommandName == "DoVoid") {
				try {
					using (TransactionScope scope = new TransactionScope()) {
						invoice = InvoiceManager.Get(invoiceID);

						claim = ClaimsManager.Get(invoice.ClaimID);

						if (invoice != null && claim != null) {
							invoice.IsVoid = true;

							InvoiceManager.Save(invoice);

							claim.IsInvoiced = false;

							ClaimsManager.Save(claim);

							scope.Complete();
						}
					}
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
				finally {
					doBind();
				}
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

				reportViewer.LocalReport.ReportPath = appPath + "/Content/Invoice.rdlc";

				this.reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subReportsEventHandler);

				reportPath = string.Format("{0}/Temp/{1}.pdf", appPath, invoiceID);

				Core.ReportHelper.savePDFFromLocalReport(reportViewer.LocalReport, reportPath);
			}

			return reportPath;
		}
		protected void subReportsEventHandler(object sender, SubreportProcessingEventArgs e) {
			e.DataSources.Add(new ReportDataSource("DataSet1", invoiceReport[0].invoiceLines));
		}
	}
}