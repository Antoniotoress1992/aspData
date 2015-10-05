using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	public class PolicyNoteManager : IDisposable {
		private bool disposed = false;		// to detect redundant calls

		private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public PolicyNoteManager() {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();
		}

		public void Delete(PolicyNote note) {
			claimRulerDBContext.PolicyNote.Remove(note);

			claimRulerDBContext.SaveChanges();
		}


		public PolicyNote Get(int id) {
			PolicyNote note = null;

			note = (from x in claimRulerDBContext.PolicyNote
				   where x.PolicyNoteID == id
				   select x).FirstOrDefault();

			return note;
		}


		public void Save(PolicyNote note) {
			if (note.PolicyNoteID == 0)
				claimRulerDBContext.PolicyNote.Add(note);

			claimRulerDBContext.SaveChanges();
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
