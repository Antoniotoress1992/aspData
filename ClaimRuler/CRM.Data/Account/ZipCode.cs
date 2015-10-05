using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using LinqKit;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public class ZipCode {

		public static List<ZipCodeMaster> getByCityID(int cityID) {
			List<ZipCodeMaster> list = null;

			list = (from x in DbContextHelper.DbContext.ZipCodeMaster
				   where x.CityID == cityID
				   orderby x.ZipCode
				   select x).ToList();

			return list;
		}

		public static ZipCodeMaster Get(string id) {
			ZipCodeMaster zipCodeMaster = null;
			int zipCodeID = 0;

			int.TryParse(id, out zipCodeID);

			zipCodeMaster = (from x in DbContextHelper.DbContext.ZipCodeMaster
						  where x.ZipCodeID == zipCodeID
						  select x).FirstOrDefault();

			return zipCodeMaster;
		}

		public static ZipCodeMaster GetByZipCode(string zipCode) {
			ZipCodeMaster zipCodeMaster = null;
		
			zipCodeMaster = (from x in DbContextHelper.DbContext.ZipCodeMaster
						  where x.ZipCode == zipCode
						  select x).FirstOrDefault();

			return zipCodeMaster;
		}
	}
}
