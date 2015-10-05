using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Repository {
	public class ClaimServiceManager : IDisposable {

		private bool disposed = false;		// to detect redundant calls

        private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public ClaimServiceManager() {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();
		}

		public void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			ClaimService claimService = new ClaimService { ClaimServiceID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("ClaimServices", claimService);


			// Do the delete the category 
			DbContextHelper.DbContext.ClaimService.Remove(claimService);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		public List<ClaimService> GetAll(int claimID) 
        {
			List<ClaimService> services = null;

			services = (from x in claimRulerDBContext.ClaimService
					  .Include("AdjusterMaster")
					  .Include("InvoiceServiceType")
					  .Include("SecUser")
					  
					  where x.ClaimID == claimID
					  select x
					).ToList<ClaimService>();

			return services;
		}

        public List<ClaimService> GetAllForInvoice(int claimID)
        {
            List<ClaimService> services = null;

            services = (from x in claimRulerDBContext.ClaimService
                      .Include("AdjusterMaster")
                      .Include("InvoiceServiceType")
                      .Include("SecUser")

                        where x.ClaimID == claimID && x.Billed == false
                        
                        select x
                    ).ToList<ClaimService>();

            return services;
        }

		public ClaimService Get(int id) {
			ClaimService service = null;

			service = (from x in claimRulerDBContext.ClaimService
					  .Include("AdjusterMaster")
					  .Include("InvoiceServiceType")
					  .Include("SecUser")
					  where x.ClaimServiceID == id
					  select x
					).FirstOrDefault<ClaimService>();

			return service;
		}

		public ClaimService Save(ClaimService claimService) {
			if (claimService.ClaimServiceID == 0)
				claimRulerDBContext.ClaimService.Add(claimService);

			claimRulerDBContext.SaveChanges();

			return claimService;
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
