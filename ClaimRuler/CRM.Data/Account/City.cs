

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

    public class City
    {
        public static List<CityMaster> GetAll(int StateId)
        {
            var list = from x in DbContextHelper.DbContext.CityMaster
                       where x.StateId == StateId
                       select x;
            return list.ToList();
        }

        public static CityMaster GetByCityName(string Name)
        {

            var users = from x in DbContextHelper.DbContext.CityMaster
                        where x.CityName == Name && x.Status == 1
                        select x;

            return users.Any() ? users.First() : new CityMaster();
        }

        
    }
}
