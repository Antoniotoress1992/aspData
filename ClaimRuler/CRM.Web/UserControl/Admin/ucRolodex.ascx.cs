using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;

using AjaxControlToolkit;
using CRM.Data.Entities;
using Microsoft.Exchange.WebServices.Data;

namespace CRM.Web.UserControl.Admin {
	public partial class ucRolodex : System.Web.UI.UserControl {
		int clientID = 0;
		protected Protected.ClaimRuler masterPage = null;

		protected void Page_Load(object sender, EventArgs e) {
			string search = null;

			search = Request.Params["q"];

			clientID = Core.SessionHelper.getClientId();

			masterPage = this.Page.Master as Protected.ClaimRuler;

			if (!Page.IsPostBack)
				loadData(search);

		}

		protected void btnCancel_Click(object sender, EventArgs e) {
			

			pnlContact.Visible = true;
			
			pnlContactDetail.Visible = false;

			loadData(null);

		}

		private void bindContactTypes() {
			List<LeadContactType> leadContactTypes = LeadContactTypeManager.GetAll(clientID);

			CollectionManager.FillCollection(ddlContactType, "ID", "Description", leadContactTypes);
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			decimal balance = 0;
            CRM.Data.Entities.Contact contact = null;
			int clientID = Core.SessionHelper.getClientId();
			int contactID = 0;

			Page.Validate("contact");

			if (!Page.IsValid)
				return;

			contactID = Convert.ToInt32(hdId.Value);

			if (contactID == 0) {
                contact = new CRM.Data.Entities.Contact();
				contact.ClientID = clientID;

				// TO-DO: remove trigger in database
				contact.IsActive = true;
                try
                {
                    int userID = SessionHelper.getUserId();
                    CRM.Data.Entities.SecUser secUser = SecUserManager.GetByUserId(userID);
                    string email = secUser.Email;
                    string password = SecurityManager.Decrypt(secUser.emailPassword);
                    string url = "https://" + secUser.emailHost + "/EWS/Exchange.asmx";


                    ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                    service.Credentials = new WebCredentials(email, password);
                    service.Url = new Uri(url);

                    Microsoft.Exchange.WebServices.Data.Contact outlookContact = new Microsoft.Exchange.WebServices.Data.Contact(service);
                    outlookContact.GivenName = txtFirstName.Text;
                    outlookContact.Surname = txtLastName.Text;
                    outlookContact.FileAsMapping = FileAsMapping.SurnameCommaGivenName;
                    outlookContact.EmailAddresses[EmailAddressKey.EmailAddress1] = txtEmail.Text;
                    outlookContact.PhoneNumbers[PhoneNumberKey.HomePhone] = txtPhone.Text;

                    outlookContact.Save();
                }
                catch (Exception ex)
                {

                }
			}
			else
				contact = ContactManager.Get(contactID);


			if (contact != null) {
				contact.CompanyName = txtCompanyName.Text;
				contact.DepartmentName = txtDepartmentName.Text;
				contact.ContactTitle = txtContactTile.Text;

				contact.FirstName = txtFirstName.Text;
				contact.LastName = txtLastName.Text;
				contact.ContactName = txtFirstName.Text + " " + txtLastName.Text;

				contact.Address1 = txtAddress1.Text;
				contact.Address2 = txtAddress2.Text;

				contact.Mobile = txtMobile.Text;
				contact.Phone = txtPhone.Text;
				contact.Email = txtEmail.Text;
				contact.Fax = txtContactFax.Text;

				contact.County = txtCounty.Text;

				//contact.ClaimName = txtClaimName.Text;

				if (ddlState.SelectedIndex > 0)
					contact.StateID = Convert.ToInt32(ddlState.SelectedValue);

				if (ddlCity.SelectedIndex > 0)
					contact.CityID = Convert.ToInt32(ddlCity.SelectedValue);

				if (ddlContactType.SelectedIndex > 0)
					contact.CategoryID = Convert.ToInt32(ddlContactType.SelectedValue);

				if (ddlLossZip.SelectedIndex > 0)
					contact.ZipCodeID = Convert.ToInt32(ddlLossZip.SelectedValue);

				decimal.TryParse(txtBalance.Text, out balance);
				contact.Balance = balance;

				try {
					ContactManager.Save(contact);

					pnlContact.Visible = true;
					pnlContactDetail.Visible = false;

					loadData(null);
				}
				catch (Exception ex) {
					EmailHelper.emailError(ex);
				}

			}
		}

		protected void btnNewContact_Click(object sender, EventArgs e) {
			pnlContact.Visible = false;
			pnlContactDetail.Visible = true;

			clearFields();

			bindStates();

			bindContactTypes();
		}

		protected void clearFields() {
			txtAddress1.Text = "";
			txtAddress2.Text = "";
			txtCompanyName.Text = "";
			txtEmail.Text = "";
			txtFirstName.Text = "";
			txtLastName.Text = "";
			txtMobile.Text = "";
			txtPhone.Text = "";
			txtBalance.Text = "";
			//txtClaimName.Text = "";
			txtCounty.Text = "";
			txtContactFax.Text = "";
			ddlState.SelectedIndex = -1;
			ddlCity.Items.Clear();
			ddlLossZip.Items.Clear();
			ddlContactType.SelectedIndex = -1;
			txtContactTile.Text = string.Empty;
			txtDepartmentName.Text = string.Empty;
		}

