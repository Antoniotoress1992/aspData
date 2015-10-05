using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities {
	public partial class AdjusterMaster {
		/// <summary>
		/// Consists of First and last name. This is not part of partial class
		/// </summary>
		public string adjusterName {
			get {
				if (string.IsNullOrEmpty(this.FirstName) && string.IsNullOrEmpty(this.LastName))
					return this.AdjusterName;
				else
					return this.FirstName + " " + this.LastName;
			}
		}
	}
}
