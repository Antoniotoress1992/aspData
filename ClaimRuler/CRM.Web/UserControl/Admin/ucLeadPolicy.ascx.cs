using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Repository;
using CRM.Web.Utilities;
using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;
using System.Data;

namespace CRM.Web.UserControl.Admin {
	public partial class ucLeadPolicy : System.Web.UI.UserControl {
		public delegate void statusChangeHandler(object sender, EventArgs e);
		public event statusChangeHandler statusChanged;
		protected Protected.ClaimRuler masterPage = null;

		int clientID = 0;
		public string policyType {
			set { ViewState["policyType"] = value; }

			get {
				return ViewState["policyType"].ToString();
			}
		}

		/// <summary>
		/// Returns policy ID being edited
		/// </summary>
		public int policyID {
			get {
				return Session["policyID"] != null ? Convert.ToInt32(Session["policyID"]) : 0;
			}
			set {
				Session["policyID"] = value;
			}
		}

		int roleID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;

			roleID = SessionHelper.getUserRoleId();
			clientID = SessionHelper.getClientId();
            

			// check permissions
			if (Core.PermissionHelper.checkViewPermission("UsersLeads.aspx") && !Core.PermissionHelper.checkEditPermission("UsersLeads.aspx"))
				masterPage.disableControls(this.pnlContent, false);

			hlnlNewClaim.Visible = Core.PermissionHelper.checkAddPermission("UsersLeads.aspx");

			if (!Page.IsPostBack) {
				//bindPolicyData();
				tabContainerPolicy.ActiveTabIndex = 0;
			}

            //bindLimits();
            SetAddCoverageAdd();

		}

		protected void btnInvoice_Click(object sender, EventArgs e) {
			string url = "~/protected/admin/LeadInvoice.aspx?t=" + policyType;

			Response.Redirect(url);
		}

		public void bindData() {
			// main binding routine 
			List<StatusMaster> statusMasters = null;
			List<SubStatusMaster> subStatusMasters = null;
			List<StateMaster> states = null;

			IQueryable<AdjusterMaster> adjusters = null;

			int clientID = SessionHelper.getClientId();

			int leadID = Convert.ToInt32(Session["LeadIds"]);

			// bind states
			states = State.GetAll();

			//CollectionManager.FillCollection(ddlState, "StateId", "StateName", CarrierManager.GetCarriers(clientID));
             var state= CarrierManager.GetCarriers(clientID);
            ddlState.DataSource = state.ToList();
            ddlState.DataValueField = "StateId";
            ddlState.DataTextField = "StateName";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("--- Select ---", "0"));




			if (clientID > 0) {
				adjusters = AdjusterManager.GetAll(clientID);

				statusMasters = StatusManager.GetAll(clientID);

				subStatusMasters = SubStatusManager.GetAll(clientID);


			}
			else {
				adjusters = AdjusterManager.GetAll();

				statusMasters = StatusManager.GetAll(); 

				subStatusMasters = SubStatusManager.GetAll();
			}

			//CollectionManager.FillCollection(ddlCarrier, "CarrierId", "CarrierName", CarrierManager.GetCarriers(clientID));
            var carrier = CarrierManager.GetCarriers(clientID);
            ddlCarrier.DataSource = state.ToList();
            ddlCarrier.DataValueField = "CarrierId";
            ddlCarrier.DataTextField = "CarrierName";
            ddlCarrier.DataBind();
            ddlCarrier.Items.Insert(0, new ListItem("--- Select ---", "0"));


			bindAgents();

