using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class ClaimContactManager {
		public static List<ClaimContact> GetAll(int claimID) {
			List<ClaimContact> contacts = null;

			contacts = (from x in DbContextHelper.DbContext.ClaimContact
					   .Include("Contact.StateMaster")
					   .Include("Contact.CityMaster")
					   .Include("Contact.LeadContactType")
					  where x.ClaimID == claimID
				   orderby x.Contact.LastName, x.Contact.FirstName
				   select x
			).ToList<ClaimContact>();

			return contacts;


		}

		public static ClaimContact Save(ClaimContact contact) {

			DbContextHelper.DbContext.Add(contact);

			DbContextHelper.DbContext.SaveChanges();

			return contact;
		}

		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			ClaimContact claimContact = new ClaimContact { ClaimContactID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("ClaimContacts", claimContact);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(claimContact);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}
	}
}
