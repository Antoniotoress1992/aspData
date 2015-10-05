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
	public partial class InvoiceLedger : System.Web.UI.Page {
		int clientID = 0;
		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			clientID = SessionHelper.getClientId();

			if (!Page.IsPostBack) {
				bindData();
			}
		}

		private void bindData() {					
		}

		protected void btnSearch_Click(object sender, ImageClickEventArgs e) {
			Expression<Func<Invoice, bool>> predicate = null;

			predicate = buildPredicate();

			IQueryable<Invoice> invoices = InvoiceManager.Search(predicate);

			gvInvoices.DataSource = invoices.ToList();

			gvInvoices.DataBind();
		}

		private Expression<Func<Invoice, bool>> buildPredicate() {
			Expression<Func<Invoice, bool>> predicate = null;

			clientID = SessionHelper.getClientId();

			// core search
			predicate = PredicateBuilder.True<CRM.Data.Entities.Invoice>();
			predicate = predicate.And(x => x.Claim.LeadPolicy.Leads.ClientID == clientID);			// claims for this client only
			predicate = predicate.And(x => x.IsVoid == false);


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

	}
}