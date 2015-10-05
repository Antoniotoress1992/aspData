using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Account;
using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	public class ClaimExpenseManager : IDisposable {
		private bool disposed = false;		// to detect redundant calls

        private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;
		
		public ClaimExpenseManager() {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();
		}

		public void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			ClaimExpense expense = new ClaimExpense { ClaimExpenseID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("ClaimExpenses", expense);


			// Do the delete the category 
			DbContextHelper.DbContext.ClaimExpense.Remove(expense);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		public List<ClaimExpense> GetAll(int claimdID) {
			List<ClaimExpense> expenses = null;

			expenses = (from x in claimRulerDBContext.ClaimExpense
						  .Include("ExpenseType")
						  .Include("AdjusterMaster")
					  where x.ClaimID == claimdID
					  orderby x.ExpenseDate
					  select x
					  ).ToList<ClaimExpense>();

			return expenses;
		}

		public List<ClaimExpense> GetExpenseForInvoice(int claimdID) {
			List<ClaimExpense> expenses = null;

			expenses = (from x in claimRulerDBContext.ClaimExpense
						  .Include("ExpenseType")
					  where x.ClaimID == claimdID && x.Billed == false
					  orderby x.ExpenseDate
					  select x
					  ).ToList<ClaimExpense>();

			return expenses;
		}

		public ClaimExpense Get(int id) {
			ClaimExpense expense = null;

			expense = (from x in claimRulerDBContext.ClaimExpense
						 .Include("ExpenseType")
						  .Include("AdjusterMaster")
					  where x.ClaimExpenseID == id
					  select x
					  ).FirstOrDefault<ClaimExpense>();

			return expense;
		}

		public ClaimExpense Save(ClaimExpense expense) {
		
			if (expense.ClaimExpenseID == 0)
				claimRulerDBContext.ClaimExpense.Add(expense);

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
