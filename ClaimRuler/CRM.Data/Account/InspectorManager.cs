
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
    public class InspectorManager
    {
        public static List<InspectorMaster> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.InspectorMaster
                       where x.Status == true
                       select x;

            return list.ToList();
        }

        public static InspectorMaster GetByName(string name)
        {

            var inspector = from x in DbContextHelper.DbContext.InspectorMaster
                              where x.InspectorName == name && x.Status == true
                              select x;

            return inspector.Any() ? inspector.First() : new InspectorMaster();
        }

        public static InspectorMaster GetById(int inspectorId)
        {
            var inspector = from x in DbContextHelper.DbContext.InspectorMaster
                            where x.InspectorId == inspectorId && x.Status == true
                            select x;

            return inspector.Any() ? inspector.First() : new InspectorMaster();
        }
        public static bool IsExist(string name, int inspectorId)
        {
            var status = from x in DbContextHelper.DbContext.InspectorMaster
                         where x.InspectorName == name && x.InspectorId != inspectorId && x.Status == true
                         select x;

            return status.Any();
        }

        public static InspectorMaster Save(InspectorMaster objInspector)
        {
            if (objInspector.InspectorId == 0)
            {
                //objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objInspector.InsertDate = DateTime.Now;
                objInspector.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                DbContextHelper.DbContext.Add(objInspector);
            }

            //secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            objInspector.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            objInspector.UpdateDate = DateTime.Now;
            DbContextHelper.DbContext.SaveChanges();

            return objInspector;
        }
    }
}
