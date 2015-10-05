using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using System.Text;
using System.Data.Objects;


using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;
using CRM.Data.Entities;


using System.Web.UI.HtmlControls;

using LinqKit;
using System.Linq.Expressions;


using CRM.Web.Utilities;


namespace CRM.Web.UserControl {
	public partial class ucClaimService : System.Web.UI.UserControl {
		protected CRM.Web.Protected.ClaimRuler masterPage = null;
		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master.Master as Protected.ClaimRuler;

			string js = string.Format("javascript:processInvoiceServiceType(this, '{0}', '{1}');", txtServiceDescription.ClientID, txtServiceQty.ClientID);

			//ddlInvoiceServiceType.Attributes.Add("onchange", js);
		}


		public void activateEditPanel() {
			lbtnNewClaimService_Click(null, null);
            var today = System.DateTime.Now;
            txtServiceDate.Value = today;
            ddlActivity.SelectedIndex = 0;
		}

		public void bindData(int claimID) {
			masterPage = this.Page.Master.Master as Protected.ClaimRuler;
			List<ClaimService> services = null;

			services = loadClaimServices(claimID);

            
			gvClaimService.DataSource = services;
			gvClaimService.DataBind();

            bindActivity();
			bindServiceTypes();

            //if (ddlInvoiceServiceType.Items.Count < 2) //if only the --select-- is available show pop up
            //{
            //    popup.Show();
            //}
		}

		 private void bindServiceTypes() 
        {
            if (Session["ComingFromAllClaims"] != null)
            {
                pnlEditService.Visible = true;
                var proID = Session["CarrierInvoiceID"];
                int clientID = SessionHelper.getClientId();
                List<InvoiceServiceType> invoiceServiceTypes = null;
                //set todays date to activity date
                var today = System.DateTime.Now;
                txtServiceDate.Value = today;

                //using (Repository.InvoiceServiceTypeManager repository = new Repository.InvoiceServiceTypeManager()) {
                //    invoiceServiceTypes = repository.GetAll(clientID).ToList();
                //}
                using (Repository.InvoiceServiceTypeManager repository = new Repository.InvoiceServiceTypeManager())
                {
                    invoiceServiceTypes = repository.GetAll(Convert.ToInt32(proID)).ToList();
                }

                Core.CollectionManager.FillCollection(ddlInvoiceServiceType, "ServiceTypeID", "ServiceDescription", invoiceServiceTypes);
                Session["ServiceCount"] = ddlInvoiceServiceType.Items.Count;
            }
            else
            {
                var proID = Session["InvoiceProfileID"]; // <~~ this will be used to pass the selected invoice profile ID 
                //(as a parameter) over into the new GetAll() function that we'll be creating today.!
                int clientID = SessionHelper.getClientId();
                List<InvoiceServiceType> invoiceServiceTypes = null;

                //using (Repository.InvoiceServiceTypeManager repository = new Repository.InvoiceServiceTypeManager()) {
                //    invoiceServiceTypes = repository.GetAll(clientID).ToList();
                //}
                using (Repository.InvoiceServiceTypeManager repository = new Repository.InvoiceServiceTypeManager())
                {
                    invoiceServiceTypes = repository.GetAll(Convert.ToInt32(proID)).ToList();
                    
                }

                Core.CollectionManager.FillCollection(ddlInvoiceServiceType, "ServiceTypeID", "ServiceDescription", invoiceServiceTypes);
                Session["ServiceCount"] = ddlInvoiceServiceType.Items.Count;
            }
		}

        //NEW OC 9/24/2014-- METHOD FOR BINDING NEW ACTIVITY DROPDOWN
        private void bindActivity()
        {
            List<Activity> myActivity = ActivityManager.GetAll();

            CollectionManager.FillCollection(ddlActivity, "ActivityID", "Activity1", myActivity);
            
            //Core.CollectionManager.FillCollection(ddlActivity, "ServiceTypeID", "ServiceDescription", invoiceServiceTypes);
        }

