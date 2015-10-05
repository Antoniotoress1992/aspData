using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqKit;
using CRM.Data;
using CRM.Data.Entities;


namespace CRM.Repository {
	public class RerportManager : IDisposable {

		private bool disposed = false;		// to detect redundant calls

        private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public RerportManager() {
            claimRulerDBContext = new CRMEntities();
		}

		public List<vw_ClaimsClosedListing> claimsClosedListing(int clientID) {
			List<vw_ClaimsClosedListing> reportData = null;

			reportData = (from x in claimRulerDBContext.vw_ClaimsClosedListing
					    where x.ClientID == clientID
					    select x
				    ).ToList();

			return reportData;
		}

		public List<vw_OpenClaimsListing> claimsOpenListing(int clientID) {
			List<vw_OpenClaimsListing> reportData = null;

			reportData = (from x in claimRulerDBContext.vw_OpenClaimsListing
					    where x.ClientID == clientID
					    select x
				    ).ToList();

			return reportData;
		}

        public List<vw_AdjusterPayoutSubReport> AdjusterPayoutSubReport(int clientID)
        {
            List<vw_AdjusterPayoutSubReport> reportData = null;

            reportData = (from x in claimRulerDBContext.vw_AdjusterPayoutSubReport
                          where x.ClientID == clientID
                          select x
                    ).ToList();

            return reportData;
        }

	   public List<vw_AdjusterPayout> AdjusterPayout(Expression<Func<vw_AdjusterPayout, bool>> predicate)
        {
            List<vw_AdjusterPayout> reportData = null;

		  reportData = (from x in claimRulerDBContext.vw_AdjusterPayout
			  .AsExpandable()
			  .Where(predicate)
					 select x					 
			  ).OrderBy(x => x.AdjusterId).ToList();


		  //reportData = (from x in claimRulerDBContext.vw_AdjusterPayout
		  //		    where x.ClientID == clientID
		  //		    select x
		  //	   ).ToList();

            return reportData;
        }

       /// <summary>
       /// Billed adjuster payroll detailed 
       /// </summary>
       /// <param name="predicate"></param>
       /// <returns></returns>
       public List<vw_AdjusterPayrollForAllBilledDetailed> AdjusterPayrollAllBilledDetailed(Expression<Func<vw_AdjusterPayrollForAllBilledDetailed, bool>> predicate)
       {
           List<vw_AdjusterPayrollForAllBilledDetailed> reportData = null;

           reportData = (from x in claimRulerDBContext.vw_AdjusterPayrollForAllBilledDetailed
               .AsExpandable()
               .Where(predicate)
                         select x
               ).OrderBy(x => x.AdjusterId).ToList();


           //reportData = (from x in claimRulerDBContext.vw_AdjusterPayout
           //		    where x.ClientID == clientID
           //		    select x
           //	   ).ToList();

           return reportData;
       }


       /// <summary>
       /// Billed adjuster payroll
       /// </summary>
       /// <param name="predicate"></param>
       /// <returns></returns>
       public List<vw_AdjusterPayrollForAllBilled> AdjusterPayrollAllBilled(Expression<Func<vw_AdjusterPayrollForAllBilled, bool>> predicate)
       {
           List<vw_AdjusterPayrollForAllBilled> reportData = null;

           reportData = (from x in claimRulerDBContext.vw_AdjusterPayrollForAllBilled
               .AsExpandable()
               .Where(predicate)
                         select x
               ).OrderBy(x => x.AdjusterId).ToList();


           //reportData = (from x in claimRulerDBContext.vw_AdjusterPayout
           //		    where x.ClientID == clientID
           //		    select x
           //	   ).ToList();

           return reportData;
       }

       /// <summary>
       /// Billed adjuster payroll detailed 
       /// </summary>
       /// <param name="predicate"></param>
       /// <returns></returns>
       public List<vw_AdjusterNames> AdjusterName()
       {
           List<vw_AdjusterNames> reportData = null;

           reportData = (from x in claimRulerDBContext.vw_AdjusterNames select x).ToList();

           //reportData = (from x in claimRulerDBContext.vw_AdjusterPayout
           //		    where x.ClientID == clientID
           //		    select x
           //	   ).ToList();

           return reportData;
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
