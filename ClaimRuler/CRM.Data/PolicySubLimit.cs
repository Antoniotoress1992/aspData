using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
	public partial class PolicySubLimit {

		/// <summary>
		/// Returns id, description and sublimit amount separated by |
		/// </summary>
		public string idDescriptionLimit {
			get {
				return string.Format("{0}|{1}|{2}", this.PolicySublimitID, this.SublimitDescription, this.Sublimit);
			}
		}
		public string subLimitAmount {
			get {
				return string.Format("{0:N2}", this.Sublimit);
			}
		}
	}
}
