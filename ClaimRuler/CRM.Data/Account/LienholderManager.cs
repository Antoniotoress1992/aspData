using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class LienholderManager {
		public static PolicyLienholder Save(PolicyLienholder lienholder) {
			if (lienholder.ID == 0) {
				DbContextHelper.DbContext.Add(lienholder);
			}

			DbContextHelper.DbContext.SaveChanges();

			return lienholder;
		}

		public static void Delete(int id) {
			PolicyLienholder lienholder = new PolicyLienholder { ID = id };

			DbContextHelper.DbContext.AttachTo("PolicyLienholders", lienholder);

			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(lienholder);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}
		public static List<PolicyLienholder> GetAll(int policyID) {
			List<PolicyLienholder> lienholders = null;

			lienholders = (from x in DbContextHelper.DbContext.PolicyLienholder.Include("Mortgagee")
						where x.PolicyID == policyID
						select x).ToList<PolicyLienholder>();

			return lienholders;
		}

		
	}
}
