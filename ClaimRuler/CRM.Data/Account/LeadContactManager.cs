using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public class LeadContactManager {

		public static List<LeadContact> GetContactByLeadID(int leadID) {
			var list = from x in DbContextHelper.DbContext.LeadContact
						 .Include("LeadContactType")
						 //.Include("InsuranceType")
					 where x.isActive == true &&
						x.LeadID == leadID
					 select x;

			return list.ToList();
		}

		public static List<LeadContact> GetContactByLeadID(int leadID, int policyTypeID) {
			List<LeadContact> list = (from x in DbContextHelper.DbContext.LeadContact
						 .Include("LeadContactType")
					 where x.isActive == true &&
						x.LeadID == leadID &&
						x.PolicyTypeID == policyTypeID
					 select x
					 ).ToList<LeadContact>();

			return list;
		}

		public static LeadContact Get(int contactID) {
			LeadContact contact = (from x in DbContextHelper.DbContext.LeadContact.Include("LeadContactType")
							   where x.isActive == true &&
								  x.ID == contactID
							   select x).FirstOrDefault();

			return contact;
		}
		

		public static string[] GetEmails(string keyword) {
			string[] list = (from x in DbContextHelper.DbContext.LeadContact
					 where x.isActive == true && x.Email != null && x.ContactName.Contains(keyword)
					 select x.Email
					 ).ToArray();

			return list;
		}

		public static void Save(LeadContact contact) {
			if (contact.ID == 0) {
				contact.DateCreated = DateTime.Now;
				DbContextHelper.DbContext.Add(contact);
			}
			
			DbContextHelper.DbContext.SaveChanges();
		}
	}
}
