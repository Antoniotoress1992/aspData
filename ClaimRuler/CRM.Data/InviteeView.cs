using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
	public class InviteeView {
		public string inviteeName { get; set; }

		public int inviteeID { get; set; }
		public int userID { get; set; }
		public int contactID { get; set; }
		public int leadID { get; set; }
	}
}
