﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Views {
	public class RuleExceptionView {
		public int? RuleID { get; set; }
		public int BusinessRuleID { get; set; }
		public string BusinessRuleDescription { get; set; }

		public DateTime ExceptionDate { get; set; }

		public string ObjectName { get; set; }

		public int? ObjectID { get; set; }
		public int? ObjectTypeID { get; set; }

		public int? UserID { get; set; }
		public string UserName { get; set; }

		public string url { get; set; }

        public string InsureClaim { get; set; }

		public string Who { get; set; }

        public string Carrier { get; set; }
	}
}
