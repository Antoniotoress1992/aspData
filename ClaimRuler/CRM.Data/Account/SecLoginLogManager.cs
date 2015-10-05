
namespace CRM.Data.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Linq.Expressions;
    using LinqKit;
    using CRM.Data.Entities;

   public  class SecLoginLogManager
    {
       public static void Save(SecLoginLog secloginlogMaster)
       {
           /*changed after DB changes*/
           //if (secloginlogMaster.logid == 0)
           if (secloginlogMaster.LogId == 0)
           {

               DbContextHelper.DbContext.Add(secloginlogMaster);
           }
           
           DbContextHelper.DbContext.SaveChanges();
       }

       public static void Update(SecLoginLog secloginlogMaster)
       {
           /*changed after DB changes*/
            // var querySecLoginLog =
            //from secloginlog in DbContextHelper.DbContext.SecLoginLogs
            //where

            //    ((from secloginlog0 in DbContextHelper.DbContext.SecLoginLogs
            //      where
            //        secloginlog0.userid == secloginlogMaster.userid
            //      orderby
            //        secloginlog0.logid descending
            //      select new
            //      {
            //          secloginlog0.logid
            //      }).Take(1)).Contains(new { secloginlog.logid })
            //      select secloginlog;
            //       foreach (var secloginlog in querySecLoginLog)
            //       {
            //           secloginlog.logouttime = DateTime.Now;
            //       }
            //       DbContextHelper.DbContext.SaveChanges();var querySecLoginLog =
            var querySecLoginLog =
            from secloginlog in DbContextHelper.DbContext.SecLoginLog
            where

                ((from secloginlog0 in DbContextHelper.DbContext.SecLoginLog
                  where
                    secloginlog0.UserId == secloginlogMaster.UserId
                  orderby
                    secloginlog0.LogId descending
                  select new
                  {
                      secloginlog0.LogId
                  }).Take(1)).Contains(new { secloginlog.LogId })
                  select secloginlog;
                   foreach (var secloginlog in querySecLoginLog)
                   {
                       secloginlog.LogoutTime = DateTime.Now;

                   }
                   DbContextHelper.DbContext.SaveChanges();
       }
       public static List<loginLog> GetLoginLogDetail(string userName, string roleName, DateTime dateFrom, DateTime dateTo)
       {
           List<loginLog> resloginLog = new List<loginLog>();
           loginLog objloginLogGet = null;

           if (dateFrom == Convert.ToDateTime("01/01/1900") && dateTo == Convert.ToDateTime("01/01/1900"))
           {
               var result = from userLock in DbContextHelper.DbContext.SecLoginLog
                            join user in DbContextHelper.DbContext.SecUser on userLock.UserId equals user.UserId
                            join role in DbContextHelper.DbContext.SecRole on user.RoleId equals role.RoleId
                            where (user.UserName == (userName == string.Empty ? user.UserName : userName)
                            || role.RoleName == (userName == string.Empty ? role.RoleName : userName))
                            orderby user.UserId
                            select new
                            {
                                UserName = user.UserName,
                                RoleName = role.RoleName,
                                UserId = userLock.UserId,
                                LoginTime = userLock.LoginTime,
                                LogoutTime = userLock.LogoutTime,
                                Sucess = userLock.Sucess
                            };

               foreach (var get in result)
               {
                   objloginLogGet = new loginLog();


                   objloginLogGet.UserId = (int?)get.UserId;
                   objloginLogGet.UserName = get.UserName;
                   objloginLogGet.RoleName = get.RoleName;
                   objloginLogGet.LoginTime = get.LoginTime;
                   objloginLogGet.LogoutTime = get.LogoutTime;
                   resloginLog.Add(objloginLogGet);
               }
           }
           else
           {
               string DateFrom = dateFrom.ToShortDateString();
               var dtf=dateFrom.ToShortDateString();
               //DateTime dtf = new DateTime(Convert.ToInt32(DateFrom.Substring(6, 4)), Convert.ToInt32(DateFrom.Substring(3, 2)), Convert.ToInt32(DateFrom.Substring(0, 2)));
               DateTime rangeStart = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
               DateTime rangeEnd = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

               var result = from userLock in DbContextHelper.DbContext.SecLoginLog
                            join user in DbContextHelper.DbContext.SecUser on userLock.UserId equals user.UserId
                            join role in DbContextHelper.DbContext.SecRole on user.RoleId equals role.RoleId
                            where (user.UserName == (userName == string.Empty ? user.UserName : userName)
                            || role.RoleName == (userName == string.Empty ? role.RoleName : userName))
                            && userLock.LoginTime >= rangeStart
                            && userLock.LogoutTime <= rangeEnd
                            //&& userLock.LogoutTime >= dateFrom 
                            //&& userLock.LogoutTime <= dateTo 
                            orderby user.UserId
                            select new
                            {
                                UserName = user.UserName,
                                RoleName = role.RoleName,
                                UserId = userLock.UserId,
                                LoginTime = userLock.LoginTime,
                                LogoutTime = userLock.LogoutTime,
                                Sucess = userLock.Sucess
                            };

               foreach (var get in result)
               {
                   objloginLogGet = new loginLog();


                   objloginLogGet.UserId = (int?)get.UserId;
                   objloginLogGet.UserName = get.UserName;
                   objloginLogGet.RoleName = get.RoleName;
                   objloginLogGet.LoginTime = get.LoginTime;
                   objloginLogGet.LogoutTime = get.LogoutTime;
                   resloginLog.Add(objloginLogGet);
               }
           }
           
           
           return resloginLog;
       }

       //public static List<VendorLockDetails> getVendorLockDetails(string Name, DateTime dtFrom, DateTime dtTo)
       //{
       //    List<VendorLockDetails> objList = new List<VendorLockDetails>();
       //    VendorLockDetails objLockDetails = null;
       //    if (dtFrom == Convert.ToDateTime("01/01/1900") && dtTo == Convert.ToDateTime("01/01/1900"))
       //    {
       //        var result = from userLock in DbContextHelper.DbContext.SecUserLocks
       //                     join user in DbContextHelper.DbContext.SecUsers on userLock.UserId equals user.UserId
       //                     join vendor in DbContextHelper.DbContext.VendorMasters on userLock.UserId equals vendor.UserId
       //                     join user1 in DbContextHelper.DbContext.SecUsers on vendor.CreatedBy equals user1.UserId 
       //                     where (user.UserName == (Name == string.Empty ? user.UserName : Name)
       //                     || vendor.VendorName == (Name == string.Empty ? vendor.VendorName : Name))
       //                     orderby user.UserId
       //                     select new
       //                     {
       //                         user.UserId,
       //                         user1.UserName,
       //                         vendor.VendorName,
       //                         userLock.DateFrom,
       //                         userLock.DateTo
       //                     };
       //        foreach (var get in result)
       //        {
       //            objLockDetails = new VendorLockDetails();


       //            objLockDetails.UserId = (int?)get.UserId;
       //            objLockDetails.UserName = get.UserName;
       //            objLockDetails.VendorName = get.VendorName;
       //            objLockDetails.DateFrom = get.DateFrom;
       //            objLockDetails.DateTO = get.DateTo;

       //            objList.Add(objLockDetails);

       //        }
       //    }
       //    else
       //    {
       //        DateTime rangeStart = new DateTime(dtFrom.Year, dtFrom.Month, dtFrom.Day, 0, 0, 0);
       //        DateTime rangeEnd = new DateTime(dtTo.Year, dtTo.Month, dtTo.Day, 23, 59, 59);

       //        var result = from userLock in DbContextHelper.DbContext.SecUserLocks
       //                     join user in DbContextHelper.DbContext.SecUsers on userLock.UserId equals user.UserId
       //                     join vendor in DbContextHelper.DbContext.VendorMasters on userLock.UserId equals vendor.UserId
       //                     where (user.UserName == (Name == string.Empty ? user.UserName : Name)
       //                     || vendor.VendorName == (Name == string.Empty ? vendor.VendorName : Name))
       //                     && userLock.DateFrom >= rangeStart
       //                     && userLock.DateTo <= rangeEnd
       //                     orderby user.UserId
       //                     select new
       //                     {
       //                         user.UserId,
       //                         user.UserName,
       //                         vendor.VendorName,
       //                         userLock.DateFrom,
       //                         userLock.DateTo
       //                     };
       //        foreach (var get in result)
       //        {
       //            objLockDetails = new VendorLockDetails();


       //            objLockDetails.UserId = (int?)get.UserId;
       //            objLockDetails.UserName = get.UserName;
       //            objLockDetails.VendorName = get.VendorName;
       //            objLockDetails.DateFrom = get.DateFrom;
       //            objLockDetails.DateTO = get.DateTo;

       //            objList.Add(objLockDetails);

       //        }
       //    }

       //    return objList;
       //}

       


       public static IQueryable<loginLog> GetLoginLog()
       {
           var result = from userLock in DbContextHelper.DbContext.SecLoginLog
                        join user in DbContextHelper.DbContext.SecUser on userLock.UserId equals user.UserId
                        orderby user.UserId
                        select new loginLog
                        {
                            UserName = user.UserName,
                            RoleName = user.SecRole.RoleName,
                            UserId = userLock.UserId,
                            LoginTime = userLock.LoginTime,
                            LogoutTime = userLock.LogoutTime,
                            Sucess = userLock.Sucess
                        };
           //foreach (var get in result)
           //{
           //    objsecRoleModuleGet = new secRoleModuleGet();

           //    objsecRoleModuleGet.ModuleId = (int?)get.ModuleId;
           //    objsecRoleModuleGet.ModuleName = get.ModuleName;
           //    objsecRoleModuleGet.ParentId = (int?)get.ParentId;
           //    objsecRoleModuleGet.Url = get.Url;
           //    objsecRoleModuleGet.ModuleType = get.ModuleType;
           //    objsecRoleModuleGet.RoleModuleId = (int?)get.RoleModuleId;
           //    objsecRoleModuleGet.RoleID = (int?)get.RoleID;
           //    objsecRoleModuleGet.ViewPermssion = (Boolean?)get.ViewPermission;
           //    objsecRoleModuleGet.AddPermssion = (Boolean?)get.AddPermssion;
           //    objsecRoleModuleGet.EditPermission = (Boolean?)get.EditPermission;
           //    objsecRoleModuleGet.DeletePermission = (Boolean?)get.DeletePermission;
           //    objsecRoleModuleGet.RoleModuleStatus = (Int32?)get.SecRoleModStatus;

           //    resRoleModule.Add(objsecRoleModuleGet);
           //}

           //return resRoleModule;
           return result;
       }
       public class loginLog
       {
           public int? UserId { get; set; }
           public string UserName { get; set; }
           public string RoleName { get; set; }
           public DateTime LoginTime { get; set; }
           public DateTime LogoutTime { get; set; }
           public Boolean? Sucess { get; set; }

       }

       public class VendorLockDetails
       {
           public int? UserId { get; set; }
           public string UserName { get; set; }
           public string VendorName { get; set; }
           public DateTime? DateFrom { get; set; }
           public DateTime? DateTO { get; set; }

       }
    }
}
