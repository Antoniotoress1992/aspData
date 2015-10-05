using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class CarrierCommentManager {

		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			CarrierComment comment = new CarrierComment { CommentId = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("CarrierComments", comment);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(comment);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		public static List<CarrierComment> GetAll(int carrierID) {
			List<CarrierComment> comments = null;

			comments = (from x in DbContextHelper.DbContext.CarrierComment.Include("SecUser")
					  where x.CarrierID == carrierID
					  orderby x.CommentDate descending
					  select x).ToList();

			return comments;
		}

		public static CarrierComment Get(int id) {
			CarrierComment comment = null;

			comment = (from x in DbContextHelper.DbContext.CarrierComment
					  where x.CommentId == id
					 select x).First<CarrierComment>();

			return comment;
		}

		public static CarrierComment Save(CarrierComment comment) {
			if (comment.CommentId == 0) {
				DbContextHelper.DbContext.Add(comment);
			}

			DbContextHelper.DbContext.SaveChanges();

			return comment;
		}
	}
}
