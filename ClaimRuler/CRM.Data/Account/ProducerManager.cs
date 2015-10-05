

namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;

	using System.Linq.Expressions;
	using LinqKit;
    using CRM.Data.Entities;
	public class ProducerManager {
		public static List<ProducerMaster> GetAll() {
			var list = from x in DbContextHelper.DbContext.ProducerMaster
					 where x.Status == 1
					 select x;

			return list.ToList();
		}

		public static IQueryable<ProducerMaster> GetAll(int clientID) {
			IQueryable<ProducerMaster> producers = null;

			producers = from x in DbContextHelper.DbContext.ProducerMaster
					  where x.Status == 1 && x.ClientId == clientID
					  orderby x.ProducerName
					  select x;
					

			return producers;
		}

		public static ProducerMaster GetByProducerName(string producerName) {

			var users = from x in DbContextHelper.DbContext.ProducerMaster
					  where x.ProducerName == producerName && x.Status == 1
					  select x;

			return users.Any() ? users.First() : new ProducerMaster();
		}

		public static ProducerMaster GetProducerId(int producerId) {

			var users = from x in DbContextHelper.DbContext.ProducerMaster
					  where x.ProducerId == producerId && x.Status == 1
					  select x;

			return users.Any() ? users.First() : new ProducerMaster();
		}

		public static ProducerMaster Save(ProducerMaster objProducer) {
			if (objProducer.ProducerId == 0) {
				//objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				objProducer.InsertDate = DateTime.Now;
				objProducer.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				DbContextHelper.DbContext.Add(objProducer);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			objProducer.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			objProducer.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return objProducer;
		}

		public static bool IsExist(string name, int clientID) {
			var status = from x in DbContextHelper.DbContext.ProducerMaster
					   where x.ProducerName == name && x.ClientId == clientID && x.Status == 1
					   select x;

			return status.Any();
		}

		public static bool IsExist(string name, int Id, int clientID) {
			var status = from x in DbContextHelper.DbContext.ProducerMaster
					   where x.ProducerName == name && x.ProducerId != Id && x.Status == 1 && x.ClientId == clientID
					   select x;

			return status.Any();
		}
	}
}
