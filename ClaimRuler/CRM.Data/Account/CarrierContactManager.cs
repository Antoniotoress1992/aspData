using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class CarrierContactManager {

		public static List<CarrierContact> GetAll(int carrierID) {
			return (from x in DbContextHelper.DbContext.CarrierContact
					   .Include("Contact.StateMaster")
					   .Include("Contact.CityMaster")
				   where x.CarrierID == carrierID
				   orderby x.Contact.LastName, x.Contact.FirstName
				   select x
			).ToList<CarrierContact>();

		}

		public static Contact GetPrimaryContact(int carrierID) {
			return (from x in DbContextHelper.DbContext.CarrierContact					  
				   where x.CarrierID == carrierID && x.Contact.IsPrimary == true				  
				   select x.Contact
			).FirstOrDefault<Contact>();

		}

		public static CarrierContact Save(CarrierContact carrierContact) {

			DbContextHelper.DbContext.Add(carrierContact);

			DbContextHelper.DbContext.SaveChanges();

			return carrierContact;
		}

		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			CarrierContact carrierContact = new CarrierContact { CarrierContactID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("CarrierContacts", carrierContact);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(carrierContact);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

	}
}
