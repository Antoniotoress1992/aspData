

namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;
    using CRM.Data.Entities;
	using System.Linq.Expressions;
	using LinqKit;

	public class PropertyTypeManager {
		public static TypeOfPropertyMaster GetByPropertyName(string Name)
        {

            var users = from x in DbContextHelper.DbContext.TypeOfPropertyMaster
                        where x.TypeOfProperty == Name && x.Status == true
                        select x;

		    return users.Any() ? users.First() : new TypeOfPropertyMaster();
            //return users.Any() ? users.First() : new PropertyType();
        }
	}
}
