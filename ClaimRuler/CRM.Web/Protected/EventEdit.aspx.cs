using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;

using CRM.Repository;
using CRM.Data.Entities;

namespace CRM.Web.Protected {
	public partial class EventEdit : System.Web.UI.Page {
		int clientID = 0;
		int roleID = 0;

		/// <summary>
		/// Returns Task ID from query string, if present
		/// </summary>
		int TaskID {
			get {
				int id = 0;
				id = Request.Params["q"] == null ? 0 : Convert.ToInt32(SecurityManager.DecryptQueryString(Request.Params["q"].ToString()));

				return id;
			}
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

		public void setErrorMessage (string message, string css) {
			lblMessage.Text = message;
			lblMessage.CssClass = css;
		}

		
		protected void Page_Load(object sender, EventArgs e) {
			clientID = SessionHelper.getClientId();
			roleID = SessionHelper.getUserRoleId();

			lblTitle.Text = (this.TaskID == 0) ? "Create Event" : "Edit Event";

			if (!Page.IsPostBack) {
				ucevent.bindData(this.TaskID);
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			ucevent.saveEvent();
		}

		protected void btnSaveNew_Click(object sender, EventArgs e) {
			ucevent.saveEvent();
		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			Response.Redirect("~/protected/Tasks.aspx");
		}


	}
}