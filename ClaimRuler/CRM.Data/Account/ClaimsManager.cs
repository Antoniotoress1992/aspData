namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;
	using System.Linq.Expressions;
	using LinqKit;
    using CRM.Data.Entities;

	public class ClaimsManager {
		public static List<Claim> GetPredicate(Expression<Func<Claim, bool>> predicate) {
            return DbContextHelper.DbContext.Claim.AsQueryable().Where(predicate).ToList();
		}

		public static Claim Save(Claim objClaim) {
			if (objClaim.ClaimID == 0) {
                DbContextHelper.DbContext.Claim.Add(objClaim);
			}

			DbContextHelper.DbContext.SaveChanges();

			return objClaim;
		}
		public static List<Claim> GetAll(int policyID) {
			List<Claim> claims = null;

			claims = (from x in DbContextHelper.DbContext.Claim.Include("AdjusterMaster")
					where x.IsActive == true && x.PolicyID == policyID
					select x).ToList<Claim>();

			return claims;
		}

		/// <summary>
		/// Returns full claim object
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Claim Get(int id) {
			Claim claim = (from x in DbContextHelper.DbContext.Claim
						.Include("LeadPolicy.LeadPolicyType")
						.Include("LeadPolicy.Carrier")
						.Include("LeadPolicy.Leads")
						.Include("LeadPolicy.Leads.StateMaster")
						.Include("LeadPolicy.Leads.CityMaster")
						.Include("AdjusterMaster")	
						where x.ClaimID == id
						select x).FirstOrDefault<Claim>();

			return claim;
		}
		public static Claim Get(string claimNumber) {
			Claim claim = (from x in DbContextHelper.DbContext.Claim
						.Include("LeadPolicy")
						.Include("LeadPolicy.Carrier")
						.Include("LeadPolicy.Leads")
						.Include("AdjusterMaster")
						where x.AdjusterClaimNumber == claimNumber
						select x).FirstOrDefault<Claim>();

			return claim;
		}

        public static Claim Get2(string InsurerClaimNumber)
        {
            Claim claim = (from x in DbContextHelper.DbContext.Claim
                        .Include("LeadPolicy")
                        .Include("LeadPolicy.Carrier")
                        .Include("LeadPolicy.Leads")
                        .Include("AdjusterMaster")
                           where x.InsurerClaimNumber == InsurerClaimNumber
                           select x).FirstOrDefault<Claim>();

            return claim;
        }

		/// <summary>
		/// Returns claims object. No navigational properties
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Claim GetByID(int id) {
			Claim claim = (from x in DbContextHelper.DbContext.Claim					
						where x.ClaimID == id
						select x).FirstOrDefault<Claim>();

			return claim;
		}

		static public string getAdjusterClaimNumber(int claimID) {
			string AdjusterClaimNumber = null;

			AdjusterClaimNumber = (from x in DbContextHelper.DbContext.Claim
						 where x.ClaimID == claimID
						 select x.AdjusterClaimNumber
					  ).FirstOrDefault<string>();

			return AdjusterClaimNumber;
		}
		static public string getInsurerClaimNumber(int claimID) {
			string InsurerClaimNumber = null;

			InsurerClaimNumber = (from x in DbContextHelper.DbContext.Claim
							   where x.ClaimID == claimID
							   select x.InsurerClaimNumber
					  ).FirstOrDefault<string>();

			return InsurerClaimNumber;
		}

		static public int getAdjusterClaimCount(int adjusterID) {
			int numberOfClaims = 0;

			numberOfClaims = (from x in DbContextHelper.DbContext.Claim
						   where x.AdjusterID == adjusterID
						   group x by x.ClaimID into g
						   select g.Key
					  ).Count<int>();

			return numberOfClaims;
		}


        public static Client GetClientByUserId(int userID) 
        {
            Client client = (from x in DbContextHelper.DbContext.Client where x.UserId == userID select x).FirstOrDefault<Client>();
            return client;
        }

        public static string GetContactByContactID(int contactID)
        {
            string contactName = string.Empty;
            contactName = (from x in DbContextHelper.DbContext.Contact where x.ContactID == contactID select x.ContactName).FirstOrDefault();
            if (!string.IsNullOrEmpty(contactName))
            {
            return contactName;
            }
            else
            {
                return string.Empty;
            }
        }


        public static Client SaveClient(Client client) 
        {
            if (client.ClientId == 0)
            {
                DbContextHelper.DbContext.Client.Add(client);
            }

            DbContextHelper.DbContext.SaveChanges();

            return client;
        }




	}
}
