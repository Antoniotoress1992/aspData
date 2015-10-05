

namespace CRM.Data.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using CRM.Data.Entities;
    using System.Linq.Expressions;
    using LinqKit;

    public class State
    {
        //public static List<StateMaster> GetAll(int countryid)
        //{
        //    var list = from x in DbContextHelper.DbContext.StateMasters
        //               where x.country_id == (countryid == 0 ? x.country_id : countryid)
        //               select x;
        //    return list.ToList();
        //    //return null;
        //}
        public static List<StateMaster> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.StateMaster
                       where x.Status==1
                       select x;
            return list.ToList();
            //return null;
        }

        public static StateMaster Getstateid(string statename)
        {
            var _member = from x in DbContextHelper.DbContext.StateMaster
					 where x.StateName == statename || x.StateCode == statename
                          select x;
            return _member.Any() ? _member.First() : new StateMaster();
        }
    }
}
