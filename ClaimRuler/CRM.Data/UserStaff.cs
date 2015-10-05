using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Data.Entities
{
	public class UserStaff {
		public int UserId { get; set; }
		public string StaffName { get; set; }
		public string EmailAddress { get; set; }
		public bool isEmailReminder { get; set; }
	}
}
