using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class AdjusterStateLicenseManager {
		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			AdjusterServiceArea adjusterServiceArea = new AdjusterServiceArea { ID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("AdjusterServiceAreas", adjusterServiceArea);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(adjusterServiceArea);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		public static AdjusterServiceArea Get(int id) {
			AdjusterServiceArea serviceLicense = (from x in DbContextHelper.DbContext.AdjusterServiceArea.Include("StateMaster")
										   where x.ID == id
										   select x).FirstOrDefault<AdjusterServiceArea>();

			return serviceLicense;
		}

		public static List<AdjusterServiceArea> GetAll(int adjusterID) {
			List<AdjusterServiceArea> list = (from x in DbContextHelper.DbContext.AdjusterServiceArea
										    .Include("StateMaster")
										    .Include("AdjusterLicenseAppointmentType")
									    where x.AdjusterID == adjusterID
									    orderby x.StateMaster.StateName
									    select x).ToList<AdjusterServiceArea>();

			return list;
		}

		public static AdjusterServiceArea Save(AdjusterServiceArea adjusterServiceArea) {
			if (adjusterServiceArea.ID == 0) {
				DbContextHelper.DbContext.Add(adjusterServiceArea);
			}
			
			DbContextHelper.DbContext.SaveChanges();

			return adjusterServiceArea;
		}
	}
}
