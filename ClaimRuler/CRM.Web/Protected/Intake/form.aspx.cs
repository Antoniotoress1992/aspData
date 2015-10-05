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
using CRM.Data;
using CRM.Data.Account;

using Infragistics.Web.UI.EditorControls;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Intake {
	public partial class form : System.Web.UI.Page {
		int clientID = 0;
		int roleID = 0;
		int userID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			clientID = Core.SessionHelper.getClientId();
			userID = Core.SessionHelper.getUserId();
			roleID = Core.SessionHelper.getUserRoleId();

			if (!Page.IsPostBack) {
				bindData();
			}

		}

		protected void bindData() {
			var carriers = CarrierManager.GetCarriers(clientID).ToList();

			//LeadPolicyCoverage[] coverages = new LeadPolicyCoverage[5];

			CollectionManager.FillCollection(ddlCarrier, "CarrierID", "CarrierName", carriers);

			var states = State.GetAll();
			CollectionManager.FillCollection(ddlState, "StateId", "StateName", states);

			CollectionManager.FillCollection(ddlInsuredLossState, "StateId", "StateName", states);

			CollectionManager.FillCollection(ddlInsuredMailingState, "StateId", "StateName", states);

			// coverage
			//gvCoverages.DataSource = coverages;
			//gvCoverages.DataBind();

			// property type
			CollectionManager.FillCollection(ddlTypeOfProperty, "TypeOfPropertyId", "TypeOfProperty", TypeOfPropertyManager.GetAll());

			// type of damage
			Fillchk(chkTypeOfDamage, "TypeOfDamageId", "TypeOfDamage", TypeofDamageManager.GetAll());
		}

		protected void btnSubmit_Click(object sender, EventArgs e) {
			string assignmentInstructions = null;
			Carrier carrier = null;
			bool isSuccess = true;
			Leads lead = null;
			string lossPerilDescription = null;
			CRM.Data.Entities.LeadPolicy policy = collectPolicyDataFromUI();
			int policyID = 0;

			lblMessage.Text = string.Empty;

			using (TransactionScope scope = new TransactionScope()) {
				try {
					// save lead/claim
					lead = saveLead();

					// save carrier
					carrier = saveCarrier();

					// save carrier contact
					saveContact(carrier.CarrierID);

					// save policy
					policy.LeadId = lead.LeadId;

					if (carrier != null)
						policy.CarrierID = carrier.CarrierID;

					policyID = LeadPolicyManager.Save(policy);

					//saveCoverages(policyID);

					// save assingment instructions in comments
					if (!string.IsNullOrEmpty(txtAssignmentInstructions.Text)) {
						assignmentInstructions = "<div><b>Assignment Instructions</b></div><div>" + this.txtAssignmentInstructions.Text.Trim() + "</div>";
						saveComments(assignmentInstructions, (int)policy.PolicyType, lead.LeadId);
					}

					// save loss Peril Description in comments
					if (!string.IsNullOrEmpty(this.txtLossPerilDescription.Text)) {
						lossPerilDescription = "<div><b>Loss/Peril Description</b></div><div>" + this.txtLossPerilDescription.Text.Trim() + "</div>";
						saveComments(lossPerilDescription, (int)policy.PolicyType, lead.LeadId);
					}

					// save final comments
					saveComments(txtComments.Text.Trim(), (int)policy.PolicyType, lead.LeadId);

					uploadFile(fileUpload1, lead.LeadId, txtfileUpload1.Text);
					uploadFile(fileUpload2, lead.LeadId, txtfileUpload2.Text);
					uploadFile(fileUpload3, lead.LeadId, txtfileUpload3.Text);
					uploadFile(fileUpload4, lead.LeadId, txtfileUpload4.Text);
					uploadFile(fileUpload5, lead.LeadId, txtfileUpload5.Text);

					// complete transaction and alert user
					scope.Complete();

					//lblMessage.Text = "Data has been submitted successfully.";
					//lblMessage.CssClass = "ok";
					isSuccess = true;
				}
				catch (Exception ex) {
					lblMessage.Text = "Unable to submit data due to error.";
					lblMessage.CssClass = "error";

					isSuccess = false;
				}

				// reload page
				if (isSuccess)
					Response.Redirect("~/Protected/Intake/form.aspx");
			}
		}

		protected void saveComments(string comments, int policyTypeID, int leadID) {
			if (!string.IsNullOrEmpty(comments)) {
				LeadComment comment = new LeadComment();

				comment.LeadId = leadID;

				comment.PolicyType = policyTypeID;

				comment.CommentText = comments;

				comment.UserId = userID;

				comment.InsertDate = DateTime.Now;

				comment.Status = 1;

				LeadCommentManager.Save(comment);
			}
		}

		//protected void saveCoverages(int policyID) {
		//	LeadPolicyCoverage coverage = null;
		//	decimal deductible = 0;

		//	foreach (GridViewRow row in gvCoverages.Rows) {
		//		if (row.RowType == DataControlRowType.DataRow) {
		//			TextBox txtCoverage = row.FindControl("txtCoverage") as TextBox;
		//			TextBox txtLimit = row.FindControl("txtLimit") as TextBox;
		//			WebTextEditor txtDeductible = row.FindControl("txtDeductible") as WebTextEditor;
		//			TextBox txtCoinsuranceForm = row.FindControl("txtCoinsuranceForm") as TextBox;

		//			// skip empty rows
		//			if (string.IsNullOrEmpty(txtCoverage.Text) && string.IsNullOrEmpty(txtLimit.Text) && string.IsNullOrEmpty(txtDeductible.Text))
		//				continue;

		//			coverage = new LeadPolicyCoverage();

		//			coverage.LeadPolicyID = policyID;
		//			decimal.TryParse(txtDeductible.Text, out deductible);
		//			coverage.Deductible = deductible;
		//			coverage.Description = txtCoverage.Text.Trim();

		//			LeadPolicyCoverageManager.Save(coverage);

		//		}
		//	}
		//}

		protected Leads saveLead() {
			Leads lead = new Leads();
			AdjusterMaster adjuster = null;

			lead.ClientID = clientID;
			lead.UserId = userID;
			lead.Status = 1;

			//if (roleID == (int)UserRole.Adjuster) {
			//	// assign adjuster
			//	adjuster = AdjusterManager.GetAdjusterByUserID(userID);
			//	if (adjuster != null)
			//		lead.Adjuster = adjuster.AdjusterId;
			//}

			lead.ClaimantFirstName = txtInsuredFirstName.Text.Trim();
			lead.ClaimantLastName = txtInsuredLastName.Text.Trim();

			#region insured loss address
			lead.LossAddress = txtInsuredLossAddressLine.Text.Trim();
			lead.LossAddress2 = txtInsuredLossAddressLine2.Text.Trim();

			if (ddlInsuredLossState.SelectedIndex > 0) {
				lead.StateId = Convert.ToInt32(ddlInsuredLossState.SelectedValue);
				lead.StateName = ddlInsuredLossState.SelectedItem.Text;
			}

			if (this.ddlInsuredLossCity.SelectedIndex > 0) {
				lead.CityId = Convert.ToInt32(ddlInsuredLossCity.SelectedValue);
				lead.CityName = ddlInsuredLossCity.SelectedItem.Text;
			}

			if (this.ddlInsuredLossZipCode.SelectedIndex > 0) {
				lead.Zip = ddlInsuredLossZipCode.SelectedItem.Text;
			}
			#endregion

			#region insured mailing address
			lead.MailingAddress = txtInsuredMailingAddress.Text.Trim();

			if (ddlInsuredMailingCity.SelectedIndex > 0)
				lead.MailingCity = ddlInsuredMailingCity.SelectedItem.Text;

			if (ddlInsuredMailingState.SelectedIndex > 0)
				lead.MailingState = ddlInsuredMailingState.SelectedItem.Text;

			if (ddlInsuredMailingZipCode.SelectedIndex > 0)
				lead.MailingZip = ddlInsuredMailingZipCode.SelectedItem.Text;
			#endregion

			lead.OriginalLeadDate = DateTime.Now;
			lead.PhoneNumber = txtInsuredPhone.Text.Trim();
			lead.EmailAddress = txtInsuredEmail.Text.Trim();
			lead.SecondaryPhone = txtInsuredFax.Text.Trim();

			if (ddlTypeOfProperty.SelectedIndex > 0) {
				lead.TypeOfProperty = Convert.ToInt32(ddlTypeOfProperty.SelectedValue);
			}

			#region damage type
			string Damagetxt = string.Empty;
			string DamageId = string.Empty;
			for (int j = 0; j < chkTypeOfDamage.Items.Count; j++) {
				if (chkTypeOfDamage.Items[j].Selected == true) {
					Damagetxt += chkTypeOfDamage.Items[j].Text + ',';
					DamageId += chkTypeOfDamage.Items[j].Value + ',';
				}
			}
			if (Damagetxt != string.Empty && DamageId != string.Empty) {
				lead.TypeOfDamage = DamageId;
				lead.TypeofDamageText = Damagetxt;
			}
			#endregion

			lead = LeadsManager.Save(lead);

			return lead;
		}

		protected Carrier saveCarrier() {
			Carrier carrier = new Carrier();


			carrier.CarrierID = Convert.ToInt32(ddlCarrier.SelectedValue);

			if (carrier.CarrierID == 0) {
				// add new carrier			

				carrier.IsActive = true;
				carrier.InsertDate = DateTime.Now;
				carrier.ClientID = clientID;

				carrier.CarrierName = txtCarrierName.Text.Trim();
				carrier.AddressLine1 = txtCarrierAddressLine1.Text.Trim();
				carrier.AddressLine2 = txtCarrierAddressLine2.Text.Trim();

				if (ddlCity.SelectedIndex > 0)
					carrier.CityName = ddlCity.SelectedItem.Text;

				if (ddlState.SelectedIndex > 0)
					carrier.StateName = ddlState.SelectedItem.Text;

				if (ddlZipCode.SelectedIndex > 0)
					carrier.ZipCode = ddlZipCode.SelectedItem.Text;


				carrier = CarrierManager.Save(carrier);
			}

			return carrier;
		}

		protected void saveContact(int carrierID) {
			if (!string.IsNullOrEmpty(txtContactFirstName.Text) && !string.IsNullOrEmpty(txtContactLastName.Text)) {
				Contact contact = new Contact();

				contact.CarrierID = carrierID;
				contact.FirstName = txtContactFirstName.Text.Trim();
				contact.LastName = txtContactLastName.Text.Trim();
				contact.Phone = txtContactPhone.Text.Trim();
				contact.Email = txtContactEmail.Text.Trim();

				ContactManager.Save(contact);
			}
		}

		protected CRM.Data.Entities.LeadPolicy collectPolicyDataFromUI() {
			CRM.Data.Entities.LeadPolicy policy = new CRM.Data.Entities.LeadPolicy();

			policy.PolicyType = Convert.ToInt32(ucPolicyType1.SelectedValue);
			policy.PolicyNumber = txtPolicyNumber.Text.Trim();
			policy.ClaimNumber = txtClaimNumber.Text.Trim();
			policy.IsActive = true;

			policy.LossDate = txtLossDate.Date;



			return policy;
		}

		protected void ddlState_SelectedIndexChanged(object sender, EventArgs e) {
			// get state
			DropDownList ddl = sender as DropDownList;
			int stateID = Convert.ToInt32(ddl.SelectedValue);

			// get cities for state
			var cities = City.GetAll(stateID);

			CollectionManager.FillCollection(ddlCity, "CityId", "CityName", cities);
		}

		protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e) {
			DropDownList ddl = sender as DropDownList;

			// get city zip codes
			int cityID = Convert.ToInt32(ddl.SelectedValue);
			var zipCodes = ZipCode.getByCityID(cityID);


			CollectionManager.FillCollection(ddlZipCode, "ZipCodeId", "ZipCode", zipCodes);
		}

		protected void ddlCarrier_SelectedIndexChanged(object sender, EventArgs e) {
			hf_carrierID.Value = ((DropDownList)sender).SelectedValue;
		}

		protected void ddlInsuredState_SelectedIndexChanged(object sender, EventArgs e) {
			// get state
			DropDownList ddl = sender as DropDownList;
			int stateID = Convert.ToInt32(ddl.SelectedValue);

			// get cities for state
			var cities = City.GetAll(stateID);

			CollectionManager.FillCollection(ddlInsuredLossCity, "CityId", "CityName", cities);
		}

		protected void ddlInsuredCity_SelectedIndexChanged(object sender, EventArgs e) {
			DropDownList ddl = sender as DropDownList;

			// get city zip codes
			int cityID = Convert.ToInt32(ddl.SelectedValue);
			var zipCodes = ZipCode.getByCityID(cityID);


			CollectionManager.FillCollection(ddlInsuredLossZipCode, "ZipCodeId", "ZipCode", zipCodes);
		}


		protected void lbtnNewCarrier_Click(object sender, EventArgs e) {
			pnlCarrier.Visible = !pnlCarrier.Visible;

			if (pnlCarrier.Visible)
				lbtnNewCarrier.Text = "Hide";
			else {
				lbtnNewCarrier.Text = "New Carrier";

				txtCarrierAddressLine1.Text = string.Empty;
				txtCarrierAddressLine2.Text = string.Empty;
				txtCarrierName.Text = string.Empty;

				txtContactEmail.Text = string.Empty;
				txtContactFax.Text = string.Empty;
				txtContactFirstName.Text = string.Empty;
				txtContactLastName.Text = string.Empty;
				txtContactPhone.Text = string.Empty;

				ddlState.SelectedIndex = -1;
				ddlCity.Items.Clear();
				ddlZipCode.Items.Clear();

				hf_carrierID.Value = "0";
			}
		}

		protected void ddlInsuredMailingState_SelectedIndexChanged(object sender, EventArgs e) {
			// get state
			DropDownList ddl = sender as DropDownList;
			int stateID = Convert.ToInt32(ddl.SelectedValue);

			// get cities for state
			var cities = City.GetAll(stateID);

			CollectionManager.FillCollection(ddlInsuredMailingCity, "CityId", "CityName", cities);
		}

		protected void ddlInsuredMailingCity_SelectedIndexChanged(object sender, EventArgs e) {
			DropDownList ddl = sender as DropDownList;

			// get city zip codes
			int cityID = Convert.ToInt32(ddl.SelectedValue);
			var zipCodes = ZipCode.getByCityID(cityID);


			CollectionManager.FillCollection(ddlInsuredMailingZipCode, "ZipCodeId", "ZipCode", zipCodes);
		}

		public static void Fillchk(ListControl control, string key, string value, object data) {
			control.DataSource = data;
			control.DataValueField = key;
			control.DataTextField = value;
			control.DataBind();

		}

		protected void btnCopyAddress_Click(object sender, EventArgs e) {
			txtInsuredMailingAddress.Text = txtInsuredLossAddressLine.Text;

			ddlInsuredMailingState.SelectedIndex = ddlInsuredLossState.SelectedIndex;

			// copy items
			ddlInsuredMailingCity.Items.Clear();
			ddlInsuredMailingZipCode.Items.Clear();

			ddlInsuredMailingCity.Items.AddRange(ddlInsuredLossCity.Items.OfType<ListItem>().ToArray());
			ddlInsuredMailingZipCode.Items.AddRange(ddlInsuredLossZipCode.Items.OfType<ListItem>().ToArray());

			ddlInsuredMailingCity.SelectedIndex = ddlInsuredLossCity.SelectedIndex;
			ddlInsuredMailingZipCode.SelectedIndex = ddlInsuredLossZipCode.SelectedIndex;

		}

		protected void uploadFile(FileUpload fileupload, int leadID, string fileDescription) {
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();
			string filePath = null;

			if (fileupload.HasFile) {
				LeadsImage objLeadImage = new LeadsImage();
				objLeadImage.LeadId = leadID;
				objLeadImage.ImageName = fileupload.FileName;
				objLeadImage.Status = 1;
				objLeadImage.InsertBy = userID;
				objLeadImage.isPrint = true;

				if (!string.IsNullOrEmpty(fileDescription))
					objLeadImage.Description = fileDescription;
				
				objLeadImage.policyTypeID = Convert.ToInt32(ucPolicyType1.SelectedValue);
				objLeadImage = LeadsUploadManager.SaveImage(objLeadImage);

				if (fileupload.PostedFile.ContentType == "application/msword") {

				}
				else {
					// images
					string filename = Path.GetFileName(fileupload.FileName);
					filePath = string.Format("{0}\\LeadsImage\\{1}\\{2}", appPath, leadID, objLeadImage.LeadImageId);

					if (!Directory.Exists(filePath))
						Directory.CreateDirectory(filePath);

					fileupload.SaveAs(filePath + "\\" + filename);
				}
			}
		}
		
	}
}