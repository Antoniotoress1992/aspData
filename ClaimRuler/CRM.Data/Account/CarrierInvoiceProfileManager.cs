using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class CarrierInvoiceProfileManager {


		static public CarrierInvoiceProfile Get(int id) {
			CarrierInvoiceProfile profile = null;

			profile = (from x in DbContextHelper.DbContext.CarrierInvoiceProfile
					 .Include("CarrierInvoiceType")
					 where x.CarrierInvoiceProfileID == id
					 select x).FirstOrDefault<CarrierInvoiceProfile>();

			return profile;
		}
		/// <summary>
		/// Returns full object
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		static public CarrierInvoiceProfile GetProfileForInvoicing(int id, int? ClaimDesignationID) {
			CarrierInvoiceProfile profile = null;

			profile = (from x in DbContextHelper.DbContext.CarrierInvoiceProfile
						.Include("CarrierInvoiceProfileFeeItemized")
						//.Include("CarrierInvoiceProfileFeeProvisions")
						.Include("CarrierInvoiceProfileFeeSchedule")
					 where x.CarrierInvoiceProfileID == id &&
							x.CarrierInvoiceProfileTypeID == ClaimDesignationID
					 select x).FirstOrDefault<CarrierInvoiceProfile>();

			return profile;
		}

		static public CarrierInvoiceProfile GetProfileForInvoicing(int id) {
			CarrierInvoiceProfile profile = null;

			profile = (from x in DbContextHelper.DbContext.CarrierInvoiceProfile
						.Include("CarrierInvoiceProfileFeeSchedule")
						.Include("CarrierInvoiceProfileFeeItemized.InvoiceServiceType")
						.Include("CarrierInvoiceProfileFeeItemized.ExpenseType")
					 where	x.CarrierInvoiceProfileID == id 
					 select	x).FirstOrDefault<CarrierInvoiceProfile>();

			return profile;
		}

		static public List<CarrierInvoiceProfile> GetAll(int carrierID) {
			List<CarrierInvoiceProfile> list = null;

			list = (from x in DbContextHelper.DbContext.CarrierInvoiceProfile
				   where x.CarrierID == carrierID && x.IsActive == true
				   select x).ToList<CarrierInvoiceProfile>();

			return list;
		}

		public static CarrierInvoiceProfile Save(CarrierInvoiceProfile profile) {
			if (profile.CarrierInvoiceProfileID == 0) {
				DbContextHelper.DbContext.Add(profile);
			}


			DbContextHelper.DbContext.SaveChanges();

			return profile;
		}
	}
}
