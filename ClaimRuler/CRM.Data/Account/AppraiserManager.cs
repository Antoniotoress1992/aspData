using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class AppraiserManager {
		public static IQueryable<AppraiserMaster> GetAll() {
			var list = from x in DbContextHelper.DbContext.AppraiserMaster
					 orderby x.AppraiserName
					 where x.Status == true
					 select x;

			return list;
		}
		public static IQueryable<AppraiserMaster> GetAll(int clientID) {
			IQueryable<AppraiserMaster> adjusters = null;

			adjusters = from x in DbContextHelper.DbContext.AppraiserMaster
					   where x.Status == true && x.ClientId == clientID
					   orderby x.AppraiserName
					   select x;

			return adjusters;
		}

		public static AppraiserMaster Get(string name) {

			var appraiser = from x in DbContextHelper.DbContext.AppraiserMaster
						  where x.AppraiserName == name && x.Status == true
						  select x;

			return appraiser.Any() ? appraiser.First() : new AppraiserMaster();
		}

		public static AppraiserMaster Get(int id) {
			var appraiser = from x in DbContextHelper.DbContext.AppraiserMaster
						  where x.AppraiserId == id && x.Status == true
						  select x;

			return appraiser.Any() ? appraiser.First() : new AppraiserMaster();
		}
		public static bool IsExist(string name, int id) {
			var status = from x in DbContextHelper.DbContext.AppraiserMaster
					   where x.AppraiserName == name && x.AppraiserId != id && x.Status == true
					   select x;

			return status.Any();
		}

		public static AppraiserMaster Save(AppraiserMaster obj) {
			if (obj.AppraiserId == 0) {
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
