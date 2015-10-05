using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data.Objects;

//using System.Data.Objects.SqlClient;
using System.Data.Entity;
using System.Data.SqlClient;
using Infragistics.WebUI.WebSchedule;
using System.Linq.Expressions;
using LinqKit;
using System.Data.Linq.SqlClient;
using CRM.Data.Entities;
using System.Data.Entity.Core.Objects;

namespace CRM.Data.Account {
	public static class TasksManager {


		public static void Delete(int id) {
			var task = new Task { id = id };

            DbContextHelper.DbContext.Task.Attach(task);

			DbContextHelper.DbContext.DeleteObject(task);

			DbContextHelper.DbContext.SaveChanges();
		}

		public static List<Task> GetAll(DateTime startDate) {

			var list = from x in DbContextHelper.DbContext.Task
					 where EntityFunctions.TruncateTime(x.start_date) >= startDate.Date
					 select x;

			return list.ToList();
		}

		public static List<LeadTask> GetAll() {
			List<LeadTask> tasks = null;

			tasks = (from x in DbContextHelper.DbContext.Task.Include("Leads").Include("SecUser").Include("TaskStatus")
				    select new LeadTask {
					    creator_id = x.creator_id,
					    details = x.details,
					    end_date = x.end_date,
					    id = x.id,
					    lead_id = x.lead_id,
					    lead_name = x.Leads == null ? "" : x.Leads.ClaimantFirstName + " " + x.Leads.ClaimantLastName,
					    owner_id = x.owner_id,
					    owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
					    start_date = x.start_date,
					    status_id = x.status_id,
                        statusName = x.Leads.StatusMaster == null ? "" : x.Leads.StatusMaster.StatusName,
					    text = x.text
				    }).ToList<LeadTask>();

			return tasks;
		}

		//public static List<LeadTask> GetLeadTask() {
		//     List<LeadTask> tasks = null;

		//     tasks = (from x in DbContextHelper.DbContext.Tasks.Include("Lead").Include("SecUser").Include("TaskStatus")				    
		//              select new LeadTask {
		//                   creator_id = x.creator_id,
		//                   details = x.details,
		//                   end_date = x.end_date,
		//                   id = x.id,
		//                   lead_id = x.lead_id,
		//                   lead_name = x.Lead == null ? "" : x.Lead.ClaimantFirstName + " " + x.Lead.ClaimantLastName,
		//                   owner_id = x.owner_id,
		//                   owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
		//                   start_date = x.start_date,
		//                   status_id = x.status_id,
		//                   statusName = x.TaskStatus == null ? "" : x.TaskStatus.label,
		//                   text = x.text
		//              }).ToList<LeadTask>();

		//     return tasks;
		//}

		public static List<LeadTask> GetEvents(Expression<Func<Task, bool>> predicate) {
			List<LeadTask> tasks = null;

			tasks = (from x in DbContextHelper.DbContext.Task
						.AsExpandable()
						.OrderBy(x => x.start_date)
						.Where(predicate)

				    select new LeadTask {
					    creator_id = x.creator_id,
					    details = x.details,
					    end_date = x.end_date,
					    id = x.id,
					    lead_id = x.lead_id,
					    lead_name = x.Leads == null ? "" : x.Leads.ClaimantFirstName + " " + x.Leads.ClaimantLastName,
					    owner_id = x.owner_id,

					    // for webSchedule
					    resourceKey = x.owner_id,

					    owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
					    start_date = x.start_date,
					    status_id = x.status_id,
					    text = x.text,
					    isReminder = x.IsReminder ?? false
				    }).ToList<LeadTask>();

			return tasks;
		}

		public static LeadTask GetLeadTask(int id) {
			LeadTask task = null;

			task = (from x in DbContextHelper.DbContext.Task.Include("Leads").Include("SecUser").Include("TaskStatus")
				   where x.id == id
				   select new LeadTask {
					   creator_id = x.creator_id,
					   details = x.details,
					   end_date = x.end_date,
					   id = x.id,
					   lead_id = x.lead_id,
					   lead_name = x.Leads == null ? "" : x.Leads.ClaimantFirstName + " " + x.Leads.ClaimantLastName,
					   owner_id = x.owner_id,
					   owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
					   start_date = x.start_date,
					   status_id = x.status_id,
                       statusName = x.Leads.StatusMaster == null ? "" : x.Leads.StatusMaster.StatusName,
					   text = x.text,
					   master_status_id = x.master_status_id,
					   lead_policy_id = x.lead_policy_id,
					   reminderInterval = x.ReminderInterval ?? 0,
					   policy_type = x.policy_type		// 1 = homeowners...

				   }).FirstOrDefault<LeadTask>();

			return task;
		}

