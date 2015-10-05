

using CRM.Web.Utilities;

namespace CRM.Web.UserControl.Admin {
	#region Namespace
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using CRM.Core;
	using CRM.Data.Account;
	using CRM.Data;
	using System.Transactions;
	using LinqKit;
    using CRM.Data.Entities;
	#endregion
	public partial class ucUserLeads : System.Web.UI.UserControl {
		int roleID = SessionHelper.getUserRoleId();
		


		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				DoBind();
				//bindAllLeads();
			}
		}

		List<Leads> objLead = null;
		private void DoBind() {
			int clientID = 0;
			
			clientID = SessionHelper.getClientId();

			var predicate = PredicateBuilder.True<CRM.Data.Entities.Leads>();
			int userLead = Convert.ToInt32(Session["UserId"]);

			if (!String.IsNullOrWhiteSpace(hfFromDate.Value)) {
				DateTime sDate = new DateTime(Convert.ToInt32(hfFromDate.Value.Trim().Substring(6, 4)), Convert.ToInt32(hfFromDate.Value.Trim().Substring(3, 2)), Convert.ToInt32(hfFromDate.Value.Trim().Substring(0, 2)));

				var datefrom = Convert.ToDateTime(sDate);
				predicate = predicate.And(Lead => Lead.OriginalLeadDate >= datefrom
								    );
			}
			if (!String.IsNullOrWhiteSpace(hfToDate.Value)) {
				DateTime sDate = new DateTime(Convert.ToInt32(hfToDate.Value.Trim().Substring(6, 4)), Convert.ToInt32(hfToDate.Value.Trim().Substring(3, 2)), Convert.ToInt32(hfToDate.Value.Trim().Substring(0, 2)));

				var dateto = Convert.ToDateTime(sDate);
				predicate = predicate.And(Lead => Lead.OriginalLeadDate <= dateto
								    );
			}


			predicate = predicate.And(Lead => Lead.UserId == userLead);

			// 2013-10-22
			predicate = predicate.And(Lead => Lead.LeadPolicy.Any(x => x.IsActive));

			// tortega 2013-08-02 - get lead for client id			
			if ((roleID == (int)UserRole.Client || roleID == (int)UserRole.SiteAdministrator) && clientID > 0) {
				
				predicate = predicate.And(Lead => Lead.ClientID == clientID);
			}


			objLead = LeadsManager.GetPredicate(predicate);

			if (objLead != null && objLead.Count > 0) {
				gvUserLeads.DataSource = objLead;
				gvUserLeads.DataBind();
			}
			else {
				gvUserLeads.DataSource = null;
				gvUserLeads.DataBind();
			}
			bindAllLeads();

			// 2013-07-17
			bindTasks();
		}
		
		private void bindAllLeads() {
			int clientID = 0;

			List<Leads> objLeadAll = null;

			clientID = SessionHelper.getClientId();

			var predicate = PredicateBuilder.True<CRM.Data.Entities.Leads>();
			int userLead = Convert.ToInt32(Session["UserId"]);

			if (!String.IsNullOrWhiteSpace(hfFromDate.Value)) {
				DateTime sDate = new DateTime(Convert.ToInt32(hfFromDate.Value.Trim().Substring(6, 4)), Convert.ToInt32(hfFromDate.Value.Trim().Substring(3, 2)), Convert.ToInt32(hfFromDate.Value.Trim().Substring(0, 2)));

				var datefrom = Convert.ToDateTime(sDate);
				predicate = predicate.And(Lead => Lead.OriginalLeadDate >= datefrom
								    );
			}
			if (!String.IsNullOrWhiteSpace(hfToDate.Value)) {
				DateTime sDate = new DateTime(Convert.ToInt32(hfToDate.Value.Trim().Substring(6, 4)), Convert.ToInt32(hfToDate.Value.Trim().Substring(3, 2)), Convert.ToInt32(hfToDate.Value.Trim().Substring(0, 2)));

				var dateto = Convert.ToDateTime(sDate);
				predicate = predicate.And(Lead => Lead.OriginalLeadDate <= dateto
								    );
			}
			predicate = predicate.And(Lead => Lead.UserId == 0);
			
			// tortega 2013-08-02 - get lead for client id			
			if ((roleID == (int)UserRole.Client || roleID == (int)UserRole.SiteAdministrator) && clientID > 0) {
				clientID = SessionHelper.getClientId();

				predicate = predicate.And(Lead => Lead.ClientID == clientID);
			}

			objLeadAll = LeadsManager.GetPredicate(predicate);

			if (objLeadAll != null && objLeadAll.Count > 0) {
				grdAllLead.DataSource = objLeadAll;
				grdAllLead.DataBind();
			}
			else {
				grdAllLead.DataSource = null;
				grdAllLead.DataBind();
			}
		}

		private void bindTasks() {
			int roleID = 0;
			int clientID = 0;
			int userID = 0;
			DateTime fromDate = DateTime.Today;
			DateTime toDate = DateTime.Today.AddHours(24).AddMinutes(-1);

			//int userID = 0;
			IQueryable<LeadTask> tasks = null;

			roleID = SessionHelper.getUserRoleId();

			if (roleID == (int)UserRole.Administrator) {
				// load tasks for user "Admin". Admin has not clientid associated with it.
				tasks = TasksManager.GetLeadTask(fromDate, toDate);
			}
			else if ((roleID == (int)UserRole.Client || roleID == (int)UserRole.SiteAdministrator) && Session["ClientId"] != null) {
				// load all tasks for client (sort of admin)
				clientID = SessionHelper.getClientId();

				tasks = TasksManager.GetLeadTask(clientID, fromDate, toDate);
			}
			else {
				userID = SessionHelper.getUserId();
				tasks = TasksManager.GetLeadTaskByUserID(userID, fromDate, toDate);
			}
			gvTasks.DataSource = tasks.ToList();
			gvTasks.DataBind();
		}

		protected void gvUserLeads_RowCommand(object sender, GridViewCommandEventArgs e) {
			if (e.CommandName == "DoView") {

				this.Context.Items.Add("selectedleadid", e.CommandArgument.ToString());
				this.Context.Items.Add("view", "U");
				Server.Transfer("~/protected/admin/newlead.aspx");
			}
			if (e.CommandName == "DoEdit") {

				this.Context.Items.Add("selectedleadid", e.CommandArgument.ToString());
				this.Context.Items.Add("view", "");
				Server.Transfer("~/protected/admin/newlead.aspx");
			}
			if (e.CommandName == "DoCopy") {
				int LId = Convert.ToInt32(e.CommandArgument);
				try {
					var list = LeadsManager.GetByLeadId(LId);
					Leads objlead = new Leads();// CRM.Web.Utilities.cln.Clone(list);

					//objlead.Adjuster = list.Adjuster;
					//objlead.AllDocumentsOnFile = list.AllDocumentsOnFile;
					objlead.BusinessName = list.BusinessName;
					objlead.CityId = list.CityId;
					objlead.ClaimantAddress = list.ClaimantAddress;
					objlead.ClaimantComments = list.ClaimantComments;
					objlead.ClaimantFirstName = list.ClaimantFirstName;
					objlead.ClaimantLastName = list.ClaimantLastName;
					objlead.ClaimsNumber = list.ClaimsNumber;
					objlead.DateSubmitted = list.DateSubmitted;
					objlead.EmailAddress = list.EmailAddress;
					//objlead.FloodPolicy = list.FloodPolicy;
					//objlead.Habitable = list.Habitable;
					objlead.hasCertifiedInsurancePolicy = list.hasCertifiedInsurancePolicy;
					objlead.hasContentList = list.hasContentList;
					objlead.hasDamageEstimate = list.hasDamageEstimate;
					objlead.hasDamagePhoto = list.hasDamagePhoto;
					objlead.hasDamageReport = list.hasDamageReport;
					objlead.hasInsurancePolicy = list.hasInsurancePolicy;
					objlead.hasOwnerContract = list.hasOwnerContract;
					objlead.hasSignedRetainer = list.hasSignedRetainer;
					objlead.HearAboutUsDetail = list.HearAboutUsDetail;
					objlead.InspectorCell = list.InspectorCell;
					objlead.InspectorEmail = list.InspectorEmail;
					objlead.InspectorName = list.InspectorName;
					//objlead.InsuranceAddress = list.InsuranceAddress;
					//objlead.InsuranceCity = list.InsuranceCity;
					//objlead.InsuranceCompanyName = list.InsuranceCompanyName;
					//objlead.InsuranceContactEmail = list.InsuranceContactEmail;
					//objlead.InsuranceContactName = list.InsuranceContactName;
					//objlead.InsuranceContactPhone = list.InsuranceContactPhone;
					//objlead.InsurancePolicyNumber = list.InsurancePolicyNumber;
					//objlead.InsuranceState = list.InsuranceState;
					//objlead.InsuranceZipCode = list.InsuranceZipCode;
					objlead.IsSubmitted = list.IsSubmitted;
					objlead.LastContactDate = list.LastContactDate;
					//objlead.LeadComments = list.LeadComments;
					objlead.LeadSource = list.LeadSource;
					//objlead.LeadStatus = list.LeadStatus;
					objlead.LFUUID = list.LFUUID;
					objlead.LossAddress = list.LossAddress;
					objlead.MarketValue = list.MarketValue;
					objlead.OriginalLeadDate = list.OriginalLeadDate;
					objlead.OtherSource = list.OtherSource;
					objlead.OwnerFirstName = list.OwnerFirstName;
					objlead.OwnerLastName = list.OwnerLastName;
					objlead.OwnerPhone = list.OwnerPhone;
					objlead.OwnerSame = list.OwnerSame;
					objlead.PhoneNumber = list.PhoneNumber;
					objlead.PrimaryProducerId = list.PrimaryProducerId;
					objlead.PropertyDamageEstimate = list.PropertyDamageEstimate;
					objlead.ReporterToInsurer = list.ReporterToInsurer;
					objlead.SecondaryEmail = list.SecondaryEmail;
					objlead.SecondaryPhone = list.SecondaryPhone;
					objlead.SecondaryProducerId = list.SecondaryProducerId;
					objlead.SiteInspectionCompleted = list.SiteInspectionCompleted;
					objlead.SiteLocation = list.SiteLocation;
					objlead.SiteSurveyDate = list.SiteSurveyDate;
					objlead.StateId = list.StateId;
					objlead.Status = list.Status;
					//objlead.SubStatus = list.SubStatus;
					objlead.TypeOfDamage = list.TypeOfDamage;
					objlead.TypeofDamageText = list.TypeofDamageText;
					objlead.TypeOfProperty = list.TypeOfProperty;
					objlead.UserId = list.UserId;
					objlead.WebformSource = list.WebformSource;
					//objlead.WindPolicy = list.WindPolicy;
					objlead.Zip = list.Zip;

					LeadsManager.Save(objlead);



					lblSave.Text = "Record Copied Sucessfully.";
					lblSave.Visible = true;
					DoBind();
				}
				catch (Exception ex) {
					lblError.Text = "Record Not Copied.";
					lblError.Visible = true;
				}
			}
		}

		protected void gvUserLeads_SelectedIndexChanging(object sender, GridViewSelectEventArgs e) {

			DoBind();
		}

		protected void gvUserLeads_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			string expression = "SecUser.UserName";
			string grid = "gvUser";
			gvUserLeads.PageIndex = e.NewPageIndex;
			//bindLeads();
			if (ViewState["Expression"] != null)
				expression = ViewState["Expression"].ToString();

			SortGrid(expression, false, grid);
			//gvUserLeads.PageIndex = e.NewPageIndex;
			// DoBind();
		}

		protected void grdAllLead_RowCommand(object sender, GridViewCommandEventArgs e) {
			if (e.CommandName == "DoView") {

				this.Context.Items.Add("selectedleadid", e.CommandArgument.ToString());
				this.Context.Items.Add("view", "U");
				Server.Transfer("~/protected/admin/newlead.aspx");
			}
			if (e.CommandName == "DoEdit") {

				this.Context.Items.Add("selectedleadid", e.CommandArgument.ToString());
				this.Context.Items.Add("view", "");
				Server.Transfer("~/protected/admin/newlead.aspx");
			}
			if (e.CommandName == "DoCopy") {
				int LId = Convert.ToInt32(e.CommandArgument);
				try {
					var list = LeadsManager.GetByLeadId(LId);
					Leads objlead = new Leads();// CRM.Web.Utilities.cln.Clone(list);

					//objlead.Adjuster = list.Adjuster;
					//objlead.AllDocumentsOnFile = list.AllDocumentsOnFile;
					objlead.BusinessName = list.BusinessName;
					objlead.CityId = list.CityId;
					objlead.ClaimantAddress = list.ClaimantAddress;
					objlead.ClaimantComments = list.ClaimantComments;
					objlead.ClaimantFirstName = list.ClaimantFirstName;
					objlead.ClaimantLastName = list.ClaimantLastName;
					objlead.ClaimsNumber = list.ClaimsNumber;
					objlead.DateSubmitted = list.DateSubmitted;
					objlead.EmailAddress = list.EmailAddress;
					//objlead.FloodPolicy = list.FloodPolicy;
					//objlead.Habitable = list.Habitable;
					objlead.hasCertifiedInsurancePolicy = list.hasCertifiedInsurancePolicy;
					objlead.hasContentList = list.hasContentList;
					objlead.hasDamageEstimate = list.hasDamageEstimate;
					objlead.hasDamagePhoto = list.hasDamagePhoto;
					objlead.hasDamageReport = list.hasDamageReport;
					objlead.hasInsurancePolicy = list.hasInsurancePolicy;
					objlead.hasOwnerContract = list.hasOwnerContract;
					objlead.hasSignedRetainer = list.hasSignedRetainer;
					objlead.HearAboutUsDetail = list.HearAboutUsDetail;
					objlead.InspectorCell = list.InspectorCell;
					objlead.InspectorEmail = list.InspectorEmail;
					objlead.InspectorName = list.InspectorName;
					//objlead.InsuranceAddress = list.InsuranceAddress;
					//objlead.InsuranceCity = list.InsuranceCity;
					//objlead.InsuranceCompanyName = list.InsuranceCompanyName;
					//objlead.InsuranceContactEmail = list.InsuranceContactEmail;
					//objlead.InsuranceContactName = list.InsuranceContactName;
					//objlead.InsuranceContactPhone = list.InsuranceContactPhone;
					//objlead.InsurancePolicyNumber = list.InsurancePolicyNumber;
					//objlead.InsuranceState = list.InsuranceState;
					//objlead.InsuranceZipCode = list.InsuranceZipCode;
					objlead.IsSubmitted = list.IsSubmitted;
					objlead.LastContactDate = list.LastContactDate;
					// objlead.LeadComments = list.LeadComments;
					objlead.LeadSource = list.LeadSource;
					//objlead.LeadStatus = list.LeadStatus;
					objlead.LFUUID = list.LFUUID;
					objlead.LossAddress = list.LossAddress;
					objlead.MarketValue = list.MarketValue;
					objlead.OriginalLeadDate = list.OriginalLeadDate;
					objlead.OtherSource = list.OtherSource;
					objlead.OwnerFirstName = list.OwnerFirstName;
					objlead.OwnerLastName = list.OwnerLastName;
					objlead.OwnerPhone = list.OwnerPhone;
					objlead.OwnerSame = list.OwnerSame;
					objlead.PhoneNumber = list.PhoneNumber;
					objlead.PrimaryProducerId = list.PrimaryProducerId;
					objlead.PropertyDamageEstimate = list.PropertyDamageEstimate;
					objlead.ReporterToInsurer = list.ReporterToInsurer;
					objlead.SecondaryEmail = list.SecondaryEmail;
					objlead.SecondaryPhone = list.SecondaryPhone;
					objlead.SecondaryProducerId = list.SecondaryProducerId;
					objlead.SiteInspectionCompleted = list.SiteInspectionCompleted;
					objlead.SiteLocation = list.SiteLocation;
					objlead.SiteSurveyDate = list.SiteSurveyDate;
					objlead.StateId = list.StateId;
					objlead.Status = list.Status;
					//objlead.SubStatus = list.SubStatus;
					objlead.TypeOfDamage = list.TypeOfDamage;
					objlead.TypeofDamageText = list.TypeofDamageText;
					objlead.TypeOfProperty = list.TypeOfProperty;
					objlead.UserId = list.UserId;
					objlead.WebformSource = list.WebformSource;
					//objlead.WindPolicy = list.WindPolicy;
					objlead.Zip = list.Zip;

					LeadsManager.Save(objlead);



					lblSave.Text = "Record Copyed Sucessfully.";
					lblSave.Visible = true;
					DoBind();
				}
				catch (Exception ex) {
					lblError.Text = "Record Not Copyed.";
					lblError.Visible = true;
				}
			}
		}

		protected void grdAllLead_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			string expression = "OriginalLeadDate";
			string grid = "grdAllLead";
			grdAllLead.PageIndex = e.NewPageIndex;
			// bindAllLeads();
			if (ViewState["Expression"] != null)
				expression = ViewState["Expression"].ToString();

			SortGrid(expression, false, grid);

			// grdAllLead.PageIndex = e.NewPageIndex;
			// bindAllLeads();
		}

		protected void btnSearch_Click(object sender, EventArgs e) {
			//lblError.Text = string.Empty;
			//lblError.Visible = false;
			//lblMessage.Text = string.Empty;
			//lblMessage.Visible = false;
			//lblSave.Text = string.Empty;
			//lblSave.Visible = false;

			hfToDate.Value = txtDateTo.Text.Trim();
			hfFromDate.Value = txtDateFrom.Text.Trim();
			//hfCriteria.Value = txtSearch.Text.Trim();
			//hfClaimantName.Value = txtClaimantName.Text.Trim();
			//hfClaimantAddress.Value = txtClaimantAddress.Text.Trim();
			//hfPolicyNumber.Value = txtInsurancePolicyNumber.Text.Trim();
			//bindLeads();
			DoBind();
		}

		protected void btnReset_Click(object sender, EventArgs e) {
			hfToDate.Value = "";
			txtDateTo.Text = string.Empty;
			hfFromDate.Value = "";
			txtDateFrom.Text = string.Empty;
			DoBind();
			//txtClaimantAddress.Text = string.Empty;
			//txtClaimantName.Text = string.Empty;
			//txtInsurancePolicyNumber.Text = string.Empty;
			//hfClaimantAddress.Value = "";
			//hfClaimantName.Value = "";
			//hfPolicyNumber.Value = "";
			//txtSearch.Text = string.Empty;
			//hfCriteria.Value = "";
			//bindLeads();
			//divForAll.Visible = true;
		}

		protected void gvUserLeads_Sorting(object sender, GridViewSortEventArgs e) {
			string grid = "gvUser";
			SortGrid(e.SortExpression, true, grid);
			ViewState["Expression"] = e.SortExpression;
		}

		protected void grdAllLead_Sorting(object sender, GridViewSortEventArgs e) {
			string grid = "grdAllLead";
			SortGrid(e.SortExpression, true, grid);
			ViewState["Expression"] = e.SortExpression;
		}


		public void SortGrid(string expression, bool fromSort, string Grid) {
			int userLead = Convert.ToInt32(Session["UserId"]);
			var predicate = PredicateBuilder.True<CRM.Data.Entities.Leads>();

			if (!String.IsNullOrWhiteSpace(hfFromDate.Value)) {
				DateTime sDate = new DateTime(Convert.ToInt32(hfFromDate.Value.Trim().Substring(6, 4)), Convert.ToInt32(hfFromDate.Value.Trim().Substring(3, 2)), Convert.ToInt32(hfFromDate.Value.Trim().Substring(0, 2)));

				var datefrom = Convert.ToDateTime(sDate);
				predicate = predicate.And(Lead => Lead.OriginalLeadDate >= datefrom
								    );
			}
			if (!String.IsNullOrWhiteSpace(hfToDate.Value)) {
				DateTime sDate = new DateTime(Convert.ToInt32(hfToDate.Value.Trim().Substring(6, 4)), Convert.ToInt32(hfToDate.Value.Trim().Substring(3, 2)), Convert.ToInt32(hfToDate.Value.Trim().Substring(0, 2)));

				var dateto = Convert.ToDateTime(sDate);
				predicate = predicate.And(Lead => Lead.OriginalLeadDate <= dateto
								    );
			}


			predicate = predicate.And(Lead => Lead.Status != 0);
			if (Grid == "gvUser") {
				predicate = predicate.And(Lead => Lead.UserId == userLead);
			}
			else {
				predicate = predicate.And(Lead => Lead.UserId == 0);
			}
			var objLead1 = LeadsManager.GetPredicate(predicate).ToList().AsEnumerable();

			string sortExpression = expression;
			string direction = string.Empty;

			if (fromSort)
				if (SortDirection == SortDirection.Ascending) SortDirection = SortDirection.Descending;
				else SortDirection = SortDirection.Ascending;


			if (SortDirection == SortDirection.Ascending) {

				if (objLead1 != null) {

					switch (expression) {
						case "SecUser.UserName":
							objLead1 = objLead1.OrderBy(x => x.SecUser != null ? x.SecUser.UserName : "").ToList();
							break;
						case "OriginalLeadDate":
							objLead1 = objLead1.OrderBy(x => x.OriginalLeadDate).ToList();
							break;

						case "ClaimsNumber":
							objLead1 = objLead1.OrderBy(x => x.ClaimsNumber).ToList();
							break;
						//////////////////////////////////////
						//case "StatusMaster.StatusName":

						//	objLead1 = objLead1.OrderBy(x => x.StatusMaster != null ? x.StatusMaster.StatusName : "").ToList();
						//	break;
						//case "SubStatusMaster.SubStatusName":
						//	objLead1 = objLead1.OrderBy(x => x.SubStatusMaster != null ? x.SubStatusMaster.SubStatusName : "").ToList();
						//	break;
						case "SiteSurveyDate":
							objLead1 = objLead1.OrderBy(x => x.SiteSurveyDate).ToList();
							break;
						//case "AllDocumentsOnFile":
						//	objLead1 = objLead1.OrderBy(x => x.AllDocumentsOnFile).ToList();
						//	break;
						case "ClaimantFirstName":
							objLead1 = objLead1.OrderBy(x => x.ClaimantFirstName).ToList();
							break;
						case "ClaimantLastName":
							objLead1 = objLead1.OrderBy(x => x.ClaimantLastName).ToList();
							break;
						case "CityMaster_1.CityName":
							objLead1 = objLead1.OrderBy(x => x.CityMaster != null ? x.CityMaster.CityName : "").ToList();
							break;
						case "StateMaster1.StateCode":
							objLead1 = objLead1.OrderBy(x => x.StateMaster != null ? x.StateMaster.StateCode : "").ToList();
							break;
						case "Zip":
							objLead1 = objLead1.OrderBy(x => x.Zip).ToList();
							break;
						case "LeadSourceMaster.LeadSourceName":
							objLead1 = objLead1.OrderBy(x => x.LeadSourceMaster != null ? x.LeadSourceMaster.LeadSourceName : "").ToList();
							break;
						case "TypeOfDamageText":
							objLead1 = objLead1.OrderBy(x => x.TypeofDamageText).ToList();
							break;
						case "TypeOfPropertyMaster.TypeOfProperty":
							objLead1 = objLead1.OrderBy(x => x.TypeOfPropertyMaster != null ? x.TypeOfPropertyMaster.TypeOfProperty : "").ToList();
							break;

					}

				}
			}
			else {

				if (objLead1 != null) {

					switch (expression) {
						case "SecUser.UserName":

							objLead1 = objLead1.OrderByDescending(x => x.SecUser != null ? x.SecUser.UserName : "").ToList();
							break;
						case "OriginalLeadDate":
							objLead1 = objLead1.OrderByDescending(x => x.OriginalLeadDate).ToList();
							break;
						case "ClaimsNumber":
							objLead1 = objLead1.OrderByDescending(x => x.ClaimsNumber).ToList();
							break;
						//case "StatusMaster.StatusName":
						//	objLead1 = objLead1.OrderByDescending(x => x.StatusMaster != null ? x.StatusMaster.StatusName : "").ToList();
						//	break;
						//case "SubStatusMaster.SubStatusName":
						//	objLead1 = objLead1.OrderByDescending(x => x.SubStatusMaster != null ? x.SubStatusMaster.SubStatusName : "").ToList();
						//	break;
						case "SiteSurveyDate":
							objLead1 = objLead1.OrderByDescending(x => x.SiteSurveyDate).ToList();
							break;
						//case "AllDocumentsOnFile":
						//	objLead1 = objLead1.OrderByDescending(x => x.AllDocumentsOnFile).ToList();
						//	break;
						case "ClaimantFirstName":
							objLead1 = objLead1.OrderByDescending(x => x.ClaimantFirstName).ToList();
							break;
						case "ClaimantLastName":
							objLead1 = objLead1.OrderByDescending(x => x.ClaimantLastName).ToList();
							break;
						case "CityMaster_1.CityName":
							objLead1 = objLead1.OrderByDescending(x => x.CityMaster != null ? x.CityMaster.CityName : "").ToList();
							break;
						case "StateMaster1.StateCode":
							objLead1 = objLead1.OrderByDescending(x => x.StateMaster != null ? x.StateMaster.StateCode : "").ToList();
							break;
						case "Zip":
							objLead1 = objLead1.OrderByDescending(x => x.Zip).ToList();
							break;
						case "LeadSourceMaster.LeadSourceName":
							objLead1 = objLead1.OrderByDescending(x => x.LeadSourceMaster != null ? x.LeadSourceMaster.LeadSourceName : "").ToList();
							break;
						case "TypeOfDamageText":
							objLead1 = objLead1.OrderByDescending(x => x.TypeofDamageText).ToList();
							break;
						case "TypeOfPropertyMaster.TypeOfProperty":
							objLead1 = objLead1.OrderBy(x => x.TypeOfPropertyMaster != null ? x.TypeOfPropertyMaster.TypeOfProperty : "").ToList();
							break;
					}

				}
			}
			if (Grid == "gvUser") {
				gvUserLeads.DataSource = objLead1;
				gvUserLeads.DataBind();
			}
			else {
				grdAllLead.DataSource = objLead1;
				grdAllLead.DataBind();
			}
		}
		public SortDirection SortDirection {
			get {
				if (ViewState["SortDirection"] == null) {
					ViewState["SortDirection"] = SortDirection.Ascending;
				}
				return (SortDirection)ViewState["SortDirection"];
			}
			set {
				ViewState["SortDirection"] = value;
			}
		}

	}
}