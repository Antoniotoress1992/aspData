using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.Protected
{
    public partial class TaskEdit : System.Web.UI.Page
    {
        int clientID = 0;
        int roleID = 0;

        /// <summary>
        /// Returns Task ID from query string, if present
        /// </summary>
        int TaskID
        {
            get
            {
                int id = 0;
                id = Request.Params["q"] == null ? 0 : Convert.ToInt32(SecurityManager.DecryptQueryString(Request.Params["q"].ToString()));

                return id;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            clientID = SessionHelper.getClientId();
            roleID = SessionHelper.getUserRoleId();

            lblTitle.Text = (this.TaskID == 0) ? "Create Task" : "Edit Task";

            if (!Page.IsPostBack)
            {
                bindData();
            }
        }

        private void bindData()
        {
            List<UserStaff> owners = null;
            List<TaskPriority> priorities = null;
            List<TaskStatus> taskStatuses = null;
            int taskID = 0;

            // load task statuses
            taskStatuses = TasksManager.GetStatuses();
            CollectionManager.FillCollection(ddlTaskStatus, "id", "title", taskStatuses.ToList(), false);

            // load system default priorities
            priorities = TasksManager.GetPriorities();
            CollectionManager.FillCollection(ddlPriority, "PriorityID", "PriorityName", priorities, false);

            // load owners for this client
            owners = SecUserManager.GetStaff(clientID);
            CollectionManager.FillCollection(ddlOwner, "UserId", "StaffName", owners);

            // disable owner DDL when not Client
            if (roleID != (int)UserRole.Client)
                ddlOwner.Enabled = false;

            if (this.TaskID > 0)
            {
                fillForm();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("task");

            if (!Page.IsValid)
                return;

            saveTask();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/protected/Tasks.aspx");
        }

        protected void btnSaveNew_Click(object sender, EventArgs e)
        {
            saveTask();

            clearFields();
        }



        private void clearFields()
        {
            txtDateDue.Value = null;
            txtSubject.Text = string.Empty;
            txtDescription.Text = string.Empty;
            ddlPriority.SelectedIndex = 0;
            ddlOwner.SelectedIndex = 0;
            ddlTaskStatus.SelectedIndex = 0;

            txtRecurrenceEndDate.Value = null;
            txtRecurrenceStartDate.Value = null;
            //ddlReminderWhen.SelectedIndex = 0;
            ddlRecurringRepeatFrequency.SelectedIndex = 0;
            //ddlRepeatFrequency.SelectedIndex = 0;
            //ddlAlertType.SelectedIndex = 0;
        }


        protected void lbtnClearRecurrence_Click(object sender, EventArgs e)
        {
            txtRecurrenceEndDate.Text = string.Empty;
            txtRecurrenceStartDate.Text = string.Empty;
            ddlRecurringRepeatFrequency.SelectedIndex = 0;

            if (ViewState["RecurringID"] != null)
            {
                RecurrenceManager.Delete(Convert.ToInt32(ViewState["RecurringID"]));

                // after recurrence is deleted, all tasks associated with recurrence are also deleted.
                // return to activities after deleting
                btnCancel_Click(sender, e);
            }
        }


        protected void lbtnClearReminder_Click(object sender, EventArgs e)
        {
            //ddlReminderWhen.SelectedIndex = 0;
            //ddlRepeatFrequency.SelectedIndex = 0;
            //ddlAlertType.SelectedIndex = 0;

            // delete reminder
            if (ViewState["ReminderID"] != null)
                ReminderManager.Delete(Convert.ToInt32(ViewState["ReminderID"]));
        }

        private void fillForm()
        {
            Reminder reminder = null;
            //Recurrence recurrence = null;
            Task task = null;

            task = TasksManager.Get(this.TaskID);

            if (task != null)
            {
                txtDateDue.Value = task.start_date;
                txtDescription.Text = task.details;
                txtSubject.Text = task.text;
                ddlPriority.SelectedValue = (task.PriorityID ?? 1).ToString();
                ddlOwner.SelectedValue = (task.owner_id ?? 0).ToString();
                ddlTaskStatus.SelectedValue = (task.status_id ?? 0).ToString();

                // contact
                if (task.ContactID != null)
                {
                    hf_contactID.Value = task.ContactID.ToString();
                    txtContact.Text = ContactManager.GetName((int)task.ContactID);
                }

                if (task.Reminder != null && task.Reminder.Count > 0)
                {
                    reminder = task.Reminder.FirstOrDefault();
                    if (reminder != null)
                    {
                        ViewState["ReminderID"] = reminder.ReminderID.ToString();
                        //ddlReminderWhen.SelectedValue = (reminder.ReminderWhen ?? 0).ToString();
                        //ddlReminderRepeatFrequency.SelectedValue = (reminder.RepeatFrequencyID ?? 0).ToString();
                        //ddlAlertType.SelectedValue = (reminder.AlertTypeID ?? 1).ToString();		// email
                    }
                }

                if (task.Recurrence != null)
                {
                    lbtnClearRecurrence.Visible = true;

                    //foreach( Recurrence rec in task.Recurrence )
                    //{

                    //}
                    foreach (Recurrence recurrence in task.Recurrence)
                    {
                        //recurrence = (Recurrence)task.Recurrence;


                        ViewState["RecurringID"] = recurrence.RecurringID.ToString();
                        txtRecurrenceStartDate.Value = recurrence.DateStart;
                        txtRecurrenceEndDate.Value = recurrence.DateEnd;
                        ddlRecurringRepeatFrequency.SelectedValue = (recurrence.RepeatFrequencyID ?? 0).ToString();

                        //repeat daily
                        cbxRecurringDailyEveryDay.Checked = recurrence.IsRepeatDailyEveryDay ?? false;
                        cbxRecurringDailyEveryNDay.Checked = recurrence.IsRepeatDailyForEveryNDays ?? false;
                        txtRecurringDailyEveryNDays.Value = recurrence.RepeatDailyForEveryNDays;

                        // repeat weekly
                        txtRepeatWeeklyEveryNWeeks.Value = recurrence.RepeatWeeklyEveryNWeeks;
                        cbxEveryWeekSun.Checked = recurrence.IsRepeatWeeklyEveryNWeeksSun ?? false;
                        cbxEveryWeekMon.Checked = recurrence.IsRepeatWeeklyEveryNWeeksMon ?? false;
                        cbxEveryWeekTue.Checked = recurrence.IsRepeatWeeklyEveryNWeeksTue ?? false;
                        cbxEveryWeekWed.Checked = recurrence.IsRepeatWeeklyEveryNWeeksWed ?? false;
                        cbxEveryWeekThu.Checked = recurrence.IsRepeatWeeklyEveryNWeeksThur ?? false;
                        cbxEveryWeekFri.Checked = recurrence.IsRepeatWeeklyEveryNWeeksFri ?? false;
                        cbxEveryWeekSat.Checked = recurrence.IsRepeatWeeklyEveryNWeeksSat ?? false;

                        // repeat monthly
                        cbxRecurringMonthlyOnDay.Checked = recurrence.IsRepeatMonthlyOnDay ?? false;
                        cbxRecurringMonthlyOn.Checked = recurrence.IsRepeatMonthlyOn ?? false;

                        txtRecurringMonthlyOnDay.Value = recurrence.RepeatMonthlyOnDay;
                        txtRecurringMonthlyOnDayEvery.Value = recurrence.RepeatMonthlyOnDayEvery;

                        ddlRecurringMonthlyOn.SelectedValue = (recurrence.RepeatMonthlyOn ?? 1).ToString();
                        ddlRecurringMonthlyWeekDay.SelectedValue = (recurrence.RepeatMonthlyOnWeekDay ?? 1).ToString();
                        txtRecurringMonthlyWeekDayOfEveryMonth.Value = recurrence.RepeatMonthlyOnEvery;

                        // repeat yearly
                        cbxRecurringYearlyOnEvery.Checked = recurrence.IsRepeatYearlyOnEvery ?? false;
                        ddlRepeatYearlyOnEveryMonth.SelectedValue = (recurrence.RepeatYearlyMonth ?? 1).ToString();
                        txtRecurringYearlyOnEveryMonthDay.Value = recurrence.RepeatYearlyMonthDay;

                        cbxRecurringYearlyOn.Checked = recurrence.IsRepeatYearlyOn ?? false;
                        ddlRecurringYearlyOn.SelectedValue = (recurrence.RepeatYearlyOn ?? 1).ToString();
                        ddlRecurringYearlyWeekDay.SelectedValue = (recurrence.RepeatYearlyOnWeekDay ?? 1).ToString();
                        ddlRecurringYearlyMonth.SelectedValue = (recurrence.RepeatYearlyOnMonth ?? 1).ToString();

                        showRepeatPanel(recurrence.RepeatFrequencyID ?? 0);
                    }
                }

            }
        }

        private void generateDailySequence(Task task, Recurrence recurrence)
        {
            int dayCount = 0;
            DateTime dateStart = DateTime.MinValue;
            //DateTime dateEnd = DateTime.MinValue;
            TimeSpan diff;
            Task recurrentTask = null;
            Reminder reminder = null;
            int taskID = 0;


            if (recurrence.IsRepeatDailyEveryDay ?? false)
            {
                diff = (DateTime)recurrence.DateEnd - (DateTime)recurrence.DateStart;

                dateStart = (DateTime)recurrence.DateStart;
                dayCount = diff.Days + 1;

                for (int i = 0; i < dayCount; i++)
                {
                    recurrentTask = new Task();
                    recurrentTask.ContactID = task.ContactID;
                    recurrentTask.creator_id = task.creator_id;
                    recurrentTask.details = task.details;
                    recurrentTask.owner_id = task.owner_id;
                    recurrentTask.PriorityID = task.PriorityID;

                    // recurrence sequence
                    recurrentTask.RecurringID = recurrence.RecurringID;

                    recurrentTask.start_date = dateStart;

                    recurrentTask.status_id = 1;
                    recurrentTask.text = task.text;
                    recurrentTask.TaskType = (int)Globals.TaskType.Task;

                    // save task
                    taskID = TasksManager.Save(recurrentTask);

                    // increment date
                    dateStart = dateStart.AddDays(1);
                }
            }
            else if (recurrence.IsRepeatDailyForEveryNDays ?? false)
            {
                dateStart = (DateTime)recurrence.DateStart;

                while (dateStart <= recurrence.DateEnd)
                {
                    recurrentTask = new Task();
                    recurrentTask.ContactID = task.ContactID;
                    recurrentTask.creator_id = task.creator_id;
                    recurrentTask.details = task.details;
                    recurrentTask.owner_id = task.owner_id;
                    recurrentTask.PriorityID = task.PriorityID;

                    // recurrence sequence
                    recurrentTask.RecurringID = recurrence.RecurringID;

                    recurrentTask.start_date = dateStart;

                    recurrentTask.status_id = 1;
                    recurrentTask.text = task.text;
                    recurrentTask.TaskType = (int)Globals.TaskType.Task;

                    // save task
                    taskID = TasksManager.Save(recurrentTask);


                    // increment date
                    dateStart = dateStart.AddDays(recurrence.RepeatDailyForEveryNDays ?? 0);
                }
            }

        }

        private void generateMonthlySequence(Task task, Recurrence recurrence)
        {
            DateTime dateStart = DateTime.MinValue;
            int onDay = 0;
            int everyNmonth = 0;
            Task recurrentTask = null;
            Reminder reminder = null;
            int taskID = 0;
            DateTime startMonthDate = DateTime.MinValue;

            if (recurrence.IsRepeatMonthlyOn ?? false)
            {
                // first, second, third, last week day of month
                dateStart = (DateTime)recurrence.DateStart;

                everyNmonth = recurrence.RepeatMonthlyOnEvery ?? 0;

                while (dateStart <= recurrence.DateEnd)
                {
                    switch (recurrence.RepeatMonthlyOn)
                    {
                        case 1:	// first
                            dateStart = DateHelper.getDate(1, (DayOfWeek)recurrence.RepeatMonthlyOnWeekDay, dateStart);
                            break;
                        case 2:	// second
                            dateStart = DateHelper.getDate(2, (DayOfWeek)recurrence.RepeatMonthlyOnWeekDay, dateStart);
                            break;
                        case 3:	// third
                            dateStart = DateHelper.getDate(3, (DayOfWeek)recurrence.RepeatMonthlyOnWeekDay, dateStart);
                            break;
                        case 4:	// fourth
                            dateStart = DateHelper.getDate(4, (DayOfWeek)recurrence.RepeatMonthlyOnWeekDay, dateStart);
                            break;
                        case 5:	// last
                            dateStart = DateHelper.getDate(5, (DayOfWeek)recurrence.RepeatMonthlyOnWeekDay, dateStart);
                            break;
                    }

                    recurrentTask = new Task();
                    recurrentTask.ContactID = task.ContactID;
                    recurrentTask.creator_id = task.creator_id;
                    recurrentTask.details = task.details;
                    recurrentTask.owner_id = task.owner_id;
                    recurrentTask.PriorityID = task.PriorityID;

                    // recurrence sequence
                    recurrentTask.RecurringID = recurrence.RecurringID;

                    recurrentTask.status_id = 1;
                    recurrentTask.text = task.text;
                    recurrentTask.TaskType = (int)Globals.TaskType.Task;

                    recurrentTask.start_date = dateStart;

                    // save task
                    taskID = TasksManager.Save(recurrentTask);


                    // increment date
                    dateStart = dateStart.AddDays(30 * everyNmonth);
                }
            }
            else if (recurrence.IsRepeatMonthlyOnDay ?? false)
            {
                // specific day of the month
                onDay = recurrence.RepeatMonthlyOnDay ?? 0;

                everyNmonth = recurrence.RepeatMonthlyOnDayEvery ?? 0;

                dateStart = (DateTime)recurrence.DateStart;

                while (dateStart <= recurrence.DateEnd)
                {
                    recurrentTask = new Task();
                    recurrentTask.ContactID = task.ContactID;
                    recurrentTask.creator_id = task.creator_id;
                    recurrentTask.details = task.details;
                    recurrentTask.owner_id = task.owner_id;
                    recurrentTask.PriorityID = task.PriorityID;

                    // recurrence sequence
                    recurrentTask.RecurringID = recurrence.RecurringID;

                    recurrentTask.status_id = 1;
                    recurrentTask.text = task.text;
                    recurrentTask.TaskType = (int)Globals.TaskType.Task;

                    dateStart = new DateTime(dateStart.Year, dateStart.Month, onDay);

                    recurrentTask.start_date = dateStart;

                    // save task
                    taskID = TasksManager.Save(recurrentTask);


                    // increment date
                    dateStart = dateStart.AddDays(30 * everyNmonth);
                }
            }
        }

        private void generateWeeklySequence(Task task, Recurrence recurrence)
        {
            DateTime dateStart = DateTime.MinValue;
            int everyNweeks = recurrence.RepeatWeeklyEveryNWeeks ?? 0;
            bool isMatch = false;
            Task recurrentTask = null;
            //Reminder reminder = null;
            int taskID = 0;
            DateTime weekStartDate = DateTime.MinValue;

            dateStart = (DateTime)recurrence.DateStart;


            while (dateStart <= recurrence.DateEnd)
            {
                weekStartDate = dateStart;

                for (int i = 1; i < 8; i++)
                {

                    isMatch = false;

                    if (weekStartDate.DayOfWeek == DayOfWeek.Sunday && (recurrence.IsRepeatWeeklyEveryNWeeksSun ?? false))
                    {
                        isMatch = true;
                    }
                    else if (weekStartDate.DayOfWeek == DayOfWeek.Monday && (recurrence.IsRepeatWeeklyEveryNWeeksMon ?? false))
                    {
                        isMatch = true;
                    }
                    else if (weekStartDate.DayOfWeek == DayOfWeek.Tuesday && (recurrence.IsRepeatWeeklyEveryNWeeksTue ?? false))
                    {
                        isMatch = true;
                    }
                    else if (weekStartDate.DayOfWeek == DayOfWeek.Wednesday && (recurrence.IsRepeatWeeklyEveryNWeeksWed ?? false))
                    {
                        isMatch = true;
                    }
                    else if (weekStartDate.DayOfWeek == DayOfWeek.Thursday && (recurrence.IsRepeatWeeklyEveryNWeeksThur ?? false))
                    {
                        isMatch = true;
                    }
                    else if (weekStartDate.DayOfWeek == DayOfWeek.Friday && (recurrence.IsRepeatWeeklyEveryNWeeksFri ?? false))
                    {
                        isMatch = true;
                    }
                    else if (weekStartDate.DayOfWeek == DayOfWeek.Saturday && (recurrence.IsRepeatWeeklyEveryNWeeksSat ?? false))
                    {
                        isMatch = true;
                    }

                    if (isMatch)
                    {
                        recurrentTask = new Task();
                        recurrentTask.ContactID = task.ContactID;
                        recurrentTask.creator_id = task.creator_id;
                        recurrentTask.details = task.details;
                        recurrentTask.owner_id = task.owner_id;
                        recurrentTask.PriorityID = task.PriorityID;

                        // recurrence sequence
                        recurrentTask.RecurringID = recurrence.RecurringID;

                        recurrentTask.status_id = 1;
                        recurrentTask.text = task.text;
                        recurrentTask.TaskType = (int)Globals.TaskType.Task;

                        recurrentTask.start_date = weekStartDate;

                        // save task
                        taskID = TasksManager.Save(recurrentTask);


                    }


                    // increment date
                    weekStartDate = weekStartDate.AddDays(1);

                    // safety check
                    if (weekStartDate > recurrence.DateEnd)
                        break;
                }

                dateStart = dateStart.AddDays(7 * everyNweeks);

            }
        }

        //private void generateYearlySequence(Task task, Recurrence recurrence) {
        //    int day = 0;
        //    DateTime dateStart = DateTime.MinValue;
        //    int month = 0;
        //    Task recurrentTask = null;
        //    //Reminder reminder = null;
        //    int taskID = 0;
        //    int year = 0;
        //    int weekday = 0;
        //    int whichOne = 0;

        //    if (recurrence.IsRepeatYearlyOn ?? false) {
        //        whichOne = recurrence.RepeatYearlyOn ?? 0;			// first, second, third, fourth
        //        weekday = recurrence.RepeatYearlyOnWeekDay ?? 0;
        //        month = recurrence.RepeatYearlyOnMonth ?? 0;

        //        dateStart = (DateTime)recurrence.DateStart;

        //        while (dateStart <= recurrence.DateEnd) {
        //            year = dateStart.Year;
        //            dateStart = new DateTime(year, month, 1);
        //            dateStart = DateHelper.getDate(whichOne, (DayOfWeek)recurrence.RepeatYearlyOnWeekDay, dateStart);

        //            recurrentTask = new Task();
        //            recurrentTask.ContactID = task.ContactID;
        //            recurrentTask.creator_id = task.creator_id;
        //            recurrentTask.details = task.details;
        //            recurrentTask.owner_id = task.owner_id;
        //            recurrentTask.PriorityID = task.PriorityID;

        //            // recurrence sequence
        //            recurrentTask.RecurringID = recurrence.RecurringID;

        //            recurrentTask.status_id = 1;
        //            recurrentTask.text = task.text;
        //            recurrentTask.TaskType = (int)Globals.TaskType.Task;

        //            recurrentTask.start_date = dateStart;

        //            // save task
        //            taskID = TasksManager.Save(recurrentTask);



        //            // increment date
        //            dateStart = dateStart.AddYears(1);
        //        }
        //    }
        //    else if (recurrence.IsRepeatYearlyOnEvery ?? false) {
        //        month = Convert.ToInt32(ddlRepeatYearlyOnEveryMonth.SelectedValue);
        //        day = txtRecurringYearlyOnEveryMonthDay.ValueInt;
        //        year = ((DateTime)recurrence.DateStart).Year;

        //        dateStart = (DateTime)recurrence.DateStart;

        //        while (dateStart <= recurrence.DateEnd) {
        //            recurrentTask = new Task();
        //            recurrentTask.ContactID = task.ContactID;
        //            recurrentTask.creator_id = task.creator_id;
        //            recurrentTask.details = task.details;
        //            recurrentTask.owner_id = task.owner_id;
        //            recurrentTask.PriorityID = task.PriorityID;

        //            // recurrence sequence
        //            recurrentTask.RecurringID = recurrence.RecurringID;

        //            recurrentTask.status_id = 1;
        //            recurrentTask.text = task.text;
        //            recurrentTask.TaskType = (int)Globals.TaskType.Task;

        //            // build date for task
        //            dateStart = new DateTime(year, month, day);

        //            recurrentTask.start_date = dateStart;

        //            // save task
        //            taskID = TasksManager.Save(recurrentTask);


        //            // increment date
        //            dateStart = dateStart.AddYears(1);
        //        }
        //    }
        //}

        private void generateRecurrenceSequence(Task task, Recurrence recurrence)
        {

            switch (recurrence.RepeatFrequencyID)
            {
                case (int)Globals.RepeatFrequency.Daily:
                    generateDailySequence(task, recurrence);
                    break;

                case (int)Globals.RepeatFrequency.Weekly:
                    generateWeeklySequence(task, recurrence);
                    break;

                case (int)Globals.RepeatFrequency.Monthly:
                    generateMonthlySequence(task, recurrence);
                    break;

                case (int)Globals.RepeatFrequency.Year:
                    //generateYearlySequence(task, recurrence);
                    break;

                default:
                    break;
            }
        }

        private void saveTask()
        {
            int? contactID = 0;
            Task task = null;
            Recurrence recurrence = null;
            int taskID = 0;
            clientID = SessionHelper.getClientId();

            if (this.TaskID > 0)
            {
                // edit
                task = TasksManager.Get(this.TaskID);
            }
            else
            {
                // new
                task = new Task();
                task.creator_id = this.clientID;
                task.TaskType = (int)Globals.TaskType.Task;
            }

            if (task != null)
            {
                try
                {
                    contactID = Convert.ToInt32(hf_contactID.Value);

                    task.text = txtSubject.Text.Trim();
                    task.details = txtDescription.Text.Trim();
                    task.start_date = txtDateDue.Date;
                    task.status_id = Convert.ToInt32(ddlTaskStatus.SelectedValue);
                    task.owner_id = Convert.ToInt32(ddlOwner.SelectedValue);
                    task.PriorityID = Convert.ToInt32(ddlPriority.SelectedValue);

                    // contact information
                    task.ContactID = contactID > 0 ? contactID : null;

                    using (TransactionScope scope = new TransactionScope())
                    {

                        // recurring
                        if (!string.IsNullOrEmpty(txtRecurrenceStartDate.Text) && task.Recurrence.Count <= 0)
                        {
                            // new recurrence
                            recurrence = new Recurrence();
                            recurrence.TaskID = task.id;
                            saveRecurrence(recurrence);

                            generateRecurrenceSequence(task, recurrence);
                        }
                        else if (!string.IsNullOrEmpty(txtRecurrenceStartDate.Text) && task.Recurrence.Count > 0)
                        {
                            // edit recurrence
                            //recurrence = (Recurrence)task.Recurrence;
                            foreach (Recurrence recu in task.Recurrence)
                            {

                                editRecurrence(recu);


                                RecurrenceManager.DeleteTasks(Convert.ToInt32(ViewState["RecurringID"]));

                                generateRecurrenceSequence(task, recu);
                            }
                        }
                        else if (string.IsNullOrEmpty(txtRecurrenceStartDate.Text) && string.IsNullOrEmpty(txtRecurrenceEndDate.Text) &&
                            task.Recurrence != null && ViewState["RecurringID"] != null)
                        {
                            // delete recurrence along with tasks
                            RecurrenceManager.Delete(Convert.ToInt32(ViewState["RecurringID"]));
                        }
                        else
                        {
                            // save task with no recurrence
                            taskID = TasksManager.Save(task);
                        }

                        // commit changes to DB
                        scope.Complete();
                    }

                    lblMessage.Text = "Task saved successfully.";
                    lblMessage.CssClass = "ok";
                }
                catch (Exception ex)
                {
                    Core.EmailHelper.emailError(ex);

                    lblMessage.Text = "Task was not saved.";
                    lblMessage.CssClass = "error";
                }
            }
        }

        private void saveRecurrence(Recurrence recurrence)
        {
            recurrence.IsActive = true;

            recurrence.DateEnd = txtRecurrenceEndDate.Date;

            recurrence.DateStart = txtRecurrenceStartDate.Date;

            recurrence.RepeatFrequencyID = ddlRecurringRepeatFrequency.SelectedIndex > 0 ? (int?)Convert.ToInt32(ddlRecurringRepeatFrequency.SelectedValue) : null;

            //repeat daily
            recurrence.IsRepeatDailyEveryDay = cbxRecurringDailyEveryDay.Checked;
            recurrence.IsRepeatDailyForEveryNDays = cbxRecurringDailyEveryNDay.Checked;
            recurrence.RepeatDailyForEveryNDays = txtRecurringDailyEveryNDays.ValueInt;

            // repeat weekly
            recurrence.RepeatWeeklyEveryNWeeks = txtRepeatWeeklyEveryNWeeks.ValueInt;
            recurrence.IsRepeatWeeklyEveryNWeeksSun = cbxEveryWeekSun.Checked;
            recurrence.IsRepeatWeeklyEveryNWeeksMon = cbxEveryWeekMon.Checked;
            recurrence.IsRepeatWeeklyEveryNWeeksTue = cbxEveryWeekTue.Checked;
            recurrence.IsRepeatWeeklyEveryNWeeksWed = cbxEveryWeekWed.Checked;
            recurrence.IsRepeatWeeklyEveryNWeeksThur = cbxEveryWeekThu.Checked;
            recurrence.IsRepeatWeeklyEveryNWeeksFri = cbxEveryWeekFri.Checked;
            recurrence.IsRepeatWeeklyEveryNWeeksSat = cbxEveryWeekSat.Checked;

            // repeat monthly
            recurrence.IsRepeatMonthlyOnDay = cbxRecurringMonthlyOnDay.Checked;
            recurrence.IsRepeatMonthlyOn = cbxRecurringMonthlyOn.Checked;

            recurrence.RepeatMonthlyOnDay = txtRecurringMonthlyOnDay.ValueInt;
            recurrence.RepeatMonthlyOnDayEvery = txtRecurringMonthlyOnDayEvery.ValueInt;

            recurrence.RepeatMonthlyOn = Convert.ToInt32(ddlRecurringMonthlyOn.SelectedValue);
            recurrence.RepeatMonthlyOnWeekDay = Convert.ToInt32(ddlRecurringMonthlyWeekDay.SelectedValue);
            recurrence.RepeatMonthlyOnEvery = txtRecurringMonthlyWeekDayOfEveryMonth.ValueInt;


            // yearly
            recurrence.IsRepeatYearlyOnEvery = cbxRecurringYearlyOnEvery.Checked;
            recurrence.RepeatYearlyMonth = Convert.ToInt32(ddlRepeatYearlyOnEveryMonth.SelectedValue);
            recurrence.RepeatYearlyMonthDay = txtRecurringYearlyOnEveryMonthDay.ValueInt;

            recurrence.IsRepeatYearlyOn = cbxRecurringYearlyOn.Checked;
            recurrence.RepeatYearlyOn = Convert.ToInt32(ddlRecurringYearlyOn.SelectedValue);
            recurrence.RepeatYearlyOnWeekDay = Convert.ToInt32(ddlRecurringYearlyWeekDay.SelectedValue);
            recurrence.RepeatYearlyOnMonth = Convert.ToInt32(ddlRecurringYearlyMonth.SelectedValue);

            // save recurrence
            recurrence = RecurrenceManager.Save(recurrence);
        }

        private void editRecurrence(Recurrence recurrence)
        {
            recurrence.IsActive = true;

            recurrence.DateEnd = txtRecurrenceEndDate.Date;

            recurrence.DateStart = txtRecurrenceStartDate.Date;

            recurrence.RepeatFrequencyID = ddlRecurringRepeatFrequency.SelectedIndex > 0 ? (int?)Convert.ToInt32(ddlRecurringRepeatFrequency.SelectedValue) : null;

            //repeat daily
            recurrence.IsRepeatDailyEveryDay = cbxRecurringDailyEveryDay.Checked;
            recurrence.IsRepeatDailyForEveryNDays = cbxRecurringDailyEveryNDay.Checked;
            recurrence.RepeatDailyForEveryNDays = txtRecurringDailyEveryNDays.ValueInt;

            // repeat weekly
            recurrence.RepeatWeeklyEveryNWeeks = txtRepeatWeeklyEveryNWeeks.ValueInt;
            recurrence.IsRepeatWeeklyEveryNWeeksSun = cbxEveryWeekSun.Checked;
            recurrence.IsRepeatWeeklyEveryNWeeksMon = cbxEveryWeekMon.Checked;
            recurrence.IsRepeatWeeklyEveryNWeeksTue = cbxEveryWeekTue.Checked;
            recurrence.IsRepeatWeeklyEveryNWeeksWed = cbxEveryWeekWed.Checked;
            recurrence.IsRepeatWeeklyEveryNWeeksThur = cbxEveryWeekThu.Checked;
            recurrence.IsRepeatWeeklyEveryNWeeksFri = cbxEveryWeekFri.Checked;
            recurrence.IsRepeatWeeklyEveryNWeeksSat = cbxEveryWeekSat.Checked;

            // repeat monthly
            recurrence.IsRepeatMonthlyOnDay = cbxRecurringMonthlyOnDay.Checked;
            recurrence.IsRepeatMonthlyOn = cbxRecurringMonthlyOn.Checked;

            recurrence.RepeatMonthlyOnDay = txtRecurringMonthlyOnDay.ValueInt;
            recurrence.RepeatMonthlyOnDayEvery = txtRecurringMonthlyOnDayEvery.ValueInt;

            recurrence.RepeatMonthlyOn = Convert.ToInt32(ddlRecurringMonthlyOn.SelectedValue);
            recurrence.RepeatMonthlyOnWeekDay = Convert.ToInt32(ddlRecurringMonthlyWeekDay.SelectedValue);
            recurrence.RepeatMonthlyOnEvery = txtRecurringMonthlyWeekDayOfEveryMonth.ValueInt;


            // yearly
            recurrence.IsRepeatYearlyOnEvery = cbxRecurringYearlyOnEvery.Checked;
            recurrence.RepeatYearlyMonth = Convert.ToInt32(ddlRepeatYearlyOnEveryMonth.SelectedValue);
            recurrence.RepeatYearlyMonthDay = txtRecurringYearlyOnEveryMonthDay.ValueInt;

            recurrence.IsRepeatYearlyOn = cbxRecurringYearlyOn.Checked;
            recurrence.RepeatYearlyOn = Convert.ToInt32(ddlRecurringYearlyOn.SelectedValue);
            recurrence.RepeatYearlyOnWeekDay = Convert.ToInt32(ddlRecurringYearlyWeekDay.SelectedValue);
            recurrence.RepeatYearlyOnMonth = Convert.ToInt32(ddlRecurringYearlyMonth.SelectedValue);

            // save recurrence
            recurrence = RecurrenceManager.Edit(recurrence);
        }

        //private Reminder getReminder() {
        //    Reminder reminder = null;

        //    if (ddlReminderWhen.SelectedIndex > 0) {
        //        reminder = new Reminder();

        //        reminder.IsActive = true;

        //        reminder.ReminderWhen = Convert.ToInt32(ddlReminderWhen.SelectedValue);

        //        reminder.RepeatFrequencyID = Convert.ToInt32(ddlReminderRepeatFrequency.SelectedValue);

        //        reminder.AlertTypeID = ddlAlertType.SelectedIndex > 0 ? (int?)Convert.ToInt32(ddlAlertType.SelectedValue) : null;
        //    }

        //    return reminder;
        //}

        protected void ddlRecurringRepeatFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            int repeatValue = Convert.ToInt32(ddlRecurringRepeatFrequency.SelectedIndex);

            showRepeatPanel(repeatValue);

        }

        private void showRepeatPanel(int repeatValue)
        {
            pnlRepeatDaily.Visible = false;
            pnlRepeatWeekly.Visible = false;
            pnlRepeatMonthly.Visible = false;
            pnlRepeatYearly.Visible = false;

            switch (repeatValue)
            {
                case 1:			// DAILY
                    pnlRepeatDaily.Visible = true;
                    break;
                case 2:			// Weekly
                    pnlRepeatWeekly.Visible = true;
                    break;
                case 3:			// monthly
                    pnlRepeatMonthly.Visible = true;
                    break;
                case 4:
                    pnlRepeatYearly.Visible = true;
                    break;
                default:
                    break;
            }
        }


        protected void cbxRecurringDailyEveryNDay_Validate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (!cbxRecurringDailyEveryDay.Checked && !cbxRecurringDailyEveryNDay.Checked)
            {
                args.IsValid = false;
            }
        }
        protected void RepeatWeeklyEveryNWeeks_Validate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;

            if (!cbxEveryWeekSun.Checked &&
                !cbxEveryWeekMon.Checked &&
                !cbxEveryWeekTue.Checked &&
                !cbxEveryWeekWed.Checked &&
                !cbxEveryWeekThu.Checked &&
                !cbxEveryWeekFri.Checked &&
                !cbxEveryWeekSat.Checked)
            {
                args.IsValid = false;
            }
        }

        protected void RepeatMonthly_Validate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;

            if (!cbxRecurringMonthlyOn.Checked && !cbxRecurringMonthlyOnDay.Checked)
            {
                args.IsValid = false;
            }

            if (cbxRecurringMonthlyOnDay.Checked && (txtRecurringMonthlyOnDay.ValueInt == 0 || txtRecurringMonthlyOnDayEvery.ValueInt == 0))
                args.IsValid = false;
            else if (cbxRecurringMonthlyOn.Checked && txtRecurringMonthlyWeekDayOfEveryMonth.ValueInt == 0)
                args.IsValid = false;
        }

        protected void RepeatYearly_Validate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;

            if (!cbxRecurringYearlyOn.Checked && !cbxRecurringYearlyOnEvery.Checked)
            {
                args.IsValid = false;
            }

            if (cbxRecurringYearlyOnEvery.Checked && txtRecurringYearlyOnEveryMonthDay.ValueInt == 0)
                args.IsValid = false;
        }

    }
}