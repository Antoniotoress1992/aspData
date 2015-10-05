using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.Linq.Expressions;
using LinqKit;
using Entity = CRM.Data.Entities;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class ContactManager {
		public static void Delete(int id) {
            Entity.Contact contact = Get(id);

			if (contact != null) {
				DbContextHelper.DbContext.DeleteObject(contact);

				DbContextHelper.DbContext.SaveChanges();
			}

		}

        public static List<Entity.Contact> GetAll()
        {
			return (from x in DbContextHelper.DbContext.Contact.Include("StateMaster").Include("CityMaster").Include("LeadContactType")
				   orderby x.LastName
				   select x
					 ).ToList<Contact>();

		}

        public static IQueryable<Entity.Contact> GetAll(int clientID)
        {
			IQueryable<Contact> contacts = null;

			contacts = from x in DbContextHelper.DbContext.Contact.Include("StateMaster").Include("CityMaster").Include("LeadContactType")
					 where x.ClientID == clientID && x.IsActive == true
					 //orderby x.LastName
					 select x;

			return contacts;


		}
        public static List<Entity.Contact> GetAll(int clientID, string typeDescription)
        {
            List<Entity.Contact> contacts = null;
			string keyword = typeDescription.ToLower();

			contacts = (from x in DbContextHelper.DbContext.Contact.Include("StateMaster").Include("CityMaster").Include("LeadContactType")
					  where x.ClientID == clientID &&
						    x.LeadContactType.Description.ToLower().Contains(keyword)
					  orderby x.LastName
					  select x
                     ).ToList<Entity.Contact>();

			return contacts;
		}

        public static List<Entity.Contact> GetAll(int clientID, int contactTypeID)
        {
            List<Entity.Contact> contacts = null;			

			contacts = (from x in DbContextHelper.DbContext.Contact
					  where x.ClientID == clientID &&
						    x.CategoryID == contactTypeID
					  orderby x.ContactName
					  select x
                     ).ToList<Entity.Contact>();

			return contacts;
		}
       
        public static List<Entity.Contact> Search(string search)
        {
			return (from x in DbContextHelper.DbContext.Contact.Include("StateMaster").Include("CityMaster").Include("LeadContactType")
				   where x.LastName.StartsWith(search)
				   orderby x.LastName
				   select x
					 ).ToList<Contact>();

		}
        public static IQueryable<Entity.Contact> Search(string search, int clientID)
        {
			return from x in DbContextHelper.DbContext.Contact.Include("StateMaster").Include("CityMaster").Include("LeadContactType")
				  where x.ClientID == clientID && x.LastName.StartsWith(search) && x.IsActive == true
				  orderby x.LastName
				  select x;


		}


        public static Entity.Contact Get(int id)
        {
			return (from x in DbContextHelper.DbContext.Contact
					   .Include("StateMaster")
					   .Include("CityMaster")
					   .Include("LeadContactType")
					   .Include("SecUser")
				   where x.ContactID == id
				   select x
					 ).FirstOrDefault<Contact>();

		}

		public static Contact Get(int clientID, string contactName) {
			return (from x in DbContextHelper.DbContext.Contact
				   where x.ClientID == clientID &&
						x.ContactName == contactName &&
						x.IsActive == true
				   select x
				).FirstOrDefault<Contact>();

		}
		public static string GetName(int contactID) {
			string contactName = null;

			contactName = (from x in DbContextHelper.DbContext.Contact
						where x.ContactID == contactID
						select x.ContactName
				).FirstOrDefault<string>();

			return contactName;
		}

        public static int GetContactId(string contactDescription)
        {
            int contactId = 0;

            contactId = (from x in DbContextHelper.DbContext.LeadContactType
                           where x.Description == contactDescription
                           select x.ID
                ).FirstOrDefault<int>();

            return contactId;
        }


		public static Contact Save(Contact contact) {
			if (contact.ContactID == 0) {
				DbContextHelper.DbContext.Add(contact);
			}
            
			DbContextHelper.DbContext.SaveChanges();

			return contact;
		}

        public static Contact SaveContact(Contact contact)
        {           
           DbContextHelper.DbContext.Add(contact);
           DbContextHelper.DbContext.SaveChanges();
           return contact;
        }


        public static Contact Update(Contact contact)
        {
            int contactId=contact.ContactID;
            Contact objContact = DbContextHelper.DbContext.Contact.First(x => x.ContactID == contactId);
            objContact.ContactName = contact.ContactName;
            objContact.Address1 = contact.Address1;
            objContact.CityName = contact.CityName;
            objContact.State = contact.State;
            objContact.ZipCode = contact.ZipCode;
            objContact.Phone = contact.Phone;
            DbContextHelper.DbContext.SaveChanges();
            return contact;
        }

        public static Contact UpdateContact(Contact contact)
        {
            int contactId = contact.ContactID;
            Contact objContact = DbContextHelper.DbContext.Contact.First(x => x.ContactID == contactId);
            objContact.ContactName = contact.ContactName;            
            DbContextHelper.DbContext.SaveChanges();
            return objContact;
        }



		public static int Save1(Contact contact) {
			int contactID = 0;
			Contact lcontact = null;

			if (contact.ContactID == 0) {
				lcontact = new Contact();

				DbContextHelper.DbContext.Add(lcontact);
			}
			else {
				lcontact = Get(contact.ContactID);
			}

			if (lcontact != null) {
				lcontact.Address1 = contact.Address1;
				lcontact.Address2 = contact.Address2;
				lcontact.CategoryID = contact.CategoryID;
				lcontact.Balance = contact.Balance;
				lcontact.CarrierID = contact.CarrierID;
				lcontact.CityID = contact.CityID;
				lcontact.ClaimName = contact.ClaimName;
				lcontact.ClientID = contact.ClientID;
				lcontact.CompanyName = contact.CompanyName;
				lcontact.County = contact.County;
				lcontact.Email = contact.Email;
				lcontact.FirstName = contact.FirstName;
				lcontact.LastName = contact.LastName;
				lcontact.Mobile = contact.Mobile;
				lcontact.Phone = contact.Phone;
				lcontact.StateID = contact.StateID;
				lcontact.ZipCodeID = contact.ZipCodeID;
				lcontact.CarrierID = contact.CarrierID;
				lcontact.ContactTitle = contact.ContactTitle;
				lcontact.DepartmentName = contact.DepartmentName;
				lcontact.IsPrimary = contact.IsPrimary;

				DbContextHelper.DbContext.SaveChanges();

				contactID = contact.ContactID;
			}

			return contactID;
		}

        public static Contact GetAllContact(int clientID)
        {
            Contact contacts = null;
            contacts = new Contact();
            contacts = (from x in DbContextHelper.DbContext.Contact
                       where x.ClientID == clientID && x.IsActive == true                      
                        select x) as Contact;

            return contacts;


        }

        public static Contact GetLeadContact(int leadId,int categoryId)
        {
            return (from x in DbContextHelper.DbContext.Contact
                     where x.LeadId == leadId && x.IsActive == true && x.CategoryID == categoryId
                    select x
                ).FirstOrDefault<Contact>();  

        }


	}
}
