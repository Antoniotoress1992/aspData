using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class CarrierManager {

		public static IQueryable<Carrier> GetCarriers(int clientID) {
			return from x in DbContextHelper.DbContext.Carrier
				  .Include("StateMaster")
				  .Include("CityMaster")
				  where x.IsActive && x.ClientID == clientID
				  select x;

		} 

		public static List<CarrierView> GetAll(int clientID) { 
			List<CarrierView> carriers = null;

			carriers = (from x in DbContextHelper.DbContext.Carrier
				  .Include("StateMaster")
				  .Include("CityMaster")
					  where x.IsActive == true && x.ClientID == clientID
					  select new CarrierView {
						  AddressLine1 = x.AddressLine1,
						  AddressLine2 = x.AddressLine2,
						  CarrierID = x.CarrierID,
						  CarrierName = x.CarrierName,
						  CityName = x.CityMaster == null ? "" : x.CityMaster.CityName,
						  StateName = x.StateMaster == null ? "" : x.StateMaster.StateName,
						  ZipCode = x.ZipCode
					  }).ToList<CarrierView>();

			return carriers;
		}

		// returns full object
		public static Carrier Get(int id) {
			Carrier carrier = null;

			carrier = (from x in DbContextHelper.DbContext.Carrier
						.Include("CarrierLocation")
						.Include("CarrierInvoiceProfile.CarrierInvoiceProfileFeeItemized")
						.Include("CarrierInvoiceProfile.CarrierInvoiceProfileFeeProvision")
						.Include("CarrierInvoiceProfile.CarrierInvoiceProfileFeeSchedule")
						.Include("StateMaster")
						.Include("CityMaster")
						.Include("ZipCodeMaster")
					 where x.CarrierID == id && x.IsActive == true
					 select x).FirstOrDefault();

			return carrier;
		}

		public static CarrierInvoiceProfile GetInvoiceProfile(int carrierID, int invoiceProfileTypeID) {
			CarrierInvoiceProfile invoiceProfile = null;

			invoiceProfile = (from x in DbContextHelper.DbContext.CarrierInvoiceProfile
											.Include("CarrierInvoiceProfileFeeItemized")
											.Include("CarrierInvoiceProfileFeeProvision")
											.Include("CarrierInvoiceProfileFeeSchedule")
						   where	x.CarrierID == carrierID
								&& x.CarrierInvoiceProfileTypeID == invoiceProfileTypeID
								&& x.IsActive == true
						   select x
						  ).FirstOrDefault();

			return invoiceProfile;
		}

		public static Carrier GetByID(int id) {
			Carrier carrier = null;

			carrier = (from x in DbContextHelper.DbContext.Carrier
						.Include("CountryMaster")
						 .Include("StateMaster")
						  .Include("CityMaster")
						  .Include("ZipCodeMaster")
					 where x.CarrierID == id
					 select x).FirstOrDefault();

			return carrier;
		}

		public static string GetName(int id) {
			string carrierName = null;

			carrierName = (from x in DbContextHelper.DbContext.Carrier
						where x.CarrierID == id
						select x.CarrierName
			 ).FirstOrDefault();

			return carrierName;
		}

		public static List<Contact> GetContacts(int carrierID) {
			return (from x in DbContextHelper.DbContext.Contact.Include("StateMaster").Include("CityMaster")
				   where x.CarrierID == carrierID
				   orderby x.LastName, x.FirstName
				   select x
			).ToList<Contact>();

		}

		public static List<LeadPolicy> GetPoliciesReadyForInvoice(int carrierID) {
			List<LeadPolicy> policies = null;

			policies = (from x in DbContextHelper.DbContext.LeadPolicy
					  .Include("LeadPolicyType")
					  .Include("Leads")
					  where x.CarrierID == carrierID && x.IsInvoiceReady == true && (x.IsInvoiced == null || x.IsInvoiced == false)
					  select x).ToList<LeadPolicy>();

			return policies;
		}

		public static Carrier Save(Carrier carrier) {
			if (carrier.CarrierID == 0) {
				carrier.InsertDate = DateTime.Now;
				DbContextHelper.DbContext.Add(carrier);
			}

			carrier.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return carrier;
		}
	}
}
