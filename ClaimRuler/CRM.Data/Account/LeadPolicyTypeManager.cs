using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;
using System.Web.UI.WebControls;

namespace CRM.Data.Account {


	public static class LeadPolicyTypeManager {
		static public List<TaskPolicyType> GetPolicyTypesForScheduler() {
			List<TaskPolicyType> policyTypes = null;

			policyTypes = (from x in DbContextHelper.DbContext.LeadPolicyType
						orderby x.LeadPolicyTypeID
						select new TaskPolicyType {
							key = x.LeadPolicyTypeID,
							label = x.Description
						}).ToList<TaskPolicyType>();

			return policyTypes;
		}

		static public LeadPolicyType Get(int id) {
			LeadPolicyType policyType = null;

			policyType = (from x in DbContextHelper.DbContext.LeadPolicyType
						where x.LeadPolicyTypeID == id
						orderby x.Description
						select x
					  ).FirstOrDefault<LeadPolicyType>();

			return policyType;
		}


		static public List<LeadPolicyType> GetAll() {
			List<LeadPolicyType> policyTypes = null;

			policyTypes = (from x in DbContextHelper.DbContext.LeadPolicyType
						orderby x.Description
						select x
					  ).ToList<LeadPolicyType>();

			return policyTypes;
		}

		static public List<CurrentLeadPolicy> GetCurrentPolicy(Leads lead) {
			List<CurrentLeadPolicy> currentPolicyTypes = null;

			if (lead.LeadPolicy != null) {
				currentPolicyTypes = (from x in lead.LeadPolicy
								  where x.IsActive
								  select new CurrentLeadPolicy {
									  policyTypeDescription = x.LeadPolicyType.Description,
									  policyTypeID = x.LeadPolicyType.LeadPolicyTypeID
								  }).ToList();
			}

			return currentPolicyTypes;
		}
	}
}
