using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Web.Utilities;
using CRM.Data.Entities;

namespace CRM.Web.UserControl.Admin {
	public partial class ucCarrierList : System.Web.UI.UserControl {
		int clientID = Core.SessionHelper.getClientId();
		int userID = Core.SessionHelper.getUserId();
		int roleID = Core.SessionHelper.getUserRoleId();

		protected Protected.ClaimRuler masterPage = null;

		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master as Protected.ClaimRuler;

			if (!Page.IsPostBack) {
				DoBind();
			}
				

		}
		
		private void DoBind() {

			List<CarrierView> carriers = null;
			carriers = CarrierManager.GetAll(clientID);

			gvCarriers.DataSource = carriers;
			gvCarriers.DataBind();
		}


		protected void btnNew_Click(object sender, EventArgs e) {
			Session["CarrierID"] = null;

			Response.Redirect("~/Protected/Admin/CarrierEdit.aspx");
		}

		private void copyCarrier(int sourceCarrierID) {
			Carrier carrier = null;
			List<CarrierLocation> carrierLocations = null;
			int copyCarrierID = 0;

			carrier = CarrierManager.Get(sourceCarrierID);
			if (carrier != null) {
				Carrier copyCarrier = new Carrier();

				copyCarrier.AddressLine1 = carrier.AddressLine1;
				copyCarrier.AddressLine2 = carrier.AddressLine2;
				copyCarrier.CarrierName = carrier.CarrierName;
				copyCarrier.CityID = carrier.CityID;
				copyCarrier.CityName = carrier.CityName;
				copyCarrier.ClientID = carrier.ClientID;
				copyCarrier.CountryID = carrier.CountryID;
				copyCarrier.IsActive = true;
				copyCarrier.StateID = carrier.StateID;
				copyCarrier.StateName = carrier.StateName;
				copyCarrier.ZipCodeID = carrier.ZipCodeID;
				copyCarrier.ZipCode = carrier.ZipCode;

				using (TransactionScope scope = new TransactionScope()) {
					try {
						copyCarrier = CarrierManager.Save(copyCarrier);

						if (carrier.CarrierLocation != null) {
							copyCarrierID = copyCarrier.CarrierID;

							carrierLocations = carrier.CarrierLocation.ToList();

							copyCarrierLocations(carrierLocations, copyCarrierID);
						}

						if (carrier.CarrierInvoiceProfile != null)
							copyCarrierInvoiceProfiles(carrier.CarrierInvoiceProfile.ToList(), copyCarrierID);

						copyCarrierContacts(sourceCarrierID, copyCarrierID);

						scope.Complete();
					}
					catch (Exception ex) {
						Core.EmailHelper.emailError(ex);
					}
				}

				// refresh grid
				DoBind();
			}

		}

		private void copyCarrierContacts(int sourceCarrierID, int cloneCarrierID) {
			List<Contact> contacts = CarrierManager.GetContacts(sourceCarrierID);
			if (contacts != null && contacts.Count > 0) {
				foreach (Contact contact in contacts) {
					Contact copyContact = new Contact();
					
					copyContact.CarrierID = cloneCarrierID;

					copyContact.Address1 = contact.Address1;
					copyContact.Address2 = contact.Address2;
					copyContact.Balance = contact.Balance;
					
					copyContact.CategoryID = contact.CategoryID;
					copyContact.CityID = contact.CityID;
					copyContact.ClientID = contact.ClientID;
					copyContact.CompanyName = contact.CompanyName;
					copyContact.ContactTitle = contact.ContactTitle;
					copyContact.County = contact.County;
					copyContact.DepartmentName = contact.DepartmentName;
					copyContact.Email = contact.Email;
					copyContact.FirstName = contact.FirstName;
					copyContact.LastName = contact.LastName;
					copyContact.Mobile = contact.Mobile;
					copyContact.Phone = contact.Phone;
					copyContact.StateID = contact.StateID;
					copyContact.ZipCodeID = contact.ZipCodeID;

					ContactManager.Save(copyContact);
				}
			}
		}

		private void copyCarrierInvoiceProfiles(List<CarrierInvoiceProfile> profiles, int cloneCarrierID) {
			int carrierInvoiceProfileID = 0;

			foreach (CarrierInvoiceProfile profile in profiles) {

				CarrierInvoiceProfile copyProfile = new CarrierInvoiceProfile();
				
				copyProfile.CarrierID = cloneCarrierID;

				//copyProfile.CarrierInvoiceProfileID = profile.CarrierInvoiceProfileID;
				copyProfile.CarrierInvoiceProfileTypeID = profile.CarrierInvoiceProfileTypeID;
				copyProfile.CoverageArea = profile.CoverageArea;
				copyProfile.EffiectiveDate = profile.EffiectiveDate;
				copyProfile.ExpirationDate = profile.ExpirationDate;
				copyProfile.IsActive = true;
				copyProfile.ProfileName = profile.ProfileName;

				copyProfile = CarrierInvoiceProfileManager.Save(copyProfile);

				carrierInvoiceProfileID = copyProfile.CarrierInvoiceProfileID;

				copyProfileItemizedItems(profile.CarrierInvoiceProfileFeeItemized.ToList(), carrierInvoiceProfileID);

				copyProfileFeeProvision(profile.CarrierInvoiceProfileFeeProvision.ToList(), carrierInvoiceProfileID);

				copyProfileFeeSchedule(profile.CarrierInvoiceProfileFeeSchedule.ToList(), carrierInvoiceProfileID);
			}
		}

