using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
	public class ProgressStatisticsView {
		public int ProgressStatusID { get; set; }

		public string ProgressDescription { get; set; }
		public int claimCount { get; set; }
	}
}
