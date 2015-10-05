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
	public partial class ucInvoiceLedger : System.Web.UI.UserControl {
		protected Protected.ClaimRuler masterPage = null;
		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;

			if (!IsPostBack) {
				bindData();
			}

		}

		private void bindData() {
			int claimID = Core.SessionHelper.getClaimID();

			List<Invoice> invoices = InvoiceManager.GetAll(claimID).ToList();

			gvInvoices.DataSource = invoices;
			
			gvInvoices.DataBind();
		}

		protected void gv_RowCommand(object sender, GridViewCommandEventArgs e) {
			int invoiceID = 0;
			Invoice invoice = null;

			if (e.CommandName == "DoDelete") {
				invoiceID = Convert.ToInt32(e.CommandArgument);

				invoice = InvoiceManager.Get(invoiceID);

				if (invoice != null) {
					invoice.IsVoid = true;

					InvoiceManager.Save(invoice);

					// refresh list
					bindData();
				}
			}
		}

		protected void gv_RowDataBound(object sender, GridViewRowEventArgs e) {
			decimal taxRate = 0;
			decimal taxAmount = 0;
			string encryptdID = null;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				ImageButton btnEdit = e.Row.FindControl("btnEdit") as ImageButton;
				Label lblTotalAmount = e.Row.FindControl("lblTotalAmount") as Label;
			
				ImageButton btnPrint = e.Row.FindControl("btnPrint") as ImageButton;
				
				Invoice invoice = e.Row.DataItem as Invoice;

				if (invoice != null && invoice.InvoiceDetail != null) {
					invoice.TotalAmount = invoice.InvoiceDetail.Where(x => x.isBillable == true).Sum(x => x.LineAmount);

					taxRate = invoice.TaxRate;
					taxAmount = (invoice.TotalAmount ?? 0) * (taxRate / 100);

					lblTotalAmount.Text = string.Format("{0:N2}", invoice.TotalAmount + taxAmount);

					encryptdID = Core.SecurityManager.EncryptQueryString(invoice.InvoiceID.ToString());

					btnEdit.PostBackUrl = string.Format("~/Protected/LeadInvoice.aspx?q={0}", encryptdID);

					btnPrint.Attributes["onclick"] = string.Format("javascript:return printInvoice('{0}');", encryptdID);
				}
			}
		}

		protected void gv_onSorting(object sender, GridViewSortEventArgs e) {
		}
	}
}