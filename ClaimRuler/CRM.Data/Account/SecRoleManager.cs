
namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;
    using CRM.Data.Entities;

	public class SecRoleManager {
		public static SecRole Save(SecRole secRole) {
			if (secRole.RoleId == 0) {
				secRole.CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				secRole.CreatedMachineIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				secRole.CreatedOn = DateTime.Now;
				DbContextHelper.DbContext.Add(secRole);				
			}

			secRole.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			secRole.UpdatedMachineIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			secRole.UpdatedOn = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return secRole;
		}

		public static List<SecRole> GetAll() {
			var list = from x in DbContextHelper.DbContext.SecRole
					 where x.RoleId != 2 && x.Status == true
					 select x;

			return list.ToList();
		}
		public static List<SecRole> GetSystemRoles() {
			var list = from x in DbContextHelper.DbContext.SecRole
					 where x.ClientID == null && x.Status == true
					 select x;

			return list.ToList();
		}

		public static List<SecRole> GetAll(int roleID) {
			List<SecRole> roles = null;

			if (roleID == (int)UserRole.Administrator) {
				roles = DbContextHelper.DbContext.SecRole.Where(x => x.Status == true).ToList();
			}
			else if (roleID == (int)UserRole.Client || roleID == (int)UserRole.SiteAdministrator) {
				//roles = DbContextHelper.DbContext.SecRoles.Where(x => (x.Status == true && x.RoleId == (int)UserRole.User)).ToList();	// show all except administrator

				// get those roles available to client
				roles = DbContextHelper.DbContext.SecRole.Where(x => x.Status == true && x.isClient == true).ToList();	// show all except administrator
			}

			return roles;
		}

		/// <summary>
		/// Returns list of roles created by client
		/// </summary>
		/// <param name="clientID"></param>
		/// <returns></returns>
		public static List<SecRole> GetRolesManagedByClient(int clientID) {
			List<SecRole> roles = null;

			// get those roles available to client
			roles = (from x in DbContextHelper.DbContext.SecRole
				    orderby x.RoleName
				   // where x.Status == true && (x.ClientID == clientID || x.isClient == true)
				    where x.Status == true && (x.ClientID == clientID)
				    select x).ToList<SecRole>();

			return roles;
		}


		public static SecRole GetByRoleId(int roleId) {
			var roles = from x in DbContextHelper.DbContext.SecRole
					  where x.RoleId == roleId
					  select x;
			return roles.Any() ? roles.First() : new SecRole();
		}

		public static void Delete(SecRole role) {
			DbContextHelper.DbContext.DeleteObject(role);
		}


	}
}
