using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Data.Entities;

namespace CRM.Data.Account {
	public static class CarrierInvoiceTypeManager {
		public static List<CarrierInvoiceType> GetAll() {
			List<CarrierInvoiceType> invoiceTypes = null;

			invoiceTypes = (from x in DbContextHelper.DbContext.CarrierInvoiceType
						 where x.IsActive == true
						 orderby x.InvoiceType
						 select x).ToList<CarrierInvoiceType>();

			return invoiceTypes;
		}
	}
}
