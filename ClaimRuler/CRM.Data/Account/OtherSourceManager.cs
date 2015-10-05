

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

    public class OtherSourceManager
    {
        public static List<OtherSourceMaster> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.OtherSourceMaster
                       where x.Status == true
                       select x;

            return list.ToList();
        }

        public static OtherSourceMaster getbyOtherSource(string OtherSource)
        {

            var plcy = from x in DbContextHelper.DbContext.OtherSourceMaster
                       where x.OtherSource == OtherSource && x.Status == true
                       select x;

            return plcy.Any() ? plcy.First() : new OtherSourceMaster();
        }

        public static OtherSourceMaster GetbyOtherSourceId(int OtherSourceId)
        {
            var plcy = from x in DbContextHelper.DbContext.OtherSourceMaster
                       where x.OtherSourceId == OtherSourceId && x.Status == true
                       select x;

            return plcy.Any() ? plcy.First() : new OtherSourceMaster();
        }


        public static bool IsExist(string OtherSource, int OtherSourceId)
        {
            var status = from x in DbContextHelper.DbContext.OtherSourceMaster
                         where x.OtherSource == OtherSource && x.OtherSourceId != OtherSourceId && x.Status == true
                         select x;

            return status.Any();
        }

        public static OtherSourceMaster Save(OtherSourceMaster objOtherSourceMaster)
        {
            if (objOtherSourceMaster.OtherSourceId == 0)
            {
                //objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objOtherSourceMaster.InsertDate = DateTime.Now;
                objOtherSourceMaster.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                DbContextHelper.DbContext.Add(objOtherSourceMaster);
            }

            //secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            objOtherSourceMaster.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            objOtherSourceMaster.UpdateDate = DateTime.Now;
            DbContextHelper.DbContext.SaveChanges();

            return objOtherSourceMaster;
        }
    }
}

