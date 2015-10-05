using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
	public partial class Leads {
		public string policyHolderName {
			get {
				return string.Format("{0} {1} {2}", this.ClaimantFirstName, this.ClaimantMiddleName ?? "", this.ClaimantLastName);
			}
		}
		/// <summary>
		/// Extended property. Returns either firstname + mi + lastname or insuredname if former is blank.
		/// </summary>
		public string insuredName {
			get {
				string name = null;

				if (string.IsNullOrEmpty(this.InsuredName))
					name = string.Format("{0} {1} {2}", this.ClaimantFirstName, this.ClaimantMiddleName ?? "", this.ClaimantLastName);
				else
					name = this.InsuredName;
				

				return name;
			}
		}
	}
}
