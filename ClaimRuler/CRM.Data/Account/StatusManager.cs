namespace CRM.Data.Account {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web;

	using System.Linq.Expressions;
	using LinqKit;
    using CRM.Data.Entities;

	public class StatusManager {
        private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

        public StatusManager()
        {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();

		}
		public static List<StatusMaster> GetAll() {
			var list = from x in DbContextHelper.DbContext.StatusMaster.Include("ReminderMaster")
					 where x.Status == true
					 orderby x.StatusName
					 select x;

			return list.ToList();
		}

		public static List<StatusMaster> GetAll(int clientID) {
			var list = from x in DbContextHelper.DbContext.StatusMaster.Include("ReminderMaster")
					 where x.Status == true &&
					 x.clientID == clientID 
					 orderby x.StatusName
					 select x;

			return list.ToList();
		}

		public static List<StatusMaster> GetList(int clientID) {
			var list = from x in DbContextHelper.DbContext.StatusMaster
					 where x.Status == true &&
					 x.clientID == clientID ||
					 x.clientID == null
					 orderby x.StatusName
					 select x;

			return list.ToList();
		}

        public List<StatusStatisticsView> getStatusStatistics(int clientID, int userID)
        {
            List<StatusStatisticsView> statistics = null;

            List<StatusStatisticsView> claimstats = (from c in claimRulerDBContext.Claim
                                                       join p in claimRulerDBContext.LeadPolicy on c.PolicyID equals p.Id
                                                       join l in claimRulerDBContext.Leads on p.LeadId equals l.LeadId
                                                       where l.ClientID == clientID &&
                                                            l.UserId == userID &&
                                                            c.StatusID != null &&
                                                            c.IsActive == true &&
                                                            l.Status == 1
                                                       group c by new { c.StatusID } into g
                                                      select new StatusStatisticsView
                                                       {
                                                           StatusID = (int)g.Key.StatusID,
                                                           claimCount = g.Count()
                                                       }).ToList();

            List<StatusMaster> progressStatuses = (from x in claimRulerDBContext.StatusMaster
                                                     where x.Status == true
                                                     orderby x.StatusName ascending
                                                     select x
                      ).ToList<StatusMaster>();

            statistics = (from x in progressStatuses
                          join c in claimstats
                          on x.StatusId equals c.StatusID into left_join
                          from leftjoin in left_join.DefaultIfEmpty(new StatusStatisticsView())	// include row from left side of join					   
                          select new StatusStatisticsView
                          {
                              StatusID = (int)x.StatusId,
                              StatusName = x.StatusName,
                              claimCount = leftjoin.claimCount
                          }).ToList();



            return statistics;
        }

		public static StatusMaster GetByStatusName(string statusName) {

			var status = from x in DbContextHelper.DbContext.StatusMaster
					   where x.StatusName == statusName && x.Status == true
					   select x;

			return status.Any() ? status.First() : new StatusMaster();
		}
		public static StatusMaster GetByStatusName(int clientID, string statusName) {

			var status = from x in DbContextHelper.DbContext.StatusMaster
					   where x.StatusName == statusName && x.Status == true && x.clientID == clientID
					   select x;

			return status.Any() ? status.First() : new StatusMaster();
		}

		/// <summary>
		/// Returns a string containing the id of those statuses designated as "Closed" claim
		/// </summary>
		/// <returns></returns>
		public static string GetClosedStatuses(int clientID) {
			string closedStatuses = null;

			int[] statuses = (from x in DbContextHelper.DbContext.StatusMaster
						   where x.Status == true
						   && (x.isCountAsOpen ?? false) == false
						   && x.clientID == clientID
						   orderby x.StatusId
						   select x.StatusId
						).ToArray();

			if (statuses != null)
				closedStatuses = string.Join(",", statuses);

			return closedStatuses;
		}

		public static int[] GetClosedStatusIDs(int clientID) {
			int[] statuses = (from x in DbContextHelper.DbContext.StatusMaster
						   where x.Status == true
						   && x.isCountAsOpen == false
						   && x.clientID == clientID
						   orderby x.StatusId
						   select x.StatusId
						).ToArray();



			return statuses;
		}
		
		public static int[] GetOpenStatusIDs(int clientID) {
			int[] statuses = (from x in DbContextHelper.DbContext.StatusMaster
						   where x.Status == true
						   && x.isCountAsOpen == true
						   && x.clientID == clientID
						   orderby x.StatusId
						   select x.StatusId
						).ToArray();



			return statuses;
		}
		public static StatusMaster GetStatusId(int statusId) {
			var status = from x in DbContextHelper.DbContext.StatusMaster
										   .Include("ReminderMaster")
					   where x.StatusId == statusId && x.Status == true
					   select x;

			return status.Any() ? status.First() : new StatusMaster();
		}


		public static StatusMaster Save(StatusMaster objStatus) {
			if (objStatus.StatusId == 0) {
				//objProducer.InsertBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
				objStatus.InsertDate = DateTime.Now;
				objStatus.InsertMachineInfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
				DbContextHelper.DbContext.Add(objStatus);
			}

			//secUser.UpdatedBy = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
			objStatus.UpdateMachineIfo = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			objStatus.UpdateDate = DateTime.Now;
			DbContextHelper.DbContext.SaveChanges();

			return objStatus;
		}
		//public static string checkDuplicateClass(int statusId, string statusName)
		//{
		//    StringBuilder sb = new StringBuilder("SELECT id ClassId,ifnull(name,'') ClassName, teacher_id TeacherId From class Where 1=1");
		//    if (Classid > 0)
		//        sb.Append(" AND id <> " + Classid.ToString());
		//    if (ClassName.Length > 0)
		//        sb.Append(" AND ifnull(name,'')='" + ClassName + "'");
		//    if (TeacherId > 0)
		//        sb.Append(" AND Teacher_Id = " + TeacherId.ToString());
		//    sb.Append(" AND ifnull(Status,0)= 1 ");


		//    return sb.ToString();
		//}
		public static bool IsExist(string statusName, int statusId) {
			var status = from x in DbContextHelper.DbContext.StatusMaster
					   where x.StatusName == statusName && x.StatusId != statusId && x.Status == true
					   select x;

			return status.Any();
		}

		public static bool IsExist(string statusName, int statusId, int clientID) {
			var status = from x in DbContextHelper.DbContext.StatusMaster
					   where x.StatusName == statusName && x.StatusId != statusId && x.Status == true && x.clientID == clientID
					   select x;

			return status.Any();
		}
	}
}
