using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;
using System.Linq.Expressions;
using LinqKit;

namespace CRM.Data.Account {
	public static class AdjusterNoteManager {
		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			AdjusterNote note = new AdjusterNote { NoteID = id };

			// Now attach the category stub object to the "Categories" set. 
			// This puts the Entity into the context in the unchanged state, 
			// This is same state it would have had if you made the query 
			DbContextHelper.DbContext.AttachTo("AdjusterNotes", note);


			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(note);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		public static IQueryable<AdjusterNote> GetAll(int adjusterID) {
			var notes = from x in DbContextHelper.DbContext.AdjusterNote
					  where x.AdjusterID == adjusterID
					  orderby x.NoteDate descending
					  select x;

			return notes;
		}

		public static AdjusterNote Get(int id) {
			AdjusterNote note = (from x in DbContextHelper.DbContext.AdjusterNote
							 where x.NoteID == id
							 select x).FirstOrDefault<AdjusterNote>();

			return note;
		}

		public static AdjusterNote Save(AdjusterNote note) {
			if (note.NoteID == 0) {

				note.NoteDate = DateTime.Now;
				DbContextHelper.DbContext.Add(note);
			}

			DbContextHelper.DbContext.SaveChanges();

			return note;
		}
	}
}
