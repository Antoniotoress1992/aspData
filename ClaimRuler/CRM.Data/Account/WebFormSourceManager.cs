using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Linq.Expressions;
using LinqKit;

namespace CRM.Data.Account
{
    public class WebFormSourceManager
    {
        public static List<WebformSourceMaster> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.WebformSourceMasters
                       where x.Status == true
                       select x;

            return list.ToList();
        }

        public static WebformSourceMaster GetByName(string name)
        {

            var webFormSource = from x in DbContextHelper.DbContext.WebformSourceMasters
                            where x.WebformSource == name && x.Status == true
                            select x;

            return webFormSource.Any() ? webFormSource.First() : new WebformSourceMaster();
        }

        public static WebformSourceMaster GetById(int Id)
        {
            var webFormSource = from x in DbContextHelper.DbContext.WebformSourceMasters
                            where x.WebformSourceId == Id && x.Status == true
                            select x;

            return webFormSource.Any() ? webFormSource.First() : new WebformSourceMaster();
        }
        public static bool IsExist(string name, int Id)
        {
            var status = from x in DbContextHelper.DbContext.WebformSourceMasters
                         where x.WebformSource == name && x.WebformSourceId != Id && x.Status == true
                         select x;

            return status.Any();
        }

        public static WebformSourceMaster Save(WebformSourceMaster objWebformSource)
        {
            if (objWebformSource.WebformSourceId == 0)
            {
                //objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objWebformSource.InsertDate = DateTime.Now;
                objWebformSource.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                DbContextHelper.DbContext.AddToWebformSourceMasters(objWebformSource);
            }

            //secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            objWebformSource.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            objWebformSource.UpdateDate = DateTime.Now;
            DbContextHelper.DbContext.SaveChanges();

            return objWebformSource;
        }
    }
}
