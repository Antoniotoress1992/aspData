using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data.Entities;

namespace CRM.Repository {
	public class ActionManager : IDisposable {
		private bool disposed = false;		// to detect redundant calls

		private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public ActionManager() {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();
		}

		public void DeleteAll(int clientID, int roleID) {
			//DbContextHelper.DbContext.ExecuteStoreCommand("DELETE FROM RoleAction WHERE RoleID = {0} AND ClientID = {1}", roleID, clientID);



		}

		public List<Data.Entities.Action> GetActions() {
			List<Data.Entities.Action> actions = null;

			actions = (from x in claimRulerDBContext.Action
					 where x.IsActive == true
					 select x
					 ).ToList<Data.Entities.Action>();

			return actions;
		}
		public List<RoleAction> GetRoleActions(int clientID, int roleID) {
			List<RoleAction> actions = null;

			actions = (from x in claimRulerDBContext.RoleAction
					 where x.RoleID == roleID &&
					 x.ClientID == clientID
					 select x
					 ).ToList<RoleAction>();

			return actions;
		}
		public List<int> GetActions(int clientID, int roleID) {
			List<int> actions = null;

			actions = (from x in claimRulerDBContext.RoleAction
					 where x.RoleID == roleID &&
					 x.ClientID == clientID
					 select x.ActionID
					 ).ToList<int>();

			return actions;
		}
		public void Save(RoleAction roleAction) {
			claimRulerDBContext.RoleAction.Add(roleAction);

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
