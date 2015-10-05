using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public class InsuranceTypeManager {

		public static List<InsuranceType> GetAll() {
			var list = from x in DbContextHelper.DbContext.InsuranceType
					 where x.isActive == true
					 select x;

			return list.ToList();

		}
	}
}
