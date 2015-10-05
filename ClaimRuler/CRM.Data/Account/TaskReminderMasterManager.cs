using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class TaskReminderMasterManager {
		public static List<TaskReminderMaster> GetAll(int clientID) {
			List<TaskReminderMaster> taskReminderMasters = null;

			taskReminderMasters = (from x in DbContextHelper.DbContext.TaskReminderMaster
							    where x.IsActive == true &&
							    (x.ClientID == clientID || x.ClientID == null)
							    select x).ToList();

			return taskReminderMasters;
		}
	}
}
