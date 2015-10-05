using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;
using CRM.Data.Entities;
using Microsoft.Exchange.WebServices.Data;
using Infragistics.Web.UI.GridControls;
using System.Data;

namespace CRM.Web.Protected {
	public partial class LeadEmail : System.Web.UI.Page {
		int clientID = 0;
		int claimID = 0;
		int leadID = 0;
		int userID = 0;

		string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

		protected void Page_Load(object sender, EventArgs e) {
			// access master page from nested master page
			Protected.ClaimRuler masterPage = Master.Master as Protected.ClaimRuler;

			// check user permission
			//masterPage.checkPermission();

			clientID = SessionHelper.getClientId();

			// get current lead
			leadID = SessionHelper.getLeadId();

			// get current lead id
			claimID = SessionHelper.getClaimID();

			// get current user
			userID = SessionHelper.getUserId();

			// set directory where client can upload pictures for signature
			txtSignature.UploadedFilesDirectory = appPath + "/clientLogo/" + clientID;

			if (!Page.IsPostBack) {
				bindData();
                userID = SessionHelper.getUserId();

                CRM.Data.Entities.SecUser secUser = SecUserManager.GetByUserId(userID);
                string email = secUser.Email;
                string password =SecurityManager.Decrypt(secUser.emailPassword);
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

                    DataTable table = new DataTable();
                    table.Columns.Add("FirstName", typeof(string));
                    table.Columns.Add("LastName", typeof(string));
                    table.Columns.Add("CompanyName", typeof(string));
                    table.Columns.Add("Email", typeof(string));
                    table.Columns.Add("ContactType", typeof(string));

                    foreach (GridRecord crow in contractGrid.Rows)
                    {
                        DataRow row = table.NewRow();
                        row[0] = crow.Items[0].Text;
                        row[1] = crow.Items[1].Text;
                        //row[2] = crow.Cells[2].Text;
                        row[3] = crow.Items[3].Text;
                        row[4] = crow.Items[4].Text;
                        table.Rows.Add(row);
			        }
                    foreach (Item item in contactItems)
                    {
                        if (item is Microsoft.Exchange.WebServices.Data.Contact)
                        {
                            item.Load();
                            Microsoft.Exchange.WebServices.Data.Contact contact = item as Microsoft.Exchange.WebServices.Data.Contact;

                           

                            DataRow row = table.NewRow();
                            row[0] = contact.GivenName;
                            row[1] =contact.Surname;
                            row[3] =contact.EmailAddresses[0].Address;
                            row[4] ="Outlook";
                            table.Rows.Add(row); 
		                }
                    }
                    contractGrid.DataSourceID = null;
                    contractGrid.Columns.Clear();
                    contractGrid.DataBind();
                    contractGrid.DataSource = table;
                    contractGrid.DataBind();
                }
                catch (Exception ex)
                {

                }
            }
        }

