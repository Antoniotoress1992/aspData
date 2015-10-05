namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;
	using System.Linq.Expressions;
	using LinqKit;
    using CRM.Data.Entities;

	public class LeadSourceManager {
		public static List<LeadSourceMaster> GetAll() {
			var list = from x in DbContextHelper.DbContext.LeadSourceMaster
					 where x.Status == true 
					 orderby x.LeadSourceName
					 select x;

			return list.ToList();
		}

		public static List<CRM.Data.Entities.LeadSourceMaster> GetAll(int clientID) {
			var list = from x in DbContextHelper.DbContext.LeadSourceMaster
					 where x.Status == true && (x.ClientId == clientID || x.ClientId == null)
					 orderby x.LeadSourceName
					 select x;

			return list.ToList();
		}
		public static LeadSourceMaster GetByLeadSourceName(string leadSourceName) {

			var lsource = from x in DbContextHelper.DbContext.LeadSourceMaster
					    where x.LeadSourceName == leadSourceName && x.Status == true
					    select x;

			return lsource.Any() ? lsource.First() : new LeadSourceMaster();
		}

		public static LeadSourceMaster GetLeadSourceId(int leadSourceId) {
			var lsource = from x in DbContextHelper.DbContext.LeadSourceMaster
					    where x.LeadSourceId == leadSourceId && x.Status == true
					    select x;

			return lsource.Any() ? lsource.First() : new LeadSourceMaster();
		}
		public static bool IsExist(string leadSourceName, int leadSourceId) {
			var status = from x in DbContextHelper.DbContext.LeadSourceMaster
					   where x.LeadSourceName == leadSourceName && x.LeadSourceId != leadSourceId && x.Status == true
					   select x;

			return status.Any();
		}

		public static LeadSourceMaster Save(LeadSourceMaster objleadSource) {
			if (objleadSource.LeadSourceId == 0) {
				//objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				objleadSource.InsertDate = DateTime.Now;
				objleadSource.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				DbContextHelper.DbContext.Add(objleadSource);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			objleadSource.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			objleadSource.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return objleadSource;
		}
	}
}
