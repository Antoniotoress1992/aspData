using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class LimitManager {
		public static List<Limit> GetAll(int limitType) {
			List<Limit> limits = null;

			limits = (from x in DbContextHelper.DbContext.Limit
					where x.LimitType == limitType && x.IsStatic==true
					select x
				).ToList<Limit>();

			return limits;

		}

        public static Limit GetLatest()
        {
            Limit limits = null;

            limits = (from x in DbContextHelper.DbContext.Limit
                      //where x.LimitType == limitType && x.IsStatic == true
                      orderby x.LimitID descending
                      select x
                ).FirstOrDefault();

            return limits;

        }

        public static Limit Save(Limit limit)
        {            
            DbContextHelper.DbContext.Add(limit);  
            DbContextHelper.DbContext.SaveChanges();
            return limit;
        }

        public static void DeleteLimit(int claimId,int policyId)
        {
            List<ClaimLimit> objclaimLimit = (from s in DbContextHelper.DbContext.ClaimLimit
                                           where s.ClaimID == claimId
                                           select s).ToList();

            foreach (var claimlimit in objclaimLimit)
            {

                ClaimLimitManager.IsDeleted(claimId);
                PolicyLimitManager.IsDeleted(policyId);
              
                Limit objLimit = (from s in DbContextHelper.DbContext.Limit
                                        where s.LimitID == claimlimit.LimitID && s.IsStatic==false
                                                  select s).FirstOrDefault();

               if (objLimit != null )
               {
                   DbContextHelper.DbContext.DeleteObject(objLimit);
                   DbContextHelper.DbContext.SaveChanges();
               }
            }
        }


        public static void DeletePolicyLimit(int policyId)
        {
            List<PolicyLimit> objPolicyLimit = (from s in DbContextHelper.DbContext.PolicyLimit
                                                where s.PolicyID == policyId
                                              select s).ToList();

            foreach (var policylimit in objPolicyLimit)
            {
                PolicyLimitManager.IsDeleted(policyId);
                Limit objLimit = (from s in DbContextHelper.DbContext.Limit
                                  where s.LimitID == policylimit.LimitID && s.IsStatic == false
                                  select s).FirstOrDefault();

                if (objLimit != null)
                {
                    DbContextHelper.DbContext.DeleteObject(objLimit);
                    DbContextHelper.DbContext.SaveChanges();
                }
                
            }
        }



        public static List<Limit> GetAllLimit(bool isStatic)
        {
            List<Limit> limits = null;

            limits = (from x in DbContextHelper.DbContext.Limit
                      where x.IsStatic == isStatic
                      select x
                ).ToList<Limit>();

            return limits;

        }

        public static List<Limit> GetLimit(int limitID)
        {
            List<Limit> limits = null;

            limits = (from x in DbContextHelper.DbContext.Limit
                      where x.LimitID == limitID
                      select x).ToList<Limit>();
            return limits;
        }

        public static void UpdateLimit(Limit objlimit)
        {
            int limitID=objlimit.LimitID;
            Limit objlimit2 = DbContextHelper.DbContext.Limit.First(x => x.LimitID == limitID);
            objlimit2.LimitLetter = objlimit.LimitLetter;
            objlimit2.LimitType = objlimit.LimitType;
            objlimit2.LimitDescription = objlimit.LimitDescription;

            DbContextHelper.DbContext.SaveChanges();

        }

        public static void EditModeDeleteLimit(int limitID)
        {
            List<Limit> limit = (from s in DbContextHelper.DbContext.Limit
                                 where s.LimitID == limitID
                                           select s).ToList();

            foreach (var data in limit)
            {
                DbContextHelper.DbContext.DeleteObject(data);
                DbContextHelper.DbContext.SaveChanges();
            }
        }

	}
}
