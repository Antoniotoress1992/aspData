using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Data.Entities
{
	public class InvoiceView {
		public int invoiceID { get; set; }
		public DateTime invoiceDate { get; set; }
		public DateTime dueDate { get; set; }
		public Nullable<decimal> totalAmount { get; set; }
		public string billTo { get; set; }
		public string billToAddress1 { get; set; }
		public string billToAddress2 { get; set; }
		public string billToAddress3 { get; set; }
		public string federalIDNo { get; set; }
		public string insurerFileNo { get; set; }
		public int invoiceNumber { get; set; }
		public DateTime claimDate { get; set; }
		public string claimNumber { get; set; }
		public string claimantName { get; set; }
		public string claimantAddress1 { get; set; }
		public string claimantAddress2 { get; set; }
		public string claimantAddress3 { get; set; }

		public string clientName { get; set; }
		public string clientAddress1 { get; set; }
		public string clientAddress2 { get; set; }
		public string clientAddress3 { get; set; }
		public string clientPhone { get; set; }
		public string clientFax { get; set; }
		public string clientEmail { get; set; }

		public string policyNumber { get; set; }
		public string policyType { get; set; }

		public string adjusterInvoiceNumber { get; set; }
		public string logoPath { get; set; }
        public string activity { get; set; }
		public decimal taxRate { get; set; }
		public List<InvoiceDetail> invoiceLines { get; set; }

		public List<LeadInvoiceDetail> legacyInvoiceLines { get; set; }
	}
}
