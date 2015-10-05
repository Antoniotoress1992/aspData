using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRM.Data;
using CRM.Data.Entities;

namespace CRM.Repository {
	public class InviteeManager : IDisposable {
		private bool disposed = false;		// to detect redundant calls

        private CRM.Data.Entities.CRMEntities claimRulerDBContext = null;

		public InviteeManager() {
            claimRulerDBContext = new CRM.Data.Entities.CRMEntities();
		}

		public void Delete(int id) {
			Invitee invitee = null;

			invitee = (from x in claimRulerDBContext.Invitee
					 where x.InviteeID == id
					 select x).FirstOrDefault();

			if (invitee != null)
				claimRulerDBContext.Invitee.Remove(invitee);

			claimRulerDBContext.SaveChanges();
		}

		public void DeleteAll(int taskID) {

			//DbContextHelper.DbContext.ExecuteStoreCommand("DELETE FROM Invitee WHERE TaskID = {0}", taskID);

            var task = DbContextHelper.DbContext.Invitee.Where(x => x.TaskID == taskID).ToList();
            foreach (Invitee objR in task)
            {
                DbContextHelper.DbContext.Invitee.Remove(objR);
            }
            DbContextHelper.DbContext.SaveChanges();

		}

		public List<InviteeView> GetAll(int taskID) {
			List<InviteeView> inviteeCollection = new List<InviteeView>();

			var oInvitees = (from x in claimRulerDBContext.Invitee
					  .Include("Contact")
					  .Include("Leads")
					  .Include("SecUser")
					  where x.TaskID == taskID
					  select x
					).ToList<Invitee>();

			if (oInvitees != null && oInvitees.Count > 0) {
				foreach (Invitee invitee in oInvitees) {
					InviteeView inviteeView = new InviteeView();
					inviteeView.contactID = invitee.ContactID ?? 0;
					inviteeView.inviteeID = invitee.InviteeID;
					inviteeView.leadID = invitee.LeadID ?? 0;
					inviteeView.userID = invitee.UserID ?? 0;

					if (invitee.Contact != null)
						inviteeView.inviteeName = invitee.Contact.ContactName;
					else if (invitee.SecUser != null)
						inviteeView.inviteeName = invitee.SecUser.FirstName + " " + invitee.SecUser.LastName;
					else if (invitee.Leads != null)
						inviteeView.inviteeName = invitee.Leads.insuredName;

					inviteeCollection.Add(inviteeView);
				}
			}

			return inviteeCollection.OrderBy(x => x.inviteeName).ToList();
		}

		public Invitee Save(Invitee invitee) {
			if (invitee.InviteeID == 0)
				claimRulerDBContext.Invitee.Add(invitee);

			claimRulerDBContext.SaveChanges();

			return invitee;
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
