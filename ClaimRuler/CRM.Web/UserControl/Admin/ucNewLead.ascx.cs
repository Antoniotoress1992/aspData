


using System.Text;

using Infragistics.Web.UI.NavigationControls;

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
	using System.Data;
	using System.IO;
    using CRM.Data.Entities;
	#endregion
	public partial class ucNewLead : System.Web.UI.UserControl {
		string ErrorMessage = string.Empty;


		protected void Page_Load(object sender, EventArgs e) {
			int leadID = 0;

			// set default button
			Page.Form.DefaultButton = btnSaveAndContinue1.UniqueID;
			Page.Form.DefaultFocus = txtOriginalLeadDate.UniqueID;

			if (!IsPostBack) {
				bindProducer();

				bindState();

				bindDDL();

				if (this.Context.Items["selectedleadid"] != null && this.Context.Items["selectedleadid"].ToString().Length > 0
					&& Convert.ToInt32(this.Context.Items["selectedleadid"]) > 0
					&& this.Context.Items["view"] != null) {
					//hfLeadId.Value = this.Context.Items["selectedleadid"].ToString();

					Session["LeadIds"] = this.Context.Items["selectedleadid"].ToString();

					ViewState["LeadIds"] = this.Context.Items["selectedleadid"].ToString();

					hfAdminView.Value = this.Context.Items["view"].ToString();

					// reset page for photos
					Session["pageIndex"] = 0;

					// update last activity date
					LeadsManager.UpdateLastActivityDate(Convert.ToInt32(this.Context.Items["selectedleadid"]));

					FillForm(ViewState["LeadIds"].ToString(), hfAdminView.Value);

					bindTasks();



				}
				else {
					// new lead/claim				
					pnlTasks.Visible = false;

					txtOriginalLeadDate.Value = DateTime.Today;

					//tr_taskPanel.Visible = false;

					checkForTrialAccount();
				}
			}



			// 2013-07-27 tortega - refresh tasks
			if (ViewState["LeadIds"] != null && int.TryParse(ViewState["LeadIds"].ToString(), out leadID) && leadID > 0) {
				//bindTasks();

				pnlTasks.Visible = true;

				//buildLeadMenu();

				showControls(true);
			}
			else {
				showControls(false);
			}

			if (Request.Params["t"] != null) {
				tabContainer.ActiveTabIndex = Convert.ToInt16(Request.Params["t"]);
			}
		}

		private void checkForTrialAccount() {
			CRM.Data.Entities.Client client = null;
			int clientID = 0;
			int totalLeads = 0;

			if (Session["ClientId"] != null) {
				clientID = Convert.ToInt32(Session["ClientId"]);

				client = ClientManager.Get(clientID);
				if (client != null && (client.isTrial ?? false)) {
					totalLeads = ClientManager.GetLeadCount(clientID);

					if (totalLeads >= client.MaxLeads) {
						Response.Redirect("~/Protected/Admin/LeadLimitReached.aspx");
					}
				}
			}
		}

		protected void ibtnTasks_click(object sender, EventArgs e) {
			Response.Redirect("~/Protected/Admin/LeadSchedule.aspx");
		}

		private void bindTasks() {
			List<LeadTask> tasks = null;
			DateTime fromDate = DateTime.Today;

			// make a week
			DateTime toDate = DateTime.Today.AddDays(7);

			int leadID = SessionHelper.getLeadId();

			tasks = TasksManager.GetLeadTaskByLeadID(leadID, fromDate, toDate);
			gvTasks.DataSource = tasks;
			gvTasks.DataBind();

			if (tasks.Count == 0)
				gvTasks.GridLines = GridLines.None;

		}

		private void bindTasks(DateTime fromDate, DateTime toDate) {
			List<LeadTask> tasks = null;

			int leadID = SessionHelper.getLeadId();

			tasks = TasksManager.GetLeadTaskByLeadID(leadID, fromDate, toDate);
			gvTasks.DataSource = tasks;
			gvTasks.DataBind();
		}

		//protected void bindContactTypes() {
		//     CollectionManager.FillCollection(ddlContactType, "ID", "Description", LeadContactTypeManager.GetAll());

		//     CollectionManager.FillCollection(ddlInsuranceType, "ID", "Description", InsuranceTypeManager.GetAll());
		//}

		//protected void bindContacts() {
		//     int leadID = 0;

		//     int.TryParse(hfLeadId.Value, out leadID);

		//     gvContacts.DataSource = LeadContactManager.GetContactByLeadID(leadID);
		//     gvContacts.DataBind();
		//}

		private void bindProducer() {
			int clientID = SessionHelper.getClientId();
			IQueryable<ProducerMaster> producers = null;
			List<SecondaryProducerMaster> secondaryProducers = null;


			producers = ProducerManager.GetAll(clientID);

			secondaryProducers = SecondaryProducerManager.GetAll(clientID);

			//CollectionManager.FillCollection(ddlPrimaryProducer, "ProducerId", "ProducerName", producers);

            ddlPrimaryProducer.DataSource = producers.ToList();
            ddlPrimaryProducer.DataValueField = "ProducerId";
            ddlPrimaryProducer.DataTextField = "ProducerName";
            ddlPrimaryProducer.DataBind();
            ddlPrimaryProducer.Items.Insert(0, new ListItem("--- Select ---", "0"));


			CollectionManager.FillCollection(ddlSecondaryProducer, "SecondaryProduceId", "SecondaryProduceName", secondaryProducers);

		}

		private void bindState() {
			List<StateMaster> states = State.GetAll();
            
			CollectionManager.FillCollection(ddlState, "StateCode", "StateName", states);

			CollectionManager.FillCollection(ddlMailingState, "StateCode", "StateName", states);

			//CollectionManager.FillCollection(ddlFloodInsuranceState, "StateId", "StateName", states);

			//CollectionManager.FillCollection(ddlInsuranceState, "StateId", "StateName", states);

			//CollectionManager.FillCollection(ddlContactState, "StateId", "StateName", states);

			//CollectionManager.FillCollection(ddlCommercialInsuranceState, "StateId", "StateName", states);

			//CollectionManager.FillCollection(ddlEarthquakeInsuranceState, "StateId", "StateName", states);
		}

		private void bindDDL() {

			//IQueryable<AdjusterMaster> adjusters = null;
			IQueryable<ContractorMaster> contractors = null;
			IQueryable<AppraiserMaster> appraisers = null;
			IQueryable<UmpireMaster> umpires = null;
			List<LeadSourceMaster> sources = null;


			int clientID = SessionHelper.getClientId();

			if (clientID > 0) {
				//	adjusters = AdjusterManager.GetAll(clientID);
				contractors = ContractorManager.GetAll(clientID);
				appraisers = AppraiserManager.GetAll(clientID);
				umpires = UmpireManager.GetAll(clientID);

				sources = LeadSourceManager.GetAll(clientID);

			}
			else {
				//	adjusters = AdjusterManager.GetAll();
				contractors = ContractorManager.GetAll();
				appraisers = AppraiserManager.GetAll();
				umpires = UmpireManager.GetAll();
				sources = LeadSourceManager.GetAll();

			}



			if (appraisers != null)
				CollectionManager.FillCollection(ddlAppraiser, "AppraiserId", "AppraiserName", appraisers.ToList());

			CollectionManager.FillCollection(ddlContractor, "ContractorId", "ContractorName", contractors.ToList());
			CollectionManager.FillCollection(ddlUmpire, "UmpireId", "UmpireName", umpires.ToList());


			//CollectionManager.FillCollection(ddlLeadStatus, "StatusId", "StatusName", StatusManager.GetAll());

			//CollectionManager.FillCollection(ddlSubStatus, "SubStatusId", "SubStatusName", SubStatusManager.GetAll());

			//CollectionManager.FillCollection(ddlAdjuster, "AdjusterId", "AdjusterName", adjusters);

			CollectionManager.FillCollection(ddlLeadSource, "LeadSourceId", "LeadSourceName", sources);
			//CollectionManager.FillCollection(ddlInspectorName, "InspectorId", "InspectorName", InspectorManager.GetAll());

			//CollectionManager.FillCollection(ddlSiteInspection, "SiteInspectionCompleteId", "SiteInspectionCompleteName", SiteInspectionManager.GetAll());

			CollectionManager.FillCollection(ddlOwnerSame, "OwnerSameId", "OwnerSame", OwnerSameManager.GetAll());
			//CollectionManager.FillCollection(ddlWebFormSource, "WebformSourceId", "WebformSource", WebFormSourceManager.GetAll());
			//CollectionManager.FillCollection(ddlOtherSource, "OtherSourceId", "OtherSource", OtherSourceManager.GetAll());

			//Fillchk(chkTypeOfDamage, "TypeOfDamageId", "TypeOfDamage", TypeofDamageManager.GetAll());


			///CollectionManager.FillCollection(ddlTypeOfProperty, "TypeOfPropertyId", "TypeOfProperty", TypeOfPropertyManager.GetAll());
			//CollectionManager.FillCollection(ddlHabitable, "HabitableId", "Habitable", HabitableManager.GetAll());
			//CollectionManager.FillCollection(ddlWindPolicy, "WindPolicyId", "WindPolicy", WindPolicyManager.GetAll());
			//CollectionManager.FillCollection(ddlFloodPolicy, "FloodPolicyId", "FloodPolicy", FloodPolicyManager.GetAll());
			//CollectionManager.FillCollection(ddlRepotedInsurer, "ReportedToInsurerId", "ReportedToInsurer", ReportedToInsurerManager.GetAll());			
		}

		//protected void btnEmail_Click(object sender, EventArgs e) {
		//     Response.Redirect("~/protected/Admin/LeadEmail.aspx");
		//}

		//protected void btnExportLead_Click(object sender, EventArgs e) {
		//     Response.Redirect("~/Protected/Admin/ExportLead.aspx");
		//}

		// Save lead contact changes
		//protected void btnSaveContact_Click(object sender, EventArgs e) {
		//     int contactID = 0;
		//     int cityID = 0;
		//     int insuranceTypeID = 0;
		//     int stateID = 0;
		//     LeadContact contact = null;

		//     if (!string.IsNullOrEmpty(hfLeadId.Value)) {
		//          if (int.TryParse(hfContactID.Value, out contactID) && contactID > 0)
		//               contact = LeadContactManager.Get(contactID);
		//          else
		//               contact = new LeadContact();

		//          contact.ID = contactID;
		//          contact.LeadID = Convert.ToInt32(hfLeadId.Value);
		//          contact.ContactName = txtContactName.Text;
		//          contact.CompanyName = txtContactCompanyName.Text.Trim();
		//          contact.isActive = true;
		//          contact.Mobile = txtContactPhone.Text;
		//          contact.Email = txtContactEmail.Text;
		//          contact.ContactTypeID = Convert.ToInt32(ddlContactType.SelectedValue);
		//          contact.CompanyAddress = txtContactCompanyName.Text.Trim();

		//          int.TryParse(ddlContactCity.SelectedValue, out cityID);
		//          int.TryParse(ddlContactState.SelectedValue, out stateID);

		//          if (cityID > 0)
		//               contact.CityID = cityID;

		//          if (stateID > 0)
		//               contact.StateID = stateID;

		//          contact.ZipCode = ddlContactZipCode.SelectedValue;

		//          int.TryParse(ddlInsuranceType.SelectedValue, out insuranceTypeID);
		//          contact.InsuranceTypeID = insuranceTypeID;

		//          LeadContactManager.Save(contact);

		//          // refresh screen
		//          bindContacts();

		//          // clear field
		//          txtContactName.Text = "";
		//          txtContactPhone.Text = "";
		//          txtContactEmail.Text = "";
		//          ddlContactType.SelectedIndex = -1;
		//          hfContactID.Value = "0";
		//          txtContactCompanyName.Text = "";
		//          txtCompanyAddress.Text = "";
		//          ddlContactState.SelectedIndex = -1;
		//          ddlContactCity.SelectedIndex = -1;
		//          ddlContactZipCode.SelectedIndex = -1;
		//          ddlInsuranceType.SelectedIndex = -1;
		//     }
		//}

		protected void btnPhotoManagement_Click(object sender, EventArgs e) {
			Session["pageIndex"] = 0;
			string url = "~/protected/admin/LeadsImagesUpload.aspx";
			Response.Redirect(url);
		}

		protected void btnSaveAndContinue_Click(object sender, EventArgs e) {
			bool isDuplicate = false;
			Leads objLead = null;
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
			bool isNew = false;

			Page.Validate("NewLead");

			if (!Page.IsValid)
				return;

			try {
				if (ViewState["LeadIds"] == null) {
					// new lead/claim
					objLead = new Leads();

					objLead.UserId = Convert.ToInt32(Session["UserId"]);

					// 2013-07-19

					objLead.ClientID = Convert.ToInt32(Session["ClientId"]);

					objLead.DateSubmitted = DateTime.Now;
                    
					objLead.Status = 1;

					isNew = true;					
				}
				else {
					objLead = LeadsManager.GetByLeadId(Convert.ToInt32(ViewState["LeadIds"].ToString()));
				}

				//int claimsNumber = 0;

				objLead.Longitude = Convert.ToDouble(hf_Longitude.Value);
				objLead.Latitude = Convert.ToDouble(hf_Latitude.Value);

				DateTime? originalLeadDate = null;
				if (!string.IsNullOrEmpty(txtOriginalLeadDate.Text.Trim()))
					originalLeadDate = Convert.ToDateTime(txtOriginalLeadDate.Text.Trim());
				//DateTime.TryParse(txtOriginalLeadDate.Text.Trim(), out originalLeadDate);

				DateTime? lastContactDate = null;

				DateTime? siteSurveyDate = null;

				objLead.OriginalLeadDate = originalLeadDate;



				//DateTime? lossDate = null;
				//if (!string.IsNullOrEmpty(txtLossDate.Text.Trim()))
				//	lossDate = Convert.ToDateTime(txtLossDate.Text.Trim());
				//objLead.LossDate = lossDate;

				// 2013-10-29 tortega
				objLead.LossLocation = txtLossLocation.Text.Trim();

				//objLead.ClaimsNumber = claimsNumber;
				objLead.InsuredName = txtInsuredName.Text.Trim();

				objLead.ClaimantFirstName = txtClaimantName.Text.Trim(); ;
				objLead.ClaimantLastName = txtClaimantLastName.Text.Trim();
				objLead.ClaimantMiddleName = txtClaimantMiddleName.Text.Trim();
				objLead.Salutation = txtSalutation.Text.Trim();

				objLead.BusinessName = txtBusinessName.Text;

				objLead.SecondaryLeadSource = txtSecondaryLeadSource.Text;


				if (Convert.ToInt32(ddlUmpire.SelectedValue) != 0) {
					objLead.UmpireID = Convert.ToInt32(ddlUmpire.SelectedValue);
				}
				if (Convert.ToInt32(ddlContractor.SelectedValue) != 0) {
					objLead.ContractorID = Convert.ToInt32(ddlContractor.SelectedValue);
				}
				if (Convert.ToInt32(ddlAppraiser.SelectedValue) != 0) {
					objLead.AppraiserID = Convert.ToInt32(ddlAppraiser.SelectedValue);
				}

				// mailing address
				objLead.MailingAddress = txtMailingAddress.Text.Trim();
				objLead.MailingAddress2 = txtMailingAddress2.Text.Trim();
				objLead.MailingCity = txtMailingCity.Text.Trim();

				if (ddlMailingState.SelectedIndex > 0)
					objLead.MailingState = ddlMailingState.SelectedValue;

				objLead.MailingZip = txtMailingZip.Text.Trim();

				// Loss Address
				objLead.LossAddress = txtLossAddress.Text;
				objLead.LossAddress2 = txtLossAddress2.Text;

				if (ddlState.SelectedIndex > 0)
					objLead.StateName = ddlState.SelectedValue;

				objLead.CityName = txtCityName.Text;
				objLead.Zip = txtZipCode.Text;

				// check for duplicate name/address
				isDuplicate = LeadsManager.CheckForDuplicate(objLead);
				if (isDuplicate) {
					lblError.Text = "Duplicate clients not allowed.";
					lblError.Visible = true;
					return;
				}

				objLead.EmailAddress = txtEmail.Text;
				if (Convert.ToInt32(ddlLeadSource.SelectedValue) != 0) {
					objLead.LeadSource = Convert.ToInt32(ddlLeadSource.SelectedValue);
				}
				if (Convert.ToInt32(ddlPrimaryProducer.SelectedValue) != 0) {
					objLead.PrimaryProducerId = Convert.ToInt32(ddlPrimaryProducer.SelectedValue);
				}
				if (Convert.ToInt32(ddlSecondaryProducer.SelectedValue) != 0) {
					objLead.SecondaryProducerId = Convert.ToInt32(ddlSecondaryProducer.SelectedValue);
				}

				objLead.PhoneNumber = txtPhoneNumber.Text;
                objLead.MobilePhone = txtMobilePhone.Text;
				
                //string Damagetxt = string.Empty;
				//string DamageId = string.Empty;
				//for (int j = 0; j < chkTypeOfDamage.Items.Count; j++) {
				//	if (chkTypeOfDamage.Items[j].Selected == true) {
				//		Damagetxt += chkTypeOfDamage.Items[j].Text + ',';
				//		DamageId += chkTypeOfDamage.Items[j].Value + ',';
				//	}
				//}
				//if (Damagetxt != string.Empty && DamageId != string.Empty) {
				//	objLead.TypeOfDamage = DamageId;
				//	objLead.TypeofDamageText = Damagetxt;
				//}

				//if (Convert.ToInt32(ddlTypeOfProperty.SelectedValue) != 0) {
				//	objLead.TypeOfProperty = Convert.ToInt32(ddlTypeOfProperty.SelectedValue);
				//}

				

				objLead.IsSubmitted = true;
			


				objLead.SiteSurveyDate = siteSurveyDate;
				objLead.LastContactDate = lastContactDate;
				if (Convert.ToInt32(ddlOwnerSame.SelectedValue) != 0) {
					objLead.OwnerSame = Convert.ToInt32(ddlOwnerSame.SelectedValue);
				}
				objLead.OwnerFirstName = txtOwnerFirstName.Text.Trim();
				objLead.OwnerLastName = txtOwnerLastName.Text.Trim();
				objLead.OwnerPhone = txtOwnerPhone.Text.Trim();

				// 2013-03-11 tortega added owner email
				objLead.OwnerEmail = txtOwnerEmail.Text.Trim();

				objLead.SecondaryEmail = txtSecondaryEmail.Text.Trim();
				objLead.SecondaryPhone = txtSecondaryPhone.Text.Trim();
                //save new fields
                if (!string.IsNullOrEmpty(txtBirthdate.Text))
                {
                objLead.Birthdate =Convert.ToDateTime(txtBirthdate.Text);
                }
                objLead.Reference = txtReference.Text;
                objLead.Languages = txtLanguage.Text;
                objLead.Title = txtTitle.Text;
                objLead.LossCounty = txtlossCountry.Text;
                objLead.MailingCounty = txtMailingCountry.Text;
               
                Leads ins = LeadsManager.Save(objLead);
               

				if (ins.LeadId > 0) {

               int categoryId = ContactManager.GetContactId("Claimant");;
               Contact objcontact = ContactManager.GetLeadContact(objLead.LeadId, categoryId);
               string[] name = txtThirdPartyName.Text.Split(' ');
               int nameLength = name.Length;
               string firstName = string.Empty;
               string lastName = string.Empty;
               firstName = firstName.Trim();
               if (nameLength > 1)
               {
                   for (int count = 0; count <= nameLength - 2; count++)
                   {
                       firstName = firstName + " " + name[count];
                   }
                   lastName = name[nameLength - 1];
               }
               else
               {
                   firstName =name[0];
               }
               
                    Contact contact = null;
                    contact = new Contact();
                    if (objcontact != null)
                    {
                        contact.ContactID = objcontact.ContactID;
                    }
                    contact.LeadId = ins.LeadId;
                    contact.ClientID = SessionHelper.getClientId();
                    contact.UserID = SessionHelper.getUserId();
                    contact.IsActive = true;
                    contact.FirstName = firstName.Trim();
                    contact.LastName = lastName.Trim();
                    contact.ContactName = firstName.Trim() + " " + lastName.Trim(); ;
                    contact.Address1 = txtThirdPartyStreet.Text;
                    contact.CityName = txtThirdPartyCity.Text;
                    contact.State = txtThirdPartyState.Text;
                    contact.ZipCode = txtThirdPartyPostalCodes.Text;
                    contact.Phone = txtThirdPartyPhoneNumber.Text;

                    contact.CategoryID = categoryId;
                    if (objcontact == null)
                    {
                    contact = ContactManager.SaveContact(contact);
                    //Contact contactupdate = new Contact();
                    //contactupdate.ContactID = contact.ContactID;
                    //contactupdate.ContactName = contact.ContactName;
                    //contact = ContactManager.UpdateContact(contactupdate);     
                    }
                    else
                    {
                        contact = ContactManager.Update(contact);  
                    }



					Session["LeadIds"] = ins.LeadId;

					ViewState["LeadIds"] = ins.LeadId;

					//savePolicy();

					//SaveLeadLog(objLead);

					showControls(true);

					lblSave.Text = "Claim/Lead information saved successfully.";

					lblSave.Visible = true;

					//// repopulate policy info after saving a new lead
					//if (isNew) {
					//	fillPolicy(ins.LeadId);
					//}


				}
			}
			catch (Exception ex) {
				lblError.Text = "Your update could not be saved for the following reason(s):<br />" + ex.ToString();
				lblError.Visible = true;

			}
		}

		//private void SaveLeadLog(Lead ins) {
		//	LeadLog objLeadlog = new LeadLog();
		//	objLeadlog.UserId = Convert.ToInt32(Session["UserId"]);
		//	objLeadlog.LeadId = ins.LeadId;
		//	//objLeadlog.LFUUID = txtLFUUId.Text;
		//	objLeadlog.OriginalLeadDate = ins.OriginalLeadDate;

		//	objLeadlog.ClaimsNumber = ins.ClaimsNumber;
		//	objLeadlog.ClaimantFirstName = txtClaimantName.Text;
		//	objLeadlog.ClaimantLastName = txtClaimantLastName.Text;
		//	objLeadlog.BusinessName = txtBusinessName.Text;
		//	//objLeadlog.InsuranceCompanyName = txtInsuranceCompanyName.Text;
		//	//objLeadlog.InsuranceContactName = txtInsuranceContactName.Text;
		//	//objLeadlog.InsuranceContactPhone = txtInsuranceContactPhone.Text;
		//	//objLeadlog.InsuranceContactEmail = txtInsuranceEmail.Text;
		//	//objLeadlog.InsuranceAddress = txtInsuranceAddress.Text;
		//	objLeadlog.InsuranceState = ins.InsuranceState;

		//	objLeadlog.InsuranceCity = ins.InsuranceCity;

		//	objLeadlog.InsuranceZipCode = ins.InsuranceZipCode;

		//	//objLeadlog.InsurancePolicyNumber = txtInsurancePolicyNumber.Text;
		//	objLeadlog.LeadStatus = ins.LeadStatus;


		//	objLeadlog.EmailAddress = txtEmail.Text;
		//	//if (Convert.ToInt32(ddlAdjuster.SelectedValue) != 0) {
		//	//     objLeadlog.Adjuster = Convert.ToInt32(ddlAdjuster.SelectedValue);
		//	//}
		//	if (Convert.ToInt32(ddlLeadSource.SelectedValue) != 0) {
		//		objLeadlog.LeadSource = Convert.ToInt32(ddlLeadSource.SelectedValue);
		//	}
		//	if (Convert.ToInt32(ddlPrimaryProducer.SelectedValue) != 0) {
		//		objLeadlog.PrimaryProducerId = Convert.ToInt32(ddlPrimaryProducer.SelectedValue);
		//	}
		//	if (Convert.ToInt32(ddlSecondaryProducer.SelectedValue) != 0) {
		//		objLeadlog.SecondaryProducerId = Convert.ToInt32(ddlSecondaryProducer.SelectedValue);
		//	}

		//	objLeadlog.PhoneNumber = txtPhoneNumber.Text;

		//	//objLeadlog.ClaimantComments = txtClaimantComments.Text;

		//	objLeadlog.TypeOfDamage = ins.TypeOfDamage;
		//	objLeadlog.TypeofDamageText = ins.TypeofDamageText;

		//	if (Convert.ToInt32(ddlTypeOfProperty.SelectedValue) != 0) {
		//		objLeadlog.TypeOfProperty = Convert.ToInt32(ddlTypeOfProperty.SelectedValue);
		//	}

		//	objLeadlog.LossAddress = txtLossAddress.Text;
		//	//if (Convert.ToInt32(ddlState.SelectedValue) > 0)
		//	//	objLeadlog.StateId = Convert.ToInt32(ddlState.SelectedValue);

		//	//if (Convert.ToInt32(ddlCity.SelectedValue) > 0)
		//	//	objLeadlog.CityId = Convert.ToInt32(ddlCity.SelectedValue);

		//	//if (Convert.ToInt32(ddlLossZip.SelectedValue) > 0)
		//	//	objLeadlog.Zip = ddlLossZip.SelectedItem.Text;


		//	objLeadlog.IsSubmitted = true;
		//	objLeadlog.Status = 1;
		//	/*New Fields*/
		//	//if (Convert.ToInt32(ddlSubStatus.SelectedValue) != 0) {
		//	//     objLeadlog.SubStatus = Convert.ToInt32(ddlSubStatus.SelectedValue);// txtSubStatus.Text.Trim();
		//	//}
		//	objLeadlog.SiteSurveyDate = ins.SiteSurveyDate;
		//	//objLeadlog.ClaimantAddress = txtClaimantAdd.Text.Trim();
		//	objLeadlog.LastContactDate = ins.LastContactDate;
		//	if (Convert.ToInt32(ddlOwnerSame.SelectedValue) != 0) {
		//		objLeadlog.OwnerSame = Convert.ToInt32(ddlOwnerSame.SelectedValue);
		//	}
		//	objLeadlog.OwnerFirstName = txtOwnerFirstName.Text.Trim();
		//	objLeadlog.OwnerLastName = txtOwnerLastName.Text.Trim();
		//	objLeadlog.OwnerPhone = txtOwnerPhone.Text.Trim();
		//	objLeadlog.SecondaryEmail = txtSecondaryEmail.Text.Trim();
		//	objLeadlog.SecondaryPhone = txtSecondaryPhone.Text.Trim();
		//	//if (Convert.ToInt32(ddlSiteInspection.SelectedValue) != 0) {
		//	//     objLeadlog.SiteInspectionCompleted = Convert.ToInt32(ddlSiteInspection.SelectedValue);
		//	//}
		//	LeadLog insLog = LeadsManager.SaveLeadLog(objLeadlog);
		//}

		protected void btnRefreshTasks_Click(object sender, EventArgs e) {
			btnSaveAndContinue_Click(sender, e);
		}

		//protected void btnRefreshTasksFromCalendar_Click(object sender, EventArgs e) {
		//	DateTime fromDate = DateTime.Now;

		//	DateTime.TryParse(hf_taskDate.Value, out fromDate);

		//	bindTasks(fromDate.Date, fromDate.Date);
		//}

		private void controlReadOnly() {
			//txtLFUUId.ReadOnly = true;
			txtOriginalLeadDate.ReadOnly = true;
			txtSalutation.ReadOnly = true;
			txtInsuredName.ReadOnly = true;
			txtClaimantName.ReadOnly = true;
			txtClaimantMiddleName.ReadOnly = true;
			//ddlLeadStatus.Enabled = false;
			txtEmail.ReadOnly = true;
			//ddlAdjuster.Enabled = false;
			ddlLeadSource.Enabled = false;
			ddlPrimaryProducer.Enabled = false;
			ddlSecondaryProducer.Enabled = false;
			//ddlInspectorName.Enabled = false;
			//txtInspectorCell.ReadOnly = true;
			//txtInspectorEmail.ReadOnly = true;
			txtPhoneNumber.ReadOnly = true;

			txtMailingAddress.Enabled = false;
			txtMailingAddress2.ReadOnly = true;
			ddlMailingState.Enabled = false;
			txtMailingCity.Enabled = false;
			txtMailingZip.Enabled = false;

			//txtClaimantComments.ReadOnly = true;
			//txtEditClaimantComments.ReadOnly = true;
			//chkTypeOfDamage.Enabled = false;
			//ddlTypeOfProperty.Enabled = false;
			//txtMarketValue.ReadOnly = true;
			ddlState.Enabled = false;
			txtCityName.ReadOnly = true;
			txtLossAddress.ReadOnly = true;
			txtLossAddress2.ReadOnly = true;
			txtZipCode.ReadOnly = true;

			txtLossLocation.ReadOnly = true;
			//ddlState.Enabled = false;
			//ddlCity.Enabled = false;
			//txtPropertyDamageEstimate.ReadOnly = true;
			//ddlWindPolicy.Enabled = false;
			//ddlFloodPolicy.Enabled = false;
			//ddlRepotedInsurer.Enabled = false;
			//ddlHabitable.Enabled = false;

			//btnGenerateReport.Visible = false;
			//btnSaveAndContinue.Visible = false;

			lblHead.Text = "View Record";
			//dvCalender.Visible = false;
			//dvCalenderSurveyDate.Visible = false;
			//dvCalenderContactDate.Visible = false;

			//btnUpload.Visible = false;
			//ddlSubStatus.Enabled = false;
			//txtSiteSurvey.ReadOnly = true;
			//txtClaimantAdd.ReadOnly = true;
			//txtLastContactDate.ReadOnly = true;
			ddlOwnerSame.Enabled = false;
			txtOwnerFirstName.ReadOnly = true;
			txtOwnerLastName.ReadOnly = true;
			txtOwnerPhone.ReadOnly = true;
			txtSecondaryEmail.ReadOnly = true;
			txtSecondaryPhone.ReadOnly = true;
			//ddlSiteInspection.Enabled = false;
			//ddlOtherSource.Enabled = false;
			txtClaimantLastName.ReadOnly = true;
			txtBusinessName.ReadOnly = true;

			ddlAppraiser.Enabled = false;
			ddlContractor.Enabled = false;
			ddlUmpire.Enabled = false;
			//btnSaveAndContinue.ValidationGroup = "";
			//btnUpload.ValidationGroup = "";
			//txtInsuranceCompanyName.ReadOnly = true;
			//txtInsuranceContactName.ReadOnly = true;
			//txtInsuranceContactPhone.ReadOnly = true;
			//txtInsuranceEmail.ReadOnly = true;
			//txtInsuranceAddress.ReadOnly = true;
			//ddlInsuranceState.Enabled = false;
			//ddlInsuranceCity.Enabled = false;
			//txtInsurancePolicyNumber.ReadOnly = true;

			//btnComments.Visible = false;
			//btnComments.ValidationGroup = "";
		}

		protected void ddlState_SelectedIndexChanged(object sender, EventArgs e) {
			//if (ddlState.SelectedIndex > 0) {
			//	CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll(Convert.ToInt32(ddlState.SelectedValue)));
			//}
			//else {
			//	ddlCity.Items.Clear();
			//}
		}

		protected void dllCity_SelectedIndexChanged(object sender, EventArgs e) {
			//if (ddlCity.SelectedIndex > 0) {
			//	CollectionManager.FillCollection(ddlLossZip, "ZipCodeID", "ZipCode", ZipCode.getByCityID(Convert.ToInt32(ddlCity.SelectedValue)));
			//}
			//else {
			//	ddlLossZip.Items.Clear();
			//}

		}


		public static void Fillchk(ListControl control, string key, string value, object data) {
			control.DataSource = data;
			control.DataValueField = key;
			control.DataTextField = value;
			control.DataBind();

		}

		private void FillForm(string leadId, string view) {
			int roleID = SessionHelper.getUserRoleId();

			if (leadId != null && leadId.Trim().Length > 0) {
				//hfLeadId.Value = leadId;
			}
			Leads objLead = LeadsManager.GetByLeadId(Convert.ToInt32(ViewState["LeadIds"].ToString()));

			if (objLead.LeadId > 0) {
				// 2013-03-11 tortega for ucLeadComments.ascx
				Session["LeadIds"] = objLead.LeadId;

				// save claimant name to display in subsuquent pages
				// remove these these in the near future
				Session["ClaimantFirstName"] = objLead.ClaimantFirstName == null ? "" : objLead.ClaimantFirstName.ToString();
				Session["ClaimantLastName"] = objLead.ClaimantLastName == null ? "" : objLead.ClaimantLastName.ToString();

				// use this in the future
				Session["InsuredName"] = objLead.InsuredName == null ? "" : objLead.InsuredName;

				lblLastActivityDate.Text = objLead.LastActivityDate == null ? "" : objLead.LastActivityDate.ToString();

				//lblLeadId.Text = objLead.LeadId.ToString();
				//lblDateSubmited.Text = objLead.DateSubmitted == null ? "" : Convert.ToDateTime(objLead.DateSubmitted).ToString("MM/dd/yyyy");
				//txtLFUUId.Text = objLead.LFUUID == null ? "" : objLead.LFUUID.ToString();
				txtOriginalLeadDate.Text = objLead.OriginalLeadDate == null ? "" : Convert.ToDateTime(objLead.OriginalLeadDate).ToString("MM/dd/yyyy");

				//txtLossDate.Text = objLead.LossDate == null ? "" : Convert.ToDateTime(objLead.LossDate).ToString("MM/dd/yyyy");

				txtLossLocation.Text = objLead.LossLocation;

				txtSecondaryLeadSource.Text = objLead.SecondaryLeadSource;
              
				txtInsuredName.Text = objLead.InsuredName;
                //fill new control
                if (objLead.Birthdate!=null)
                {
                    txtBirthdate.Text = Convert.ToString(objLead.Birthdate);
                }
                txtReference.Text = objLead.Reference;
                txtLanguage.Text = objLead.Languages;
                txtTitle.Text = objLead.Title;
                txtlossCountry.Text = objLead.LossCounty;
                txtMailingCountry.Text = objLead.MailingCounty;

				//txtClaimsNumber.Text = objLead.ClaimsNumber.ToString();
				txtClaimantName.Text = objLead.ClaimantFirstName == null ? "" : objLead.ClaimantFirstName.ToString();
				txtClaimantLastName.Text = objLead.ClaimantLastName == null ? "" : objLead.ClaimantLastName.ToString();
				txtSalutation.Text = objLead.Salutation;
				txtClaimantMiddleName.Text = objLead.ClaimantMiddleName;

				txtBusinessName.Text = objLead.BusinessName == null ? "" : objLead.BusinessName.ToString();

				txtEmail.Text = objLead.EmailAddress == null ? "" : objLead.EmailAddress.ToString();
				ddlLeadSource.SelectedValue = objLead.LeadSource == null ? "0" : objLead.LeadSource.ToString();
				ddlPrimaryProducer.SelectedValue = objLead.PrimaryProducerId.ToString();
				ddlSecondaryProducer.SelectedValue = objLead.SecondaryProducerId == null ? "0" : objLead.SecondaryProducerId.ToString();
				txtPhoneNumber.Text = objLead.PhoneNumber == null ? "" : objLead.PhoneNumber.ToString();
                txtMobilePhone.Text = objLead.MobilePhone == null ? "" : objLead.MobilePhone.ToString();

				//string damage = objLead.TypeOfDamage == null ? "0" : objLead.TypeOfDamage.ToString();
				//if (damage != null) {
				//	string[] damg = damage.Split(',');
				//	for (int i = 0; i < damg.Length - 1; i++) {
				//		for (int j = 0; j < chkTypeOfDamage.Items.Count; j++) {
				//			if (chkTypeOfDamage.Items[j].Value == damg[i].ToString()) {
				//				chkTypeOfDamage.Items[j].Selected = true;
				//			}
				//		}
				//	}
				//}


				///ddlTypeOfProperty.SelectedValue = objLead.TypeOfProperty == null ? "0" : objLead.TypeOfProperty.ToString();
				//txtMarketValue.Text = objLead.MarketValue == null ? "" : objLead.MarketValue.ToString();
				txtLossAddress.Text = objLead.LossAddress == null ? "" : objLead.LossAddress.ToString();
				txtLossAddress2.Text = objLead.LossAddress2 == null ? "" : objLead.LossAddress2.ToString();


				if (objLead.AppraiserID != null)
					ddlAppraiser.SelectedValue = objLead.AppraiserID.ToString();

				if (objLead.ContractorID != null)
					ddlContractor.SelectedValue = objLead.ContractorID.ToString();

				if (objLead.UmpireID != null)
					ddlUmpire.SelectedValue = objLead.UmpireID.ToString();

				txtCityName.Text = objLead.CityName;

				if (!string.IsNullOrEmpty(objLead.StateName)) {
					setLossState(objLead.StateName);
				}

				//if (!string.IsNullOrEmpty(objLead.StateName))
				//	ddlState.SelectedValue = objLead.StateName;

				txtZipCode.Text = objLead.Zip;


				txtMailingAddress.Text = objLead.MailingAddress;
				txtMailingAddress2.Text = objLead.MailingAddress2;
				txtMailingCity.Text = objLead.MailingCity;

				if (!string.IsNullOrEmpty(objLead.MailingState))
					ddlMailingState.SelectedValue = objLead.StateName;

				txtMailingZip.Text = objLead.MailingZip;


                lblHead.Text = "Insured Details";

				//if (objLead.IsSubmitted != null && objLead.IsSubmitted == true) {
				//     btnGenerateReport.Visible = true;
				//}

				ddlOwnerSame.SelectedValue = objLead.OwnerSame == null ? "0" : objLead.OwnerSame.ToString();
				txtOwnerFirstName.Text = objLead.OwnerFirstName == null ? "" : objLead.OwnerFirstName.ToString();
				txtOwnerLastName.Text = objLead.OwnerLastName == null ? "" : objLead.OwnerLastName.ToString();
				txtOwnerPhone.Text = objLead.OwnerPhone == null ? "" : objLead.OwnerPhone.ToString();

				// 2013-03-11 tortega added owner email
				txtOwnerEmail.Text = objLead.OwnerEmail == null ? "" : objLead.OwnerEmail;

				txtSecondaryEmail.Text = objLead.SecondaryEmail == null ? "" : objLead.SecondaryEmail.ToString();
				txtSecondaryPhone.Text = objLead.SecondaryPhone == null ? "" : objLead.SecondaryPhone.ToString();

                int categoryId = ContactManager.GetContactId("Claimant");
               Contact objcontact = ContactManager.GetLeadContact(objLead.LeadId, categoryId);
               if (objcontact!=null)
                {
                    if (!string.IsNullOrEmpty(objcontact.ContactName))
                    {
                        txtThirdPartyName.Text = objcontact.ContactName;
                    }
                    else
                    {
                        txtThirdPartyName.Text = objcontact.FirstName.Trim()+" "+ objcontact.LastName.Trim() ;
                    }
             
               txtThirdPartyStreet.Text = objcontact.Address1;
               txtThirdPartyCity.Text = objcontact.CityName;
               txtThirdPartyState.Text = objcontact.State;
               txtThirdPartyPostalCodes.Text = objcontact.ZipCode;
               txtThirdPartyPhoneNumber.Text = objcontact.Phone;
                }

			}
			else {
				// empty lead
				objLead.OriginalLeadDate = DateTime.Now;
			}


			if (view != null && view.Trim().Length > 0) {
				controlReadOnly();
				//btnUpload.Visible = true;
				//btnUpload.Text = "View Photos And Docs";
				//btnComments.Visible = true;

			}

		}


		private void setLossState(string stateValue) {
			// find state by "FL"
			ListItem stateItem = ddlState.Items.FindByValue(stateValue);
			if (stateItem != null)
				ddlState.SelectedValue = stateValue;
			else {
				// find state by name 
				stateItem = ddlState.Items.FindByText(stateValue);
				if (stateItem != null)
					ddlState.SelectedIndex = ddlState.Items.IndexOf(stateItem);
			}
		}

		protected void generatePhotoReport() {
			bool isSuccess = true;
			string filename1 = null;

			// 2013-03-12 tortega
			//Page.Validate();
			//if (!Page.IsValid)
			//	return;

			int LeadId = SessionHelper.getLeadId();

			if (LeadId > 0) {

				try {

					// 2013-09-26 tortega - Mike Benhart asked to remove this
					// check all location/description have been entered prior to print
					//isBlankDescription = LeadsUploadManager.checkForLocationDescriptionBlank(LeadId);
					//if (isBlankDescription) {
					//     lblError.Text = "Image locations/descriptions are required prior to printing report.";
					//     lblError.Visible = true;
					//     return;
					//}

					filename1 = CreatePDF.CreateAndGetPDF(LeadId, Request.PhysicalApplicationPath + "PDF\\", out ErrorMessage);


					//LeadReportGenerateLog objLeadReportGenerateLog = new LeadReportGenerateLog();
					//objLeadReportGenerateLog.LeadId = Convert.ToInt32(ViewState["LeadIds"].ToString());
					//objLeadReportGenerateLog.GenerateDate = DateTime.Now;
					//objLeadReportGenerateLog.Generatedby = Convert.ToInt32(Session["UserId"]);
					//LeadReportLogManager.Save(objLeadReportGenerateLog);

					// 2013-03-21 tortega - moved out of try/catch - causes exception
					//OpenNewWindow(LeadId);
				}
				catch (Exception ex) {
					lblError.Text = "Report Not Generated.";
					lblError.Visible = true;

					Core.EmailHelper.emailError(ex);

					isSuccess = false;
				}

				// show report when success
				if (isSuccess)
					OpenNewWindow(LeadId);
			}
		}

		protected void gvTasks_onSorting(object sender, GridViewSortEventArgs e) {
			System.Web.UI.WebControls.GridView gridView = sender as System.Web.UI.WebControls.GridView;
			int userID = 0;

			IQueryable<LeadTask> tasks = null;

			int roleID = SessionHelper.getUserRoleId();

			int clientID = SessionHelper.getClientId();

			DateTime fromDate = DateTime.Today;
			DateTime toDate = DateTime.Today;

			if (roleID == (int)UserRole.Administrator) {
				// load tasks for user "Admin". Admin has not clientid associated with it.
				tasks = TasksManager.GetLeadTask(fromDate, toDate);
			}
			else if ((roleID == (int)UserRole.Client || roleID == (int)UserRole.SiteAdministrator) && clientID > 0) {
				// load all tasks for client (sort of admin)				
				tasks = TasksManager.GetLeadTask(clientID, fromDate, toDate);
			}
			else {
				userID = SessionHelper.getUserId();
				tasks = TasksManager.GetLeadTaskByUserID(userID, fromDate, toDate);
			}

			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			gridView.DataSource = tasks.orderByExtension(e.SortExpression, descending);

			gridView.DataBind();

		}





		public void OpenNewWindow(int LeadId) {
			string url = Request.PhysicalApplicationPath + "PDF\\" + LeadId + ".pdf";
			if (File.Exists(url)) {

				string FileName = LeadId + ".pdf";
				Response.ContentType = "Application/pdf";
				Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + " ");
				Response.TransmitFile(url);
				Response.End();
			}
		}


		// this is called from ucPolicy.ascx upon Status is changed.
		protected void policy_statusChanged(object sender, EventArgs e) {
			bindTasks();
			//string js = "<script type='text/javascript'>refreshTasks();</script>";

			//Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "jstaks", js);


		}

		private void savePolicy() {
			policyEditForm.saveForm();
		}


		private void showControls(bool isVisible) {


			tabPanelPolicies.Visible = isVisible;

			//tpnlFlood.Visible = isVisible;
			//tpnlEarthquake.Visible = isVisible;

		}

		protected void webTaskPanel_ItemClick(object sender, ExplorerBarItemClickEventArgs e) {
			if (e.Item.Text == "Print Photo Report")
				generatePhotoReport();
		}

		protected void TaskCalendar_SelectedDateChanged(object sender, Infragistics.Web.UI.EditorControls.CalendarSelectedDateChangedEventArgs e) {
			DateTime fromDate = e.NewDate;
			DateTime toDate = Convert.ToDateTime(string.Format("{0} 11:59:59 PM", e.NewDate.ToShortDateString()));

			bindTasks(fromDate.Date, toDate);
		}









		#region ************************** Policy List Methods **************************
		//protected void bindPolicyList() {
		//	int leadID = SessionHelper.getLeadId();

		//	gvPolicyList.DataSource = LeadPolicyManager.GetPolicies(leadID);

		//	gvPolicyList.DataBind();

		//}

		protected void btnPolicyList_Click(object sender, EventArgs e) {
			pnlPolicyList.Visible = true;
			pnlPolicyEdit.Visible = false;

			Session.Remove("policyID");

		}





		protected void btnNewPolicy_Click(object sender, EventArgs e) {
            if (HttpContext.Current.Session["Limit"] != null && HttpContext.Current.Session["PolicyLimit"] != null && HttpContext.Current.Session["tblCasulityPolicylimit"] != null && HttpContext.Current.Session["tblAllPolicylimit"] != null)
            {
                HttpContext.Current.Session["Limit"] = null;
                HttpContext.Current.Session["PolicyLimit"] = null;
                HttpContext.Current.Session["tblCasulityPolicylimit"] = null;
                HttpContext.Current.Session["tblAllPolicylimit"] = null;

                
            }


			Session.Remove("policyID");

			//pnlPolicyList.Visible = false;
			//pnlPolicyEdit.Visible = true;

			//policyEditForm.bindPolicyData();




			Response.Redirect("~/Protected/LeadPolicyEdit.aspx");
		}
		#endregion

		protected void btnNewPolicy_Command(object sender, CommandEventArgs e) {
			Session["policyID"] = 0;

			Response.Redirect("~/Protected/LeadPolicyEdit.aspx");
		}







	}
}