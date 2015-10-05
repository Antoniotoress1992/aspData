using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Data.Account;
using CRM.Data;
using CRM.Repository;
using LinqKit;
using CRM.Core;
using System.Globalization;
using System.Transactions;
using Infragistics.Web.UI.EditorControls;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucAdjuster : System.Web.UI.UserControl {
        int roleID = 0;
		private int adjusterID {
			get {
				string decryptedID = null;
				int id = 0;

				if (Request.QueryString["q"] != null) {
					decryptedID = Core.SecurityManager.DecryptQueryString(Request.QueryString["q"].ToString());
					int.TryParse(decryptedID, out id);
				}

				return id;
			}
		}

		int clientID = 0;

		protected void Page_Load(object sender, EventArgs e) {
			clientID = Core.SessionHelper.getClientId();
			
			this.Page.Form.DefaultButton = this.btnSaveAdjuster.UniqueID;

			if (!IsPostBack) {
				hdId.Value = this.adjusterID.ToString();

				
				DoBind();
                bindRole();
			}
			//if (Session["LeadIds"] == null)
			//	btnReturnToClaim.Visible = false;
		}
        private void bindRole()
        {
            List<SecRole> roles = null;

            if (roleID == (int)UserRole.Administrator)
                roles = SecRoleManager.GetAll();
            else
                roles = SecRoleManager.GetRolesManagedByClient(clientID);

            CollectionManager.FillCollection(ddlRole, "RoleId", "RoleName", roles);
        }

		protected void btnCreateAccount_Click(object sender, EventArgs e) {
			AdjusterMaster adjuster = null;
            
			int adjusterID = 0;
			int clientID = 0;
			string newUserName = null;
			string password = null;
			//CRM.Data.Entities.SecUser newUserAccount = null;
			CRM.Data.Entities.SecUser user = null;


			// prepare user name
			newUserName = string.Format("{0}.{1}", txtFirstName.Text.ToLower(), txtLastName.Text.ToLower());


			// check username is not taken
			if (SecUserManager.IsUserNameExist(newUserName)) {
				lblError.Text = "User Name " + newUserName + " is already taken.";
				return;
			}

			

			try 
            {
				using (TransactionScope scope = new TransactionScope()) 
                {
					// add new user account
                    // initialize user object
                    user = new CRM.Data.Entities.SecUser();

                    user.UserName = newUserName;
                    user.FirstName = txtFirstName.Text;
                    user.LastName = txtLastName.Text;
                    user.Status = true;
                    user.Blocked = false;
                    //user.isSSL = true;

                    // assign client for this user
                    clientID = Core.SessionHelper.getClientId();
                    if (clientID > 0)
                        user.ClientID = clientID;

                    user.CreatedBy = Core.SessionHelper.getUserId();
                    user.CreatedOn = DateTime.Now;
                    user.Email = txtEmail.Text;

                    // assign adjuster role..well now they can select what ever role they want
                    user.RoleId = Convert.ToInt32(ddlRole.SelectedValue); //38; //(int)UserRole.Adjuster;
                    user.isViewAllClaims = cbxViewAllClaims.Checked;
                    
                    // random password
                    password = Guid.NewGuid().ToString().Substring(0, 8);

                    // encrypt password
                    user.Password = Core.SecurityManager.Encrypt(password);

					user = SecUserManager.Save(user);
                   
					//int.TryParse(hdId.Value, out adjusterID);

                   
                    adjusterID = AdjusterManager.GetAdjusterIdForNewUser(); //added new to get the last adjuster created n 

					if (user != null && adjusterID > 0) 
                    {
						// get adjuster 
						adjuster = AdjusterManager.GetAdjusterId(adjusterID);

						if (adjuster != null) {
							// assign new user id to adjuster
							adjuster.userID = user.UserId;

							// update adjuster
							AdjusterManager.Save(adjuster);

                            
							// email adjuster about newly created account
							
                            if (!string.IsNullOrEmpty(adjuster.email))
                                newAccountNotification(adjuster, user);
                      
							// hide create account button
							btnCreateAccount.Visible = false;
						}
					}
                    scope.Complete();
                    lblSave.Text = "User account for adjuster has been created.";
                    lblSave.Visible = true;
				}
			}
			catch (Exception ex) 
            {
				lblError.Visible = true;
				lblError.Text = "Unable to create user account.";

				Core.EmailHelper.emailError(ex);
			}

		}

	
		protected void btnRefreshLicense_Click(object sender, EventArgs e) {
			clearServiceStateLicense();

			showServiceStateLicenseGrid();

			bindServiceStateLicense();
		}

		protected void btnRefreshAdjusterPhoto_Click(object sender, EventArgs e) {
			AdjusterMaster adjuster = null;
			int adjusterID = 0;

			if (int.TryParse(hdId.Value, out adjusterID) && adjusterID > 0) {

				adjuster = AdjusterManager.GetAdjusterId(adjusterID);
				if (adjuster != null)
					bindPhoto(adjusterID, adjuster.PhotoFileName);
			}
			
		}
		

		private void DoBind() {
			AdjusterMaster adjuster = null;
            
            CRM.Data.Entities.SecUser user = null;

            SecUserManager user2 = null;

			List<StateMaster> states = State.GetAll();

            List<DeployementAddressData> objAdjusterDeploymentAddress = AdjusterManager.GetAllDeployementAddress(Core.SessionHelper.getUserId());
			
			List<LeadPolicyType> policyTypes = LeadPolicyTypeManager.GetAll();

			// 2014-05-04 tortega
			List<UserStaff> staff = SecUserManager.GetStaff(clientID);
            List<AdjusterLicenseAppointmentType> licenseAppointmentTypes = null;

			CollectionManager.FillCollection(ddlState, "StateId", "StateName", states);

			CollectionManager.FillCollection(ddlStateLicense, "StateId", "StateName", states);
            licenseAppointmentTypes = AdjusterLicenseAppointmentTypeManager.GetAll(clientID);
            CollectionManager.FillCollection(ddlAppointmentType, "LicenseAppointmentTypeID", "LicenseAppointmentType", licenseAppointmentTypes);



			CollectionManager.FillCollection(ddlDeployState, "StateId", "StateName", states);

			CollectionManager.FillCollection(ddlPolicyType, "LeadPolicyTypeID", "Description", policyTypes);

			// 2014-05-04 tortega
			CollectionManager.FillCollection(ddlSupervisor, "UserId", "StaffName", staff);
            CollectionManager.FillCollection(ddlDeploymentAddress, "Id", "DeploymentAddress", objAdjusterDeploymentAddress);
           
            
           

			if (adjusterID > 0) {
				// edit adjuster
				adjuster = AdjusterManager.Get(adjusterID);

                
				if (adjuster != null && adjuster.AdjusterId > 0) {
					showTabs(true);
					btnCreateAccount.Visible = true;

					txtFirstName.Text = adjuster.FirstName;
					txtLastName.Text = adjuster.LastName;
					txtAddress.Text = adjuster.Address1;
					txtAddress2.Text = adjuster.Address2;
					txtFedID.Text = adjuster.FederalTaxID;
                    
                    if (adjuster.userID != null)
                    {
                        int myID = (int)adjuster.userID;
                        user = SecUserManager.GetByUserId(myID);
                        ddlRole.SelectedValue = user.RoleId.ToString();
                    }
					
                    if (adjuster.StateID != null) {
						try {
							ListItem item = ddlState.Items.FindByValue(adjuster.StateID.ToString());
							if (item != null)
								ddlState.SelectedIndex = ddlState.Items.IndexOf(item);
						}
						catch (Exception) {
							ddlState.SelectedIndex = -1;
						}
					}

					//txtFeeContract.Text = adjuster.FeePerContract.ToString();
					txtEmail.Text = adjuster.email;
					cbxNotification.Checked = adjuster.isEmailNotification ?? true;

					txtFaxNumber.Text = adjuster.FaxNumber;
					txtPhoneNumber.Text = adjuster.PhoneNumber;
                    txtCompanyPhone.Text = adjuster.CompanyPhone;//NEW OC 9/22/14
					txtCompanyName.Text = adjuster.CompanyName;

					txtZip.Text = adjuster.ZipCode;
					txtCityName.Text = adjuster.CityName;

					txtMaxNumberClaims.Text = adjuster.MaxClaimNumber == null ? "0" : adjuster.MaxClaimNumber.ToString();

					// activate create account button when user account has not been created for adjuster
					if (adjuster.userID == null)
						btnCreateAccount.Visible = true;
					else
						btnCreateAccount.Visible = false;

					// show status
					cbxStatus.Checked = adjuster.Status ?? true;

					cbxisW9.Checked = adjuster.isW9 ?? false;

					cbxNotifyUserUploadDocument.Checked = adjuster.isNotifyUserUploadDocument ?? true;

					txtServiceArea.Text = adjuster.GeographicalSeriveArea;

					txtYearExperience.Text = adjuster.YearsExperiece.ToString();

					txtCertification.Text = adjuster.Certifications;

					// deployment address
					cbxUseDeploymentAddress.Checked = adjuster.UseDeploymentAddress ?? false;
					txtDeployAddress.Text = adjuster.DeploymentAddress;
                    txtDeployAddress2.Text = adjuster.DeploymentAddress2;
					txtDeployCity.Text = adjuster.DeploymentCity;
					txtDeployZipCode.Text = adjuster.DeploymentZipCode;

					// 2014-05-02
					txtHourlyRate.Value = adjuster.HourlyRate;
					txtCommissionRate.Value = adjuster.CommissionRate;
					txtXactNetAddress.Text = adjuster.XactNetAddress;

					if (adjuster.DeploymentStateID != null) {
						try {
							ddlDeployState.SelectedValue = adjuster.DeploymentStateID.ToString();
						}
						catch (Exception ex) {
							ddlDeployState.SelectedIndex = -1;
						}
					}

					// 2014-05-04 tortega
					ddlSupervisor.SelectedValue = (adjuster.SupervisorID ?? 0).ToString();

					bindPhoto((int)adjuster.AdjusterId, adjuster.PhotoFileName);

					bindNotes();

					bindServiceStateLicense();

					bindClaimTypeHandled();

					bindReferences();

                    BindPayrollSetting();

                    BindDeploymentEvent();


				}	//if (adjuster != null && adjuster.AdjusterId > 0) {
			}	// if (adjusterID > 0) {
			else {
				// new adjuster
				showTabs(false);
				btnCreateAccount.Visible = false;

			
				txtMaxNumberClaims.Text = "0";

				cbxStatus.Checked = true;
				cbxNotification.Checked = true;
				cbxNotifyUserUploadDocument.Checked = true;
				cbxUseDeploymentAddress.Checked = false;				
			}

			
		}

		private void bindPhoto(int adjusterID, string photoFilename) {
			string appUrl = ConfigurationManager.AppSettings["appURL"].ToString();

			if (adjusterID > 0  && !string.IsNullOrEmpty(photoFilename)) {
				adjusterPhoto.ImageUrl = string.Format("{0}/Adjusters/{1}/Photo/{2}", appUrl, adjusterID, photoFilename);
			}			
		}
		
		protected void btnReturnToList_Click(object sender, EventArgs e) {
			Response.Redirect("~/Protected/Admin/Adjuster.aspx");
		}

		

	
		protected void gvAdjuster_RowCommand(object sender, GridViewCommandEventArgs e) {
			int adjusterID = 0;

			if (e.CommandName == "DoEdit") {
				hdId.Value = e.CommandArgument.ToString();
				adjusterID = Convert.ToInt32(e.CommandArgument.ToString());

				AdjusterMaster adjuster = AdjusterManager.GetAdjusterId(adjusterID);

				hfStateID.Value = (adjuster.StateID ?? 0).ToString();

				txtFirstName.Text = adjuster.FirstName;
				txtLastName.Text = adjuster.LastName;

				txtAddress.Text = adjuster.Address1;
				txtAddress2.Text = adjuster.Address2;
				txtFedID.Text = adjuster.FederalTaxID;

				if (adjuster.StateID != null) {
					ddlState.SelectedValue = adjuster.StateID.ToString();

					
				}

				txtZip.Text = adjuster.ZipCode;
				txtCityName.Text = adjuster.CityName;

				//txtFeeContract.Text = adjuster.FeePerContract.ToString();
				txtEmail.Text = adjuster.email;
				cbxNotification.Checked = adjuster.isEmailNotification ?? true;
				cbxNotifyUserUploadDocument.Checked = adjuster.isNotifyUserUploadDocument ?? true;

				txtFaxNumber.Text = adjuster.FaxNumber;
				txtPhoneNumber.Text = adjuster.PhoneNumber;
                txtCompanyPhone.Text = adjuster.CompanyPhone; //NEW OC 9/22/14
				txtCompanyName.Text = adjuster.CompanyName;
				txtMaxNumberClaims.Text = adjuster.MaxClaimNumber == null ? "0" : adjuster.MaxClaimNumber.ToString();

				// activate create account button when user account has not been created for adjuster
				if (adjuster.userID == null)
					btnCreateAccount.Visible = true;
				else
					btnCreateAccount.Visible = false;



			}
			else if (e.CommandName == "DoDelete") {
				adjusterID = Convert.ToInt32(e.CommandArgument.ToString());

				AdjusterMaster adjuster = AdjusterManager.GetAdjusterId(adjusterID);

				if (adjuster != null) {
					adjuster.Status = false;

					AdjusterManager.Save(adjuster);
				}
			}

			loadAdjusters();
		}

		protected void gv_onSorting(object sender, GridViewSortEventArgs e) {
			IQueryable<AdjusterMaster> adjusters = null;
			int clientID = Core.SessionHelper.getClientId();

			if (clientID > 0)
				adjusters = AdjusterManager.GetAll(clientID);
			else
				adjusters = AdjusterManager.GetAll();

			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			//gvAdjuster.DataSource = adjusters.orderByExtension(e.SortExpression, descending);

			//gvAdjuster.DataBind();

		}

		string saveMsg = string.Empty;

		protected void loadAdjusters() {
			IQueryable<AdjusterMaster> adjusters = null;
			int clientID = Core.SessionHelper.getClientId();

			if (clientID > 0)
				adjusters = AdjusterManager.GetAll(clientID);
			else
				adjusters = AdjusterManager.GetAll();

			//gvAdjuster.DataSource = adjusters.ToList();
			//gvAdjuster.DataBind();

		}

		private void newAccountNotification(AdjusterMaster adjuster, CRM.Data.Entities.SecUser user) {
			StringBuilder emailText = new StringBuilder();

			string[] recipients = new string[] { adjuster.email };

			// get system email account
            string userEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
            string emailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
			string appURL = ConfigurationManager.AppSettings["appURL"].ToString();

			// name of CRM client
			//string clientName = adjuster.Client == null ? "" : adjuster.Client.BusinessName;

			string subject = /*clientName +*/  "IMPORTANT Account Information - New User Created (Claim Ruler Software)";

			emailText.Append(string.Format("<br>Congratulations, {0}!<br>", adjuster.AdjusterName));

			emailText.Append(/*string.Format*/"<br>You have been granted access to Claim Ruler in order for you to work on those claims that are assigned to you."/*, clientName)*/);

			emailText.Append("<br><br>Below is your account information:");
			emailText.Append(string.Format("<br>User Name: <b>{0}</b>", user.UserName));
			emailText.Append(string.Format("<br>Password: <b>{0}</b>", Core.SecurityManager.Decrypt(user.Password)));

			emailText.Append(string.Format("<br><br>Click {0} to login.", appURL));

            Core.EmailHelper.sendEmail(/*adjuster.email*/userEmail, recipients, null, subject, emailText.ToString(), null, userEmail, emailPassword);
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;

			Page.Validate("Adjuster");
			if (!Page.IsValid)
				return;

			try {
				if (saveAdjuster()) {

					lblSave.Text = "Adjuster information saved successfully.";

					lblSave.Visible = true;

					// activate create account button when user account has not been created for adjuster				
					btnCreateAccount.Visible = true;
                    List<DeployementAddressData> listAdjusterDeploymentAddress = AdjusterManager.GetAllDeployementAddress(Core.SessionHelper.getUserId());
                    CollectionManager.FillCollection(ddlDeploymentAddress, "Id", "DeploymentAddress", listAdjusterDeploymentAddress);
				}
			}

			catch (Exception ex) {
				lblError.Visible = true;
				lblError.Text = "Unable to save adjuster information!";

				Core.EmailHelper.emailError(ex);
			}
		}

		protected bool saveAdjuster() {
			AdjusterMaster adjuster = null;
			bool isSuccess = true;
			int yearsExperience = 0;
			int deploymentStateID = 0;
			int maxClaimNumner = 0;

			// 2013-08-07 tortega
			int clientID = Core.SessionHelper.getClientId();

			decimal fee = 0;
			int adjusterID = Convert.ToInt32(hdId.Value);

			string adjusterName = string.Format("{0} {1}", txtFirstName.Text, txtLastName.Text);

			using (TransactionScope scope = new TransactionScope()) {

				bool exists = AdjusterManager.IsExist(adjusterName, clientID);

				if (adjusterID == 0 && exists) {
					lblMessage.Text = "Adjuster name already exists.";
					lblMessage.Visible = true;
					txtFirstName.Focus();
					return false;
				}

				if (adjusterID > 0) {
					// update adjuster
					adjuster = AdjusterManager.GetAdjusterId(adjusterID);

					adjuster.Status = cbxStatus.Checked;
				}
				else {
					// mew adjuster
					adjuster = new AdjusterMaster();

					adjuster.Status = true;

				}

				adjuster.AdjusterName = adjusterName;
				adjuster.FirstName = txtFirstName.Text;
				adjuster.LastName = txtLastName.Text;
				adjuster.FederalTaxID = txtFedID.Text.Trim();
				adjuster.Address1 = txtAddress.Text.Trim();
				adjuster.Address2 = txtAddress2.Text.Trim();

				

				adjuster.CityName = txtCityName.Text.Trim();
				adjuster.ZipCode = txtZip.Text.Trim();
				adjuster.FederalTaxID = txtFedID.Text.Trim();
								
				adjuster.CompanyName = txtCompanyName.Text.Trim();
				adjuster.PhoneNumber = txtPhoneNumber.Text.Trim();//NEW OC 9/22/2014
                adjuster.CompanyPhone = txtCompanyPhone.Text.Trim();
				adjuster.FaxNumber = txtFaxNumber.Text.Trim();

				//decimal.TryParse(txtFeeContract.Text, out fee);
				adjuster.FeePerContract = fee;

				adjuster.isEmailNotification = cbxNotification.Checked;
				adjuster.isNotifyUserUploadDocument = cbxNotifyUserUploadDocument.Checked;
				adjuster.email = txtEmail.Text;

				

				adjuster.isW9 = cbxisW9.Checked;
				int.TryParse(txtYearExperience.Text, out yearsExperience);

				adjuster.YearsExperiece = yearsExperience;
				adjuster.GeographicalSeriveArea = txtServiceArea.Text;

				adjuster.Certifications = txtCertification.Text;

				// 2013-08-07 tortega
				if (clientID > 0)
					adjuster.ClientId = clientID;

                adjuster.UseDeploymentAddress = cbxUseDeploymentAddress.Checked;

                if (ddlState.SelectedIndex > 0)
                    adjuster.StateID = Convert.ToInt32(ddlState.SelectedValue);
                else
                    adjuster.StateID = null;

				



				int.TryParse(txtMaxNumberClaims.Text, out maxClaimNumner);
				adjuster.MaxClaimNumber = maxClaimNumner;

				// 2014-05-02
				adjuster.XactNetAddress = txtXactNetAddress.Text;
				adjuster.HourlyRate = txtHourlyRate.ValueDecimal;
				adjuster.CommissionRate = txtCommissionRate.ValueDecimal;

				// 2014-05-04 tortega
				if (ddlSupervisor.SelectedIndex > 0)
					adjuster.SupervisorID = Convert.ToInt32(ddlSupervisor.SelectedValue);


                int deploymentAddressId = 0;
                int.TryParse(ddlDeploymentAddress.SelectedValue, out deploymentAddressId);
                if (deploymentAddressId > 0)
                {
                    AdjusterDeploymentAddress objAdjusterDeploymentAddress = AdjusterManager.GetDeployementAddress(deploymentAddressId);
                    if (objAdjusterDeploymentAddress!=null)
                    {


                        adjuster.DeploymentAddress = objAdjusterDeploymentAddress.DeploymentAddress;
                        adjuster.DeploymentAddress2 = objAdjusterDeploymentAddress.DeploymentAddress2;
                        adjuster.DeploymentCity = objAdjusterDeploymentAddress.City;
                        if (objAdjusterDeploymentAddress.ZipCode!=null)
                        {
                            adjuster.DeploymentZipCode = Convert.ToString(objAdjusterDeploymentAddress.ZipCode);
                        }                        
                        adjuster.DeploymentStateID =objAdjusterDeploymentAddress.States;
                    }

                    //fill control
                    txtDeployAddress.Text = objAdjusterDeploymentAddress.DeploymentAddress;
                    txtDeployAddress2.Text = objAdjusterDeploymentAddress.DeploymentAddress2;
                    if (objAdjusterDeploymentAddress.States != null)
                    {
                        ddlDeployState.SelectedValue = Convert.ToString(objAdjusterDeploymentAddress.States);
                    }
                    else
                    {
                        ddlDeployState.SelectedValue = "0";
                    }
                    txtDeployCity.Text = objAdjusterDeploymentAddress.City;
                    if (objAdjusterDeploymentAddress.ZipCode!=null)
                       {
                            txtDeployZipCode.Text = Convert.ToString(objAdjusterDeploymentAddress.ZipCode);
                       }  
                    
                }
                else
                {
                    // deployment address    
                    adjuster.DeploymentAddress = txtDeployAddress.Text;
                    adjuster.DeploymentAddress2 = txtDeployAddress2.Text;
                    adjuster.DeploymentCity = txtDeployCity.Text;
                    adjuster.DeploymentZipCode = txtDeployZipCode.Text;

                    int.TryParse(ddlDeployState.SelectedValue, out deploymentStateID);
                    if (deploymentStateID > 0)
                        adjuster.DeploymentStateID = deploymentStateID;



                    //code fro save deployment address
                    if (chkSaveAddress.Checked)
                    {
                        AdjusterDeploymentAddress objAdjusterDeploymentAddress2 = new AdjusterDeploymentAddress();
                        objAdjusterDeploymentAddress2.UserId = SessionHelper.getUserId();
                        objAdjusterDeploymentAddress2.DeploymentAddress = txtDeployAddress.Text;
                        objAdjusterDeploymentAddress2.DeploymentAddress2 = txtDeployAddress2.Text;
                        if (ddlDeployState.SelectedIndex > 0)
                            objAdjusterDeploymentAddress2.States = Convert.ToInt32(ddlDeployState.SelectedValue);
                        else
                            objAdjusterDeploymentAddress2.States = null;

                        objAdjusterDeploymentAddress2.City = txtDeployCity.Text;
                        objAdjusterDeploymentAddress2.IsActive = true;

                        if (!string.IsNullOrEmpty(txtDeployZipCode.Text))
                        {
                        objAdjusterDeploymentAddress2.ZipCode =Convert.ToInt32(txtDeployZipCode.Text);
                        }                      

                      AdjusterDeploymentAddress objAdjusterDeploymentAddress3 =  AdjusterManager.SaveDeploymentAddress(objAdjusterDeploymentAddress2);
                      adjuster.DeploymentAddressID = objAdjusterDeploymentAddress3.Id;
                    }

                }
				AdjusterManager.Save(adjuster);

				scope.Complete();

				isSuccess = true;
                
			}
			return isSuccess;
		}

		
		private void showTabs(bool isVisible) {
			tabPanelNotes.Visible = isVisible;
			tabPanelstates.Visible = isVisible;
			tabPanelClaimHandle.Visible = isVisible;
			tabPanelReferences.Visible = isVisible;
            tabPanelAdjusterSettingsAndPayroll.Visible = isVisible;
		}

		#region        Note methods
		protected void bindNotes() {
			int adjusterID = Convert.ToInt32(hdId.Value);
			gvNotes.DataSource = AdjusterNoteManager.GetAll(adjusterID).ToList();
			gvNotes.DataBind();
		}
		protected void blnkNewNote_Click(object sender, EventArgs e) {
			ViewState["noteID"] = "0";
			
			txtNote.Focus();

			showNotePanel();
		}

		protected void btnNoteSave_Click(object sender, EventArgs e) {
			int adjusterID = Convert.ToInt32(hdId.Value);
			int noteID = 0;

			AdjusterNote note = null;

			int.TryParse(ViewState["noteID"].ToString(), out noteID);

			if (noteID == 0)
				note = new AdjusterNote();
			else
				note = AdjusterNoteManager.Get(noteID);

			if (note != null) {
				note.AdjusterID = adjusterID;

				note.UserID = Core.SessionHelper.getUserId();

				note.Notes = txtNote.Text;

				note.NoteID = noteID;

				try {
					AdjusterNoteManager.Save(note);

					bindNotes();

					showAdjusterNotes();

				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}
		}

		protected void btnNoteCancel_Click(object sender, EventArgs e) {
			txtNote.Text = string.Empty;
			showAdjusterNotes();
		}

		private void showAdjusterNotes() {
			pnlNotes.Visible = false;
			gvNotes.Visible = true;
		}


		protected void gvNotes_RowCommand(object sender, GridViewCommandEventArgs e) {
			AdjusterNote note = null;
			int noteID = Convert.ToInt32(e.CommandArgument);

			if (e.CommandName == "DoEdit") {
				note = AdjusterNoteManager.Get(noteID);

				ViewState["noteID"] = e.CommandArgument;

				if (note != null) {
					txtNote.Text = note.Notes;

					showNotePanel();
				}
			}
			if (e.CommandName == "DoDelete") {
				try {
					AdjusterNoteManager.Delete(noteID);

					bindNotes();
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}

				showAdjusterNotes();
			}
		}

		protected void showNotePanel() {
			gvNotes.Visible = false;
			pnlNotes.Visible = true;
		}
		#endregion

		#region	State(s) of Service Licensure per State
		protected void bindServiceStateLicense() {
			int adjusterID = Convert.ToInt32(hdId.Value);

			// load adjuster's states where licensed
			gvServiceStateLicense.DataSource = AdjusterStateLicenseManager.GetAll(adjusterID);
			gvServiceStateLicense.DataBind();
		}

		protected void lbtnNewServiceStateLicense_Click(object sender, EventArgs e) {
			ViewState["ServiceStateLicenseID"] = "0";

			ddlStateLicense.Focus();

			clearServiceStateLicense();

			showServiceStateLicensePanel();

			btnUploadLicense.Visible = false;
		}

		protected void gvServiceStateLicense_RowCommand(object sender, GridViewCommandEventArgs e) {
			AdjusterServiceArea service = null;
			int id = Convert.ToInt32(e.CommandArgument);

			List<AdjusterLicenseAppointmentType> licenseAppointmentTypes = null;

			if (e.CommandName == "DoEdit") {
				service = AdjusterStateLicenseManager.Get(id);

				if (service != null) {
					licenseAppointmentTypes = AdjusterLicenseAppointmentTypeManager.GetAll(clientID);

					CollectionManager.FillCollection(ddlAppointmentType, "LicenseAppointmentTypeID", "LicenseAppointmentType", licenseAppointmentTypes);

					ddlAppointmentType.SelectedValue = (service.AppointmentTypeID ?? 0).ToString();
					
					ViewState["ServiceStateLicenseID"] = e.CommandArgument;
					
					txtLicenseNumber.Text = service.LicenseNumber;

					if (service.LicenseEffectiveDate != null)
						txtEffectiveDate.Text = string.Format("{0:MM/dd/yyyy}", service.LicenseEffectiveDate);

					if (service.LicenseExpirationDate != null)
						txtExpirationDate.Text = string.Format("{0:MM/dd/yyyy}", service.LicenseExpirationDate);

					if (service.StateID != null) {
						ddlStateLicense.SelectedValue = service.StateID.ToString();

						hfStateID.Value = service.StateID.ToString();
					}

					showServiceStateLicensePanel();

					// show upload license panle after record is saved
					btnUploadLicense.Visible = true;
					
				}
			}
			if (e.CommandName == "DoDelete") {
				try {
					AdjusterStateLicenseManager.Delete(id);

					bindServiceStateLicense();
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}

				showServiceStateLicenseGrid();
			}
		}

		protected void showServiceStateLicensePanel() {
			pnlServiceStateLicense.Visible = true;
			gvServiceStateLicense.Visible = false;
			lbtnNewServiceStateLicense.Visible = false;

			
		}

		protected void showServiceStateLicenseGrid() {
			pnlServiceStateLicense.Visible = false;
			gvServiceStateLicense.Visible = true;
			lbtnNewServiceStateLicense.Visible = true;

			
		}


		protected void btnServiceStateLicenseSave_Click(object sender, EventArgs e) {
			AdjusterServiceArea service = null;
			int id = 0;

			Page.Validate("license");
			if (!Page.IsValid)
				return;
			
			int adjusterID = Convert.ToInt32(hdId.Value);

			int.TryParse(ViewState["ServiceStateLicenseID"].ToString(), out id);
			if (id == 0)
				service = new AdjusterServiceArea();
			else
				service = AdjusterStateLicenseManager.Get(id);

			if (service != null) {
				service.AdjusterID = adjusterID;

				service.StateID = Convert.ToInt32(ddlStateLicense.SelectedValue);

				service.LicenseNumber = txtLicenseNumber.Text;

				if (!string.IsNullOrEmpty(txtExpirationDate.Text))
					service.LicenseExpirationDate = txtExpirationDate.Date;

				if (!string.IsNullOrEmpty(txtEffectiveDate.Text))
					service.LicenseEffectiveDate = txtEffectiveDate.Date;

				if (ddlAppointmentType.SelectedIndex > 0)
					service.AppointmentTypeID = Convert.ToInt32(ddlAppointmentType.SelectedValue);

				AdjusterStateLicenseManager.Save(service);
			}
			showServiceStateLicenseGrid();

			bindServiceStateLicense();
		}

		protected void btnServiceStateLicenseCancel_Click(object sender, EventArgs e) {
			clearServiceStateLicense();

			showServiceStateLicenseGrid();			
		}

		protected void clearServiceStateLicense() {
			txtExpirationDate.Text = string.Empty;

			txtEffectiveDate.Text = string.Empty;

			txtLicenseNumber.Text = string.Empty;

			ddlStateLicense.SelectedIndex = -1;
		}

		protected void gvServiceStateLicense_RowDataBound(object sender, GridViewRowEventArgs e) {
			string licensePath = null;
			AdjusterServiceArea serviceArea = null;

			int adjusterID = Convert.ToInt32(hdId.Value);

			// get application path
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();
			string appUrl = ConfigurationManager.AppSettings["appURL"].ToString();


			if (e.Row.RowType == DataControlRowType.DataRow) {
				HyperLink hlnkLicense = e.Row.FindControl("hlnkLicense") as HyperLink;
				serviceArea = e.Row.DataItem as AdjusterServiceArea;

				if (serviceArea != null && serviceArea.StateID != null) {
					licensePath = string.Format("{0}/Adjusters/{1}/License/{2}.pdf", appPath, serviceArea.AdjusterID, serviceArea.StateID);
					if (File.Exists(licensePath)) {
						hlnkLicense.NavigateUrl = string.Format("{0}/Adjusters/{1}/License/{2}.pdf", appUrl, serviceArea.AdjusterID, serviceArea.StateID);
						hlnkLicense.CssClass = "link";
					}
				}
			}
		}
		#endregion

		#region Types of Claims Handled
		protected void bindClaimTypeHandled() {
			int adjusterID = Convert.ToInt32(hdId.Value);

			gvTypeClaimHandled.DataSource = AdjusterTypeClaimHandledManager.GetAll(adjusterID);

			gvTypeClaimHandled.DataBind();
		}
		protected void ddlPolicyType_SelectedIndexChanged(object sender, EventArgs e) {
			AdjusterHandleClaimType claimHandle = null;
			
			int adjusterID = Convert.ToInt32(hdId.Value);

			int policyTypeID = Convert.ToInt32(ddlPolicyType.SelectedValue);

			if (policyTypeID > 0) {
				claimHandle = new AdjusterHandleClaimType();
				
				claimHandle.AdjusterID = adjusterID;
				
				claimHandle.PolicyTypeID = policyTypeID;

				AdjusterTypeClaimHandledManager.Save(claimHandle);

				ddlPolicyType.SelectedIndex = -1;

				bindClaimTypeHandled();
			}
		}

		protected void gvTypeClaimHandled_RowCommand(object sender, GridViewCommandEventArgs e) {
			int id = Convert.ToInt32(e.CommandArgument);

			if (e.CommandName == "DoDelete") {
				AdjusterTypeClaimHandledManager.Delete(id);

				bindClaimTypeHandled();
			}
		}
		#endregion

		#region References

		protected void bindReferences() {
			int adjusterID = Convert.ToInt32(hdId.Value);
			gvReferences.DataSource = AdjusterReferenceManager.GetAll(adjusterID).ToList();
			gvReferences.DataBind();
		}
		protected void lbtnNewReference_Click(object sender, EventArgs e) {
			ViewState["ReferenceID"] = "0";

			txtRefenreceName.Focus();

			clearReferenceFields();

			showReferencePanel();


		}
		protected void showReferencePanel() {
			pnlReference.Visible = true;
			gvReferences.Visible = false;
		}
		protected void showReferences() {
			pnlReference.Visible = false;
			gvReferences.Visible = true;
		}
		protected void clearReferenceFields() {
			txtRefenreceEmail.Text = string.Empty;
			txtRefenreceName.Text = string.Empty;
			txtRefenrecePhone.Text = string.Empty;
			txtReferenceCompanyName.Text = string.Empty;
			txtReferencePosition.Text = string.Empty;
		}
		

		protected void btnReferenceSave_Click(object sender, EventArgs e) {
			Page.Validate("Reference");
			if (!Page.IsValid)
				return;

			AdjusterReference adjusterReference = null;
			int adjusterID = Convert.ToInt32(hdId.Value);
			int referenceID = Convert.ToInt32(ViewState["ReferenceID"]);

			if (referenceID == 0)
				adjusterReference = new AdjusterReference();
			else
				adjusterReference = AdjusterReferenceManager.Get(referenceID);

			if (adjusterReference != null) {
				adjusterReference.AdjusterID = adjusterID;
				adjusterReference.CompanyName = txtReferenceCompanyName.Text;
				adjusterReference.Email = txtRefenreceEmail.Text;
				adjusterReference.Phone = txtRefenrecePhone.Text;
				adjusterReference.Position = txtReferencePosition.Text;
				adjusterReference.RereferenceName = txtRefenreceName.Text;

				try {
					AdjusterReferenceManager.Save(adjusterReference);
					
					showReferences();

					bindReferences();
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}
		}

		protected void btnReferenceCancel_Click(object sender, EventArgs e) {
			showReferences();

			bindReferences();
		}

		protected void gvReferences_RowCommand(object sender, GridViewCommandEventArgs e) {
			int referenceID = Convert.ToInt32(e.CommandArgument);
			AdjusterReference adjusterReference = null;

			if (e.CommandName == "DoDelete") {
				AdjusterReferenceManager.Delete(referenceID);

				showReferences();

				bindReferences();
			}

			if (e.CommandName == "DoEdit") {
				ViewState["ReferenceID"] = e.CommandArgument;

				adjusterReference = AdjusterReferenceManager.Get(referenceID);

				if (adjusterReference != null) {
					txtRefenreceEmail.Text = adjusterReference.Email;
					txtRefenreceName.Text = adjusterReference.RereferenceName;
					txtRefenrecePhone.Text = adjusterReference.Phone;
					txtReferenceCompanyName.Text = adjusterReference.CompanyName;
					txtReferencePosition.Text = adjusterReference.Position;

					showReferencePanel();
				}
			}
		}
		#endregion

        protected void btnSavePayrollSetting_Click(object sender, EventArgs e)
        {

            AdjusterSettingsPayroll objSettingPayroll = null;

            objSettingPayroll = new AdjusterSettingsPayroll();
            objSettingPayroll.AdjusterId = adjusterID;
            if (ddlAdjusterRating.SelectedValue!="0")
            {
            objSettingPayroll.Rating = ddlAdjusterRating.SelectedValue;
            }
            if (!string.IsNullOrEmpty(txtAdjusterQaScore.Text))
            {
            objSettingPayroll.QAScore = int.Parse(txtAdjusterQaScore.Text, NumberStyles.AllowThousands);
            }
            if (ddlAdjusterDesignation.SelectedValue != "0")
            {
                objSettingPayroll.Designation = ddlAdjusterDesignation.SelectedValue;
            }
            if (!string.IsNullOrEmpty(txtAdjusterExperience.Text))
            {
                objSettingPayroll.AdjustingExperience = int.Parse(txtAdjusterExperience.Text, NumberStyles.AllowThousands);
            }
            if (!string.IsNullOrEmpty(txtAdjusterMaximumClaims.Text))
            {
                objSettingPayroll.MaximumNumberofClaims = int.Parse(txtAdjusterMaximumClaims.Text, NumberStyles.AllowThousands);
            }
            if (!string.IsNullOrEmpty(txtAdjusterMaximumReserve.Text))
            {
                objSettingPayroll.MaximumReserves = decimal.Parse(txtAdjusterMaximumReserve.Text.Replace('$', ' ').Trim(), NumberStyles.AllowCurrencySymbol |
                                NumberStyles.AllowLeadingWhite |
                                NumberStyles.AllowTrailingWhite |
                                NumberStyles.AllowDecimalPoint |
                                NumberStyles.AllowLeadingSign |
                                NumberStyles.AllowThousands);
            }
            if (!string.IsNullOrEmpty(txtAdjusterNationalProducer.Text))
            {
                objSettingPayroll.NationalProducerId = int.Parse(txtAdjusterNationalProducer.Text, NumberStyles.AllowThousands);
            }
            if (!string.IsNullOrEmpty(txtAdjusterGeoAreaOfService.Text))
            {
                objSettingPayroll.GeographicalAreaofService = txtAdjusterGeoAreaOfService.Text;
            }
            objSettingPayroll.IsActive = chkAdjusterIsActive.Checked;
            objSettingPayroll.Supervisor = chkAdjusterSupervisor.Checked;
            if (!string.IsNullOrEmpty(txtAdjusterEmployeeType.Text))
            {
            objSettingPayroll.EmployeeType = txtAdjusterEmployeeType.Text;
            }

            if (!string.IsNullOrEmpty(txtAdjusterHourlyRate.Text))
            {
                objSettingPayroll.DefaultAdjusterHourlyRate = decimal.Parse(txtAdjusterHourlyRate.Text.Replace('$', ' ').Trim(), NumberStyles.AllowCurrencySymbol |
                                NumberStyles.AllowLeadingWhite |
                                NumberStyles.AllowTrailingWhite |
                                NumberStyles.AllowDecimalPoint |
                                NumberStyles.AllowLeadingSign |
                                NumberStyles.AllowThousands);
            }
            if (!string.IsNullOrEmpty(txtAdjusterComissionRate.Text))
            {
                objSettingPayroll.DefaultAdjusterCommissionRate = decimal.Parse(txtAdjusterComissionRate.Text.Replace('%', ' ').Trim(), NumberStyles.AllowCurrencySymbol |
                                NumberStyles.AllowLeadingWhite |
                                NumberStyles.AllowTrailingWhite |
                                NumberStyles.AllowDecimalPoint |
                                NumberStyles.AllowLeadingSign |
                                NumberStyles.AllowThousands);
            }

            objSettingPayroll.IndependenContractorAgreementOnFile = chkAdjusterContractorAgreementOnFile.Checked;
            if (!string.IsNullOrEmpty(txtAdjusterLastYearAgreementoOnFile.Text))
            {
                objSettingPayroll.LastYear1099AgreementonFile = int.Parse(txtAdjusterLastYearAgreementoOnFile.Text, NumberStyles.AllowThousands);
            }
            objSettingPayroll.ResumeOnFile = chkResumeOnFile.Checked;
            objSettingPayroll.AdjusterBranch = txtAdjusterBranch.Text.Trim();
            objSettingPayroll.BranchCode = txtBranchCode.Text.Trim();

            bool exist = AdjusterManager.GetAdjusterSttingPayroll(this.adjusterID);
            if (exist)
            {
                AdjusterManager.UpdatePayrollSetting(objSettingPayroll);
            }
            else
            {
                AdjusterManager.SavePayrollSetting(objSettingPayroll);
            }


        }

        protected void BindPayrollSetting()
        {

            AdjusterSettingsPayroll objSettingPayroll = AdjusterManager.GetAdjusterSttingPayrollData(adjusterID);

            if (objSettingPayroll != null)
            {
                txtAdjusterBranch.Text = objSettingPayroll.AdjusterBranch;
                txtBranchCode.Text = objSettingPayroll.BranchCode;

                if (!string.IsNullOrEmpty( objSettingPayroll.Rating))
                {
                ddlAdjusterRating.SelectedValue = objSettingPayroll.Rating;
                }
                else
                {
                    ddlAdjusterRating.SelectedValue = "0";
                }
                txtAdjusterQaScore.Text = Convert.ToString(objSettingPayroll.QAScore);
                if (!string.IsNullOrEmpty(objSettingPayroll.Designation))
                {
                    ddlAdjusterDesignation.SelectedValue = objSettingPayroll.Designation;
                }
                else
                {
                    ddlAdjusterDesignation.SelectedValue = "0";
                }
                txtAdjusterExperience.Text = Convert.ToString(objSettingPayroll.AdjustingExperience);
                txtAdjusterMaximumClaims.Text = Convert.ToString(objSettingPayroll.MaximumNumberofClaims);
                txtAdjusterMaximumReserve.Text = Convert.ToString(objSettingPayroll.MaximumReserves);
                txtAdjusterNationalProducer.Text = Convert.ToString(objSettingPayroll.NationalProducerId);
                txtAdjusterGeoAreaOfService.Text = objSettingPayroll.GeographicalAreaofService;
                chkAdjusterIsActive.Checked = objSettingPayroll.IsActive ?? false;
                chkAdjusterSupervisor.Checked = objSettingPayroll.Supervisor ?? false;
                txtAdjusterEmployeeType.Text = objSettingPayroll.EmployeeType;
                txtAdjusterHourlyRate.Text = Convert.ToString(objSettingPayroll.DefaultAdjusterHourlyRate);
                txtAdjusterComissionRate.Text = Convert.ToString(objSettingPayroll.DefaultAdjusterCommissionRate);
                chkAdjusterContractorAgreementOnFile.Checked = objSettingPayroll.IndependenContractorAgreementOnFile ?? false;
                txtAdjusterLastYearAgreementoOnFile.Text = Convert.ToString(objSettingPayroll.LastYear1099AgreementonFile);
                chkResumeOnFile.Checked = objSettingPayroll.ResumeOnFile ?? false;
            }
        }


        protected void BindDeploymentEvent()
        {
            lbtnEventSave.Visible = true;
            List<AdjusterDeploymentEvent> lstAdjusterDeploymentEvent = AdjusterManager.GetDeploymentEvent(adjusterID);
            gvDeployEvents.DataSource = lstAdjusterDeploymentEvent.OrderByDescending(x=>x.Id);
            gvDeployEvents.DataBind();
        }

        protected void btnEventSave_Click(object sender, EventArgs e)
        {
            if (txtEventName.Text.Trim() != string.Empty && txtEventType.Text.Trim() != string.Empty && txtEventDeploymentDate.Text.Trim() != string.Empty)
            {
            AdjusterDeploymentEvent objAdjusterDeploymentEvent = new AdjusterDeploymentEvent();
            objAdjusterDeploymentEvent.AdjusterId = adjusterID;
            objAdjusterDeploymentEvent.EventName = txtEventName.Text;
            objAdjusterDeploymentEvent.EventType = txtEventType.Text;
            if (!string.IsNullOrEmpty(txtEventDeploymentDate.Text))
            {
                objAdjusterDeploymentEvent.DeploymentDate = Convert.ToDateTime(txtEventDeploymentDate.Text);
            }
            if (!string.IsNullOrEmpty(txtEventArrivalDate.Text))
            {
                objAdjusterDeploymentEvent.ArrivalDate = Convert.ToDateTime(txtEventArrivalDate.Text);
            }
            if (!string.IsNullOrEmpty(txtEventDepartureDate.Text))
            {
                objAdjusterDeploymentEvent.DepartureDate = Convert.ToDateTime(txtEventDepartureDate.Text);
            }
            if (!string.IsNullOrEmpty(txtEventDateReturned.Text))
            {
                objAdjusterDeploymentEvent.DateReturned = Convert.ToDateTime(txtEventDateReturned.Text);
            }
            if (ddlDeploymentAddress.SelectedValue!="0")
            {
            objAdjusterDeploymentEvent.DeploymentAddressId =Convert.ToInt32(ddlDeploymentAddress.SelectedValue);
            }
            AdjusterManager.SaveEvent(objAdjusterDeploymentEvent);
            divEventAdd.Visible = false;
            BlankEventControl();
            BindDeploymentEvent();
            }
           
        }

        protected void lbtnEventSave_Click(object sender, EventArgs e)
        {
            if(adjusterID>0)
            {
            divEventAdd.Visible = true;
            txtEventName.Focus();
            }
        }

        protected void btnEventCancel_Click(object sender, EventArgs e)
        {
            divEventAdd.Visible = false;
            BlankEventControl();
        }

        private void BlankEventControl()
        {
            txtEventName.Text = string.Empty;
            txtEventType.Text = string.Empty;
            txtEventDeploymentDate.Text = string.Empty;
            txtEventDepartureDate.Text = string.Empty;
            txtEventDateReturned.Text = string.Empty;
            txtEventArrivalDate.Text = string.Empty;
        }

		//protected void webUpload1_OnUploadFinishing(object sender, UploadFinishingEventArgs e) {
		//	int adjusterID = Convert.ToInt32(hdId.Value);
		//	int stateID = Convert.ToInt32(Session["stateID"]);

		//	string imageFolderPath = null;
		//	string destinationFilePath = null;

		//	// get application path
		//	string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

		//	// get uploaded file
		//	string tempFilePath = String.Format("{0}{1}", e.FolderPath, e.TemporaryFileName);

		//	if (adjusterID > 0 && stateID > 0 && File.Exists(tempFilePath)) {
		//		try {

		//			//imageFolderPath = HttpContext.Current.Server.MapPath("~/LeadsImage/" + LeadId.ToString() + "/" + objLeadImage.LeadImageId.ToString());
		//			imageFolderPath = string.Format("{0}/Adjusters/{1}/License", appPath, adjusterID);

		//			if (!Directory.Exists(imageFolderPath))
		//				Directory.CreateDirectory(imageFolderPath);


		//			//destinationFilePath = HttpContext.Current.Server.MapPath("~/LeadsImage/" + LeadId.ToString() + "/" + objLeadImage.LeadImageId.ToString() + "/" + e.FileName);
		//			destinationFilePath = string.Format("{0}/{1}{2}", imageFolderPath, stateID, Path.GetExtension(e.FileName));

		//			System.IO.File.Copy(tempFilePath, destinationFilePath, true);

		//			// delete temp file
		//			File.Delete(tempFilePath);
		//		}
		//		catch (Exception ex) {
		//			Core.EmailHelper.emailError(ex);
		//		}
		//		finally {
					
		//		}
		//	}
		//}

		//protected void WebUpload_photo_OnUploadFinishing(object sender, UploadFinishingEventArgs e) {
		//	int adjusterID = Convert.ToInt32(hdId.Value);
		//	AdjusterMaster adjuster = null;
		//	string photoFileName = null;

		//	string imageFolderPath = null;
		//	string destinationFilePath = null;

		//	// get application path
		//	string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

		//	// get uploaded file
		//	string tempFilePath = String.Format("{0}{1}", e.FolderPath, e.TemporaryFileName);

		//	adjuster = AdjusterManager.GetAdjusterId(adjusterID);

		//	if (adjuster != null && File.Exists(tempFilePath)) {				
		//		try {
		//			imageFolderPath = string.Format("{0}/Adjusters/{1}/Photo", appPath, adjuster.AdjusterId);

		//			if (!Directory.Exists(imageFolderPath))
		//				Directory.CreateDirectory(imageFolderPath);

		//			photoFileName = adjuster.AdjusterId.ToString() + Path.GetExtension(e.FileName);

		//			destinationFilePath = string.Format("{0}/{1}", imageFolderPath, photoFileName);

		//			System.IO.File.Copy(tempFilePath, destinationFilePath, true);

		//			// delete temp file
		//			File.Delete(tempFilePath);

		//			adjuster.PhotoFileName = photoFileName;

		//			AdjusterManager.Save(adjuster);
		//		}
		//		catch (Exception ex) {
		//			Core.EmailHelper.emailError(ex);
		//		}
		//		finally {

		//		}
		//	}
		//}

		
		

	}
}