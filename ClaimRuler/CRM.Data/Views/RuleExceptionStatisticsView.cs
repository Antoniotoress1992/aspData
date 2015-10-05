using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Views {
	public class RuleExceptionStatisticsView {
		public int RuleID { get; set; }

		public string RuleName { get; set; }
		public int ExceptionCount { get; set; }
	}
}
