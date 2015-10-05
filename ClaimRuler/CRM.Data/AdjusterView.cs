using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
	public class AdjusterView {
		public int AdjusterID { get; set; }
		public string AdjusterName { get; set; }

		public int MumberOfClaims { get; set; }
	}
}
