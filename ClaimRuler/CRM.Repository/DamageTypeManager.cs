using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	public class DamageTypeManager : IDisposable {

		private bool disposed = false;		// to detect redundant calls

        private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public DamageTypeManager() {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();
		}
		public  List<TypeOfDamageMaster> GetAll() {
			List<TypeOfDamageMaster> list = (from x in claimRulerDBContext.TypeOfDamageMaster
									   where x.Status == true
									   orderby x.Sort
									   select x).ToList();

			return list;
		}
		public  List<TypeOfDamageMaster> GetAll(int clientID) {
			List<TypeOfDamageMaster> list = (from x in claimRulerDBContext.TypeOfDamageMaster
									   where x.Status == true && (x.ClientId == clientID || x.ClientId == null)
									   orderby x.TypeOfDamage
									   select x).ToList();

			return list;
		}
		public  List<TypeOfDamageMaster> GetAll(int clientID, bool isHidden) {
			List<TypeOfDamageMaster> list = (from x in claimRulerDBContext.TypeOfDamageMaster
									   where (x.Status == true) &&
											(x.ClientId == clientID || x.ClientId == null) &&
											(x.IsHidden == false || x.IsHidden == null)
									   orderby x.TypeOfDamage
									   select x).ToList();

			return list;
		}

		public  TypeOfDamageMaster getbyTypeOfDamage(string TypeOfDamage) {

			var plcy = from x in claimRulerDBContext.TypeOfDamageMaster
					 where x.TypeOfDamage == TypeOfDamage && x.Status == true
					 select x;

			return plcy.Any() ? plcy.First() : new TypeOfDamageMaster();
		}

		public  TypeOfDamageMaster GetTypeOfDamage(int clientID, string TypeOfDamage) {

			var plcy = from x in claimRulerDBContext.TypeOfDamageMaster
					 where x.TypeOfDamage == TypeOfDamage && x.Status == true && x.ClientId == clientID
					 select x;

			return plcy.Any() ? plcy.First() : new TypeOfDamageMaster();
		}

		public  TypeOfDamageMaster GetbyTypeOfDamageId(int TypeOfDamageId) {
			var plcy = from x in claimRulerDBContext.TypeOfDamageMaster
					 where x.TypeOfDamageId == TypeOfDamageId && x.Status == true
					 select x;

			return plcy.Any() ? plcy.First() : new TypeOfDamageMaster();
		}
		public  string[] GetDescriptions(string TypeOfDamageIdList) {
			int[] ids = TypeOfDamageIdList.Split(new char[] { ',' }).Select(x => Int32.Parse(x)).ToArray<int>();

			string[] TypeOfDamages = null;

			TypeOfDamages = (from x in claimRulerDBContext.TypeOfDamageMaster
						  where ids.Contains(x.TypeOfDamageId)
						  select x.TypeOfDamage
					  ).ToArray<string>();

			return TypeOfDamages;
		}

		public  bool IsExist(string TypeOfDamage, int TypeOfDamageId) {
			var status = from x in claimRulerDBContext.TypeOfDamageMaster
					   where x.TypeOfDamage == TypeOfDamage && x.TypeOfDamageId != TypeOfDamageId && x.Status == true
					   select x;

			return status.Any();
		}

		public  TypeOfDamageMaster Save(TypeOfDamageMaster objTypeOfDamageMaster) {
			if (objTypeOfDamageMaster.TypeOfDamageId == 0) {
				//objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				objTypeOfDamageMaster.InsertDate = DateTime.Now;
				
				claimRulerDBContext.TypeOfDamageMaster.Add(objTypeOfDamageMaster);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			
			objTypeOfDamageMaster.UpdateDate = DateTime.Now;
			claimRulerDBContext.SaveChanges();

			return objTypeOfDamageMaster;
		}

		#region ===== memory management =====

		public void Dispose() {
			// Perform any object clean up here.

			// If you are inheriting from another class that
			// also implements IDisposable, don't forget to
			// call base.Dispose() as well.
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing) {
			if (!disposed) {
				if (disposing) {
					if (claimRulerDBContext != null) {

						claimRulerDBContext.Dispose();
					}
				}

				disposed = true;
			}
		}
		#endregion
	}

}
