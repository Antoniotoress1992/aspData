using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	public class InvoiceServiceTypeManager : IDisposable {
		
		private bool disposed = false;		// to detect redundant calls

		private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public InvoiceServiceTypeManager() {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();
		}

		public  IQueryable<InvoiceServiceType> GetAll(int proID) {
			IQueryable<InvoiceServiceType> services = null;

            //services = from x in claimRulerDBContext.InvoiceServiceType
            //             //.Include("InvoiceServiceUnit")						
            //         where x.ClientID == clientID && x.isActive == true
            //         orderby x.ServiceDescription
            //         select x;
            //NEW STUFF GOES HERE-- NEED TO JOIN TABLES ON YOUR FAR RIGHT MONITOR-- JOIN STATEMENT WAS CREATED FOR YOU BY YOU..HAPPY MONDAY! LOL
            services = from x in claimRulerDBContext.InvoiceServiceType
                       //.Include("InvoiceServiceUnit")	
                       join c in claimRulerDBContext.CarrierInvoiceProfileFeeItemized on x.ServiceTypeID equals c.ServiceTypeID
                       join i in claimRulerDBContext.CarrierInvoiceProfile on c.CarrierInvoiceProfileID equals i.CarrierInvoiceProfileID
                       where i.CarrierInvoiceProfileID == proID && x.isActive == true
                       orderby x.ServiceDescription
                       select x;

			return services;
		}
		/// <summary>
		/// text = string containing (ServiceTypeID|ServiceRate|DefaultQty)
		/// </summary>
		/// <param name="clientID"></param>
		/// <returns></returns>
		public List<InvoiceServiceTypeView> GetForDropdown(int clientID) {
			List<InvoiceServiceType> invoiceServiceTypes = null;

			List<InvoiceServiceTypeView> serviceViews = null;

			invoiceServiceTypes = GetAll(clientID).ToList();

			if (invoiceServiceTypes != null && invoiceServiceTypes.Count > 0) {
				serviceViews = (from x in invoiceServiceTypes														
							 select new InvoiceServiceTypeView {
								 ServiceTypeID = x.ServiceTypeID,
								 text = x.ServiceDescription,
								 value = string.Format("{0}|{1:N2}|{2}", x.ServiceTypeID, x.ServiceRate, x.DefaultQty)
							 }).ToList<InvoiceServiceTypeView>();
			}


			return serviceViews;
		}


		public  InvoiceServiceType Get(int id) {
			InvoiceServiceType service = null;

			service = (from x in claimRulerDBContext.InvoiceServiceType
					 where x.ServiceTypeID == id
					 select x).FirstOrDefault();

			return service;
		}

		public  List<InvoiceServiceUnit> GetServiceUnits(int clientID) {
			List<InvoiceServiceUnit> serviceUnits = null;

			serviceUnits = (from x in claimRulerDBContext.InvoiceServiceUnit
						 where x.ClientID == clientID || x.ClientID == null
						 select x).ToList();

			return serviceUnits;
		}

		public  InvoiceServiceType Delete(int id) {
			InvoiceServiceType service = null;

			service = (from x in claimRulerDBContext.InvoiceServiceType
					 where x.ServiceTypeID == id
					 select x).FirstOrDefault();

			return service;
		}

		public  InvoiceServiceType Save(InvoiceServiceType obj) {
			if (obj.ServiceTypeID == 0) {
				claimRulerDBContext.InvoiceServiceType.Add(obj);
			}

			claimRulerDBContext.SaveChanges();

			return obj;
		}

		

		#region ===== memory management =====

		public void Dispose() {
			// Perform any object clean up here.

			// If you are inheriting from another class that
			// also implements IDisposable, don't forget to
			// call base.Dispose() as well.
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing) {
			if (!disposed) {
				if (disposing) {
					if (claimRulerDBContext != null) {

						claimRulerDBContext.Dispose();
					}
				}

				disposed = true;
			}
		}
		#endregion
	}
}
