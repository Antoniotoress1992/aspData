
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

    public class LeadReportLogManager
    {
        public static void Save(LeadReportGenerateLog objLeadReportGenerateLog)
        {
            if (objLeadReportGenerateLog.LeadReportGenerateId == 0)
            {
                //secModule.CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objLeadReportGenerateLog.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objLeadReportGenerateLog.InsertDate = DateTime.Now;
                objLeadReportGenerateLog.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                DbContextHelper.DbContext.Add(objLeadReportGenerateLog);
            }
            DbContextHelper.DbContext.SaveChanges();
        }
    }
}