		public static LeadTask GetPolicyReminderTask(int leadID, int leadPolicyID) {
			LeadTask task = null;

			task = (from x in DbContextHelper.DbContext.Task.Include("Leads").Include("SecUser").Include("TaskStatus")
				   where x.lead_policy_id == leadPolicyID &&
						x.lead_id == leadID
				   select new LeadTask {
					   creator_id = x.creator_id,
					   details = x.details,
					   end_date = x.end_date,
					   id = x.id,
					   lead_id = x.lead_id,
					   lead_name = x.Leads == null ? "" : x.Leads.ClaimantFirstName + " " + x.Leads.ClaimantLastName,
					   owner_id = x.owner_id,
					   owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
					   start_date = x.start_date,
					   status_id = x.status_id,
                       statusName = x.Leads.StatusMaster == null ? "" : x.Leads.StatusMaster.StatusName,//x.TaskStatus == null ? "" : x.TaskStatus.label,
					   text = x.text,
					   master_status_id = x.master_status_id,
					   lead_policy_id = x.lead_policy_id,
					   reminderInterval = x.ReminderInterval ?? 0,
					   policy_type = x.policy_type		// 1 = homeowners...

				   }).FirstOrDefault<LeadTask>();

			return task;
		}

		public static IQueryable<LeadTask> GetLeadTask(int clientID, DateTime fromDate, DateTime endDate) {
			IQueryable<LeadTask> tasks = null;

			tasks = from x in DbContextHelper.DbContext.Task.Include("Leads").Include("SecUser").Include("TaskStatus")
				   where EntityFunctions.TruncateTime(x.start_date) >= fromDate.Date &&
					    EntityFunctions.TruncateTime(x.end_date) <= endDate.Date &&
					    x.creator_id == clientID
				   orderby x.start_date ascending
				   select new LeadTask {
					   creator_id = x.creator_id,
					   details = x.details,
					   end_date = x.end_date,
					   id = x.id,
					   lead_id = x.lead_id,
					   lead_name = x.Leads == null ? "" : x.Leads.ClaimantFirstName + " " + x.Leads.ClaimantLastName,
					   //lead_name = x.Lead == null ? "" : "<a href='NewLead.aspx?id=" + System.Data.Objects.SqlClient.SqlFunctions.StringConvert((decimal)x.lead_id).Trim() +
					   //           "'>" + x.Lead.ClaimantFirstName + " " + x.Lead.ClaimantLastName + "<a/>",
					   owner_id = x.owner_id,
					   owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
					   start_date = x.start_date,
					   status_id = x.status_id,
                       statusName = x.Leads.StatusMaster == null ? "" : x.Leads.StatusMaster.StatusName,
					   text = x.text,
					   master_status_id = x.master_status_id,
					   lead_policy_id = x.lead_policy_id,
					   policy_type = x.policy_type		// 1 = homeowners...
				   };

			return tasks;
		}

		public static List<Task> GetTasks(Expression<Func<Task, bool>> predicate) {
			List<Task> tasks = null;

			tasks = DbContextHelper.DbContext.Task
					.AsExpandable()
					.OrderBy(x => x.start_date)
					.Where(predicate)						
					.ToList();

			return tasks;
		}
		public static List<Task> GetTasks(DateTime fromDate) {
			List<Task> tasks = null;

			tasks = (from x in
				    
					    DbContextHelper.DbContext.Task
				    where SqlMethods.DateDiffMinute(x.start_date, fromDate) == 0
				    select x).ToList();

			return tasks;
		}

