using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class PolicyLimitManager {
		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			PolicyLimit limit = new PolicyLimit { PolicyLimitID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("PolicyLimits", limit);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(limit);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		public static PolicyLimit Get(int id) {
			PolicyLimit limit = null;

			limit = (from x in DbContextHelper.DbContext.PolicyLimit
					  where x.PolicyLimitID == id
				    select x).FirstOrDefault<PolicyLimit>();

			return limit;
		}

        public static List<PolicyLimit> GetPolicyLimit(int limitID)
        {
            List<PolicyLimit> limits = null;

            limits = (from x in DbContextHelper.DbContext.PolicyLimit
                      where x.LimitID == limitID
                      select x).ToList<PolicyLimit>();
            return limits;
        }

		/// <summary>
		/// Returns first 6 limits
		/// </summary>
		/// <param name="policyID"></param>
		/// <param name="limitType"></param>
		/// <returns></returns>
		public static List<PolicyLimit> GetAll(int policyID, int limitType) {
			List<PolicyLimit> limits = null;

            limits = (from x in DbContextHelper.DbContext.PolicyLimit.Include("Limit")
                      .Include("ClaimLimit")

                      //join s in DbContextHelper.DbContext.ClaimLimit on x.PolicyID equals s.PolicyID into abc
                      //from subabc in abc.DefaultIfEmpty()
                      
                      //join c in DbContextHelper.DbContext.Claim on x.PolicyID equals c.PolicyID
                      //join s in DbContextHelper.DbContext.ClaimLimit on c.ClaimID equals s.ClaimID
                      where x.PolicyID == policyID //&& x.Limit.IsStatic == true //x.Limit.LimitType == limitType 
                      select  x ).Take(100).ToList<PolicyLimit>();

			return limits;
		}

       

        public static bool LimitIsStatic(int policyID)
        {
           // List<Limit> limits = null;
            bool isStatic = false;
            List<PolicyLimit> policyLimit = (from s in DbContextHelper.DbContext.PolicyLimit
                                             where s.PolicyID == policyID
                                             select s).ToList();
            if (policyLimit != null && policyLimit.Count>0)
            {
                int limitId = policyLimit[0].LimitID;
                    var limits = (from x in DbContextHelper.DbContext.Limit
                                  where x.LimitID == limitId
                              select x).FirstOrDefault();

                   if (limits != null)
                   {
                       isStatic = limits.IsStatic??false;

                   }
           }
            return isStatic;
        }




		public static bool Exists(int policyID, int limitType) {
			bool exists = false;

			exists = (from x in DbContextHelper.DbContext.PolicyLimit.Include("Limit")
					where x.PolicyID == policyID && x.Limit.LimitType == limitType
					select x).Any();

			return exists;
		}

		public static void primePolicyLimits(int policyID) {
			PolicyLimit propertyLimit = null;
			List<Limit> limits = null;

			// read policy limits
			limits = LimitManager.GetAll(LimitType.LIMIT_TYPE_PROPERTY);

			if (limits != null && limits.Count > 0) {
				using (TransactionScope scope = new TransactionScope()) {
					try {
						foreach (Limit limit in limits) {
							// create new policy limit
							propertyLimit = new PolicyLimit {
								LimitID = limit.LimitID,
								PolicyID = policyID,
							};

							// add policy limit
							Save(propertyLimit);
						}
						scope.Complete();
					}
					catch (Exception ex) {						
					}
				}
			}
		}
		public static PolicyLimit Save(PolicyLimit limit) {
			if (limit.PolicyLimitID == 0) {
				DbContextHelper.DbContext.Add(limit);
			}

			DbContextHelper.DbContext.SaveChanges();

			return limit;
		}

        public static void IsDeleted(int policyID)
        {
            List<PolicyLimit> policyLimit = (from s in DbContextHelper.DbContext.PolicyLimit
                                             where s.PolicyID == policyID
                                           select s).ToList();

            foreach (var limit in policyLimit)
            {
                DbContextHelper.DbContext.DeleteObject(limit);
                DbContextHelper.DbContext.SaveChanges();
            }
        }
        public static List<PolicyLimit> GetAll(int policyID)
        {
            List<PolicyLimit> limits = null;

            limits = (from x in DbContextHelper.DbContext.PolicyLimit.Include("Limit")
                      where x.PolicyID == policyID
                      select x).Take(6).ToList<PolicyLimit>();

            return limits;
        }

        public static decimal getPolicyLimitAmount(string coverageString , int policyId)
        {
            
            decimal total = 0;

            var priceQuery =
               from prod in DbContextHelper.DbContext.PolicyLimit
               join li in DbContextHelper.DbContext.Limit on prod.LimitID equals li.LimitID
               where li.LimitLetter == coverageString && prod.PolicyID == policyId
               group prod by prod.PolicyID into grouping
               select new 
               {
                   grouping.Key,
                   TotalPrice = grouping.Sum(p => p.LimitAmount )
               };

            foreach (var grp in priceQuery)
            {

                total = Convert.ToDecimal(grp.TotalPrice);
            }



            return total;

        }

        public static decimal getPolicyLimitAmountOther(int policyId)
        {
            
            decimal total = 0;

            var priceQuery =
               from prod in DbContextHelper.DbContext.PolicyLimit
               join li in DbContextHelper.DbContext.Limit on prod.LimitID equals li.LimitID
               where li.LimitLetter != "A"
                    && li.LimitLetter != "B"
                    && li.LimitLetter != "C"
                    && li.LimitLetter != "D"
                    && li.LimitLetter != "E"
                    && prod.PolicyID == policyId
               group prod by prod.PolicyID into grouping
               select new 
               {
                   grouping.Key,
                   TotalPrice = grouping.Sum(p => p.LimitAmount )
               };

            foreach (var grp in priceQuery)
            {

                total = Convert.ToDecimal(grp.TotalPrice);
            }



            return total;

        }
       


        public static void UpdatePolicyLimit(PolicyLimit objlimit)
        {
            int limitID = objlimit.LimitID;
            int PolicyID = objlimit.PolicyID;
            PolicyLimit objPolicyLimit = DbContextHelper.DbContext.PolicyLimit.First(x => x.LimitID == limitID && x.PolicyID == PolicyID);
            objPolicyLimit.LimitAmount = objlimit.LimitAmount;
            objPolicyLimit.LimitDeductible = objlimit.LimitDeductible;
            objPolicyLimit.LimitDeductible = objlimit.LimitDeductible;
            objPolicyLimit.CATDeductible = objlimit.CATDeductible;
            objPolicyLimit.ConInsuranceLimit = objlimit.ConInsuranceLimit;
            objPolicyLimit.ITV = objlimit.ITV;
            objPolicyLimit.Reserve = objlimit.Reserve;
            objPolicyLimit.ApplyAcrossAllCoverage = objlimit.ApplyAcrossAllCoverage;
            objPolicyLimit.ApplyTo = objlimit.ApplyTo;

            DbContextHelper.DbContext.SaveChanges();

        }
        public static void EditModeDeletePolicyLimit(int limitID)
        {
            List<PolicyLimit> limit = (from s in DbContextHelper.DbContext.PolicyLimit
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
