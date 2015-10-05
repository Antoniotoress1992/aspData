

namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;
	using System.Web.UI.WebControls;
    using CRM.Data.Entities;

	public class SecRoleModuleManager {
		public static void Save(SecRoleModule secRoleModule) {
			if (secRoleModule.RoleModuleId == 0) {
				//secRoleModule.CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				secRoleModule.CreatedBy = 1;
				secRoleModule.CreatedOn = DateTime.Now;
				DbContextHelper.DbContext.Add(secRoleModule);
			}

			//secRoleModule.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			secRoleModule.UpdatedBy = 1;
			secRoleModule.UpdatedOn = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();
		}
		public static void SaveRoleModule(SecRoleModule secRoleModule) {
			DbContextHelper.DbContext.Add(secRoleModule);
			DbContextHelper.SaveChanges();
		}
		public static SecRoleModule GetByRoleModuleId(int roleModuleId) {
			var rolemodule = from x in DbContextHelper.DbContext.SecRoleModule
						  where x.RoleModuleId == roleModuleId
						  select x;
			return rolemodule.Any() ? rolemodule.First() : new SecRoleModule();
		}
		public static SecRoleModule GetByRoleId(int roleId) {
			var rolemodule = from x in DbContextHelper.DbContext.SecRoleModule
						  where x.RoleID == roleId //&& x.Status == true
						  select x;
			return rolemodule.Any() ? rolemodule.First() : new SecRoleModule();
			//return new SecRoleModule();
		}
        //get for accounting invoice page

        //NEW OC 10/14/14 ADDED new function to get a record specifically for invoice approval queue
        public static SecRoleModule GetByRoleIdAccounting(int roleId)
        {
            var rolemodule = from x in DbContextHelper.DbContext.SecRoleModule
                             where x.RoleID == roleId && x.ModuleID == 113
                             select x;
            return rolemodule.Any() ? rolemodule.First() : new SecRoleModule();
            //return new SecRoleModule();
        }

		public static void UpdateSecRoleModuleStatus(int roleID) {
			var rolemodule = from x in DbContextHelper.DbContext.SecRoleModule
						  where x.RoleID == roleID //&& x.Status == true
						  select x;

			foreach (SecRoleModule secRoleModule in rolemodule) {
				secRoleModule.Status = 0;
				secRoleModule.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				secRoleModule.UpdatedMachineIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				secRoleModule.UpdatedOn = DateTime.Now;

				// tortega 2013-08-09
				secRoleModule.CreatedOn = DateTime.Now;
			}

			DbContextHelper.DbContext.SaveChanges();
		}

		public static List<SecModule> GetALL()//int roleId
		{
			//var list = from x in DbContextHelper.DbContext.SecModules
			//           where (x.ModuleType == 0) //&& (x.Status == true)
			//           select x;

			//return list.ToList();

			var list = from sm in DbContextHelper.DbContext.SecModule
					 join srm in DbContextHelper.DbContext.SecRoleModule on new { ModuleId = sm.ModuleId } equals new { ModuleId = (Int32)srm.ModuleID } into srm_join
					 from srm in srm_join
					 //where srm.RoleID == roleId
					 select sm;
			//new
			//{
			//    ModuleId = (Int32?)sm.ModuleId,
			//    sm.ModuleName,
			//    AddPermssion = (Boolean?)srm.AddPermssion,
			//    EditPermission = (Boolean?)srm.EditPermission,
			//    DeletePermission = (Boolean?)srm.DeletePermission
			//};
			//return list.ToList();
			List<SecModule> resList = new List<SecModule>();
			SecModule su = null;
			var v = from secmodule in DbContextHelper.DbContext.SecModule
				   join secrolemodule in DbContextHelper.DbContext.SecRoleModule on new { ModuleId = secmodule.ModuleId }
					  equals new { ModuleId = (Int32)secrolemodule.ModuleID }
					  into secrolemodule_join
				   from secrolemodule in secrolemodule_join.DefaultIfEmpty()
				   select new {
					   ModuleId = (Int32?)secmodule.ModuleId,
					   secmodule.ModuleName,
					   secmodule.ModuleDesc,
					   secmodule.ParentId,
					   secmodule.Url,
					   secmodule.ModuleType,
					   secmodule.Status,
					   RoleModuleId = (Int32?)secrolemodule.RoleModuleId,
					   RoleID = (Int32?)secrolemodule.RoleID,
					   AddPermssion = (Boolean?)secrolemodule.AddPermssion,
					   EditPermission = (Boolean?)secrolemodule.EditPermission,
					   DeletePermission = (Boolean?)secrolemodule.DeletePermission
				   };

			foreach (var res in resList) {
				su = new SecModule();

				su.ModuleId = res.ModuleId;
				resList.Add(su);

				//su = new SecModule();
				//su.ModuleId = res.ModuleId;

			}

			return list.ToList();
			//from sm in db.SecModule
			//join srm in db.SecRoleModule on new { ModuleId = sm.ModuleId } equals new { ModuleId = (Int32)srm.ModuleID } into srm_join
			//from srm in srm_join.DefaultIfEmpty()
			//select new {
			//  ModuleId = (Int32?)sm.ModuleId,
			//  sm.ModuleName,
			//  AddPermssion = (Boolean?)srm.AddPermssion,
			//  EditPermission = (Boolean?)srm.EditPermission,
			//  DeletePermission = (Boolean?)srm.DeletePermission
			//}
		}


		public static object getData(int roleID)// int roleID
		{
			//var v = from secmodule in DbContextHelper.DbContext.SecModules
			//        join secrolemodule in DbContextHelper.DbContext.SecRoleModules on new { ModuleId = secmodule.ModuleId }
			//            equals new { ModuleId = (Int32)secrolemodule.ModuleID }
			//            into secrolemodule_join
			//        from secrolemodule in secrolemodule_join.DefaultIfEmpty()
			//        //where secrolemodule.RoleID=roleID
			//        select new
			//        {
			//            ModuleId = (Int32?)secmodule.ModuleId,
			//            secmodule.ModuleName,
			//            secmodule.ModuleDesc,
			//            secmodule.ParentId,
			//            secmodule.Url,
			//            secmodule.ModuleType,
			//            secmodule.Status,
			//            RoleModuleId = (Int32?)secrolemodule.RoleModuleId,
			//            RoleID = (Int32?)secrolemodule.RoleID,
			//            AddPermssion = (Boolean?)secrolemodule.AddPermssion,
			//            EditPermission = (Boolean?)secrolemodule.EditPermission,
			//            DeletePermission = (Boolean?)secrolemodule.DeletePermission
			//        };
			//return v;

			var resList = from secmodule in DbContextHelper.DbContext.SecModule
					    join secrolemodule in DbContextHelper.DbContext.SecRoleModule
							on new { secmodule.ModuleId, RoleID = roleID }
						 equals new { ModuleId = (Int32)secrolemodule.ModuleID, secrolemodule.RoleID } into secrolemodule_join
					    from secrolemodule in secrolemodule_join.DefaultIfEmpty()
					    select
					    new {
						    ModuleId = (Int32?)secmodule.ModuleId,
						    secmodule.ModuleName,
						    secmodule.ModuleDesc,
						    secmodule.ParentId,
						    secmodule.Url,
						    secmodule.ModuleType,
						    secmodule.Status,
						    RoleModuleId = (Int32?)secrolemodule.RoleModuleId,
						    RoleID = (Int32?)secrolemodule.RoleID,
						    AddPermssion = (Boolean?)secrolemodule.AddPermssion,
						    EditPermission = (Boolean?)secrolemodule.EditPermission,
						    DeletePermission = (Boolean?)secrolemodule.DeletePermission
					    };


			return resList.ToList();//.Where(x => (x.ParentId == 0 || x.ParentId == null)).ToList();
		}
		//public static List<vwSecRoleModule> GetModuleInfo()//int roleId
		//{
		//    var result = DbContextHelper.DbContext.vwSecRoleModules.Where(x => (x.ModuleType == 0));
		//    return result.ToList<vwSecRoleModule>();
		//}
		//public static List<vwSecRoleModule> GetModuleInfo1()//int roleId
		//{
		//    var result = DbContextHelper.DbContext.vwSecRoleModules;
		//    return result.ToList<vwSecRoleModule>();
		//}

		public static List<SecRoleModule> getRoleModules(int roleID) {
			List<SecRoleModule> roleModules = null;

			roleModules = (from x in DbContextHelper.DbContext.SecRoleModule.Include("SecModule")
						where x.RoleID == roleID && x.Status == 1
						select x).ToList<SecRoleModule>();

			return roleModules;
		}

		public static void deleteRoleModules(int clientID, int roleID) {
			//DbContextHelper.DbContext.ExecuteStoreCommand("DELETE FROM SecRoleModule WHERE RoleID = {0} AND ClientID = {1}", roleID, clientID);

            var secrole = DbContextHelper.DbContext.SecRoleModule.Where(x => x.RoleID == roleID && x.ClientID ==clientID).ToList();
            foreach (SecRoleModule objR in secrole)
            {
                DbContextHelper.DbContext.DeleteObject(objR);
            }
            DbContextHelper.DbContext.SaveChanges();

		}

		public static List<secRoleModuleGet> getRoleModule(int roleID) {
			List<secRoleModuleGet> resRoleModule = new List<secRoleModuleGet>();
			secRoleModuleGet objsecRoleModuleGet = null;

			var resList = from secmodule in DbContextHelper.DbContext.SecModule
					    join secrolemodule in DbContextHelper.DbContext.SecRoleModule
							on new { secmodule.ModuleId, RoleID = roleID, SecroleModStatus = 1 }
						 equals new { ModuleId = (Int32)secrolemodule.ModuleID, secrolemodule.RoleID, SecroleModStatus = (Int32)secrolemodule.Status } into secrolemodule_join
					    from secrolemodule in secrolemodule_join.DefaultIfEmpty()
					    //join secrolemodules in DbContextHelper.DbContext.SecRoleModules
					    //      on new { secmodule.ModuleId, RoleID = 2, Status = 1 }
					    //  equals new { ModuleId = secrolemodules.ModuleID, secrolemodules.RoleID, secrolemodules.Status } into secrolemodules_join
					    //from secrolemodules in secrolemodules_join.DefaultIfEmpty()
					    where secmodule.Status == true
					    select new {
						    ModuleId = (Int32?)secmodule.ModuleId,
						    secmodule.ModuleName,
						    secmodule.ModuleDesc,
						    secmodule.ParentId,
						    secmodule.Url,
						    secmodule.ModuleType,
						    SecModuleStatus = secmodule.Status,
						    RoleModuleId = (Int32?)secrolemodule.RoleModuleId,
						    RoleID = (Int32?)secrolemodule.RoleID,
						    ViewPermission = (Boolean?)secrolemodule.ViewPermission,
						    AddPermssion = (Boolean?)secrolemodule.AddPermssion,
						    EditPermission = (Boolean?)secrolemodule.EditPermission,
						    DeletePermission = (Boolean?)secrolemodule.DeletePermission,
						    SecRoleModStatus = (Int32?)secrolemodule.Status
							    /*added by asutosh*/
								    ,
						    HasNew = (Boolean?)secmodule.HasNew,
						    HasEdit = (Boolean?)secmodule.HasEdit,
						    HasDelete = (Boolean?)secmodule.HasDelete,
						    SortOrder = secmodule.SortOrder

					    };

			foreach (var get in resList) {
				objsecRoleModuleGet = new secRoleModuleGet();

				objsecRoleModuleGet.ModuleId = (int?)get.ModuleId;
				objsecRoleModuleGet.ModuleName = get.ModuleName;
				objsecRoleModuleGet.ParentId = (int?)get.ParentId;
				objsecRoleModuleGet.Url = get.Url;
				objsecRoleModuleGet.ModuleType = get.ModuleType;
				objsecRoleModuleGet.RoleModuleId = (int?)get.RoleModuleId;
				objsecRoleModuleGet.RoleID = (int?)get.RoleID;
				objsecRoleModuleGet.ViewPermssion = get.ViewPermission ?? false;
				objsecRoleModuleGet.AddPermssion = get.AddPermssion ?? false;
				objsecRoleModuleGet.EditPermission = get.EditPermission ?? false;
				objsecRoleModuleGet.DeletePermission = get.DeletePermission ?? false;
				objsecRoleModuleGet.RoleModuleStatus = (Int32?)get.SecRoleModStatus;

				objsecRoleModuleGet.HasNew = get.HasNew ?? false;
				objsecRoleModuleGet.HasEdit = get.HasEdit ?? false;
				objsecRoleModuleGet.HasDelete = get.HasDelete ?? false;

				objsecRoleModuleGet.SortOrder = get.SortOrder ?? 0;

				resRoleModule.Add(objsecRoleModuleGet);
			}

			return resRoleModule;
			//return resList.ToList();
		}
		public static List<secRoleModuleGet> getRoleModule(int clientID, int roleID) {
			List<secRoleModuleGet> resRoleModule = new List<secRoleModuleGet>();
			secRoleModuleGet objsecRoleModuleGet = null;

			var resList = from secmodule in DbContextHelper.DbContext.SecModule
					    join secrolemodule in DbContextHelper.DbContext.SecRoleModule
							on new {
								secmodule.ModuleId,
								RoleID = roleID,
								SecroleModStatus = 1,
								ClientID = (int)clientID
							}
							equals new {
								ModuleId = (Int32)secrolemodule.ModuleID,
								secrolemodule.RoleID,
								SecroleModStatus = (Int32)secrolemodule.Status,
								ClientID = (int)secrolemodule.ClientID
							} into secrolemodule_join
					    from secrolemodule in secrolemodule_join.DefaultIfEmpty()
					    where secmodule.Status == true && secmodule.IsSystem == false
					    select new {
						    ModuleId = (Int32?)secmodule.ModuleId,
						    secmodule.ModuleName,
						    secmodule.ModuleDesc,
						    secmodule.ParentId,
						    secmodule.Url,
						    secmodule.ModuleType,
						    SecModuleStatus = secmodule.Status,
						    RoleModuleId = (Int32?)secrolemodule.RoleModuleId,
						    RoleID = (Int32?)secrolemodule.RoleID,
						    ViewPermission = (Boolean?)secrolemodule.ViewPermission,
						    AddPermssion = (Boolean?)secrolemodule.AddPermssion,
						    EditPermission = (Boolean?)secrolemodule.EditPermission,
						    DeletePermission = (Boolean?)secrolemodule.DeletePermission,
						    SecRoleModStatus = (Int32?)secrolemodule.Status,

						    /*added by asutosh*/
						    HasNew = (Boolean?)secmodule.HasNew,
						    HasEdit = (Boolean?)secmodule.HasEdit,
						    HasDelete = (Boolean?)secmodule.HasDelete,

						    SortOrder = secmodule.SortOrder,
						    ClientID = secrolemodule.ClientID

					    };

			foreach (var get in resList) {
				objsecRoleModuleGet = new secRoleModuleGet();

				objsecRoleModuleGet.ModuleId = (int?)get.ModuleId;
				objsecRoleModuleGet.ModuleName = get.ModuleName;
				objsecRoleModuleGet.ParentId = (int?)get.ParentId;
				objsecRoleModuleGet.Url = get.Url;
				objsecRoleModuleGet.ModuleType = get.ModuleType;
				objsecRoleModuleGet.RoleModuleId = (int?)get.RoleModuleId;
				objsecRoleModuleGet.RoleID = (int?)get.RoleID;
				objsecRoleModuleGet.ViewPermssion = get.ViewPermission ?? false;
				objsecRoleModuleGet.AddPermssion = get.AddPermssion ?? false;
				objsecRoleModuleGet.EditPermission = get.EditPermission ?? false;
				objsecRoleModuleGet.DeletePermission = get.DeletePermission ?? false;
				objsecRoleModuleGet.RoleModuleStatus = (Int32?)get.SecRoleModStatus;

				objsecRoleModuleGet.HasNew = get.HasNew ?? false;
				objsecRoleModuleGet.HasEdit = get.HasEdit ?? false;
				objsecRoleModuleGet.HasDelete = get.HasDelete ?? false;

				objsecRoleModuleGet.SortOrder = get.SortOrder ?? 0;
				objsecRoleModuleGet.ClientID = get.ClientID ?? 0;

				resRoleModule.Add(objsecRoleModuleGet);
			}

			return resRoleModule;
			//return resList.ToList();
		}

		/// <summary>
		/// This is used to prime database
		/// </summary>
		/// <param name="roleID"></param>
		/// <returns></returns>
		public static List<secRoleModuleGet> primeRoleModule(int roleID) {
			List<secRoleModuleGet> resRoleModule = new List<secRoleModuleGet>();
			secRoleModuleGet objsecRoleModuleGet = null;

			var resList = from secmodule in DbContextHelper.DbContext.SecModule
					    //join secrolemodule in DbContextHelper.DbContext.SecRoleModules
					    //	 on new { secmodule.ModuleId, RoleID = roleID, SecroleModStatus = 1 }
					    //	 equals new { ModuleId = (Int32)secrolemodule.ModuleID, secrolemodule.RoleID, SecroleModStatus = (Int32)secrolemodule.Status } into secrolemodule_join
					    //from secrolemodule in secrolemodule_join.DefaultIfEmpty()
					    where secmodule.Status == true
					    select new {
						    ModuleId = (Int32?)secmodule.ModuleId,
						    secmodule.ModuleName,
						    secmodule.ModuleDesc,
						    secmodule.ParentId,
						    secmodule.Url,
						    secmodule.ModuleType,
						    SecModuleStatus = secmodule.Status,
						    //RoleModuleId = (Int32?)secrolemodule.RoleModuleId,
						    RoleID = roleID,
						    ViewPermission = false,
						    AddPermssion = false,
						    EditPermission = false,
						    DeletePermission = false,
						    SecRoleModStatus = 1, //(Int32?)secrolemodule.Status,

						    /*added by asutosh*/
						    HasNew = false,
						    HasEdit = false,
						    HasDelete = false,

						    SortOrder = secmodule.SortOrder,
					    };

			foreach (var get in resList) {
				objsecRoleModuleGet = new secRoleModuleGet();

				objsecRoleModuleGet.ModuleId = (int?)get.ModuleId;
				objsecRoleModuleGet.ModuleName = get.ModuleName;
				objsecRoleModuleGet.ParentId = (int?)get.ParentId;
				objsecRoleModuleGet.Url = get.Url;
				objsecRoleModuleGet.ModuleType = get.ModuleType;
				//objsecRoleModuleGet.RoleModuleId = (int?)get.RoleModuleId;
				objsecRoleModuleGet.RoleID = (int?)get.RoleID;
				objsecRoleModuleGet.ViewPermssion = get.ViewPermission;
				objsecRoleModuleGet.AddPermssion = get.AddPermssion;
				objsecRoleModuleGet.EditPermission = get.EditPermission;
				objsecRoleModuleGet.DeletePermission = get.DeletePermission;
				objsecRoleModuleGet.RoleModuleStatus = (Int32?)get.SecRoleModStatus;

				objsecRoleModuleGet.HasNew = get.HasNew;
				objsecRoleModuleGet.HasEdit = get.HasEdit;
				objsecRoleModuleGet.HasDelete = get.HasDelete;

				objsecRoleModuleGet.SortOrder = get.SortOrder ?? 0;
				//objsecRoleModuleGet.ClientID = get.ClientID ?? 0;

				resRoleModule.Add(objsecRoleModuleGet);
			}

			return resRoleModule;
			//return resList.ToList();
		}

		public static List<secRoleModuleGet> getRoleModuleMenu(int clientID, int roleID) {
			List<secRoleModuleGet> resRoleModule = new List<secRoleModuleGet>();


			resRoleModule = (from secmodule in DbContextHelper.DbContext.SecModule
						  join secrolemodule in DbContextHelper.DbContext.SecRoleModule
							   on secmodule.ModuleId equals secrolemodule.ModuleID
						  where secmodule.Status == true &&
							   secrolemodule.ClientID == clientID &&
							   secrolemodule.RoleID == roleID &&
							   secrolemodule.Status == 1
						  select new secRoleModuleGet {
							  ModuleId = (Int32?)secmodule.ModuleId,
							  ModuleName = secmodule.ModuleName,
							  ModuleDesc = secmodule.ModuleDesc,
							  ParentId = secmodule.ParentId,
							  Url = secmodule.Url,
							  ModuleType = secmodule.ModuleType,
							  //= secmodule.Status,
							  RoleModuleId = (Int32?)secrolemodule.RoleModuleId,
							  RoleID = (Int32?)secrolemodule.RoleID,
							  ViewPermssion = (Boolean?)secrolemodule.ViewPermission,
							  AddPermssion = (Boolean?)secrolemodule.AddPermssion,
							  EditPermission = (Boolean?)secrolemodule.EditPermission,
							  DeletePermission = (Boolean?)secrolemodule.DeletePermission,
							  RoleModuleStatus = (Int32?)secrolemodule.Status,

							  /*added by asutosh*/
							  HasNew = (Boolean?)secmodule.HasNew,
							  HasEdit = (Boolean?)secmodule.HasEdit,
							  HasDelete = (Boolean?)secmodule.HasDelete,

							  SortOrder = secmodule.SortOrder,
							  ClientID = secrolemodule.ClientID ?? 0

						  }).ToList<secRoleModuleGet>();



			return resRoleModule;

		}
		public static List<secRoleModuleGet> getRoleModuleMenu(int roleID) {
			List<secRoleModuleGet> resRoleModule = null;
			//secRoleModuleGet objsecRoleModuleGet = null;

			resRoleModule = (from secmodule in DbContextHelper.DbContext.SecModule
						  join secrolemodule in DbContextHelper.DbContext.SecRoleModule
							   on new { secmodule.ModuleId, RoleID = roleID, SecroleModStatus = 1 }
						    equals new { ModuleId = (Int32)secrolemodule.ModuleID, secrolemodule.RoleID, SecroleModStatus = (Int32)secrolemodule.Status } into secrolemodule_join
						  from secrolemodule in secrolemodule_join.DefaultIfEmpty()

						  where secmodule.Status == true && secrolemodule.RoleID == roleID

						  orderby secmodule.SortOrder

						  select new secRoleModuleGet {
							  ModuleId = (Int32?)secmodule.ModuleId,
							  ModuleName = secmodule.ModuleName,
							  ModuleDesc = secmodule.ModuleDesc,
							  ParentId = (int?)secmodule.ParentId,
							  Url = secmodule.Url,
							  ModuleType = (int?)secmodule.ModuleType,
							  RoleModuleId = (Int32?)secrolemodule.RoleModuleId,
							  RoleID = (Int32?)secrolemodule.RoleID,
							  ViewPermssion = (Boolean?)secrolemodule.ViewPermission,
							  AddPermssion = (Boolean?)secrolemodule.AddPermssion,
							  EditPermission = (Boolean?)secrolemodule.EditPermission,
							  DeletePermission = (Boolean?)secrolemodule.DeletePermission,
							  RoleModuleStatus = (Int32?)secrolemodule.Status
								  /*added by asutosh*/
									  ,
							  HasNew = (Boolean?)secmodule.HasNew,
							  HasEdit = (Boolean?)secmodule.HasEdit,
							  HasDelete = (Boolean?)secmodule.HasDelete,

							  // tortega 2013-11-07
							  SortOrder = secmodule.SortOrder

						  }).ToList();

			//foreach (var get in resList) {
			//	objsecRoleModuleGet = new secRoleModuleGet();

			//	objsecRoleModuleGet.ModuleId = (int?)get.ModuleId;
			//	objsecRoleModuleGet.ModuleName = get.ModuleName;
			//	objsecRoleModuleGet.ParentId = (int?)get.ParentId;
			//	objsecRoleModuleGet.Url = get.Url;
			//	objsecRoleModuleGet.ModuleType = get.ModuleType;
			//	objsecRoleModuleGet.RoleModuleId = (int?)get.RoleModuleId;
			//	objsecRoleModuleGet.RoleID = (int?)get.RoleID;
			//	objsecRoleModuleGet.ViewPermssion = (Boolean?)get.ViewPermission;
			//	objsecRoleModuleGet.AddPermssion = (Boolean?)get.AddPermssion;
			//	objsecRoleModuleGet.EditPermission = (Boolean?)get.EditPermission;
			//	objsecRoleModuleGet.DeletePermission = (Boolean?)get.DeletePermission;
			//	objsecRoleModuleGet.RoleModuleStatus = (Int32?)get.SecRoleModStatus;

			//	objsecRoleModuleGet.HasNew = (Boolean?)get.HasNew;
			//	objsecRoleModuleGet.HasEdit = (Boolean?)get.HasEdit;
			//	objsecRoleModuleGet.HasDelete = (Boolean?)get.HasDelete;

			//	objsecRoleModuleGet.SortOrder = get.SortOrder;
			//	resRoleModule.Add(objsecRoleModuleGet);
			//}

			return resRoleModule;
			//return resList.ToList();
		}

		public static List<secRoleModuleGet> getRolePermission(int roleID) {
			List<secRoleModuleGet> resRoleModule = new List<secRoleModuleGet>();
			secRoleModuleGet objsecRoleModuleGet = null;

			var resList = from sm in DbContextHelper.DbContext.SecModule
					    join srm in DbContextHelper.DbContext.SecRoleModule
							on new { sm.ModuleId, RoleID = roleID, SecroleModStatus = 1 }
						 equals new { ModuleId = (Int32)srm.ModuleID, srm.RoleID, SecroleModStatus = (Int32)srm.Status }
					    //where sm.Status == true
					    //
					    select new {
						    ModuleId = (Int32?)sm.ModuleId,
						    sm.ModuleName,
						    sm.ModuleDesc,
						    sm.ParentId,
						    sm.Url,
						    sm.ModuleType,
						    SecModuleStatus = sm.Status,
						    RoleModuleId = (Int32?)srm.RoleModuleId,
						    RoleID = (Int32?)srm.RoleID,
						    ViewPermission = (Boolean?)srm.ViewPermission,
						    AddPermssion = (Boolean?)srm.AddPermssion,
						    EditPermission = (Boolean?)srm.EditPermission,
						    DeletePermission = (Boolean?)srm.DeletePermission,
						    SecRoleModStatus = (Int32?)srm.Status
					    };

			foreach (var get in resList) {
				objsecRoleModuleGet = new secRoleModuleGet();

				objsecRoleModuleGet.ModuleId = (int?)get.ModuleId;
				objsecRoleModuleGet.ModuleName = get.ModuleName;
				objsecRoleModuleGet.ParentId = (int?)get.ParentId;		

				objsecRoleModuleGet.Url = get.Url;
				objsecRoleModuleGet.ModuleType = get.ModuleType;
				objsecRoleModuleGet.RoleModuleId = (int?)get.RoleModuleId;
				objsecRoleModuleGet.RoleID = (int?)get.RoleID;
				objsecRoleModuleGet.ViewPermssion = (Boolean?)get.ViewPermission;
				objsecRoleModuleGet.AddPermssion = (Boolean?)get.AddPermssion;
				objsecRoleModuleGet.EditPermission = (Boolean?)get.EditPermission;
				objsecRoleModuleGet.DeletePermission = (Boolean?)get.DeletePermission;
				objsecRoleModuleGet.RoleModuleStatus = (Int32?)get.SecRoleModStatus;

				resRoleModule.Add(objsecRoleModuleGet);
			}

			return resRoleModule;
		}


		public class secRoleModuleGet {
			public int? ModuleId { get; set; }
			public string ModuleName { get; set; }
			public string ModuleDesc { get; set; }
			public int? ParentId { get; set; }
			public string Url { get; set; }
			public int? ModuleType { get; set; }
			public int? RoleModuleId { get; set; }
			public int? RoleID { get; set; }
			public Boolean? ViewPermssion { get; set; }
			public Boolean? AddPermssion { get; set; }
			public Boolean? EditPermission { get; set; }
			public Boolean? DeletePermission { get; set; }
			public int? RoleModuleStatus { get; set; }

			/*added by ashutosh*/
			public Boolean? HasNew { get; set; }
			public Boolean? HasEdit { get; set; }
			public Boolean? HasDelete { get; set; }

			// tortega - 2013-11-07
			public int? SortOrder { get; set; }

			public int ClientID { get; set; }
		}
	}
}
