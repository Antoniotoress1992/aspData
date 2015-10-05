using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;
using CRM.RuleEngine;
using CRM.Data.Entities;
using System.Text;

namespace CRM.Web.UserControl {
	public partial class ucClaimExpense : System.Web.UI.UserControl {
		protected CRM.Web.Protected.ClaimRuler masterPage = null;
		protected void Page_Load(object sender, EventArgs e) {
			masterPage = this.Page.Master.Master as Protected.ClaimRuler;
		}

		public void activateEditPanel() {
			lbtnNewExpense_Click(null, null);
		}

		public void bindData(int claimID) {
			masterPage = this.Page.Master.Master as Protected.ClaimRuler;

			int clientID = SessionHelper.getClientId();

			List<ClaimExpense> expenses = null;
			List<ExpenseType> expenseTypes = null;

			expenses = loadExpenses(claimID);

		    if(Session["ComingFromAllClaimsExpense"] != null)
            {
                pnlEditExpense.Visible = true;
            }

			gvExpense.DataSource = expenses;
			gvExpense.DataBind();
            if (Session["ComingFromAllClaimsExpense"] != null)//NEW OC 10/20/2014 ADDED to handle if coming from the all claims screen on the "addNote" button
            {
                var proID = Session["CarrierInvoiceID"]; //only thing that changed
                using (ExpenseTypeManager repository = new ExpenseTypeManager())
                {
                    expenseTypes = repository.GetAll2(Convert.ToInt32(proID)).ToList(); //was clientID
                }
                Core.CollectionManager.FillCollection(ddlExpenseType, "ExpenseTypeID", "ExpenseName", expenseTypes);
                Session["ExpenseCount"] = ddlExpenseType.Items.Count;
            }
            else
            {
                var proID = Session["InvoiceProfileID"];
                using (ExpenseTypeManager repository = new ExpenseTypeManager())
                {
                    expenseTypes = repository.GetAll2(Convert.ToInt32(proID)).ToList(); //was clientID
                }
                Core.CollectionManager.FillCollection(ddlExpenseType, "ExpenseTypeID", "ExpenseName", expenseTypes);
                Session["ExpenseCount"] = ddlExpenseType.Items.Count;
            }
		}

		protected void gvExpense_RowCommand(object sender, GridViewCommandEventArgs e) {
			ClaimExpense claimExpense = null;
			int claimID = SessionHelper.getClaimID();
			int id = 0;

			if (e.CommandName == "DoEdit") {
				id = Convert.ToInt32(e.CommandArgument);

				using (ClaimExpenseManager repository = new ClaimExpenseManager()) {
					claimExpense = repository.Get(id);					
				}
				if (claimExpense != null) 
                {
					txtExpenseAmount.Value = claimExpense.ExpenseAmount;
					txtExpenseQty.Value = claimExpense.ExpenseQty;

					if (claimExpense.ExpenseDate != null)
						txtExpenseDate.Value = claimExpense.ExpenseDate;

					txtExpenseDescription.Text = claimExpense.ExpenseDescription;
					cbxExpenseReimburse.Checked = claimExpense.IsReimbursable;
					ddlExpenseType.SelectedValue = claimExpense.ExpenseTypeID.ToString();
                    txtMyComments.Text = claimExpense.InternalComments;
                    //cbIsBillable.Checked = claimExpense.IsBillable.Value;
                    //NEW OC 10/7/2014 // PUT IN PLACE TO GRAB THE NEW RATE IF USER HITS EDIT
                    ExpenseType expense = null;
                    int expenseID = 0;
                    if (ddlExpenseType.SelectedIndex > 0)
                    {
                        expenseID = Convert.ToInt32(ddlExpenseType.SelectedValue);

                        using (ExpenseTypeManager repository = new ExpenseTypeManager())
                        {
                            expense = repository.Get(expenseID);
                            if (expense != null)
                            {
                                txtExpenseAmount.Value = expense.ExpenseRate;
                                Session["multiplier"] = expense.ExpenseRate;
                            }
                        }

                    }
                    // END NEW OC 10/7/2014
					
                    if (claimExpense.AdjusterID != null) {
						txtExpenseAdjuster.Text = claimExpense.AdjusterMaster.adjusterName;
						hf_expenseAdjusterID.Value = claimExpense.AdjusterMaster.AdjusterId.ToString();
					}

					showExpenseEditPanel();

					ViewState["ClaimExpenseID"] = e.CommandArgument.ToString();
				}
			}
			else if (e.CommandName == "DoDelete") {
				id = Convert.ToInt32(e.CommandArgument);

				try {
					using (ClaimExpenseManager repository = new ClaimExpenseManager()) {
						repository.Delete(id);
					}

					// refresh grid
					gvExpense.DataSource = loadExpenses(claimID); 
					gvExpense.DataBind();
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);

					lblMessage.Text = "Unable to delete claim expense.";
					lblMessage.CssClass = "error";
				}
			}
		}

