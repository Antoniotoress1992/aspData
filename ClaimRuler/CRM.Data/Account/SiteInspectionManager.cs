
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Linq.Expressions;
using LinqKit;
using CRM.Data.Entities;

namespace CRM.Data.Account
{
    public class SiteInspectionManager
    {
        public static List<SiteInspectionCompleteMaster> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.SiteInspectionCompleteMaster
                       where x.Status == true
                       select x;

            return list.ToList();
        }

        public static SiteInspectionCompleteMaster GetByName(string name)
        {

            var siteInspection = from x in DbContextHelper.DbContext.SiteInspectionCompleteMaster
                              where x.SiteInspectionCompleteName == name && x.Status == true
                              select x;

            return siteInspection.Any() ? siteInspection.First() : new SiteInspectionCompleteMaster();
        }

        public static SiteInspectionCompleteMaster GetById(int Id)
        {
            var siteInspection = from x in DbContextHelper.DbContext.SiteInspectionCompleteMaster
                              where x.SiteInspectionCompleteId == Id && x.Status == true
                              select x;

            return siteInspection.Any() ? siteInspection.First() : new SiteInspectionCompleteMaster();
        }
        public static bool IsExist(string name, int id)
        {
            var status = from x in DbContextHelper.DbContext.SiteInspectionCompleteMaster
                         where x.SiteInspectionCompleteName == name && x.SiteInspectionCompleteId != id && x.Status == true
                         select x;

            return status.Any();
        }

        public static SiteInspectionCompleteMaster Save(SiteInspectionCompleteMaster objSiteInspectionComplete)
        {
            if (objSiteInspectionComplete.SiteInspectionCompleteId == 0)
            {
                //objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objSiteInspectionComplete.InsertDate = DateTime.Now;
                objSiteInspectionComplete.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                DbContextHelper.DbContext.Add(objSiteInspectionComplete);
            }

            //secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            objSiteInspectionComplete.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            objSiteInspectionComplete.UpdateDate = DateTime.Now;
            DbContextHelper.DbContext.SaveChanges();

            return objSiteInspectionComplete;
        }
    }
}
