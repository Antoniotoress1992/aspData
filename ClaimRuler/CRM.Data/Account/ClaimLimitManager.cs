using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class ClaimLimitManager {
		public static List<vw_ClaimLimit> GetAll(int claimID, int limitType) 
        {
			List<vw_ClaimLimit> limits = null;

			limits = (from x in DbContextHelper.DbContext.vw_ClaimLimit
					where x.ClaimID == claimID && x.LimitType == limitType
					orderby x.ClaimLimitID
					select x).Take(4).ToList<vw_ClaimLimit>();

			return limits;
		}

        public static ClaimLimit GetLatest()
        {
            ClaimLimit Claimlimits = null;

            Claimlimits = (from x in DbContextHelper.DbContext.ClaimLimit
                      //where x.LimitType == limitType && x.IsStatic == true
                      orderby x.LimitID descending
                      select x
                ).FirstOrDefault();

            return Claimlimits;

        }
        
        public static ClaimLimit GetLatest2(int PolicyLimitID)//get the claimLimiID when the user enters data in the new fields and "add new" is not selected
        {
            ClaimLimit Claimlimits = null;

            Claimlimits = (from x in DbContextHelper.DbContext.ClaimLimit
                           where x.PolicyLimitID == PolicyLimitID
                           orderby x.LimitID descending
                           select x
                ).FirstOrDefault();

            return Claimlimits;

        }
        public static List<vw_ClaimLimit> GetAll2(int claimID, int limitType)
        {
            List<vw_ClaimLimit> limits = null;

            limits = (from x in DbContextHelper.DbContext.vw_ClaimLimit
                      where x.ClaimID == claimID && x.LimitType == limitType && x.IsDeletedClaim == false && x.IsDeletedPolicy == false
                      orderby x.ClaimLimitID
                      select x).Take(4).ToList<vw_ClaimLimit>();

            return limits;
        }
		public static List<vw_ClaimLimit> GetAll(int limitType) {
			List<vw_ClaimLimit> limits = null;

			limits = (from x in DbContextHelper.DbContext.vw_ClaimLimit
					where x.LimitType == limitType
					orderby x.ClaimLimitID
					select x).Take(4).ToList<vw_ClaimLimit>();

			return limits;
		}

		public static ClaimLimit Get(int id) {
			ClaimLimit limit = null;

			limit = (from x in DbContextHelper.DbContext.ClaimLimit
					where x.ClaimLimitID == id
					select x).FirstOrDefault<ClaimLimit>();

			return limit;
		}


		public static ClaimLimit Save(ClaimLimit limit) {
			if (limit.ClaimLimitID == 0) {
				DbContextHelper.DbContext.Add(limit);
			}

			DbContextHelper.DbContext.SaveChanges();

			return limit;
		}

        public static void IsDeleted(int claimId)
        {
            List<ClaimLimit> claimLimit = (from s in DbContextHelper.DbContext.ClaimLimit
                              where s.ClaimID == claimId
                        select s).ToList();

            foreach (var limit in claimLimit)
            {
                DbContextHelper.DbContext.DeleteObject(limit);
                DbContextHelper.DbContext.SaveChanges();
            }            
        }


        public static bool IsStaticTrue(int claimId)
        {
            bool isTemplate = false;
            ClaimLimit claimLimit = (from s in DbContextHelper.DbContext.ClaimLimit
                                           where s.ClaimID == claimId
                                           select s).FirstOrDefault();

            if (claimLimit!=null)
            {

                Limit Limit = (from s in DbContextHelper.DbContext.Limit
                               where s.LimitID == claimLimit.LimitID
                                         select s).FirstOrDefault();
                if (Limit!=null)
                {
                    isTemplate = Limit.IsStatic ?? false;

                }

            }

            return isTemplate;
        }

        public static bool PolicyIsStaticTrue(int policyID)
        {
            bool isTemplate = false;
            PolicyLimit policyLimit = (from s in DbContextHelper.DbContext.PolicyLimit
                                       where s.PolicyID == policyID
                                     select s).FirstOrDefault();

            if (policyLimit != null)
            {

                Limit Limit = (from s in DbContextHelper.DbContext.Limit
                               where s.LimitID == policyLimit.LimitID
                               select s).FirstOrDefault();
                if (Limit != null)
                {
                    isTemplate = Limit.IsStatic ?? false;

                }

            }

            return isTemplate;
        }


        public static void EditModeDeleteClaimLimit(int limitID,int claimID)
        {
            List<ClaimLimit> limit = (from s in DbContextHelper.DbContext.ClaimLimit
                                       where s.LimitID == limitID && s.ClaimID==claimID
                                       select s).ToList();

            foreach (var data in limit)
            {
                DbContextHelper.DbContext.DeleteObject(data);
                DbContextHelper.DbContext.SaveChanges();
            }
        }
	}
}