		public static List<LeadTask> GetLeadTask(Expression<Func<Task, bool>> predicate, DateTime fromDate, DateTime endDate) {
			List<LeadTask> tasks = null;

			//tasks = DbContextHelper.DbContext.Tasks
			//			.AsExpandable()
			//			.Where(predicate).ToList();

			tasks = (from x in DbContextHelper.DbContext.Task
						.AsExpandable()
						.OrderBy(x => x.start_date).ThenBy(x => x.PriorityID)
						.Where(predicate)

				    select new LeadTask {
					    creator_id = x.creator_id,
					    details = x.details,
					    end_date = x.end_date, 
					    id = x.id,
					    lead_id = x.lead_id,
					    lead_name = x.Leads == null ? "" : x.Leads.InsuredName, //ClaimantFirstName + " " + x.Leads.ClaimantLastName,
					    //lead_name = x.Lead == null ? "" : "<a href='NewLead.aspx?id=" + System.Data.Objects.SqlClient.SqlFunctions.StringConvert((decimal)x.lead_id).Trim() +
					    //		  "'>" + x.Lead.ClaimantFirstName + " " + x.Lead.ClaimantLastName + "<a/>",
					    owner_id = x.owner_id,

					    // for webSchedule
					    resourceKey = x.owner_id,

					    owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
					    start_date = x.start_date,
					    status_id = x.status_id,
					    statusName ="", //x.TaskStatus == null ? "" : (x.end_date < endDate ? "Expired" : x.TaskStatus.label),
					    text = x.text,
					    master_status_id = x.master_status_id,
					    lead_policy_id = x.lead_policy_id,
					    policy_type = x.policy_type,		// 1 = homeowners...
					    priorityID = x.PriorityID ?? 1,
					    priorityName = x.PriorityID != null ? x.TaskPriority.PriorityName : "Low",
					    isAllDay = x.isAllDay ?? false,
					    reminderInterval = x.ReminderInterval ?? 0,
					    isReminder = x.IsReminder ?? false
				    }).ToList<LeadTask>();

			return tasks;
		}

		public static List<LeadTask> GetLeadTaskByClientId(int clientID) {
			List<LeadTask> tasks = null;

			tasks = (from x in DbContextHelper.DbContext.Task.Include("Leads").Include("SecUser").Include("TaskStatus")
				    where x.creator_id == clientID
				    orderby x.start_date ascending
				    select new LeadTask {
					    creator_id = x.creator_id,
					    details = x.details,
					    end_date = x.end_date,
					    id = x.id,
					    lead_id = x.lead_id,
					    lead_name = x.Leads == null ? "" : x.Leads.ClaimantFirstName + " " + x.Leads.ClaimantLastName,
					    owner_id = x.owner_id,
					    owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
					    start_date = x.start_date,
					    status_id = x.status_id,
					    statusName = "",//x.TaskStatus == null ? "" : x.TaskStatus.label,
					    text = x.text,
					    master_status_id = x.master_status_id,
					    lead_policy_id = x.lead_policy_id,
					    reminderInterval = x.ReminderInterval ?? 0,
					    policy_type = x.policy_type		// 1 = homeowners...

				    }).ToList<LeadTask>();

			return tasks;
		}

		public static IQueryable<LeadTask> GetLeadTask(DateTime fromDate, DateTime endDate) {
			IQueryable<LeadTask> tasks = null;

			tasks = from x in DbContextHelper.DbContext.Task.Include("Leads").Include("SecUser").Include("TaskStatus")
				   where EntityFunctions.TruncateTime(x.start_date) >= fromDate.Date &&
					    EntityFunctions.TruncateTime(x.end_date) <= endDate.Date
				   orderby x.start_date ascending
				   select new LeadTask {
					   creator_id = x.creator_id,
					   details = x.details,
					   end_date = x.end_date,
					   id = x.id,
					   lead_id = x.lead_id,
					   lead_name = x.Leads == null ? "" : x.Leads.ClaimantFirstName + " " + x.Leads.ClaimantLastName,
					   //lead_name = x.Lead == null ? "" : "<a id='" + System.Data.Objects.SqlClient.SqlFunctions.StringConvert((decimal)x.lead_id).Trim() + "' href='NewLead.aspx?id=" + System.Data.Objects.SqlClient.SqlFunctions.StringConvert((decimal)x.lead_id).Trim() +
					   //           "'>" + x.Lead.ClaimantFirstName + " " + x.Lead.ClaimantLastName + "</a>",
					   owner_id = x.owner_id,
					   owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
					   start_date = x.start_date,
					   status_id = x.status_id,
					   statusName = "",//x.TaskStatus == null ? "" : x.TaskStatus.label,
					   text = x.text,
					   master_status_id = x.master_status_id,
					   lead_policy_id = x.lead_policy_id,
					   reminderInterval = x.ReminderInterval ?? 0,
					   policy_type = x.policy_type		// 1 = homeowners...

				   };

			return tasks;
		}

		//public static List<LeadTask> GetLeadTask(DateTime startDate) {
		//     List<LeadTask> tasks = null;

