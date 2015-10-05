using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;
using CRM.Data.Entities;

namespace CRM.Web.Protected {
	public partial class EventEditPopUp : System.Web.UI.Page {

		int TaskID {
			get {
				int id = 0;
				id = Request.Params["id"] == null ? 0 : Convert.ToInt32(Request.Params["id"].ToString());

				return id;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			if (!Page.IsPostBack) {
				uc_event.bindData(this.TaskID);
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			uc_event.saveEvent();
		}

		[System.Web.Services.WebMethod]
		static public void inviteeAddContact(int taskID, int? contactID, int? userID, int? leadID) {
			Invitee invitee = null;

			if (taskID > 0) {
				invitee = new Invitee();
				invitee.TaskID = taskID;
				invitee.ContactID = contactID;
				invitee.UserID = userID;
				invitee.LeadID = leadID;

				using (InviteeManager repository = new InviteeManager()) {
					repository.Save(invitee);
				}
			}
		}

		public void setErrorMessage(string message, string css) {
			lblMessage.Text = message;
			lblMessage.CssClass = css;
		}

	}
}