		protected void lbtnNewExpense_Click(object sender, EventArgs e) {
			AdjusterMaster adjuster = null;
			
			Claim claim = null;
			int claimID = SessionHelper.getClaimID();
			showExpenseEditPanel();
			
			txtExpenseAmount.Value = 0;
			txtExpenseQty.Value = 0;
			txtExpenseDescription.Text = string.Empty;
			txtExpenseDate.Text = string.Empty;
			ddlExpenseType.SelectedIndex = -1;
			cbxExpenseReimburse.Checked = false;
            txtMyComments.Text = "";
			ViewState["ClaimExpenseID"] = 0;

			// load current adjuster assigned to claim
			claim = Data.Account.ClaimsManager.GetByID(claimID);
			if (claim != null && claim.AdjusterID != null) {
				adjuster = Data.Account.AdjusterManager.GetAdjusterId((int)claim.AdjusterID);
				if (adjuster != null) {
					txtExpenseAdjuster.Text = adjuster.adjusterName;
					hf_expenseAdjusterID.Value = adjuster.AdjusterId.ToString();
				}
			}

		}

		protected void btnSaveExpense_Click(object sender, EventArgs e) {
			Claim claim = null;
            Claim myClaim = null;
			ClaimExpense claimExpense = null;
            AdjusterMaster adjuster = null;
            CarrierInvoiceProfile CarrierInvoice = null;

			int clientID = SessionHelper.getClientId();
			int userID = SessionHelper.getUserId();
			int claimID = SessionHelper.getClaimID();
            int myAdjusterID = 0;
            int profileID = 0;
			int expenseTypeID = 0;
			int id = 0;
			

			Page.Validate("expense");
			if (!Page.IsValid)
				return;			

			id = Convert.ToInt32(ViewState["ClaimExpenseID"]);

            ClaimManager cm = new ClaimManager();
            myClaim = cm.Get(claimID);
			
			try 
            {
				expenseTypeID = Convert.ToInt32(ddlExpenseType.SelectedValue);
				
				using (TransactionScope scope = new TransactionScope()) 
                {
					using (ClaimExpenseManager repository = new ClaimExpenseManager()) 
                    {
						if (id == 0) {
							claimExpense = new ClaimExpense();
							claimExpense.ClaimID = claimID;
						}
						else {
							claimExpense = repository.Get(id);
						}
						
						// populate fields
                        if(txtExpenseAmount.Visible == false)
                        {
                            var x = Session["multiplier"].ToString();
                            
                            double expenseAmount = Convert.ToDouble(x) * Convert.ToDouble(txtExpenseQty.ValueDecimal); //newOC 10/7/14
                            claimExpense.ExpenseAmount = Convert.ToDecimal(expenseAmount);

                            decimal d = Convert.ToDecimal(expenseAmount);
                            //Session["EmailAmount"] = "$" + Math.Round(d, 2);
                            Session["EmailAmount"] = String.Format("{0:0.00}", expenseAmount);
                           // Session["EmailAmount"] = expenseAmount;
                        }
                        else
                        {
                            claimExpense.ExpenseAmount = txtExpenseAmount.ValueDecimal;
                        }
						claimExpense.ExpenseDate = txtExpenseDate.Date;
						claimExpense.ExpenseDescription = txtExpenseDescription.Text.Trim();
						claimExpense.ExpenseTypeID = expenseTypeID;
						claimExpense.IsReimbursable = cbxExpenseReimburse.Checked;
						claimExpense.UserID = userID;
						claimExpense.AdjusterID = Convert.ToInt32(hf_expenseAdjusterID.Value);
						claimExpense.ExpenseQty = txtExpenseQty.ValueDecimal;
                        claimExpense.InternalComments = txtMyComments.Text.Trim();
                        claimExpense.Billed = false;
                       // claimExpense.IsBillable = cbIsBillable.Checked;
						// save expense
						claimExpense = repository.Save(claimExpense);
					}

					// update diary entry
					ClaimComment diary = new ClaimComment();
					diary.ClaimID = claimID;
					diary.CommentDate = DateTime.Now;
					diary.UserId = userID;
					diary.CommentText = string.Format("Expense: {0}, Description: {1}, Date: {2:MM/dd/yyyy}, Amount: {3:N2}, Adjuster: {4} Qty: {5:N2}",
												ddlExpenseType.SelectedItem.Text,
												claimExpense.ExpenseDescription,
												claimExpense.ExpenseDate,
												claimExpense.ExpenseAmount,
												txtExpenseAdjuster.Text,
												claimExpense.ExpenseQty
												);
					ClaimCommentManager.Save(diary);

					// 2014-05-02 apply rule
					using (SpecificExpenseTypePerCarrier ruleEngine = new SpecificExpenseTypePerCarrier()) 
                    {
						claim = new Claim();
						claim.ClaimID = claimID;
						RuleException ruleException = ruleEngine.TestRule(clientID, claim, expenseTypeID);

						if (ruleException != null) {
							ruleException.UserID = Core.SessionHelper.getUserId();
							ruleEngine.AddException(ruleException);
                            CheckSendMail(ruleException);
						}
					}
                    myAdjusterID = Convert.ToInt32(claimExpense.AdjusterID); //Convert.ToInt32(myClaim.AdjusterID);
                    //EMAIL ADJUSTER OC 10/22/2014
                    if (myAdjusterID != 0 || myAdjusterID != null)
                    {
                        adjuster = AdjusterManager.GetAdjusterId(myAdjusterID);
                    }
                    if (cbEmailAdjuster.Checked == true)
                    {
                        try
                        {
                            notifyAdjuster(adjuster, claimExpense, myClaim);
                           
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
                              notifyClientContact(CarrierInvoice, claimExpense, myClaim, adjuster);
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Unable to send email to client contact";
                            lblMessage.CssClass = "error";
                        }

                    }
                    //EMAIL TO WHOMEVER OC 10/22/2014
                    if (txtEmailTo.Text != "")
                    {

                        try
                        {
                             notifySpecifiedUser(adjuster, claimExpense, myClaim);
                        }
                        catch (Exception ex)
                        {
                            lblMessage.Text = "Unable to send email to adjuster";
                            lblMessage.CssClass = "error";
                        }
                    }
					
					// complete transaction
					scope.Complete();
				}

				lblMessage.Text = "Expense saved successfully.";
				lblMessage.CssClass = "ok";

				// refresh grid
				gvExpense.DataSource = loadExpenses(claimID);
				gvExpense.DataBind();

				// keep edit form active
				lbtnNewExpense_Click(null, null);
                lblAmount.Text = "";
                lblAmount.Visible = false;
                txtExpenseAmount.Visible = true;
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);

				lblMessage.Text = "Unable to save claim expense.";
				lblMessage.CssClass = "error";
			}
		}
        //EMAIL ADJUSTER
        private void notifyAdjuster(AdjusterMaster adjuster, ClaimExpense claimExpense, Claim myClaim)
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

            string EmailExpense = ddlExpenseType.SelectedItem.Text;
           // string EmailActivity = ddlActivity.SelectedItem.Text;
            string EmailDescription = txtExpenseDescription.Text;
            string EmailInternal = txtMyComments.Text;
            string EmailQuantity = txtExpenseQty.Text;
            string EmailDate = txtExpenseDate.Text;
            string userName = SessionHelper.getUserName();
            string isReimburse = null;
            string EmailAmount = null;
            string EmailRate = null;
            if(lblAmount.Visible == true)
            {
                //EmailAmount = Session["EmailAmount"];
                //decimal d = Convert.ToDecimal(Session["EmailAmount"]);
                EmailAmount = "$" + Session["EmailAmount"];
                EmailRate = lblAmount.Text;
            }
            else
            {
                EmailAmount = txtExpenseAmount.Text;
                EmailRate = "No rate specified";
            }

            bool reimburse = cbxExpenseReimburse.Checked;
            if(reimburse == true)
            {
                isReimburse = "Yes";
            }
            else
            {
                isReimburse = "No";
            }
            
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
          

            emailBody.Append("<table >");
            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Expense:</b></td><td align=\"left\"> " + EmailExpense + "</td>");
            emailBody.Append("</tr>");
            // emailBody.Append("</table><br><br>");


            emailBody.Append("<tr>");
            emailBody.Append("<td align=\"left\"><b>Description:</b></td><td align=\"left\"> " + EmailDescription + "</td>");
            emailBody.Append("</tr>");


            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Internal Comments:</b></td><td align=\"left\"> " + EmailInternal + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Adjuster:</b></td><td align=\"left\"> " + adjuster.adjusterName + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr></tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>User Name:</b></td><td align=\"left\"> " + userName + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Expense Date/Time:</b></td><td align=\"left\"> " + EmailDate + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Reimburse?</b></td><td align=\"left\"> " + isReimburse + "</td>");
            emailBody.Append("</tr>"); 

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Quantity:</b></td><td align=\"left\"> " + EmailQuantity + "</td>");
            emailBody.Append("</tr>");


            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Rate:</b></td><td align=\"left\"> " + EmailRate + "</td>");
            emailBody.Append("</tr>"); 

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Amount:</b></td><td align=\"left\"> " + EmailAmount + "</td>");
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
        //EMAIL CLIENT CONTACT
        private void notifyClientContact(CarrierInvoiceProfile CarrierInvoice, ClaimExpense claimExpense, Claim myClaim, AdjusterMaster adjuster)
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

            string EmailExpense = ddlExpenseType.SelectedItem.Text;
            // string EmailActivity = ddlActivity.SelectedItem.Text;
            string EmailDescription = txtExpenseDescription.Text;
            string EmailInternal = txtMyComments.Text;
            string EmailQuantity = txtExpenseQty.Text;
            string EmailDate = txtExpenseDate.Text;
            string userName = SessionHelper.getUserName();
            string isReimburse = null;
            string EmailAmount = null;
            string EmailRate = null;
            if (lblAmount.Visible == true)
            {
                //EmailAmount = "$" + Convert.ToDecimal(Session["EmailAmount"]);
                //EmailRate = lblAmount.Text;
                EmailAmount = "$" + Session["EmailAmount"];
                EmailRate = lblAmount.Text;
            }
            else
            {
                EmailAmount = txtExpenseAmount.Text;
                EmailRate = "No rate specified";
            }

            bool reimburse = cbxExpenseReimburse.Checked;
            if (reimburse == true)
            {
                isReimburse = "Yes";
            }
            else
            {
                isReimburse = "No";
            }

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
            emailBody.Append("<td align=\"left\"><b>Expense:</b></td><td align=\"left\"> " + EmailExpense + "</td>");
            emailBody.Append("</tr>");
            // emailBody.Append("</table><br><br>");


            emailBody.Append("<tr>");
            emailBody.Append("<td align=\"left\"><b>Description:</b></td><td align=\"left\"> " + EmailDescription + "</td>");
            emailBody.Append("</tr>");


            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Internal Comments:</b></td><td align=\"left\"> " + EmailInternal + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Adjuster:</b></td><td align=\"left\"> " + adjuster.adjusterName + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr></tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>User Name:</b></td><td align=\"left\"> " + userName + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Expense Date/Time:</b></td><td align=\"left\"> " + EmailDate + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Reimburse?</b></td><td align=\"left\"> " + isReimburse + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Quantity:</b></td><td align=\"left\"> " + EmailQuantity + "</td>");
            emailBody.Append("</tr>");


            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Rate:</b></td><td align=\"left\"> " + EmailRate + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Amount:</b></td><td align=\"left\"> " + EmailAmount + "</td>");
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
        private void notifySpecifiedUser(AdjusterMaster adjuster, ClaimExpense claimExpense, Claim myClaim)
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

            string EmailExpense = ddlExpenseType.SelectedItem.Text;
            // string EmailActivity = ddlActivity.SelectedItem.Text;
            string EmailDescription = txtExpenseDescription.Text;
            string EmailInternal = txtMyComments.Text;
            string EmailQuantity = txtExpenseQty.Text;
            string EmailDate = txtExpenseDate.Text;
            string userName = SessionHelper.getUserName();
            string isReimburse = null;
            string EmailAmount = null;
            string EmailRate = null;
            if (lblAmount.Visible == true)
            {
                EmailAmount = "$" + Session["EmailAmount"];
                //EmailRate = lblAmount.Text;
            }
            else
            {
                EmailAmount = txtExpenseAmount.Text;
                EmailRate = "No rate specified";
            }

            bool reimburse = cbxExpenseReimburse.Checked;
            if (reimburse == true)
            {
                isReimburse = "Yes";
            }
            else
            {
                isReimburse = "No";
            }

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
            emailBody.Append("<td align=\"left\"><b>Expense:</b></td><td align=\"left\"> " + EmailExpense + "</td>");
            emailBody.Append("</tr>");
            // emailBody.Append("</table><br><br>");


            emailBody.Append("<tr>");
            emailBody.Append("<td align=\"left\"><b>Description:</b></td><td align=\"left\"> " + EmailDescription + "</td>");
            emailBody.Append("</tr>");


            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Internal Comments:</b></td><td align=\"left\"> " + EmailInternal + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Adjuster:</b></td><td align=\"left\"> " + adjuster.adjusterName + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr></tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>User Name:</b></td><td align=\"left\"> " + userName + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Expense Date/Time:</b></td><td align=\"left\"> " + EmailDate + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Reimburse?</b></td><td align=\"left\"> " + isReimburse + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Quantity:</b></td><td align=\"left\"> " + EmailQuantity + "</td>");
            emailBody.Append("</tr>");


            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Rate:</b></td><td align=\"left\"> " + EmailRate + "</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr> ");
            emailBody.Append("<td align=\"left\"><b>Amount:</b></td><td align=\"left\"> " + EmailAmount + "</td>");
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

		protected void btnCancelExpense_Click(object sender, EventArgs e) {
			showExpenseGridPanel();					
		}


		private List<ClaimExpense> loadExpenses(int claimID) {
			List<ClaimExpense> expenses = null;

			using (ClaimExpenseManager repository = new ClaimExpenseManager()) {
				expenses = repository.GetAll(claimID);
			}

			return expenses;
		}

		
		private void showExpenseEditPanel() {
			pnlEditExpense.Visible = true;
			//pnlGrid.Visible = false;
		}

		private void showExpenseGridPanel() {
			pnlEditExpense.Visible = false;
			//pnlGrid.Visible = true;
		}

		protected void ddlExpenseType_SelectedIndexChanged(object sender, EventArgs e) 
        {
			ExpenseType expense = null;
            
			int expenseID = 0;

			if (ddlExpenseType.SelectedIndex > 0) {
				expenseID = Convert.ToInt32(ddlExpenseType.SelectedValue);

				using(ExpenseTypeManager repository = new ExpenseTypeManager()){
					expense = repository.Get(expenseID);
					if (expense != null) 
                    {
                        Session["multiplier"] = expense.ExpenseRate;
                        if(Convert.ToDouble(expense.ExpenseRate) == 0.00)
                        {
                            txtExpenseQty.Text = "";
                            txtExpenseQty.Enabled = false;
                            txtExpenseAmount.Visible = true;
                            lblMyAmount.Text = "Amount";
                            lblAmount.Text = "";
                            lblAmount.Visible = false;
                        }
                        else
                        {
                            txtExpenseQty.Enabled = true;
                            lblAmount.Visible = true;
                            lblAmount.Text = "$" + expense.ExpenseRate.ToString();
                            txtExpenseAmount.Visible = false;
                            lblMyAmount.Text = "Rate";
                        }
                       // txtExpenseAmount.Value = "";//expense.ExpenseRate;
                        
					}
				}
				
			}
		}

		protected void CustomValidator_Amount_Qty(object source, ServerValidateEventArgs args) {
			if (txtExpenseAmount.ValueDecimal > 0 && txtExpenseQty.ValueDecimal > 0)
				args.IsValid = false;
			else
				args.IsValid = true;
		}

        #region send reg flag mail

        public static void CheckSendMail(RuleException ruleExp)
        {
            if (ruleExp != null)
            {
                string adjusterEmail = string.Empty;
                string supervisorEmail = string.Empty;
                bool sendAdjuster = false;
                bool sendSupervisor = false;
                string recipient = string.Empty;
                int claimId = 0;

                BusinessRuleManager objRuleManager = new BusinessRuleManager();
                BusinessRule objRule = new BusinessRule();
                CRM.Data.Entities.Claim objClaim = new CRM.Data.Entities.Claim();
                CRM.Data.Entities.SecUser objSecUser = new Data.Entities.SecUser();
                AdjusterMaster adjustermaster = new AdjusterMaster();

                int businessRuleID = ruleExp.BusinessRuleID ?? 0;
                objRule = objRuleManager.GetBusinessRule(businessRuleID);
                if (objRule != null)
                {
                    claimId = ruleExp.ObjectID ?? 0;

                    objClaim = objRuleManager.GetClaim(claimId);
                    adjustermaster = objRuleManager.GetAdjuster(objClaim.AdjusterID ?? 0);
                    objSecUser = objRuleManager.GetSupervisor(objClaim.SupervisorID ?? 0);
                    if (objSecUser != null)
                    {
                        adjusterEmail = adjustermaster.email;
                        supervisorEmail = objSecUser.Email;

                        sendAdjuster = objRule.EmailAdjuster;
                        sendSupervisor = objRule.EmailSupervisor;

                        if (sendAdjuster == true && sendSupervisor == true)
                        {
                            recipient = adjusterEmail + "," + supervisorEmail;
                            notifyUser(objRule.Description, claimId, recipient);
                        }
                        else if (sendAdjuster == false && sendSupervisor == true)
                        {
                            recipient = supervisorEmail;
                            notifyUser(objRule.Description, claimId, recipient);
                        }
                        else if (sendAdjuster == true && sendSupervisor == false)
                        {

                            recipient = adjusterEmail;
                            notifyUser(objRule.Description, claimId, recipient);
                        }
                    }
                }
            }

        }

        public static void notifyUser(string description, int claimid, string recipient)
        {

            StringBuilder emailBody = new StringBuilder();
            string password = null;
            string[] recipients = null;
            string smtpHost = null;
            int smtpPort = 0;
            //string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
            string subject = "Red-Flag Alert: " + description + " Claim # " + claimid;
            CRM.Data.Entities.SecUser user = null;

            // get logged in user
            int userID = SessionHelper.getUserId();
            // get logged in user email info
            user = SecUserManager.GetByUserId(userID);
            // load email credentials
            smtpHost = user.emailHost;
            int.TryParse(user.emailHostPort, out smtpPort);

            // recipients
            recipients = new string[] { recipient };

            // build email body
            // .containerBox
            emailBody.Append("<div>");
            emailBody.Append("<div>");
            emailBody.Append("Claim Ruler Red Flag Alert.<br><br>");
            emailBody.Append("Please correct the following issue as soon as possible:  ");
            emailBody.Append(description + "with claim # " + claimid);
            emailBody.Append("</div>");	// paneContentInner
            emailBody.Append("</div>");	// containerBox

            password = Core.SecurityManager.Decrypt(user.emailPassword);

            Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, true);//user.isSSL ??

        }





        #endregion

        protected void gvExpense_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //ALL NEW OC 11/4/14 put in place to check whether or not a service has been billed or not
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hF = (HiddenField)e.Row.FindControl("hfBilled");
                ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                ImageButton btnDel = (ImageButton)e.Row.FindControl("btnDelete");
                if (hF.Value == "True")
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



	}
}