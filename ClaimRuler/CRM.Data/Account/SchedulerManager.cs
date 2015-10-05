using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Infragistics.WebUI.WebSchedule;
using Infragistics.WebUI.Shared;
using CRM.Data.Entities;
using CRM.Data.Account;

namespace CRM.Data.Account {
	public static class SchedulerManager {
		public static void BindAppointments(WebScheduleInfo scheduleInfo, int clientID, DateTime fromDate, DateTime endDate) {
			List<LeadTask> tasks = TasksManager.GetLeadTask(clientID, fromDate, endDate).ToList();

			BindAppointment(scheduleInfo, tasks);
		}

		public static void BindAppointment(WebScheduleInfo scheduleInfo, List<LeadTask> tasks) {
			Appointment appointment = null;

			if (tasks != null) {
				scheduleInfo.Activities.Clear();


				foreach (LeadTask task in tasks) {
					appointment = new Appointment(scheduleInfo);
					//appointment.Key = task.id.ToString();
					appointment.DataKey = task.id.ToString();

					//appointment.DataKey = task.id.ToString();
					appointment.StartDateTime = new SmartDate((DateTime)task.start_date);

					if (task.isAllDay) {
						appointment.AllDayEvent = task.isAllDay;
					}
					else {
						TimeSpan duration = ((DateTime)task.end_date) - ((DateTime)task.start_date);
						appointment.EndDateTime = new SmartDate((DateTime)task.end_date);
						appointment.Duration = duration;
					}					
						
				
					if (task.text.ToLower().Equals("alert")) {
						appointment.AllDayEvent = true;
						appointment.Subject = task.details;
					}
					else {
						appointment.Subject = task.text;
						appointment.Location = task.details;
					}
															
					appointment.Description = task.details;

					appointment.EnableReminder = task.isReminder;
					if (task.reminderInterval != null)
						appointment.ReminderInterval = new TimeSpan(0, (int)task.reminderInterval, 0); 

					if (task.priorityID > 0) {
						switch(task.priorityID) {
							case (int)Globals.Task_Priority.Low:
								appointment.Importance = Importance.Low;
								break;

							case (int)Globals.Task_Priority.Normal:
								appointment.Importance = Importance.Normal;
								break;

							case (int)Globals.Task_Priority.High:
								appointment.Importance = Importance.High;
								break;
						}
					}

					//appointment.ResourceKey = task.owner_id.ToString();
					appointment.ResourceKey = task.resourceKey.ToString();

					if (task.status_id == (int)Globals.Task_Status.Active && ((DateTime)task.start_date) < DateTime.Now)
						appointment.Status = ActivityStatus.Expired;
					else
						appointment.Status = ActivityStatus.Normal;

					scheduleInfo.Activities.Add(appointment);
				}

			}
		}

		public static void BindUserAppointments(WebScheduleInfo scheduleInfo, int userID, DateTime fromDate, DateTime endDate) {
			List<LeadTask> tasks = TasksManager.GetLeadTaskByUserID(userID, fromDate, endDate).ToList();
			BindAppointment(scheduleInfo, tasks);
			
		}

		public static DateTime GetScheduleFromDate(WebScheduleInfo scheduleInfo) {
			return scheduleInfo.ActiveDayUtc.Value;
		}

		public static DateTime GetScheduleEndDate(WebScheduleInfo scheduleInfo, string view) {
			string endDate = null;
			SmartDate activeDate = null;

			if (view == "day")
				//endDate = string.Format("{0} 11:59:59 PM", scheduleInfo.ActiveDayUtc.ToShortDateString());
				activeDate = scheduleInfo.ActiveDayUtc;
			else if (view == "days")
				activeDate = scheduleInfo.ActiveDayUtc.AddDays(5);
				//endDate = string.Format("{0} 11:59:59 PM", scheduleInfo.ActiveDayUtc.AddDays(5).ToShortDateString());


			endDate = string.Format("{0} 11:59:59 PM", activeDate.ToShortDateString());

			return Convert.ToDateTime(endDate);
		}

		public static DateTime GetScheduleEndDate(WebDayView dayView) {
			string srtEndDate = null;
			SmartDate endDate = null;

			if (dayView.VisibleDays == 1)
				endDate = dayView.WebScheduleInfo.ActiveDayUtc;
			else if (dayView.VisibleDays == 5) {
				endDate = dayView.WebScheduleInfo.ActiveDayUtc.AddDays(4);
			}

			srtEndDate = string.Format("{0} 11:59:59 PM", endDate.ToShortDateString());

			return Convert.ToDateTime(srtEndDate);
		}

		public static List<vw_Reminder> GetUpcomingReminders(int userID) {
			List<vw_Reminder> reminders = null;

			reminders = (from x in
					     DbContextHelper.DbContext.vw_Reminder
						where x.owner_id == userID
					   select x
				    ).ToList();

			return reminders;
		}
	}
}
