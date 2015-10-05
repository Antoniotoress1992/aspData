using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class CarrierInvoiceProfileFeeProvisionManager {
		static public void Delete(int id) {
			CarrierInvoiceProfileFeeProvision provision = new CarrierInvoiceProfileFeeProvision { ID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("CarrierInvoiceProfileFeeProvisions", provision);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(provision);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}
		static public CarrierInvoiceProfileFeeProvision Save(CarrierInvoiceProfileFeeProvision provision) {

			if (provision.ID == 0)
				DbContextHelper.DbContext.Add(provision);

			DbContextHelper.DbContext.SaveChanges();

			return provision;
		}

		static public CarrierInvoiceProfileFeeProvision Get(int id) {
			CarrierInvoiceProfileFeeProvision provision = null;

			provision = (from x in DbContextHelper.DbContext.CarrierInvoiceProfileFeeProvision
					   where x.ID == id
					   select x).FirstOrDefault<CarrierInvoiceProfileFeeProvision>();


			return provision;
		}

		static public List<CarrierInvoiceProfileFeeProvision> GetAll(int invoiceFeeProfileID) {
			List<CarrierInvoiceProfileFeeProvision> provisions = null;

			provisions = (from x in DbContextHelper.DbContext.CarrierInvoiceProfileFeeProvision
					    where x.CarrierInvoiceProfileID == invoiceFeeProfileID
					    select x).ToList();

			return provisions;
		}
	}
}
