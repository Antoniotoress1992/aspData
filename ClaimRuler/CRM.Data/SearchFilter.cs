using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
	public class SearchFilter {
		public string carrierName { get; set; }

		public string claimNumber { get; set; }
		public DateTime? createDateFrom { get; set; }
		public DateTime? createDateTo { get; set; }
		public DateTime? lossDateFrom { get; set; }
		public DateTime? lossDateTo { get; set; }

		public string stateName { get; set; }
		public string cityName { get; set; }

		public int? policyTypeID { get; set; }

		public string zipCode { get; set; }
	}
}
