using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;

using CRM.RuleEngine;
using CRM.Data.Entities;

namespace CRM.Web.UserControl {


	public partial class ucClaimEdit : System.Web.UI.UserControl {
		//public delegate void statusChangeHandler(object sender, EventArgs e);
		//public event statusChangeHandler statusChanged;

		public int claimID {
			get {
				return Session["ClaimID"] != null ? Convert.ToInt32(Session["ClaimID"]) : 0;
			}
			set {
				Session["ClaimID"] = value;
			}
		}

		public int policyID {
			get {
				return Session["policyID"] != null ? Convert.ToInt32(Session["policyID"]) : 0;
			}
		}

        private int carrierID
        {
            get
            {
                return Session["CarrierID"] != null ? Convert.ToInt32(Session["CarrierID"]) : 0;
            }
        }



		int clientID = 0;
		int leadID = 0;


		protected CRM.Web.Protected.ClaimRuler masterPage = null;

		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master.Master as Protected.ClaimRuler;

			// check permissions
			if (Core.PermissionHelper.checkViewPermission("UsersLeads.aspx") && !Core.PermissionHelper.checkEditPermission("UsersLeads.aspx"))
				masterPage.disableControls(this.pnlContent, false);

			showFormFields();
            Session["InvoiceProfileID"] = ddlCarrierInvoiceProfile.SelectedValue;
            var a = Session["InvoiceProfileID"];

		}

		private void activateTabs(bool isVisible) {
			this.tabPanelComments.Visible = isVisible;
			this.tabPanelContacts.Visible = isVisible;
			this.tabPanelDocuments.Visible = isVisible;
		}

		private void bindCarrierInvoiceProfile(int carrierID) {
			List<CarrierInvoiceProfile> invoiceProfiles = CarrierInvoiceProfileManager.GetAll(carrierID);
            
			CollectionManager.FillCollection(ddlCarrierInvoiceProfile, "CarrierInvoiceProfileID", "ProfileName", invoiceProfiles);
		}

		public void bindData(int claimID, bool isPrimeSessionFields = false) {
			// main binding routine 
			Claim claim = null;
			clientID = SessionHelper.getClientId();
			leadID = SessionHelper.getLeadId();
			CRM.Data.Entities.LeadPolicy policy = null;
            
            hf_ClaimIdForStatus.Value =Convert.ToString(claimID);

			List<StatusMaster> statusMasters = null;
			List<SubStatusMaster> subStatusMasters = null;
			List<ProgressStatus> progressStatuses = null;


			// check for policy limits were defined. if no limit, create them automatically.
			// this is to fix claims created for the app.clamruler.com
			checkPolicyLimits(claimID, this.policyID);

			statusMasters = StatusManager.GetList(clientID);

			subStatusMasters = SubStatusManager.GetAll(clientID);

			CollectionManager.Fillchk(chkLossType, "TypeOfDamageId", "TypeOfDamage", TypeofDamageManager.GetAll(clientID, false));

			CollectionManager.FillCollection(ddlLeadStatus, "StatusId", "StatusName", statusMasters);
            //chetu code
            CollectionManager.FillCollection(ddlClaimStatusReview, "StatusId", "StatusName", statusMasters);
            fillClaimStatusReview(clientID);

            //Bind InvoiceType

            CollectionManager.FillCollection(ddlInvoiceType, "InvoiceTypeID", "InvoiceTypes", InvoiceTypeManager.GetAll());


			CollectionManager.FillCollection(ddlSubStatus, "SubStatusId", "SubStatusName", subStatusMasters);

			CollectionManager.FillCollection(ddlSupervisors, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.Supervisor));

