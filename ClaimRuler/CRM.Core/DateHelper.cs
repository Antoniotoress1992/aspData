using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core {
	static public class DateHelper {

		static public DateTime getDate(int whichOne, DayOfWeek dayOfWeek, DateTime date) {
			DateTime result = DateTime.MinValue;
			int count = 0;

			//int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
			DateTime dt = new DateTime(date.Year, date.Month, 1);

			while (dt.Month == date.Month) {
				// check for first, second, thrid, fouth
				if (whichOne < 5) {
					if (dt.DayOfWeek == dayOfWeek)
						++count;

					if (count == whichOne) {
						result = dt;
						break;
					}

				}
				else {
					// check for last
					if (dt.DayOfWeek == dayOfWeek) {
						result = dt;
					}
				}

				dt = dt.AddDays(1);
			}

			return result;
		}


		static public DateTime geFirstMonday(DateTime date) {
			DateTime dt = new DateTime(date.Year, date.Month, 1);
			//while (dt.DayOfWeek != DayOfWeek.Monday) {
			//	dt = dt.AddDays(1);
			//}

			for (int i = 0; i < 7; i++) {
				if (dt.DayOfWeek == DayOfWeek.Monday) {
					return dt;
				}
				else {
					dt = dt.AddDays(1);
				}
			}

			return dt;
		}
	}
}
