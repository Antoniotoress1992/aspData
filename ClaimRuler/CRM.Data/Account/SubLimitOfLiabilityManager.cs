using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;


namespace CRM.Data.Account {
	static public class SubLimitOfLiabilityManager {
		public static SubLimitOfLiabilityMaster Get(int id) {
			SubLimitOfLiabilityMaster sublimit = null;

			sublimit = (from x in DbContextHelper.DbContext.SubLimitOfLiabilityMaster
					  where x.SublimitLiabilityID == id
					  select x
						).FirstOrDefault<SubLimitOfLiabilityMaster>();

			return sublimit;
		}

		public static List<SubLimitOfLiabilityMaster> GetAll(int clientID) {
			List<SubLimitOfLiabilityMaster> list = null;

			list = (from x in DbContextHelper.DbContext.SubLimitOfLiabilityMaster
				   where x.IsActive == true && (x.ClientId == clientID || x.ClientId == null)
				   orderby x.Description
				   select x
						).ToList<SubLimitOfLiabilityMaster>();

			return list;
		}

		public static bool IsExist(string sublimt, int clientID) {
			var status = from x in DbContextHelper.DbContext.SubLimitOfLiabilityMaster
					   where x.Description == sublimt && x.ClientId == clientID && x.IsActive
					   select x;

			return status.Any();
		}

		public static SubLimitOfLiabilityMaster Save(SubLimitOfLiabilityMaster sublimit) {
			if (sublimit.SublimitLiabilityID == 0) {
				DbContextHelper.DbContext.Add(sublimit);
			}
			
			DbContextHelper.DbContext.SaveChanges();

			return sublimit;
		}

	}
}
