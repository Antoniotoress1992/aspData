

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

    public class ReportedToInsurerManager
    {
        public static List<ReportedToInsurerMaster> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.ReportedToInsurerMaster
                       where x.Status == true
                       select x;

            return list.ToList();
        }

        public static ReportedToInsurerMaster GetByReportedToInsurer(string ReportedToInsurer)
        {

            var plcy = from x in DbContextHelper.DbContext.ReportedToInsurerMaster
                       where x.ReportedToInsurer == ReportedToInsurer && x.Status == true
                       select x;

            return plcy.Any() ? plcy.First() : new ReportedToInsurerMaster();
        }

        public static ReportedToInsurerMaster GetReportedToInsurerId(int ReportedToInsurerId)
        {
            var plcy = from x in DbContextHelper.DbContext.ReportedToInsurerMaster
                       where x.ReportedToInsurerId == ReportedToInsurerId && x.Status == true
                       select x;

            return plcy.Any() ? plcy.First() : new ReportedToInsurerMaster();
        }


        public static bool IsExist(string ReportedToInsurer, int ReportedToInsurerId)
        {
            var status = from x in DbContextHelper.DbContext.ReportedToInsurerMaster
                         where x.ReportedToInsurer == ReportedToInsurer && x.ReportedToInsurerId != ReportedToInsurerId && x.Status == true
                         select x;

            return status.Any();
        }

        public static ReportedToInsurerMaster Save(ReportedToInsurerMaster objReportedToInsurerMaster)
        {
            if (objReportedToInsurerMaster.ReportedToInsurerId == 0)
            {
                //objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objReportedToInsurerMaster.InsertDate = DateTime.Now;
                objReportedToInsurerMaster.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                DbContextHelper.DbContext.Add(objReportedToInsurerMaster);
            }

            //secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            objReportedToInsurerMaster.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            objReportedToInsurerMaster.UpdateDate = DateTime.Now;
            DbContextHelper.DbContext.SaveChanges();

            return objReportedToInsurerMaster;
        }
    }
}
