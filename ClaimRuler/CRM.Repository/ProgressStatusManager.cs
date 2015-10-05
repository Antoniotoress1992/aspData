using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	public class ProgressStatusManager : IDisposable {
		private bool disposed = false;		// to detect redundant calls

		private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public ProgressStatusManager() {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();

		}

		public List<ProgressStatus> GetAll() {
			List<ProgressStatus> statuses = null;

			statuses = (from x in claimRulerDBContext.ProgressStatus
					  where x.IsActive == true
					  orderby x.SortOrder
					  select x
					  ).ToList<ProgressStatus>();

			return statuses;
		}
		public string GetDescription(int progressID) {
			string description = null;

			description = (from x in claimRulerDBContext.ProgressStatus
						where x.ProgressStatusID == progressID

						select x.ProgressDescription
					  ).FirstOrDefault();

			return description;
		}

		public List<ProgressStatisticsView> getProgressStatistics(int clientID, int userID) {
			List<ProgressStatisticsView> statistics = null;

			List<ProgressStatisticsView> claimstats = (from c in claimRulerDBContext.Claim
											   join p in claimRulerDBContext.LeadPolicy on c.PolicyID equals p.Id
											   join l in claimRulerDBContext.Leads on p.LeadId equals l.LeadId
											   where	l.ClientID == clientID && 
													l.UserId == userID &&
													c.ProgressStatusID != null && 
													c.IsActive == true && 
													l.Status == 1
											   group c by new { c.ProgressStatusID } into g
											   select new ProgressStatisticsView {
												   ProgressStatusID = (int)g.Key.ProgressStatusID,
												   claimCount = g.Count()
											   }).ToList();

			List<ProgressStatus> progressStatuses = (from x in claimRulerDBContext.ProgressStatus
											 where x.IsActive == true
											 orderby x.SortOrder descending
											 select x
					  ).ToList<ProgressStatus>();

			statistics = (from x in progressStatuses
					    join c in claimstats
					    on x.ProgressStatusID equals c.ProgressStatusID into left_join
					    from leftjoin in left_join.DefaultIfEmpty(new ProgressStatisticsView())	// include row from left side of join					   
					    select new ProgressStatisticsView {
						    ProgressStatusID = (int)x.ProgressStatusID,
						    ProgressDescription = x.ProgressDescription,
						    claimCount = leftjoin.claimCount
					    }).ToList();


			
			return statistics;
		}

		public List<ProgressStatisticsView> getProgressStatistics(int clientID) {
			List<ProgressStatisticsView> statistics = null;



            List<ProgressStatisticsView> claimstats = (from c in claimRulerDBContext.Claim
                                                       join p in claimRulerDBContext.LeadPolicy on c.PolicyID equals p.Id
                                                       join l in claimRulerDBContext.Leads on p.LeadId equals l.LeadId
                                                       where l.ClientID == clientID && c.ProgressStatusID != null && c.IsActive == true && l.Status == 1
                                                       group c by new { c.ProgressStatusID } into g
                                                       select new ProgressStatisticsView
                                                       {
                                                           ProgressStatusID = (int)g.Key.ProgressStatusID,
                                                           claimCount = g.Count()
                                                       }).ToList();




			List<ProgressStatus> progressStatuses = (from x in claimRulerDBContext.ProgressStatus
											 where x.IsActive == true	
											 orderby x.SortOrder descending
											 select x
					  ).ToList<ProgressStatus>();

			statistics = (from x in progressStatuses
					    join c in claimstats
					    on new {
						    x.ProgressStatusID,
					    }
					    equals new {
						    c.ProgressStatusID,
					    } into left_join
					    from leftjoin in left_join.DefaultIfEmpty(new ProgressStatisticsView())	// include row from left side of join					   
					    select new ProgressStatisticsView {
						    ProgressStatusID = (int)x.ProgressStatusID,
						    ProgressDescription = x.ProgressDescription,
						    claimCount = leftjoin.claimCount
					    }).ToList();


			//var statistics = (from prg in claimRulerDBContext.ProgressStatuses
			//			   join clm in
			//				   (
			//					   from c in claimRulerDBContext.Claims
			//					   join p in claimRulerDBContext.LeadPolicies on c.PolicyID equals p.Id
			//					   join l in claimRulerDBContext.Leads on p.LeadId equals l.LeadId
			//					   where l.ClientID == clientID && c.ProgressStatusID != null
			//					   //group c by new { l.ClientID, c.ProgressStatusID } into g
			//					   group c by new { c.ProgressStatusID } into g
			//					   select new {
			//						   //ClientID = clientID,
			//						   ProgressStatusID = (int)g.Key.ProgressStatusID,
			//						   claimCount = g.Count()
			//					   }
			//					)
			//			   on new {
			//				   //ClientID = clientID,
			//				   prg.ProgressStatusID,
			//				   IsActive = prg.IsActive
			//			   }
			//			   equals new {
			//				   //ClientID = clientID,
			//				   clm.ProgressStatusID,
			//				   IsActive = true
			//			   }
			//			   into claimstats_join
			//			   from stats in claimstats_join.DefaultIfEmpty(new ProgressStat())
			//			   where prg.IsActive == true
			//			   select new {
			//				   prg.ProgressDescription,
			//				   stats.claimCount

			//			   }).ToList();

			return statistics;//.OrderByDescending( x => x).ToList();
			//return statistics;
		}

        /// <summary>
        /// function used for chart on deshboard
        /// develop by chetu team
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="adjusterId"></param>
        /// <param name="carrierId"></param>
        /// <returns></returns>
        public List<ProgressStatisticsView> getProgressStatisticsFilter(int clientID, int userID, int adjusterId, int carrierId)
        {
            List<ProgressStatisticsView> statistics = null;
            List<ProgressStatisticsView> claimstats = null;
            //chetu code
            if (adjusterId != 0 && carrierId != 0)
            {

                claimstats = (from claim in claimRulerDBContext.Claim
                              join policy in claimRulerDBContext.LeadPolicy on claim.PolicyID equals policy.Id
                              join lead in claimRulerDBContext.Leads on policy.LeadId equals lead.LeadId
                              join adjuster in claimRulerDBContext.AdjusterMaster on claim.AdjusterID equals adjuster.AdjusterId
                              join carrier in claimRulerDBContext.Carrier on policy.CarrierID equals carrier.CarrierID

                              where lead.ClientID == clientID && lead.UserId == userID && claim.ProgressStatusID != null && claim.IsActive == true && lead.Status == 1
                              && adjuster.AdjusterId == adjusterId && carrier.CarrierID == carrierId
                              group claim by new { claim.ProgressStatusID } into g
                              select new ProgressStatisticsView
                              {
                                  ProgressStatusID = (int)g.Key.ProgressStatusID,
                                  claimCount = g.Count()
                              }).ToList();

            }
            else if (adjusterId != 0 && carrierId == 0)
            {
                claimstats = (from claim in claimRulerDBContext.Claim
                              join policy in claimRulerDBContext.LeadPolicy on claim.PolicyID equals policy.Id
                              join lead in claimRulerDBContext.Leads on policy.LeadId equals lead.LeadId
                              join adjuster in claimRulerDBContext.AdjusterMaster on claim.AdjusterID equals adjuster.AdjusterId
                              where lead.ClientID == clientID && lead.UserId == userID && claim.ProgressStatusID != null && claim.IsActive == true && lead.Status == 1
                              && adjuster.AdjusterId == adjusterId
                              group claim by new { claim.ProgressStatusID } into g
                              select new ProgressStatisticsView
                              {
                                  ProgressStatusID = (int)g.Key.ProgressStatusID,
                                  claimCount = g.Count()
                              }).ToList();

            }
            else if (adjusterId == 0 && carrierId != 0)
            {
                claimstats = (from claim in claimRulerDBContext.Claim
                              join policy in claimRulerDBContext.LeadPolicy on claim.PolicyID equals policy.Id
                              join lead in claimRulerDBContext.Leads on policy.LeadId equals lead.LeadId
                              join carrier in claimRulerDBContext.Carrier on policy.CarrierID equals carrier.CarrierID
                              where lead.ClientID == clientID && lead.UserId == userID && claim.ProgressStatusID != null && claim.IsActive == true && lead.Status == 1
                              && carrier.CarrierID == carrierId
                              group claim by new { claim.ProgressStatusID } into g
                              select new ProgressStatisticsView
                              {
                                  ProgressStatusID = (int)g.Key.ProgressStatusID,
                                  claimCount = g.Count()
                              }).ToList();

            }
            else
            {
                claimstats = (from claim in claimRulerDBContext.Claim
                              join policy in claimRulerDBContext.LeadPolicy on claim.PolicyID equals policy.Id
                              join lead in claimRulerDBContext.Leads on policy.LeadId equals lead.LeadId

                              where lead.ClientID == clientID && lead.UserId == userID && claim.ProgressStatusID != null && claim.IsActive == true && lead.Status == 1
                              group claim by new { claim.ProgressStatusID } into g
                              select new ProgressStatisticsView
                              {
                                  ProgressStatusID = (int)g.Key.ProgressStatusID,
                                  claimCount = g.Count()
                              }).ToList();

            }
            //chetu code           

            List<ProgressStatus> progressStatuses = (from x in claimRulerDBContext.ProgressStatus
                                                     where x.IsActive == true
                                                     orderby x.SortOrder descending
                                                     select x
                      ).ToList<ProgressStatus>();

            statistics = (from x in progressStatuses
                          join c in claimstats
                          on x.ProgressStatusID equals c.ProgressStatusID into left_join
                          from leftjoin in left_join.DefaultIfEmpty(new ProgressStatisticsView())	// include row from left side of join					   
                          select new ProgressStatisticsView
                          {
                              ProgressStatusID = (int)x.ProgressStatusID,
                              ProgressDescription = x.ProgressDescription,
                              claimCount = leftjoin.claimCount
                          }).ToList();



            return statistics;
        }


        /// <summary>
        /// function used for chart on deshboard
        /// develop by chetu team
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="adjusterId"></param>
        /// <param name="carrierId"></param>
        /// <returns></returns>
        public List<ProgressStatisticsView> getProgressStatisticsFilter(int clientID, int adjusterId,int carrierId)
        {
            List<ProgressStatisticsView> statistics = null;
            List<ProgressStatisticsView> claimstats = null;
            /*--- Chetu Code ----*/
            if (adjusterId != 0 && carrierId != 0)
            {
               
                 claimstats = (from claim in claimRulerDBContext.Claim
                                                           join policy in claimRulerDBContext.LeadPolicy on claim.PolicyID equals policy.Id
                                                           join lead in claimRulerDBContext.Leads on policy.LeadId equals lead.LeadId
                                                           join adjuster in claimRulerDBContext.AdjusterMaster on claim.AdjusterID equals adjuster.AdjusterId
                                                           join carrier in claimRulerDBContext.Carrier on policy.CarrierID equals carrier.CarrierID
                                                           where lead.ClientID == clientID && claim.ProgressStatusID != null && claim.IsActive == true && lead.Status == 1
                                                           && adjuster.AdjusterId == adjusterId && carrier.CarrierID == carrierId
                                                           group claim by new { claim.ProgressStatusID } into g
                                                           select new ProgressStatisticsView
                                                           {
                                                               ProgressStatusID = (int)g.Key.ProgressStatusID,
                                                               claimCount = g.Count()
                                                           }).ToList();
               
            }
            else if (adjusterId != 0 && carrierId == 0)
            {
                 claimstats = (from claim in claimRulerDBContext.Claim
                                                           join policy in claimRulerDBContext.LeadPolicy on claim.PolicyID equals policy.Id
                                                           join lead in claimRulerDBContext.Leads on policy.LeadId equals lead.LeadId
                                                           join adjuster in claimRulerDBContext.AdjusterMaster on claim.AdjusterID equals adjuster.AdjusterId
                                                           where lead.ClientID == clientID && claim.ProgressStatusID != null && claim.IsActive == true && lead.Status == 1 && adjuster.AdjusterId ==  adjusterId
                                                           group claim by new { claim.ProgressStatusID } into g
                                                           select new ProgressStatisticsView
                                                           {
                                                               ProgressStatusID = (int)g.Key.ProgressStatusID,
                                                               claimCount = g.Count()
                                                           }).ToList();
               
            }
            else if (adjusterId == 0 && carrierId != 0)
            {
                 claimstats = (from claim in claimRulerDBContext.Claim
                                                           join policy in claimRulerDBContext.LeadPolicy on claim.PolicyID equals policy.Id
                                                           join lead in claimRulerDBContext.Leads on policy.LeadId equals lead.LeadId
                                                           join carrier in claimRulerDBContext.Carrier on policy.CarrierID equals carrier.CarrierID
                                                           where lead.ClientID == clientID && claim.ProgressStatusID != null && claim.IsActive == true && lead.Status == 1 && carrier.CarrierID == carrierId
                                                           group claim by new { claim.ProgressStatusID } into g
                                                           select new ProgressStatisticsView
                                                           {
                                                               ProgressStatusID = (int)g.Key.ProgressStatusID,
                                                               claimCount = g.Count()
                                                           }).ToList();
               
            }
            else
            {
                claimstats = (                             from claim in claimRulerDBContext.Claim
                                                           join policy in claimRulerDBContext.LeadPolicy on claim.PolicyID equals policy.Id
                                                           join lead in claimRulerDBContext.Leads on policy.LeadId equals lead.LeadId  
                                                           where lead.ClientID == clientID && claim.ProgressStatusID != null && claim.IsActive == true && lead.Status == 1 
                                                           group claim by new { claim.ProgressStatusID } into g
                                                           select new ProgressStatisticsView
                                                           {
                                                               ProgressStatusID = (int)g.Key.ProgressStatusID,
                                                               claimCount = g.Count()
                                                           }).ToList();
               
            }

            /*--- Chetu Code ----*/
           


            List<ProgressStatus> progressStatuses = (from x in claimRulerDBContext.ProgressStatus
                                                     where x.IsActive == true
                                                     orderby x.SortOrder descending
                                                     select x
                      ).ToList<ProgressStatus>();

            statistics = (from x in progressStatuses
                          join c in claimstats
                          on new
                          {
                              x.ProgressStatusID,
                          }
                          equals new
                          {
                              c.ProgressStatusID,
                          } into left_join
                          from leftjoin in left_join.DefaultIfEmpty(new ProgressStatisticsView())	// include row from left side of join					   
                          select new ProgressStatisticsView
                          {
                              ProgressStatusID = (int)x.ProgressStatusID,
                              ProgressDescription = x.ProgressDescription,
                              claimCount = leftjoin.claimCount
                          }).ToList();


            

            return statistics;//.OrderByDescending( x => x).ToList();
            //return statistics;
        }


        /// <summary>
        /// function used for get carrier data
        /// develop by chetu team
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="adjusterId"></param>
        /// <param name="carrierId"></param>
        /// <returns></returns>
        /// 
        //modified 9/8/14 -OC; added clientID paramter to filter the dropdown to only show carriers for theat portal
        public List<Carrier> GetCarrierData(int clientID)
        {
            List<Carrier> lstCarrier = null;
            lstCarrier = (from c in claimRulerDBContext.Carrier.ToList()
                          where c.ClientID == clientID
                          select new Carrier
                          {
                              CarrierID = c.CarrierID,
                              CarrierName = c.CarrierName,
                          }).ToList();
            return lstCarrier;
        }
        /// <summary>
        /// function used for get adjuster data
        /// develop by chetu team
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="adjusterId"></param>
        /// <param name="carrierId"></param>
        /// <returns></returns>
        public List<AdjusterMaster> GetAdjsuterData(int clientID)
        {
            List<AdjusterMaster> lstAdjuster = null;
           
            lstAdjuster = (from c in claimRulerDBContext.AdjusterMaster.ToList()
                           where c.ClientId == clientID
                          select new AdjusterMaster
                          {
                              AdjusterId=c.AdjusterId,
                              AdjusterName=c.AdjusterName
                          }).ToList();
            return lstAdjuster;
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
