using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;

using LinqKit;
using System.Linq.Expressions;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class LeadInvoiceLedger : System.Web.UI.Page {
		int clientID = 0;
		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			clientID = SessionHelper.getClientId();

			if (!Page.IsPostBack) {
				bindData();
                tdsearch.Visible = false;
			}
		}

		private void bindData() {
            Expression<Func<Invoice, bool>> predicate = null;

            predicate = buildPredicate();

            IQueryable<Invoice> invoices = InvoiceManager.SearchLeadInvoiceLedger(predicate);

            gvInvoices.DataSource = invoices.ToList();

            gvInvoices.DataBind();
		}

		protected void btnSearch_Click(object sender, ImageClickEventArgs e) {
            bindData();
            tdsearch.Visible = false;
		}

		private Expression<Func<Invoice, bool>> buildPredicate() {
			Expression<Func<Invoice, bool>> predicate = null;

			clientID = SessionHelper.getClientId();

			// core search
			predicate = PredicateBuilder.True<CRM.Data.Entities.Invoice>();
			predicate = predicate.And(x => x.Claim.LeadPolicy.Leads.ClientID == clientID);			// claims for this client only
			predicate = predicate.And(x => x.IsVoid == false);
			predicate = predicate.And(x => x.IsApprove == true);

			// apply search filter


			if (!string.IsNullOrEmpty(this.txtDateFrom.Text)) {
				predicate = predicate.And(x => x.InvoiceDate >= txtDateFrom.Date);
			}

			if (!string.IsNullOrEmpty(this.txtDateTo.Text)) {
				predicate = predicate.And(x => x.InvoiceDate <= txtDateTo.Date);
			}

			return predicate;
		}

		protected void lbtnClear_Click(object sender, EventArgs e) {
			txtDateFrom.Value = null;
			txtDateTo.Value = null;
		}

		protected void gvInvoices_RowDataBound(object sender, GridViewRowEventArgs e) {
			string encryptdID = null;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				ImageButton btnEdit = e.Row.FindControl("btnEdit") as ImageButton;
				ImageButton btnPrint = e.Row.FindControl("btnPrint") as ImageButton;

				Invoice invoice = e.Row.DataItem as Invoice;

				encryptdID = Core.SecurityManager.EncryptQueryString(invoice.InvoiceID.ToString());

				btnEdit.PostBackUrl = string.Format("~/Protected/LeadInvoice.aspx?q={0}", encryptdID);

				btnPrint.Attributes["onclick"] = string.Format("javascript:return printInvoice('{0}');", encryptdID);

			}
		}

		protected void gvInvoices_RowCommand(object sender, GridViewCommandEventArgs e) {
			int invoiceID = 0;
			Invoice invoice = null;

			if (e.CommandName == "DoDelete") {
				try {
					invoiceID = Convert.ToInt32(e.CommandArgument);

					invoice = InvoiceManager.GetByID(invoiceID);

					invoice.IsVoid = true;

					InvoiceManager.Save(invoice);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
					lblMessage.Text = "Unable to delete invoice.";
					lblMessage.CssClass = "error";
				}

				// refresh grid
				btnSearch_Click(null, null);
			}
		}
       
        protected void lbtnClaim_Click(object sender, EventArgs e)
        {
            Claim claim = null;
            string url = "~/Protected/ClaimEdit.aspx";
            string claimNumber = ((LinkButton)(sender)).ValidationGroup;

            if (!string.IsNullOrEmpty(claimNumber))
            {
                claim = ClaimsManager.Get(claimNumber);

                if (claim != null)
                {
                    Session["LeadIds"] = claim.LeadPolicy.LeadId;
                    Session["policyID"] = claim.LeadPolicy.Id;
                    Session["ClaimID"] = claim.ClaimID;

                    Response.Redirect(url);
                }
            }
        }        

        protected void lbtrnSearchPanel_Click(object sender, EventArgs e)
        {
            tdsearch.Visible = true;
        }

	}
}