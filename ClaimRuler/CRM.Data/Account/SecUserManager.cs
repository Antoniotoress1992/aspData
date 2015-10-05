
namespace CRM.Data.Account {
	#region Namespace
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Web;
	using System.Text;
	using LinqKit;
    using CRM.Data.Entities;
	#endregion

	public class SecUserManager {


		public static List<SecUser> Search(string userName, int roleId, bool? status) {
			List<SecUser> result = (from c in DbContextHelper.DbContext.SecUser
							    where c.UserName == (userName == string.Empty ? c.UserName : userName)
							    && c.RoleId == (roleId <= 0 ? c.RoleId : roleId)
							    && c.Status == (status == null ? c.Status : status)
							    select c).ToList<SecUser>();
			return result;
		}

		public static List<SecUser> GetPredicate(Expression<Func<SecUser, bool>> predicate) {
			return DbContextHelper.DbContext.SecUser
			   .AsExpandable()
			   .Where(predicate)//.DefaultIfEmpty()
			   .ToList();
		}

		public static IQueryable<SecUser> GetUsers(Expression<Func<SecUser, bool>> predicate) {
			return DbContextHelper.DbContext.SecUser
			   .AsExpandable()
			   .Where(predicate);
			   
		}

		public static List<SecUser> GetUsers(int clientID) {
			return DbContextHelper.DbContext.SecUser.Where(x => x.ClientID == clientID && x.Status == true).ToList();
			   

		}

		public static SecUser Save(SecUser secUser) {
			if (secUser.UserId == 0) {

				secUser.CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				secUser.CreatedOn = DateTime.Now;
				secUser.CreatedMachineIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				DbContextHelper.DbContext.Add(secUser);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			secUser.UpdatedMachineIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			secUser.UpdatedOn = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return secUser;
		}

		public static SecUser GetByUserName(string userName) {
			//var users = from x in DbContextHelper.DbContext.SecUsers
			//            where x.UserName == userName && x.Status == true && x.Blocked==false
			//            select x;
			var users = from x in DbContextHelper.DbContext.SecUser.Include("Client")
					  where x.UserName == userName && x.Status == true
					  select x;

			return users.Any() ? users.First() : new SecUser();
		}

        public static CRM.Data.Entities.SecUser GetByUserId(int userId)
        {
			var users = from x in DbContextHelper.DbContext.SecUser.Include("Client")
					  .Include("SecRole")
					  where x.UserId == userId
					  select x;

			return users.Any() ? users.First() : new SecUser();
		}

        public static bool GetTutorialMode(int userId)
        {
            var tutorialMode = from x in DbContextHelper.DbContext.SecUser                      
                                where x.UserId == userId
                                 select x.TutorialMode;

            return tutorialMode.FirstOrDefault() ?? false;
        }

        public static void SetTutorialMode(int userId, bool tutorialMode)
        {
            SecUser secUser = DbContextHelper.DbContext.SecUser.First(x => x.UserId == userId);

            secUser.TutorialMode = tutorialMode;
            DbContextHelper.DbContext.SaveChanges();
           
        }



		public static SecUser GetById(int userId) {
			SecUser user = (from x in DbContextHelper.DbContext.SecUser
						 where x.UserId == userId
						 select x).FirstOrDefault();

			return user;
		}

		public static bool IsUserNameExist(string userName) {
			var users = from x in DbContextHelper.DbContext.SecUser
					  where x.UserName == userName
					  select x;

			return users.Any();
		}

		public static List<SecUser> GetAll() {
			string active = Globals.Status.Active.ToString();
			var list = from x in DbContextHelper.DbContext.SecUser
					 select x;

			return list.ToList();
		}
		
		/// <summary>
		/// Return all users for a client
		/// </summary>
		/// <param name="clientID"></param>
		/// <returns></returns>
        public static List<CRM.Data.Entities.UserStaff> GetStaff(int clientID)
        {
			List<UserStaff> users = null;

			users = (from x in DbContextHelper.DbContext.SecUser
				    where x.ClientID == clientID && x.Status == true
				    orderby x.LastName, x.FirstName
				    select new UserStaff {
					    UserId = x.UserId,
					    StaffName = x.FirstName + " " + x.LastName,
					    EmailAddress = x.Email
				    }).ToList();

			return users;
		}

		//public static SecUser SelectUser()
		//{
		//    var users = from x in DbContextHelper.DbContext.SecUsers

		//                select x;

		//    return users.Any() ? users.First() : new SecUser();
		//}
		public static void Delete(SecUser user) {
			DbContextHelper.DbContext.DeleteObject(user);
		}

		public static bool IsEmailExist(string email) {
			var users = from x in DbContextHelper.DbContext.SecUser
					  where x.Email == email
					  select x;
			return users.Any();
		}
	}
}


namespace CRM.Data {
	public partial class SecUser {
		public string RoleName {
			get { return this.RoleName; }
		}
	}
}