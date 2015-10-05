using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Views {
	public class ClaimProgressView {
		public string adjusterName { get; set; }

		public int claimID { get; set; }
		
		public string claimStatus { get; set; }

		public string insuredName { get; set; }

		public DateTime? lossDate { get; set; }

		public string policyType { get; set; }

		public string url { get; set; }

        public string adjusterName1 { get; set; }
	}
}
