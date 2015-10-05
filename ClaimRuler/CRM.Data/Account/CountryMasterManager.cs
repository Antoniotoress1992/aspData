

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

    public class CountryMasterManager
    {
        public static void Save(CountryMaster countryMaster)
        {
            if (countryMaster.CountryID == 0)
            {
                //secModule.CreatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                DbContextHelper.DbContext.Add(countryMaster);
            }

            DbContextHelper.DbContext.SaveChanges();
        }
        public static CountryMaster GetByCountryId(int countryId)
        {
            var country = from x in DbContextHelper.DbContext.CountryMaster
                          where x.CountryID == countryId
                          select x;
            return country.Any() ? country.First() : new CountryMaster();
        }
        public static List<CountryMaster> GetAll()
        {
            var list = from x in DbContextHelper.DbContext.CountryMaster
                       //where x.Status == true
                       select x;

            return list.ToList();
        }
        public static void Delete(CountryMaster country)
        {
            DbContextHelper.DbContext.DeleteObject(country);
            DbContextHelper.DbContext.SaveChanges();
        }
        public static List<CountryMaster> GetPredicate(Expression<Func<CountryMaster, bool>> predicate)
        {
            return DbContextHelper.DbContext.CountryMaster
               .AsExpandable()
               .Where(predicate).OrderBy(c => c.CountryName)
               .ToList();
        }
        public static bool IsCountryExists(string countryName)
        {
            var country = from x in DbContextHelper.DbContext.CountryMaster
                          where x.CountryName == countryName
                          select x;
            return country.Any();
        }
    }
}