		private void copyProfileFeeSchedule(List<CarrierInvoiceProfileFeeSchedule> feeSchedules, int carrierInvoiceProfileID) {
			if (feeSchedules != null && feeSchedules.Count > 0) {
				foreach(CarrierInvoiceProfileFeeSchedule feeSchedule in feeSchedules) {
					CarrierInvoiceProfileFeeSchedule copyFeeSchedule = new CarrierInvoiceProfileFeeSchedule();

					copyFeeSchedule.CarrierInvoiceProfileID = carrierInvoiceProfileID;
					copyFeeSchedule.FlatFee = feeSchedule.FlatFee;
					copyFeeSchedule.MinimumFee = feeSchedule.MinimumFee;
					copyFeeSchedule.PercentFee = feeSchedule.PercentFee;
					copyFeeSchedule.RangeAmountFrom = feeSchedule.RangeAmountFrom;
					copyFeeSchedule.RangeAmountTo = feeSchedule.RangeAmountTo;

					CarrierInvoiceProfileFeeScheduleManager.Save(copyFeeSchedule);
				}
			}
		}

		private void copyProfileFeeProvision(List<CarrierInvoiceProfileFeeProvision> feeProvisions, int carrierInvoiceProfileID) {
			if (feeProvisions != null && feeProvisions.Count > 0) {
				foreach (CarrierInvoiceProfileFeeProvision feeProvision in feeProvisions) {
					CarrierInvoiceProfileFeeProvision copyFeeProvision = new CarrierInvoiceProfileFeeProvision();

					copyFeeProvision.CarrierInvoiceProfileID = carrierInvoiceProfileID;
					copyFeeProvision.ProvisionText = feeProvision.ProvisionText;
					copyFeeProvision.ProvisionAmount = feeProvision.ProvisionAmount;

					CarrierInvoiceProfileFeeProvisionManager.Save(copyFeeProvision);
				}
			}
		}

		private void copyProfileItemizedItems(List<CarrierInvoiceProfileFeeItemized> itemizedItems, int carrierInvoiceProfileID) {			
			if (itemizedItems != null && itemizedItems.Count > 0) {
				foreach (CarrierInvoiceProfileFeeItemized sourceItem in itemizedItems) {
					CarrierInvoiceProfileFeeItemized copyItem = new CarrierInvoiceProfileFeeItemized();
					
					copyItem.CarrierInvoiceProfileID = carrierInvoiceProfileID;
					copyItem.IsActive = true;
					copyItem.ItemDescription = sourceItem.ItemDescription;
					copyItem.ItemPercentage = sourceItem.ItemPercentage;
					copyItem.ItemRate = sourceItem.ItemRate;

					CarrierInvoiceProfileFeeItemizedManager.Save(copyItem);
				}
			}
		}

		private void copyCarrierLocations(List<CarrierLocation> carrierLocations, int cloneCarrierID) {
			foreach (CarrierLocation location in carrierLocations) {
				CarrierLocation copyLocation = new CarrierLocation();

				copyLocation.CarrierID = cloneCarrierID;

				copyLocation.AddressLine1 = location.AddressLine1;
				copyLocation.AddressLine2 = location.AddressLine2;
				
				copyLocation.CityId = location.CityId;
				copyLocation.CityName = location.CityName;
				
				copyLocation.CountryID = location.CountryID;
				copyLocation.DepartmentName = location.DepartmentName;
				copyLocation.IsActive = true;
				copyLocation.LocationName = location.LocationName;

				copyLocation.StateId = location.StateId;
				copyLocation.StateName = location.StateName;

				copyLocation.ZipCode = location.ZipCode;
				copyLocation.ZipCodeId = location.ZipCodeId;

				CarrierLocationManager.Save(copyLocation);
			}
		}

		
		protected void gvCarriers_Sorting(object sender, GridViewSortEventArgs e) {
			List<CarrierView> carriers = CarrierManager.GetAll(clientID);
			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			string sortClause = e.SortExpression + (descending ? " Desc " : " Asc ");

			gvCarriers.DataSource = carriers.sort(sortClause).ToList();

			gvCarriers.DataBind();
		}

		protected void gvCarriers_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			gvCarriers.PageIndex = e.NewPageIndex;

			DoBind();
		}

		protected void gvCarriers_RowCommand(object sender, GridViewCommandEventArgs e) {
			int carrierID = 0;
			Carrier carrier = null;

			if (e.CommandName == "DoEdit") {
				Session["CarrierID"] = e.CommandArgument.ToString();

				Response.Redirect("~/Protected/Admin/CarrierEdit.aspx");
			}
			else if (e.CommandName == "DoDelete") {
				carrierID = Convert.ToInt32(e.CommandArgument);

				carrier = CarrierManager.Get(carrierID);
				if (carrier != null) {
					carrier.IsActive = false;

					try {
						CarrierManager.Save(carrier);

						// reload carriers
						DoBind();
					}
					catch (Exception ex) {
						Core.EmailHelper.emailError(ex);
					}
				}
			}
			else if (e.CommandName == "DoCopy")
                carrierID = Convert.ToInt32(e.CommandArgument);
				copyCarrier(carrierID);
		}

		
	}
}