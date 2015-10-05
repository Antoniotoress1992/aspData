using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;
using CRM.Data;

namespace CRM.Data.Account {
	static public class LeadPolicyDamageTypeManager {
		public static void DeleteAll(int policyID) {
			//DbContextHelper.DbContext.ExecuteStoreCommand("DELETE FROM LeadPolicyDamageType WHERE PolicyID = {0}", policyID);


            var policy = DbContextHelper.DbContext.LeadPolicyDamageType.Where(x => x.PolicyID == policyID).ToList();
            foreach (LeadPolicyDamageType objR in policy)
            {
                DbContextHelper.DbContext.DeleteObject(objR);
            }
            DbContextHelper.DbContext.SaveChanges();
		}

		public static void Save(LeadPolicyDamageType lossType) {
			
			DbContextHelper.DbContext.Add(lossType);
			
			DbContextHelper.DbContext.SaveChanges();			
		}
	}
}
