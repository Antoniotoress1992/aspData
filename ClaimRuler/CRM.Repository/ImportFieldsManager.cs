using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	public class ImportFieldsManager : IDisposable {
		private bool disposed = false;		// to detect redundant calls

		private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public ImportFieldsManager() {
            claimRulerDBContext = new CRMEntities();
		}

		public List<ImportField> GetAll() {
			List<ImportField> importFields = null;

			importFields = (from x in claimRulerDBContext.ImportField
						 orderby x.FieldName
						 select x
							 ).ToList();

			return importFields;
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
