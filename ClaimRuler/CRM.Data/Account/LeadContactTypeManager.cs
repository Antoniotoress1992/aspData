using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Linq.Expressions;
using LinqKit;
using CRM.Data.Entities;
using CRM.Data;

namespace CRM.Data.Account {
	public class LeadContactTypeManager {
		public static LeadContactType Get(int id) {
			LeadContactType contactType = (from x in DbContextHelper.DbContext.LeadContactType
					 where x.ID == id
					 select x).FirstOrDefault<LeadContactType>();

			return contactType;
		}
		public static LeadContactType Get(int clientID, string description) {
			LeadContactType contactType = (from x in DbContextHelper.DbContext.LeadContactType
									 where x.ClientID == clientID &&
									 x.Description.Contains(description)
									 select x
								).FirstOrDefault<LeadContactType>();

			return contactType;
		}

		public static List<LeadContactType> GetAll(int clientID) {
			var list = from x in DbContextHelper.DbContext.LeadContactType
					 where x.isActive == true && (x.ClientID == null || x.ClientID == clientID)
					 orderby x.Description
					 select x;

			return list.ToList();

		}

		public static LeadContactType Save(LeadContactType contactType) {
			if (contactType.ID == 0)
				DbContextHelper.DbContext.Add(contactType);

			DbContextHelper.DbContext.SaveChanges();

			return contactType;
		}
	}
}
