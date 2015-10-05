using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	static public class AdjusterLicenseAppointmentTypeManager {
		static public AdjusterLicenseAppointmentType Get(string appointmentType, int clientID) {
			AdjusterLicenseAppointmentType LicenseAppointmentType = null;

			LicenseAppointmentType = (from x in DbContextHelper.DbContext.AdjusterLicenseAppointmentType
									    where x.LicenseAppointmentType == appointmentType && x.ClientID == clientID
									    select x).FirstOrDefault();

			return LicenseAppointmentType;

		}
		static public List<AdjusterLicenseAppointmentType> GetAll(int clientID) {
			List<AdjusterLicenseAppointmentType> LicenseAppointmentTypes = null;

			LicenseAppointmentTypes = (from x in DbContextHelper.DbContext.AdjusterLicenseAppointmentType
								  where x.ClientID == clientID && x.IsActive == true
								  select x).ToList<AdjusterLicenseAppointmentType>();

			return LicenseAppointmentTypes;

		}

		static public AdjusterLicenseAppointmentType Save(AdjusterLicenseAppointmentType appointmentType) {
			if (appointmentType.LicenseAppointmentTypeID == 0)
				DbContextHelper.DbContext.AdjusterLicenseAppointmentType.Add(appointmentType);

			DbContextHelper.SaveChanges();

			return appointmentType;

		}
	}
}
