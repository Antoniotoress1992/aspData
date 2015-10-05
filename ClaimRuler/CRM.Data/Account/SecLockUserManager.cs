

namespace CRM.Data.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Linq.Expressions;
    using LinqKit;
    using CRM.Data.Entities;

    public class SecLockUserManager
    {
        public static void UpdateSecUserLockStatus(int userId)
        {
            var userLock = from x in DbContextHelper.DbContext.SecUserLock
                              where x.UserId == userId //&& x.Status == true
                              select x;
            foreach (SecUserLock secUserLock in userLock)
            {
                secUserLock.Status = false;
                secUserLock.UpdatedBy = 1;
                secUserLock.UpdatedOn = DateTime.Now;
            }
            DbContextHelper.DbContext.SaveChanges();
        }
        public static void Save(SecUserLock secUserLock)
        {
            DbContextHelper.DbContext.Add(secUserLock);
            DbContextHelper.SaveChanges();

        }
        public static SecUserLock GetByLockId(int lockId)
        {
            var userLock = from x in DbContextHelper.DbContext.SecUserLock
                           where x.LockId == lockId
                           select x;
            return userLock.Any() ? userLock.First() : new SecUserLock();
        }
        //Get Functionality
        public static List<SecUserLock> GetAll()//int skipPage, int pageSize
        {
           

            List<SecUserLock> result = (from c in DbContextHelper.DbContext.SecUserLock
                                        where c.Status == true && c.IsLocked == true && c.SecUser.RoleId !=3
                                        select c).ToList<SecUserLock>();
            return result;
        }
        public static List<SecUser> Search(string userName, int roleId, bool? status)
        {
            List<SecUser> result = (from c in DbContextHelper.DbContext.SecUser
                                    where c.UserName == (userName == string.Empty ? c.UserName : userName)
                                    && c.RoleId == (roleId <= 0 ? c.RoleId : roleId)
                                    && c.Status == (status == null ? c.Status : status)
                                    select c).ToList<SecUser>();
            return result;
        }
        public static List<SecUserLock> GetSelectVendor(Expression<Func<SecUserLock, bool>> predicate)//, int skipPage, int pageSize
        {
            //return DbContextHelper.DbContext.SecUserLocks
            //   .AsExpandable()
            //   .Where(predicate).OrderBy(i => i.Note).Skip(skipPage * pageSize).Take(pageSize).ToList();

            return DbContextHelper.DbContext.SecUserLock
               .AsExpandable()
               .Where(predicate)//.DefaultIfEmpty()
               .ToList();
        }

        public static List<SecUserLock> GetPredicate(Expression<Func<SecUserLock, bool>> predicate)//, int skipPage, int pageSize
        {
            //return DbContextHelper.DbContext.SecUserLocks
            //   .AsExpandable()
            //   .Where(predicate).OrderBy(i => i.Note).Skip(skipPage * pageSize).Take(pageSize).ToList();

            return DbContextHelper.DbContext.SecUserLock
               .AsExpandable()
               .Where(predicate).DefaultIfEmpty()
               .ToList();
        }
        //
        public static SecUserLock Save1(SecUserLock secUserLock)
        {
            if (secUserLock.LockId == 0)
            {

                secUserLock.CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                secUserLock.CreatedOn = DateTime.Now;
                DbContextHelper.DbContext.Add(secUserLock);
            }

            secUserLock.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            secUserLock.UpdatedOn = DateTime.Now;
            DbContextHelper.DbContext.SaveChanges();

            return secUserLock;
        }
        public static List<SecUser> SelectVendor()
        {

            var users = from x in DbContextHelper.DbContext.SecUser
                        where x.RoleId == 3 && !DbContextHelper.DbContext.SecUserLock.Any(e2 => (DateTime.Now >= e2.DateFrom && DateTime.Now <= e2.DateTo))
                        select x;
            return users.ToList();


           

        }
        public static IQueryable<selectedVendor> GetAllSelectedVendor()
        {
            
            var result = from su in DbContextHelper.DbContext.SecUser
                         where
                           su.RoleId == 2 && 
                           !
                          (from secuserlock in DbContextHelper.DbContext.SecUserLock
                           where
                               //SqlFunctions.GetDate() >= secuserlock.DateFrom && SqlFunctions.GetDate() <= secuserlock.DateTo
                           DateTime.Now >= secuserlock.DateFrom && DateTime.Now <= secuserlock.DateTo
                           select new
                           {
                               secuserlock.UserId
                           }).Contains(new { UserId = (Int32?)su.UserId })
                         select new selectedVendor
                         {
                             UserId =su.UserId,
                             UserName= su.UserName,
                             
                         };
            return result;
        }
        public class selectedVendor
        {
            
            public int UserId { get; set; }
            public string UserName { get; set; }

        }
    }
}
