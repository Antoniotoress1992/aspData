

namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;
    using CRM.Data.Entities;

	public class SecModuleManager {
		public static void Save(SecModule secModule) {
			if (secModule.ModuleId == 0) {
				secModule.CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				secModule.CreatedOn = DateTime.Now;
				secModule.CreatedMachineIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				DbContextHelper.DbContext.Add(secModule);
			}

			secModule.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			secModule.UpdatedOn = DateTime.Now;
			secModule.UpdatedMachineIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			DbContextHelper.DbContext.SaveChanges();
		}

		public static List<SecModule> GetAll() {
			var list = from x in DbContextHelper.DbContext.SecModule
					 where x.Status == true
					 orderby x.ModuleName
					 select x;

			return list.ToList();
		}
		public static List<SecModule> GetAll(int parentID) {
			var list = from x in DbContextHelper.DbContext.SecModule
					 where x.Status == true && x.ParentId == parentID
					 orderby x.ModuleName
					 select x;

			return list.ToList();
		}

		public static List<SecModule> GetModuleType() {
			var list = from x in DbContextHelper.DbContext.SecModule
					 where (x.ModuleType == 0) //&& (x.Status == true)
					 select x;

			return list.ToList();
		}

        public static CRM.Data.Entities.SecModule GetByModuleId(int moduleId)
        {
			var modules = from x in DbContextHelper.DbContext.SecModule
					    where x.ModuleId == moduleId
					    select x;
			return modules.Any() ? modules.First() : new SecModule();
		}

		public static void Delete(SecModule module) {
			DbContextHelper.DbContext.DeleteObject(module);
		}
	}
}
namespace CRM.Data {

	public partial class SecModule {

		public string ParentModuleName {
			get {
				return string.Empty;
                //if (ParentModule == null)
                //    return string.Empty;
                //else
                //    return ParentModule.ModuleName != null ? this.ParentModule.ModuleName : string.Empty;
			}
		}

	}
}
