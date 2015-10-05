using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class UmpireManager {
		public static IQueryable<UmpireMaster> GetAll() {
			var list = from x in DbContextHelper.DbContext.UmpireMaster
					 where x.Status == true
					 orderby x.UmpireName
					 select x;

			return list;
		}
		public static IQueryable<UmpireMaster> GetAll(int clientID) {
			IQueryable<UmpireMaster> umpires = null;

			umpires = from x in DbContextHelper.DbContext.UmpireMaster
					 where x.Status == true && x.ClientId == clientID
					 orderby x.UmpireName
					 select x;

			return umpires;
		}

		public static UmpireMaster Get(string name) {

			var umpire = from x in DbContextHelper.DbContext.UmpireMaster
					   where x.UmpireName == name && x.Status == true
					   select x;

			return umpire.Any() ? umpire.First() : new UmpireMaster();
		}

		public static UmpireMaster Get(int id) {
			var umpire = from x in DbContextHelper.DbContext.UmpireMaster
					   where x.UmpireId == id && x.Status == true
					   select x;

			return umpire.Any() ? umpire.First() : new UmpireMaster();
		}

		public static bool IsExist(string name, int id) {
			var status = from x in DbContextHelper.DbContext.UmpireMaster
					   where x.UmpireName == name && x.UmpireId != id && x.Status == true
					   select x;

			return status.Any();
		}

		public static UmpireMaster Save(UmpireMaster obj) {
			if (obj.UmpireId == 0) {
				//objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				obj.InsertDate = DateTime.Now;
				DbContextHelper.DbContext.Add(obj);
			}

			obj.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return obj;
		}
	}
}
