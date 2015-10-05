using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using LinqKit;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class ReminderManager {
		public static Reminder Save(Reminder reminder) {
			if (reminder.ReminderID == 0) {
				DbContextHelper.DbContext.Add(reminder);
			}
			
			DbContextHelper.DbContext.SaveChanges();

			return reminder;
		}
		public static void Delete(int id) {
			// Create an entity to represent the Entity you wish to delete 
			// Notice you don't need to know all the properties, in this 
			// case just the ID will do. 
			Reminder reminder = (from x in DbContextHelper.DbContext.Reminder
							 where x.ReminderID == id
							 select x).FirstOrDefault();			

			// Do the delete the category 
			DbContextHelper.DbContext.DeleteObject(reminder);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}
	}
}
