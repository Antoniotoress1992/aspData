using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class InvoiceServiceManager {
		#region Invoice Services

		public static IQueryable<InvoiceServiceType> GetAll(int clientID) {
			IQueryable<InvoiceServiceType> services = null;

			services = from x in DbContextHelper.DbContext.InvoiceServiceType.Include("InvoiceServiceUnit")
					 where x.ClientID == clientID && x.isActive == true
					 orderby x.ServiceDescription
					 select x;

			return services;
		}

		public static InvoiceServiceType Get(int id) {
			InvoiceServiceType service = null;

			service = (from x in DbContextHelper.DbContext.InvoiceServiceType
					 where x.ServiceTypeID == id
					 select x).FirstOrDefault();

			return service;
		}

		public static List<InvoiceServiceUnit> GetServiceUnits(int clientID) {
			List<InvoiceServiceUnit> serviceUnits = null;

			serviceUnits = (from x in DbContextHelper.DbContext.InvoiceServiceUnit
					 where x.ClientID == clientID || x.ClientID == null
					 select x).ToList();

			return serviceUnits;
		}

		public static InvoiceServiceType Delete(int id) {
			InvoiceServiceType service = null;

			service = (from x in DbContextHelper.DbContext.InvoiceServiceType
					 where x.ServiceTypeID == id
					 select x).FirstOrDefault();

			return service;
		}

		public static InvoiceServiceType Save(InvoiceServiceType obj) {
			if (obj.ServiceTypeID == 0) {
				DbContextHelper.DbContext.Add(obj);
			}

			DbContextHelper.DbContext.SaveChanges();

			return obj;
		}

		#endregion
	}
}
