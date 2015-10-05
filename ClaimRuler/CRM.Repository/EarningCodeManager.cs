using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;

namespace CRM.Repository {
	public class EarningCodeManager : IDisposable {
			private bool disposed = false;		// to detect redundant calls

		private CRM.Data.CRMEntities claimRulerDBContext = null;

		public EarningCodeManager() {
			claimRulerDBContext = new Data.CRMEntities();
		}
		
		public EarningCode Get(int id) {
			EarningCode earningCode = null;

			earningCode = (from x in claimRulerDBContext.EarningCodes
						 where x.EarningCodeID == id
						 orderby x.CodeDescription
						 select x).FirstOrDefault<EarningCode>();
			
			return earningCode;
		}

		public List<EarningCode> GetAll(int clientID) {
			List<EarningCode> earningCodes = null;

			earningCodes = (from x in claimRulerDBContext.EarningCodes
						 where x.ClientID == clientID && x.IsActive == true
						 orderby x.CodeDescription
						 select x).ToList<EarningCode>();
			return earningCodes;
		}

		public EarningCode Save(EarningCode earningCode) {
			if (earningCode.EarningCodeID == 0)
				claimRulerDBContext.EarningCodes.AddObject(earningCode);

			claimRulerDBContext.SaveChanges();

			return earningCode;
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
