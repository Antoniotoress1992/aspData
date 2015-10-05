
namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;
	using System.Linq.Expressions;
	using LinqKit;
    using CRM.Data.Entities;

	public class SubStatusManager {
		public static List<SubStatusMaster> GetAll() {
			var list = from x in DbContextHelper.DbContext.SubStatusMaster
					 where x.Status == true && x.clientID == null
					 orderby x.SubStatusName
					 select x;

			return list.ToList();
		}

		public static List<SubStatusMaster> GetAll(int clientID) {
			List<SubStatusMaster> list = null;

			list = (from x in DbContextHelper.DbContext.SubStatusMaster
				   where x.Status == true && x.clientID == clientID
				   orderby x.SubStatusName
				   select x
						 ).ToList<SubStatusMaster>();

			return list;
		}

		public static SubStatusMaster GetBySumStatusName(string subStatusName) {

			var subStatus = from x in DbContextHelper.DbContext.SubStatusMaster
						 where x.SubStatusName == subStatusName && x.Status == true
						 select x;

			return subStatus.Any() ? subStatus.First() : new SubStatusMaster();
		}

		public static SubStatusMaster GetSubStatusId(int subStatusId) {
			var subStatus = from x in DbContextHelper.DbContext.SubStatusMaster
						 where x.SubStatusId == subStatusId && x.Status == true
						 select x;

			return subStatus.Any() ? subStatus.First() : new SubStatusMaster();
		}
		public static bool IsExist(string subStatusName, int clientID) {
			var status = from x in DbContextHelper.DbContext.SubStatusMaster
					   where x.SubStatusName == subStatusName && x.clientID == clientID && x.Status == true
					   select x;

			return status.Any();
		}

		public static SubStatusMaster Save(SubStatusMaster objSubStatus) {
			if (objSubStatus.SubStatusId == 0) {
				//objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				objSubStatus.InsertDate = DateTime.Now;
				objSubStatus.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				DbContextHelper.DbContext.Add(objSubStatus);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			objSubStatus.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			objSubStatus.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return objSubStatus;
		}

	}
}
