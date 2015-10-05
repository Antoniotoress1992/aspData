using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace CRM.Web.Protected.Admin {
	public partial class ImportData : System.Web.UI.Page {

		private List<ImportField> claimRulerImportFields = null;
		private List<ImportMapField> mappedFields = null;

		protected void Page_Load(object sender, EventArgs e) {

		}

		// method to handle "Next" sstep button
		protected void Wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e) {

			lblMessage.Text = string.Empty;
			lblMessage.CssClass = "";

			switch (e.CurrentStepIndex) {
				case 1:	// user selected file to upload and clicked next

					loadFieldsFromCSVFile();
					break;

				default:
					break;
			}
		}

		protected void gvClaimFieldMap_RowDataBound(object sender, GridViewRowEventArgs e) {
			DropDownList ddlClaimRulerFields = null;
			Label lblYourFieldName = null;
			string yourFieldName = null;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				ddlClaimRulerFields = e.Row.FindControl("ddlClaimRulerFields") as DropDownList;

				Core.CollectionManager.FillCollection(ddlClaimRulerFields, "FieldName", "FieldName", claimRulerImportFields);

				lblYourFieldName = e.Row.FindControl("lblYourFieldName") as Label;

				yourFieldName = lblYourFieldName.Text;

				if (ddlClaimRulerFields.Items != null && ddlClaimRulerFields.Items.Count > 0 && !string.IsNullOrEmpty(yourFieldName)) {
					// try to mathc user's field name with ours
					ListItem listItem = ddlClaimRulerFields.Items.FindByValue(yourFieldName);
					if (listItem != null)
						ddlClaimRulerFields.SelectedIndex = ddlClaimRulerFields.Items.IndexOf(listItem);
				}
			}
		}

		private List<ImportMapField> getMappedFields() {
			List<ImportMapField> mappedFields = new List<ImportMapField>();
			ImportMapField mappedField = null;
			DropDownList ddlClaimRulerFields = null;
			Label lblYourFieldName = null;

			foreach (GridViewRow row in gvClaimFieldMap.Rows) {
				if (row.RowType == DataControlRowType.DataRow) {
					ddlClaimRulerFields = row.FindControl("ddlClaimRulerFields") as DropDownList;

					lblYourFieldName = row.FindControl("lblYourFieldName") as Label;

					if (ddlClaimRulerFields.SelectedIndex > 0) {
						// add those fields mapped by user only
						mappedField = new ImportMapField();

						mappedField.UserFieldName = lblYourFieldName.Text;
						mappedField.ClaimRulerFieldName = ddlClaimRulerFields.SelectedValue;

						mappedFields.Add(mappedField);
					}
				}
			}

			return mappedFields;
		}
		private void loadFieldsFromCSVFile() {
			string fileExtension = null;
			string filepath = null;
			ImportMapField[] importFileFields = null;

			if (fileUpload.HasFile) {
				fileExtension = System.IO.Path.GetExtension(fileUpload.PostedFile.FileName);
				// validate file
				if (fileExtension.ToLower() != ".csv") {
					lblMessage.Text = ".CSV file allowed only.";
					lblMessage.CssClass = "error";
					wizardImport.ActiveStepIndex = 1;
				}

				// create temp folder
				if (!Directory.Exists(Server.MapPath("~//Temp"))) {
					Directory.CreateDirectory(Server.MapPath("~//Temp"));
				}

				filepath = Server.MapPath("~//Temp//" + fileUpload.FileName);

				fileUpload.SaveAs(filepath);

				// save filename
				ViewState["fileName"] = fileUpload.FileName;

				importFileFields = Core.CSVHelper.readHeaders(filepath);

				if (importFileFields != null) {
					// load claim ruler fields
					using (ImportFieldsManager repository = new ImportFieldsManager()) {
						claimRulerImportFields = repository.GetAll();
					}

					// bind fields to grid
					gvClaimFieldMap.DataSource = importFileFields;
					gvClaimFieldMap.DataBind();
				}
			}
		}

		private void processClaimData() {
			Claim claim = null;
			CRM.Data.Entities.LeadPolicy policy = null;
			Leads lead = null;

			DataTable importTable = null;
			lblMessage.Text = string.Empty;
			lblMessage.CssClass = string.Empty;

			// intial phase
			try {
				// load file for processing
				string filepath = Server.MapPath("~//Temp//" + ViewState["fileName"].ToString());

				// load csv file into a table
				importTable = CSVHelper.ReadCSVFile(filepath);

				// get collection of mapped fields
				mappedFields = getMappedFields();
			}
			catch (Exception ex) {
				lblMessage.Text = "Initial phase of failed.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
				return;
			}

			// second phase - process each record
			using (TransactionScope scope = new TransactionScope()) {
				try {
					foreach (DataRow dataRow in importTable.Rows) {
						lead = createLead(dataRow);

						if (lead == null)
							continue;

						policy = createPolicy(lead.LeadId, dataRow);

						claim = createClaim(policy.Id, dataRow);
					}

					scope.Complete();

					lblMessage.Text = "Import completed successfully.";
					lblMessage.CssClass = "ok";
				}
				catch (Exception ex) {
					lblMessage.Text = "Import not completed successfully.";
					lblMessage.CssClass = "error";

					Core.EmailHelper.emailError(ex);
				}
			}

		}

		private Claim createClaim(int policyID, DataRow dataRow) {
			decimal amount = 0;
			AdjusterMaster adjuster = null;
			string adjusterName = null;
			string causeofLoss = null;
			Claim claim = null;
			string claimstatus = null;
			int clientID = SessionHelper.getClientId();
			Contact contact = null;
			DateTime dateTime = DateTime.MinValue;
			LeadContactType contactType = null;
			string ownerManagerName = null;
			string ownerManagerEntityName = null;
			int severity = 0;
			string supervisorName = null;
			string teamLead = null;
			TypeOfDamageMaster damageType = null;
			int userID = SessionHelper.getUserId();
			string userFieldName = null;

			claim = new Claim();
			claim.PolicyID = policyID;
			claim.IsActive = true;

			// Insurer Claim ID
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Insurer Claim ID");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				claim.InsurerClaimNumber = dataRow[userFieldName].ToString().Trim();
				claim.AdjusterClaimNumber = claim.InsurerClaimNumber;
			}

			// Adjuster Claim Number
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Adjuster Claim Number");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				claim.AdjusterClaimNumber = dataRow[userFieldName].ToString().Trim();	
			}

			// Claim Status
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Claim Status");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				claimstatus = dataRow[userFieldName].ToString().Trim();
				if (!string.IsNullOrEmpty(claimstatus)) {
					StatusMaster statusMaster = StatusManager.GetByStatusName(clientID, claimstatus);
					if (statusMaster.StatusId == 0) {
						statusMaster.StatusName = claimstatus;
						statusMaster.clientID = clientID;
						statusMaster.Status = true;
						statusMaster = StatusManager.Save(statusMaster);
					}
					claim.StatusID = statusMaster.StatusId;
				}
			}

			#region Dates
			// Date of Loss
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date of Loss");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.LossDate = dateTime;
			}

			// Date Reported
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Reported");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateOpenedReported = dateTime;
			}

			// Date First Closed
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date First Closed");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateFirstClosed = dateTime;
			}

			// Date First Reopen
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date First Reopen");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateFirstReOpened = dateTime;
			}
			// Date Assigned
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Assigned");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateAssigned = dateTime;
			}
			// Date Acknowledged
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Acknowledged");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateAcknowledged = dateTime;
			}
			// Date First Contact Attempted
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date First Contact Attempted");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateFirstContactAttempt = dateTime;
			}
			//Date Contacted
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Contacted");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateContacted = dateTime;
			}
			//Date Inspection Scheduled
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Inspection Scheduled");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateInspectionScheduled = dateTime;
			}
			// Date Inspection Completed
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Inspection Completed");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateInspectionCompleted = dateTime;
			}
			// Date Estimate Uploaded
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Estimate Uploaded");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateEstimateUploaded = dateTime;
			}
			// Date of Initial Reserve Change
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date of Initial Reserve Change");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateInitialReserveChange = dateTime;
			}
			// Date Indemnity Payment Requested
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Indemnity Payment Requested");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateIndemnityPaymentRequested = dateTime;
			}
			// Date Indemnity Payment Approved
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Indemnity Payment Approved");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateIndemnityPaymentApproved = dateTime;
			}
			// Date Indemnity Payment Issued
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Indemnity Payment Issued");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateIndemnityPaymentIssued = dateTime;
			}
			// Date Completed
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Completed");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateCompleted = dateTime;
			}
			// Date Closed
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Closed");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateClosed = dateTime;
			}
			// Date Closed
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Date Closed");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (DateTime.TryParse(dataRow[userFieldName].ToString(), out dateTime))
					claim.DateClosed = dateTime;
			}

			#endregion

			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Cycle Time");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (decimal.TryParse(dataRow[userFieldName].ToString(), out amount))
					claim.CycleTime = amount;
			}
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "ReOpenCycle Time");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (decimal.TryParse(dataRow[userFieldName].ToString(), out amount))
					claim.ReopenCycleTime = amount;
			}

			#region adjuster
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Adjuster Name");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				adjusterName = dataRow[userFieldName].ToString().Trim();
				adjuster = AdjusterManager.GetByAdjusterName(clientID, adjusterName);
				if (adjuster.AdjusterId == 0) {
					// add adjuster
					adjuster = new AdjusterMaster();
					adjuster.Status = true;
					adjuster.AdjusterName = adjusterName.Trim();
					adjuster.ClientId = clientID;
					adjuster.InsertBy = userID;
					adjuster.InsertDate = DateTime.Now;
					adjuster.isEmailNotification = true;
					adjuster = AdjusterManager.Save(adjuster);
				}

				claim.AdjusterID = adjuster.AdjusterId;
			}
			#endregion

			#region supervisor
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Supervisor Name");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				supervisorName = dataRow[userFieldName].ToString().Trim();
				if (!string.IsNullOrEmpty(supervisorName)) {

					contactType = LeadContactTypeManager.Get(clientID, "Supervisor");
					if (contactType == null)
						contactType = addNewContactType(clientID, "Supervisor");

					contact = ContactManager.Get(clientID, supervisorName);
					if (contact == null) {
						contact = addNewContact(clientID, supervisorName, contactType.ID);
					}

					claim.SupervisorID = contact.ContactID;
				}
			}
			#endregion

			#region Team Lead
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Team Lead Name");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				teamLead = dataRow[userFieldName].ToString().Trim();
				if (!string.IsNullOrEmpty(teamLead)) {
					contactType = LeadContactTypeManager.Get(clientID, "Team Lead");
					if (contactType == null)
						contactType = addNewContactType(clientID, "Team Lead");

					contact = ContactManager.Get(clientID, teamLead);
					if (contact == null) {
						contact = addNewContact(clientID, teamLead, contactType.ID);
					}

					claim.TeamLeadID = contact.ContactID;
				}
			}
			#endregion

			#region Owner Manager Name
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Owner Manager Name");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				ownerManagerName = dataRow[userFieldName].ToString().Trim();
				if (!string.IsNullOrEmpty(ownerManagerName)) {
					contactType = LeadContactTypeManager.Get(clientID, "Owner Manager");
					if (contactType == null)
						contactType = addNewContactType(clientID, "Owner Manager");

					contact = ContactManager.Get(clientID, ownerManagerName);
					if (contact == null) {
						contact = addNewContact(clientID, ownerManagerName, contactType.ID);
					}

					claim.ManagerID = contact.ContactID;
				}
			}
			#endregion

			#region Owner Manager Entity Name
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Owner Manager Entity Name");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				ownerManagerEntityName = dataRow[userFieldName].ToString().Trim();
				if (!string.IsNullOrEmpty(ownerManagerEntityName)) {
					contactType = LeadContactTypeManager.Get(clientID, "Owner Manager Entity Name");
					if (contactType == null)
						contactType = addNewContactType(clientID, "Owner Manager Entity Name");

					contact = ContactManager.Get(clientID, ownerManagerEntityName);
					if (contact == null) {
						contact = addNewContact(clientID, ownerManagerEntityName, contactType.ID);
					}
					claim.ManagerEntityID = contact.ContactID;
				}
			}
			#endregion

			// severity
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Severity");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				if (int.TryParse(dataRow[userFieldName].ToString().Trim(), out severity))
					claim.SeverityNumber = severity;
			}

			// event
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Event Type");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null)
				claim.EventType = dataRow[userFieldName].ToString().Trim();

			// event name
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Event Name");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null)
				claim.EventName = dataRow[userFieldName].ToString().Trim();

			// Cause of Loss Description
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Cause of Loss Description");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				causeofLoss = dataRow[userFieldName].ToString().Trim();

				damageType = TypeofDamageManager.GetTypeOfDamage(clientID, causeofLoss);
				if (damageType.TypeOfDamageId == 0) {
					damageType.ClientId = clientID;
					damageType.Status = true;
					damageType.TypeOfDamage = causeofLoss;

					damageType = TypeofDamageManager.Save(damageType);
				}

				claim.CauseOfLoss = damageType.TypeOfDamageId.ToString();
			}

			#region Amounts
			// Claim Workflow Type
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Claim Workflow Type");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null)
				claim.ClaimWorkflowType = dataRow[userFieldName].ToString().Trim();

			// Outstanding Indemnity Reserve
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Outstanding Indemnity Reserve");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				decimal.TryParse(dataRow[userFieldName].ToString().Trim(), out amount);
				claim.OutstandingIndemnityReserve = amount;
			}

			// Outstanding LAE Reserves
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Outstanding LAE Reserves");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				decimal.TryParse(dataRow[userFieldName].ToString().Trim(), out amount);
				claim.OutstandingLAEReserves = amount;
			}

			// Total Indemnity Paid
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Total Indemnity Paid");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				decimal.TryParse(dataRow[userFieldName].ToString().Trim(), out amount);
				claim.TotalIndemnityPaid = amount;
			}

			// Coverage A Paid
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Coverage A Paid");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				decimal.TryParse(dataRow[userFieldName].ToString().Trim(), out amount);
				claim.CoverageAPaid = amount;
			}

			// Coverage B Paid
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Coverage B Paid");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				decimal.TryParse(dataRow[userFieldName].ToString().Trim(), out amount);
				claim.CoverageBPaid = amount;
			}

			// Coverage C Paid
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Coverage C Paid");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				decimal.TryParse(dataRow[userFieldName].ToString().Trim(), out amount);
				claim.CoverageCPaid = amount;
			}

			// Coverage D Paid
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Coverage D Paid");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				decimal.TryParse(dataRow[userFieldName].ToString().Trim(), out amount);
				claim.CoverageDPaid = amount;
			}

			// Total Expenses Paid
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Total Expenses Paid");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				decimal.TryParse(dataRow[userFieldName].ToString().Trim(), out amount);
				claim.TotalExpensesPaid = amount;
			}
			#endregion

			claim = ClaimsManager.Save(claim);

			return claim;
		}
        private CRM.Data.Entities.LeadPolicy createPolicy(int leadID, DataRow dataRow)
        {
            CRM.Data.Entities.LeadPolicy policy = null;
			string userFieldName = null;

            policy = new CRM.Data.Entities.LeadPolicy();

			policy.LeadId = leadID;
			policy.IsActive = true;

			// Policy Number
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Policy Number");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null)
				policy.PolicyNumber = dataRow[userFieldName].ToString().Trim();

			// Policy Form
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Policy Form");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null)
				policy.PolicyFormType = dataRow[userFieldName].ToString().Trim();

			LeadPolicyManager.Save(policy);

			return policy;
		}
		private Leads createLead(DataRow dataRow) {
			Leads lead = null;
			string userFieldName = null;
			int clientID = SessionHelper.getClientId();
			int userID = SessionHelper.getUserId();
			string insuredName = null;
			string strValue = null;

			// insured name
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Insured Name");

			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null)
				insuredName = dataRow[userFieldName].ToString().Trim();

			if (string.IsNullOrEmpty(insuredName))
				return null;

			lead = new Leads();
			lead.ClientID = clientID;
			lead.UserId = userID;
			lead.Status = 1;
			lead.OriginalLeadDate = DateTime.Now;

			lead.InsuredName = insuredName;

			// mailing address 1				
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Mailing Address");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				strValue = dataRow[userFieldName].ToString().Trim();

				lead.MailingAddress = strValue.Length > 50 ? strValue.Substring(0, 50) : strValue;
				lead.LossAddress = lead.MailingAddress;
			}

			// mailing address 2				
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Mailing Address2");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				strValue = dataRow[userFieldName].ToString().Trim();

				lead.MailingAddress2 = strValue.Length > 50 ? strValue.Substring(0, 50) : strValue;
				lead.LossAddress2 = lead.MailingAddress2;
			}

			// mailing city				
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Mailing City");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				strValue = dataRow[userFieldName].ToString().Trim();

				lead.MailingCity = strValue.Length > 50 ? strValue.Substring(0, 50) : strValue;
				lead.CityName = lead.MailingCity;
			}

			// mailing state				
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Mailing State");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				strValue = dataRow[userFieldName].ToString().Trim();

				lead.MailingState = strValue.Length > 20 ? strValue.Substring(0, 20) : strValue;
				lead.StateName = lead.MailingState;
			}

			// mailing zip				
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Mailing Zip");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				strValue = dataRow[userFieldName].ToString().Trim();

				lead.MailingZip = strValue.Length > 10 ? strValue.Substring(0, 10) : strValue;
				lead.Zip = lead.MailingZip;
			}

			// mailing county				
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Mailing County");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				strValue = dataRow[userFieldName].ToString().Trim();

				lead.MailingCounty = strValue.Length > 50 ? strValue.Substring(0, 50) : strValue;
				lead.LossCounty = lead.MailingCounty;
			}

			// Business Phone Number				
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Business Phone Number");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				strValue = dataRow[userFieldName].ToString().Trim();
				lead.SecondaryPhone = strValue.Length > 35 ? strValue.Substring(0, 35) : strValue;
			}

			// Home Phone Number			
			userFieldName = Core.CSVHelper.getUserFieldName(this.mappedFields, "Home Phone Number");
			if (!string.IsNullOrEmpty(userFieldName) && dataRow[userFieldName] != null) {
				strValue = dataRow[userFieldName].ToString().Trim();
				lead.PhoneNumber = strValue.Length > 35 ? strValue.Substring(0, 35) : strValue;
			}

			lead = LeadsManager.Save(lead);

			return lead;
		}

		private LeadContactType addNewContactType(int clientID, string typeDescription) {
			LeadContactType contactType = new LeadContactType();
			contactType.Description = typeDescription;
			contactType.ClientID = clientID;

			contactType = LeadContactTypeManager.Save(contactType);

			return contactType;
		}

		private Contact addNewContact(int clientID, string contactName, int contactTypeID) {
			Contact contact = new Contact();
			contact.ClientID = clientID;
			contact.IsActive = true;
			contact.ContactName = contactName;
			contact.CategoryID = contactTypeID;
			contact = ContactManager.Save(contact);

			return contact;
		}

		protected void wizardImport_FinishButtonClick(object sender, WizardNavigationEventArgs e) {
			int importDataType = 0;

			if (ddlImportDataType.SelectedIndex > 0)
				importDataType = Convert.ToInt32(ddlImportDataType.SelectedValue);

			switch (importDataType) {
				case 1:	// claim data
					processClaimData();

					break;

				default:
					break;
			}

		}
	}

}