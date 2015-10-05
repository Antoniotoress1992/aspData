
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
    public class OwnerSameManager
    {
        public static List<OwnerSameMaster> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.OwnerSameMaster
                       where x.Status == true
                       select x;

            return list.ToList();
        }

        public static OwnerSameMaster GetByName(string name)
        {

            var ownerSame = from x in DbContextHelper.DbContext.OwnerSameMaster
                              where x.OwnerSame == name && x.Status == true
                              select x;

            return ownerSame.Any() ? ownerSame.First() : new OwnerSameMaster();
        }

        public static OwnerSameMaster GetById(int ownerSameId)
        {
            var ownerSame = from x in DbContextHelper.DbContext.OwnerSameMaster
                            where x.OwnerSameId == ownerSameId && x.Status == true
                            select x;

            return ownerSame.Any() ? ownerSame.First() : new OwnerSameMaster();
        }
        public static bool IsExist(string name, int ownerSameId)
        {
            var status = from x in DbContextHelper.DbContext.OwnerSameMaster
                         where x.OwnerSame == name && x.OwnerSameId != ownerSameId && x.Status == true
                         select x;

            return status.Any();
        }

        public static OwnerSameMaster Save(OwnerSameMaster objOwnerSame)
        {
            if (objOwnerSame.OwnerSameId == 0)
            {
                //objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                objOwnerSame.InsertDate = DateTime.Now;
                objOwnerSame.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                DbContextHelper.DbContext.Add(objOwnerSame);
            }

            //secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            objOwnerSame.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            objOwnerSame.UpdateDate = DateTime.Now;
            DbContextHelper.DbContext.SaveChanges();

            return objOwnerSame;
        }
    }
}