			CollectionManager.FillCollection(ddlManager, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.OwnerManager));

			//CollectionManager.FillCollection(ddlOwnerManagerEntityName, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.OwnerManagerEntityName));



            ddlOwnerManagerEntityName.DataSource = CarrierLocationManager.GetCarrierLocation(clientID); ;
            ddlOwnerManagerEntityName.DataValueField = "CarrierLocationID";
            ddlOwnerManagerEntityName.DataTextField = "LocationName";
            ddlOwnerManagerEntityName.DataBind();
            ddlOwnerManagerEntityName.Items.Insert(0, new ListItem("--- Select ---", "0"));

			CollectionManager.FillCollection(ddlTeamLeader, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.TeamLead));

			// bind contacts
			CollectionManager.FillCollection(ddlOutsideAdjuster, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.OutsideAdjuster));
			CollectionManager.FillCollection(ddlContentsAdjuster, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.ContentsAdjuster));
			CollectionManager.FillCollection(ddlExaminer, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.Examiner));
			CollectionManager.FillCollection(ddlCompanyBuilder, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.CompanyBuilder));
			CollectionManager.FillCollection(ddlCompanyInventory, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.CompanyInventory));
			CollectionManager.FillCollection(ddlOurBuilder, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.OurBuilder));
			CollectionManager.FillCollection(ddlInventoryCompany, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.InventoryCompany));
            //NEW OC 11/10 EXAMINER
            CollectionManager.FillCollection(ddlEstimator, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.Estimator));
            CollectionManager.FillCollection(ddlDeskAdjuster, "ContactID", "ContactName", ContactManager.GetAll(clientID, (int)Globals.ContactType.DeskAdjuster));
			// 2014-04-27
			using (ProgressStatusManager repository = new ProgressStatusManager()) 
            {
				progressStatuses = repository.GetAll();
			}
			CollectionManager.FillCollection(ddlProgressStatus, "ProgressStatusID", "ProgressDescription", progressStatuses);
			

			claim = ClaimsManager.Get(claimID);

			if (claim != null) {
				// carrier invoice program
				if (claim.LeadPolicy != null && claim.LeadPolicy.Carrier != null) {
					bindCarrierInvoiceProfile(claim.LeadPolicy.Carrier.CarrierID);
				}

				if (isPrimeSessionFields) {
					// prime session fields when claim is access via direct URL
					this.claimID = claim.ClaimID;

					SessionHelper.setPolicyID(claim.LeadPolicy.Id);

					SessionHelper.setLeadId(claim.LeadPolicy.LeadId);
				}

				activateTabs(true);

				fillForm(claim);

				claimComments.dataBind(claimID);

				claimContacts.bindData(claimID);

				claimDocuments.bindData(claimID);

                propertyLimits.bindData(this.policyID);

				//casualtyLimits.bindData(this.claimID);

				propertySubLimits.bindData(this.claimID);

				//bindDocuments(leadID);

				//bindContacts();

				//bindLienHolders();

				//fillComments(leadID);

				//bindCoverages();

				//activateLinks();
                if (claim.AdjusterID.HasValue)
                {
                    // adde two new fiels
                    AdjusterSettingsPayroll adjusterSettingsPayroll = AdjusterManager.GetAdjusterSttingPayrollData(claim.AdjusterID.Value);

                    if (adjusterSettingsPayroll != null)
                    {
                        txtAdjusterBranch.Text = adjusterSettingsPayroll.AdjusterBranch;
                        txtBranchCode.Text = adjusterSettingsPayroll.BranchCode;
                       
                    }
                }      

			}
			else {
				// new claim
				activateTabs(false);

				fillPolicyInfo(policyID);

                propertyLimits.bindData(this.policyID);

				//casualtyLimits.bindData(this.claimID);

				propertySubLimits.bindData(this.claimID);

				ViewState["LastStatusID"] = "0";

				//clearFields();
			}
		}

		private void checkPolicyLimits(int claimID, int policyID) {

			bool hasPolicyLimits = false;

			hasPolicyLimits = PolicyLimitManager.Exists(policyID, LimitType.LIMIT_TYPE_PROPERTY);

			if (hasPolicyLimits == false) {
				PolicyLimitManager.primePolicyLimits(policyID);
			}
		}

		protected void clearFields() {
			ddlLeadStatus.SelectedIndex = -1;
			ddlSubStatus.SelectedIndex = -1;
			//ddlAdjuster.SelectedItemIndex = 0;

			txtAdjuster.Text = string.Empty;

			txtClaimNumber.Text = string.Empty;
			txtPolicyNumber.Text = string.Empty;

			txtLossDate.Text = string.Empty;
		}

		public void showFormFields() {
			List<FormFieldView> formFields = null;
			clientID = SessionHelper.getClientId();
			string strFieldID = null;

			using (DataFormManager repository = new DataFormManager()) {
				formFields = repository.GetFormFields(1, clientID);
			}

			foreach (FormFieldView field in formFields) {
				strFieldID = "f_" + field.FieldID.ToString();
				//HtmlTableRow tr = (HtmlTableRow)tabPanelClaim.FindControl(strFieldID) as HtmlTableRow;
				Control c = (Control)tabPanelClaim.FindControl(strFieldID) as Control;
				if (c != null)
					c.Visible = field.IsVisible;
			}
		}

		private void fillForm(Claim claim) {

			Session["InsuredName"] = claim.LeadPolicy.Leads.insuredName;
            //Session["myClient"] = claim.ManagerEntityID.Value;
            //Session["myInsurer"] = claim.LeadPolicy.Leads.InsuranceCompanyName;
            //Session["myCarrierType"] = claim.LeadPolicy.Carrier.CarrierInvoiceProfile;
			fillPolicyInfo(claim.LeadPolicy);
			

			ddlCarrierInvoiceProfile.SelectedValue = (claim.CarrierInvoiceProfileID ?? 0).ToString();
            Session["CarrierType"] = ddlCarrierInvoiceProfile.SelectedItem.Text;
			ViewState["LastStatusID"] = (claim.StatusID ?? 0).ToString();

			//ddlAdjuster.SelectedValue = (claim.AdjusterID ?? 0).ToString();
			if (claim.AdjusterMaster == null) {
				txtAdjuster.Text = "Unassigned";
				hf_adjusterID.Value = "0";
			}
			else {
				txtAdjuster.Text = claim.AdjusterMaster.adjusterName;
				hf_adjusterID.Value = claim.AdjusterMaster.AdjusterId.ToString();

                txtClaimAdjuster.Text = claim.AdjusterMaster.adjusterName;
                hf_ClaimAdjusterID.Value = claim.AdjusterMaster.AdjusterId.ToString();
                Client c = ClaimsManager.GetClientByUserId(SessionHelper.getUserId());
                if(c!=null)
                {
                txtAdjusterComapnyName.Text = c.BusinessName;
                }
                ddlClaimCarrier.SelectedValue = claim.LeadPolicy.CarrierID.ToString();
                txtInsurerName.Text = claim.LeadPolicy.Leads.insuredName;
			}





			fillOTherAdjusters(claim);

			ddlSupervisors.SelectedValue = (claim.SupervisorID ?? 0).ToString();
			ddlTeamLeader.SelectedValue = (claim.TeamLeadID ?? 0).ToString();

			txtClaimNumber.Text = claim.AdjusterClaimNumber;
			txtInsurerClaimNumber.Text = claim.InsurerClaimNumber;
            txtInsurerClaimId.Text = claim.InsurerClaimNumber;

            //if (claim.LeadPolicy.ApplyAcrossAllCoverage != null && claim.LeadPolicy.ApplyAcrossAllCoverage == true)
            //{
            //    txtDeductible.Enabled = false;
            //    acrossAllCoverages.Checked = true;

            //    acrossAllCoverages.Enabled = false;
            //    coverageSpecific.Enabled = false;

            //    coverageSpecific.Checked = false;
            //    hdnApplyDeductible.Value = "1";
               
            //}
            //else
            //{
            //    acrossAllCoverages.Enabled = true;
            //    coverageSpecific.Enabled = true;
            //    txtDeductible.Enabled = true;
            //    acrossAllCoverages.Checked = false;
            //    coverageSpecific.Checked = true;
            //    hdnApplyDeductible.Value = "0";
            //}

            //if (claim.LeadPolicy.ApplyDeductibleSet != null && claim.LeadPolicy.ApplyDeductibleSet == true)
            //{
            //    acrossAllCoverages.Enabled = false;
            //    coverageSpecific.Enabled = false;
            //}



			txtSeverity.Text = claim.SeverityNumber == null ? null : claim.SeverityNumber.ToString();
			txtEventName.Text = claim.EventName;
			txtEventType.Text = claim.EventType;
            //chetu code
            txtAdjCommOveride.Text = claim.AdjCommOverride == null ? string.Empty : claim.AdjCommOverride.ToString();
            txtAdjCommFlatFeeOveride.Value = claim.AdjCommFlatFeeOverride == null ? string.Empty : claim.AdjCommFlatFeeOverride.ToString();
            txtCat.Text = claim.CatId == null ? string.Empty : claim.CatId.ToString();
            //txtTypeofLoss.Text = claim.TypeofLoss == null ? string.Empty : claim.TypeofLoss.ToString();
            ddlTypeOfLoss.SelectedValue = claim.TypeofLoss == null ? "None" : claim.TypeofLoss.ToString();

            ///////////
			fillCauseOfLoss(claim);

			ddlLeadStatus.SelectedValue = (claim.StatusID ?? 0).ToString();
			ddlSubStatus.SelectedValue = (claim.SubStatusID ?? 0).ToString();
            ddlClaimStatusReview.SelectedValue = (claim.StatusID ?? 0).ToString();
            txtLossDescription.Text = claim.LossDescription;//NEW 9/16 OC
			txtLossDate.Value = claim.LossDate; 
			txtDateOpened.Value = claim.DateOpenedReported;
            txtDateEntered.Value = claim.DateEntered;//NEW OC 11/10
            txtDateProjCompleted.Value = claim.DateProjCompleted; // NEW OC 11/10
			txtDateInitialReservedChanged.Value = claim.DateInitialReserveChange;
			txtDateAssigned.Value = claim.DateAssigned;
			txtDateAcknowledge.Value = claim.DateAcknowledged;
			txtDateFirstContactAttempted.Value = claim.DateFirstContactAttempt;
			txtDateContacted.Value = claim.DateContacted;
			txtDateInspectionSchedule.Value = claim.DateInspectionScheduled;
			txtDateInspectionCompleted.Value = claim.DateInspectionCompleted;
			txtDateSubmitted.Value = claim.DateSubmitted;
			txtDateIndemnityRequetsted.Value = claim.DateIndemnityPaymentRequested;
			txtDateIndemnityPaymentApproved.Value = claim.DateIndemnityPaymentApproved;
			txtDateIndemnityPaymentIssued.Value = claim.DateIndemnityPaymentIssued;
			txtDateClosed.Value = claim.DateClosed;
			txtDateFirstReopen.Value = claim.DateFirstReOpened;
			txtDateReopenCompleted.Value = claim.DateReopenCompleted;
			txtDateFinalClosed.Value = claim.DateFinalClosed;

			claim.DateReopenCompleted = txtDateReopenCompleted.Date;

			txtCycleTime.Text = String.Format("{0:0.00}", claim.CycleTime ?? 0);
			txtReopenCycleTime.Text = String.Format("{0:0.00}", claim.ReopenCycleTime ?? 0);
            //new OC 11/6/14
            ddlInvoiceType.SelectedValue = claim.InvoiceTypeID.ToString();//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			if (claim.FeeInvoiceDesignation != null)
				ddlPropertyFeeInvoiceDesignation.SelectedValue = claim.FeeInvoiceDesignation.ToString();

			txtGrossLossPayable.Value = claim.GrossLossPayable ?? 0;
			txtDepreciation.Value = claim.Depreciation ?? 0;
            //chetucode
            txtNonDepreciation.Value = claim.NonRecoverableDepreciation ?? 0;
            ///
			txtPolicyDeductible.Value = claim.Deductible ?? 0;

			cbxInvoiceReady.Checked = claim.IsInvoiceReady ?? false;

                   if (claim.LeadPolicy.Leads.LossOfUseAmount != null)
                   {
                       txtLossAmount.Text = Convert.ToString(claim.LeadPolicy.Leads.LossOfUseAmount);
                    }
                    if (claim.LeadPolicy.Leads.LossOfUseReserve != null)
                    {
                        txtLossReserve.Text = Convert.ToString(claim.LeadPolicy.Leads.LossOfUseReserve);
                    }
            //fill new entry
            if (claim.JobSizeCode != "-1" && claim.JobSizeCode!=null)
            {
                ddlJobSideCode.SelectedValue = claim.JobSizeCode;
            }
            if (claim.EstimateCount != "-1" && claim.EstimateCount != null)
            {
                ddlEstimateCount.SelectedValue = claim.EstimateCount;
            }
            if (claim.Mitigation != "-1" && claim.Mitigation != null)
            {
                ddlMitigation.SelectedValue = claim.Mitigation;
            }
            if (!string.IsNullOrEmpty(claim.ProfileCode))
            {
            txtProfileCode.Text = claim.ProfileCode;
            }
            //fill new fields

            txtOverHead.Text = claim.Overhead??"";
            txtProfit.Text=claim.profit??"";
            txtCumulative.Text=claim.CumulativeOp??"";
            txtDefaultRepairedBY.Text=claim.DefaultRepairedBy??"";
            chkDepriationMat.Checked=claim.DepOnMaterials??false;
            chkDepriationNonMat.Checked=claim.DepOnNonMaterials??false;
            chkDepriationTaxes.Checked=claim.DepOnTaxes ??false;
            if (claim.MaxAllowedDep!=null)
            {
                txtMaxDepriation.Text = claim.MaxAllowedDep.ToString();
            }

            txttaxJurisdiction.Text = claim.taxJurisdiction ?? "";








			// net claim payable
			txtNetClaimPayable.Value = claim.NetClaimPayable ?? 0;
            Session["ClaimPayable"] = txtNetClaimPayable.Text;//NEW OC requested to have this display at top in t&e screen
			// supervisors/manager/team lead
			ddlSupervisors.SelectedValue = (claim.SupervisorID ?? 0).ToString();
			ddlManager.SelectedValue = (claim.ManagerID ?? 0).ToString();
			ddlTeamLeader.SelectedValue = (claim.TeamLeadID ?? 0).ToString();
			ddlOwnerManagerEntityName.SelectedValue = (claim.ManagerEntityID ?? 0).ToString();//branch/insurer dropdown OC
            Session["InsurerBranch"] = ddlOwnerManagerEntityName.SelectedItem.Text;//NEW OC requested to have this display at top in t&e screen
			//Carrier Finance Details(Optional) 
			this.txtOutstandingIndemnityReserve.Text = String.Format("{0:0.00}", claim.OutstandingIndemnityReserve ?? 0);
			this.txtOutstandingLAEReserves.Text = String.Format("{0:0.00}", claim.OutstandingLAEReserves ?? 0);
			this.txtTotalIndemnityPaid.Text = String.Format("{0:0.00}", claim.TotalIndemnityPaid ?? 0);
			this.txtCoverageAPaid.Text = String.Format("{0:0.00}", claim.CoverageAPaid ?? 0);
			this.txtCoverageBPaid.Text = String.Format("{0:0.00}", claim.CoverageBPaid ?? 0);
			this.txtCoverageCPaid.Text = String.Format("{0:0.00}", claim.CoverageCPaid ?? 0);
			this.txtCoverageDPaid.Text = String.Format("{0:0.00}", claim.CoverageDPaid ?? 0);
			this.txtTotalExpensePaid.Text = String.Format("{0:0.00}", claim.TotalExpensesPaid ?? 0);

			// 2014-04-27 tortega
			ddlProgressStatus.SelectedValue = (claim.ProgressStatusID ?? 0).ToString();
		}

		private void fillCauseOfLoss(Claim claim) {
			Leads lead = null;
			string[] selectedValues = null;

			if (!string.IsNullOrEmpty(claim.CauseOfLoss)) {
				// use damage type from claim level
				selectedValues = claim.CauseOfLoss.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			}
			else {
				// use damage type from lead level
				lead = claim.LeadPolicy.Leads;
                

				if (lead != null && !string.IsNullOrEmpty(lead.TypeOfDamage))
					selectedValues = lead.TypeOfDamage.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			}

			if (selectedValues != null && selectedValues.Length > 0) {
				foreach (string value in selectedValues) {
					ListItem item = chkLossType.Items.FindByValue(value);
					if (item != null)
						item.Selected = true;
				}
			}

		}

		private void fillPolicyInfo(int policyID) {
			Data.Entities.LeadPolicy policy = LeadPolicyManager.GetWithLeadCarrier(policyID);

			// get active policy			
			if (policy != null) {			
				fillPolicyInfo(policy); 
			}
		}

		private void fillPolicyInfo(Data.Entities.LeadPolicy policy) {

			// get active policy			
			if (policy != null) {
				txtPolicyNumber.Text = policy.PolicyNumber;

				txtPolicyType.Text = policy.LeadPolicyType == null ? "" : policy.LeadPolicyType.Description;

				txtInsuranceCarrier.Text = policy.Carrier == null ? "" : policy.Carrier.CarrierName;
                Session["myClient"] = txtInsuranceCarrier.Text;
				//txtPolicyHolder.Text = string.Format("{0} {1}", policy.Leads.ClaimantFirstName, policy.Leads.ClaimantLastName);

                txtPolicyHolder.Text = policy.Leads == null ? "" : policy.Leads.InsuredName; //OC 9/3/14 to carry over the name from new lead to edit claim.

				//txtBusinessName.Text = policy.Leads.BusinessName ?? string.Empty;

				if (policy.CarrierID != null)
					bindCarrierInvoiceProfile((int)policy.CarrierID);
			}
		}

		private void fillOTherAdjusters(Claim claim) 
        {

			if (claim.OutsideAdjusterID != null) 
            {
				ddlOutsideAdjuster.SelectedValue = claim.OutsideAdjusterID.ToString();
			}

			if (claim.ContentAdjusterID != null) 
            {
				this.ddlContentsAdjuster.SelectedValue = claim.ContentAdjusterID.ToString();
			}

			if (claim.ExaminerID != null) 
            {
				this.ddlExaminer.SelectedValue = claim.ExaminerID.ToString();
			}
            if(claim.EstimatorID != null)//new oc estimator 11/10
            {
                ddlEstimator.SelectedValue = claim.EstimatorID.ToString();
            }
            if (claim.DeskAdjusterID != null) // new oc desk adjuster 11/10
            {
                ddlDeskAdjuster.SelectedValue = claim.DeskAdjusterID.ToString();
            }

			if (claim.CompanyBuilderID != null) 
            {
				this.ddlCompanyBuilder.SelectedValue = claim.CompanyBuilderID.ToString();
			}

			if (claim.InventoryCompanyID != null) 
            {
				this.ddlInventoryCompany.SelectedValue = claim.InventoryCompanyID.ToString();
			}

			if (claim.OurBuilderID != null) 
            {
				this.ddlOurBuilder.SelectedValue = claim.OurBuilderID.ToString();
			}

			if (claim.CompanyInventoryID != null) 
            {
				this.ddlCompanyInventory.SelectedValue = claim.CompanyInventoryID.ToString();
			}
		}
		private string getCauseOfLoss() {
			string causeOfLossTypes = null;

			string[] selectedValues = (from x in chkLossType.Items.Cast<ListItem>()
								  where x.Selected
								  select x.Value).ToArray<string>();

			if (selectedValues != null && selectedValues.Count() > 0)
				causeOfLossTypes = string.Join(",", selectedValues);

			return causeOfLossTypes;
		}

		protected void validateAdjuster(object sender, ServerValidateEventArgs args) {
			args.IsValid = !args.Value.Equals("--- Select ---");
		}

        private void notifyNewClaimCreated(int claimID)
        {
            string appUrl = null;
            AdjusterMaster adjuster = null;
            Claim claim = null;
            string[] cc = null;
            //Carrier carrier = null;
            string clientName = null;
            string claimantName = null;
            Contact claimSupervisor = null;
           // string[] damageTypes = null;
          //  string damageTypeDescription = null;
            StringBuilder emailBody = new StringBuilder();
            string encryptedClaimNumber = null;
            Leads lead = null;
            CRM.Data.Entities.LeadPolicy policy = null;
            string[] recipients = null;
            string subject = null;
            Client client = null;

            clientID = SessionHelper.getClientId();

            client = ClientManager.Get(clientID);

            

            claim = ClaimsManager.Get(claimID);

            if (claim == null || claim.AdjusterID == null)
                return;

            // get adjuster info
            adjuster = claim.AdjusterMaster;

            // retreive lead information			
            lead = claim.LeadPolicy.Leads;

            // retreive policy information			
            policy = claim.LeadPolicy;

            
                try
                {
                    recipients = new string[] { client.PrimaryEmailId };

                    string smtpHost = ConfigurationManager.AppSettings["smtpHost"].ToString();
                    int smtpPort = ConfigurationManager.AppSettings["smtpPort"] == null ? 25 : Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
                    string smtpEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
                    string smtpPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();

                    string fromEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
                    //emailPassword = ConfigurationManager.AppSettings["Password"].ToString();

                    // get claim supervisor
                    if (claim.SupervisorID != null)
                    {
                        claimSupervisor = ContactManager.Get((int)claim.SupervisorID);
                        if (claimSupervisor != null && !string.IsNullOrEmpty(claimSupervisor.Email))
                        {
                            cc = new string[] { claimSupervisor.Email };
                        }
                    }

                    // name of CRM client(portal)
                    clientName = adjuster.Client == null ? "" : adjuster.Client.BusinessName;

                    claimantName = lead.insuredName;

                    encryptedClaimNumber = Core.SecurityManager.EncryptQueryString(claimID.ToString());

                    appUrl = ConfigurationManager.AppSettings["siteURL"].ToString();

                    subject = string.Format("Insurer Claim # {0}, InsuredName: {1}, is a new claim that is awaiting attention in Claim Ruler Software.", claim.InsurerClaimNumber, lead.insuredName);

                    #region build email body
                    // .containerBox
                    emailBody.Append("<div style=\"margin:auto;width:600px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");

                    // .header
                    emailBody.Append("<div style=\"background-image:url('~/Images/email_header_small.jpg');background-repeat: no-repeat;background-size: 100% 100%;height:70px;\"></div>");
                    //emailBody.Append("test"); //("<div><img src=\"http://app.claimruler.com/images/email_header_small.jpg\"></image></div>");

                    // .paneContentInner
                    emailBody.Append("<div style=\"margin: 20px;\">");

                  //  emailBody.Append("Hi " + adjuster.FirstName + "!<br><br>");
                    emailBody.Append(string.Format("Your firm uses Claim Ruler software for claims management and Insurer Claim # {0} was just created.<br><br>", claim.InsurerClaimNumber));
                    emailBody.Append("Please review the claim right away to begin handling the file.<br>");
                    emailBody.Append("Time is of the essence!<br><br>");

                    emailBody.Append(string.Format("<p><a href=\"{0}/login.aspx?url=~/protected/claimedit.aspx?id={1}\">Please click here to access claim.</a></p>", appUrl, encryptedClaimNumber));

                    emailBody.Append("<br><br>Thank you.<br><br>");

                    // .containerBox
                   // emailBody.Append("<div style=\"margin:auto;width:550px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");
                    emailBody.Append("<table >");
                    emailBody.Append("<tr> ");
                    emailBody.Append("<td align=\"left\"><b>Insurer Claim #</b></td><td align=\"left\"> " + claim.InsurerClaimNumber + "</td>");
                    emailBody.Append("</tr>");
                    // emailBody.Append("</table><br><br>");


                    emailBody.Append("<tr>");
                    emailBody.Append("<td align=\"left\"><b>Client:</b></td><td align=\"left\"> " + client.BusinessName + "</td>");
                    emailBody.Append("</tr>");


                    emailBody.Append("<tr> ");
                    emailBody.Append("<td align=\"left\"><b>Insurer/Branch</b></td><td align=\"left\"> " + Session["InsurerBranch"] + "</td>");
                    emailBody.Append("</tr>");

                    emailBody.Append("<tr> ");
                    emailBody.Append("<td align=\"left\"><b>Insured</b></td><td align=\"left\"> " + lead.insuredName + "</td>");
                    emailBody.Append("</tr>");

                    emailBody.Append("<tr></tr>");

                    emailBody.Append("<tr> ");
                    emailBody.Append("<td align=\"left\"><b>Insured Home #</b></td><td align=\"left\"> " + lead.PhoneNumber + "</td>");
                    emailBody.Append("</tr>");

                    emailBody.Append("<tr> ");
                    emailBody.Append("<td align=\"left\"><b>Insured Mobile #</b></td><td align=\"left\"> " + lead.MobilePhone + "</td>");
                    emailBody.Append("</tr>");

                    emailBody.Append("<tr> ");
                    emailBody.Append("<td align=\"left\"><b>Insured Email Address</b></td><td align=\"left\"> " + lead.EmailAddress + "</td>");
                    emailBody.Append("</tr>");

                    emailBody.Append("<tr> ");
                    emailBody.Append("<td align=\"left\"><b>Policy Number</b></td><td align=\"left\"> " + policy.PolicyNumber + "</td>");
                    emailBody.Append("</tr>");


                    emailBody.Append("<tr> ");
                    emailBody.Append("<td align=\"left\"><b>Type Of Damage</b></td><td align=\"left\"> " + claim.TypeofLoss + "</td>");
                    emailBody.Append("</tr>");

                    emailBody.Append("<tr> ");
                    emailBody.Append("<td align=\"left\"><b>Loss Description</b></td><td align=\"left\"> " + claim.LossDescription + "</td>");
                    emailBody.Append("</tr>");
                    
                    
                    emailBody.Append("</table ><br><br>");
                  

                    #endregion

                    Core.EmailHelper.sendEmail(fromEmail, recipients, cc, subject, emailBody.ToString(), null, smtpHost, smtpPort, smtpEmail, smtpPassword, true);
                }
                catch (Exception ex)
                {
                    Core.EmailHelper.emailError(ex);
                }
            
        }

		private void notifyAdjuster(int claimID) {
			string appUrl = null;
			AdjusterMaster adjuster = null;
			Claim claim = null;
			string[] cc = null;
			Carrier carrier = null;
			string clientName = null;
			string claimantName = null;
			Contact claimSupervisor = null;
			string[] damageTypes = null;
			string damageTypeDescription = null;
			StringBuilder emailBody = new StringBuilder();
			string encryptedClaimNumber = null;
			Leads lead = null;
			CRM.Data.Entities.LeadPolicy policy = null;
			string[] recipients = null;
			string subject = null;


			claim = ClaimsManager.Get(claimID);

			if (claim == null || claim.AdjusterID == null)
				return;

			// get adjuster info
			adjuster = claim.AdjusterMaster;

			// retreive lead information			
			lead = claim.LeadPolicy.Leads;

			// retreive policy information			
			policy = claim.LeadPolicy;

			if (adjuster != null && lead != null && policy != null && !string.IsNullOrEmpty(adjuster.email) && (adjuster.isEmailNotification ?? true)) {
				try {
					recipients = new string[] { adjuster.email };

					string smtpHost = ConfigurationManager.AppSettings["smtpHost"].ToString();
					int smtpPort = ConfigurationManager.AppSettings["smtpPort"] == null ? 25 : Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
					string smtpEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
					string smtpPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();

                    string fromEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
					//emailPassword = ConfigurationManager.AppSettings["Password"].ToString();

					// get claim supervisor
					if (claim.SupervisorID != null) {
						claimSupervisor = ContactManager.Get((int)claim.SupervisorID);
						if (claimSupervisor != null && !string.IsNullOrEmpty(claimSupervisor.Email)) {
							cc = new string[] { claimSupervisor.Email };
						}
					}

					// name of CRM client(portal)
					clientName = adjuster.Client == null ? "" : adjuster.Client.BusinessName;

					claimantName = lead.insuredName;

					encryptedClaimNumber = Core.SecurityManager.EncryptQueryString(claimID.ToString());

					appUrl = ConfigurationManager.AppSettings["siteURL"].ToString();

					subject = string.Format("{0} has assigned {1} to you."  , clientName, claimantName);

					#region build email body
					// .containerBox
					emailBody.Append("<div style=\"margin:auto;width:600px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");

					// .header
				   emailBody.Append("<div style=\"background-image:url('~/Images/email_header_small.jpg');background-repeat: no-repeat;background-size: 100% 100%;height:70px;\"></div>");
                    //emailBody.Append("test"); //("<div><img src=\"http://app.claimruler.com/images/email_header_small.jpg\"></image></div>");

					// .paneContentInner
					emailBody.Append("<div style=\"margin: 20px;\">");

					emailBody.Append("Hi " + adjuster.FirstName + "!<br><br>");
					emailBody.Append(string.Format("Your firm uses Claim Ruler software for claims management and Claim # {0} was assigned to you.<br><br>", claim.InsurerClaimNumber));
					emailBody.Append(string.Format("Please review {0}'s claim right away to begin handling the file.<br>", claimantName));
					emailBody.Append("Time is of the essence!<br><br>");

					emailBody.Append(string.Format("<p><a href=\"{0}/login.aspx?url=~/protected/claimedit.aspx?id={1}\">Please click here to access claim.</a></p>", appUrl, encryptedClaimNumber));

					emailBody.Append("<br><br>Thank you.<br><br>");

					// .containerBox
					emailBody.Append("<div style=\"margin:auto;width:550px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");

					// .section-title
					emailBody.Append("<div style=\"background-image: url(http://app.claimruler.com/images/header-grad.gif);background-repeat: repeat-x;height: 18px; font-family: inheret;font-size: 12px; color: White;padding-left: 7px;padding-top: 1px;display: block;vertical-align: middle;\">Claim Details</div>");
					emailBody.Append("<table style=\"width:550px;\">");
					emailBody.Append(string.Format("<tr><td style=\"width:40%;\">Policyholder Entity(if Any)</td><td>{0}</td></tr>", ""));
					emailBody.Append(string.Format("<tr><td>Policyholder Name</td><td>{0}</td></tr>", claimantName ?? ""));
					emailBody.Append(string.Format("<tr><td>Policyholder Business</td><td>{0}</td></tr>", lead.BusinessName ?? ""));
					emailBody.Append(string.Format("<tr><td>Policyholder Phone #</td><td>{0}</td></tr>", lead.PhoneNumber ?? ""));
					emailBody.Append(string.Format("<tr><td>Policyholder E-mail Address #</td><td>{0}</td></tr>", lead.EmailAddress ?? ""));
					emailBody.Append(string.Format("<tr><td>Policy Number</td><td>{0}</td></tr>", policy.PolicyNumber ?? ""));
					emailBody.Append(string.Format("<tr><td>Carrier Name</td><td>{0}</td></tr>", carrier != null ? (carrier.CarrierName ?? "") : policy.InsuranceCompanyName));

					if (!string.IsNullOrEmpty(claim.CauseOfLoss)) {
						damageTypes = TypeofDamageManager.GetDescriptions(claim.CauseOfLoss);

						if (damageTypes != null && damageTypes.Length > 0)
							damageTypeDescription = string.Join(",", damageTypes);
					}

					emailBody.Append(string.Format("<tr><td>Type of Damage</td><td>{0}</td></tr>", damageTypeDescription ?? ""));

					emailBody.Append("</table>");
					emailBody.Append("</div>");	// inner containerBox

					emailBody.Append("</div>");	// paneContentInner 
					emailBody.Append("</div>");	// containerBox

					#endregion

					Core.EmailHelper.sendEmail(fromEmail, recipients, cc, subject, emailBody.ToString(), null, smtpHost, smtpPort, smtpEmail, smtpPassword, true);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}
		}

		private void setReminder(int masterStatusID) {
			int lastStatusID = 0;
			double duration = 0;
			LeadTask task = null;
			DateTime reminderDate = DateTime.MaxValue;
			string sdate = null;

			// return if no change in status
			int.TryParse(ViewState["LastStatusID"].ToString(), out lastStatusID);

			if (lastStatusID == masterStatusID)
				return;

			// get status with reminder duration
			StatusMaster statusMaster = StatusManager.GetStatusId(masterStatusID);

			// return if master status not loaded properly
			if (statusMaster == null)
				return;

			// get task associated with policy
			task = TasksManager.GetPolicyReminderTask(leadID, policyID);

			// master status has no reminder
			if (statusMaster.ReminderMaster == null) {
				// delete existing reminder
				if (task != null)
					TasksManager.Delete(task.id);
			}
			else {
				if (task == null) {
					// create new reminder as task
					task = new LeadTask();
					task.lead_id = leadID;
					task.lead_policy_id = policyID;
					//task.policy_type = policy.PolicyType;
				}

				if (SessionHelper.getClientId() > 0)
					task.creator_id = SessionHelper.getClientId();

				// update existing reminder
				task.master_status_id = masterStatusID;

				sdate = DateTime.Now.ToShortDateString() + " 5:00:00 AM";

				DateTime.TryParse(sdate, out reminderDate);

				// get duration from status reminder
				if (statusMaster.ReminderMaster != null)
					duration = (double)(statusMaster.ReminderMaster.Duration ?? 0);

				if ((int)statusMaster.ReminderMaster.DurationType == 1)
					task.start_date = reminderDate.AddHours(duration);
				else
					task.start_date = reminderDate.AddDays(duration);

				task.end_date = task.start_date;
				task.status_id = 1;
				task.text = "Alert";
				task.isAllDay = true;
				task.details = statusMaster.StatusName;// +"&lt;div&gt;Ins. Co: " + policy.InsuranceCompanyName + "&lt;/div&gt;&lt;div&gt;Pol: " + policy.PolicyNumber + "&lt;/div&gt;";
				task.owner_id = SessionHelper.getUserId();

				TasksManager.Save(task);
			}
		}



		protected void btnShowDiary_Click(object sender, EventArgs e) {
			pnlDiary.Visible = true;
		}

		protected void btnHiddenShowContact_Click(object sender, EventArgs e) {
			pnlContacts.Visible = true;
		}

		protected void btnHiddenShowDocuments_Click(object sender, EventArgs e) {
			pnlDocument.Visible = true;
			claimDocuments.bindData(this.claimID);
		}

		protected void cbxInvoiceReady_CheckedChanged(object sender, EventArgs e) {

		}

		private void calculateCommissionPerCarrierInvoiceProfile(CarrierInvoiceProfile invoiceProfile, out decimal rate, out decimal commissionAmount) {
			decimal claimAmount = 0;
			commissionAmount = 0;
			rate = 0;

			if (invoiceProfile != null && invoiceProfile.CarrierInvoiceProfileFeeSchedule != null && invoiceProfile.CarrierInvoiceProfileFeeSchedule.Count > 0) {

				claimAmount = getClaimAmount(invoiceProfile);


				foreach (CarrierInvoiceProfileFeeSchedule feeSchedule in invoiceProfile.CarrierInvoiceProfileFeeSchedule) {
					if (claimAmount >= feeSchedule.RangeAmountFrom && claimAmount <= feeSchedule.RangeAmountTo) {
						if (feeSchedule.FlatFee > 0) {
							commissionAmount = feeSchedule.FlatFee;
							rate = feeSchedule.FlatFee;
						}
						else if (feeSchedule.PercentFee > 0) {
							commissionAmount = claimAmount * feeSchedule.PercentFee;
							rate = feeSchedule.PercentFee * 100;
						}
						else if (feeSchedule.MinimumFee > 0) {
							commissionAmount = feeSchedule.MinimumFee;
							rate = feeSchedule.MinimumFee;
						}
					}
				}
			}

		}

		private decimal getClaimAmount(CarrierInvoiceProfile invoiceProfile) {
			decimal premiumAmount = 0;

			switch (invoiceProfile.InvoiceType ?? 0) {
				case (int)Globals.InvoiceType.NetClaimPayable:
					premiumAmount = txtNetClaimPayable.ValueDecimal;
					break;

				case (int)Globals.InvoiceType.GrossClaimPayable:
					premiumAmount = txtGrossLossPayable.ValueDecimal;
					break;
                    
				default:
					break;
			}

			return premiumAmount;
		}

		public void generateInvoice() {
			int clientID = SessionHelper.getClientId();
			Client client = null;
			int feeInvoiceDesignationID = 0;


			client = ClientManager.GetByID(clientID);
			feeInvoiceDesignationID = Convert.ToInt32(ddlPropertyFeeInvoiceDesignation.SelectedValue);

            switch (client.InvoiceSettingID)
            {
                case 1:
                    // independent adjuster
                    generateAutomaticInvoiceIndependentAdjuster(client);
                    break;

                case 2:
                    //public adjuster 
                    if (feeInvoiceDesignationID == (int)Globals.FeeInvoiceDesignation.LossPercentageFee)
                        generateAutomaticInvoicePublicAdjuster(client);

                    break;

                default:
                    // no auto invoice method selected
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "invoiceAlert", "automaticInvoiceMethodSelectionAlert();", true);
                    break;
            }

		}

		private void generateAutomaticInvoicePublicAdjuster(Client client) {
			Claim claim = null;
			int claimID = 0;
			int clientID = Core.SessionHelper.getClientId();
			int days = 0;
			int invoiceID = 0;
			Invoice invoice = null;
			InvoiceDetail invoiceDetail = null;
			Leads lead = null;
			CRM.Data.Entities.LeadPolicy policy = null;
			int nextInvoiceNumber = 0;
			decimal totalAmount = 0;

			claimID = SessionHelper.getClaimID();

			claim = ClaimsManager.Get(claimID);

			if (claim != null && claim.LeadPolicy != null && claim.LeadPolicy.Leads != null) {
				lead = claim.LeadPolicy.Leads;
				policy = claim.LeadPolicy;

				invoice = new Invoice();
				days = client.InvoicePaymentTerms ?? 0;

				totalAmount = txtNetClaimPayable.ValueDecimal * (client.InvoiceContingencyFee ?? 0);

				// invoice
				invoice.InvoiceDate = DateTime.Now;
				invoice.DueDate = DateTime.Now.AddDays(days);

				invoice.BillToName = lead.insuredName;
				invoice.BillToAddress1 = lead.MailingAddress ?? "";
				invoice.BillToAddress2 = lead.MailingAddress2 ?? "";
				invoice.BillToAddress3 = string.Format("{0}, {1} {2}", lead.MailingCity ?? "", lead.MailingState, lead.MailingZip);

				invoice.ClaimID = claim.ClaimID;
				invoice.IsVoid = false;
				invoice.IsApprove = false;

				invoice.TotalAmount = totalAmount;
                invoice.InvoiceTypeID = claim.InvoiceTypeID;// Convert.ToInt32(ddlInvoiceType.SelectedValue);//NEW OC 11/6/14
				// invoice detail
				invoiceDetail = new InvoiceDetail();
				invoiceDetail.LineDate = DateTime.Now;
				invoiceDetail.LineDescription = "Contingency Fee";
				invoiceDetail.Total = totalAmount;
				invoiceDetail.LineAmount = totalAmount;
				invoiceDetail.isBillable = true;
				invoiceDetail.Qty = txtNetClaimPayable.ValueDecimal;
				invoiceDetail.Rate = client.InvoiceContingencyFee * 100;

				try {
					using (TransactionScope scope = new TransactionScope()) {
						// assign next invoice number to new invoice
						nextInvoiceNumber = InvoiceManager.GetNextInvoiceNumber(client.ClientId);

						invoice.InvoiceNumber = nextInvoiceNumber;

						invoiceID = InvoiceManager.Save(invoice);

						invoiceDetail.InvoiceID = invoiceID;

						InvoiceDetailManager.Save(invoiceDetail);

						// update invoice ready flag 
						claim.IsInvoiceReady = cbxInvoiceReady.Checked;

						claim.IsInvoiced = true;

						ClaimsManager.Save(claim);

						// 2014-05-02 apply rule
						using (SpecificExpenseTypePerCarrier ruleEngine = new SpecificExpenseTypePerCarrier()) {
							RuleException ruleException = ruleEngine.TestRule(clientID, invoice);

							if (ruleException != null) {
								ruleException.UserID = Core.SessionHelper.getUserId();
								ruleEngine.AddException(ruleException);
                                CheckSendMail(ruleException);
							}
						}

						// commit transaction
						scope.Complete();

						lblMessage.Text = "Invoice has been generated successfully.";
						lblMessage.CssClass = "ok";
					}
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);

					lblMessage.Text = "Invoice was not generated.";
					lblMessage.CssClass = "error";

				}
			}
		}

		private void generateAutomaticInvoiceIndependentAdjuster(Client client) {
			Carrier carrier = null;
			CarrierInvoiceProfile invoiceProfile = null;
			string cityName = null;
			Claim claim = null;
			int claimID = 0;
			int clientID = SessionHelper.getClientId();
			decimal commissionAmount = 0;
			int days = 0;
			int invoiceID = 0;
            int quantity = 1;
			Invoice invoice = null;
			Leads lead = null;
			CRM.Data.Entities.LeadPolicy policy = null;
			int nextInvoiceNumber = 0;
			decimal claimAmount = 0;
			decimal rate = 0;
			string stateName = null;

			claimID = SessionHelper.getClaimID();

			claim = ClaimsManager.Get(claimID);

			if (claim != null && claim.LeadPolicy != null && claim.LeadPolicy.Leads != null && claim.LeadPolicy.Carrier != null) {
				lead = claim.LeadPolicy.Leads;
				policy = claim.LeadPolicy;
				carrier = CarrierManager.GetByID((int)policy.CarrierID);

				// load carrier invoice profile assigned to claim
				invoiceProfile = CarrierInvoiceProfileManager.GetProfileForInvoicing(claim.CarrierInvoiceProfileID ?? 0);

				// exit if no profile found
				if (invoiceProfile == null) {
					lblMessage.Text = "Invoice was not generated because no 'Invoice Profile' assigned to claim.";
					lblMessage.CssClass = "error";
					return;
				}

				invoice = new Invoice();
				days = client.InvoicePaymentTerms ?? 0;
			
				// create invoice
				invoice.InvoiceDate = DateTime.Now;
				invoice.DueDate = DateTime.Now.AddDays(days);

				// bill claim carrier
				invoice.BillToName = carrier.CarrierName;
				invoice.BillToAddress1 = carrier.AddressLine1 ?? "";
				invoice.BillToAddress2 = carrier.AddressLine2 ?? "";

				cityName = carrier.CityMaster != null ? carrier.CityMaster.CityName : string.Empty;
				stateName = carrier.StateMaster != null ? carrier.StateMaster.StateName : string.Empty;

				invoice.BillToAddress3 = string.Format("{0}, {1} {2}", cityName, stateName, carrier.ZipCode ?? "");

				invoice.ClaimID = claim.ClaimID;
				invoice.IsVoid = false;
				invoice.IsApprove = false;
                invoice.InvoiceTypeID = claim.InvoiceTypeID;// Convert.ToInt32(ddlInvoiceType.SelectedValue);//new OC 11/6/14
				try {
					using (TransactionScope scope = new TransactionScope()) {
						// assign next invoice number to new invoice
						nextInvoiceNumber = InvoiceManager.GetNextInvoiceNumber(client.ClientId);

						invoice.InvoiceNumber = nextInvoiceNumber;

						// create invoice in db
						invoiceID = InvoiceManager.Save(invoice);

						if (claim.FeeInvoiceDesignation != (int)Globals.FeeInvoiceDesignation.TimeAndExpenseOnly) 
                        {
							claimAmount = getClaimAmount(invoiceProfile);

							calculateCommissionPerCarrierInvoiceProfile(invoiceProfile, out rate, out commissionAmount);

							//insertDetailLine(invoiceID, "Adjuster Service Fee", commissionAmount, claimAmount, rate);
							//insertDetailLine(invoiceID, "Adjuster Service Fee", claimAmount, rate);
                            if (ddlPropertyFeeInvoiceDesignation.SelectedValue == "1" || ddlPropertyFeeInvoiceDesignation.SelectedValue == "2")
                            {
                                claimAmount = 1;
                            }
                            insertDetailLine(invoiceID, "Adjuster Service Fee", claimAmount, rate);
						}
                        
						//processCarrierInvoiceProfileFeeProvisions(invoiceProfile, invoiceID);
						
						processCarrierInvoiceProfileTimeExpense(claim.ClaimID, invoiceProfile, invoiceID, claimAmount);

						processFirmDiscount(invoiceProfile, invoiceID);

						// update invoice total
						invoice.TotalAmount = InvoiceManager.calculateInvoiceTotal(invoiceID);

						InvoiceManager.Save(invoice);
                        

						// update claim - invoice ready flag 
						claim.IsInvoiceReady = true;

						claim.IsInvoiced = true;

						ClaimsManager.Save(claim);

						// 2014-05-02 apply rule
						using (SpecificExpenseTypePerCarrier ruleEngine = new SpecificExpenseTypePerCarrier()) {
							RuleException ruleException = ruleEngine.TestRule(clientID, invoice);

							if (ruleException != null) {
								ruleException.UserID = Core.SessionHelper.getUserId();
								ruleEngine.AddException(ruleException);
                                CheckSendMail(ruleException);
							}
						}

						// complete transaction
						scope.Complete();

						lblMessage.Text = "Invoice has been generated successfully.";
						lblMessage.CssClass = "ok";
					}
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);

					lblMessage.Text = "Invoice was not generated.";
					lblMessage.CssClass = "error";

				}
			}
		}

		private void processClaimServices(int claimID, CarrierInvoiceProfile invoiceProfile, int invoiceID) {
			CarrierInvoiceProfileFeeItemized profileTEFee = null;
			List<ClaimService> claimServices = null;
			decimal lineTotal = 0;
			decimal quantity = 0;
			decimal rateAmount = 0;
			string serviceDescription = null;
			string serviceComments = null;
            string activity = null;
            string activityDescription = null;
            bool isBillable = true;
            bool billed = true;
			InvoiceServiceType invoiceServiceType = null;

			// get TE services for claim entered by adjuster
			using (ClaimServiceManager repositiory = new ClaimServiceManager()) 
            {
                claimServices = repositiory.GetAllForInvoice(claimID);
			}

			if (claimServices != null && claimServices.Count > 0) 
            {
                
				foreach (ClaimService claimService in claimServices) 
                {
                    
					profileTEFee = (from x in invoiceProfile.CarrierInvoiceProfileFeeItemized
								 where x.ServiceTypeID == claimService.ServiceTypeID
								 select x
								 ).FirstOrDefault();
                    int claimServiceID = claimService.ClaimServiceID;
					quantity = (claimService.ServiceQty ?? 0);
					serviceDescription = claimService.InvoiceServiceType == null ? string.Empty : claimService.InvoiceServiceType.ServiceDescription;
					invoiceServiceType = claimService.InvoiceServiceType;
                    activity = claimService.Activity == null ? string.Empty : claimService.Activity;
                    activityDescription = claimService.ServiceDescription;
                    isBillable = claimService.IsBillable.Value;
                    billed = claimService.Billed.Value;
					if (profileTEFee != null) 
                    {
						// use override from invoice profile											
						if (profileTEFee.ItemRate > 0) 
                        {
							serviceComments = profileTEFee.ItemDescription;
							rateAmount = profileTEFee.ItemRate;

							lineTotal = rateAmount * quantity;
                            if (isBillable == true)
                            {
                                insertDetailLine(invoiceID, serviceDescription, activity, activityDescription, lineTotal, quantity, rateAmount, claimService.ServiceDate, serviceComments);
                                using (ClaimServiceManager myRepository = new ClaimServiceManager())
                                {
                                    ClaimService myClaimService = myRepository.Get(claimServiceID);
                                    myClaimService.Billed = true;
                                    myRepository.Save(myClaimService);
                                }
                                
                            }
						}
						else if (profileTEFee.ItemPercentage > 0) 
                        {
						}
					}
					else 
                    {
						if ((invoiceServiceType.ServiceRate ?? 0) > 0) 
                        {
							rateAmount = invoiceServiceType.ServiceRate ?? 0;
							quantity = claimService.ServiceQty ?? 0;
							lineTotal = rateAmount * quantity;
						}
						else if ((invoiceServiceType.ServicePercentage ?? 0) > 0) 
                        {						
						}
                        if (isBillable == true )
                        {
                            insertDetailLine(invoiceID, serviceDescription, activity, activityDescription, lineTotal, quantity, rateAmount, claimService.ServiceDate, serviceComments);
                            using (ClaimServiceManager myRepository = new ClaimServiceManager())
                            {
                                ClaimService myClaimService = myRepository.Get(claimServiceID);
                                myClaimService.Billed = true;
                                myRepository.Save(myClaimService);
                            }
                        }
                       
					}
				}
			}
		}

		private void processClaimExpenses(int claimID, CarrierInvoiceProfile invoiceProfile, int invoiceID) {
			CarrierInvoiceProfileFeeItemized profileTEFee = null;
			List<ClaimExpense> claimExpenses = null;
			decimal lineTotal = 0;
			decimal quantity = 0;
			decimal operand = 0;
			decimal expenseAmount = 0;
			decimal rateAmount = 0;
			string serviceDescription = null;
			string serviceComments = null;
            bool billed = true;
			// get TE services for claim entered by adjuster
			using (ClaimExpenseManager repositiory = new ClaimExpenseManager()) 
            {
				claimExpenses = repositiory.GetExpenseForInvoice(claimID);
			}

			if (claimExpenses != null && claimExpenses.Count > 0) 
            {				
				foreach (ClaimExpense claimExpense in claimExpenses) 
                {
					rateAmount = 0;
                    int claimExpenseID = claimExpense.ClaimExpenseID;
					profileTEFee = (from x in invoiceProfile.CarrierInvoiceProfileFeeItemized
								 where x.ExpenseTypeID == claimExpense.ExpenseTypeID
								 select x
								 ).FirstOrDefault();

				
					if (profileTEFee != null) 
                    {
						// use override from invoice profile
                        serviceDescription = claimExpense.ExpenseType.ExpenseName;// profileTEFee.ExpenseType == null ? string.Empty : profileTEFee.ExpenseType.ExpenseDescription;
                        serviceComments = claimExpense.ExpenseDescription; // profileTEFee.ItemDescription;
						operand = profileTEFee.LogicalOperatorOperand ?? 0;
                        billed = claimExpense.Billed.Value;
                        rateAmount = profileTEFee.ItemRate; //claimExpense.ExpenseAmount;
                        quantity = Convert.ToDecimal(claimExpense.ExpenseQty);
                        Session["ExpAmount"] = claimExpense.ExpenseAmount;
						if ((profileTEFee.LogicalOperator ?? 0) > 0 && operand > 0) 
                        {
							quantity = claimExpense.ExpenseQty ?? 0;
							expenseAmount = claimExpense.ExpenseAmount;

							switch (profileTEFee.LogicalOperator) 
                            {
								case 1:		// =
									//if (expenseAmount > 0 && expenseAmount == operand) {
									//	rateAmount = profileTEFee.ItemRate;
									//	expenseAmount = quantity * rateAmount;										
									//}
									//else if (quantity > 0 && quantity == operand) {
									//	rateAmount = profileTEFee.ItemRate;
									//	expenseAmount = quantity * rateAmount;
									//}
									break;

								case 2:		// <									
								case 3:		// <=
									if (expenseAmount > 0 && expenseAmount <= operand) 
                                    {
										quantity = 1;
										rateAmount = profileTEFee.ItemRate;										
									}
									else if (quantity > 0 && quantity <= operand) {
                                        rateAmount = profileTEFee.ItemRate; //; 
									}
									break;

								case 4:		// >									
								case 5:		// >=
									if (expenseAmount > 0 && expenseAmount >= operand) 
                                    {
										quantity = 1;
										rateAmount = operand;
									}
									else if (quantity > 0 && quantity >= operand) 
                                    {
										quantity = quantity - operand;
										rateAmount = profileTEFee.ItemRate; 
									}
									break;

								default:
									quantity = 1;
									rateAmount = profileTEFee.ItemRate; //claimExpense.ExpenseAmount;
									break;
							}
                            if (billed == false)
                            {
                                insertDetailLine(invoiceID, serviceDescription, quantity, rateAmount, claimExpense.ExpenseDate, serviceComments);
                                using (ClaimExpenseManager myRepositiory = new ClaimExpenseManager())
                                {
                                    ClaimExpense myClaimExpense = myRepositiory.Get(claimExpenseID);
                                    myClaimExpense.Billed = true;
                                    myRepositiory.Save(myClaimExpense);
                                }
                            }
                            
						}
						else if (profileTEFee.ItemRate > 0) 
                        {
							// no condition specified
							quantity = claimExpense.ExpenseQty ?? 1;

							rateAmount = profileTEFee.ItemRate;

							lineTotal = expenseAmount;
                            if (billed == false)
                            {
                                insertDetailLine(invoiceID, serviceDescription, quantity, rateAmount, claimExpense.ExpenseDate, serviceComments);
                                using (ClaimExpenseManager myRepositiory = new ClaimExpenseManager())
                                {
                                    ClaimExpense myClaimExpense = myRepositiory.Get(claimExpenseID);
                                    myClaimExpense.Billed = true;
                                    myRepositiory.Save(myClaimExpense);
                                }
                            }
                            
						}
						else if (profileTEFee.ItemPercentage > 0) 
                        {
						}
					}
					else 
                    {
						// no override found
						serviceDescription = claimExpense.ExpenseType.ExpenseDescription;
						
						serviceComments = claimExpense.ExpenseDescription;
						
						quantity = claimExpense.ExpenseQty ?? 1;

						rateAmount = claimExpense.ExpenseAmount;
                        if (billed == false)
                        {
                            insertDetailLine(invoiceID, serviceDescription, quantity, rateAmount,  claimExpense.ExpenseDate, serviceComments);
                            using (ClaimExpenseManager myRepositiory = new ClaimExpenseManager())
                            {
                                ClaimExpense myClaimExpense = myRepositiory.Get(claimExpenseID);
                                myClaimExpense.Billed = true;
                                myRepositiory.Save(myClaimExpense);
                            }
                        }
                        
					}
				}
			}
		}


		private void insertDetailLine(int invoiceID, string serviceDescription, decimal qty, decimal rate, DateTime? date = null, string comments = null) {
			InvoiceDetail invoiceDetail = null;

			// invoice detail
			invoiceDetail = new InvoiceDetail();
			invoiceDetail.InvoiceID = invoiceID;

			invoiceDetail.LineDate = date == null ? DateTime.Now : date;

			invoiceDetail.LineDescription = serviceDescription;
			invoiceDetail.Comments = comments;
            invoiceDetail.LineAmount = Convert.ToDecimal( Session["ExpAmount"]); // * qty
			invoiceDetail.Total = invoiceDetail.LineAmount;
			invoiceDetail.isBillable = true;
			invoiceDetail.Qty = qty;
			invoiceDetail.Rate = rate;

			InvoiceDetailManager.Save(invoiceDetail);
		}
		private void insertDetailLine(int invoiceID, string serviceDescription, string activity, string activityDescription, decimal amount, decimal qty, decimal rate, DateTime? date = null, string comments = null) {
			InvoiceDetail invoiceDetail = null;

			// invoice detail
			invoiceDetail = new InvoiceDetail();
			invoiceDetail.InvoiceID = invoiceID;

			invoiceDetail.LineDate = date == null ? DateTime.Now : date;

			invoiceDetail.LineDescription = serviceDescription;
            invoiceDetail.Activity = activity;
			invoiceDetail.Comments = comments;
			invoiceDetail.Total = amount;
			invoiceDetail.LineAmount = amount;
			invoiceDetail.isBillable = true;
			invoiceDetail.Qty = qty;
			invoiceDetail.Rate = rate;
            invoiceDetail.ActivityDescription = activityDescription; 
			InvoiceDetailManager.Save(invoiceDetail);
		}

		private void processCarrierInvoiceProfileFeeProvisions(CarrierInvoiceProfile invoiceProfile, int invoiceID) {
			if (invoiceProfile != null && invoiceProfile.CarrierInvoiceProfileFeeProvision != null && invoiceProfile.CarrierInvoiceProfileFeeProvision.Count > 0) {
				foreach (CarrierInvoiceProfileFeeProvision feePrivision in invoiceProfile.CarrierInvoiceProfileFeeProvision) {
					//insertDetailLine(invoiceID, feePrivision.ProvisionText, feePrivision.ProvisionAmount, 1, feePrivision.ProvisionAmount);
				}
			}
		}

		private void processCarrierInvoiceProfileTimeExpense(int claimID, CarrierInvoiceProfile invoiceProfile, int invoiceID, decimal premiumAmount) {						
			if (invoiceProfile != null && invoiceProfile.CarrierInvoiceProfileFeeItemized != null && invoiceProfile.CarrierInvoiceProfileFeeItemized.Count > 0) {

				processClaimServices(claimID, invoiceProfile, invoiceID);

				processClaimExpenses(claimID, invoiceProfile, invoiceID);

				//foreach (CarrierInvoiceProfileFeeItemized itemizedFee in invoiceProfile.CarrierInvoiceProfileFeeItemizeds) {

				//	if (itemizedFee.ItemRate > 0) {
				//		feeAmount = itemizedFee.ItemRate;
				//		insertDetailLine(invoiceID, itemizedFee.ItemDescription, feeAmount, 1, itemizedFee.ItemRate);
				//	}
				//	else if (itemizedFee.ItemPercentage > 0) {
				//		feeAmount = premiumAmount * itemizedFee.ItemRate;
				//		insertDetailLine(invoiceID, itemizedFee.ItemDescription, feeAmount, premiumAmount, feeAmount);
				//	}
				//}
			}
		}

		private void processFirmDiscount(CarrierInvoiceProfile invoiceProfile, int invoiceID) {
			decimal discountRate = 0;
			decimal discountAmount = 0;
			decimal invoiceTotalAmount = 0;
			string serviceDescription = null;
			decimal percentage = 0;
            string activity = null;
            string activityDescription = null;

			discountRate = invoiceProfile.FirmDiscountPercentage ?? 0;

			if (discountRate > 0) {

				invoiceTotalAmount = InvoiceManager.calculateInvoiceTotal(invoiceID);

				discountAmount = (invoiceTotalAmount * discountRate) * -1;
				percentage = discountRate * 100;
				serviceDescription = string.Format("Firm Discount Percentage", percentage);

				insertDetailLine(invoiceID, serviceDescription, activity,activityDescription, discountAmount, 1, percentage);
			}
		}

		public void saveForm() {
			int adjusterID = 0;
			int? adjusterSupervisorID = 0;
			int companyBuilderID = 0;
			int companyInventoryID = 0;
			int contentsAdjusterID = 0;
			int examinerID = 0;
            int estimatorID = 0;
            int deskAdjusterID = 0;
			int inventoryCompanyID = 0;
			bool isNewClaim = false;
			bool isAdjusterChanged = false;
			int masterStatusID = 0;
			int nextClaimNumber = 0;
			int outsideAdjusterID = 0;
			int ourBuilderID = 0;
			int previousProgressStatusID = 0;
			int priorAdjustID = 0;
			string taskDescription = null;
            Session["InvoiceProfileID"] = ddlCarrierInvoiceProfile.SelectedValue;
            var a = Session["InvoiceProfileID"];
            Session["CarrierType"] = ddlCarrierInvoiceProfile.SelectedItem.Text;
            //Page.Validate("claim");
            //if (!Page.IsValid)
            //    return;

			clientID = SessionHelper.getClientId();
			leadID = SessionHelper.getLeadId();

			Claim claim = null;
			Client client = null;

			// clear any message
			lblMessage.Text = string.Empty;
			lblMessage.CssClass = string.Empty;
            
			//if (int.TryParse(hf_policyID.Value, out id) && id > 0) {
			if (this.claimID > 0) {
				claim = ClaimsManager.GetByID(this.claimID);
			}
			else {
				// new claim
				claim = new Claim();
				claim.PolicyID = policyID;

				claim.IsActive = true;
				claim.IsInvoiced = false;
				claim.IsInvoiceReady = false;
				isNewClaim = true;
			}
            //save insurer/branch OC 9/9/14
            claim.ManagerEntityID = Convert.ToInt32(ddlOwnerManagerEntityName.SelectedValue);
            Session["InsurerBranch"] = ddlOwnerManagerEntityName.SelectedItem.Text;
            claim.LossDescription = txtLossDescription.Text == null ? "" : txtLossDescription.Text; // new 9/16/14 OC

			// 2014-05-13 tortega
			previousProgressStatusID = claim.ProgressStatusID ?? 0;

			claim.InsurerClaimNumber = txtInsurerClaimNumber.Text.Trim();
			claim.AdjusterClaimNumber = txtClaimNumber.Text.Trim();
            //claim.Overhead = txtOverHeadProfit.Text;
            claim.InvoiceTypeID = Convert.ToInt32(ddlInvoiceType.SelectedValue);
			// save previous assigned adjuster
			priorAdjustID = claim.AdjusterID ?? 0;

			adjusterID = Convert.ToInt32(hf_adjusterID.Value);
			if (adjusterID > 0) {
				claim.AdjusterID = adjusterID;

				// flag to register any change in adjuster assignment
				isAdjusterChanged = !claim.AdjusterID.Equals(priorAdjustID);
			}
			else {
				// clear prior selection
				claim.AdjusterID = null;
			}

			#region other Adjusters
			outsideAdjusterID = Convert.ToInt32(ddlOutsideAdjuster.SelectedValue);
			contentsAdjusterID = Convert.ToInt32(this.ddlContentsAdjuster.SelectedValue);
			examinerID = Convert.ToInt32(this.ddlExaminer.SelectedValue);
			companyBuilderID = Convert.ToInt32(this.ddlCompanyBuilder.SelectedValue);
			companyInventoryID = Convert.ToInt32(this.ddlCompanyInventory.SelectedValue);
			ourBuilderID = Convert.ToInt32(this.ddlOurBuilder.SelectedValue);
			inventoryCompanyID = Convert.ToInt32(this.ddlInventoryCompany.SelectedValue);
            estimatorID = Convert.ToInt32(ddlEstimator.SelectedValue);
            deskAdjusterID = Convert.ToInt32(ddlDeskAdjuster.SelectedValue);

			claim.OutsideAdjusterID = outsideAdjusterID > 0 ? (int?)outsideAdjusterID : null;
			claim.ContentAdjusterID = contentsAdjusterID > 0 ? (int?)contentsAdjusterID : null;
			claim.ExaminerID = examinerID > 0 ? (int?)examinerID : null;
			claim.CompanyBuilderID = companyBuilderID > 0 ? (int?)companyBuilderID : null;
			claim.CompanyInventoryID = companyInventoryID > 0 ? (int?)companyInventoryID : null;
			claim.OurBuilderID = ourBuilderID > 0 ? (int?)ourBuilderID : null;
			claim.InventoryCompanyID = inventoryCompanyID > 0 ? (int?)inventoryCompanyID : null;
            claim.EstimatorID = estimatorID > 0 ? (int?)estimatorID : null;
            claim.DeskAdjusterID = deskAdjusterID > 0 ? (int?)deskAdjusterID : null;
			#endregion

			// manager
			if (ddlManager.SelectedIndex > 0)
				claim.ManagerID = Convert.ToInt32(ddlManager.SelectedValue);
			else
				claim.ManagerID = null;

			// supervisor
			if (ddlSupervisors.SelectedIndex > 0)
				claim.SupervisorID = Convert.ToInt32(ddlSupervisors.SelectedValue);
			else
				claim.SupervisorID = null;

			// team leader
			if (ddlTeamLeader.SelectedIndex > 0)
				claim.TeamLeadID = Convert.ToInt32(ddlTeamLeader.SelectedValue);
			else
				claim.TeamLeadID = null;

			if (txtSeverity.Value != null)
				claim.SeverityNumber = Convert.ToInt32(txtSeverity.Value);
			else
				claim.SeverityNumber = null;

			claim.EventName = txtEventName.Text;
			claim.EventType = txtEventType.Text;
            //Chetu code
            claim.CatId = txtCat.Text;
            claim.TypeofLoss = ddlTypeOfLoss.SelectedValue;
           
                claim.AdjCommOverride = txtAdjCommOveride.Value == null ? 0 : (Convert.ToDecimal(txtAdjCommOveride.Value)*100);
               // Convert.ToDecimal(txtAdjCommOveride.Value);           

                claim.AdjCommFlatFeeOverride = txtAdjCommFlatFeeOveride.Value == null ? 0 : Convert.ToDecimal(txtAdjCommFlatFeeOveride.Value);
                   // Convert.ToDecimal(txtAdjCommFlatFeeOveride.Text);
           
            /////

			if (!string.IsNullOrEmpty(txtCycleTime.Text))
				claim.CycleTime = Convert.ToDecimal(txtCycleTime.Value);

			if (!string.IsNullOrEmpty(txtReopenCycleTime.Text))
				claim.ReopenCycleTime = Convert.ToDecimal(txtReopenCycleTime.Value);

			claim.ClaimWorkflowType = txtClaimWorkflowType.Text;

			claim.CauseOfLoss = getCauseOfLoss();

			// get fields from middle columns			
			masterStatusID = Convert.ToInt32(this.ddlLeadStatus.SelectedValue);

			if (masterStatusID > 0) {
				if ((claim.StatusID ?? 0) != masterStatusID)
					claim.LastStatusUpdate = DateTime.Now;

				claim.StatusID = masterStatusID;
			}
			else
				claim.StatusID = null;

			if (Convert.ToInt32(this.ddlSubStatus.SelectedValue) > 0)
				claim.SubStatusID = Convert.ToInt32(ddlSubStatus.SelectedValue);
			else
				claim.SubStatusID = null;

			#region dates
			if (!String.IsNullOrEmpty(txtLossDate.Text))
				claim.LossDate = txtLossDate.Date;

			if (!string.IsNullOrEmpty(this.txtDateOpened.Text))
				claim.DateOpenedReported = txtDateOpened.Date;
			else
				claim.DateOpenedReported = null;
            //new OC date entered 11/10 
            if (!string.IsNullOrEmpty(this.txtDateEntered.Text))
                claim.DateEntered = txtDateEntered.Date;
            else
                claim.DateEntered = null;
            //new OC date proj completed 11/10 
            if (!string.IsNullOrEmpty(this.txtDateProjCompleted.Text))
                claim.DateProjCompleted = txtDateProjCompleted.Date;
            else
                claim.DateProjCompleted = null;


			if (!string.IsNullOrEmpty(this.txtDateInitialReservedChanged.Text))
				claim.DateInitialReserveChange = txtDateInitialReservedChanged.Date;
			else
				claim.DateInitialReserveChange = null;

			if (!string.IsNullOrEmpty(this.txtDateAssigned.Text))
				claim.DateAssigned = txtDateAssigned.Date;
			else
				claim.DateAssigned = null;

			if (!string.IsNullOrEmpty(txtDateAcknowledge.Text))
				claim.DateAcknowledged = txtDateAcknowledge.Date;
			else
				claim.DateAcknowledged = null;

			if (!string.IsNullOrEmpty(this.txtDateFirstContactAttempted.Text))
				claim.DateFirstContactAttempt = txtDateFirstContactAttempted.Date;
			else
				claim.DateFirstContactAttempt = null;

			if (!string.IsNullOrEmpty(this.txtDateContacted.Text))
				claim.DateContacted = txtDateContacted.Date;
			else
				claim.DateContacted = null;

			if (!string.IsNullOrEmpty(this.txtDateAssigned.Text))
				claim.DateAssigned = txtDateAssigned.Date;
			else
				claim.DateAssigned = null;

			if (!string.IsNullOrEmpty(this.txtDateInspectionSchedule.Text))
				claim.DateInspectionScheduled = txtDateInspectionSchedule.Date;
			else
				claim.DateInspectionScheduled = null;

			if (!string.IsNullOrEmpty(this.txtDateInspectionCompleted.Text))
				claim.DateInspectionCompleted = txtDateInspectionCompleted.Date;
			else
				claim.DateInspectionCompleted = null;

			if (!string.IsNullOrEmpty(this.txtDateSubmitted.Text))
				claim.DateSubmitted = txtDateSubmitted.Date;
			else
				claim.DateSubmitted = null;

			if (!string.IsNullOrEmpty(this.txtDateIndemnityRequetsted.Text))
				claim.DateIndemnityPaymentRequested = txtDateIndemnityRequetsted.Date;
			else
				claim.DateIndemnityPaymentRequested = null;

			if (!string.IsNullOrEmpty(this.txtDateIndemnityPaymentApproved.Text))
				claim.DateIndemnityPaymentApproved = txtDateIndemnityPaymentApproved.Date;
			else
				claim.DateIndemnityPaymentApproved = null;

			if (!string.IsNullOrEmpty(this.txtDateIndemnityPaymentIssued.Text))
				claim.DateIndemnityPaymentIssued = txtDateIndemnityPaymentIssued.Date;
			else
				claim.DateIndemnityPaymentIssued = null;

			if (!string.IsNullOrEmpty(this.txtDateClosed.Text))
				claim.DateClosed = txtDateClosed.Date;
			else
				claim.DateClosed = null;

			if (!string.IsNullOrEmpty(this.txtDateFirstReopen.Text))
				claim.DateFirstReOpened = txtDateFirstReopen.Date;
			else
				claim.DateFirstReOpened = null;

			if (!string.IsNullOrEmpty(this.txtDateReopenCompleted.Text))
				claim.DateReopenCompleted = txtDateReopenCompleted.Date;
			else
				claim.DateReopenCompleted = null;

			if (!string.IsNullOrEmpty(this.txtDateFinalClosed.Text))
				claim.DateFinalClosed = txtDateFinalClosed.Date;
			else
				claim.DateFinalClosed = null;

			txtReopenCycleTime.Text = (claim.ReopenCycleTime ?? 0).ToString("N2");
			txtCycleTime.Text = (claim.CycleTime ?? 0).ToString("N2");
			#endregion

			if (ddlCarrierInvoiceProfile.SelectedIndex > 0)
				claim.CarrierInvoiceProfileID = Convert.ToInt32(ddlCarrierInvoiceProfile.SelectedValue);

			#region property loss totals
			claim.FeeInvoiceDesignation = Convert.ToInt32(ddlPropertyFeeInvoiceDesignation.SelectedValue);
			claim.GrossLossPayable = txtGrossLossPayable.Value == null ? 0 : Convert.ToDecimal(txtGrossLossPayable.Value);
			claim.Depreciation = txtDepreciation.Value == null ? 0 : Convert.ToDecimal(txtDepreciation.Value);
            //chetu code
            claim.NonRecoverableDepreciation = txtNonDepreciation.Value == null ? 0 : Convert.ToDecimal(txtNonDepreciation.Value);
            //
			claim.Deductible = txtPolicyDeductible.Value == null ? 0 : Convert.ToDecimal(txtPolicyDeductible.Value);

			// net claim payble
			claim.NetClaimPayable = txtNetClaimPayable.Value == null ? 0 : Convert.ToDecimal(txtNetClaimPayable.Value);
            Session["ClaimPayable"] = txtNetClaimPayable.Text;
			#endregion

			// invoice ready flag
			claim.IsInvoiceReady = cbxInvoiceReady.Checked;

			#region casualty loss totals
			claim.CasualtyFeeInvoiceDesignation = Convert.ToInt32(ddlPropertyFeeInvoiceDesignation.SelectedValue);
			claim.CasualtyGrossClaimPayable = txtCasualtyGrossClaimPayable.Value == null ? 0 : Convert.ToDecimal(txtCasualtyGrossClaimPayable.Value);
			#endregion

			#region Carrier Finance Details(Optional)
			claim.OutstandingIndemnityReserve = this.txtOutstandingIndemnityReserve.Value == null ? 0 : Convert.ToDecimal(txtOutstandingIndemnityReserve.Value);
			claim.OutstandingLAEReserves = this.txtOutstandingLAEReserves.Value == null ? 0 : Convert.ToDecimal(txtOutstandingLAEReserves.Value);
			claim.TotalIndemnityPaid = this.txtTotalIndemnityPaid.Value == null ? 0 : Convert.ToDecimal(txtTotalIndemnityPaid.Value);
			claim.CoverageAPaid = this.txtCoverageAPaid.Value == null ? 0 : Convert.ToDecimal(txtCoverageAPaid.Value);
			claim.CoverageBPaid = this.txtCoverageBPaid.Value == null ? 0 : Convert.ToDecimal(txtCoverageBPaid.Value);
			claim.CoverageCPaid = this.txtCoverageCPaid.Value == null ? 0 : Convert.ToDecimal(txtCoverageCPaid.Value);
			claim.CoverageDPaid = this.txtCoverageDPaid.Value == null ? 0 : Convert.ToDecimal(txtCoverageDPaid.Value);
			claim.TotalExpensesPaid = this.txtTotalExpensePaid.Value == null ? 0 : Convert.ToDecimal(txtTotalExpensePaid.Value);
			#endregion

			// 2014-04-27 tortega
			if (ddlProgressStatus.SelectedIndex > 0) {
				claim.ProgressStatusID = Convert.ToInt32(ddlProgressStatus.SelectedValue);
				
				// 2014-05-13 tortega
				if (claim.ProgressStatusID != previousProgressStatusID)
					claim.LastProgressChanged = DateTime.Now;
			}

			try {
				using (TransactionScope scope = new TransactionScope()) {
					if (isNewClaim && string.IsNullOrEmpty(claim.AdjusterClaimNumber)) {
						client = ClientManager.Get(clientID);

						if (client != null) {
							nextClaimNumber = client.NextClaimNumber ?? 999;

							// increase claim counter
							++nextClaimNumber;

							// assign new claim number
							client.NextClaimNumber = nextClaimNumber;

							ClientManager.Save(client);

							claim.AdjusterClaimNumber = string.Format("{0}-{1}", client.ClientId, nextClaimNumber);

							// show new claim number in UI
							txtClaimNumber.Text = claim.AdjusterClaimNumber;
						}
					}
                    //save new fields
                    if (ddlJobSideCode.SelectedValue != "-1")
                    {
                        claim.JobSizeCode = ddlJobSideCode.SelectedValue;
                    }
                    if (ddlEstimateCount.SelectedValue != "-1")
                    {
                        claim.EstimateCount = ddlEstimateCount.SelectedValue;
                    }
                    if (ddlMitigation.SelectedValue != "-1")
                    {
                        claim.Mitigation = ddlMitigation.SelectedValue;
                    }

                    claim.ProfileCode = txtProfileCode.Text;
                    //new fields
                    claim.Overhead = txtOverHead.Text;
                    claim.profit = txtProfit.Text;
                    claim.CumulativeOp = txtCumulative.Text;
                    claim.DefaultRepairedBy = txtDefaultRepairedBY.Text;
                    claim.DepOnMaterials = chkDepriationMat.Checked;
                    claim.DepOnNonMaterials = chkDepriationNonMat.Checked;
                    claim.DepOnTaxes = chkDepriationTaxes.Checked;
                    if (!string.IsNullOrEmpty(txtMaxDepriation.Text))
                    {
                        claim.MaxAllowedDep = Convert.ToDecimal(txtMaxDepriation.Text);
                    }
                    else
                    {
                        claim.MaxAllowedDep = 0;
                    }
                    claim.taxJurisdiction = txttaxJurisdiction.Text;
                    
					// save claim id
					claim = ClaimsManager.Save(claim);

					this.claimID = claim.ClaimID;

                    //code for save loss of amount and loss of reserve
                    //Data.LeadPolicy lp = new Data.LeadPolicy();
                    //int leadid = claim.LeadPolicy.LeadId??0;
                    
                    //Leads objlead = new Leads();
                    //objlead.LeadId = leadid;
                    //if (!string.IsNullOrEmpty(txtLossAmount.Text))
                    //{
                    //    objlead.LossOfUseAmount = Convert.ToDecimal(txtLossAmount.Text);
                    //}
                    //if (!string.IsNullOrEmpty(txtLossReserve.Text))
                    //{
                    //    objlead.LossOfUseReserve = Convert.ToDecimal(txtLossReserve.Text);
                    //}
                    //LeadsManager.UpdateLossAmount(objlead);




                    propertyLimits.saveLimits(this.policyID);

					//casualtyLimits.saveLimits(this.claimID);

					propertySubLimits.saveLimits(this.claimID);

					// 2014-05-02 apply rule
					if (isAdjusterChanged) {
						using (AdjusterClaimReview ruleEngine = new AdjusterClaimReview()) {
							RuleException ruleException = ruleEngine.TestRule(clientID, claim);

							if (ruleException != null) {
								// add exception
								ruleException.UserID = Core.SessionHelper.getUserId();

								ruleEngine.AddException(ruleException);
                               // CheckSendMail(ruleException);
								// notify supervisor by adding task
								adjusterSupervisorID = AdjusterManager.GetAdjusterSupervisor((int)claim.AdjusterID);

								if (adjusterSupervisorID != null) {
									taskDescription = string.Format("Review Claim #{0}", claim.AdjusterClaimNumber);

									this.leadID = SessionHelper.getLeadId();

									createTaskForAdjusterSupervisor((int)adjusterSupervisorID, this.leadID, "Adjuster Claim Review", taskDescription);
								}
							}
						}
					}

					// complete transaction
					scope.Complete();
				}

				setReminder(masterStatusID);

				activateTabs(true);

				// refresh claim limits when new claim is added
                propertyLimits.bindData(this.policyID);

				//casualtyLimits.bindData(this.claimID);

				propertySubLimits.bindData(this.claimID);


                if (isAdjusterChanged)
                {
                    notifyAdjuster(this.claimID);
                }
                if (isNewClaim)
                {
                    notifyNewClaimCreated(claimID);
                }
                

                //Console.WriteLine("Success ");
				lblMessage.Text = "Claim Information saved successfully.";
				lblMessage.CssClass = "ok";
			}
			catch (Exception ex) {
                //Console.WriteLine("The error: " + ex);
                System.Diagnostics.Debug.WriteLine("test -" + ex);
                lblMessage.Text = "Claim Information was not saved"; //"Claim Information was not saved.";
				lblMessage.CssClass = "error";

				//Core.EmailHelper.emailError(ex);
			}


		}

       

		protected void CustomValidator_chkLossType(object source, ServerValidateEventArgs args) {
			args.IsValid = chkLossType.Items.Cast<ListItem>().Any(x => x.Selected);
		}

		protected void createTaskForAdjusterSupervisor(int adjusterSupervisorID, int leadID, string ruleName, string taskDescription) {
			Task task = null;

			task = new Task();
			task.creator_id = this.clientID;
			task.TaskType = (int)Globals.TaskType.Task;

			task.text = ruleName;
			task.details = taskDescription;
	
			task.start_date = DateTime.Now;
			task.end_date = DateTime.Now;

			task.status_id = (int)Globals.Task_Status.Active;
			task.owner_id = adjusterSupervisorID;
			task.PriorityID = (int)Globals.Task_Priority.High;

			task.lead_id = leadID;

			TasksManager.Save(task);
        }

        protected void fillClaimStatusReview(int clientd)
        {
            ClaimManager objClaimManager = new ClaimManager();
            List<Carrier> objCarrier = new List<Carrier>();
            objCarrier = objClaimManager.GetAllCarrier(clientd);
            if (objCarrier != null)
            {
                CollectionManager.FillCollection(ddlClaimCarrier, "CarrierID", "CarrierName", objCarrier);
            }

            List<ContactList> objContactlist = new List<ContactList>() ;

             List<Contact> listContact= ContactManager.GetAll(clientd).ToList();

             ContactList objcontact1;
             foreach (Contact data in listContact)
             {
                 objcontact1 = new ContactList();
                 objcontact1.ContactID = data.ContactID;
                 objcontact1.FirstName = data.FirstName;
                 objcontact1.LastName = data.LastName;
                 objcontact1.Email = data.Email;
                 objcontact1.CompanyName = data.CompanyName;
                 objcontact1.IdOf = "c";
                 objContactlist.Add(objcontact1);
             }
             List<AdjusterMaster> listAdjuster = CRM.Data.Account.AdjusterManager.GetAll(clientd).ToList();          
            foreach (AdjusterMaster data in listAdjuster)
            {
                objcontact1 = new ContactList();
                objcontact1.ContactID = data.AdjusterId;
                objcontact1.FirstName = data.FirstName;
                objcontact1.LastName = data.LastName;
                objcontact1.Email = data.email;
                objcontact1.CompanyName = data.CompanyName;
                objcontact1.IdOf = "a";
                objContactlist.Add(objcontact1);
            }
            gvSelectRecipients.DataSource = objContactlist.AsQueryable();
           gvSelectRecipients.DataBind();
          }


        #region send reg flag mail

        public static void CheckSendMail(RuleException ruleExp)
        {
            if (ruleExp != null)
            {
                string adjusterEmail = string.Empty;
                string supervisorEmail = string.Empty;
                bool sendAdjuster = false;
                bool sendSupervisor = false;
                string recipient = string.Empty;
                int claimId = 0;

                BusinessRuleManager objRuleManager = new BusinessRuleManager();
                BusinessRule objRule = new BusinessRule();
                CRM.Data.Entities.Claim objClaim = new CRM.Data.Entities.Claim();
                CRM.Data.Entities.SecUser objSecUser = new Data.Entities.SecUser();
                AdjusterMaster adjustermaster = new AdjusterMaster();

                int businessRuleID = ruleExp.BusinessRuleID ?? 0;
                objRule = objRuleManager.GetBusinessRule(businessRuleID);
                if (objRule != null)
                {
                    claimId = ruleExp.ObjectID ?? 0;

                    objClaim = objRuleManager.GetClaim(claimId);
                    adjustermaster = objRuleManager.GetAdjuster(objClaim.AdjusterID ?? 0);
                    objSecUser = objRuleManager.GetSupervisor(objClaim.SupervisorID ?? 0);
                    if (objSecUser != null)
                    {
                        adjusterEmail = adjustermaster.email;
                        supervisorEmail = objSecUser.Email;

                        sendAdjuster = objRule.EmailAdjuster;
                        sendSupervisor = objRule.EmailSupervisor;

                        if (sendAdjuster == true && sendSupervisor == true)
                        {
                            recipient = adjusterEmail + " " + supervisorEmail;
                            notifyUser(objRule.Description, claimId, recipient);
                        }
                        else if (sendAdjuster == false && sendSupervisor == true)
                        {
                            recipient = supervisorEmail;
                            notifyUser(objRule.Description, claimId, recipient);
                        }
                        else if (sendAdjuster == true && sendSupervisor == false)
                        {

                            recipient = adjusterEmail;
                            notifyUser(objRule.Description, claimId, recipient);
                        }
                    }
                }
            }

        }

        public static void notifyUser(string description, int claimid, string recipient)
        {

            StringBuilder emailBody = new StringBuilder();
            string password = null;
            string[] recipients = null;
            string smtpHost = null;
            int smtpPort = 0;
            //string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
            string subject = "Red-Flag Alert: " + description + " Claim # " + claimid;
            CRM.Data.Entities.SecUser user = null;

            // get logged in user
            int userID = SessionHelper.getUserId();
            // get logged in user email info
            user = SecUserManager.GetByUserId(userID);
            // load email credentials
            smtpHost = user.emailHost;
            int.TryParse(user.emailHostPort, out smtpPort);

            // recipients
            recipients = new string[] { recipient };

            // build email body
            // .containerBox
            emailBody.Append("<div>");
            emailBody.Append("<div>");
            emailBody.Append("Claim Ruler Red Flag Alert.<br><br>");
            emailBody.Append("Please correct the following issue as soon as possible:  ");
            emailBody.Append(description + "with claim # " + claimid);
            emailBody.Append("</div>");	// paneContentInner
            emailBody.Append("</div>");	// containerBox

            password = Core.SecurityManager.Decrypt(user.emailPassword);

            Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, true);//user.isSSL ??

        }





        #endregion

        protected void btnShowLossTemplate_Click(object sender, EventArgs e)
        {
            int policyId = 0;
            Claim objClaim = null;
            ClaimManager objClaimManager = new ClaimManager();
            int claimId = Convert.ToInt32(hf_ClaimIdForStatus.Value);
            using (TransactionScope scope = new TransactionScope())
            {
                

                objClaim = objClaimManager.Get(claimID);
                policyId = objClaim.PolicyID;

                // delete limit,claimlimit,policylimit data which enter as loss details
                LimitManager.DeleteLimit(claimId, policyId);

                //ClaimLimitManager.IsDeleted(claimId);
                // PolicyLimitManager.IsDeleted(policyId);

                //enter claim limit and policy limit
                //first get all limit
                List<Limit> objLimit = LimitManager.GetAllLimit(true);

                foreach (var limit in objLimit)
                {
                    ClaimLimit objClaimLimit = new ClaimLimit();
                    objClaimLimit.ClaimID = claimId;
                    objClaimLimit.LimitID = limit.LimitID;
                    ClaimLimitManager.Save(objClaimLimit);

                    PolicyLimit objPolicyLimit = new PolicyLimit();
                    objPolicyLimit.PolicyID = policyId;
                    objPolicyLimit.LimitID = limit.LimitID;
                    PolicyLimitManager.Save(objPolicyLimit);
                }

                Data.Entities.LeadPolicy objLeadPolicy = new Data.Entities.LeadPolicy();
                objLeadPolicy.Id = policyId;
                objLeadPolicy.ApplyAcrossAllCoverage = false;
                objLeadPolicy.ApplyDeductibleSet = false;
                LeadPolicyManager.Update(objLeadPolicy);
                scope.Complete();
            }

            //acrossAllCoverages.Enabled = true;
            //coverageSpecific.Enabled = true;
            txtDeductible.Enabled = true;


            propertyLimits.bindData(policyID);

           // casualtyLimits.bindData(claimId);

        }

    }


    public class ContactList
    {
        public int ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string ContactType { get; set; }
        public string IdOf { get; set; }
    }


}