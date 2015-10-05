using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class PolicySublimitManager {

		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			PolicySubLimit limit = new PolicySubLimit { PolicySublimitID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("PolicySubLimits", limit);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(limit);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		public static List<PolicySubLimit> GetAll(int policyID, int limitType) {
			List<PolicySubLimit> limits = null;

			limits = (from x in DbContextHelper.DbContext.PolicySubLimit
					where x.PolicyID == policyID && x.LimitType == limitType
					select x).ToList<PolicySubLimit>();

			return limits;
		}

		public static PolicySubLimit Get(int id) {
			PolicySubLimit limit = null;

			limit = (from x in DbContextHelper.DbContext.PolicySubLimit
					where x.PolicySublimitID == id
					select x).FirstOrDefault<PolicySubLimit>();

			return limit;
		}

		public static PolicySubLimit Save(PolicySubLimit limit) {
			if (limit.PolicySublimitID == 0) {
				DbContextHelper.DbContext.Add(limit);
			}

			DbContextHelper.DbContext.SaveChanges();

			return limit;
		}
	}
}
