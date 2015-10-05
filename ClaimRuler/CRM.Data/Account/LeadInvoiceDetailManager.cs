using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class LeadInvoiceDetailManager {
		static public void Delete(int id) {
			LeadInvoiceDetail invoiceDetail = null;

			invoiceDetail = Get(id);
			if (invoiceDetail != null) {
				DbContextHelper.DbContext.DeleteObject(invoiceDetail);

				DbContextHelper.DbContext.SaveChanges();
			}
		}

		static public int Save(LeadInvoiceDetail invoiceDetail) {
			if (invoiceDetail.InvoiceLineID == 0)
				DbContextHelper.DbContext.Add(invoiceDetail);

			DbContextHelper.DbContext.SaveChanges();

			return invoiceDetail.InvoiceLineID;
		}

		static public List<LeadInvoiceDetail> GetInvoiceDetails(int invoiceID) {
			List<LeadInvoiceDetail> invoiceDetails = null;

			invoiceDetails = (from x in DbContextHelper.DbContext.LeadInvoiceDetail
								.Include("InvoiceServiceType")
								.Include("InvoiceServiceType.InvoiceServiceUnit")
						   where x.InvoiceID == invoiceID
						   select x).ToList<LeadInvoiceDetail>();

			return invoiceDetails;
		}

		static public LeadInvoiceDetail Get(int id) {
			LeadInvoiceDetail invoiceDetail = null;

			invoiceDetail = (from x in DbContextHelper.DbContext.LeadInvoiceDetail
							  .Include("InvoiceServiceType")
							  .Include("InvoiceServiceType.InvoiceServiceUnit")
						  where x.InvoiceLineID == id
						  select x).FirstOrDefault<LeadInvoiceDetail>();

			return invoiceDetail;
		}
	}
}
