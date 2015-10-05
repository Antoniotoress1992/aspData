using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Entities;
using CRM.Data.Account;
using CRM.Web.Protected;

namespace CRM.Web.UserControl.Admin {
	public partial class ucInvoiceLedgerLegacy : System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {

		}

		public void bindData() {
			int leadID = Core.SessionHelper.getLeadId();

			List<CRM.Data.Entities.LeadInvoice> invoices = LeadInvoiceManager.GetInvoices(leadID).ToList();

			gvInvoices.DataSource = invoices;
			gvInvoices.DataBind();
		}

		protected void gvInvoices_RowDataBound(object sender, GridViewRowEventArgs e) {
			if (e.Row.RowType == DataControlRowType.DataRow) {
				ImageButton btnEdit = e.Row.FindControl("btnEdit") as ImageButton;
				Label lblTotalAmount = e.Row.FindControl("lblTotalAmount") as Label;
				decimal taxRate = 0;
				decimal taxAmount = 0;

				ImageButton btnPrint = e.Row.FindControl("btnPrint") as ImageButton;

                CRM.Data.Entities.LeadInvoice invoice = e.Row.DataItem as CRM.Data.Entities.LeadInvoice;

				if (invoice != null && invoice.LeadInvoiceDetail != null) {
					invoice.TotalAmount = invoice.LeadInvoiceDetail.Where(x => x.isBillable == true).Sum(x => x.LineAmount);

					taxRate = invoice.TaxRate ?? 0;
					taxAmount = (invoice.TotalAmount ?? 0) * (taxRate / 100);

					lblTotalAmount.Text = string.Format("{0:N2}", invoice.TotalAmount + taxAmount);


					btnEdit.PostBackUrl = string.Format("~/Protected/LeadInvoiceLegacy.aspx?id={0}", invoice.InvoiceID);

					btnPrint.Attributes["onclick"] = string.Format("javascript:return printInvoiceLegacy({0});", invoice.InvoiceID);
				}
			}
		}

		protected void gvInvoices_RowCommand(object sender, GridViewCommandEventArgs e) {
			int invoiceID = 0;
			CRM.Data.Entities.LeadInvoice invoice = null;

			if (e.CommandName == "DoDelete") {
				invoiceID = Convert.ToInt32(e.CommandArgument);

				invoice = LeadInvoiceManager.Get(invoiceID);

				if (invoice != null) {
					invoice.isVoid = true;

					LeadInvoiceManager.Save(invoice);

					// refresh list
					bindData();
				}
			}
		}

		protected void gvInvoices_Sorting(object sender, GridViewSortEventArgs e) {

		}
	}
}