		protected void bindStates() {
			List<StateMaster> states = State.GetAll();

			CollectionManager.FillCollection(ddlState, "StateId", "StateName", states);
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
				CollectionManager.FillCollection(ddlLossZip, "ZipCodeID", "ZipCode", ZipCode.getByCityID(Convert.ToInt32(ddlCity.SelectedValue)));
			}
			else {
				ddlLossZip.Items.Clear();
			}

		}



		

	
		protected void gvContact_RowCommand(object sender, GridViewCommandEventArgs e) {
			int contactID = 0;
            CRM.Data.Entities.Contact contact = null;
			int stateID = 0;

			if (e.CommandName == "DoEdit") {
				hdId.Value = e.CommandArgument.ToString();
				contactID = Convert.ToInt32(e.CommandArgument);

				pnlContact.Visible = false;

				pnlContactDetail.Visible = true;

				contact = ContactManager.Get(contactID);
				if (contact != null) {
					txtAddress1.Text = contact.Address1;
					txtAddress2.Text = contact.Address2;
					txtCompanyName.Text = contact.CompanyName;
					txtDepartmentName.Text = contact.DepartmentName;
					txtContactTile.Text = contact.ContactTitle;
				
					txtEmail.Text = contact.Email;
					txtFirstName.Text = contact.FirstName;
					txtLastName.Text = contact.LastName;
					
					txtMobile.Text = contact.Mobile;
					txtPhone.Text = contact.Phone;
					txtBalance.Text = contact.Balance.ToString();
					//txtClaimName.Text = contact.ClaimName;
					txtCounty.Text = contact.County;
					txtContactFax.Text = contact.Fax;
					
					bindContactTypes();

					bindStates();
					stateID = contact.StateID ?? 0;

					if (stateID > 0) {
						ddlState.SelectedValue = stateID.ToString();

						CollectionManager.FillCollection(ddlCity, "CityId", "CityName", City.GetAll(stateID));
					}

					if (contact.CityID != null && contact.CityID > 0) {
						try {
							ddlCity.SelectedValue = contact.CityID.ToString();
						}
						catch {
						}

						CollectionManager.FillCollection(ddlLossZip, "ZipCodeID", "ZipCode", ZipCode.getByCityID((int)contact.CityID));
					}
					if (contact.ZipCodeID != null && contact.ZipCodeID > 0) {
						try {
							ddlLossZip.SelectedValue = contact.ZipCodeID.ToString();
						}
						catch {
						}
					}

					if (contact.CategoryID != null) {
						try {
							ddlContactType.SelectedValue = contact.CategoryID.ToString();
						}
						catch (Exception ex) {
							Core.EmailHelper.emailError(ex);
						}
					}
				}

			}
			else if (e.CommandName == "DoDelete") {
				contactID = Convert.ToInt32(e.CommandArgument);
				try {
					contact = ContactManager.Get(contactID);

					contact.IsActive = false;

					contact = ContactManager.Save(contact);

					loadData(null);
				}
				catch (Exception ex) {
					EmailHelper.emailError(ex);
				}
			}

		}

		protected void loadData(string keyword) {
			int clientID = Core.SessionHelper.getClientId();

            IQueryable<CRM.Data.Entities.Contact> contacts = null;

			if (keyword == null) {
				contacts = ContactManager.GetAll(clientID);
			}
			else {
				contacts = ContactManager.Search(keyword, clientID);
			}

            List<CRM.Data.Entities.Contact> contactList = contacts.ToList();

            int userID = SessionHelper.getUserId();

            CRM.Data.Entities.SecUser secUser = SecUserManager.GetByUserId(userID);
            string email = secUser.Email;
            string password = SecurityManager.Decrypt(secUser.emailPassword);
            string url = "https://" + secUser.emailHost + "/EWS/Exchange.asmx";

            try
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                service.Credentials = new WebCredentials(email, password);
                service.Url = new Uri(url);

                ContactsFolder contactsfolder = ContactsFolder.Bind(service, WellKnownFolderName.Contacts);

                int numItems = contactsfolder.TotalCount < 50 ? contactsfolder.TotalCount : 50;

                ItemView view = new ItemView(int.MaxValue);

                view.PropertySet = new PropertySet(BasePropertySet.IdOnly, ContactSchema.DisplayName);

                FindItemsResults<Item> contactItems = service.FindItems(WellKnownFolderName.Contacts, view);


                foreach (Item item in contactItems)
                {
                    if (item is Microsoft.Exchange.WebServices.Data.Contact)
                    {
                        item.Load();
                        Microsoft.Exchange.WebServices.Data.Contact contact = item as Microsoft.Exchange.WebServices.Data.Contact;
                        CRM.Data.Entities.Contact newContact = new Data.Entities.Contact();
                        newContact.FirstName = contact.GivenName;
                        newContact.LastName = contact.Surname;
                        newContact.Email = contact.EmailAddresses[0].Address;
                        newContact.DepartmentName = "Outlook";
                        bool exist = false;
                        if (keyword == null || keyword[0] == newContact.Email[0])
                            foreach (var con in contactList)
                            {
                                if (con.Email == newContact.Email)
                                    exist = true;

                            }
                        if (!exist)
                            contactList.Add(newContact);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            gvContact.DataSource = contactList;
			gvContact.DataBind();
		}

		protected void gvContact_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			gvContact.PageIndex = e.NewPageIndex;

			loadData(null);
		}

		protected void gvContact_Sorting(object sender, GridViewSortEventArgs e) {
			int clientID = Core.SessionHelper.getClientId();

            IQueryable<CRM.Data.Entities.Contact> contacts = null;

			contacts = ContactManager.GetAll(clientID);
			

			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;
		
			//gvContact.DataSource = contacts.orderByExtension(e.SortExpression, descending);
            System.Web.UI.WebControls.SortDirection sortDirection = descending ? System.Web.UI.WebControls.SortDirection.Descending : System.Web.UI.WebControls.SortDirection.Ascending;

			gvContact.DataSource = contacts.orderByNested(e.SortExpression, sortDirection);

			gvContact.DataBind();
		}
	}
}