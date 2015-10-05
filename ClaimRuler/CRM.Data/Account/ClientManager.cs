using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Linq.Expressions;
using LinqKit;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class ClientManager {

		public static Client Get(int id) {
			return (from x in DbContextHelper.DbContext.Client
					   .Include("SecUser")
					   .Include("CityMaster")
					   .Include("StateMaster")
					   where x.ClientId == id
					   select x
					).FirstOrDefault();
		}
		public static Client GetByID(int id) {
			return (from x in DbContextHelper.DbContext.Client					  
				   where x.ClientId == id
				   select x
					).FirstOrDefault();
		}

		public static IQueryable<Client> GetClients() {
			return from x in DbContextHelper.DbContext.Client
				  where x.Active != null && x.Active == 1
				  select x;
					
		}

		public static List<Client> GetAll() {
			return (from x in DbContextHelper.DbContext.Client
				   where x.Active != null && x.Active == 1
				   orderby x.BusinessName
				   select x
					).ToList();
		}

		public static int GetMaximumUsersAllowed(int id) {
			int? count = (from x in DbContextHelper.DbContext.Client
					    where x.ClientId == id
					    select x.maxUsers).FirstOrDefault();

			// minimum of one user
			return count ?? 1;
					
		}

		public static int GetUsersCount(int id) {
			int? count = (from x in DbContextHelper.DbContext.Client
					    where x.ClientId == id
					    select x.maxUsers).Count();

			// minimum of one user
			return count ?? 1;

		}

		public static int GetLeadCount(int id) {
			int? count = (from x in DbContextHelper.DbContext.Leads
					    where x.ClientID == id
					    select x.LeadId).Count();
			
			return count ?? 0;

		}

		public static bool CheckAutomaticInvoiceMethodSelection(int clientID) {
			int? invoiceSettingID = (from x in DbContextHelper.DbContext.Client
						    where x.ClientId == clientID
						    select x.InvoiceSettingID 
					    ).FirstOrDefault();


			return (invoiceSettingID ?? 0) > 0;
		}
		public static bool UsersLimitReached(int id) {
			int maxUsers = GetMaximumUsersAllowed(id);

			int usersCount = GetUsersCount(id);

			return usersCount >= maxUsers;
		}

		public static int Save(Client client) {
			if (client.ClientId == 0) {

				client.InsertDate = DateTime.Now;
				DbContextHelper.DbContext.Add(client);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			client.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return client.ClientId;
		}
	}
}
