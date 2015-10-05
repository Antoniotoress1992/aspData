using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Web;
using System.Web.UI;

using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using LinqKit;
using System.Linq.Expressions;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;

using CRM.Web.Utilities;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class BatchAssignment : System.Web.UI.Page {
		int clientID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			clientID = SessionHelper.getClientId();

			Page.Form.DefaultButton = btnSearch.UniqueID;

			if (!Page.IsPostBack) {
				bindData();
			}
			
			// to bypass error from bing map
			// http://social.msdn.microsoft.com/Forums/en-US/72e169d0-a7e1-4e1e-849c-e168e22db169/appendchild?forum=bingmapsajax
			HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("main_body");
			body.Attributes.Add("onload", "LoadMap();");
		}

		private void bindData() {
			List<StateMaster> states = State.GetAll();

			CollectionManager.FillCollection(ddlState, "StateId", "StateCode", states);

			bindAdjusters();
		}

		private void bindAdjusters() {
			string streetAddress = null;
			string stateName = null;
			string handleClaimsType = null;
			string[] policyTypes = null;

			List<AdjusterMaster> adjusters = AdjusterManager.GetAll(clientID).ToList();
			if (adjusters != null && adjusters.Count > 0) {
				hf_adjusters.Value = string.Empty;
				foreach(AdjusterMaster adjuster in adjusters) {
					if (adjuster.UseDeploymentAddress ?? false) {
						stateName = adjuster.StateMaster== null ? "" : adjuster.StateMaster.StateName;

						streetAddress = string.Format("{0} {1} {2} {3}", adjuster.DeploymentAddress ?? "", adjuster.DeploymentCity ?? "", stateName ?? "", adjuster.DeploymentZipCode ?? "");
					}
					else {
						stateName = adjuster.StateMaster == null ? "" : adjuster.StateMaster.StateName;

						streetAddress = string.Format("{0} {1} {2} {3}", adjuster.Address1 ?? "", adjuster.CityName, stateName ?? "", adjuster.ZipCode ?? "");
					}

					if (!string.IsNullOrEmpty(streetAddress.Trim())) {
						try {
							geoResponse response = Core.bingMapHelper.geocodeAddress(streetAddress);
							if (response.status == "ok") {
								if (adjuster.AdjusterHandleClaimType != null) {
									policyTypes = null;
									handleClaimsType = null;

									policyTypes = (from x in adjuster.AdjusterHandleClaimType
												select x.LeadPolicyType.Description
													).ToArray();

									if (policyTypes != null && policyTypes.Length > 0)
										handleClaimsType = string.Join(":", policyTypes);
								}

								hf_adjusters.Value += string.Format("{0}|{1}|{2},{3}|{4}$",
									adjuster.AdjusterName, streetAddress, response.latitude, response.longitude, handleClaimsType ?? "");
							}
						}
						catch (Exception ex) {
							Core.EmailHelper.emailError(ex);
						}
					}
				}
			}
		}

		private Expression<Func<Claim, bool>> buildPredicate() {
			int policyTypeID = 0;
			Expression<Func<Claim, bool>> predicate = null;
			string stateCode = null;

			// core search
			predicate = PredicateBuilder.True<CRM.Data.Entities.Claim>();
			predicate = predicate.And(x => x.LeadPolicy.Leads.ClientID == clientID);			// claims for this client only
			//predicate = predicate.And(x => (x.AdjusterID ?? 0) == 0);						// no adjuster assigned
			predicate = predicate.And(x => x.IsActive == true);
			///predicate = predicate.And(x => x.LeadPolicy.Lead.Status != 0);					// active
            predicate = predicate.And(x => x.LeadPolicy.Leads.Status == 1);                 //OC 9/9/14 show only active, when deleting from all claims, was stll showing here
			
            // apply search filter
			if (!string.IsNullOrEmpty(txtClaimNumber.Text)) {
				predicate = predicate.And(x => x.AdjusterClaimNumber.Contains(txtClaimNumber.Text));
			}

			if (!string.IsNullOrEmpty(txtCarrierName.Text)) {
				predicate = predicate.And(x => x.LeadPolicy.Carrier.CarrierName.Contains(txtCarrierName.Text));
			}

			if (!string.IsNullOrEmpty(this.createDateFrom.Text) && !string.IsNullOrEmpty(this.createDateTo.Text)) {
				predicate = predicate.And(x => x.LeadPolicy.Leads.DateSubmitted >= createDateFrom.Date && x.LeadPolicy.Leads.DateSubmitted <= createDateTo.Date);
			}
			else if (!string.IsNullOrEmpty(this.createDateFrom.Text)) {
				predicate = predicate.And(x => x.LeadPolicy.Leads.DateSubmitted >= createDateFrom.Date);
			}


			if (!string.IsNullOrEmpty(this.lossDateFrom.Text) && !string.IsNullOrEmpty(this.lossDateTo.Text)) {
				predicate = predicate.And(x => x.LossDate >= lossDateFrom.Date && x.LossDate <= lossDateTo.Date);
			}
			else if (!string.IsNullOrEmpty(this.lossDateFrom.Text))
				predicate = predicate.And(x => x.LossDate >= lossDateFrom.Date);

			if (ddlState.SelectedIndex > 0) {
				stateCode = ddlState.SelectedItem.Text.Trim();
				predicate = predicate.And(x => x.LeadPolicy.Leads.StateName == stateCode);
			}

			if (ddlCity.SelectedIndex > 0)
				predicate = predicate.And(x => x.LeadPolicy.Leads.CityName.Contains(ddlCity.SelectedItem.Text));

			if (!string.IsNullOrEmpty(txtZipCode.Text))
				predicate = predicate.And(x => x.LeadPolicy.Leads.Zip == txtZipCode.Text);

			if (this.ucPolicyType1.SelectedIndex > 0) {
				policyTypeID = Convert.ToInt32(ucPolicyType1.SelectedValue);

				predicate = predicate.And(x => x.LeadPolicy.PolicyType == policyTypeID);
			}

			return predicate;
		}

		protected void btnSearch_Click(object sender, EventArgs e) {
			Expression<Func<Claim, bool>> predicate = null;
			List<AdjusterView> adjusters = null;
			List<LeadView> leads = null;

			predicate = buildPredicate();

			using (ClaimManager repository = new ClaimManager()) {
				leads = repository.Search(predicate).orderBy("ClaimantLastName").ToList();
			}
            
			gvSearchResult.DataSource = leads;
			gvSearchResult.DataBind();

			if (leads != null && leads.Count > 0) {
				pnlResult.Visible = true;
				pnlToolbar.Visible = true;

				// load adjusters
				adjusters = AdjusterManager.GetClaimAssigned(clientID);

				gvAdjusters.DataSource = adjusters;
				gvAdjusters.DataBind();

				CollectionManager.FillCollection(ddlAdjuster, "AdjusterID", "AdjusterName", adjusters);
			}
		}


		protected void ddlState_SelectedIndexChanged(object sender, EventArgs e) {
			if (ddlState.SelectedIndex > 0) {
				CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll(Convert.ToInt32(ddlState.SelectedValue)));
			}
			else {
				ddlCity.Items.Clear();
			}
		}

		protected void cbxAll_CheckedChanged(object sender, EventArgs e) {
			CheckBox cbxAll = gvSearchResult.HeaderRow.FindControl("cbxAll") as CheckBox;
			if (cbxAll != null) {
				foreach (GridViewRow row in gvSearchResult.Rows) {
					if (row.RowType == DataControlRowType.DataRow) {
						CheckBox cbxSelected = row.FindControl("cbxSelected") as CheckBox;

						cbxSelected.Checked = cbxAll.Checked;
					}
				}
			}
		}

		protected void btnAssign_Click(object sender, EventArgs e) {
			int adjusterID = 0;
			Claim claim = null;
			int claimID = 0;
			AdjusterMaster adjuster = null;
			StringBuilder claimList = new StringBuilder();
			

			if (int.TryParse(ddlAdjuster.SelectedValue, out adjusterID) && adjusterID > 0) {
				adjuster = AdjusterManager.GetAdjusterId(adjusterID);
			
				foreach (GridViewRow row in gvSearchResult.Rows) {
					if (row.RowType == DataControlRowType.DataRow) {
						CheckBox cbxSelected = row.FindControl("cbxSelected") as CheckBox;

						if (cbxSelected.Checked) {
							claimID = (int)gvSearchResult.DataKeys[row.RowIndex].Value;
							claim = ClaimsManager.GetByID(claimID);
							if (claim != null) {
								claim.AdjusterID = adjusterID;
								ClaimsManager.Save(claim);

								HyperLink lnkLead = row.FindControl("hlnkLead") as HyperLink;

								claimList.Append(string.Format("<tr align=\"center\"><td>{0}</td><td>{1}</td><td>{2}</td></tr>", 
									lnkLead.Text, claim.AdjusterClaimNumber ?? "", claim.InsurerClaimNumber ?? ""));
							}
						}
					}
				} // foreach

				notifyAdjuster(adjuster, claimList);

				btnSearch_Click(null, null);
			}

		}

		protected void gvSearchResult_Sorting(object sender, GridViewSortEventArgs e) {
			Expression<Func<Claim, bool>> predicate = null;

			predicate = buildPredicate();

			IQueryable<LeadView> leads = LeadsManager.SearchClaim(predicate);
			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			gvSearchResult.DataSource = leads.orderByExtension(e.SortExpression, descending);

			gvSearchResult.DataBind();
		}

		protected void lbtnClear_Click(object sender, EventArgs e) {
			txtCarrierName.Text = string.Empty;
			txtClaimNumber.Text = string.Empty;
			txtZipCode.Text = string.Empty;

			lossDateFrom.Text = string.Empty;
			lossDateTo.Text = string.Empty;

			createDateFrom.Text = string.Empty;
			createDateTo.Text = string.Empty;

			ddlCity.SelectedIndex = -1;
			ddlState.SelectedIndex = -1;
			ucPolicyType1.SelectedIndex = -1;
		}

		protected void gvSearchResult_RowDataBound(object sender, GridViewRowEventArgs e) {
			

			if (e.Row.RowType == DataControlRowType.DataRow) {
				LeadView leadView = e.Row.DataItem as LeadView;

				HiddenField hf_lossLocation = e.Row.FindControl("hf_lossLocation") as HiddenField;

				HyperLink lnkLead = e.Row.FindControl("hlnkLead") as HyperLink;

				// navigation link to lead
				lnkLead.NavigateUrl = "~/Protected/NewLead.aspx?id=" + leadView.LeadId.ToString();
                lnkLead.Text = leadView.InsuredName;// string.Format("{0}, {1}", leadView.ClaimantLastName, leadView.ClaimantFirstName);
								
				// loss address information for map
				hf_lossLocation.Value = string.Format("{0} {1}|{2} {3} {4} {5}",
						leadView.ClaimantFirstName,
						leadView.ClaimantLastName,
						leadView.LossAddress,
						leadView.CityName,
						leadView.StateName,
						leadView.Zip);

				if (string.IsNullOrEmpty(leadView.Email))
					e.Row.CssClass = "red";
						
			}
		}

		private void notifyAdjuster(AdjusterMaster adjuster, StringBuilder claimList) {
			StringBuilder emailBody = new StringBuilder();
			string password = null;
			string[] recipients = null;
			string smtpHost = null;
			int smtpPort = 0; 
			string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
			string subject = "Claim Assignment Notification";
			CRM.Data.Entities.SecUser user = null;

			string itsgHost = ConfigurationManager.AppSettings["smtpHost"].ToString();
			string itsgHostPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
			string itsgEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
			string itsgEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
			int.TryParse(itsgHostPort, out smtpPort);

			// get logged in user
			int userID = SessionHelper.getUserId();
            //Convert.ToInt32(itsgHostPort);
			// get current user email info
			user = SecUserManager.GetByUserId(userID);

			// load email credentials
			smtpHost = user.emailHost;
			int.TryParse(user.emailHostPort, out smtpPort);
		

			// recipients
			recipients = new string[] { adjuster.email };

			// build email body
			// .containerBox
			emailBody.Append("<div style=\"margin:auto;width:600px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");

			// .header
			//emailBody.Append("<div style=\"background-image:url(https://appv3.claimruler.com/images/email_header_small.jpg);background-repeat: no-repeat;background-size: 100% 100%;height:70px;\"></div>");
			emailBody.Append("<div><img src=\"http://app.claimruler.com/images/email_header_small.jpg\"></image></div>");

			// .paneContentInner
			emailBody.Append("<div style=\"margin: 20px;\">");
			emailBody.Append("Hi " + adjuster.adjusterName + "!<br><br>");
			emailBody.Append("Your firm uses Claim Ruler software for claims management and the following claim(s) have been assigned to you.<br><br>");
			emailBody.Append("Please review claim(s) right away to begin handling the file(s). Time is of the essence!<br><br>");

			emailBody.Append("<table style=\"width:550px;\">");
			emailBody.Append(string.Format("<tr align=\"center\"><th scope=\"col\">{0}</th><th scope=\"col\">{1}</th><th scope=\"col\">{2}</th></tr>", "Insured Name", "Adjuster Claim #", "Insurer Claim #"));

			// claim list
			emailBody.Append(claimList.ToString());

			// end email body
			emailBody.Append("</table>");
			emailBody.Append(string.Format("<p><a href=\"{0}/login.aspx\">Please click here to access Claim Ruler.</a></p>", siteUrl));
			emailBody.Append("<br>Thank you.");

			emailBody.Append("</div>");	// inner containerBox
			emailBody.Append("</div>");	// paneContentInner
			emailBody.Append("</div>");	// containerBox

			//Core.EmailHelper.sendEmail(user.Email, recipients, null, "Claim Assignment Notification", emailBody.ToString(), null, user.emailHost, smtpPort, smtpEmail, smtpPassword, true);
			password = Core.SecurityManager.Decrypt(user.emailPassword);

			//Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, user.isSSL ?? true);
            Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, itsgHost, Convert.ToInt32(itsgHostPort), itsgEmail, itsgEmailPassword);
		}


	}
}