		protected void gvClaimService_RowCommand(object sender, GridViewCommandEventArgs e) {
			int id = 0;
			ClaimService claimService = null;

			if (e.CommandName == "DoEdit") {
				id = Convert.ToInt32(e.CommandArgument);

				using (ClaimServiceManager repository = new ClaimServiceManager()) {
					claimService = repository.Get(id);
                    
					if (claimService != null) {
						bindServiceTypes();

						if (claimService.AdjusterMaster != null) {
							txtServiceAdjuster.Text = claimService.AdjusterMaster.adjusterName;
							hf_serviceAdjusterID.Value = claimService.AdjusterID.ToString();
						}

						txtServiceDate.Value = claimService.ServiceDate;
						txtServiceDescription.Text = claimService.ServiceDescription;
						txtServiceQty.Value = claimService.ServiceQty;
                        txtMyComments.Text = claimService.InternalComments;
                        cbIsBillable.Checked = claimService.IsBillable.Value;
                        
                        try
                        {
                            Activity myActivity = ActivityManager.GetByAcctivity(claimService.Activity);
                            ddlActivity.SelectedValue =  myActivity.ActivityID.ToString();
                            RequiredFieldValidator4.Enabled = false;
                            
                        }
                        catch (Exception ex)
                        {
                            ddlActivity.SelectedIndex = 0;
                            RequiredFieldValidator4.Enabled = true;
                            Core.EmailHelper.emailError(ex);
                        }
						try 
                        {
							ddlInvoiceServiceType.SelectedValue = claimService.ServiceTypeID.ToString();
                           
						}
						catch (Exception ex) {
							ddlInvoiceServiceType.SelectedIndex = -1;
							Core.EmailHelper.emailError(ex);
						}

						ViewState["ClaimServiceID"] = claimService.ClaimServiceID.ToString();

						showServiceEditPanel();
					}
				}
			}
			else if (e.CommandName == "DoDelete") {
				id = Convert.ToInt32(e.CommandArgument);
				lblMessage.Text = string.Empty;

				using (ClaimServiceManager repository = new Repository.ClaimServiceManager()) {
					try {
						claimService = repository.Get(id);
						if (claimService != null) {
							repository.Delete(id);

							bindData(SessionHelper.getClaimID());
						}
					}
					catch (Exception ex) {
						lblMessage.Text = "Unable to delete service.";
						lblMessage.CssClass = "error";

						Core.EmailHelper.emailError(ex);
					}

				}
			}
		}


