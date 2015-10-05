using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CRM.Data;
using CRM.Data.Account;

using CRM.Core;
using CRM.Repository;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class ClientEdit : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			int clientID = 0;

			if (!Page.IsPostBack) {
				bindState();

				if (Session["EditClientId"] != null && int.TryParse(Session["EditClientId"].ToString(), out clientID) && clientID > 0) {
					fillForm(clientID);
				}
			}
		}

		private void fillForm(int clientID) {
			Client client = ClientManager.Get(clientID);

			if (client != null) {
				hfclientid.Value = clientID.ToString();

				ddlClientType.SelectedValue = (client.ClientTypeID ?? 1).ToString();	// default to public adjuster

				txtAddress.Text = client.StreetAddress1 ?? "";

				txtFirstName.Text = client.FirstName;

				txtLastName.Text = client.LastName;

				txtEmail.Text = client.PrimaryEmailId ?? "";

				txtBusinessName.Text = client.BusinessName;

				txtPhone.Text = client.PrimaryPhoneNo;

				txtFaxNumber.Text = client.SecondaryPhoneNo;

				txtNumberUsers.Text = client.maxUsers.ToString();

				txtFederalTaxID.Text = client.FederalIDNo;

				txtFeePerContract.Text = string.Format("{0:N2}", client.FeePerContract);

				txtMaxLeads.Text = client.MaxLeads.ToString();

				cbxTrial.Checked = client.isTrial ?? false;

				txtInactivityPeriod.Text = client.InactivityDays == null ? "0" : client.InactivityDays.ToString();

				txtImapHost.Text = client.imapHost;

				txtImapHostPort.Text = client.imapHostPort.ToString();

				cbxImapUseSSL.Checked = client.imapHostUseSSL ?? true;

				if (client.InvoiceSettingID != null)
					ddlInvoiceSetting.SelectedValue = client.InvoiceSettingID.ToString();

				txtContingencyFee.Value = client.InvoiceContingencyFee ?? 0;

				showLogo(client.ClientId);

				if (client.StateId != null) {
					ddlState.SelectedValue = client.StateId.ToString();

					CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll(client.StateId ?? 0));

					ddlCity.SelectedValue = client.CityId != null ? client.CityId.ToString() : "";

					if (client.CityId != null) {
						ddlCity.SelectedValue = client.CityId.ToString();

						CollectionManager.FillCollection(ddlZipCode, "ZipCodeID", "ZipCode", ZipCode.getByCityID(client.CityId ?? 0));
						ddlZipCode.SelectedValue = client.ZipCode != null ? client.ZipCode : "";
					}
				}

				if (client.SecUser != null) {
					// show user name
					txtUserName.Text = client.SecUser.UserName;

					// disable user name
					txtUserName.Enabled = false;
				}

				cbxShowTasks.Checked = (client.isShowTasks ?? false);

				if (client.UserId != null)
					hfuserid.Value = client.UserId.ToString();
			}
		}

		protected void btnUpload_Click(object sender, EventArgs e) {
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			string filePath = null;
			string fileExtension = null;
			int clientID = 0;

			Page.Validate("newLogo");
			if (!Page.IsValid)
				return;

			int.TryParse(Session["EditClientId"].ToString(), out clientID);

			clientLogo.ImageUrl = string.Format("{0}/ClientLogo/{1}.jpg", appPath, clientID);

			if (clientID > 0 && fileUpload.HasFile) {

				fileExtension = System.IO.Path.GetExtension(fileUpload.FileName);

				filePath = appPath + "/ClientLogo/" + clientID.ToString() + fileExtension;

				fileUpload.SaveAs(filePath);

				showLogo(clientID);
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			int clientID = 0;
			Client client = null;
			CRM.Data.Entities.SecUser user = null;
			bool isNew = false;
			int invoiceSettingID = 0;
			int userID = 0;


			Page.Validate("Client");

			if (!Page.IsValid)
				return;


			if (int.TryParse(hfclientid.Value, out clientID) && clientID > 0) {
				// update existing
				client = ClientManager.Get(clientID);
			}
			else {
				// new user
				client = new Client();

				// default value
				client.InactivityDays = 30;

				isNew = true;
			}

			if (client != null) {
				try {
					client.ClientTypeID = Convert.ToInt32(ddlClientType.SelectedValue);

					client.FirstName = txtFirstName.Text.Trim();

					client.LastName = txtLastName.Text.Trim();

					client.BusinessName = txtBusinessName.Text.Trim();

					client.StreetAddress1 = txtAddress.Text.Trim();

					client.CityId = Convert.ToInt32(ddlCity.SelectedValue);

					client.StateId = Convert.ToInt32(ddlState.SelectedValue);

					client.ZipCode = ddlZipCode.SelectedValue;

					client.PrimaryPhoneNo = txtPhone.Text.Trim();

					client.SecondaryPhoneNo = txtFaxNumber.Text.Trim();

					client.PrimaryEmailId = txtEmail.Text.Trim();

					client.Active = 1;

					client.maxUsers = string.IsNullOrEmpty(txtNumberUsers.Text) ? 1 : Convert.ToInt32(txtNumberUsers.Text);

					client.FeePerContract = string.IsNullOrEmpty(txtFeePerContract.Text) ? 1 : Convert.ToDecimal(txtFeePerContract.Text);

					client.FederalIDNo = txtFederalTaxID.Text.Trim();

					client.MaxLeads = Convert.ToInt32(string.IsNullOrEmpty(txtMaxLeads.Text) ? "0" : txtMaxLeads.Text);

					client.isTrial = cbxTrial.Checked;

					client.isShowTasks = cbxShowTasks.Checked;

					client.InactivityDays = string.IsNullOrEmpty(txtInactivityPeriod.Text) ? 0 : Convert.ToInt32(txtInactivityPeriod.Text);

					// imap settings
					client.imapHost = txtImapHost.Text.Trim();

					client.imapHostPort = string.IsNullOrEmpty(txtImapHostPort.Text) ? 0 : Convert.ToInt32(txtImapHostPort.Text);

					client.imapHostUseSSL = cbxImapUseSSL.Checked;

					// invoice billing settings
					invoiceSettingID = Convert.ToInt32(ddlInvoiceSetting.SelectedValue);
					if (invoiceSettingID > 0)
						client.InvoiceSettingID = invoiceSettingID;

					client.InvoiceContingencyFee = txtContingencyFee.Value == null ? 0 : Convert.ToDecimal(txtContingencyFee.Value);

					// save client
					clientID = ClientManager.Save(client);

					if (isNew) {
						try {
							user = new CRM.Data.Entities.SecUser();
							user.UserName = txtUserName.Text.Trim();
							user.FirstName = txtFirstName.Text.Trim();
							user.LastName = txtLastName.Text.Trim();
							user.Status = true;
							user.RoleId = (int)UserRole.Client;
							user.Email = txtEmail.Text.Trim();
							user.Password = SecurityManager.Encrypt(txtUserPassword.Text.Trim());
							user.CreatedOn = DateTime.Now;
							user.CreatedBy = 1;
							user.UpdatedOn = DateTime.Now;
							user.UpdatedBy = 1;
							user.ClientID = clientID;

							CRM.Data.Entities.SecUser newUser = SecUserManager.Save(user);

							client = ClientManager.Get(clientID);

							if (client != null) {
								client.UserId = newUser.UserId;

								ClientManager.Save(client);
							}

						}
						catch (Exception ex) {
							lblMessage.Text = "Unable to create new user account.";
							lblMessage.CssClass = "error";

							Core.EmailHelper.emailError(ex);
						}
					}
					else {
						// update user
						if (int.TryParse(hfuserid.Value, out userID) && userID > 0) {
							user = SecUserManager.GetByUserId(userID);

							if (user != null) {
								user.FirstName = txtFirstName.Text.Trim();
								user.LastName = txtLastName.Text.Trim();

								if (!string.IsNullOrEmpty(txtUserPassword.Text))
									user.Password = SecurityManager.Encrypt(txtUserPassword.Text.Trim());

								SecUserManager.Save(user);
							}
						}
					}
					
					lblMessage.Text = "Client changes saved.";
					lblMessage.CssClass = "ok";
				}
				catch (Exception ex) {
					lblMessage.Text = "Unable to create new user account.";
					lblMessage.CssClass = "error";

					Core.EmailHelper.emailError(ex);
				}
			} // if (client != null) {


			// clear after edit
			//Session.Remove("EditClientId");

			//Response.Redirect("~/Protected/Admin/ClientList.aspx");
		}

		private void bindState() {
			CollectionManager.FillCollection(ddlState, "StateId", "StateName", State.GetAll());
		}
		
		protected void ddlState_selectedIndex(object sender, EventArgs e) {
			if (ddlState.SelectedIndex > 0) {
				CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll(Convert.ToInt32(ddlState.SelectedValue)));
			}
			else {
				ddlCity.Items.Clear();
			}
		}

		protected void dllCity_SelectedIndexChanged(object sender, EventArgs e) {
			if (ddlCity.SelectedIndex > 0) {
				CollectionManager.FillCollection(ddlZipCode, "ZipCodeID", "ZipCode", ZipCode.getByCityID(Convert.ToInt32(ddlCity.SelectedValue)));
			}
			else {
				ddlZipCode.Items.Clear();
			}

		}



		protected void btnCancel_Click(object sender, EventArgs e) {
			// clear after edit
			Session.Remove("EditClientId");

			Response.Redirect("~/Protected/Admin/ClientList.aspx");
		}

		protected void showLogo(int clientID) {
			string appUrl = ConfigurationManager.AppSettings["appURL"].ToString();

			clientLogo.ImageUrl = string.Format("{0}/ClientLogo/{1}.jpg", appUrl, clientID);
		}

		protected void ddlInvoiceSetting_SelectedIndexChanged(object sender, EventArgs e) {
			bool isvisible = ddlInvoiceSetting.SelectedValue.Equals("1");

			tr_percentage.Visible = isvisible;
		}

		protected void btnImportSettings_Click(object sender, EventArgs e) {
			int terryDemoID = 13;
			int clientID = Convert.ToInt32(Session["EditClientId"]);
			Client client = ClientManager.Get(clientID);

			int userID = (int)client.UserId;

			copyMasterStatus(terryDemoID, clientID, userID);
			
			copyMasterSubStatus(terryDemoID, clientID, userID);

			copySubLimitOfLiability(terryDemoID, clientID);

			copyLeadSource(terryDemoID, clientID, userID);

			copyInvoiceServices(terryDemoID, clientID, userID);
		}

		private void copySubLimitOfLiability(int sourceClientID, int targetClientID) {
			List<SubLimitOfLiabilityMaster> sublimits = null;

			sublimits = SubLimitOfLiabilityManager.GetAll(sourceClientID);
			foreach (SubLimitOfLiabilityMaster sublimit in sublimits) {
				SubLimitOfLiabilityMaster newSublimit = new SubLimitOfLiabilityMaster();
				newSublimit.ClientId = targetClientID;
				newSublimit.Description = sublimit.Description;
				newSublimit.IsActive = sublimit.IsActive;
				SubLimitOfLiabilityManager.Save(sublimit);
			}
		}

		private void copyInvoiceServices(int sourceClientID, int targetClientID, int userID) {
			//List<InvoiceService> services = null;
			List<InvoiceServiceType> serviceTypes = null;
			List<InvoiceServiceUnit> serviceUnits = null;

			// invoice service units
			serviceUnits = InvoiceServiceManager.GetServiceUnits(sourceClientID);
			if (serviceUnits != null) {
				foreach (InvoiceServiceUnit serviceUnit in serviceUnits) {
					InvoiceServiceUnit newServiceUnit = new InvoiceServiceUnit();
					newServiceUnit.ClientID = targetClientID;
					newServiceUnit.IsActive = serviceUnit.IsActive;
					newServiceUnit.UnitDescription = serviceUnit.UnitDescription;

					InvoiceServiceUnitManager.Save(newServiceUnit);
				}
			}

			// invoice service  types
			serviceTypes = InvoiceServiceManager.GetAll(sourceClientID).ToList();

			foreach (InvoiceServiceType serviceType in serviceTypes) {
				InvoiceServiceType newServiceType = new InvoiceServiceType();
				newServiceType.ClientID = targetClientID;
				newServiceType.isActive = serviceType.isActive;
				newServiceType.ServiceDescription = serviceType.ServiceDescription;
				newServiceType.ServicePercentage = serviceType.ServicePercentage;
				newServiceType.ServiceRate = serviceType.ServiceRate;

				InvoiceServiceManager.Save(newServiceType);
			}

		}
		
		private void copyLeadSource(int sourceClientID, int targetClientID, int userID) {
			List<LeadSourceMaster> sources = LeadSourceManager.GetAll(sourceClientID);
			LeadSourceMaster newSource = null;

			if (sources != null) {
				foreach(LeadSourceMaster source in sources){
					newSource = new LeadSourceMaster();
					newSource.ClientId = targetClientID;
					newSource.InsertBy = userID;
					newSource.InsertDate = DateTime.Now;
					newSource.LeadSourceName = source.LeadSourceName;
					newSource.Status = source.Status;

					LeadSourceManager.Save(newSource);
				}
			}
		}
		
		private void copyMasterStatus(int sourceClientID, int targetClientID, int clientUserID) {
			List<StatusMaster> statuses = null;

			// get list of status from model
			statuses = StatusManager.GetAll(sourceClientID);
			foreach (StatusMaster status in statuses) {
				StatusMaster newStatusMaster = new StatusMaster();
				newStatusMaster.clientID = targetClientID;
				newStatusMaster.InsertBy = clientUserID; 
				newStatusMaster.InsertDate = DateTime.Now;
				newStatusMaster.isCountable = status.isCountable;
				newStatusMaster.Status = status.Status;
				newStatusMaster.StatusName = status.StatusName;
				newStatusMaster.UpdateBy = clientUserID;
				newStatusMaster.UpdateDate = DateTime.Now;
				newStatusMaster.isCountAsOpen = status.isCountAsOpen;

				StatusManager.Save(newStatusMaster);
			}
		}

		private void copyMasterSubStatus(int sourceClientID, int targetClientID, int clientUserID) {
			List<SubStatusMaster> statuses = null;

			// get list of status from model
			statuses = SubStatusManager.GetAll(sourceClientID);
			foreach (SubStatusMaster status in statuses) {
				SubStatusMaster newStatusMaster = new SubStatusMaster();
				newStatusMaster.clientID = targetClientID;
				newStatusMaster.InsertBy = clientUserID;
				newStatusMaster.InsertDate = DateTime.Now;
				newStatusMaster.Status = status.Status;
				newStatusMaster.UpdateBy = clientUserID;
				newStatusMaster.UpdateDate = DateTime.Now;

				SubStatusManager.Save(newStatusMaster);
			}
		}

	}
}