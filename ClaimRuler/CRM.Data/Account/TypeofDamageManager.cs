

namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;
	using System.Linq.Expressions;
	using LinqKit;
    using CRM.Data.Entities;

	public class TypeofDamageManager {
		public static List<TypeOfDamageMaster> GetAll() {
			List<TypeOfDamageMaster> list = (from x in DbContextHelper.DbContext.TypeOfDamageMaster
									   where x.Status == true
									   orderby x.Sort
									   select x).ToList();

			return list;
		}
		public static List<TypeOfDamageMaster> GetAll(int clientID) {
			List<TypeOfDamageMaster> list = (from x in DbContextHelper.DbContext.TypeOfDamageMaster
									   where x.Status == true && (x.ClientId == clientID || x.ClientId == null)
									   orderby x.TypeOfDamage
									   select x).ToList();

			return list;
		}
		public static List<TypeOfDamageMaster> GetAll(int clientID, bool isHidden) {
			List<TypeOfDamageMaster> list = (from x in DbContextHelper.DbContext.TypeOfDamageMaster
									   where (x.Status == true) &&
											(x.ClientId == clientID || x.ClientId == null) &&
											(x.IsHidden == false || x.IsHidden == null)
									   orderby x.TypeOfDamage
									   select x).ToList();

			return list;
		}

		public static TypeOfDamageMaster getbyTypeOfDamage(string TypeOfDamage) {

			var plcy = from x in DbContextHelper.DbContext.TypeOfDamageMaster
					 where x.TypeOfDamage == TypeOfDamage && x.Status == true
					 select x;

			return plcy.Any() ? plcy.First() : new TypeOfDamageMaster();
		}

		public static TypeOfDamageMaster GetTypeOfDamage(int clientID, string TypeOfDamage) {

			var plcy = from x in DbContextHelper.DbContext.TypeOfDamageMaster
					 where x.TypeOfDamage == TypeOfDamage && x.Status == true && x.ClientId == clientID
					 select x;

			return plcy.Any() ? plcy.First() : new TypeOfDamageMaster();
		}

		public static TypeOfDamageMaster GetbyTypeOfDamageId(int TypeOfDamageId) {
			var plcy = from x in DbContextHelper.DbContext.TypeOfDamageMaster
					 where x.TypeOfDamageId == TypeOfDamageId && x.Status == true
					 select x;

			return plcy.Any() ? plcy.First() : new TypeOfDamageMaster();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="TypeOfDamageIdList"></param>
		/// <returns></returns>
		public static string[] GetDescriptions(string TypeOfDamageIDCSV) {
			int[] ids = TypeOfDamageIDCSV.Split(new char[] { ',' }).Select(x => Int32.Parse(x)).ToArray<int>();

			string[] TypeOfDamages = null;

			TypeOfDamages = (from x in DbContextHelper.DbContext.TypeOfDamageMaster
						  where ids.Contains(x.TypeOfDamageId)
						  select x.TypeOfDamage
					  ).ToArray<string>();

			return TypeOfDamages;
		}

		public static bool IsExist(string TypeOfDamage, int TypeOfDamageId) {
			var status = from x in DbContextHelper.DbContext.TypeOfDamageMaster
					   where x.TypeOfDamage == TypeOfDamage && x.TypeOfDamageId != TypeOfDamageId && x.Status == true
					   select x;

			return status.Any();
		}

		public static TypeOfDamageMaster Save(TypeOfDamageMaster objTypeOfDamageMaster) {
			if (objTypeOfDamageMaster.TypeOfDamageId == 0) {
				//objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				objTypeOfDamageMaster.InsertDate = DateTime.Now;
				objTypeOfDamageMaster.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				DbContextHelper.DbContext.Add(objTypeOfDamageMaster);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			objTypeOfDamageMaster.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			objTypeOfDamageMaster.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return objTypeOfDamageMaster;
		}
	}
}

