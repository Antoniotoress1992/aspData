

namespace CRM.Data.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Linq.Expressions;
    using LinqKit;

    public class WindPolicyManager
    {
        public static List<WindPolicyMaster> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.WindPolicyMasters
                       where x.Status == true
                       select x;

            return list.ToList();
        }

        public static WindPolicyMaster getbyWindPolicy(string WindPolicy)
        {

            var plcy = from x in DbContextHelper.DbContext.WindPolicyMasters
                       where x.WindPolicy == WindPolicy && x.Status == true
                       select x;

            return plcy.Any() ? plcy.First() : new WindPolicyMaster();
        }

        public static WindPolicyMaster GetbyWindPolicyId(int WindPolicyId)
        {
            var plcy = from x in DbContextHelper.DbContext.WindPolicyMasters
                       where x.WindPolicyId == WindPolicyId && x.Status == true
                       select x;

            return plcy.Any() ? plcy.First() : new WindPolicyMaster();
        }


        public static bool IsExist(string WindPolicy, int WindPolicyId)
        {
            var status = from x in DbContextHelper.DbContext.WindPolicyMasters
                         where x.WindPolicy == WindPolicy && x.WindPolicyId != WindPolicyId && x.Status == true
                         select x;

            return status.Any();
        }

        public static WindPolicyMaster Save(WindPolicyMaster objWindPolicyMaster)
        {
            if (objWindPolicyMaster.WindPolicyId == 0)
            {
                //objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objWindPolicyMaster.InsertDate = DateTime.Now;
                objWindPolicyMaster.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                DbContextHelper.DbContext.AddToWindPolicyMasters(objWindPolicyMaster);
            }

            //secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            objWindPolicyMaster.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            objWindPolicyMaster.UpdateDate = DateTime.Now;
            DbContextHelper.DbContext.SaveChanges();

            return objWindPolicyMaster;
        }
    }
}
