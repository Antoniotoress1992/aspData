using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Linq.Expressions;
using LinqKit;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class MortgageeManager {
		public static Mortgagee Get(string mortgageeName) {

			Mortgagee mortgagee = (from x in DbContextHelper.DbContext.Mortgagee
						where x.MortageeName == mortgageeName && x.Status == true
							   select x).FirstOrDefault();

			return mortgagee;
		}
		
		public static Mortgagee Get(int id) {

			Mortgagee mortgagee = (from x in DbContextHelper.DbContext.Mortgagee.Include("CityMaster").Include("StateMaster").Include("ZipCodeMaster")
							   where x.MortgageeID == id 
							   select x).FirstOrDefault();

			return mortgagee;
		}
		
		public static Mortgagee Get(string mortgageeName, int clientID) {

			Mortgagee mortgagee = (from x in DbContextHelper.DbContext.Mortgagee
							   where x.MortageeName == mortgageeName && x.Status == true & x.ClientID == clientID
							   select x).FirstOrDefault();

			return mortgagee;
		}

		public static IQueryable<Mortgagee> GetAll(int clientID) {

			IQueryable<Mortgagee> mortgagees = from x in DbContextHelper.DbContext.Mortgagee
							   where x.ClientID == clientID && x.Status == true
							   select x;

			return mortgagees;
		}

		

		public static LeadPolicyLienholder GetLienHolder(int id) {
			LeadPolicyLienholder lienholder = null;

			lienholder = (from x in DbContextHelper.DbContext.LeadPolicyLienholder
					    where x.ID == id
					    select x).FirstOrDefault();

			return lienholder;
		}
		
		public static void DeleteLienHolder(int id) {
			LeadPolicyLienholder lienholder = null;

			lienholder = (from x in DbContextHelper.DbContext.LeadPolicyLienholder
					    where x.ID == id
					    select x).FirstOrDefault();

			if (lienholder != null) {
				DbContextHelper.DbContext.DeleteObject(lienholder);

				DbContextHelper.DbContext.SaveChanges();

			}

		}
		
		public static Mortgagee Save(Mortgagee mortgagee) {
			if (mortgagee.MortgageeID == 0) {
				DbContextHelper.DbContext.Add(mortgagee);
			}
			
			DbContextHelper.DbContext.SaveChanges();

			return mortgagee;
		}

		public static bool IsExist(string name, int clientID) {
			bool exists = (from x in DbContextHelper.DbContext.Mortgagee
					   where x.MortageeName == name && x.ClientID == clientID && x.Status == true
					   select x).Any();

			return exists;
		}
	}
}