		protected void gvClaimService_RowDataBound(object sender, GridViewRowEventArgs e) 
        {
            //ALL NEW OC 11/4/14 put in place to check whether or not a service has been billed or not
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hF = (HiddenField)e.Row.FindControl("hfBilled");
                ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                ImageButton btnDel = (ImageButton)e.Row.FindControl("btnDelete");
                if(hF.Value == "True")
                {
                    e.Row.Enabled = false;
                   // e.Row.ForeColor = System.Drawing.Color.Gray;
                    e.Row.BackColor = System.Drawing.Color.Gainsboro;
                    btnEdit.Visible = false;
                    btnDel.Visible = false;
                   // gvClaimService.Columns[0].Visible = false;
                }
            }
		}

		private List<ClaimService> loadClaimServices(int claimID) {
			List<ClaimService> services = null;

			using (Repository.ClaimServiceManager repository = new Repository.ClaimServiceManager()) {
				services = repository.GetAll(claimID);
			}

			return services;
		}


		protected void btnSaveClaimService_Click(object sender, EventArgs e) {
			ClaimService claimService = null;
			ClaimComment diary = null;
            AdjusterMaster adjuster = null;
            Claim myClaim = null;
            CarrierInvoiceProfile CarrierInvoice = null;

			int userID = SessionHelper.getUserId();
			int claimID = SessionHelper.getClaimID();
			int id = 0;
            int myAdjusterID = 0;
            int profileID = 0;

			Page.Validate("service");
			if (!Page.IsValid)
				return;
            
			id = Convert.ToInt32(ViewState["ClaimServiceID"]);

            //Get current claim info to pass through to emails
            ClaimManager cm = new ClaimManager();
            myClaim =  cm.Get(claimID);

         
            //AdjusterManager

			try {
				using (TransactionScope scope = new TransactionScope()) 
                {
					using (ClaimServiceManager repository = new ClaimServiceManager()) 
                    {
						if (id == 0) 
                        {
							claimService = new ClaimService();
							claimService.ClaimID = claimID;
						}
						else 
                        {
							claimService = repository.Get(id);
						}

						claimService.ServiceQty = this.txtServiceQty.Value == null ? 0 : Convert.ToDecimal(txtServiceQty.Value);
						claimService.ServiceDate = txtServiceDate.Date;
						claimService.ServiceDescription = txtServiceDescription.Text.Trim();
						claimService.ServiceTypeID = Convert.ToInt32(this.ddlInvoiceServiceType.SelectedValue);
						claimService.UserID = userID;
						claimService.AdjusterID = Convert.ToInt32(hf_serviceAdjusterID.Value);
                        claimService.Activity = ddlActivity.SelectedItem.Text;
                        claimService.InternalComments = txtMyComments.Text.Trim();
                        claimService.IsBillable = cbIsBillable.Checked;
                        claimService.Billed = false;
                        //save to db
                        claimService = repository.Save(claimService);
                        
                        //string EmailService = ddlInvoiceServiceType.SelectedItem.Text;
                        //string EmailActivity = ddlActivity.SelectedItem.Text;
                        //string EmailDescription = txtServiceDescription.Text;
                        //string EmailInternal = txtMyComments.Text;
                        //string EmailQuantity = txtServiceQty.Text;
                        //string EmailDate = txtServiceDate.Text;
                        
					}

					// diary
					diary = new ClaimComment();
					diary.ClaimID = claimID;
					diary.CommentDate = DateTime.Now;
					diary.UserId = userID;
                    diary.ActivityType = ddlActivity.SelectedItem.Text;
                    diary.InternalComments = txtMyComments.Text.Trim();
					diary.CommentText = string.Format("Service: {0}, Description: {1}, Date {2:MM/dd/yyyy}, Qty: {3:N2}, Adjuster: {4}",
												ddlInvoiceServiceType.SelectedItem.Text,
												claimService.ServiceDescription,
												claimService.ServiceDate,
												claimService.ServiceQty,
												txtServiceAdjuster.Text
												);
					ClaimCommentManager.Save(diary);

                    myAdjusterID = Convert.ToInt32(claimService.AdjusterID); //Convert.ToInt32(myClaim.AdjusterID);
                   

                    //EMAIL ADJUSTER OC 10/21/2014
                    if (myAdjusterID != 0 || myAdjusterID != null)
                    {
                        adjuster = AdjusterManager.GetAdjusterId(myAdjusterID);
                    }
                    if (cbEmailAdjuster.Checked == true)
                    {
                        try
                        {
                            notifyAdjuster(adjuster, claimService, myClaim);
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Unable to send email to adjuster";
                            lblMessage.CssClass = "error";
                        }
                    }
                    //EMAIL CLIENT CONTACT OC 10/22/2014
                    if (cbEmailClient.Checked == true) //Dont need to check if invoice Pro ID is empty becuase to get to this point, one has to exist already
                    {
                        if (Session["ComingFromAllClaims"] != null) //if the user got here straight from the all claims screen
                        {
                            profileID = Convert.ToInt32(Session["CarrierInvoiceID"]);
                        }
                        else//coming from claim detail page
                        {
                            profileID = Convert.ToInt32(Session["InvoiceProfileID"]);
                        }
                        CarrierInvoice = CarrierInvoiceProfileManager.Get(profileID);
                        try
                        {
                            notifyClientContact(CarrierInvoice, claimService, myClaim, adjuster);
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Unable to send email to client contact";
                            lblMessage.CssClass = "error";
                        }
                        
                    }
                    //EMAIL TO WHOMEVER
                    if(txtEmailTo.Text != "")
                    {
                        
                        try
                        {
                            notifySpecifiedUser(adjuster, claimService, myClaim);
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Unable to send email to adjuster";
                            lblMessage.CssClass = "error";
                        }
                    }

					scope.Complete();

				}

				lblMessage.Text = "Service was saved successfully.";
				lblMessage.CssClass = "ok";

				// keep edit form active
				lbtnNewClaimService_Click(null, null);

				// refresh grid
				gvClaimService.DataSource = loadClaimServices(claimID);
				gvClaimService.DataBind();

			}
			catch (Exception ex) 
            {
				Core.EmailHelper.emailError(ex);

				lblMessage.Text = "Unable to save claim service.";
				lblMessage.CssClass = "error";
			}
            //send email to adjuster
           


		}
        //EMAIL ADJUSTER
        private void notifyAdjuster(AdjusterMaster adjuster, ClaimService service, Claim myClaim)
        {
            StringBuilder emailBody = new StringBuilder();
            string password = null;
            string[] recipients = null;
            string smtpHost = null;
            int smtpPort = 0;
            string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
            string insuredName = Session["InsuredName"].ToString();
            string subject = "Claim # " + myClaim.InsurerClaimNumber + ", Please Read Claim Note, Claim Ruler Note for : " + insuredName;
            CRM.Data.Entities.SecUser user = null;

            string itsgHost = ConfigurationManager.AppSettings["smtpHost"].ToString();
            string itsgHostPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
            string itsgEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
            string itsgEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();

            string EmailService = ddlInvoiceServiceType.SelectedItem.Text;
            string EmailActivity = ddlActivity.SelectedItem.Text;
            string EmailDescription = txtServiceDescription.Text;
            string EmailInternal = txtMyComments.Text;
            string EmailQuantity = txtServiceQty.Text;
            string EmailDate = txtServiceDate.Text;
            string userName = SessionHelper.getUserName();

            int.TryParse(itsgHostPort, out smtpPort);
            
            // get logged in user
            int userID = SessionHelper.getUserId();
            //Convert.ToInt32(itsgHostPort);
            // get current user email info
            user = SecUserManager.GetByUserId(userID);
            
            // load email credentials
            smtpHost = user.emailHost;
            int.TryParse(user.emailHostPort, out smtpPort);


            // recipients
            recipients = new string[] { adjuster.email };

            // build email body
            // .containerBox
            emailBody.Append("<div style=\"margin:auto;width:600px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");

            // .header
            //emailBody.Append("<div style=\"background-image:url(https://appv3.claimruler.com/images/email_header_small.jpg);background-repeat: no-repeat;background-size: 100% 100%;height:70px;\"></div>");
            //emailBody.Append("<div><img src=\"http://app.claimruler.com/images/email_header_small.jpg\"></image></div>");

            // .paneContentInner
            emailBody.Append("<div style=\"margin: 20px;\">");
            emailBody.Append("Hi " + adjuster.adjusterName + ",<br><br>");
            emailBody.Append("The following activity was just logged on Claim # " + myClaim.InsurerClaimNumber + " for Insured: " + insuredName + "<br><br><br>");
            //emailBody.Append("<b>Service:</b> &nbsp &nbsp &nbsp &nbsp &nbsp " + EmailService + "<br><br>");
            //emailBody.Append("<b>Activity:</b> &nbsp &nbsp &nbsp &nbsp &nbsp" + EmailActivity + "<br><br>");
            //emailBody.Append("<b>Description:</b> &nbsp &nbsp &nbsp &nbsp &nbsp" + EmailDescription + "<br><br>");
            //emailBody.Append("<b>Internal Comments:</b> &nbsp &nbsp &nbsp &nbsp &nbsp" + EmailInternal + "<br><br><br>");

            //emailBody.Append("<b>Adjuster:</b> &nbsp &nbsp &nbsp &nbsp &nbsp" + adjuster.adjusterName + "<br>");
            ////emailBody.Append("<b>User:</b> &nbsp &nbsp &nbsp &nbsp &nbsp" + adjuster.adjusterName + "<br>"); -- will be user
            //emailBody.Append("<b>Service Date/Time:/<b> &nbsp &nbsp &nbsp &nbsp &nbsp" + EmailDate + "<br>");
            //emailBody.Append("<b>Timer Quantity:</b> &nbsp &nbsp &nbsp &nbsp &nbsp" + EmailQuantity + "<br>");

            emailBody.Append("<table >");
            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Service:</b></td><td align=\"left\"> " + EmailService + "</td>");
            emailBody.Append("</tr>");
           // emailBody.Append("</table><br><br>");

           
            emailBody.Append("<tr>");
            emailBody.Append("<td align=\"left\"><b>Activity:</b></td><td align=\"left\"> " + EmailActivity + "</td>");
            emailBody.Append("</tr>");
            
           
            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Description:</b></td><td align=\"left\"> " + EmailDescription + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Internal Comments:</b></td><td align=\"left\"> " + EmailInternal + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr></tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Adjuster:</b></td><td align=\"left\"> " + adjuster.adjusterName + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>User Name:</b></td><td align=\"left\"> " + userName + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Service Date:</b></td><td align=\"left\"> " + EmailDate + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Timer Quantity:</b></td><td align=\"left\"> " + EmailQuantity + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("</table ><br><br>");

            //emailBody.Append("<table >");
            //emailBody.Append(string.Format("<tr align=\"left\"><th scope=\"col\">{0}</th><th scope=\"col\">{1}</th></tr>", "Activity:", EmailActivity));
            //emailBody.Append("</table>");

            //emailBody.Append("<table style=\"width:550px;\">");
            //emailBody.Append(string.Format("<tr align=\"left\"><th scope=\"col\">{0}</th><th scope=\"col\">{1}</th></tr>", "Description:", EmailDescription));
            //emailBody.Append("</table>");

           // // claim list
           //// emailBody.Append(claimList.ToString());

           // // end email body
           // emailBody.Append("</table>");
           // emailBody.Append(string.Format("<p><a href=\"{0}/login.aspx\">Please click here to access Claim Ruler.</a></p>", siteUrl));
           // emailBody.Append("<br>Thank you.");

            emailBody.Append("</div>");	// inner containerBox
            emailBody.Append("</div>");	// paneContentInner
            emailBody.Append("</div>");	// containerBox

            //Core.EmailHelper.sendEmail(user.Email, recipients, null, "Claim Assignment Notification", emailBody.ToString(), null, user.emailHost, smtpPort, smtpEmail, smtpPassword, true);
            password = Core.SecurityManager.Decrypt(user.emailPassword);

            //Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, user.isSSL ?? true);
            Core.EmailHelper.sendEmail(itsgEmail, recipients, null, subject, emailBody.ToString(), null, itsgHost, Convert.ToInt32(itsgHostPort), itsgEmail, itsgEmailPassword);
        }
        //EMAIL CLIENT CONTACT
        private void notifyClientContact(CarrierInvoiceProfile CarrierInvoice, ClaimService service, Claim myClaim, AdjusterMaster adjuster)
        {
            StringBuilder emailBody = new StringBuilder();
            string password = null;
            string[] recipients = null;
            string smtpHost = null;
            int smtpPort = 0;
            string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
            string insuredName = Session["InsuredName"].ToString();
            string subject = "Claim # " + myClaim.InsurerClaimNumber + ", Please Read Claim Note, Claim Ruler Note for : " + insuredName;
            CRM.Data.Entities.SecUser user = null;

            string itsgHost = ConfigurationManager.AppSettings["smtpHost"].ToString();
            string itsgHostPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
            string itsgEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
            string itsgEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();

            string EmailService = ddlInvoiceServiceType.SelectedItem.Text;
            string EmailActivity = ddlActivity.SelectedItem.Text;
            string EmailDescription = txtServiceDescription.Text;
            string EmailInternal = txtMyComments.Text;
            string EmailQuantity = txtServiceQty.Text;
            string EmailDate = txtServiceDate.Text;
           
            string userName = SessionHelper.getUserName();

            int.TryParse(itsgHostPort, out smtpPort);

            // get logged in user
            int userID = SessionHelper.getUserId();
            //Convert.ToInt32(itsgHostPort);
            // get current user email info
            user = SecUserManager.GetByUserId(userID);

            // load email credentials
            smtpHost = user.emailHost;
            int.TryParse(user.emailHostPort, out smtpPort);


            // recipients
            recipients = new string[] { CarrierInvoice.AccountingContactEmail };
            
            // build email body
            // .containerBox
            emailBody.Append("<div style=\"margin:auto;width:600px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");

            // .header
            //emailBody.Append("<div style=\"background-image:url(https://appv3.claimruler.com/images/email_header_small.jpg);background-repeat: no-repeat;background-size: 100% 100%;height:70px;\"></div>");
            //emailBody.Append("<div><img src=\"http://app.claimruler.com/images/email_header_small.jpg\"></image></div>");

            // .paneContentInner
            emailBody.Append("<div style=\"margin: 20px;\">");
            emailBody.Append("Hi " + CarrierInvoice.AccountingContact + ",<br><br>");
            emailBody.Append("The following activity was just logged on Claim # " + myClaim.InsurerClaimNumber + " for Insured: " + insuredName + "<br><br><br>");
          
            emailBody.Append("<table >");
            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Service:</b></td><td align=\"left\"> " + EmailService + "</td>");
            emailBody.Append("</tr>");
          

            emailBody.Append("<tr>");
            emailBody.Append("<td align=\"left\"><b>Activity:</b></td><td align=\"left\"> " + EmailActivity + "</td>");
            emailBody.Append("</tr>");


            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Description:</b></td><td align=\"left\"> " + EmailDescription + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Internal Comments:</b></td><td align=\"left\"> " + EmailInternal + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr></tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Adjuster:</b></td><td align=\"left\"> " + adjuster.adjusterName + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>User Name:</b></td><td align=\"left\"> " + userName + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Service Date:</b></td><td align=\"left\"> " + EmailDate + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Timer Quantity:</b></td><td align=\"left\"> " + EmailQuantity + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("</table ><br><br>");

            emailBody.Append("</div>");	// inner containerBox
            emailBody.Append("</div>");	// paneContentInner
            emailBody.Append("</div>");	// containerBox

            //Core.EmailHelper.sendEmail(user.Email, recipients, null, "Claim Assignment Notification", emailBody.ToString(), null, user.emailHost, smtpPort, smtpEmail, smtpPassword, true);
            password = Core.SecurityManager.Decrypt(user.emailPassword);

            //Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, user.isSSL ?? true);
            Core.EmailHelper.sendEmail(itsgEmail, recipients, null, subject, emailBody.ToString(), null, itsgHost, Convert.ToInt32(itsgHostPort), itsgEmail, itsgEmailPassword);
        }
        //EMAIL TO: Specified User
        private void notifySpecifiedUser(AdjusterMaster adjuster, ClaimService service, Claim myClaim)
        {
            StringBuilder emailBody = new StringBuilder();
            string password = null;
            string[] recipients = null;
            string smtpHost = null;
            int smtpPort = 0;
            string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
            string insuredName = Session["InsuredName"].ToString();
            string subject = "Claim # " + myClaim.InsurerClaimNumber + ", Please Read Claim Note, Claim Ruler Note for : " + insuredName;
            CRM.Data.Entities.SecUser user = null;

            string itsgHost = ConfigurationManager.AppSettings["smtpHost"].ToString();
            string itsgHostPort = ConfigurationManager.AppSettings["smtpPort"].ToString();
            string itsgEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
            string itsgEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();

            string EmailService = ddlInvoiceServiceType.SelectedItem.Text;
            string EmailActivity = ddlActivity.SelectedItem.Text;
            string EmailDescription = txtServiceDescription.Text;
            string EmailInternal = txtMyComments.Text;
            string EmailQuantity = txtServiceQty.Text;
            string EmailDate = txtServiceDate.Text;
            string userName = SessionHelper.getUserName();

            int.TryParse(itsgHostPort, out smtpPort);

            // get logged in user
            int userID = SessionHelper.getUserId();
            //Convert.ToInt32(itsgHostPort);
            // get current user email info
            user = SecUserManager.GetByUserId(userID);

            // load email credentials
            smtpHost = user.emailHost;
            int.TryParse(user.emailHostPort, out smtpPort);


            // recipients
            recipients = new string[] { txtEmailTo.Text };

            // build email body
            // .containerBox
            emailBody.Append("<div style=\"margin:auto;width:600px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");

            // .header
            //emailBody.Append("<div style=\"background-image:url(https://appv3.claimruler.com/images/email_header_small.jpg);background-repeat: no-repeat;background-size: 100% 100%;height:70px;\"></div>");
            //emailBody.Append("<div><img src=\"http://app.claimruler.com/images/email_header_small.jpg\"></image></div>");

            // .paneContentInner
            emailBody.Append("<div style=\"margin: 20px;\">");
            emailBody.Append("Hi " + adjuster.adjusterName + ",<br><br>");
            emailBody.Append("The following activity was just logged on Claim # " + myClaim.InsurerClaimNumber + " for Insured: " + insuredName + "<br><br><br>");
        

            emailBody.Append("<table >");
            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Service:</b></td><td align=\"left\"> " + EmailService + "</td>");
            emailBody.Append("</tr>");
            // emailBody.Append("</table><br><br>");


            emailBody.Append("<tr>");
            emailBody.Append("<td align=\"left\"><b>Activity:</b></td><td align=\"left\"> " + EmailActivity + "</td>");
            emailBody.Append("</tr>");


            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Description:</b></td><td align=\"left\"> " + EmailDescription + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Internal Comments:</b></td><td align=\"left\"> " + EmailInternal + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr></tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Adjuster:</b></td><td align=\"left\"> " + adjuster.adjusterName + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>User Name:</b></td><td align=\"left\"> " + userName + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Service Date:</b></td><td align=\"left\"> " + EmailDate + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Timer Quantity:</b></td><td align=\"left\"> " + EmailQuantity + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("</table ><br><br>");


            emailBody.Append("</div>");	// inner containerBox
            emailBody.Append("</div>");	// paneContentInner
            emailBody.Append("</div>");	// containerBox

            //Core.EmailHelper.sendEmail(user.Email, recipients, null, "Claim Assignment Notification", emailBody.ToString(), null, user.emailHost, smtpPort, smtpEmail, smtpPassword, true);
            password = Core.SecurityManager.Decrypt(user.emailPassword);

            //Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, user.isSSL ?? true);
            Core.EmailHelper.sendEmail(itsgEmail, recipients, null, subject, emailBody.ToString(), null, itsgHost, Convert.ToInt32(itsgHostPort), itsgEmail, itsgEmailPassword);
        }

		protected void btnCancelClaimService_Click(object sender, EventArgs e) {
			showServiceGridPanel();
		}

		protected void lbtnNewClaimService_Click(object sender, EventArgs e) {
			AdjusterMaster adjuster = null;
			Claim claim = null;
			int claimID = SessionHelper.getClaimID();
			int clientID = SessionHelper.getClientId();

			showServiceEditPanel();

			this.txtServiceQty.Value = 0;
			this.txtServiceDescription.Text = string.Empty;
			this.ddlInvoiceServiceType.SelectedIndex = -1;
			txtServiceDate.Text = string.Empty;

			// load current adjuster assigned to claim
			claim = Data.Account.ClaimsManager.GetByID(claimID);
			if (claim != null && claim.AdjusterID != null) {
				adjuster = Data.Account.AdjusterManager.GetAdjusterId((int)claim.AdjusterID);
				if (adjuster != null) {
					txtServiceAdjuster.Text = adjuster.adjusterName;
					this.hf_serviceAdjusterID.Value = adjuster.AdjusterId.ToString();
				}
			}

			ViewState["ClaimServiceID"] = "0";

			bindServiceTypes();
		}

		private void showServiceEditPanel() {
			pnlEditService.Visible = true;
			//pnlGrid.Visible = false;
		}

		private void showServiceGridPanel() {
			pnlEditService.Visible = false;
			//pnlGrid.Visible = true;
		}

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Protected/ClaimEdit.aspx");
        }
	}
}