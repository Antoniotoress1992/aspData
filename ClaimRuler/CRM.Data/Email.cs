using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Data.Entities
{
	public class Email {
		public DateTime ReceivedDate { get; set; }
		public string From { get; set; }
		public string Subject { get; set; }
		public string TextBody { get; set; }
	}
}
