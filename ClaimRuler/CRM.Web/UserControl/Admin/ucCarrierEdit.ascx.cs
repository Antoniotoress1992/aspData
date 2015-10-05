using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Data.Account;
using CRM.Data;
using CRM.Core;
using System.Transactions;
using CRM.Web.Utilities;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucCarrierEdit : System.Web.UI.UserControl {

		private int carrierID {
			get {
				return Session["CarrierID"] != null ? Convert.ToInt32(Session["CarrierID"]) : 0;
			}
		}
		int clientID = SessionHelper.getClientId();
		private string[] carrierInvoiceProfileTypes = { "N/A", "Flat Rate per File", "CAT Rate per File", "Itemized Billing per File" };
		protected void Page_Load(object sender, EventArgs e) {
			this.Page.Form.DefaultButton = btnSave.UniqueID;

			if (!Page.IsPostBack) {
				DoBind();
			}
		}

		#region ================== Carrier methods ==================
		private void DoBind() {
			Carrier carrier = null;

			List<StateMaster> states = State.GetAll();

			CollectionManager.FillCollection(ddlCountry, "CountryId", "CountryName", CountryMasterManager.GetAll());

			CollectionManager.FillCollection(ddlState, "StateId", "StateName", states);

			if (this.carrierID > 0) {
				tabContainer.Visible = true;
				

				carrier = CarrierManager.Get(carrierID);

				if (carrier != null) {
					txtName.Text = carrier.CarrierName;
					txtAddress.Text = carrier.AddressLine1;
					txtAddress2.Text = carrier.AddressLine2;

					if (carrier.CountryID != null)
						ddlCountry.SelectedValue = carrier.CountryID.ToString();

					if (carrier.StateID != null) {
						ddlState.SelectedValue = carrier.StateID.ToString();

						bindStateCities(carrier.StateID ?? 0);
					}

					if (carrier.CityID != null) {
						ddlCity.SelectedValue = carrier.CityID.ToString();
					}

					txtZipCode.Text = carrier.ZipCode;

					showLocations();

					bindLocations();

					bindInvoiceProfiles();

					carrierContacts.bindData(carrierID);

					carrierComments.bindData();

					carrierDocuments.bindData(carrierID);

					carrierTasks.bindData();
				}
			}
		}

		protected void btnClose_Click(object sender, EventArgs e) {
			// end edit 
			Session["CarrierID"] = null;

			// go back to list
			Response.Redirect("~/Protected/Admin/CarrierList.aspx");
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;

			Page.Validate("carrier");
			if (!Page.IsValid)
				return;

			try {
				saveCarrier();

				lblSave.Text = "Carrier information saved successfully.";

				lblSave.Visible = true;


			}

			catch (Exception ex) {
				lblError.Visible = true;
				lblError.Text = "Unable to save Carrier information!";
			}
		}

		private void bindInvoiceType() {
			CollectionManager.FillCollection(ddlInvoiceType, "InvoiceTypeID", "InvoiceType", CarrierInvoiceTypeManager.GetAll());
		}

		private void bindStateCities(int stateID) {
			ddlCity.Items.Clear();

			CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll(stateID));
		}

		protected void ddlState_SelectedIndexChanged(object sender, EventArgs e) {
			if (ddlState.SelectedIndex > 0)
				bindStateCities(Convert.ToInt32(ddlState.SelectedValue));
		}

		protected void saveCarrier() {
			Carrier carrier = null;
			int countryID = 0;

			using (TransactionScope scope = new TransactionScope()) {
				if (carrierID == 0)
					carrier = new Carrier();						// new carrier
				else
					carrier = CarrierManager.Get(carrierID);		// update carrier

				if (carrier != null) {
					carrier.CarrierName = txtName.Text;
					carrier.AddressLine1 = txtAddress.Text.Trim();
					carrier.AddressLine2 = txtAddress2.Text.Trim();

					if (ddlState.SelectedIndex > 0) {
						carrier.StateID = Convert.ToInt32(ddlState.SelectedValue);
						carrier.StateName = ddlState.SelectedItem.Text;
					}

					if (ddlCity.SelectedIndex > 0)
						carrier.CityID = Convert.ToInt32(ddlCity.SelectedValue);

					carrier.ZipCode = txtZipCode.Text;

					carrier.IsActive = true;

					carrier.ClientID = clientID;

					countryID = Convert.ToInt32(ddlCountry.SelectedValue);
					if (countryID > 0)
						carrier.CountryID = countryID;

					carrier = CarrierManager.Save(carrier);

					scope.Complete();

					Session["CarrierID"] = carrier.CarrierID;

					tabContainer.Visible = true;
				}
			}

		}
		#endregion

		#region ================== Location methods ==================
		private void bindLocationCountry() {
			List<StateMaster> states = State.GetAll();

			CollectionManager.FillCollection(ddlLocationCountry, "CountryId", "CountryName", CountryMasterManager.GetAll());
		}

		private void bindLocationState() {
			CollectionManager.FillCollection(ddlLocationState, "StateId", "StateName", State.GetAll());
		}

		private void bindLocationStateCities(int stateID) {
			ddlLocationCity.Items.Clear();

			CollectionManager.FillCollection(ddlLocationCity, "CityId", "CityName", City.GetAll(stateID));
		}

		protected void ddlLocationState_SelectedIndexChanged(object sender, EventArgs e) {
			if (ddlLocationState.SelectedIndex > 0)
				bindLocationStateCities(Convert.ToInt32(ddlLocationState.SelectedValue));
		}


		protected void gvLocation_RowDataBound(object sender, GridViewRowEventArgs e) {

		}

		protected void gvLocation_RowCommand(object sender, GridViewCommandEventArgs e) {
			CarrierLocation location = null;
			int carrierLocationID = Convert.ToInt32(e.CommandArgument);

			if (e.CommandName == "DoEdit") {

				CollectionManager.FillCollection(ddlLocationState, "StateId", "StateName", State.GetAll());

				location = CarrierLocationManager.Get(carrierLocationID);

				if (location != null) {
					ViewState["CarrierLocationID"] = location.CarrierLocationID.ToString();

					showLocationPanel();

					bindLocationCountry();

					txtLocationAddressLine1.Text = location.AddressLine1;
					txtLocationAddressLine2.Text = location.AddressLine2;

					if (location.StateId != null) {
						ddlLocationState.SelectedValue = location.StateId.ToString();

						bindLocationStateCities((int)location.StateId);
					}

					if (location.CityId != null) {
						ddlLocationCity.SelectedValue = location.CityId.ToString();
					}

					txtLocationName.Text = location.LocationName;
					txtLocationZipCode.Text = location.ZipCode;
					txtLocationDepartment.Text = location.DepartmentName;

					if (location.CountryID != null)
						ddlLocationCountry.SelectedValue = location.CountryID.ToString();
				}
			}

			if (e.CommandName == "DoDelete") {
				location = CarrierLocationManager.Get(carrierLocationID);

				if (location != null) {
					location.IsActive = false;

					CarrierLocationManager.Save(location);


					showLocations();

					bindLocations();
				}
			}
		}

		protected void lbtnNewLocation_Click(object sender, EventArgs e) {
			ViewState["CarrierLocationID"] = "0";

			showLocationPanel();

			bindLocationCountry();

			bindLocationState();
		}

		protected void btnLocationSave_Click(object sender, EventArgs e) {
			CarrierLocation location = null;

			Page.Validate("Location");
			if (!Page.IsValid)
				return;

			int countryID = Convert.ToInt32(ddlLocationCountry.SelectedValue);

			try {
				int carrierLocationID = Convert.ToInt32(ViewState["CarrierLocationID"]);


				if (carrierLocationID == 0) {
					location = new CarrierLocation();
					location.IsActive = true;
				}
				else
					location = CarrierLocationManager.Get(carrierLocationID);

				if (location != null) {
					location.CarrierID = carrierID;
					location.LocationName = txtLocationName.Text;
					location.AddressLine1 = txtLocationAddressLine1.Text;
					location.AddressLine2 = txtLocationAddressLine2.Text;

					if (ddlLocationState.SelectedIndex > 0)
						location.StateId = Convert.ToInt32(ddlLocationState.SelectedValue);

					if (ddlLocationCity.SelectedIndex > 0)
						location.CityId = Convert.ToInt32(ddlLocationCity.SelectedValue);

					location.ZipCode = txtLocationZipCode.Text;
					location.DepartmentName = txtLocationDepartment.Text;

					if (countryID > 0)
						location.CountryID = countryID;

					CarrierLocationManager.Save(location);

					showLocations();

					bindLocations();
				}
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
			}
		}

		protected void btnLocationCancel_Click(object sender, EventArgs e) {
			showLocations();
		}

		private void bindLocations() {
			gvLocation.DataSource = CarrierLocationManager.GetAll(carrierID);
			gvLocation.DataBind();
		}

		private void showLocationPanel() {
			pnlLocation.Visible = true;
			gvLocation.Visible = false;
		}

		private void showLocations() {
			pnlLocation.Visible = false;
			gvLocation.Visible = true;
		}
		#endregion

		#region ================== Invoice Profile methods ==================
		private void bindInvoiceProfiles() {
			gvInvoiceProfile.DataSource = CarrierInvoiceProfileManager.GetAll(carrierID);
			gvInvoiceProfile.DataBind();
		}

		protected void blnkNewInvoiceProfile_Click(object sender, EventArgs e) {
			ViewState["CarrierInvoiceProfileID"] = "0";

			showInvoiceProfilePanel();

			clearProfileFields();

			bindInvoiceType();

			tabContainerFee.Visible = false;

		}

		private void clearProfileFields() {
			txtProgramName.Text = string.Empty;
			effetiveDate.Text = string.Empty;
			expirationDate.Text = string.Empty;
		}

		private void showInvoiceProfilePanel() {
			pnlInvoiceProfile.Visible = true;
			gvInvoiceProfile.Visible = false;
			blnkNewInvoiceProfile.Visible = false;

			//CollectionManager.Fillchk(cbxCoverageArea, "StateId", "StateName", State.GetAll());

			//cbxCoverageArea.Items.Insert(0, new ListItem("All", "all"));
		}

		private void showInvoiceProfiles() {
			gvInvoiceProfile.Visible = true;

			pnlInvoiceProfile.Visible = false;

			blnkNewInvoiceProfile.Visible = true;
		}



		protected void btnProfileSave_Click(object sender, EventArgs e) {
			int profileID = 0;
			CarrierInvoiceProfile profile = null;

			Page.Validate("Profile");
			if (!Page.IsValid)
				return;

			int.TryParse(ViewState["CarrierInvoiceProfileID"].ToString(), out profileID);

			try {
				if (profileID == 0) {
					// new profile
					profile = new CarrierInvoiceProfile();

					profile.CarrierID = carrierID;

					profile.IsActive = true;
				}
				else
					profile = CarrierInvoiceProfileManager.Get(profileID);
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
			}

			if (profile != null) {


				profile.CarrierID = carrierID;

				profile.ProfileName = txtProgramName.Text;

				if (ucProfileType.SelectedIndex > 0)
					profile.CarrierInvoiceProfileTypeID = Convert.ToInt32(ucProfileType.SelectedValue);

				if (!string.IsNullOrEmpty(effetiveDate.Text))
					profile.EffiectiveDate = effetiveDate.Date;
				else
					profile.EffiectiveDate = null;

				if (!string.IsNullOrEmpty(expirationDate.Text))
					profile.ExpirationDate = expirationDate.Date;
				else
					profile.ExpirationDate = null;

				//profile.CoverageArea = CollectionManager.GetSelectedItemsID(cbxCoverageArea);

				profile.InvoiceType = Convert.ToInt32(ddlInvoiceType.SelectedValue);

				profile.AccountingContact = txtAccoutingContact.Text;

				profile.AccountingContactEmail = txtAccoutingContactEmail.Text;

				profile.FirmDiscountPercentage = txtFirmDiscountPercentage.ValueDecimal;

               // profile.FlatCatPercent = txtFlatCatPercent.ValueDecimal;//NEW OC

               // profile.FlatCatFee = txtFlatCatFee.ValueDecimal; // NEW OC
                
				try {
					CarrierInvoiceProfileManager.Save(profile);

					showInvoiceProfiles();

					bindInvoiceProfiles();
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}
		}

		protected void btnProfileCancel_Click(object sender, EventArgs e) {
			showInvoiceProfiles();

			bindInvoiceProfiles();
		}

		protected void gvInvoiceProfile_RowCommand(object sender, GridViewCommandEventArgs e) {
			int profileID = Convert.ToInt32(e.CommandArgument);
			CarrierInvoiceProfile profile = null;

			if (e.CommandName == "DoEdit") {
				ViewState["CarrierInvoiceProfileID"] = profileID.ToString();

				profile = CarrierInvoiceProfileManager.Get(profileID);

				if (profile != null) {
					// enable edit panel
					showInvoiceProfilePanel();

					bindInvoiceType();

					txtProgramName.Text = profile.ProfileName;

					effetiveDate.Text = profile.EffiectiveDate == null ? "" : profile.EffiectiveDate.ToString();

					expirationDate.Text = profile.ExpirationDate == null ? "" : profile.ExpirationDate.ToString();

					ddlInvoiceType.SelectedValue = string.Format("{0}", profile.InvoiceType ?? 1);

					// 2014-04-21
					//CollectionManager.SetSelectedItems(cbxCoverageArea, profile.CoverageArea);
					
					txtAccoutingContact.Text = profile.AccountingContact;

					txtAccoutingContactEmail.Text = profile.AccountingContactEmail;

					txtFirmDiscountPercentage.Value = profile.FirmDiscountPercentage;

                    //txtFlatCatPercent.Value = profile.FlatCatPercent;//new oc
                   // txtFlatCatFee.Value = profile.FlatCatFee;//new oc
					tabContainerFee.Visible = true;

					if (profile.CarrierInvoiceProfileTypeID != null)
						ucProfileType.SelectedValue = profile.CarrierInvoiceProfileTypeID.ToString();

					ucFeeSchedule.bindFeeSchedule(profileID);

					// 2014-04-21
					//ucFeeProvision.bindProvisions(profileID);

					ucFeeItemized.bindItems(profileID);
				}
			}

			if (e.CommandName == "DoDelete") {
				try {
					profile = CarrierInvoiceProfileManager.Get(profileID);
					if (profile != null) {
						profile.IsActive = false;

						CarrierInvoiceProfileManager.Save(profile);

						showInvoiceProfiles();

						bindInvoiceProfiles();
					}
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}
            if (e.CommandName == "DoCopy")
            {
                CopyProfile(profileID);
            }
		}

		protected void gvInvoiceProfile_RowDataBound(object sender, GridViewRowEventArgs e) {
			CarrierInvoiceProfile invoiceProfile = null;
			int carrierInvoiceProfileTypeID = 0;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				Label lblProfileTypeDescription = e.Row.FindControl("lblProfileTypeDescription") as Label;

				if (e.Row.DataItem != null) {
					invoiceProfile = e.Row.DataItem as CarrierInvoiceProfile;

					if (invoiceProfile.CarrierInvoiceProfileTypeID != null) {
						carrierInvoiceProfileTypeID = invoiceProfile.CarrierInvoiceProfileTypeID ?? 0;
						try {
							lblProfileTypeDescription.Text = carrierInvoiceProfileTypes[carrierInvoiceProfileTypeID];
						}
						catch (Exception ex) {
							Core.EmailHelper.emailError(ex);
						}
					}

				}
			}
		}

        //chetu com
        public void CopyProfile(int profileID)
        {
            //int profileID = 0;
            CarrierInvoiceProfile profile = null;
            CarrierInvoiceProfile profileold = null;
            profileold = CarrierInvoiceProfileManager.Get(profileID);
              profile = new CarrierInvoiceProfile();

              profile.CarrierID = carrierID;

              profile.IsActive = true;  
            if (profile != null)
            {
                profile.ProfileName = profileold.ProfileName;

                
               profile.CarrierInvoiceProfileTypeID = profileold.CarrierInvoiceProfileTypeID;
               profile.EffiectiveDate = profileold.EffiectiveDate;
               profile.ExpirationDate = profileold.ExpirationDate;
               profile.InvoiceType = profileold.InvoiceType;
               profile.AccountingContact = profileold.AccountingContact;
               profile.AccountingContactEmail = profileold.AccountingContactEmail;
               profile.FirmDiscountPercentage = profileold.FirmDiscountPercentage;
               profile.FlatCatFee = profileold.FlatCatFee;
               profile.FlatCatPercent = profileold.FlatCatPercent;
                try
                {
                    CarrierInvoiceProfileManager.Save(profile);

                    showInvoiceProfiles();

                    bindInvoiceProfiles();
                }
                catch (Exception ex)
                {
                    Core.EmailHelper.emailError(ex);
                }
            }
        }
        //
		#endregion
				
	}
}