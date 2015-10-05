using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;
using System.Linq.Expressions;
using LinqKit;

namespace CRM.Data.Account {
	static public class ReminderMasterManager {
		public static List<ReminderMaster> GetAll() {
			var list = from x in DbContextHelper.DbContext.ReminderMaster
					 where x.isActive == true
					 select x;

			return list.ToList();
		}

		public static List<ReminderMaster> GetAll(int clientID) {
			var list = from x in DbContextHelper.DbContext.ReminderMaster
					 where x.isActive == true &&
					 x.clientID == clientID
					 select x;

			return list.ToList();
		}

		public static ReminderMaster Get(int id) {
			return DbContextHelper.DbContext.ReminderMaster.Where(x => x.ReminderID == id).FirstOrDefault();
		}

		public static void Save(ReminderMaster reminder) {
			ReminderMaster lreminder = null;

			if (reminder.ReminderID == 0) {
				lreminder = new ReminderMaster();
				DbContextHelper.DbContext.Add(lreminder);
			}
			else {
				lreminder = Get(reminder.ReminderID);
			}

			lreminder.Description = reminder.Description;
			
			lreminder.Duration = reminder.Duration;
			
			lreminder.DurationType = reminder.DurationType;
			
			lreminder.clientID = reminder.clientID;

			lreminder.isActive = reminder.isActive;

			DbContextHelper.DbContext.SaveChanges();

		}
	}
}