			if (this.policyID > 0) {
				// edit existing policy

				tabPanelAgent.Visible = true;
				tabPanelPolicyNotes.Visible = true;

				fillForm();

				//bindDocuments(leadID);

				//bindContacts();

				bindLienHolders();

				//fillComments(leadID);

				bindLimits();

				bindSubLimits();

				bindClaims();

				activateLinks();

				tabPanelLienHolder.Visible = true;
                ShowAddCoverage();
                SetAddCoverage(this.policyID);
			}
			else {
				// create blank one
				tabPanelAgent.Visible = false;
				tabPanelPolicyNotes.Visible = false;

				bindLimits();

				// create blank one
				bindSubLimits();

				tabPanelLienHolder.Visible = false;

				hf_policyID.Value = "0";
				hf_lastStatusID.Value = "0";

				clearFields();
                HideAddCoverage();
                ShowAddCoverageAdd();
			}



		}

        protected void HideAddCoverage()
        {
            //divradiobuttonCoverageAdd.Visible = false;
            //divbuttonCoverageAdd.Visible = false;

            divradiobuttonCoverage.Visible = false;
            divbuttonCoverage.Visible = false;
        }

        protected void ShowAddCoverage()
        {
            divradiobuttonCoverageAdd.Visible = false;
            divbuttonCoverageAdd.Visible = false;

            divradiobuttonCoverage.Visible = true;
            divbuttonCoverage.Visible = true;
            if (this.policyID>0)
            {
            hdnPolicyIdDetuctible.Value = this.policyID.ToString();
            }

        }

        protected void SetAddCoverage(int policyId)
        {
            List<PolicyLimit> limits = null;

            limits = PolicyLimitManager.GetAll(policyID);
            if (limits != null && limits.Count > 0)
            {
                bool applyAcrossAll = limits[0].ApplyAcrossAllCoverage ?? false;
                if (applyAcrossAll)
                {
                    txtDeductible.Enabled = false;
                    acrossAllCoverages.Checked = true;

                    acrossAllCoverages.Enabled = false;
                    coverageSpecific.Enabled = false;

                    coverageSpecific.Checked = false;
                    hdnApplyDeductible.Value = "1";
                }
                else
                {
                    acrossAllCoverages.Enabled = false;
                    coverageSpecific.Enabled = false;

                    txtDeductible.Enabled = true;
                    acrossAllCoverages.Checked = false;
                    coverageSpecific.Checked = true;
                    hdnApplyDeductible.Value = "0";
                }
                if (limits[0].ApplyAcrossAllCoverage == null)
                {
                    acrossAllCoverages.Enabled = true;
                    coverageSpecific.Enabled = true;
                    txtDeductible.Enabled = true;
                    acrossAllCoverages.Checked = false;
                    coverageSpecific.Checked = true;
                    hdnApplyDeductible.Value = "0";
                }

                //if (limits[0].ApplyAcrossAllCoverage==null)
                //{

                //}
            }
            else
            {
                acrossAllCoverages.Enabled = true;
                coverageSpecific.Enabled = true;
                txtDeductible.Enabled = true;
                acrossAllCoverages.Checked = true;
                coverageSpecific.Checked = false;
                hdnApplyDeductible.Value = "1";
            }

        }

        protected void SetAddCoverageAdd()
        {
            if (HttpContext.Current.Session["Limit"] != null && HttpContext.Current.Session["PolicyLimit"] != null && HttpContext.Current.Session["tblCasulityPolicylimit"] != null && HttpContext.Current.Session["tblAllPolicylimit"] != null)
            {
                acrossAllCoveragesAdd.Enabled = false;
                coverageSpecificAdd.Enabled = false;

                DataTable policyLimit = HttpContext.Current.Session["tblAllPolicylimit"] as DataTable;
                if(policyLimit.Rows.Count>0)
                {
                    bool acrossAll=Convert.ToBoolean(policyLimit.Rows[0]["ApplyAcrossAllCoverage"].ToString());
                    if (acrossAll == true)
                    {
                        acrossAllCoveragesAdd.Checked = true;
                        coverageSpecificAdd.Checked = false;
                        txtDeductibleAdd.Enabled = false;
                        hdnApplyDeductibleAdd.Value = "1";
                    }
                    else
                    {
                        acrossAllCoveragesAdd.Checked = false;
                        coverageSpecificAdd.Checked = true;
                        hdnApplyDeductibleAdd.Value = "0";
                    }

                }

                if (policyLimit.Rows.Count<=0)
                {
                    acrossAllCoveragesAdd.Enabled = true;
                    coverageSpecificAdd.Enabled = true;

                    acrossAllCoveragesAdd.Checked = true;
                    coverageSpecificAdd.Checked = false;

                    txtDeductibleAdd.Enabled = true;
                    hdnApplyDeductibleAdd.Value = "1";
                }



            }
            else
            {
                acrossAllCoveragesAdd.Enabled = true;
                coverageSpecificAdd.Enabled = true;
                txtDeductibleAdd.Enabled = true;
                acrossAllCoveragesAdd.Checked = true;
                hdnApplyDeductibleAdd.Value = "1";
            }
        }


        protected void ShowAddCoverageAdd()
        {
            divradiobuttonCoverageAdd.Visible = true;
            divbuttonCoverageAdd.Visible = true;

            divradiobuttonCoverage.Visible = false;
            divbuttonCoverage.Visible = false;

            hdnPolicyIdDetuctible.Value = "0";

        }

        protected void HideAddCoverageAdd()
        {
            //divradiobuttonCoverageAdd.Visible = false;
            //divbuttonCoverageAdd.Visible = false;

            divradiobuttonCoverageAdd.Visible = false;
            divbuttonCoverageAdd.Visible = false;
        }


		private void bindAgents() {
			int clientID = SessionHelper.getClientId();
			List<Contact> agents = null;

			agents = ContactManager.GetAll(clientID, (int)Globals.ContactType.Agent);

			CollectionManager.FillCollection(ddlAgent, "ContactID", "contactName", agents);
		}

		private void bindClaims() {
			pnlClaims.Visible = true;

			claimLists.bindData(this.policyID);
		}

		//protected void bindContacts() {
		//	int leadID = Convert.ToInt32(Session["LeadIds"]);

		//	int policyTypeID = Convert.ToInt32(policyType);

		//	//leadPolicyContact.bindData(leadID, policyTypeID);
		//}

		protected void bindLimits() {
			propertyLimits.bindData(policyID);

			//casualtyLimits.bindData(policyID);
		}

		protected void bindSubLimits() {
			propertySubLimits.bindData(policyID, 1);
		}

		protected void bindLienHolders() {
			int policyID = SessionHelper.getPolicyID();

			List<PolicyLienholder> lienholders = LienholderManager.GetAll(policyID);

			gvMortgagee.DataSource = lienholders;
			gvMortgagee.DataBind();


		}

		protected void bindMasterLienholders() {


			// load lienholders
			clientID = SessionHelper.getClientId();

			List<Mortgagee> mortgagees = MortgageeManager.GetAll(clientID).ToList();

			CollectionManager.FillCollection(ddlMorgagee, "MortgageeID", "MortageeName", mortgagees);
		}


		protected void ddlLeadStatus_selectedIndexChanged(object sender, EventArgs e) {
			DropDownList ddl = sender as DropDownList;

			if (Convert.ToInt32(Session["LeadIds"]) > 0 && Convert.ToInt32(ddl.SelectedValue) > 0) {
				saveForm();

				this.statusChanged(sender, new EventArgs());
			}
		}

		protected void clearFields() {
			ucPolicyType1.SelectedIndex = -1;

			txtInsuranceCompanyName.Text = string.Empty;
			txtInsuranceAddress.Text = string.Empty;
            txtInsuranceAddress2.Text = string.Empty;
			txtInsurancePhoneNumber.Text = string.Empty;
			txtInsuranceFaxNumber.Text = string.Empty;
			txtInsurancePolicyNumber.Text = string.Empty;

		}

		public void fillForm() {
			Contact primaryContact = null;
			string zipCode = null;

			if (policyID > 0) {
                CRM.Data.Entities.LeadPolicy policy = LeadPolicyManager.Get(policyID);

				if (policy == null)
					return;

				ViewState["policyType"] = policy.PolicyType.ToString();

				hf_policyID.Value = policy.Id.ToString();
				hf_lastStatusID.Value = (policy.LeadStatus ?? 0).ToString();

				//cbxAllDocumentUploaded.Checked = policy.isAllDocumentUploaded ?? false;

				if (policy.PolicyType != null)
					ucPolicyType1.SelectedValue = policy.PolicyType.ToString();

				txtPolicyFormType.Text = policy.PolicyFormType;

				txtInsuranceCompanyName.Text = policy.Carrier == null ? policy.InsuranceCompanyName : policy.Carrier.CarrierName;
				txtInsuranceAddress.Text = policy.Carrier == null ? policy.InsuranceAddress : policy.Carrier.AddressLine1;
                txtInsuranceAddress2.Text = policy.Carrier == null ? "" : policy.Carrier.AddressLine2;
				txtInsurancePolicyNumber.Text = policy.PolicyNumber;

				if (policy.Carrier != null) {
					txtInsuranceState.Text = policy.Carrier.StateName;
					txtInsuranceCity.Text = policy.Carrier.CityMaster != null ? policy.Carrier.CityMaster.CityName : string.Empty;
					txtInsuranceZipCode.Text = policy.Carrier.ZipCode;

					ddlCarrier.SelectedValue = policy.CarrierID.ToString();

					// get primary carrier contact
					primaryContact = CarrierContactManager.GetPrimaryContact((int)policy.CarrierID);
					if (primaryContact != null) {
						txtPrimaryContactName.Text = primaryContact.fullName;
						txtPrimaryContactEmail.Text = primaryContact.Email;
						txtPrimaryContactPhone.Text = primaryContact.Phone;
					}
				}
				else {
					zipCode = policy.InsuranceZipCode ?? "";
				}

				txtInsuranceFaxNumber.Text = policy.FaxNumber;

				txtInsurancePhoneNumber.Text = policy.PhoneNumber;

				txtEffectiveDate.Value = policy.EffectiveDate;

				txtExpirationDate.Value = policy.ExpirationDate;

				txtInitialCoverageDate.Value = policy.InitialCoverageDate;

				if (policy.AgentID != null)
					fillAgentDetails((int)policy.AgentID);
			}
		}


		protected void activateLinks() {
			// hook "New Comment" link							
			//hlnlNewComment.Attributes["href"] = string.Format("javascript:addNewComment('{0}');", policyType);

			// hook "New Document" link		
			//hlnkNewDocument.Attributes["href"] = string.Format("javascript:uploadDocument('{0}');", policyType);
		}

		protected void hlnlNewClaim_Click(object sender, EventArgs e) {
			Session["ClaimID"] = 0;

			string url = "~/Protected/ClaimEdit.aspx";

			Response.Redirect(url);
		}

		public void saveForm() {
			Page.Validate("Policy");
            //if (!Page.IsValid)
            //    return;

            CRM.Data.Entities.LeadPolicy policy = null;

			lblMessage.Text = string.Empty;
			lblMessage.CssClass = string.Empty;

			//if (int.TryParse(hf_policyID.Value, out id) && id > 0) {
			if (policyID > 0) {
				policy = LeadPolicyManager.Get(policyID);
			}
			else {
				// new
                policy = new CRM.Data.Entities.LeadPolicy();
				policy.LeadId = Core.SessionHelper.getLeadId();
				policy.Id = 0;
				policy.IsActive = true;
				policy.isAllDocumentUploaded = false;
			}
            if (ucPolicyType1.SelectedValue == "0" || ucPolicyType1.SelectedValue == "")
            {
                policy.PolicyType = null;
            }
            else
            {
                policy.PolicyType = Convert.ToInt32(ucPolicyType1.SelectedValue);
            }

			// carrier information
			if (ddlCarrier.SelectedIndex > 0)
				policy.CarrierID = Convert.ToInt32(ddlCarrier.SelectedValue);
			else
				policy.CarrierID = null;

			policy.PolicyNumber = txtInsurancePolicyNumber.Text.Trim();
			policy.PolicyFormType = txtPolicyFormType.Text.Trim();

			policy.InsuranceCompanyName = txtInsuranceCompanyName.Text;

			if (!string.IsNullOrEmpty(txtExpirationDate.Text))
				policy.ExpirationDate = txtExpirationDate.Date;
			else
				policy.ExpirationDate = null;

			if (!string.IsNullOrEmpty(txtExpirationDate.Text))
				policy.EffectiveDate = txtEffectiveDate.Date;
			else
				policy.EffectiveDate = null;

			if (!string.IsNullOrEmpty(txtInitialCoverageDate.Text))
				policy.InitialCoverageDate = txtInitialCoverageDate.Date;

			if (ddlAgent.SelectedIndex > 0)
				policy.AgentID = Convert.ToInt32(ddlAgent.SelectedValue);
			
			try {
				using (TransactionScope scope = new TransactionScope()) {
					// save policy id
					this.policyID = LeadPolicyManager.Save(policy);



                    if (HttpContext.Current.Session["Limit"] != null && HttpContext.Current.Session["PolicyLimit"] != null && HttpContext.Current.Session["tblCasulityPolicylimit"] != null && HttpContext.Current.Session["tblAllPolicylimit"] != null)
                    {
                        AddLimitPolicy(this.policyID);
                        CacheBlank();
                        ShowAddCoverage();
                        SetAddCoverage(this.policyID);
                        HideAddCoverageAdd();
                    }
                    else
                    {

                        propertyLimits.saveLimits(policyID);

                        //casualtyLimits.saveLimits(policyID);

                    }





					propertySubLimits.saveLimits(policyID);

					scope.Complete();
				}

				// save policy type in viewstate
				policyType = ucPolicyType1.SelectedValue;

				// save policy id
				hf_policyID.Value = policyID.ToString();

				// activate other tabs
				tabContainerPolicy.Visible = true;

				activateLinks();

				bindLimits();

				bindClaims();

                //bindData();
                

				lblMessage.Text = "Policy Information saved successfully.";
				lblMessage.CssClass = "ok";
                
			}
			catch (Exception ex) {
				lblMessage.Text = "Policy Information was not saved.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}
		}

        protected void CacheBlank()
        {
           // Response.Redirect(Request.RawUrl)
            if (HttpContext.Current.Session["Limit"] != null && HttpContext.Current.Session["PolicyLimit"] != null && HttpContext.Current.Session["tblCasulityPolicylimit"] != null && HttpContext.Current.Session["tblAllPolicylimit"] != null)
            {
                HttpContext.Current.Session["Limit"] = null;
                HttpContext.Current.Session["PolicyLimit"] = null;
                HttpContext.Current.Session["tblCasulityPolicylimit"] = null;
                HttpContext.Current.Session["tblAllPolicylimit"] = null;
            }
        }

       


		#region lienholder methods
		protected void gvMortgagee_RowCommand(object sender, GridViewCommandEventArgs e) {
			LeadPolicyLienholder lienHolder = null;

			if (e.CommandName == "DoDelete") {
				LienholderManager.Delete(Convert.ToInt32(e.CommandArgument));

				bindLienHolders();
			}
		}
		#endregion

		private void notifyAdjuster(CRM.Data.Entities.LeadPolicy policy) {
			AdjusterMaster adjuster = null;
			string clientName = null;
			string claimantName = null;
			string emailText = null;
			string emailPassword = null;
			Leads lead = null;
			int leadID = 0;
			string[] recipients = null;
			string userEmail = null;
			string subject = null;

			// get adjuster info
			adjuster = AdjusterManager.Get(policy.AdjusterID ?? 0);

			// retreive lead information
			leadID = SessionHelper.getLeadId();

			lead = LeadsManager.GetByLeadId(leadID);

			if (lead != null && adjuster != null && !string.IsNullOrEmpty(adjuster.email) && (adjuster.isEmailNotification ?? true)) {
				recipients = new string[] { adjuster.email };

				userEmail = ConfigurationManager.AppSettings["userID"].ToString();
				emailPassword = ConfigurationManager.AppSettings["Password"].ToString();

				// name of CRM client
				clientName = adjuster.Client == null ? "" : adjuster.Client.BusinessName;

				claimantName = string.Format("{0} {1}", lead.ClaimantFirstName ?? "", lead.ClaimantLastName ?? "");

				subject = string.Format("{0} has assigned {1} to you.", clientName, claimantName);

				emailText = string.Format("<br>Claim # {0} was assigned to you.<br>Please review {1} right away to begin the file.<br><br>Thank you.<br><br>http://app.claimruler.com",
						policy.ClaimNumber,
						claimantName);

				Core.EmailHelper.sendEmail(adjuster.email, recipients, null, subject, emailText, null, userEmail, emailPassword);
			}
		}

		private void setReminder(int leadID, CRM.Data.Entities.LeadPolicy policy, int masterStatusID) {
			int lastStatusID = 0;
			double duration = 0;
			LeadTask task = null;
			DateTime reminderDate = DateTime.MaxValue;
			string sdate = null;

			// return if no change in status
			int.TryParse(hf_lastStatusID.Value, out lastStatusID);

			if (lastStatusID == masterStatusID)
				return;

			// get status with reminder duration
			StatusMaster statusMaster = StatusManager.GetStatusId(masterStatusID);

			// return if master status not loaded properly
			if (statusMaster == null)
				return;



			// get task associated with policy
			task = TasksManager.GetPolicyReminderTask(leadID, policy.Id);

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
					task.lead_policy_id = policy.Id;
					task.policy_type = policy.PolicyType;
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

			// refresh tasks on parent page
			if (statusChanged != null)
				statusChanged(this, null);
		}


		protected void ddlCarrier_SelectedIndexChanged(object sender, EventArgs e) {
			int carrierID = Convert.ToInt32(ddlCarrier.SelectedValue);
			Carrier carrier = CarrierManager.GetByID(carrierID);

			if (carrier != null) {
				txtInsuranceCompanyName.Text = carrier.CarrierName;
				txtInsuranceAddress.Text = carrier.AddressLine1;
				txtInsuranceState.Text = carrier.StateName;
				txtInsuranceCity.Text = carrier.CityName;
				txtInsuranceZipCode.Text = carrier.ZipCode;

				txtInsuranceCountry.Text = carrier.CountryMaster == null ? "" : carrier.CountryMaster.CountryName;

			}
		}

		protected void btnSaveHidden_Click(object sender, EventArgs e) {
			saveForm();

		}


		protected void btnPolicyNoteSave_Click(object sender, EventArgs e) {
			PolicyNote policyNote = null;
			int policyID = 0;
			int policyNoteID = 0;

			Page.Validate("policyNote");
			if (!Page.IsValid)
				return;

			policyID = Convert.ToInt32(hf_policyID.Value);
			policyNoteID = Convert.ToInt32(ViewState["policyNoteID"]);

			using (PolicyNoteManager repository = new PolicyNoteManager()) {
				if (policyNoteID == 0) {
					policyNote = new PolicyNote();
					policyNote.PolicyID = policyID;
					policyNote.NoteDate = DateTime.Now;
					policyNote.UserID = SessionHelper.getUserId();
				}
				else {
					policyNote = repository.Get(policyNoteID);
				}

				if (policyNote != null) {
					policyNote.Notes = txtPolicyNote.Text.Trim();
					repository.Save(policyNote);
				}
			}

			pnlPolicyNotes.Visible = true;
			pnlPolicyNoteEdit.Visible = false;

			wdgNotes.DataBind();
		}

		protected void btnPolicyNoteCancel_Click(object sender, EventArgs e) {
			pnlPolicyNotes.Visible = true;
			pnlPolicyNoteEdit.Visible = false;

			wdgNotes.DataBind();
		}

		protected void lbtnPolicyNoteNew_Click(object sender, EventArgs e) {
			pnlPolicyNotes.Visible = false;
			pnlPolicyNoteEdit.Visible = true;
			txtPolicyNote.Text = string.Empty;

			ViewState["policyNoteID"] = "0";
		}

		protected void wdgNotes_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e) {
			int policyNoteID = 0;
			PolicyNote note = null;

			if (e.CommandName == "DoEdit") {
				policyNoteID = Convert.ToInt32(e.CommandArgument);
				using (PolicyNoteManager repository = new PolicyNoteManager()) {
					note = repository.Get(policyNoteID);

					pnlPolicyNoteEdit.Visible = true;
					pnlPolicyNotes.Visible = false;

					txtPolicyNote.Text = note.Notes;
				}
				ViewState["policyNoteID"] = policyNoteID.ToString();
			}
			else if (e.CommandName == "DoRemove") {
				policyNoteID = Convert.ToInt32(e.CommandArgument);

				using (PolicyNoteManager repository = new PolicyNoteManager()) {
					note = repository.Get(policyNoteID);
					repository.Delete(note);

					btnPolicyNoteCancel_Click(null, null);
				}
			}
		}

		protected void lbtnMortgageeAdd_Click(object sender, EventArgs e) {
			pnlMortgageeAdd.Visible = true;
			pnlMortgageeGrid.Visible = false;

			bindMasterLienholders();
		}

		protected void btnMortageeAddSave_Click(object sender, EventArgs e) {
			Page.Validate("mortgagee");
			if (!Page.IsValid)
				return;

			PolicyLienholder lienholder = new PolicyLienholder();

			lienholder.PolicyID = this.policyID;

			lienholder.MortgageeID = Convert.ToInt32(this.ddlMorgagee.SelectedValue);

			lienholder.LoanNumber = txtLoanNumber.Text;

			LienholderManager.Save(lienholder);

			bindLienHolders();

			btnMortageeAddCancel_Click(null, null);
		}

		protected void btnMortageeAddCancel_Click(object sender, EventArgs e) {
			pnlMortgageeAdd.Visible = false;
			pnlMortgageeGrid.Visible = true;

			bindLienHolders();
		}

		protected void ddlState_SelectedIndexChanged(object sender, EventArgs e) {
			if (ddlState.SelectedIndex > 0) {
				CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll(Convert.ToInt32(ddlState.SelectedValue)));
			}
			else {
				ddlCity.Items.Clear();
			}
		}

		protected void dllCity_SelectedIndexChanged(object sender, EventArgs e) {
			if (ddlCity.SelectedIndex > 0) {
				CollectionManager.FillCollection(ddlAgentZip, "ZipCodeID", "ZipCode", ZipCode.getByCityID(Convert.ToInt32(ddlCity.SelectedValue)));
			}
			else {
				ddlAgentZip.Items.Clear();
			}

		}

		protected void ddlAgent_SelectedIndexChanged(object sender, EventArgs e) {

			if (ddlAgent.SelectedIndex > 0) {
				fillAgentDetails(Convert.ToInt32(ddlAgent.SelectedValue));

			}
		}

		private void fillAgentDetails(int contactID) {
			Contact contact = null;

			contact = ContactManager.Get(contactID);

			if (contact != null) {

				showInputFields(false);

				showLabelField(true);

				lblAgentFirstName.Text = contact.FirstName;
				lblAgentLastName.Text = contact.LastName;
				lblAgentAddress1.Text = contact.Address1;
				lblAgentAddress2.Text = contact.Address2;

				if (contact.StateID != null)
					ddlState.SelectedValue = contact.StateID.ToString();

				if (contact.CityID != null) {
					CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll((int)contact.StateID));
					ddlCity.SelectedValue = contact.CityID.ToString();
				}
				if (contact.ZipCodeID != null) {
					CollectionManager.FillCollection(ddlAgentZip, "ZipCodeID", "ZipCode", ZipCode.getByCityID((int)contact.CityID));
					ddlAgentZip.SelectedValue = contact.ZipCodeID.ToString();
				}
				lblAgentEmail.Text = contact.Email;
				lblAgentPhone.Text = contact.Phone;
				lblAgentFax.Text = contact.Fax;

				lblAgentSubCode.Text = contact.AgenctSubcode;
				lblAgentCode.Text = contact.AgentCode;
				lblAgentCustomerID.Text = contact.AgentCustomerID;

			}
		}

		protected void lbtnAgentNew_Click(object sender, EventArgs e) {

			showInputFields(true);

			showLabelField(false);

			ddlAgent.SelectedIndex = 0;

			txtAgenctSubcode.Text = string.Empty;
			txtAgentAddress1.Text = string.Empty;
			txtAgentAddress2.Text = string.Empty;
			txtAgentCode.Text = string.Empty;
			txtAgentCustomerID.Text = string.Empty;
			txtAgentEmail.Text = string.Empty;
			txtAgentEntityName.Text = string.Empty;
			txtAgentFax.Text = string.Empty;
			txtAgentFirstName.Text = string.Empty;
			txtAgentLastName.Text = string.Empty;
			txtAgentPhoneNumber.Text = string.Empty;
			
			ddlAgentZip.SelectedIndex = -1;
			ddlCity.SelectedIndex = -1;
			ddlState.SelectedIndex = -1;
		}

		private void showLabelField(bool isVisible) {
			pnlAgentButton.Visible = !isVisible;

			lblAgentAddress1.Visible = isVisible;
			lblAgentAddress2.Visible = isVisible;
			lblAgentCity.Visible = isVisible;
			lblAgentCode.Visible = isVisible;
			lblAgentCustomerID.Visible = isVisible;
			lblAgentEmail.Visible = isVisible;
			lblAgentEntityName.Visible = isVisible;
			lblAgentFax.Visible = isVisible;
			lblAgentFirstName.Visible = isVisible;
			lblAgentLastName.Visible = isVisible;
			lblAgentPhone.Visible = isVisible;
			lblAgentState.Visible = isVisible;
			lblAgentSubCode.Visible = isVisible;
			lblAgentZip.Visible = isVisible;
		}

		private void showInputFields(bool isVisible) {
			pnlAgentButton.Visible = !isVisible;

			txtAgentEntityName.Visible = isVisible;
			txtAgentFirstName.Visible = isVisible;
			txtAgentLastName.Visible = isVisible;
			txtAgentAddress1.Visible = isVisible;
			txtAgentAddress2.Visible = isVisible;
			ddlState.Visible = isVisible;
			ddlCity.Visible = isVisible;
			ddlAgentZip.Visible = isVisible;
			txtAgentEmail.Visible = isVisible;
			txtAgentPhoneNumber.Visible = isVisible;
			txtAgentFax.Visible = isVisible;
			txtAgentCode.Visible = isVisible;
			txtAgenctSubcode.Visible = isVisible;
			txtAgentCustomerID.Visible = isVisible;
		}

		protected void btnAgentNewSave_Click(object sender, EventArgs e) {
            CRM.Data.Entities.LeadPolicy policy = null;

			Contact contact = null;

			contact = new Contact();
			contact.ClientID = SessionHelper.getClientId();
			contact.IsActive = true;
			contact.Address1 = txtAgentAddress1.Text;
			contact.Address2 = txtAgentAddress2.Text;

			contact.CategoryID = (int)Globals.ContactType.Agent;
			contact.CompanyName = txtAgentEntityName.Text;
			contact.ContactName = txtAgentFirstName.Text + " " + txtAgentLastName.Text;

			contact.Email = txtAgentEmail.Text;
			contact.Fax = txtAgentFax.Text;
			contact.FirstName = txtAgentFirstName.Text;
			contact.LastName = txtAgentLastName.Text;

			contact.Phone = txtAgentPhoneNumber.Text;

			if (ddlState.SelectedIndex > 0)
				contact.StateID = Convert.ToInt32(ddlState.SelectedValue);

			if (ddlCity.SelectedIndex > 0)
				contact.CityID = Convert.ToInt32(ddlCity.SelectedValue);

			if (ddlAgentZip.SelectedIndex > 0)
				contact.ZipCodeID = Convert.ToInt32(ddlAgentZip.SelectedValue);


			try {
				using (TransactionScope scope = new TransactionScope()) {
					contact = ContactManager.Save(contact);

					policy = LeadPolicyManager.Get(this.policyID);

					policy.AgentID = contact.ContactID;

					LeadPolicyManager.Save(policy);

					scope.Complete();
				}

				showInputFields(false);

				showLabelField(true);

				bindAgents();

				ddlAgent.SelectedValue = contact.ContactID.ToString();

				fillAgentDetails(contact.ContactID);
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
			}


		}

		protected void btnAgentNewCancel_Click(object sender, EventArgs e) {
			showInputFields(false);

			showLabelField(true);
		}

        protected void btnShowLossTemplate_Click(object sender, EventArgs e)
        {
            //int policyId = 0;
            
            ClaimManager objClaimManager = new ClaimManager();
            int policyId = Convert.ToInt32(hdnPolicyIdDetuctible.Value);

           List<Claim> lstClaim  =objClaimManager.GetPolicyClaim(policyId);


            using (TransactionScope scope = new TransactionScope())
            {
                //delete all claim from claim limit
                foreach (var claim in lstClaim)
                {
                    int claimId = claim.ClaimID;
                    ClaimLimitManager.IsDeleted(claimId);
                }


                // delete limit,claimlimit,policylimit data which enter as loss details
                LimitManager.DeletePolicyLimit( policyId);
               
                //first get all limit
                List<Limit> objLimit = LimitManager.GetAllLimit(true);

               
                foreach (var limit in objLimit)
                {    
                    //enter in 
                    PolicyLimit objPolicyLimit = new PolicyLimit();
                    objPolicyLimit.PolicyID = policyId;
                    objPolicyLimit.LimitID = limit.LimitID;
                    PolicyLimitManager.Save(objPolicyLimit); 

                }

                //code for enter in all claim in claim limit
                foreach (var claim in lstClaim)
                {
                    foreach (var limit in objLimit)
                    {
                        ClaimLimit objClaimLimit = new ClaimLimit();
                        objClaimLimit.ClaimID = claim.ClaimID;
                        objClaimLimit.LimitID = limit.LimitID;
                        ClaimLimitManager.Save(objClaimLimit);
                    }

                }

                scope.Complete();
            }

            acrossAllCoverages.Enabled = true;
            coverageSpecific.Enabled = true;
            txtDeductible.Enabled = true;


            propertyLimits.bindData(policyId);
            //casualtyLimits.bindData(policyId);
        }

        protected void btnShowLossTemplateAdd_Click(object sender, EventArgs e)
        {
            
            if (HttpContext.Current.Session["Limit"] != null && HttpContext.Current.Session["PolicyLimit"] != null && HttpContext.Current.Session["tblCasulityPolicylimit"] != null && HttpContext.Current.Session["tblAllPolicylimit"] != null)
            {              

                HttpContext.Current.Session["Limit"] = null;
                HttpContext.Current.Session["PolicyLimit"] = null;
                HttpContext.Current.Session["tblCasulityPolicylimit"] = null;
                HttpContext.Current.Session["tblAllPolicylimit"] = null;
            }
            acrossAllCoveragesAdd.Enabled = true;
            coverageSpecificAdd.Enabled = true;
            txtDeductibleAdd.Enabled = true;
            acrossAllCoveragesAdd.Checked = true;
            hdnApplyDeductibleAdd.Value = "1";
            bindLimits();            
        }

        protected void AddLimitPolicy(int policyId)
        {

            Limit limits = null;
            Limit limits2 = null;
            PolicyLimit objPolicyLimit = null;

            DataTable tbllimitGet = HttpContext.Current.Session["Limit"] as DataTable;
            DataTable tblPolicylimitGet = HttpContext.Current.Session["PolicyLimit"] as DataTable;
            DataTable tblAllPolicylimitGet = HttpContext.Current.Session["tblAllPolicylimit"] as DataTable;

            string type = string.Empty;

            for (int count = 0; count < tbllimitGet.Rows.Count; count++)
            {
                limits = new Limit();
                limits.LimitLetter = tbllimitGet.Rows[count]["LimitLetter"].ToString();
                limits.LimitType =Convert.ToInt32(tbllimitGet.Rows[count]["LimitType"].ToString());
                limits.LimitDescription = tbllimitGet.Rows[count]["LimitDescription"].ToString();
                limits.IsStatic =Convert.ToBoolean(tbllimitGet.Rows[count]["IsStatic"].ToString());
                limits2 = LimitManager.Save(limits);


                objPolicyLimit = new PolicyLimit();
                objPolicyLimit.PolicyID = policyId;
                objPolicyLimit.LimitID = limits2.LimitID;
                objPolicyLimit.LimitAmount = Convert.ToDecimal(tblAllPolicylimitGet.Rows[count]["LimitAmount"].ToString());
                objPolicyLimit.LimitDeductible = Convert.ToDecimal(tblAllPolicylimitGet.Rows[count]["LimitDeductible"].ToString());
                objPolicyLimit.ITV = Convert.ToDecimal(tblAllPolicylimitGet.Rows[count]["ITV"].ToString());
                objPolicyLimit.Reserve = Convert.ToDecimal(tblAllPolicylimitGet.Rows[count]["Reserve"].ToString());
                objPolicyLimit.IsDeleted = Convert.ToBoolean(tblAllPolicylimitGet.Rows[count]["IsDeleted"].ToString());
                objPolicyLimit.ApplyAcrossAllCoverage = Convert.ToBoolean(tblAllPolicylimitGet.Rows[count]["ApplyAcrossAllCoverage"].ToString());
                objPolicyLimit.ApplyTo = tblAllPolicylimitGet.Rows[count]["ApplyTo"].ToString(); 
                PolicyLimitManager.Save(objPolicyLimit);

            }


               







        }


	}
}