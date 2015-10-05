using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class AdjusterReferenceManager {
		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			AdjusterReference adjusterReference = new AdjusterReference { ID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("AdjusterReferences", adjusterReference);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(adjusterReference);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		public static List<AdjusterReference> GetAll(int adjusterID) {
			List<AdjusterReference> references = null;

			references = (from x in DbContextHelper.DbContext.AdjusterReference
					    where x.AdjusterID == adjusterID
					    orderby x.RereferenceName
					    select x).ToList<AdjusterReference>();

			return references;
		}

		public static AdjusterReference Get(int id) {
			AdjusterReference reference = null;

			reference = (from x in DbContextHelper.DbContext.AdjusterReference
					   where x.ID == id
					   select x).FirstOrDefault<AdjusterReference>();

			return reference;
		}

		public static AdjusterReference Save(AdjusterReference adjusterReference) {
			if (adjusterReference.ID == 0) {
				DbContextHelper.DbContext.Add(adjusterReference);
			}

			DbContextHelper.DbContext.SaveChanges();

			return adjusterReference;
		}
	}
}
