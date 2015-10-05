

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

    public class ApplicationConfigurationManager
    {
        public static void UpdateAppConfigStatus()//int roleID SecRoleModule secRoleModule
        {
            var config = from x in DbContextHelper.DbContext.ApplicationConfiguration
                             where x.Status == 1
                             select x;
            foreach (ApplicationConfiguration appConfig in config)
            {
                appConfig.Status = 0;
                appConfig.UpdateBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                appConfig.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                appConfig.UpdateDate = DateTime.Now;
            }
            DbContextHelper.DbContext.SaveChanges();
        }
        public static List<ApplicationConfiguration> GetPredicate(Expression<Func<ApplicationConfiguration, bool>> predicate)
        {
            return DbContextHelper.DbContext.ApplicationConfiguration
               .AsExpandable()
               .Where(predicate)
               .ToList();
        }
        public static List<ApplicationConfiguration> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.ApplicationConfiguration
                       //where x.StateId == StateId
                       where x.Status == 1
                       select x;
            return list.ToList();
        }
        public static ApplicationConfiguration Save(ApplicationConfiguration objAppConfig)
        {
            if (objAppConfig.ApplicationConfigurationId == 0)
            {

                objAppConfig.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objAppConfig.InsertDate = DateTime.Now;
                objAppConfig.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                DbContextHelper.DbContext.Add(objAppConfig);
            }

            //secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            objAppConfig.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            objAppConfig.UpdateDate = DateTime.Now;
            DbContextHelper.DbContext.SaveChanges();

            return objAppConfig;
        }
    }
}
