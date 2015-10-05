using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Transactions;

using LinqKit;
using System.Linq.Expressions;

using CRM.Core;
using CRM.Data.Account;
using CRM.Data;
using CRM.Web.Utilities;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class LeadTransfer : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			Form.DefaultButton = btnSearch.UniqueID;
		}

		private Expression<Func<Leads, bool>> buildPredicate() {
			Expression<Func<Leads, bool>> predicate = null;
			string searchText = null;

			int clientID = 0;
			int roleID = 0;

			clientID = SessionHelper.getClientId();
			roleID = SessionHelper.getUserRoleId();


			predicate = PredicateBuilder.True<CRM.Data.Entities.Leads>();

			predicate = predicate.And(Lead => Lead.Status != 0);
			predicate = predicate.And(Lead => Lead.UserId != 0);
			predicate = predicate.And(Lead => Lead.ClientID == clientID);

			searchText = txtSeach.Text;

			if (!string.IsNullOrEmpty(searchText)) {
				predicate = predicate.And(Lead => Lead.ClaimantFirstName.Contains(searchText) ||
											Lead.ClaimantLastName.Contains(searchText) ||
											Lead.InsuredName.Contains(searchText));
			}


			//switch (roleID) {

			//	case (int)UserRole.Client:
			//	case (int)UserRole.SiteAdministrator:
			//		if (clientID > 0)

			//		break;


			//	default:
			//		// get all leads created by user		
			//		predicate = predicate.And(Lead => Lead.ClientID == clientID);
			//		break;
			//}

			return predicate;
		}

		protected void btnSearch_Click(object sender, ImageClickEventArgs e) {
			List<LeadView> leads = null;
			List<UserStaff> users = null;
			int clientID = 0;

			var predicate = buildPredicate();

			leads = LeadsManager.GetLeads(predicate).ToList();

			if (leads != null && leads.Count > 0) {
				gvUserLeads.DataSource = leads;

				gvUserLeads.DataBind();

				// load users
				clientID = SessionHelper.getClientId();

				users = SecUserManager.GetStaff(clientID);
				CollectionManager.FillCollection(ddlUsers, "UserId", "StaffName", users);

				pnlGrid.Visible = true;
				lblError.Text = string.Empty;

				pnlSearch.Visible = false;
			}
			else {
				lblError.Text = "No results found.";
				lblError.Visible = true;
			}
		}


		protected void btnAnotheSearch_click(object sender, EventArgs e) {
			resetSearch();
		}

		protected void btnSave_click(object sender, EventArgs e) {
			int userID = 0;
			Leads lead = null;
			int leadID = 0;
			StringBuilder claimList = new StringBuilder();
			int transferCount = 0;

			userID = Convert.ToInt32(ddlUsers.SelectedValue);

			if (userID > 0) {
				try {
					using (TransactionScope scope = new TransactionScope()) {
						foreach (GridViewRow row in gvUserLeads.Rows) {
							if (row.RowType == DataControlRowType.DataRow) {
								CheckBox cbx = row.FindControl("cbxLead") as CheckBox;
								if (cbx != null && cbx.Checked) {
									leadID = (int)gvUserLeads.DataKeys[row.RowIndex].Value;

									lead = LeadsManager.GetByLeadId(leadID);

									if (lead != null) {
										lead.UserId = userID;

										LeadsManager.Save(lead);

										++transferCount;

										claimList.Append(string.Format("<tr align=\"center\"><td>{0}</td>", lead.insuredName));
										claimList.Append(string.Format("<td>{0}</td>", lead.LossAddress ?? ""));
										claimList.Append(string.Format("<td>{0}</td>", lead.CityName ?? ""));
										claimList.Append(string.Format("<td>{0}</td>", lead.StateName ?? ""));
										claimList.Append(string.Format("<td>{0:MM/dd/yyyy}</td></tr>", lead.LossDate));
									}
								}

							}
						} //	foreach

						scope.Complete();						
					}	// using

					// send email notification to assigned use
					//notifyUser(userID, claimList);

					// reset search
					//resetSearch();

					lblSave.Text = "Transfer completed successfully.";
					lblSave.Visible = true;
				}
				catch (Exception ex) {
					lblError.Text = "Transfer was not completed due to error.";
					lblError.Visible = true;

					Core.EmailHelper.emailError(ex);
				}

			}
		}

		protected void gvUserLeads_Sorting(object sender, GridViewSortEventArgs e) {

			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			ViewState["lastSortExpression"] = e.SortExpression;
			ViewState["lastSortDirection"] = descending;

			Expression<Func<Leads, bool>> predicate = buildPredicate();

			List<LeadView> leads = LeadsManager.GetLeads(predicate);

			//gvUserLeads.DataSource = LeadsManager.GetLeads(predicate, e.SortExpression, descending);
			//gvUserLeads.DataBind();

		}

		private void notifyUser(int assignedUserID, StringBuilder claimList) {
			StringBuilder emailBody = new StringBuilder();
			string password = null;
			string[] recipients = null;
			string smtpHost = null;
			int smtpPort = 0; ;
			string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
			string subject = "Claim Assignment Notification";
			CRM.Data.Entities.SecUser user = null;
			CRM.Data.Entities.SecUser assignedUser = null;

			// get logged in user
			int userID = SessionHelper.getUserId();

			// get logged in user email info
			user = SecUserManager.GetByUserId(userID);

			// load email credentials
			smtpHost = user.emailHost;
			int.TryParse(user.emailHostPort, out smtpPort);

			// get assigned user
			assignedUser = SecUserManager.GetByUserId(assignedUserID);

			// recipients
			recipients = new string[] { assignedUser.Email };

			// build email body
			// .containerBox
			emailBody.Append("<div style=\"margin:auto;width:600px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");

			// .header
			emailBody.Append("<div><img src=\"http://app.claimruler.com/images/email_header_small.jpg\"></image></div>");

			// .paneContentInner
			emailBody.Append("<div style=\"margin: 20px;\">");
			emailBody.Append("Hi " + assignedUser.FirstName + "!<br><br>");
			emailBody.Append("Your firm uses Claim Ruler software for claims management and the following claim(s) have been assigned to you.<br><br>");
			emailBody.Append("Please review claim(s) right away to begin handling the file(s). Time is of the essence!<br><br>");

			emailBody.Append("<table style=\"width:550px;\">");
			emailBody.Append(string.Format("<tr align=\"center\"><th scope=\"col\">{0}</th>", "Insured Name"));
			emailBody.Append(string.Format("<th scope=\"col\">{0}</th>", "Loss Address"));
			emailBody.Append(string.Format("<th scope=\"col\">{0}</th>", "Loss City"));
			emailBody.Append(string.Format("<th scope=\"col\">{0}</th>", "Loss State"));
			emailBody.Append(string.Format("<th scope=\"col\">{0}</th></tr>", "Loss Date"));


			// claim list
			//emailBody.Append(claimList.ToString());

			// end email body
			emailBody.Append("</table>");
			emailBody.Append(string.Format("<p><a href=\"{0}/login.aspx\">Please click here to access Claim Ruler.</a></p>", siteUrl));
			emailBody.Append("<br>Thank you.");

			emailBody.Append("</div>");	// inner containerBox
			emailBody.Append("</div>");	// paneContentInner
			emailBody.Append("</div>");	// containerBox

			password = Core.SecurityManager.Decrypt(user.emailPassword);

            Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, true);//user.isSSL ??
		}


		private void resetSearch() {
			pnlGrid.Visible = false;
			pnlSearch.Visible = true;

			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblError.Visible = false;
		}


	}
}