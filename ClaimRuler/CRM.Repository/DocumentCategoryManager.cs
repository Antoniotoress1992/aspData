using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	public class DocumentCategoryManager : IDisposable {

		private bool disposed = false;		// to detect redundant calls

		private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public DocumentCategoryManager() {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();
		}

		public List<DocumentCategory> GetAll() {
			List<DocumentCategory> categories = null;

			categories = (from x in claimRulerDBContext.DocumentCategory
					    where x.IsActive == true
					    orderby x.CategoryName
					    select x
				).ToList<DocumentCategory>();


			return categories;
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
