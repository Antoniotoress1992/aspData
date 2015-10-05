using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class LeadPolicyManager {

		/// <summary>
		/// Returns complete policy object
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        static public CRM.Data.Entities.LeadPolicy Get(int id)
        {
			LeadPolicy policy = null;

			policy = (from x in DbContextHelper.DbContext.LeadPolicy
						.Include("StatusMaster")
						.Include("SubStatusMaster")
						.Include("LeadPolicyType")
						.Include("Carrier")
						.Include("Carrier.StateMaster")
						.Include("Carrier.CityMaster")
						.Include("LeadPolicyDamageType")
					where x.Id == id
					select x
					  ).FirstOrDefault<LeadPolicy>();

			return policy;
		}

		static public LeadPolicy GetWithLeadCarrier(int id) {
			LeadPolicy policy = null;

			policy = (from x in DbContextHelper.DbContext.LeadPolicy
						.Include("StatusMaster")
						.Include("SubStatusMaster")
						.Include("LeadPolicyType")
						.Include("Carrier")
						.Include("Carrier.StateMaster")
						.Include("Carrier.CityMaster")
						.Include("Leads")
					where x.Id == id
					select x
					  ).FirstOrDefault<LeadPolicy>();

			return policy;
		}

		static public LeadPolicy GetByID(int id) {
			LeadPolicy policy = null;

			policy = (from x in DbContextHelper.DbContext.LeadPolicy
					where x.Id == id
					select x
					  ).FirstOrDefault<LeadPolicy>();

			return policy;
		}
		static public List<LeadPolicy> GetPolicies(int leadID) {
			List<LeadPolicy> policies = null;

			policies = (from x in DbContextHelper.DbContext.LeadPolicy
					  .Include("StatusMaster")
					  .Include("SubStatusMaster")
					   .Include("AdjusterMaster")
					   .Include("LeadPolicyType")
					   .Include("Carrier")
					   .Include("LeadPolicyCoverage")
					  where x.LeadId == leadID && x.IsActive
					  select x
					  ).ToList<LeadPolicy>();

			return policies;
		}

		static public string GetPolicyTypeDescription(int policyID) {
			string policyType = null;

			policyType = (from x in DbContextHelper.DbContext.LeadPolicy.Include("LeadPolicyType")
							 where x.Id == policyID
							 select x.LeadPolicyType.Description
					  ).FirstOrDefault<string>();

			return policyType;
		}

		static public string GetPolicyNumber(int policyID) {
			string policyNumber = null;

			policyNumber = (from x in DbContextHelper.DbContext.LeadPolicy
					    where x.Id == policyID
					    select x.PolicyNumber
					  ).FirstOrDefault<string>();

			return policyNumber;
		}

		static public List<LeadPolicy> GetAll(int leadID) {
			List<LeadPolicy> policies = null;

			policies = (from x in DbContextHelper.DbContext.LeadPolicy
					  .Include("StatusMaster")
					  .Include("SubStatusMaster")
					  where x.LeadId == leadID && x.IsActive
					  select x
					  ).ToList<LeadPolicy>();

			return policies;
		}

		//static public List<LeadPolicyType> GetPolicyTypes() {
		//     List<LeadPolicyType> policyTypes = null;

		//     policyTypes = (from x in DbContextHelper.DbContext.LeadPolicyTypes					  
		//                 select x
		//                 ).ToList<LeadPolicyType>();

		//     return policyTypes;
		//}

		static public string[] GetPolicyClaims(int leadID) {
			return (from x in DbContextHelper.DbContext.LeadPolicy
				   where x.LeadId == leadID
				   select x.ClaimNumber
					  ).ToArray();
		}

		static public int Save(LeadPolicy policy) {
			if (policy.Id == 0) {
				DbContextHelper.DbContext.Add(policy);
			}

			DbContextHelper.DbContext.SaveChanges();

			return policy.Id;
		}

        static public int Update(LeadPolicy policy)
        {
            LeadPolicy policy2 = DbContextHelper.DbContext.LeadPolicy.First(x=>x.Id==policy.Id);
            if (policy2!=null)
            {
            policy2.ApplyAcrossAllCoverage = policy.ApplyAcrossAllCoverage;
            DbContextHelper.DbContext.SaveChanges();
            }

            return policy.Id;
        }


	}
}
