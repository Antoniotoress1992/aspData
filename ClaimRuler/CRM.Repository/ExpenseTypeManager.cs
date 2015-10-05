using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	public class ExpenseTypeManager : IDisposable {
		private bool disposed = false;		// to detect redundant calls

        private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public ExpenseTypeManager() {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();
		}

		public ExpenseType Get(int id) {
			ExpenseType expense = null;

			expense = (from x in claimRulerDBContext.ExpenseType
					 where x.ExpenseTypeID == id
					 select x
				   ).FirstOrDefault();

			return expense;
		}

		public IQueryable<ExpenseType> GetAll(int clientID) 
        {
			IQueryable<ExpenseType> expenses = null;

			expenses = from x in claimRulerDBContext.ExpenseType
                       //join c in claimRulerDBContext.CarrierInvoiceProfileFeeItemized on x.ExpenseTypeID equals c.ExpenseTypeID
                       //join p in claimRulerDBContext.CarrierInvoiceProfile on c.CarrierInvoiceProfileID equals p.CarrierInvoiceProfileID
					 where 
                     //c.CarrierInvoiceProfileID == proID && x.IsActive == true 
                     x.ClientID == clientID && x.IsActive == true
					 orderby x.ExpenseName
					 select x;
				   

			return expenses;
		}
        public IQueryable<ExpenseType> GetAll2(int proID) // was int clientID
        {
            IQueryable<ExpenseType> expenses = null;

            expenses = from x in claimRulerDBContext.ExpenseType
                       join c in claimRulerDBContext.CarrierInvoiceProfileFeeItemized on x.ExpenseTypeID equals c.ExpenseTypeID
                       join p in claimRulerDBContext.CarrierInvoiceProfile on c.CarrierInvoiceProfileID equals p.CarrierInvoiceProfileID
                       where c.CarrierInvoiceProfileID == proID && x.IsActive == true //x.ClientID == clientID && x.IsActive == true
                       orderby x.ExpenseName
                       select x;


            return expenses;
        }

		public ExpenseType Save(ExpenseType expense) {

			if (expense.ExpenseTypeID == 0)
				claimRulerDBContext.ExpenseType.Add(expense);


			claimRulerDBContext.SaveChanges();

			return expense;
		}

		#region ===== memory management =====

		public void Dispose() {
			// Perform any object clean up here.

			// If you are inheriting from another class that
			// also implements IDisposable, don't forget to
			// call base.Dispose() as well.
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing) {
			if (!disposed) {
				if (disposing) {
					if (claimRulerDBContext != null) {

						claimRulerDBContext.Dispose();
					}
				}

				disposed = true;
			}
		}
		#endregion
	}
}
