using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class ContractorManager {
		public static IQueryable<ContractorMaster> GetAll() {
			var list = from x in DbContextHelper.DbContext.ContractorMaster
					 where x.Status == true
					 orderby x.ContractorName
					 select x;

			return list;
		}
		public static IQueryable<ContractorMaster> GetAll(int clientID) {
			IQueryable<ContractorMaster> adjusters = null;

			adjusters = from x in DbContextHelper.DbContext.ContractorMaster
					  where x.Status == true && x.ClientId == clientID
					  orderby x.ContractorName
					  select x;

			return adjusters;
		}

		public static ContractorMaster Get(string name) {

			var contractor = from x in DbContextHelper.DbContext.ContractorMaster
						where x.ContractorName == name && x.Status == true
						select x;

			return contractor.Any() ? contractor.First() : new ContractorMaster();
		}

		public static ContractorMaster Get(int id) {
			var contractor = from x in DbContextHelper.DbContext.ContractorMaster
						where x.ContractorId == id && x.Status == true
						select x;

			return contractor.Any() ? contractor.First() : new ContractorMaster();
		}
		public static bool IsExist(string name, int id) {
			var status = from x in DbContextHelper.DbContext.ContractorMaster
					   where x.ContractorName == name && x.ContractorId != id && x.Status == true
					   select x;

			return status.Any();
		}

		public static ContractorMaster Save(ContractorMaster obj) {
			if (obj.ContractorId == 0) {
				//objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				obj.InsertDate = DateTime.Now;
				DbContextHelper.DbContext.Add(obj);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			obj.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return obj;
		}
	}
	
}
