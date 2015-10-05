using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
	public class CarrierView {
		public int CarrierID { get; set; }
		public string CarrierName { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }

		public string StateName { get; set; }
		public string CityName { get; set; }
		public string ZipCode { get; set; }
		
	}
}
