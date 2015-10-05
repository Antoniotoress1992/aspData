using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class ClaimSubLimitManager {
		public static List<ClaimSubLimit> GetAll(int claimID) {
			List<ClaimSubLimit> sublimits = null;

			sublimits = (from x in DbContextHelper.DbContext.ClaimSubLimit
					where x.ClaimID == claimID					
					select x).ToList<ClaimSubLimit>();

			return sublimits;
		}

		public static ClaimSubLimit Get(int id) {
			ClaimSubLimit sublimit = null;

			sublimit = (from x in DbContextHelper.DbContext.ClaimSubLimit
					where x.ClaimSubLimitID == id
					select x).FirstOrDefault<ClaimSubLimit>();

			return sublimit;
		}

		public static ClaimSubLimit Save(ClaimSubLimit sublimit) {
			if (sublimit.ClaimSubLimitID == 0)
				DbContextHelper.DbContext.Add(sublimit);

			DbContextHelper.DbContext.SaveChanges();

			return sublimit;
		}
	}
}
