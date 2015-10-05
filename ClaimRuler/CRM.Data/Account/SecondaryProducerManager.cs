
namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;
	using System.Linq.Expressions;
	using LinqKit;
    using CRM.Data.Entities;

	public class SecondaryProducerManager {
		public static List<SecondaryProducerMaster> GetAll() {
			var list = from x in DbContextHelper.DbContext.SecondaryProducerMaster
					 where x.Status == true
					 orderby x.SecondaryProduceName
					 select x;

			return list.ToList();
		}

		public static List<SecondaryProducerMaster> GetAll(int clientID) {
			List<SecondaryProducerMaster> producers = null;

			producers = (from x in DbContextHelper.DbContext.SecondaryProducerMaster
					   where x.Status == true && x.ClientID == clientID
					   orderby x.SecondaryProduceName
					   select x
					 ).ToList();

			return producers;
		}

		public static SecondaryProducerMaster GetBySecondaryProducerName(string name) {

			var secProducer = from x in DbContextHelper.DbContext.SecondaryProducerMaster
						   where x.SecondaryProduceName == name && x.Status == true
						   select x;

			return secProducer.Any() ? secProducer.First() : new SecondaryProducerMaster();
		}

		public static SecondaryProducerMaster GetSecondaryProducerId(int secondaryProducerId) {
			var secProducer = from x in DbContextHelper.DbContext.SecondaryProducerMaster
						   where x.SecondaryProduceId == secondaryProducerId && x.Status == true
						   select x;

			return secProducer.Any() ? secProducer.First() : new SecondaryProducerMaster();
		}
		public static bool IsExist(string secondaryProducerName, int secondaryProducerId) {
			var status = from x in DbContextHelper.DbContext.SecondaryProducerMaster
					   where x.SecondaryProduceName == secondaryProducerName && x.SecondaryProduceId != secondaryProducerId && x.Status == true
					   select x;

			return status.Any();
		}

		public static SecondaryProducerMaster Save(SecondaryProducerMaster objsecondaryProducer) {
			if (objsecondaryProducer.SecondaryProduceId == 0) {
				//objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				objsecondaryProducer.InsertDate = DateTime.Now;
				objsecondaryProducer.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				DbContextHelper.DbContext.Add(objsecondaryProducer);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			objsecondaryProducer.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			objsecondaryProducer.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return objsecondaryProducer;
		}
	}
}
