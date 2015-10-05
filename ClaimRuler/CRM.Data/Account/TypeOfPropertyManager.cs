

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

    public class TypeOfPropertyManager
    {
        public static List<TypeOfPropertyMaster> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.TypeOfPropertyMaster
                       where x.Status == true
                       select x;

            return list.ToList();
        }

        public static TypeOfPropertyMaster getbyTypeOfProperty(string TypeOfProperty)
        {

            var plcy = from x in DbContextHelper.DbContext.TypeOfPropertyMaster
                       where x.TypeOfProperty == TypeOfProperty && x.Status == true
                       select x;

            return plcy.Any() ? plcy.First() : new TypeOfPropertyMaster();
        }

        public static TypeOfPropertyMaster GetbyTypeOfPropertyId(int TypeOfPropertyId)
        {
            var plcy = from x in DbContextHelper.DbContext.TypeOfPropertyMaster
                       where x.TypeOfPropertyId == TypeOfPropertyId && x.Status == true
                       select x;

            return plcy.Any() ? plcy.First() : new TypeOfPropertyMaster();
        }


        public static bool IsExist(string TypeOfProperty, int TypeOfPropertyId)
        {
            var status = from x in DbContextHelper.DbContext.TypeOfPropertyMaster
                         where x.TypeOfProperty == TypeOfProperty && x.TypeOfPropertyId != TypeOfPropertyId && x.Status == true
                         select x;

            return status.Any();
        }

        public static TypeOfPropertyMaster Save(TypeOfPropertyMaster objTypeOfPropertyMaster)
        {
            if (objTypeOfPropertyMaster.TypeOfPropertyId == 0)
            {
                //objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objTypeOfPropertyMaster.InsertDate = DateTime.Now;
                objTypeOfPropertyMaster.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                DbContextHelper.DbContext.Add(objTypeOfPropertyMaster);
            }

            //secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            objTypeOfPropertyMaster.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            objTypeOfPropertyMaster.UpdateDate = DateTime.Now;
            DbContextHelper.DbContext.SaveChanges();

            return objTypeOfPropertyMaster;
        }
    }
}
