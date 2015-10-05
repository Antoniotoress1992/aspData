using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class RecurrenceManager {
		public static Recurrence Save(Recurrence recurring) {
			if (recurring.RecurringID == 0) {
				DbContextHelper.DbContext.Add(recurring);
			}

			DbContextHelper.DbContext.SaveChanges();

			return recurring;
		}

        public static Recurrence Edit(Recurrence recurring)
        {
            int recid=recurring.RecurringID;
            if (recurring.RecurringID != 0)
            {
               Recurrence reccu=(from rec in DbContextHelper.DbContext.Recurrence
                                   where rec.RecurringID==recid select rec).FirstOrDefault();

               reccu.IsActive = true;

               reccu.DateEnd = recurring.DateEnd;

               reccu.DateStart = recurring.DateStart;

               reccu.RepeatFrequencyID = recurring.RepeatFrequencyID;

               //repeat daily
               reccu.IsRepeatDailyEveryDay = recurring.IsRepeatDailyEveryDay;
               reccu.IsRepeatDailyForEveryNDays = recurring.IsRepeatDailyForEveryNDays;
               reccu.RepeatDailyForEveryNDays = recurring.RepeatDailyForEveryNDays;

               // repeat weekly
               reccu.RepeatWeeklyEveryNWeeks = recurring.RepeatWeeklyEveryNWeeks;
               reccu.IsRepeatWeeklyEveryNWeeksSun = recurring.IsRepeatWeeklyEveryNWeeksSun;
               reccu.IsRepeatWeeklyEveryNWeeksMon = recurring.IsRepeatWeeklyEveryNWeeksMon;
               reccu.IsRepeatWeeklyEveryNWeeksTue = recurring.IsRepeatWeeklyEveryNWeeksTue;
               reccu.IsRepeatWeeklyEveryNWeeksWed = recurring.IsRepeatWeeklyEveryNWeeksWed;
               reccu.IsRepeatWeeklyEveryNWeeksThur = recurring.IsRepeatWeeklyEveryNWeeksThur;
               reccu.IsRepeatWeeklyEveryNWeeksFri = recurring.IsRepeatWeeklyEveryNWeeksFri;
               reccu.IsRepeatWeeklyEveryNWeeksSat = recurring.IsRepeatWeeklyEveryNWeeksSat;

               // repeat monthly
               reccu.IsRepeatMonthlyOnDay = recurring.IsRepeatMonthlyOnDay;
               reccu.IsRepeatMonthlyOn = recurring.IsRepeatMonthlyOn;

               reccu.RepeatMonthlyOnDay = recurring.RepeatMonthlyOnDay;
               reccu.RepeatMonthlyOnDayEvery = recurring.RepeatMonthlyOnDayEvery;

               reccu.RepeatMonthlyOn = recurring.RepeatMonthlyOn;
               reccu.RepeatMonthlyOnWeekDay = recurring.RepeatMonthlyOnWeekDay;
               reccu.RepeatMonthlyOnEvery = recurring.RepeatMonthlyOnEvery;


               // yearly
               reccu.IsRepeatYearlyOnEvery = recurring.IsRepeatYearlyOnEvery;
               reccu.RepeatYearlyMonth = recurring.RepeatYearlyMonth;
               reccu.RepeatYearlyMonthDay = recurring.RepeatYearlyMonthDay;

               reccu.IsRepeatYearlyOn = recurring.IsRepeatYearlyOn;
               reccu.RepeatYearlyOn = recurring.RepeatYearlyOn;
               reccu.RepeatYearlyOnWeekDay = recurring.RepeatYearlyOnWeekDay;
               reccu.RepeatYearlyOnMonth = recurring.RepeatYearlyOnMonth;
                
            }
            DbContextHelper.DbContext.SaveChanges();
            

            return recurring;
        }

		public static void Delete(int id) {
			Recurrence recurrence = (from x in DbContextHelper.DbContext.Recurrence
							   where x.RecurringID == id
							   select x).FirstOrDefault();


			DbContextHelper.DbContext.DeleteObject(recurrence);

			// Apply the delete to the database 
			DbContextHelper.DbContext.SaveChanges();
		}

		public static void DeleteTasks(int id) {
			//DbContextHelper.DbContext.ExecuteStoreCommand("DELETE FROM Task WHERE RecurringID = {0}", id);

            var taskdelete = DbContextHelper.DbContext.Task.Where(x => x.RecurringID == id).ToList();
            foreach (CRM.Data.Entities.Task objR in taskdelete)
            {
                DbContextHelper.DbContext.DeleteObject(objR);
            }
            DbContextHelper.DbContext.SaveChanges();

		}
	}
}
