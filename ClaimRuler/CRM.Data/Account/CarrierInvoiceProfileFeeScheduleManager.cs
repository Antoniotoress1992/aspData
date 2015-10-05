using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class CarrierInvoiceProfileFeeScheduleManager {

		static public void Delete(int id) {
			CarrierInvoiceProfileFeeSchedule fee = new CarrierInvoiceProfileFeeSchedule { ID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("CarrierInvoiceProfileFeeSchedules", fee);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(fee);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		static public CarrierInvoiceProfileFeeSchedule Get(int id) {
			CarrierInvoiceProfileFeeSchedule fee = (from x in DbContextHelper.DbContext.CarrierInvoiceProfileFeeSchedule
									 where x.ID == id
											select x).FirstOrDefault<CarrierInvoiceProfileFeeSchedule>();
			return fee;
		}

		static public List<CarrierInvoiceProfileFeeSchedule> GetAll(int profileID) {
			List<CarrierInvoiceProfileFeeSchedule> fees = (from x in DbContextHelper.DbContext.CarrierInvoiceProfileFeeSchedule
												  where x.CarrierInvoiceProfileID == profileID
												  select x).ToList<CarrierInvoiceProfileFeeSchedule>();
			return fees;
		}

		static public CarrierInvoiceProfileFeeSchedule Save(CarrierInvoiceProfileFeeSchedule fee) {
			if (fee.ID == 0)
				DbContextHelper.DbContext.Add(fee);

			DbContextHelper.DbContext.SaveChanges();

			return fee;
		}
	}
}