		//     tasks = (from x in DbContextHelper.DbContext.Tasks.Include("Lead").Include("SecUser").Include("TaskStatus")
		//              where EntityFunctions.TruncateTime(x.start_date) >= startDate.Date
		//              select new LeadTask {
		//                   creator_id = x.creator_id,
		//                   details = x.details,
		//                   end_date = x.end_date,
		//                   id = x.id,
		//                   lead_id = x.lead_id,
		//                   lead_name = x.Lead == null ? "" : "<a href='NewLead.aspx?id=" + System.Data.Objects.SqlClient.SqlFunctions.StringConvert((decimal)x.lead_id).Trim() +
		//                              "'>" + x.Lead.ClaimantFirstName + " " + x.Lead.ClaimantLastName + "<a/>",
		//                   owner_id = x.owner_id,
		//                   owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
		//                   start_date = x.start_date,
		//                   status_id = x.status_id,
		//                   statusName = x.TaskStatus == null ? "" : x.TaskStatus.label,
		//                   text = x.text,
		//                   master_status_id = x.master_status_id,
		//                   lead_policy_id = x.lead_policy_id

		//              }).ToList<LeadTask>();

		//     return tasks;
		//}

		public static List<LeadTask> GetLeadTaskByUserID(int userID) {
			List<LeadTask> tasks = null;

			tasks = (from x in DbContextHelper.DbContext.Task.Include("Leads").Include("SecUser").Include("TaskStatus")
				    where x.owner_id == userID
				    select new LeadTask {
					    creator_id = x.creator_id,
					    details = x.details,
					    end_date = x.end_date,
					    id = x.id,
					    lead_id = x.lead_id,
					    lead_name = x.Leads == null ? "" : x.Leads.ClaimantFirstName + " " + x.Leads.ClaimantLastName,
					    owner_id = x.owner_id,
					    owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
					    start_date = x.start_date,
					    status_id = x.status_id,
					    statusName = "",//x.TaskStatus == null ? "" : x.TaskStatus.label,
					    text = x.text,
					    master_status_id = x.master_status_id,
					    lead_policy_id = x.lead_policy_id,
					    reminderInterval = x.ReminderInterval ?? 0,
					    policy_type = x.policy_type		// 1 = homeowners...

				    }).ToList<LeadTask>();

			return tasks;
		}

		public static IQueryable<LeadTask> GetLeadTaskByUserID(int userID, DateTime fromDate, DateTime endDate) {
			IQueryable<LeadTask> tasks = null;

			tasks = from x in DbContextHelper.DbContext.Task.Include("Leads").Include("SecUser").Include("TaskStatus")
				   where x.owner_id == userID &&
					    EntityFunctions.TruncateTime(x.start_date) >= fromDate.Date &&
					    EntityFunctions.TruncateTime(x.end_date) <= endDate.Date
				   orderby x.start_date ascending
				   select new LeadTask {
					   creator_id = x.creator_id,
					   details = x.details,
					   end_date = x.end_date,
					   id = x.id,
					   lead_id = x.lead_id,
					   lead_name = x.Leads == null ? "" : x.Leads.ClaimantFirstName + " " + x.Leads.ClaimantLastName,
					   owner_id = x.owner_id,
					   owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
					   start_date = x.start_date,
					   status_id = x.status_id,
                       statusName ="",// x.TaskStatus == null ? "" : x.TaskStatus.label,
					   text = x.text,
					   master_status_id = x.master_status_id,
					   lead_policy_id = x.lead_policy_id,
					   reminderInterval = x.ReminderInterval ?? 0,
					   policy_type = x.policy_type		// 1 = homeowners...

				   };

			return tasks;
		}

		//public static List<LeadTask> GetLeadTaskByLeadID(int leadID) {
		//     List<LeadTask> tasks = null;

		//     tasks = (from x in DbContextHelper.DbContext.Tasks.Include("Lead").Include("SecUser").Include("TaskStatus")
		//              where x.lead_id == leadID
		//              select new LeadTask {
		//                   creator_id = x.creator_id,
		//                   details = x.details,
		//                   end_date = x.end_date,
		//                   id = x.id,
		//                   lead_id = x.lead_id,
		//                   lead_name = x.Lead == null ? "" : "<a href='NewLead.aspx?id=" + System.Data.Objects.SqlClient.SqlFunctions.StringConvert((decimal)x.lead_id).Trim() +
		//                             "'>" + x.Lead.ClaimantFirstName + " " + x.Lead.ClaimantLastName + "<a/>",
		//                   owner_id = x.owner_id,
		//                   owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
		//                   start_date = x.start_date,
		//                   status_id = x.status_id,
		//                   statusName = x.TaskStatus == null ? "" : x.TaskStatus.label,
		//                   text = x.text,
		//                   master_status_id = x.master_status_id,
		//                   lead_policy_id = x.lead_policy_id

		//              }).ToList<LeadTask>();

		//     return tasks;
		//}

