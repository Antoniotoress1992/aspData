using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Data.Entities
{
	public class LeadTask {
		public int id { get; set; }
		public int? creator_id { get; set; }
		public string details { get; set; }
		public int? lead_id { get; set; }
		public int? owner_id { get; set; }
		public string owner_name { get; set; }
		public string lead_name { get; set; }
		public int? status_id { get; set; }
		public DateTime? start_date { get; set; }
		public DateTime? end_date { get; set; }
		public string text { get; set; }
		public string statusName { get; set; }

		public int? master_status_id { get; set; }
		public int? lead_policy_id { get; set; }
		public int? policy_type { get; set; }

		public int priorityID { get; set; }
		public string priorityName { get; set; }

		public bool isAllDay { get; set; }
		public bool isReminder { get; set; }
		public string location { get; set; }
		public int? resourceKey { get; set; }
		public int? reminderInterval { get; set; }

		public LeadTask() {
		}
	}
}
