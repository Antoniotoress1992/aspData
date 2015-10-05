using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class AdjusterTypeClaimHandledManager {
		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			AdjusterHandleClaimType handleClaimType = new AdjusterHandleClaimType { ID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("AdjusterHandleClaimTypes", handleClaimType);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(handleClaimType);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		public static List<AdjusterHandleClaimType> GetAll(int adjusterID) {
			List<AdjusterHandleClaimType> list = null;

			list = (from x in DbContextHelper.DbContext.AdjusterHandleClaimType.Include("LeadPolicyType")
				   where x.AdjusterID == adjusterID
				   orderby x.LeadPolicyType.Description
				   select x).ToList<AdjusterHandleClaimType>();

			return list;
		}

		public static AdjusterHandleClaimType Save(AdjusterHandleClaimType handleClaimType) {
			if (handleClaimType.ID == 0) {
				DbContextHelper.DbContext.Add(handleClaimType);
			}

			DbContextHelper.DbContext.SaveChanges();

			return handleClaimType;
		}
	}
}