		public static List<LeadTask> GetLeadTaskByLeadID(int leadID, DateTime fromDate, DateTime endDate) {
			List<LeadTask> tasks = null;

			tasks = (from x in DbContextHelper.DbContext.Task.Include("Leads").Include("SecUser").Include("TaskStatus")
				    where x.lead_id == leadID &&
						EntityFunctions.TruncateTime(x.start_date) >= fromDate.Date &&
						EntityFunctions.TruncateTime(x.end_date) <= endDate.Date
				    orderby x.start_date ascending
				    select new LeadTask {
					    creator_id = x.creator_id,
					    details = x.details,
					    end_date = x.end_date,
					    id = x.id,
					    lead_id = x.lead_id,
					    lead_name = x.Leads == null ? "" : x.Leads.ClaimantFirstName + " " + x.Leads.ClaimantLastName,
					    //lead_name = x.Lead == null ? "" : "<a href='NewLead.aspx?id=" + System.Data.Objects.SqlClient.SqlFunctions.StringConvert((decimal)x.lead_id).Trim() +
					    //           "'>" + x.Lead.ClaimantFirstName + " " + x.Lead.ClaimantLastName + "<a/>",
					    owner_id = x.owner_id,
					    owner_name = x.SecUser == null ? "" : x.SecUser.FirstName + " " + x.SecUser.LastName,
					    start_date = x.start_date,
					    status_id = x.status_id,
                        statusName = x.Leads.StatusMaster == null ? "" : x.Leads.StatusMaster.StatusName,
					    text = x.text,
					    master_status_id = x.master_status_id,
					    lead_policy_id = x.lead_policy_id,
					    reminderInterval = x.ReminderInterval ?? 0,
					    policy_type = x.policy_type		// 1 = homeowners...

				    }).ToList<LeadTask>();

			return tasks;
		}

		/// <summary>
		/// Returns complete object
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Task Get(int id) {
			Task task = null;

			task = (from x in DbContextHelper.DbContext.Task
				   .Include("Reminder")
				   .Include("Recurrence")
				   where x.id == id
				   select x
				).FirstOrDefault();


			return task;
		}

		public static List<TaskPriority> GetPriorities(int clientID) {
			List<TaskPriority> priorities = null;

			priorities = (from x in DbContextHelper.DbContext.TaskPriority
					    where x.IsActive == true && (x.ClientID == null || x.ClientID == clientID)
					    select x
					    ).ToList();

			return priorities;
		}
		public static List<TaskPriority> GetPriorities() {
			List<TaskPriority> priorities = null;

			priorities = (from x in DbContextHelper.DbContext.TaskPriority
					    where x.IsActive == true && (x.ClientID == null)
					    select x
					    ).ToList();

			return priorities;
		}

		public static List<TaskStatus> GetStatuses() {
			List<TaskStatus> list = (from x in DbContextHelper.DbContext.TaskStatus
								select x
					 ).ToList();

			return list;
		}

		public static List<TaskOwner> GetOwners() {
			List<TaskOwner> owners = null;

			owners = (from x in DbContextHelper.DbContext.SecUser
					where x.Status == true
					orderby x.LastName, x.FirstName
					select new TaskOwner {
						key = x.UserId,
						label = x.FirstName + " " + x.LastName
					}).ToList<TaskOwner>();

			return owners;
		}

		public static List<TaskOwner> GetOwners(int userID) {
			List<TaskOwner> owners = null;

			owners = (from x in DbContextHelper.DbContext.SecUser
					where x.Status == true && x.UserId == userID
					orderby x.LastName, x.FirstName
					select new TaskOwner {
						key = x.UserId,
						label = x.FirstName + " " + x.LastName
					}).ToList<TaskOwner>();

			return owners;
		}

		public static int Save(Task task) {
			if (task.id == 0) {
				DbContextHelper.DbContext.Add(task);
			}

			DbContextHelper.DbContext.SaveChanges();

			return task.id;
		}

		public static int Save(LeadTask leadTask) {
			Task task = null;

			if (leadTask.id == 0) {
				task = new Task();

				DbContextHelper.DbContext.Add(task);
			}
			else {
				task = DbContextHelper.DbContext.Task.Where(x => x.id == leadTask.id).FirstOrDefault();
			}

			task.creator_id = leadTask.creator_id;
			task.details = leadTask.details;
			task.end_date = leadTask.end_date;
			task.lead_id = leadTask.lead_id;
			task.owner_id = leadTask.owner_id;
			task.start_date = leadTask.start_date;
			task.status_id = leadTask.status_id;
			task.text = leadTask.text;

			task.lead_policy_id = leadTask.lead_policy_id;
			task.master_status_id = leadTask.master_status_id;

			task.policy_type = leadTask.policy_type;

			DbContextHelper.DbContext.SaveChanges();

			return task.id;
		}

	}
}
