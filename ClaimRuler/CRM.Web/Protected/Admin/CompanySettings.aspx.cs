using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using CRM.Data;
using CRM.Data.Account;

using CRM.Core;
using CRM.Repository;
using CRM.Data.Entities;
using System.Drawing;

namespace CRM.Web.Protected.Admin {
	public partial class CompanySettings : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			Master.checkPermission();

			if (!Page.IsPostBack) {
				bindState();

				fillForm();
			}

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetSlidingExpiration(true); 
		}

		private void bindState() {
			CollectionManager.FillCollection(ddlState, "StateId", "StateName", State.GetAll());
		}

		protected void btnUpload_Click(object sender, EventArgs e) {
            lblImgUploadSize.Visible = false;
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			string filePath = null;
            string filePathCheck = null;
			string fileExtension = null;
			int clientID = 0;

			Page.Validate("newLogo");
			if (!Page.IsValid)
				return;
			clientID = SessionHelper.getClientId();
			//clientLogo.ImageUrl = string.Format("{0}/ClientLogo/{1}.jpg", appPath, clientID);           

            if (clientID > 0 && fileUpload.HasFile)
            {
                fileExtension = System.IO.Path.GetExtension(fileUpload.FileName);
               filePath = Server.MapPath(string.Format(@"~\ClientLogo\{0}.jpg", clientID));
                //filePath = string.Format("/ClientLogo/{0}.jpg", clientID);              

               string checkfolderpath = @"~\ClientLogoCheck"; // your code goes here

               bool isExists = System.IO.Directory.Exists(Server.MapPath(checkfolderpath));

               if (!isExists)
                   System.IO.Directory.CreateDirectory(Server.MapPath(checkfolderpath));


               filePathCheck = Server.MapPath(string.Format(@"~\ClientLogoCheck\{0}.jpg", clientID));
               fileUpload.SaveAs(filePathCheck);


               Bitmap bmp = new Bitmap(filePathCheck);
                int widthImage = bmp.Width;               
                bmp.Dispose();
                if (widthImage <= 600)
                {
                    File.Delete(filePathCheck);
                    fileUpload.SaveAs(filePath);
                    filePath = string.Format(@"~\ClientLogo\{0}.jpg", clientID);
                    clientLogo.ImageUrl = filePath;// Server.MapPath(filePath);
                }
                else
                {
                    File.Delete(filePathCheck);
                    lblImgUploadSize.Visible = true;
                }
                //showLogo(clientID);
            }
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			int clientID = 0;
			Client client = null;

			Page.Validate("Client");
            lblImgUploadSize.Visible = false;
			if (!Page.IsValid)
				return;

			clientID = SessionHelper.getClientId();

			client = ClientManager.Get(clientID);

			if (client != null) {
				try {

					client.BusinessName = txtBusinessName.Text.Trim();

					client.StreetAddress1 = txtAddress.Text.Trim();

					client.CityId = Convert.ToInt32(ddlCity.SelectedValue);

					client.StateId = Convert.ToInt32(ddlState.SelectedValue);

					client.ZipCode = ddlZipCode.SelectedValue;

					client.PrimaryPhoneNo = txtPhone.Text.Trim();

					client.SecondaryPhoneNo = txtFaxNumber.Text.Trim();

					client.PrimaryEmailId = txtEmail.Text.Trim();


					client.FederalIDNo = txtFederalTaxID.Text.Trim();

					client.isShowTasks = cbxShowTasks.Checked;

					client.InactivityDays = string.IsNullOrEmpty(txtInactivityPeriod.Text) ? 0 : Convert.ToInt32(txtInactivityPeriod.Text);

					// imap settings
					client.imapHost = txtImapHost.Text.Trim();

					client.imapHostPort = string.IsNullOrEmpty(txtImapHostPort.Text) ? 0 : Convert.ToInt32(txtImapHostPort.Text);

					client.imapHostUseSSL = cbxImapUseSSL.Checked;
				
					// save client
					clientID = ClientManager.Save(client);
					

					lblMessage.Text = "Changes saved successfully.";
					lblMessage.CssClass = "ok";
				}
				catch (Exception ex) {
					lblMessage.Text = "Changes were not saved.";
					lblMessage.CssClass = "error";

					Core.EmailHelper.emailError(ex);
				}
			} // if (client != null) {			
		}


		private void fillForm() {
			int clientID = SessionHelper.getClientId();

			Client client = ClientManager.Get(clientID);

			if (client != null) {				

				txtAddress.Text = client.StreetAddress1 ?? "";


				txtEmail.Text = client.PrimaryEmailId ?? "";

				txtBusinessName.Text = client.BusinessName;

				txtPhone.Text = client.PrimaryPhoneNo;

				txtFaxNumber.Text = client.SecondaryPhoneNo;
			
				txtFederalTaxID.Text = client.FederalIDNo;


				txtInactivityPeriod.Text = client.InactivityDays == null ? "0" : client.InactivityDays.ToString();

				txtImapHost.Text = client.imapHost;

				txtImapHostPort.Text = client.imapHostPort.ToString();

				cbxImapUseSSL.Checked = client.imapHostUseSSL ?? true;

			
			
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

				

				cbxShowTasks.Checked = (client.isShowTasks ?? false);
			}
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

		protected void showLogo(int clientID) {
			//string appUrl = ConfigurationManager.AppSettings["appURL"].ToString();

			//clientLogo.ImageUrl = string.Format("{0}/ClientLogo/{1}.jpg", appUrl, clientID);
            clientLogo.ImageUrl = string.Format(@"~\ClientLogo\{0}.jpg", clientID);
		}

	}
}