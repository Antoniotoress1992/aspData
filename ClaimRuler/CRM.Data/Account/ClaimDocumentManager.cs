using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class ClaimDocumentManager {

		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			ClaimDocument document = new ClaimDocument { ClaimDocumentID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("ClaimDocuments", document);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(document);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		public static List<ClaimDocument> GetAll(int claimID) {
			List<ClaimDocument> documents = null;


			documents = (from x in DbContextHelper.DbContext.ClaimDocument
					   .Include("DocumentCategory")
					   where x.ClaimID == claimID
					   orderby x.DocumentDate descending
					   select x
					   ).ToList<ClaimDocument>();

			return documents;
		}

		public static ClaimDocument Get(int id) {
			ClaimDocument document = null;

			document = (from x in DbContextHelper.DbContext.ClaimDocument
					  where x.ClaimDocumentID == id
					  select x
					   ).FirstOrDefault<ClaimDocument>();

			return document;
		}

		public static ClaimDocument Save(ClaimDocument document) {
		
			if (document.ClaimDocumentID == 0)
				DbContextHelper.DbContext.Add(document);

			DbContextHelper.DbContext.SaveChanges();

			return document;
		}

		
	}
}
