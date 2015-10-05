using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class Accounting : System.Web.UI.Page {
		decimal totalCommission = 0;
		decimal totalExpenses = 0;
		decimal totalNetAdjuster = 0;
		decimal totalInvoice = 0;

		//public class Ledger {
		//	public string cms { get; set; }
		//	public int invoiceNumber { get; set; }
		//	public DateTime date { get; set; }
		//	public int leadID { get; set; }
		//	public DateTime clientPaidDate { get; set; }
		//	public int adjusterID { get; set; }
		//	public DateTime adjusterPaidDate { get; set; }
		//	public decimal totalCommision { get; set; }
		//	public decimal totalExpenses { get; set; }
		//	public decimal adjusterNetAmount { get; set; }
		//	public decimal invoiceTotal { get; set; }

		//	public Ledger();
		//	public Ledger(string cms, int invoiceNumber, DateTime date, int leadID, DateTime clientPaidDate, int adjusterID, DateTime adjusterPaidDatem,
		//		decimal totalCommision, decimal totalExpenses, decimal adjusterNetAmount, decimal invoiceTotal);
		//}
		protected void Page_Load(object sender, EventArgs e) {
			List<Ledger> ledger = LedgerManager.GetAll();

			gvInvoices.DataSource = ledger;
			gvInvoices.DataBind();
		}

		protected void gv_RowDataBound(object sender, GridViewRowEventArgs e) {
			if (e.Row.RowType == DataControlRowType.Footer) {

				Label lblCommissionTotal = e.Row.FindControl("lblCommissionTotal") as Label;
				lblCommissionTotal.Text = string.Format("{0:N2}", totalCommission);

				Label lblTotalExpenses = e.Row.FindControl("lblTotalExpenses") as Label;
				lblTotalExpenses.Text = string.Format("{0:N2}", totalExpenses);

				Label lblAdjusterNet = e.Row.FindControl("lblAdjusterNet") as Label;
				lblAdjusterNet.Text = string.Format("{0:N2}", totalNetAdjuster);

				Label lblInvoiceTotal = e.Row.FindControl("lblInvoiceTotal") as Label;
				lblInvoiceTotal.Text = string.Format("{0:N2}", totalInvoice);

			}
			else if (e.Row.RowType == DataControlRowType.DataRow) {
				ImageButton btnEdit = e.Row.FindControl("btnEdit") as ImageButton;
				Label lblTotalAmount = e.Row.FindControl("lblTotalAmount") as Label;
				List<CRM.Data.Entities.LeadPolicy> policies = null;
				string claimNumber = null;
				int policyTypeID = 0;

				ImageButton btnPrint = e.Row.FindControl("btnPrint") as ImageButton;

				Ledger ledger = e.Row.DataItem as Ledger;

				totalCommission += ledger.CommissionTotal ?? 0;
				totalExpenses += ledger.TotalExpenses ?? 0;
				totalNetAdjuster += ledger.AdjusterNet ?? 0;
				totalInvoice += ledger.InvoiceTotal ?? 0;

				//adjusterNet = (ledger.LeadInvoice.TotalAmount ?? 0 ) - (ledger.TotalExpenses ?? 0);

				//Label lblAdjusterNet = e.Row.FindControl("lblAdjusterNet") as Label;
				//lblAdjusterNet.Text = string.Format("{0:N2}", adjusterNet);

				//HyperLink hlink = e.Row.FindControl("lnlEditAdjuster") as HyperLink;
				//hlink.NavigateUrl = "~/Protected/Admin/AdjusterEdit.aspx?id=" + ledger.AdjusterID.ToString();

				policies = LeadPolicyManager.GetPolicies((int)ledger.LeadInvoice.LeadId);
				if (policies != null) {
					policyTypeID = ledger.LeadInvoice.PolicyTypeID ?? 0;
					claimNumber = (from x in policies
								where x.PolicyType == policyTypeID
								select x.ClaimNumber).FirstOrDefault();

					if (claimNumber != null) {
						Label lblClaimNumber = e.Row.FindControl("lblClaimNumber") as Label;
						lblClaimNumber.Text = claimNumber;
					}

				}

				if (btnEdit != null && ledger != null) {
					//btnEdit.Attributes["onclick"] = string.Format("javascript:return editInvoice({0});", invoice.InvoiceID);
					btnEdit.PostBackUrl = string.Format("~/Protected/Admin/LeadInvoice.aspx?id={0}", ledger.InvoiceID);
				}

				if (btnPrint != null && ledger != null) {
					btnPrint.Attributes["onclick"] = string.Format("javascript:return printInvoice({0});", ledger.InvoiceID);
				}
			}
		}
		
		protected void gv_RowCommand(object sender, GridViewCommandEventArgs e) {		
		
		}
	}
}