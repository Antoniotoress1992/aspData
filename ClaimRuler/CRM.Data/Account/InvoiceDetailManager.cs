using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class InvoiceDetailManager {
		static public void Delete(int id) {
			InvoiceDetail invoiceDetail = null;

			invoiceDetail = Get(id);
			if (invoiceDetail != null) {
				DbContextHelper.DbContext.DeleteObject(invoiceDetail);

				DbContextHelper.DbContext.SaveChanges();
			}
		}

		static public int Save(InvoiceDetail invoiceDetail) {
			if (invoiceDetail.InvoiceLineID == 0)
				DbContextHelper.DbContext.Add(invoiceDetail);

			DbContextHelper.DbContext.SaveChanges();

			return invoiceDetail.InvoiceLineID;
		}

		static public List<InvoiceDetail> GetInvoiceDetails(int invoiceID) {
			List<InvoiceDetail> invoiceDetails = null;

			invoiceDetails = (from x in DbContextHelper.DbContext.InvoiceDetail
								.Include("InvoiceServiceType")
								.Include("InvoiceServiceType.InvoiceServiceUnit")
						   where x.InvoiceID == invoiceID
						   select x).ToList<InvoiceDetail>();

			return invoiceDetails;
		}

		/// <summary>
		/// Returns a collection of unique ExpenseTypeID present in an invoice. 
		/// </summary>
		/// <param name="invoiceID"></param>
		/// <returns></returns>
		static public List<int> GetInvoiceExpenseTypeIDCollection(int invoiceID) {
			List<int> invoiceExpenseTypeIDCollection = null;

			invoiceExpenseTypeIDCollection = (from x in DbContextHelper.DbContext.InvoiceDetail
						   where x.InvoiceID == invoiceID && x.ExpenseTypeID != null
						   group x by x.ExpenseTypeID into g
						   select (int)g.Key				  
						   ).ToList<int>();

			return invoiceExpenseTypeIDCollection;
		}
        static public decimal getTotalInvoice(int claimId) {

            decimal total = 0;

            var priceQuery =
               from prod in DbContextHelper.DbContext.Invoice

               where prod.ClaimID == claimId
               group prod by prod.ClaimID into grouping
               select new
               {
                   grouping.Key,
                   TotalPrice = grouping.Sum(p => p.TotalAmount)
               };

            foreach (var grp in priceQuery)
            {

                total = Convert.ToDecimal(grp.TotalPrice);
            }



            return total;
        }

        public static decimal getTotalMiles(int claimId, int clientID)
        {
            decimal total = 0;

            var priceQuery =
               from prod in DbContextHelper.DbContext.ClaimExpense
               join ex in DbContextHelper.DbContext.ExpenseType on prod.ExpenseTypeID equals ex.ExpenseTypeID
               where prod.ClaimID == claimId && ex.ClientID == clientID
               group prod by prod.ClaimID into grouping
               select new
               {
                   grouping.Key,
                   TotalPrice = grouping.Sum(p => p.ExpenseAmount)
               };

            foreach (var grp in priceQuery)
            {

                total = Convert.ToDecimal(grp.TotalPrice);
            }



            return total;

        }
		static public InvoiceDetail Get(int id) {
			InvoiceDetail invoiceDetail = null;

			invoiceDetail = (from x in DbContextHelper.DbContext.InvoiceDetail
							  .Include("InvoiceServiceType")
							  .Include("InvoiceServiceType.InvoiceServiceUnit")
						  where x.InvoiceLineID == id
						  select x).FirstOrDefault<InvoiceDetail>();

			return invoiceDetail;
		}
	}
}
