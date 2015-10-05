using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	static public class CarrierLocationManager {
		static public CarrierLocation Get(int id) {
				CarrierLocation location = (from x in DbContextHelper.DbContext.CarrierLocation.Include("CountryMaster").Include("StateMaster").Include("CityMaster")
									where x.CarrierLocationID == id
									select x).FirstOrDefault<CarrierLocation>();

				return location;
		}

		static public List<CarrierLocation> GetAll(int carrierID) {
			List<CarrierLocation> locations = (from x in DbContextHelper.DbContext.CarrierLocation.Include("CountryMaster").Include("StateMaster").Include("CityMaster")
								   where x.CarrierID  == carrierID && x.IsActive == true
								   select x).ToList<CarrierLocation>();

			return locations;
		}


        static public List<CarrierLocation> GetAllList(int[] carrierID)
        {
            List<CarrierLocation> locations = (from x in DbContextHelper.DbContext.CarrierLocation.Include("CountryMaster").Include("StateMaster").Include("CityMaster")
                                               where carrierID.Contains(x.CarrierID) && x.IsActive == true
                                               select x).ToList<CarrierLocation>();

            return locations;
        }

        static public List<CarrierLocation> GetCarrierLocation(int clientID)
        {
            List<CarrierLocation> locations = (from x in DbContextHelper.DbContext.Carrier
                                               join y in DbContextHelper.DbContext.CarrierLocation on x.CarrierID equals y.CarrierID
                                               where x.ClientID == clientID && y.IsActive == true
                                               select y).ToList<CarrierLocation>();



            return locations.GroupBy(x => x.CarrierLocationID).Select(group => group.First()).ToList<CarrierLocation>();
        }

		public static CarrierLocation Save(CarrierLocation location) {
			if (location.CarrierLocationID == 0) {
				DbContextHelper.DbContext.Add(location);
			}

			DbContextHelper.DbContext.SaveChanges();

			return location;
		}
	}
}
