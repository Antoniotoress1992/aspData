using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class CarrierDocumentManager {

		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			CarrierDocument document = Get(id);

			if (document != null) {
				// Do the delete the category 
				DbContextHelper.DbContext.DeleteObject(document);

				// Apply the delete to the database 
				DbContextHelper.DbContext.SaveChanges();
			}

			

			
		}
		public static CarrierDocument Get(int id) {
			CarrierDocument document = null;

			document = (from x in DbContextHelper.DbContext.CarrierDocument
					  where x.CarrierDocumentID == id					  
					   select x).FirstOrDefault<CarrierDocument>();

			return document;
		}

		public static List<CarrierDocument> GetAll(int carrierID) {
			List<CarrierDocument> documents = null;

			documents = (from x in DbContextHelper.DbContext.CarrierDocument
					  where x.CarrierID == carrierID
					  orderby x.DocumentDate ascending
					   select x).ToList<CarrierDocument>();

			return documents;
		}

		public static CarrierDocument Save(CarrierDocument document) {
			if (document.CarrierDocumentID == 0) {
				DbContextHelper.DbContext.Add(document);
			}

			DbContextHelper.DbContext.SaveChanges();

			return document;
		}
	}
}
