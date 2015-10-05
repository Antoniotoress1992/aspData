using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
	public partial class Claim {
		/// <summary>
		/// calcuted field: datediff(day,[DateAssigned],[DateClosed])
		/// </summary>
		public double cycleTime {
			get {
				double spanUnit = 0;
				if (this.DateClosed != null && this.DateAssigned != null) {
					TimeSpan timespan = ((DateTime)this.DateClosed).Subtract((DateTime)this.DateAssigned);
					spanUnit = timespan.TotalDays;
				}
				return spanUnit;
			}
		}
		/// <summary>
		/// calculated field: (datediff(day,[DateReopenCompleted],[DateFirstReopened]))
		/// </summary>
		public double reopenCycleTime {
			get {
				double spanUnit = 0;


				if (this.DateReopenCompleted != null && this.DateFirstReOpened != null) {
					TimeSpan timespan = ((DateTime)this.DateReopenCompleted).Subtract((DateTime)this.DateFirstReOpened);
					spanUnit = timespan.TotalDays;
				}

				return spanUnit;
			}
		}
	}
}