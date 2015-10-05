using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Core;

using LinqKit;
using System.Linq.Expressions;

using System.Data.Linq.SqlClient;
using System.Data.Objects.SqlClient;
using CRM.Data.Entities;

namespace EmailReminder {
	class Program {
		static CRMEntities InternalDbContext;

		static void Main(string[] args) {
			var conString = ConfigurationManager.ConnectionStrings["CRMEntities"].ToString();
			InternalDbContext = new CRM.Data.Entities.CRMEntities();

			checkForComingUpTasks();
		}

		static void checkForComingUpTasks() {
			List<vw_Reminder> reminders = null;

			reminders = GetUpcomingReminders();

			if (reminders != null && reminders.Count > 0) {
				sendEmail(reminders);
			}
		}

		static List<vw_Reminder> GetUpcomingReminders() {
			List<vw_Reminder> reminders = null;

			reminders = (from x in
					    InternalDbContext.vw_Reminder
				    select x
				    ).ToList();

			return reminders;
		}
		static void sendEmail(List<vw_Reminder> reminders) {
			string emailHost = null;
			string emailAccount = null;
			string emailPassword = null;
			int emailPort = 0;
			string[] recipients = null;

			emailHost = ConfigurationManager.AppSettings["emailHost"].ToString();
			emailAccount = ConfigurationManager.AppSettings["emailAccount"].ToString();
			emailPassword = ConfigurationManager.AppSettings["emailPassword"].ToString();
			emailPort = Convert.ToInt32(ConfigurationManager.AppSettings["emailPort"]);

			foreach (vw_Reminder reminder in reminders) {
				if (reminder.owner_id != null && !string.IsNullOrEmpty(reminder.Email)) {
					recipients = new string[] { reminder.Email };

					EmailHelper.sendEmail(emailAccount, recipients, null, reminder.text, reminder.details, null, emailHost, emailPort, emailAccount, emailPassword);
				}
			}
		}
	}
}
