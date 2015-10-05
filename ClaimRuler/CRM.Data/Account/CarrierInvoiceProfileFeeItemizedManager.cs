using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class CarrierInvoiceProfileFeeItemizedManager {

		static public void Delete(int id) {
			//DbContextHelper.DbContext.ExecuteStoreCommand("DELETE FROM CarrierInvoiceProfileFeeItemized WHERE ID = {0}", id);

            var carrier = DbContextHelper.DbContext.CarrierInvoiceProfileFeeItemized.Where(x => x.ID == id).ToList();
            foreach (CarrierInvoiceProfileFeeItemized objR in carrier)
            {
                DbContextHelper.DbContext.DeleteObject(objR);
            }
            DbContextHelper.DbContext.SaveChanges();

		}

		static public CarrierInvoiceProfileFeeItemized Get(int id) {
			CarrierInvoiceProfileFeeItemized item = null;

			item = (from x in DbContextHelper.DbContext.CarrierInvoiceProfileFeeItemized
				   where x.ID == id
				   select x).FirstOrDefault<CarrierInvoiceProfileFeeItemized>();

			return item;
		}

		static public List<CarrierInvoiceProfileFeeItemized> GetAll(int profileID) {
			List<CarrierInvoiceProfileFeeItemized> items = null;

			items = (from x in DbContextHelper.DbContext.CarrierInvoiceProfileFeeItemized
				    where x.CarrierInvoiceProfileID == profileID && x.IsActive == true
				    select x).ToList<CarrierInvoiceProfileFeeItemized>();

			return items;
		}

		static public CarrierInvoiceProfileFeeItemized Save(CarrierInvoiceProfileFeeItemized item) {
			if (item.ID == 0)
				DbContextHelper.DbContext.Add(item);

			DbContextHelper.DbContext.SaveChanges();

			return item;
		}
	}
}
