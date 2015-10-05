using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;
using CRM.Web.Utilities;

using LinqKit;
using System.Linq.Expressions;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucAdjusterList : System.Web.UI.UserControl {
		int clientID = 0;
		protected Protected.ClaimRuler masterPage = null;

		protected void Page_Load(object sender, EventArgs e) {
			clientID = SessionHelper.getClientId();

			masterPage = this.Page.Master as Protected.ClaimRuler;

			Page.Form.DefaultButton = btnSearch.UniqueID;

			if (!IsPostBack) {				
				DoBind();
			}
			if (Session["LeadIds"] == null)
				btnReturnToClaim.Visible = false;
		}

		protected void btnNew_Click(object sender, EventArgs e) {
			Response.Redirect("~/protected/admin/AdjusterEdit.aspx");
		}

		protected void btnReturnToClaim_Click(object sender, EventArgs e) {
			if (Session["LeadIds"] != null) {
				Response.Redirect("~/protected/newlead.aspx?q=" + Core.SecurityManager.EncryptQueryString(Session["LeadIds"].ToString()));
			}
		}

		private void DoBind() {

			IQueryable<AdjusterMaster> adjusters = null;

			adjusters = AdjusterManager.GetAll(clientID);

			gvAdjuster.DataSource = adjusters.ToList();
			gvAdjuster.DataBind();

			List<StateMaster> states = State.GetAll();

			ddlState.DataSource = states;
			ddlState.DataBind();

			CollectionManager.FillCollection(ddlState, "StateId", "StateName", states);

			CollectionManager.FillCollection(ddlServiceStateLicense, "StateId", "StateName", states);

			CollectionManager.FillCollection(ddlTypeClaimHandled, "LeadPolicyTypeID", "Description", LeadPolicyTypeManager.GetAll());
		}

		protected void gvAdjuster_RowCommand(object sender, GridViewCommandEventArgs e) {
			int adjusterID = 0;
			string encryptedID = null;

			if (e.CommandName == "DoEdit") {
				encryptedID = Core.SecurityManager.EncryptQueryString(e.CommandArgument.ToString());

				Response.Redirect("~/protected/admin/AdjusterEdit.aspx?q=" + encryptedID);

			}
			else if (e.CommandName == "DoDelete") {
				adjusterID = Convert.ToInt32(e.CommandArgument.ToString());

				AdjusterMaster adjuster = AdjusterManager.GetAdjusterId(adjusterID);

				if (adjuster != null) {
					adjuster.Status = false;

					AdjusterManager.Save(adjuster);
				}
			}

			DoBind();
		}

		protected void gv_onSorting(object sender, GridViewSortEventArgs e) {
			IQueryable<AdjusterMaster> adjusters = null;



			adjusters = AdjusterManager.GetAll(clientID);

            List<AdjusterMaster> objListAdjuster = adjusters.ToList();

			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;


            string sortClause = e.SortExpression + (descending ? " Desc " : " Asc ");           

            gvAdjuster.DataSource = objListAdjuster.sort(sortClause).ToList();

			//gvAdjuster.DataSource = adjusters.orderByExtension(e.SortExpression, descending);

			gvAdjuster.DataBind();

		}

		protected void lbtnClear_Click(object sender, EventArgs e) {
			txtCity.Text = string.Empty;
			txtCompanyName.Text = string.Empty;
			txtCredentials.Text = string.Empty;
			txtName.Text = string.Empty;
			txtServiceArea.Text = string.Empty;
			txtStreetAddress.Text = string.Empty;
			txtYearExperience.Text = string.Empty;
			txtZipCode.Text = string.Empty;
			ddlServiceStateLicense.SelectedIndex = 0;
			ddlState.SelectedIndex = 0;
			ddlStatus.SelectedIndex = 0;
			ddlTypeClaimHandled.SelectedIndex = 0;
			ddlW9.SelectedIndex = 0;
		}

		protected void btnSearch_Click(object sender, EventArgs e) {
			int policyTypeID = 0;
			int stateID = 0;
			int yearExperience = 0;

			Expression<Func<AdjusterMaster, bool>> predicate = PredicateBuilder.True<CRM.Data.Entities.AdjusterMaster>();
			predicate = predicate.And(x => x.ClientId == clientID);


			if (!string.IsNullOrEmpty(txtName.Text)) {
				predicate = predicate.And(x => x.FirstName.Contains(txtName.Text) || x.LastName.Contains(txtName.Text));
			}

			if (!string.IsNullOrEmpty(txtCompanyName.Text)) {
				predicate = predicate.And(x => x.CompanyName.Contains(txtCompanyName.Text));
			}

			if (!string.IsNullOrEmpty(txtStreetAddress.Text)) {
				predicate = predicate.And(x => x.Address1.Contains(txtStreetAddress.Text) || x.Address2.Contains(txtStreetAddress.Text));
			}

			// filter: state
			if (ddlState.SelectedIndex > 0) {
				stateID = Convert.ToInt32(ddlState.SelectedValue);
				predicate = predicate.And(x => x.StateID == stateID);
			}

			// filter: city name
			if (!string.IsNullOrEmpty(txtCity.Text)) {
				predicate = predicate.And(x => x.CityName.Contains(txtCity.Text));
			}

			// filter: zip code
			if (!string.IsNullOrEmpty(txtZipCode.Text)) {
				predicate = predicate.And(x => x.ZipCode == txtZipCode.Text);
			}

			// filter: status
			if (ddlStatus.SelectedIndex > 0) {
				predicate = predicate.And(x => x.Status == ddlStatus.SelectedValue.Equals("1"));
			}

			// filter: credentials
			if (!string.IsNullOrEmpty(txtCredentials.Text)) {
				predicate = predicate.And(x => x.Certifications.Contains(txtCredentials.Text));
			}

			// filter: federal tax id
			if (!string.IsNullOrEmpty(txtFEIN.Text)) {
				predicate = predicate.And(x => x.FederalTaxID.Contains(txtFEIN.Text));
			}

			// filter: year experience
			if (int.TryParse(txtYearExperience.Text, out yearExperience) && yearExperience > 0) {
				predicate = predicate.And(x => x.YearsExperiece == yearExperience);
			}

			// filter: Geographical Area of Service
			if (!string.IsNullOrEmpty(txtServiceArea.Text)) {
				predicate = predicate.And(x => x.GeographicalSeriveArea.Contains(txtServiceArea.Text));
			}

			// filter: w9
			if (ddlW9.SelectedIndex > 0) {
				predicate = predicate.And(x => x.isW9 == ddlW9.SelectedValue.Equals("1"));
			}

			// filter: Types of Claims Handled
			if (ddlTypeClaimHandled.SelectedIndex > 0) {
				policyTypeID = Convert.ToInt32(ddlTypeClaimHandled.SelectedValue);

				predicate = predicate.And(x => x.AdjusterHandleClaimType.Any(a => a.PolicyTypeID == policyTypeID));
			}

			// State(s) of Service & Licensure per State
			if (ddlServiceStateLicense.SelectedIndex > 0) {
				stateID = Convert.ToInt32(ddlServiceStateLicense.SelectedValue);

				predicate = predicate.And(x => x.AdjusterServiceArea.Any(a => a.StateID == stateID));
			}

			gvAdjuster.DataSource = AdjusterManager.Search(predicate).ToList();
			gvAdjuster.DataBind();
		}

		protected void gvAdjuster_RowDataBound(object sender, GridViewRowEventArgs e) {
			AdjusterMaster adjuster = null;
			Image adjusterPhoto = null;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				adjuster = e.Row.DataItem as AdjusterMaster;

				if (adjuster != null) {
					Repeater repeaterStateLicense = e.Row.FindControl("repeaterStateLicense") as Repeater;
					if (repeaterStateLicense != null) {
						repeaterStateLicense.DataSource = adjuster.AdjusterServiceArea;
						repeaterStateLicense.DataBind();
					}

					// type of claim handled
					Repeater repeaterTypeClaimHandle = e.Row.FindControl("repeaterTypeClaimHandle") as Repeater;
					if (repeaterTypeClaimHandle != null) {
						repeaterTypeClaimHandle.DataSource = adjuster.AdjusterHandleClaimType;
						repeaterTypeClaimHandle.DataBind();
					}

					adjusterPhoto = e.Row.FindControl("adjusterPhoto") as Image;


					adjusterPhoto.ImageUrl = Core.Common.getAdjusterPhotoURL(adjuster.AdjusterId, adjuster.PhotoFileName);
				}
			}
		}

		protected void gvAdjuster_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			gvAdjuster.PageIndex = e.NewPageIndex;

			btnSearch_Click(null, null);
		}
	}

}