		protected void btnSend_Click(object sender, EventArgs e) {
			string[] attachments = null;
			ClaimComment comment = null;
			string emailTo = txtEmailTo.Text.Trim().Replace(",", ";");
			string emailBody = null;
			string[] cc = null;
			string cc_list = null;
			string decryptedPassword = null;
			ArrayList documentArray = null;
			bool isSSL = true;
			int leadID = 0;
			int port = 0;
			string[] recipients = emailTo.Split(';');
			string subject = null;
            CRM.Data.Entities.SecUser user = null;


			Page.Validate("email");
			if (!Page.IsValid)
				return;


			// get user information
			user = SecUserManager.GetByUserId(SessionHelper.getUserId());

			if (user == null) {
				clearFields();
				return;
			}

			// decrypt user password
			decryptedPassword = Core.SecurityManager.Decrypt(user.emailPassword);

			// email CC
			if (!string.IsNullOrEmpty(txtEmailCC.Text)) {
				cc_list = txtEmailCC.Text.Replace(",", ";");

				cc = cc_list.Split(';');
			}



			// get attachments
			if (lbxDocuments != null && lbxDocuments.Items != null && lbxDocuments.Items.Count > 0) {
				documentArray = new ArrayList();

				foreach (ListItem item in lbxDocuments.Items) {
					if (item.Selected) {						
						string documentPath = appPath + "/" + item.Value;
												
						documentArray.Add(documentPath);
					}
				}
				attachments = documentArray.ToArray(typeof(string)) as string[];
			}


			// add to comments

			int.TryParse(user.emailHostPort, out port);

			emailBody = string.Format("<div>{0}</div><div>{1}</div>", txtEmailText.Text.Trim(), txtSignature.Text.Trim());

			subject = txtEmailSubject.Text.Trim();

            //isSSL = user.isSSL ?? true;
            //string smtpHost = ConfigurationManager.AppSettings["smtpHost"].ToString();
            //int smtpPort = ConfigurationManager.AppSettings["smtpPort"] == null ? 25 : Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
            //string smtpEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
            //string smtpPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
           
			try {
               // Core.EmailHelper.sendEmail(txtEmailFrom.Text, recipients, cc, subject, emailBody, attachments, smtpHost, smtpPort, smtpEmail, smtpPassword, isSSL);
                userID = SessionHelper.getUserId();

                CRM.Data.Entities.SecUser secUser = SecUserManager.GetByUserId(userID);
                string emailaddress = secUser.Email;
                string password = SecurityManager.Decrypt(secUser.emailPassword);
                string url = "https://" + secUser.emailHost + "/EWS/Exchange.asmx";

                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                service.Credentials = new WebCredentials(emailaddress, password);
                service.Url = new Uri(url);

                EmailMessage email = new EmailMessage(service);

                foreach (var recipient in recipients)
                {
                    if (!string.IsNullOrEmpty(recipient))
                        email.ToRecipients.Add(recipient);
                }
                email.Subject = subject;
                email.Body = new MessageBody(emailBody);

                foreach (var filename in attachments)
                {
                    if (!string.IsNullOrEmpty(filename))
                        email.Attachments.AddFileAttachment(filename);
                }

                email.SendAndSaveCopy();
				comment = new ClaimComment();
				comment.ClaimID = this.claimID;
				comment.CommentDate = DateTime.Now;
				comment.CommentText = string.Format("Sent Email:<br/>To: {0}<br/>Subject: {1}<br/>{2}",
									txtEmailTo.Text.Trim(), txtEmailSubject.Text.Trim(), txtEmailText.Text.Trim());
				comment.IsActive = true;
				comment.UserId = this.userID;

				ClaimCommentManager.Save(comment);

				clearFields();

				lblMessage.Text = "Email sent successfully.";
				lblMessage.CssClass = "ok";
			}
			catch (Exception ex) {
                lblMessage.Text = "Incorrect Email Settings. Email not sent.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}


		}

		protected void clearFields() {
			// clear email fields				
			txtEmailSubject.Text = "";
			txtEmailText.Text = "";
			txtEmailTo.Text = "";
		}

		protected void bindData() {
			//AppraiserMaster appraiser = null;
			Claim claim = null;
			//List<LeadContact> contacts = null;
			List<DocumentList> attachments = null;
			//ContractorMaster contractor = null;
			//List<SecUser> users = null;
			Leads lead = null;
			//Data.UmpireMaster umpire = null;
			// get email for user sending email

			CRM.Data.Entities.SecUser user = SecUserManager.GetByUserId(userID);
			if (user != null) {
				txtEmailFrom.Text = user.Email;
				txtEmailCC.Text = user.Email;
				txtSignature.Text = user.emailSignature ?? "";
			}


			claim = ClaimsManager.Get(this.claimID);

			if (claim != null) {
				attachments = new List<DocumentList>();

				lead = claim.LeadPolicy.Leads;

				// build subject line
				txtEmailSubject.Text = string.Format("{0} Claim #: {1}", lead.policyHolderName, claim.AdjusterClaimNumber);

				// load legacy documents
				List<LeadsDocument> documents = LeadsUploadManager.getLeadsDocumentByLeadID((int)claim.LeadPolicy.LeadId);

				if (documents != null && documents.Count > 0) {

					foreach (LeadsDocument x in documents) {
						DocumentList attachment = new DocumentList();
						attachment.DocumentName = x.Description;
						attachment.DocumentPath = string.Format("LeadsDocument/{0}/{1}/{2}",						
							x.LeadId,
							x.LeadDocumentId,		// document id
							x.DocumentName);		// document file name

						attachments.Add(attachment);
					}					
				}

				// load claim documents
				List<ClaimDocument> claimDocuments = ClaimDocumentManager.GetAll(this.claimID);
				if (claimDocuments != null && claimDocuments.Count > 0) {

					foreach (ClaimDocument x in claimDocuments) {
						DocumentList attachment = new DocumentList();

						attachment.DocumentName = x.Description;
						attachment.DocumentPath = string.Format("ClaimDocuments/{0}/{1}/{2}",							
							x.ClaimID,
							x.ClaimDocumentID,		// document id
							x.DocumentName);		// document file name

						attachments.Add(attachment);
					}
				}

				lbxDocuments.DataSource = attachments;
				lbxDocuments.DataBind();

				//// load legacy contacts
				//contacts = LeadContactManager.GetContactByLeadID(leadID);

				//// add adjusters to contact list
				//if (claim.AdjusterMaster != null)
				//	addToContactList(claim.AdjusterMaster.AdjusterName, claim.AdjusterMaster.email, contacts);


				//// add appraiser to contact list
				//if (lead.AppraiserID != null) {
				//	appraiser = AppraiserManager.Get((int)lead.AppraiserID);

				//	contacts.Add(new LeadContact {
				//		Email = appraiser.Email,
				//		ContactName = appraiser.AppraiserName
				//	});
				//}

				//// add contractor to contact list
				//if (lead.ContractorID != null) {
				//	contractor = ContractorManager.Get((int)lead.ContractorID);
				//	contacts.Add(new LeadContact {
				//		Email = lead.ContractorMaster.Email,
				//		ContactName = lead.ContractorMaster.ContractorName
				//	});
				//}

				//// add umpire to contact list
				//if (lead.UmpireID != null) {
				//	umpire = UmpireManager.Get((int)lead.UmpireID);

				//	contacts.Add(new LeadContact {
				//		Email = lead.UmpireMaster.Email,
				//		ContactName = lead.UmpireMaster.UmpireName
				//	});
				//}


				// add users 2013-12-12
				//users = SecUserManager.GetUsers(clientID);
				//if (users != null && users.Count > 0) {

				//	users.ForEach(x =>
				//		contacts.Add(new LeadContact {
				//			Email = x.Email,
				//			ContactName = x.FirstName ?? "" + " " + x.LastName ?? ""
				//		}));
				//}
			}

			//lbxContacts.DataSource = contacts;
			//lbxContacts.DataBind();

		}

		


		private void addToContactList(string contactName, string email, List<LeadContact> contacts) {
			if (!string.IsNullOrEmpty(contactName) && !string.IsNullOrEmpty(email)) {
				contacts.Add(new Data.Entities.LeadContact {
					Email = email,
					ContactName = contactName
				});
			}			
		}



		[System.Web.Services.WebMethod]
		[System.Web.Script.Services.ScriptMethod]
		static public string[] getContactEmail(string prefixText, int count) {
			return LeadContactManager.GetEmails(prefixText);
		}
	}
}