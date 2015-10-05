﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Views;
using LinqKit;
using System.Linq.Expressions;

using CRM.Core;
using CRM.Repository;

using Newtonsoft.Json;
using CRM.Data.Entities;
using System.Transactions;
using System.Text;
using CRM.RuleEngine;
using System.Web.Script.Serialization;
using System.Data;
using CRM.Data.Account;
namespace CRM.Web.Protected
{
    public partial class AjaxClientServices : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string InvoiceMessage { get; set; }
        public int ClaimIDNew { get; set; }
        public string CarrierInvoiceProfile { get; set; }
        public string FeeDesignation { get; set; }

        public decimal GrossLossPayable { get; set; }
        public decimal Depreciation { get; set; }
        public decimal PolicyDeductible { get; set; }
        public decimal NetClaimPayable { get; set; }

        [System.Web.Services.WebMethod]
        public static void saveUserPhoto(int userID, string filePath)
        {
            string photoFileName = null;
            System.Drawing.Image image = null;
            string imageFolderPath = null;
            string destinationFilePath = null;

            // get application path
            string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

            // get uploaded file
            string tempFilePath = HttpContext.Current.Server.MapPath(String.Format("~\\Temp\\{0}", filePath));



            try
            {
                imageFolderPath = string.Format("{0}/UserPhoto", appPath);

                if (!Directory.Exists(imageFolderPath))
                    Directory.CreateDirectory(imageFolderPath);

                // get file extension, 
                //photoFileName = userID.ToString() + Path.GetExtension(filePath);
                photoFileName = userID.ToString() + ".jpg";
                destinationFilePath = string.Format("{0}/{1}", imageFolderPath, photoFileName);

                // load image into memory
                image = System.Drawing.Image.FromFile(tempFilePath);
                image = Core.Common.resizeImage(image, new Size(100, 100));

                image.Save(destinationFilePath);
                image.Dispose();


                //System.IO.File.Copy(tempFilePath, destinationFilePath, true);

                // delete temp file
                File.Delete(tempFilePath);
            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

        }

        [System.Web.Services.WebMethod]
        public static string getClaimForProgress(int clientID, int progressID, int carrierid, int adjusterid)
        {
            string json = null;
            List<ClaimProgressData> claims = null;
            string encryptedClaimID = null;

            using (ClaimManager repository = new ClaimManager())
            {
                claims = repository.GetByProgressID(clientID, progressID, carrierid, adjusterid);
            }

            // add link to claim page
            if (claims != null && claims.Count > 0)
            {
                foreach (ClaimProgressData claim in claims)
                {
                    encryptedClaimID = Core.SecurityManager.EncryptQueryString(claim.claimID.ToString());

                    claim.url = string.Format("<a href=\"javascript:PopupCenter('../Protected/ClaimEdit.aspx?q={0}');\">View</a>", encryptedClaimID);
                }

            }

            //ComputerBeacon.Json.Serializer.Serialize(leads);


            json = Newtonsoft.Json.JsonConvert.SerializeObject(claims);

            return json;
        }

        [System.Web.Services.WebMethod]
        public static string getRuleExceptionStatistics(int clientID)
        {
            string json = null;
            List<RuleExceptionStatisticsView> stats = null;

            using (RuleExceptionManager repository = new RuleExceptionManager())
            {
                stats = repository.GetExceptionStatistics(clientID);
            }



            if (stats != null && stats.Count > 0)
            {
                json = Newtonsoft.Json.JsonConvert.SerializeObject(stats);
            }


            return json;
        }

        [System.Web.Services.WebMethod]
        public static string getExceptionsForRule(int clientID, int ruleID)
        {
            string encryptedValue = null;
            string json = null;
            List<RuleExceptionView> ruleExceptions = null;

            using (RuleExceptionManager repository = new RuleExceptionManager())
            {
                ruleExceptions = repository.GetByRuleID(clientID, ruleID);
            }


            if (ruleExceptions != null && ruleExceptions.Count > 0)
            {
                foreach (RuleExceptionView view in ruleExceptions)
                {
                    if (view.UserID == null)
                        view.UserName = "System";

                    switch (view.ObjectTypeID)
                    {
                        case (int)Globals.ObjectType.Invoice:

                            Invoice invoice = InvoiceManager.GetByID((int)view.ObjectID);

                            if (invoice != null)
                            {
                                encryptedValue = Core.SecurityManager.EncryptQueryString(view.ObjectID.ToString());

                                // add link to invoice page
                                view.url = string.Format("<a class='link' href=\"javascript:PopupCenter('../Protected/LeadInvoice.aspx?q={0}');\">Invoice #{1}</a>", encryptedValue, invoice.InvoiceNumber);
                                
                            }
                            break;

                        case (int)Globals.ObjectType.Claim:
                            using (ClaimManager repository = new ClaimManager())
                            {
                                Claim claim = repository.Get((int)view.ObjectID);

                                if (claim != null)
                                {
                                    encryptedValue = Core.SecurityManager.EncryptQueryString(view.ObjectID.ToString());

                                    // add link to claim page
                                    view.url = string.Format("<a class='link' href=\"javascript:PopupCenter('../Protected/ClaimEdit.aspx?id={0}');\">{1}</a>", encryptedValue, claim.LeadPolicy.Leads.insuredName);
                                    view.InsureClaim = claim.InsurerClaimNumber;
                                    Carrier CarrierObj = new Carrier();
   
                                    if (claim.CarrierID != null) {
                                        CarrierObj = CarrierManager.GetByID(Convert.ToInt32(claim.CarrierID));
                                        view.Carrier = CarrierObj.CarrierName;
                                    }
                                   
                                    
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }


            }

            json = Newtonsoft.Json.JsonConvert.SerializeObject(ruleExceptions);

            return json;
        }


        [System.Web.Services.WebMethod]
        public static string SaveNotes(int claimID, string serviceQty, string serviceDate, string descp, string invoiceServiceType, int invoiceServiceTypeId, string serviceAdjuster, string serviceAdjustId, int leadID, string emailTo)
        {
            string json = "";
            ClaimService claimService = null;
            ClaimComment diary = null;
            Leads objLeads = null;
            Claim objClaim = null;
            int userID = SessionHelper.getUserId();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ClaimServiceManager repository = new ClaimServiceManager())
                    {
                        claimService = new ClaimService();
                        claimService.ClaimID = claimID;


                        claimService.ServiceQty = serviceQty == null ? 0 : Convert.ToDecimal(serviceQty);
                        claimService.ServiceDate = Convert.ToDateTime(serviceDate);
                        claimService.ServiceDescription = descp.Trim();
                        claimService.ServiceTypeID = Convert.ToInt32(invoiceServiceTypeId);
                        claimService.UserID = userID;
                        claimService.AdjusterID = Convert.ToInt32(serviceAdjustId);

                        claimService = repository.Save(claimService);
                    }

                    // diary
                    diary = new ClaimComment();
                    diary.ClaimID = claimID;
                    diary.CommentDate = DateTime.Now;
                    diary.UserId = userID;
                    diary.ActivityType = "Service: " + invoiceServiceType;
                    diary.CommentText = string.Format("Description: {0}, Date {1:MM/dd/yyyy h:mm}, Qty: {2:N2}, Adjuster: {3}",
                                                claimService.ServiceDescription,
                                                claimService.ServiceDate,
                                                claimService.ServiceQty,
                                                serviceAdjuster
                                                );
                    ClaimCommentManager.Save(diary);

                    scope.Complete();
                    //  SendNoteEmail();
                }
                objLeads = LeadsManager.GetByLeadId(leadID);
                ClaimManager objClaimManager = new ClaimManager();
                objClaim = objClaimManager.Get(claimID);
                string insuerFileId = objClaim.InsurerClaimNumber;
                string insurerName = objLeads.InsuredName;
                string claimNumber = objClaim.AdjusterClaimNumber;
                string userName = SessionHelper.getUserName();
                SendNoteEmail(insuerFileId, insurerName, claimNumber, serviceAdjuster, descp.Trim(), userName, emailTo, serviceDate, serviceQty);

                json = "Service save successfully";

            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

            return json;
        }

        public static void SendNoteEmail(string insuerFileId, string insurerName, string claimNumber, string adjusterName, string desc, string userName, string emailTo, string serviceDate, string serviceQty)
        {

            StringBuilder emailBody = new StringBuilder();
            string password = null;
            string[] recipients = null;
            string smtpHost = null;
            int smtpPort = 0;
            //string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
            string subject = "Claim # " + insuerFileId + ", Please Read Claim Note, Claim Ruler Note for : " + insurerName;
            CRM.Data.Entities.SecUser user = null;

            // get logged in user
            int userID = SessionHelper.getUserId();
            // get logged in user email info
            user = SecUserManager.GetByUserId(userID);
            // load email credentials
            smtpHost = user.emailHost;
            string appurl = ConfigurationManager.AppSettings["appURL"].ToString();

            int.TryParse(user.emailHostPort, out smtpPort);


            // code for add adjuster and supervisor email for add in recipients


            // recipients
            recipients = new string[] { };
            recipients = emailTo.Split(';');
            // build email body
            // .containerBox
            emailBody.Append("<div>");
            emailBody.Append("<div>");
            emailBody.Append("Hi " + adjusterName + ",<br><br>");
            emailBody.Append("The following service hours that were just logged on Claim # " + claimNumber + " for Insured: " + insurerName + "<br>");


            emailBody.Append("<table>");

            emailBody.Append("<tr>");
            emailBody.Append("<td style='width:200px;'>");
            emailBody.Append("<b>Description:</b>");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(desc);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("<b>Adjuster</b>");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(adjusterName);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("<b>User Name:</b>");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(userName);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("<b>Service Date/Time:</b>");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(serviceDate);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("<b>Quantity:</b>");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(serviceQty);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            //emailBody.Append("<tr>");
            //emailBody.Append("<td>");
            //emailBody.Append("<b>E-mail Recipients:</b>");
            //emailBody.Append("</td>");
            //emailBody.Append("<td>");
            //emailBody.Append(emailTo);
            //emailBody.Append("</td>");
            //emailBody.Append("</tr>");


            emailBody.Append("</table>");
            emailBody.Append("</div>");	// paneContentInner
            emailBody.Append("</div>");	// containerBox

            password = Core.SecurityManager.Decrypt(user.emailPassword);

            // Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, true);//user.isSSL ??
            string crsupportEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
            string crsupportEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
            Core.EmailHelper.sendEmail(crsupportEmail, recipients, null, subject, emailBody.ToString(), null, crsupportEmail, crsupportEmailPassword);


        }




        [System.Web.Services.WebMethod]
        public static string SaveClaimStatus(int claimStatus, string insurerClaimId, string insurerName, int claimAdjusterId, string adjusterComapnyName, string updatedby, string commentNote, string emailTo, int carrierID, int claimID, string recipientId, string claimAdjuster, string claimStatusName, string carrier, string idOf)
        {
            string json = "";
            Claim objclaim = null;
            AdjusterMaster objAdjusterMaster = null;
            Leads objLeads = null;
            CRM.Data.Entities.LeadPolicy objLeadPolicy = null;
            ClaimComment comment = null;
            Client objClient = null;
            int userID = SessionHelper.getUserId();
            int leadID = 0;
            int policyId = 0;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ClaimManager repository = new ClaimManager())
                    {
                        objclaim = new Claim();
                        objclaim.ClaimID = claimID;
                        objclaim.StatusID = claimStatus;
                        objclaim.InsurerClaimNumber = insurerClaimId;
                        //objclaim.CarrierID = carrierID;
                        objclaim.AdjusterID = claimAdjusterId;
                        objclaim.StatusUpdatedBy = updatedby;
                        objclaim.StatusCommentNote = commentNote;
                        //objclaim.StatusEmailTo = EmailTo;
                        repository.UpdateClaimStatus(objclaim);


                        // AdjusterMaster
                        objAdjusterMaster = new AdjusterMaster();
                        objAdjusterMaster.AdjusterId = claimAdjusterId;
                        //objAdjusterMaster.CompanyName = AdjusterComapnyName;
                        repository.UpdateAdjusterName(objAdjusterMaster);

                        //leads
                        leadID = repository.GetPolicyId(claimID);
                        objLeads = new Leads();
                        objLeads.LeadId = leadID;
                        objLeads.InsuredName = insurerName;
                        repository.UpdateInsurerName(objLeads);
                        //save carrier id in Lead policy
                        policyId = repository.GetLeadPolicyId(claimID);
                        objLeadPolicy = new Data.Entities.LeadPolicy();
                        objLeadPolicy.Id = policyId;
                        objLeadPolicy.CarrierID = carrierID;
                        repository.UpdateCarrierId(objLeadPolicy);
                        //claim comment for add notes                         			
                        comment = new ClaimComment();
                        comment.ClaimID = claimID;
                        comment.IsActive = true;
                        comment.UserId = Core.SessionHelper.getUserId();
                        comment.CommentDate = DateTime.Now;
                        comment.ActivityType = "Status Changed";
                        comment.CommentText = commentNote.Trim();
                        ClaimCommentManager.Save(comment);


                        //client company name
                        //Client c = ClaimsManager.GetClientByUserId(SessionHelper.getUserId());
                        //c.BusinessName = AdjusterComapnyName;
                        //ClaimsManager.SaveClient(c);

                    }
                    scope.Complete();
                }
                string[] recipId = recipientId.Split(',');
                string recipientEmailId = string.Empty;

                string[] idofTable = idOf.Split(',');
                int index2 = 0;
                for (int index = 0; index < recipId.Length; index++)
                {
                    index2 = 0;
                    int.TryParse(recipId[index], out index2);
                    if (idofTable[index] == "c")
                    {

                        Contact objContact = ContactManager.Get(index2);
                        if (!string.IsNullOrEmpty(objContact.Email))
                        {
                            if (recipientEmailId == "")
                            {
                                recipientEmailId = objContact.Email;
                            }
                            else
                            {
                                recipientEmailId = recipientEmailId + "," + objContact.Email;
                            }
                        }
                    }
                    else
                    {

                        AdjusterMaster objAdjuster = AdjusterManager.GetAdjusterId(index2);

                        if (!string.IsNullOrEmpty(objAdjuster.email))
                        {
                            if (recipientEmailId == "")
                            {
                                recipientEmailId = objAdjuster.email;
                            }
                            else
                            {
                                recipientEmailId = recipientEmailId + "," + objAdjuster.email;
                            }
                        }

                    }
                }
                if (!string.IsNullOrEmpty(recipientEmailId))
                {
                    notifyUser(claimStatus, insurerClaimId, insurerName, claimAdjusterId, adjusterComapnyName, updatedby, commentNote, "", carrierID, claimID, recipientEmailId, claimAdjuster, claimStatusName, carrier, recipId, idofTable);
                }
                json = "Status save successfully";

            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

            return json;
        }

        public static void notifyUser(int claimStatus, string insurerClaimId, string insurerName, int claimAdjusterId, string adjusterComapnyName, string updatedby, string commentNote, string emailTo, int carrierID, int claimid, string recipient, string claimAdjuster, string claimStatusName, string carrier, string[] recipId, string[] idofTable)
        {

            StringBuilder emailBody = new StringBuilder();
            string password = null;
            string[] recipients = null;
            string smtpHost = null;
            int smtpPort = 0;
            int supervisorId = 0;
            AdjusterMaster objAdjusterMaster = null;
            CRM.Data.Entities.SecUser objSecUser = null;
            //string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
            string subject = "Please Read Note, Claim Ruler Status Change to: " + claimStatusName + " on Claim # " + insurerClaimId;
            CRM.Data.Entities.SecUser user = null;

            // get logged in user
            int userID = SessionHelper.getUserId();
            // get logged in user email info
            user = SecUserManager.GetByUserId(userID);
            // load email credentials
            smtpHost = user.emailHost;
            string appurl = ConfigurationManager.AppSettings["appURL"].ToString();

            int.TryParse(user.emailHostPort, out smtpPort);

            recipient = emailTo != string.Empty && recipient != string.Empty ? emailTo + "," + recipient : recipient;
            recipient = emailTo != string.Empty && recipient == string.Empty ? emailTo : recipient;

            objAdjusterMaster = new AdjusterMaster();
            objAdjusterMaster = AdjusterManager.GetAdjusterId(claimAdjusterId);
            // code for add adjuster and supervisor email for add in recipients
            if (user.Email != string.Empty)
            {
                recipient = recipient + "," + user.Email;
            }
            if (!string.IsNullOrEmpty(objAdjusterMaster.email))
            {
                recipient = recipient + "," + objAdjusterMaster.email;
            }
            if (!string.IsNullOrEmpty(Convert.ToString(objAdjusterMaster.SupervisorID)))
            {
                SecUserManager.GetByUserId(objAdjusterMaster.SupervisorID ?? 0);
                objSecUser = new Data.Entities.SecUser();
                if (!string.IsNullOrEmpty(objSecUser.Email))
                {
                    recipient = recipient + "," + objSecUser.Email;
                }
            }

            // recipients
            recipients = new string[] { };
            recipients = recipient.Split(',');
            // build email body
            // .containerBox
            emailBody.Append("<div>");
            emailBody.Append("<div>");
            emailBody.Append("Hi " + claimAdjuster + ",<br><br>");

            emailBody.Append("<table>");
            emailBody.Append("<tr><td style='width:100px;'></td>");
            emailBody.Append("<td>");
            emailBody.Append("<b>Update Claim Status/Review</b><br/>");
            emailBody.Append("</td>");
            emailBody.Append("</tr>");
            emailBody.Append("</table>");

            emailBody.Append("<table>");

            emailBody.Append("<tr>");
            emailBody.Append("<td style='width:200px;'>");
            emailBody.Append("Update Status To:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(claimStatusName);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Insurer Claim #:");
            emailBody.Append("</td>");
            emailBody.Append("<td><a href='" + appurl + "/protected/Admin/UsersLeads.aspx?s=" + insurerClaimId + "'>");
            emailBody.Append(insurerClaimId);
            emailBody.Append("</a></td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Insured Name:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(insurerName);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Carrier:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(carrier);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Adjuster:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(claimAdjuster);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Adjuster Company Name:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(adjusterComapnyName);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");


            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Updated By:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(user.UserName);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Comment/Note:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(commentNote);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Email To:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(emailTo);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            if (recipId.Length > 0)
            {
                emailBody.Append("<tr>");
                emailBody.Append("<td style='vertical-align:top'>");
                emailBody.Append("Selected Recipients");
                emailBody.Append("</td>");
                emailBody.Append("<td>");

                //////

                emailBody.Append("<table style='border:1px solid;'>");
                emailBody.Append("<tr>");
                emailBody.Append("<td>");
                emailBody.Append("First Name");
                emailBody.Append("</td>");
                emailBody.Append("<td>");
                emailBody.Append("Last Name");
                emailBody.Append("</td>");
                emailBody.Append("<td>");
                emailBody.Append("Company Name");
                emailBody.Append("</td>");
                emailBody.Append("<td>");
                emailBody.Append("Email");
                emailBody.Append("</td>");
                emailBody.Append("<td>");
                emailBody.Append("Contact Title");
                emailBody.Append("</td>");
                emailBody.Append("</tr>");
                int index2 = 0;
                for (int index = 0; index < recipId.Length; index++)
                {
                    index2 = 0;
                    int.TryParse(recipId[index], out index2);
                    if (idofTable[index] == "c")
                    {

                        Contact objContact = ContactManager.Get(index2);
                        emailBody.Append("<tr>");

                        emailBody.Append("<td>");
                        emailBody.Append(objContact.FirstName);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objContact.LastName);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objContact.CompanyName);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objContact.Email);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objContact.ContactTitle);
                        emailBody.Append("</td>");
                        emailBody.Append("</tr>");

                    }
                    else
                    {
                        AdjusterMaster objAdjuster = AdjusterManager.GetAdjusterId(index2);

                        emailBody.Append("<tr>");

                        emailBody.Append("<td>");
                        emailBody.Append(objAdjuster.FirstName);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objAdjuster.LastName);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objAdjuster.CompanyName);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objAdjuster.email);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append("");
                        emailBody.Append("</td>");
                        emailBody.Append("</tr>");
                    }
                }


                emailBody.Append("</table>");
                emailBody.Append("</td>");
                emailBody.Append("</tr>");
            }

            emailBody.Append("</table>");
            emailBody.Append("</div>");	// paneContentInner
            emailBody.Append("</div>");	// containerBox

            password = Core.SecurityManager.Decrypt(user.emailPassword);

            // Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, true);//user.isSSL ??
            string crsupportEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
            string crsupportEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
            Core.EmailHelper.sendEmail(crsupportEmail, recipients, null, subject, emailBody.ToString(), null, crsupportEmail, crsupportEmailPassword);


        }


        [System.Web.Services.WebMethod]
        public static string[] GetStatusData(int claimID)
        {
            string[] json = new string[6];
            Claim objclaim = null;
            int userID = SessionHelper.getUserId();

            try
            {
                objclaim = ClaimsManager.Get(claimID);
                if (objclaim != null)
                {
                    json[0] = (objclaim.StatusID ?? 0).ToString();
                    json[1] = objclaim.InsurerClaimNumber;
                    json[2] = objclaim.LeadPolicy.Leads.insuredName; ;
                    json[3] = objclaim.LeadPolicy.CarrierID.ToString();
                    json[4] = objclaim.AdjusterMaster.adjusterName;
                    json[5] = objclaim.AdjusterMaster.AdjusterId.ToString();
                }

            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

            return json;
        }


        [System.Web.Services.WebMethod]
        public static string SaveClaimExpense(string expenseType, string insurerClaimId, string insurerName, int claimAdjusterId, string adjusterComapnyName, string updatedby, string commentNote, string emailTo, int carrierID, int claimID, string recipientId, string claimAdjuster, string expenseTypeName, string carrier, string idOf, string expenseQty, string expenseAmount, string expenseDate, string reimbrance)
        {
            string json = "";
            Claim objclaim = null;
            AdjusterMaster objAdjusterMaster = null;
            Leads objLeads = null;
            ClaimComment comment = null;
            ClaimExpense claimExpense = null;
            CRM.Data.Entities.LeadPolicy objLeadPolicy = null;
            int userID = SessionHelper.getUserId();
            int leadID = 0;
            int policyId = 0;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (ClaimManager repository = new ClaimManager())
                    {
                        objclaim = new Claim();
                        objclaim.ClaimID = claimID;

                        objclaim.InsurerClaimNumber = insurerClaimId;
                        //objclaim.CarrierID = carrierID;
                        objclaim.AdjusterID = claimAdjusterId;
                        // objclaim.StatusUpdatedBy = updatedby; 
                        repository.UpdateClaimStatus(objclaim);


                        // AdjusterMaster
                        objAdjusterMaster = new AdjusterMaster();
                        objAdjusterMaster.AdjusterId = claimAdjusterId;
                        //objAdjusterMaster.CompanyName = AdjusterComapnyName;
                        repository.UpdateAdjusterName(objAdjusterMaster);

                        //leads
                        leadID = repository.GetPolicyId(claimID);
                        objLeads = new Leads();
                        objLeads.LeadId = leadID;
                        objLeads.InsuredName = insurerName;
                        repository.UpdateInsurerName(objLeads);
                        //save carrier id in Lead policy
                        policyId = repository.GetLeadPolicyId(claimID);
                        objLeadPolicy = new Data.Entities.LeadPolicy();
                        objLeadPolicy.Id = policyId;
                        objLeadPolicy.CarrierID = carrierID;
                        repository.UpdateCarrierId(objLeadPolicy);

                        //add expense
                        ClaimExpenseManager objClaimExpenseManager = new ClaimExpenseManager();
                        claimExpense = new ClaimExpense();
                        claimExpense.ClaimID = claimID;
                        if (!string.IsNullOrEmpty(expenseAmount))
                        {
                            claimExpense.ExpenseAmount = Convert.ToDecimal(expenseAmount);
                        }
                        if (!string.IsNullOrEmpty(expenseDate))
                        {
                            claimExpense.ExpenseDate = Convert.ToDateTime(expenseDate);
                        }
                        claimExpense.ExpenseDescription = commentNote.Trim();
                        claimExpense.ExpenseTypeID = Convert.ToInt32(expenseType);
                        if (reimbrance == "1")
                        {
                            claimExpense.IsReimbursable = true;
                        }
                        else
                        {
                            claimExpense.IsReimbursable = false;
                        }

                        claimExpense.UserID = userID;
                        claimExpense.AdjusterID = Convert.ToInt32(claimAdjusterId);
                        if (!string.IsNullOrEmpty(expenseQty))
                        {
                            claimExpense.ExpenseQty = Convert.ToDecimal(expenseQty);
                        }
                        objClaimExpenseManager.Save(claimExpense);

                        //claim comment for add notes                         			
                        comment = new ClaimComment();
                        comment.ClaimID = claimID;
                        comment.IsActive = true;
                        comment.UserId = Core.SessionHelper.getUserId();
                        comment.CommentDate = DateTime.Now;
                        comment.ActivityType = "Add Expense";
                        comment.CommentText = string.Format("Expense: {0}, Description: {1}, Date: {2:MM/dd/yyyy}, Amount: {3:N2}, Adjuster: {4} Qty: {5:N2}",
                                                 expenseTypeName,
                                                 commentNote.Trim(),
                                                 Convert.ToDateTime(expenseDate),
                                                 expenseAmount,
                                                 claimAdjuster,
                                                 expenseQty
                                                 );
                        ClaimCommentManager.Save(comment);

                    }
                    scope.Complete();
                }
                string[] recipId = recipientId.Split(',');
                string recipientEmailId = string.Empty;

                string[] IdofTable = idOf.Split(',');
                int index2 = 0;
                for (int index = 0; index < recipId.Length; index++)
                {
                    index2 = 0;
                    int.TryParse(recipId[index], out index2);
                    if (IdofTable[index] == "c")
                    {

                        Contact objContact = ContactManager.Get(index2);
                        if (!string.IsNullOrEmpty(objContact.Email))
                        {
                            if (recipientEmailId == "")
                            {
                                recipientEmailId = objContact.Email;
                            }
                            else
                            {
                                recipientEmailId = recipientEmailId + "," + objContact.Email;
                            }
                        }
                    }
                    else
                    {

                        AdjusterMaster objAdjuster = AdjusterManager.GetAdjusterId(index2);

                        if (!string.IsNullOrEmpty(objAdjuster.email))
                        {
                            if (recipientEmailId == "")
                            {
                                recipientEmailId = objAdjuster.email;
                            }
                            else
                            {
                                recipientEmailId = recipientEmailId + "," + objAdjuster.email;
                            }
                        }

                    }
                }
                SendExpenseEmail(expenseType, insurerClaimId, insurerName, claimAdjusterId, adjusterComapnyName, updatedby, commentNote, "", carrierID, claimID, recipientEmailId, claimAdjuster, expenseTypeName, carrier, recipId, IdofTable, expenseQty, expenseAmount, expenseDate, reimbrance);
                json = "Expense add successfully";

            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

            return json;
        }



        public static void SendExpenseEmail(string expenseType, string insurerClaimId, string insurerName, int claimAdjusterId, string adjusterComapnyName, string updatedby, string commentNote, string emailTo, int carrierID, int claimID, string recipient, string claimAdjuster, string expenseTypeName, string carrier, string[] recipId, string[] IdofTable, string expenseQty, string expenseAmount, string expenseDate, string reimbrance)
        {
            StringBuilder emailBody = new StringBuilder();
            string password = null;
            string[] recipients = null;
            string smtpHost = null;
            int smtpPort = 0;
            //string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
            string subject = "Claim Expense add to: " + expenseTypeName + " on Claim # " + insurerClaimId;
            CRM.Data.Entities.SecUser user = null;

            // get logged in user
            int userID = SessionHelper.getUserId();
            // get logged in user email info
            user = SecUserManager.GetByUserId(userID);
            // load email credentials
            smtpHost = user.emailHost;
            string appurl = ConfigurationManager.AppSettings["appURL"].ToString();

            int.TryParse(user.emailHostPort, out smtpPort);

            recipient = emailTo != string.Empty && recipient != string.Empty ? emailTo + "," + recipient : recipient;
            recipient = emailTo != string.Empty && recipient == string.Empty ? emailTo : recipient;

            // recipients
            recipients = new string[] { };
            recipients = recipient.Split(',');
            // build email body
            // .containerBox
            emailBody.Append("<div>");
            emailBody.Append("<div>");
            emailBody.Append("Hi " + claimAdjuster + ",<br><br>");

            emailBody.Append("<table>");
            emailBody.Append("<tr><td style='width:100px;'></td>");
            emailBody.Append("<td>");
            emailBody.Append("<b>Add Claim Expense</b><br/>");
            emailBody.Append("</td>");
            emailBody.Append("</tr>");
            emailBody.Append("</table>");

            emailBody.Append("<table>");

            emailBody.Append("<tr>");
            emailBody.Append("<td style='width:200px;'>");
            emailBody.Append("Expense Type:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(expenseTypeName);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Insurer Claim #:");
            emailBody.Append("</td>");
            emailBody.Append("<td><a href='" + appurl + "/protected/Admin/UsersLeads.aspx?s=" + insurerClaimId + "'>");
            emailBody.Append(insurerClaimId);
            emailBody.Append("</a></td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Insured Name:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(insurerName);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Carrier:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(carrier);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Adjuster:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(claimAdjuster);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Adjuster Company Name:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(adjusterComapnyName);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");


            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Updated By:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(user.UserName);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Expense Date:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(expenseDate);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Description:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(commentNote);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Reimburse:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            if (reimbrance == "1")
            {
                emailBody.Append("True");
            }
            else
            {
                emailBody.Append("False");
            }
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Quantity:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(expenseQty);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append("OR");
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            emailBody.Append("<tr>");
            emailBody.Append("<td>");
            emailBody.Append("Amount:");
            emailBody.Append("</td>");
            emailBody.Append("<td>");
            emailBody.Append(expenseAmount);
            emailBody.Append("</td>");
            emailBody.Append("</tr>");

            if (recipId.Length > 0)
            {
                emailBody.Append("<tr>");
                emailBody.Append("<td style='vertical-align:top'>");
                emailBody.Append("Selected Recipients");
                emailBody.Append("</td>");
                emailBody.Append("<td>");

                //////

                emailBody.Append("<table style='border:1px solid;'>");
                emailBody.Append("<tr>");
                emailBody.Append("<td>");
                emailBody.Append("First Name");
                emailBody.Append("</td>");
                emailBody.Append("<td>");
                emailBody.Append("Last Name");
                emailBody.Append("</td>");
                emailBody.Append("<td>");
                emailBody.Append("Company Name");
                emailBody.Append("</td>");
                emailBody.Append("<td>");
                emailBody.Append("Email");
                emailBody.Append("</td>");
                emailBody.Append("<td>");
                emailBody.Append("Contact Title");
                emailBody.Append("</td>");
                emailBody.Append("</tr>");
                int index2 = 0;
                for (int index = 0; index < recipId.Length; index++)
                {
                    index2 = 0;
                    int.TryParse(recipId[index], out index2);
                    if (IdofTable[index] == "c")
                    {

                        Contact objContact = ContactManager.Get(index2);
                        emailBody.Append("<tr>");

                        emailBody.Append("<td>");
                        emailBody.Append(objContact.FirstName);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objContact.LastName);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objContact.CompanyName);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objContact.Email);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objContact.ContactTitle);
                        emailBody.Append("</td>");
                        emailBody.Append("</tr>");

                    }
                    else
                    {
                        AdjusterMaster objAdjuster = AdjusterManager.GetAdjusterId(index2);

                        emailBody.Append("<tr>");

                        emailBody.Append("<td>");
                        emailBody.Append(objAdjuster.FirstName);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objAdjuster.LastName);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objAdjuster.CompanyName);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append(objAdjuster.email);
                        emailBody.Append("</td>");
                        emailBody.Append("<td>");
                        emailBody.Append("");
                        emailBody.Append("</td>");
                        emailBody.Append("</tr>");
                    }
                }


                emailBody.Append("</table>");
                emailBody.Append("</td>");
                emailBody.Append("</tr>");
            }

            emailBody.Append("</table>");
            emailBody.Append("</div>");	// paneContentInner
            emailBody.Append("</div>");	// containerBox

            password = Core.SecurityManager.Decrypt(user.emailPassword);

            // Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, true);//user.isSSL ??
            string crsupportEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
            string crsupportEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
            Core.EmailHelper.sendEmail(crsupportEmail, recipients, null, subject, emailBody.ToString(), null, crsupportEmail, crsupportEmailPassword);


        }

        [System.Web.Services.WebMethod]
        public static string[] GetCarrierInvoiceProfileData(int claimID)
        {
            string[] json = new string[6];
            Claim objclaim = null;
            int userID = SessionHelper.getUserId();

            try
            {
                objclaim = ClaimsManager.Get(claimID);
                if (objclaim != null)
                {
                    json[0] = objclaim.GrossLossPayable.ToString();
                    json[1] = objclaim.Depreciation.ToString();
                    json[2] = objclaim.Deductible.ToString();
                    json[3] = objclaim.NetClaimPayable.ToString();
                    json[4] = objclaim.FeeInvoiceDesignation.ToString();
                    json[5] = objclaim.CarrierInvoiceProfileID.ToString();
                }

            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

            return json;
        }

        [System.Web.Services.WebMethod]
        public static string GetCarrierInvoiceProfile(int claimID)
        {
            string[] json = new string[6];
            Claim objclaim = null;
            int userID = SessionHelper.getUserId();
            int carrierId = 0;
            //CarrierInvoiceProfile objCarrierInvoiceProfile = new CarrierInvoiceProfile();
            List<CarrierInvoiceProfile> listinvoiceProfiles = null;
            List<InvoiceProfile> objlist = new List<InvoiceProfile>();

            try
            {
                objclaim = ClaimsManager.Get(claimID);
                if (objclaim != null)
                {
                    carrierId = objclaim.LeadPolicy.CarrierID ?? 0;
                    listinvoiceProfiles = CarrierInvoiceProfileManager.GetAll(carrierId);
                    foreach (var item in listinvoiceProfiles)
                    {

                        InvoiceProfile objprofile = new InvoiceProfile();
                        objprofile.ID = item.CarrierInvoiceProfileID.ToString();
                        objprofile.Value1 = item.ProfileName;
                        objlist.Add(objprofile);
                    }

                }

            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

            JavaScriptSerializer jscript = new JavaScriptSerializer();


            string myJsonString = jscript.Serialize(objlist);
            return myJsonString;
        }
        /// <summary>
        /// class for Serialize data
        /// </summary>
        public class InvoiceProfile
        {

            public string ID { get; set; }
            public string Value1 { get; set; }

        }



        [System.Web.Services.WebMethod]
        public static string AutoInvoiceGenerate(int claimID, string carrierInvoiceProfile, string feeDesignation, string grossLossPayable, string depreciation, string policyDeductible, string netClaimPayable)
        {
            string json = "";
            Claim objclaim = null;
            int userID = SessionHelper.getUserId();

            try
            {
                grossLossPayable = grossLossPayable.Replace('$', ' ').Trim();
                depreciation = depreciation.Replace('$', ' ').Trim();
                policyDeductible = policyDeductible.Replace('$', ' ').Trim();
                netClaimPayable = netClaimPayable.Replace('$', ' ').Trim();

                AjaxClientServices objAjaxClientServices = new AjaxClientServices();
                objAjaxClientServices.ClaimIDNew = claimID;
                objAjaxClientServices.CarrierInvoiceProfile = carrierInvoiceProfile;
                objAjaxClientServices.FeeDesignation = feeDesignation;
                objAjaxClientServices.GrossLossPayable = Convert.ToDecimal(grossLossPayable);
                objAjaxClientServices.Depreciation = Convert.ToDecimal(depreciation);
                objAjaxClientServices.PolicyDeductible = Convert.ToDecimal(policyDeductible);
                objAjaxClientServices.NetClaimPayable = Convert.ToDecimal(netClaimPayable);
                json = objAjaxClientServices.generateInvoice();

            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

            return json;
        }



        #region AutoInvoicesFunction

        public string generateInvoice()
        {
            int clientID = SessionHelper.getClientId();
            Client client = null;
            int feeInvoiceDesignationID = 0;
            string message = string.Empty;

            client = ClientManager.GetByID(clientID);
            feeInvoiceDesignationID = Convert.ToInt32(FeeDesignation);

            switch (client.InvoiceSettingID)
            {
                case 1:
                    // independent adjuster
                    message = generateAutomaticInvoiceIndependentAdjuster(client);
                    break;

                case 2:
                    // public adjuster 
                    if (feeInvoiceDesignationID == (int)Globals.FeeInvoiceDesignation.LossPercentageFee)
                        message = generateAutomaticInvoicePublicAdjuster(client);

                    break;

                default:
                    // no auto invoice method selected
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "invoiceAlert", "automaticInvoiceMethodSelectionAlert();", true);
                    break;
            }
            return message;
        }

        private string generateAutomaticInvoicePublicAdjuster(Client client)
        {
            Claim claim = null;
            int claimID = 0;
            int clientID = Core.SessionHelper.getClientId();
            int days = 0;
            int invoiceID = 0;
            Invoice invoice = null;
            InvoiceDetail invoiceDetail = null;
            Leads lead = null;
            CRM.Data.Entities.LeadPolicy policy = null;
            int nextInvoiceNumber = 0;
            decimal totalAmount = 0;
            string message = string.Empty;

            // claimID = SessionHelper.getClaimID();
            claimID = ClaimIDNew;

            claim = ClaimsManager.Get(claimID);

            if (claim != null && claim.LeadPolicy != null && claim.LeadPolicy.Leads != null)
            {
                lead = claim.LeadPolicy.Leads;
                policy = claim.LeadPolicy;

                invoice = new Invoice();
                days = client.InvoicePaymentTerms ?? 0;

                totalAmount = NetClaimPayable * (client.InvoiceContingencyFee ?? 0);

                // invoice
                invoice.InvoiceDate = DateTime.Now;
                invoice.DueDate = DateTime.Now.AddDays(days);

                invoice.BillToName = lead.insuredName;
                invoice.BillToAddress1 = lead.MailingAddress ?? "";
                invoice.BillToAddress2 = lead.MailingAddress2 ?? "";
                invoice.BillToAddress3 = string.Format("{0}, {1} {2}", lead.MailingCity ?? "", lead.MailingState, lead.MailingZip);

                invoice.ClaimID = claim.ClaimID;
                invoice.IsVoid = false;
                invoice.IsApprove = false;

                invoice.TotalAmount = totalAmount;

                // invoice detail
                invoiceDetail = new InvoiceDetail();
                invoiceDetail.LineDate = DateTime.Now;
                invoiceDetail.LineDescription = "Contingency Fee";
                invoiceDetail.Total = totalAmount;
                invoiceDetail.LineAmount = totalAmount;
                invoiceDetail.isBillable = true;
                invoiceDetail.Qty = NetClaimPayable;
                invoiceDetail.Rate = client.InvoiceContingencyFee * 100;

                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        // assign next invoice number to new invoice
                        nextInvoiceNumber = InvoiceManager.GetNextInvoiceNumber(client.ClientId);

                        invoice.InvoiceNumber = nextInvoiceNumber;

                        invoiceID = InvoiceManager.Save(invoice);

                        invoiceDetail.InvoiceID = invoiceID;

                        InvoiceDetailManager.Save(invoiceDetail);

                        // update invoice ready flag 
                        //claim.IsInvoiceReady = cbxInvoiceReady.Checked;
                        claim.IsInvoiceReady = true;

                        claim.IsInvoiced = true;

                        ClaimsManager.Save(claim);

                        // 2014-05-02 apply rule
                        using (SpecificExpenseTypePerCarrier ruleEngine = new SpecificExpenseTypePerCarrier())
                        {
                            RuleException ruleException = ruleEngine.TestRule(clientID, invoice);

                            if (ruleException != null)
                            {
                                ruleException.UserID = Core.SessionHelper.getUserId();
                                ruleEngine.AddException(ruleException);
                                CheckSendMail(ruleException);
                            }
                        }

                        // commit transaction
                        scope.Complete();

                        // lblMessage.Text = "Invoice has been generated successfully.";
                        // lblMessage.CssClass = "ok";
                        message = "Invoice has been generated successfully.";
                    }
                }
                catch (Exception ex)
                {
                    Core.EmailHelper.emailError(ex);

                    // lblMessage.Text = "Invoice was not generated.";
                    // lblMessage.CssClass = "error";
                    message = "Invoice was not generated.";
                }
            }
            return message;
        }

        private string generateAutomaticInvoiceIndependentAdjuster(Client client)
        {
            Carrier carrier = null;
            CarrierInvoiceProfile invoiceProfile = null;
            string cityName = null;
            Claim claim = null;
            int claimID = 0;
            int clientID = SessionHelper.getClientId();
            decimal commissionAmount = 0;
            int days = 0;
            int invoiceID = 0;
            int quantity = 1;
            Invoice invoice = null;
            Leads lead = null;
            CRM.Data.Entities.LeadPolicy policy = null;
            int nextInvoiceNumber = 0;
            decimal claimAmount = 0;
            decimal rate = 0;
            string stateName = null;
            string message = string.Empty;

            // claimID = SessionHelper.getClaimID();
            claimID = ClaimIDNew;
            claim = ClaimsManager.Get(claimID);

            if (claim != null && claim.LeadPolicy != null && claim.LeadPolicy.Leads != null && claim.LeadPolicy.Carrier != null)
            {
                lead = claim.LeadPolicy.Leads;
                policy = claim.LeadPolicy;
                carrier = CarrierManager.GetByID((int)policy.CarrierID);

                // load carrier invoice profile assigned to claim
                invoiceProfile = CarrierInvoiceProfileManager.GetProfileForInvoicing(claim.CarrierInvoiceProfileID ?? 0);

                // exit if no profile found
                if (invoiceProfile == null)
                {
                    // lblMessage.Text = "Invoice was not generated because no 'Invoice Profile' assigned to claim.";
                    // lblMessage.CssClass = "error";
                    message = "Invoice was not generated because no 'Invoice Profile' assigned to claim.";
                    return message;
                }

                invoice = new Invoice();
                days = client.InvoicePaymentTerms ?? 0;

                // create invoice
                invoice.InvoiceDate = DateTime.Now;
                invoice.DueDate = DateTime.Now.AddDays(days);

                // bill claim carrier
                invoice.BillToName = carrier.CarrierName;
                invoice.BillToAddress1 = carrier.AddressLine1 ?? "";
                invoice.BillToAddress2 = carrier.AddressLine2 ?? "";

                cityName = carrier.CityMaster != null ? carrier.CityMaster.CityName : string.Empty;
                stateName = carrier.StateMaster != null ? carrier.StateMaster.StateName : string.Empty;

                invoice.BillToAddress3 = string.Format("{0}, {1} {2}", cityName, stateName, carrier.ZipCode ?? "");

                invoice.ClaimID = claim.ClaimID;
                invoice.IsVoid = false;
                invoice.IsApprove = false;

                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        // assign next invoice number to new invoice
                        nextInvoiceNumber = InvoiceManager.GetNextInvoiceNumber(client.ClientId);

                        invoice.InvoiceNumber = nextInvoiceNumber;

                        // create invoice in db
                        invoiceID = InvoiceManager.Save(invoice);

                        if (claim.FeeInvoiceDesignation != (int)Globals.FeeInvoiceDesignation.TimeAndExpenseOnly)
                        {
                            claimAmount = getClaimAmount(invoiceProfile);

                            calculateCommissionPerCarrierInvoiceProfile(invoiceProfile, out rate, out commissionAmount);

                            //insertDetailLine(invoiceID, "Adjuster Service Fee", commissionAmount, claimAmount, rate);
                            //insertDetailLine(invoiceID, "Adjuster Service Fee", claimAmount, rate);
                            if (FeeDesignation == "1" || FeeDesignation == "2")
                            {
                                claimAmount = 1;
                            }
                            insertDetailLine(invoiceID, "Adjuster Service Fee", claimAmount, rate);
                        }

                        //processCarrierInvoiceProfileFeeProvisions(invoiceProfile, invoiceID);

                        processCarrierInvoiceProfileTimeExpense(claim.ClaimID, invoiceProfile, invoiceID, claimAmount);

                        processFirmDiscount(invoiceProfile, invoiceID);

                        // update invoice total
                        invoice.TotalAmount = InvoiceManager.calculateInvoiceTotal(invoiceID);

                        InvoiceManager.Save(invoice);


                        // update claim - invoice ready flag 
                        claim.IsInvoiceReady = true;

                        claim.IsInvoiced = true;

                        ClaimsManager.Save(claim);

                        // 2014-05-02 apply rule
                        using (SpecificExpenseTypePerCarrier ruleEngine = new SpecificExpenseTypePerCarrier())
                        {
                            RuleException ruleException = ruleEngine.TestRule(clientID, invoice);

                            if (ruleException != null)
                            {
                                ruleException.UserID = Core.SessionHelper.getUserId();
                                ruleEngine.AddException(ruleException);
                                CheckSendMail(ruleException);
                            }
                        }

                        // complete transaction
                        scope.Complete();

                        // lblMessage.Text = "Invoice has been generated successfully.";
                        // lblMessage.CssClass = "ok";
                        message = "Invoice has been generated successfully.";
                    }
                }
                catch (Exception ex)
                {
                    Core.EmailHelper.emailError(ex);

                    // lblMessage.Text = "Invoice was not generated.";
                    // lblMessage.CssClass = "error";
                    message = "Invoice was not generated.";

                }
            }
            return message;
        }

        private decimal getClaimAmount(CarrierInvoiceProfile invoiceProfile)
        {
            decimal premiumAmount = 0;

            switch (invoiceProfile.InvoiceType ?? 0)
            {
                case (int)Globals.InvoiceType.NetClaimPayable:
                    premiumAmount = NetClaimPayable;
                    break;

                case (int)Globals.InvoiceType.GrossClaimPayable:
                    premiumAmount = GrossLossPayable;
                    break;

                default:
                    break;
            }

            return premiumAmount;
        }

        private void calculateCommissionPerCarrierInvoiceProfile(CarrierInvoiceProfile invoiceProfile, out decimal rate, out decimal commissionAmount)
        {
            decimal claimAmount = 0;
            commissionAmount = 0;
            rate = 0;

            if (invoiceProfile != null && invoiceProfile.CarrierInvoiceProfileFeeSchedule != null && invoiceProfile.CarrierInvoiceProfileFeeSchedule.Count > 0)
            {

                claimAmount = getClaimAmount(invoiceProfile);


                foreach (CarrierInvoiceProfileFeeSchedule feeSchedule in invoiceProfile.CarrierInvoiceProfileFeeSchedule)
                {
                    if (claimAmount >= feeSchedule.RangeAmountFrom && claimAmount <= feeSchedule.RangeAmountTo)
                    {
                        if (feeSchedule.FlatFee > 0)
                        {
                            commissionAmount = feeSchedule.FlatFee;
                            rate = feeSchedule.FlatFee;
                        }
                        else if (feeSchedule.PercentFee > 0)
                        {
                            commissionAmount = claimAmount * feeSchedule.PercentFee;
                            rate = feeSchedule.PercentFee * 100;
                        }
                        else if (feeSchedule.MinimumFee > 0)
                        {
                            commissionAmount = feeSchedule.MinimumFee;
                            rate = feeSchedule.MinimumFee;
                        }
                    }
                }
            }

        }

        private void insertDetailLine(int invoiceID, string serviceDescription, decimal qty, decimal rate, DateTime? date = null, string comments = null)
        {
            InvoiceDetail invoiceDetail = null;

            // invoice detail
            invoiceDetail = new InvoiceDetail();
            invoiceDetail.InvoiceID = invoiceID;

            invoiceDetail.LineDate = date == null ? DateTime.Now : date;

            invoiceDetail.LineDescription = serviceDescription;
            invoiceDetail.Comments = comments;
            invoiceDetail.LineAmount = qty * rate;
            invoiceDetail.Total = invoiceDetail.LineAmount;
            invoiceDetail.isBillable = true;
            invoiceDetail.Qty = qty;
            invoiceDetail.Rate = rate;

            InvoiceDetailManager.Save(invoiceDetail);
        }
        private void insertDetailLine(int invoiceID, string serviceDescription, decimal amount, decimal qty, decimal rate, DateTime? date = null, string comments = null)
        {
            InvoiceDetail invoiceDetail = null;

            // invoice detail
            invoiceDetail = new InvoiceDetail();
            invoiceDetail.InvoiceID = invoiceID;

            invoiceDetail.LineDate = date == null ? DateTime.Now : date;

            invoiceDetail.LineDescription = serviceDescription;
            invoiceDetail.Comments = comments;
            invoiceDetail.Total = amount;
            invoiceDetail.LineAmount = amount;
            invoiceDetail.isBillable = true;
            invoiceDetail.Qty = qty;
            invoiceDetail.Rate = rate;

            InvoiceDetailManager.Save(invoiceDetail);
        }

        private void processCarrierInvoiceProfileTimeExpense(int claimID, CarrierInvoiceProfile invoiceProfile, int invoiceID, decimal premiumAmount)
        {
            if (invoiceProfile != null && invoiceProfile.CarrierInvoiceProfileFeeItemized != null && invoiceProfile.CarrierInvoiceProfileFeeItemized.Count > 0)
            {

                processClaimServices(claimID, invoiceProfile, invoiceID);

                processClaimExpenses(claimID, invoiceProfile, invoiceID);

                //foreach (CarrierInvoiceProfileFeeItemized itemizedFee in invoiceProfile.CarrierInvoiceProfileFeeItemizeds) {

                //	if (itemizedFee.ItemRate > 0) {
                //		feeAmount = itemizedFee.ItemRate;
                //		insertDetailLine(invoiceID, itemizedFee.ItemDescription, feeAmount, 1, itemizedFee.ItemRate);
                //	}
                //	else if (itemizedFee.ItemPercentage > 0) {
                //		feeAmount = premiumAmount * itemizedFee.ItemRate;
                //		insertDetailLine(invoiceID, itemizedFee.ItemDescription, feeAmount, premiumAmount, feeAmount);
                //	}
                //}
            }
        }

        private void processFirmDiscount(CarrierInvoiceProfile invoiceProfile, int invoiceID)
        {
            decimal discountRate = 0;
            decimal discountAmount = 0;
            decimal invoiceTotalAmount = 0;
            string serviceDescription = null;
            decimal percentage = 0;

            discountRate = invoiceProfile.FirmDiscountPercentage ?? 0;

            if (discountRate > 0)
            {

                invoiceTotalAmount = InvoiceManager.calculateInvoiceTotal(invoiceID);

                discountAmount = (invoiceTotalAmount * discountRate) * -1;
                percentage = discountRate * 100;
                serviceDescription = string.Format("Firm Discount Percentage", percentage);

                insertDetailLine(invoiceID, serviceDescription, discountAmount, 1, percentage);
            }
        }

        private void processClaimServices(int claimID, CarrierInvoiceProfile invoiceProfile, int invoiceID)
        {
            CarrierInvoiceProfileFeeItemized profileTEFee = null;
            List<ClaimService> claimServices = null;
            decimal lineTotal = 0;
            decimal quantity = 0;
            decimal rateAmount = 0;
            string serviceDescription = null;
            string serviceComments = null;
            InvoiceServiceType invoiceServiceType = null;

            // get TE services for claim entered by adjuster
            using (ClaimServiceManager repositiory = new ClaimServiceManager())
            {
                claimServices = repositiory.GetAll(claimID);
            }

            if (claimServices != null && claimServices.Count > 0)
            {
                foreach (ClaimService claimService in claimServices)
                {
                    profileTEFee = (from x in invoiceProfile.CarrierInvoiceProfileFeeItemized
                                    where x.ServiceTypeID == claimService.ServiceTypeID
                                    select x
                                 ).FirstOrDefault();

                    quantity = (claimService.ServiceQty ?? 0);
                    serviceDescription = claimService.InvoiceServiceType == null ? string.Empty : claimService.InvoiceServiceType.ServiceDescription;
                    invoiceServiceType = claimService.InvoiceServiceType;

                    if (profileTEFee != null)
                    {
                        // use override from invoice profile											
                        if (profileTEFee.ItemRate > 0)
                        {
                            serviceComments = profileTEFee.ItemDescription;
                            rateAmount = profileTEFee.ItemRate;

                            lineTotal = rateAmount * quantity;

                            insertDetailLine(invoiceID, serviceDescription, lineTotal, quantity, rateAmount, claimService.ServiceDate, serviceComments);
                        }
                        else if (profileTEFee.ItemPercentage > 0)
                        {
                        }
                    }
                    else
                    {
                        if ((invoiceServiceType.ServiceRate ?? 0) > 0)
                        {
                            rateAmount = invoiceServiceType.ServiceRate ?? 0;
                            quantity = claimService.ServiceQty ?? 0;
                            lineTotal = rateAmount * quantity;
                        }
                        else if ((invoiceServiceType.ServicePercentage ?? 0) > 0)
                        {
                        }

                        insertDetailLine(invoiceID, serviceDescription, lineTotal, quantity, rateAmount, claimService.ServiceDate, serviceComments);
                    }
                }
            }
        }

        private void processClaimExpenses(int claimID, CarrierInvoiceProfile invoiceProfile, int invoiceID)
        {
            CarrierInvoiceProfileFeeItemized profileTEFee = null;
            List<ClaimExpense> claimExpenses = null;
            decimal lineTotal = 0;
            decimal quantity = 0;
            decimal operand = 0;
            decimal expenseAmount = 0;
            decimal rateAmount = 0;
            string serviceDescription = null;
            string serviceComments = null;

            // get TE services for claim entered by adjuster
            using (ClaimExpenseManager repositiory = new ClaimExpenseManager())
            {
                claimExpenses = repositiory.GetExpenseForInvoice(claimID);
            }

            if (claimExpenses != null && claimExpenses.Count > 0)
            {
                foreach (ClaimExpense claimExpense in claimExpenses)
                {
                    rateAmount = 0;

                    profileTEFee = (from x in invoiceProfile.CarrierInvoiceProfileFeeItemized
                                    where x.ExpenseTypeID == claimExpense.ExpenseTypeID
                                    select x
                                 ).FirstOrDefault();


                    if (profileTEFee != null)
                    {
                        // use override from invoice profile
                        serviceDescription = profileTEFee.ExpenseType == null ? string.Empty : profileTEFee.ExpenseType.ExpenseDescription;
                        serviceComments = profileTEFee.ItemDescription;
                        operand = profileTEFee.LogicalOperatorOperand ?? 0;



                        if ((profileTEFee.LogicalOperator ?? 0) > 0 && operand > 0)
                        {
                            quantity = claimExpense.ExpenseQty ?? 0;
                            expenseAmount = claimExpense.ExpenseAmount;

                            switch (profileTEFee.LogicalOperator)
                            {
                                case 1:		// =
                                    //if (expenseAmount > 0 && expenseAmount == operand) {
                                    //	rateAmount = profileTEFee.ItemRate;
                                    //	expenseAmount = quantity * rateAmount;										
                                    //}
                                    //else if (quantity > 0 && quantity == operand) {
                                    //	rateAmount = profileTEFee.ItemRate;
                                    //	expenseAmount = quantity * rateAmount;
                                    //}
                                    break;

                                case 2:		// <									
                                case 3:		// <=
                                    if (expenseAmount > 0 && expenseAmount <= operand)
                                    {
                                        quantity = 1;
                                        rateAmount = profileTEFee.ItemRate;
                                    }
                                    else if (quantity > 0 && quantity <= operand)
                                    {
                                        rateAmount = profileTEFee.ItemRate;
                                    }
                                    break;

                                case 4:		// >									
                                case 5:		// >=
                                    if (expenseAmount > 0 && expenseAmount >= operand)
                                    {
                                        quantity = 1;
                                        rateAmount = operand;
                                    }
                                    else if (quantity > 0 && quantity >= operand)
                                    {
                                        quantity = quantity - operand;
                                        rateAmount = profileTEFee.ItemRate;
                                    }
                                    break;

                                default:
                                    quantity = 1;
                                    rateAmount = claimExpense.ExpenseAmount;
                                    break;
                            }

                            insertDetailLine(invoiceID, serviceDescription, quantity, rateAmount, claimExpense.ExpenseDate, serviceComments);
                        }
                        else if (profileTEFee.ItemRate > 0)
                        {
                            // no condition specified
                            quantity = claimExpense.ExpenseQty ?? 1;

                            rateAmount = profileTEFee.ItemRate;

                            lineTotal = expenseAmount;

                            insertDetailLine(invoiceID, serviceDescription, quantity, rateAmount, claimExpense.ExpenseDate, serviceComments);
                        }
                        else if (profileTEFee.ItemPercentage > 0)
                        {
                        }
                    }
                    else
                    {
                        // no override found
                        serviceDescription = claimExpense.ExpenseType.ExpenseDescription;

                        serviceComments = claimExpense.ExpenseDescription;

                        quantity = claimExpense.ExpenseQty ?? 1;

                        rateAmount = claimExpense.ExpenseAmount;

                        insertDetailLine(invoiceID, serviceDescription, quantity, rateAmount, claimExpense.ExpenseDate, serviceComments);
                    }
                }
            }
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
                            notifyUser2(objRule.Description, claimId, recipient);
                        }
                        else if (sendAdjuster == false && sendSupervisor == true)
                        {
                            recipient = supervisorEmail;
                            notifyUser2(objRule.Description, claimId, recipient);
                        }
                        else if (sendAdjuster == true && sendSupervisor == false)
                        {

                            recipient = adjusterEmail;
                            notifyUser2(objRule.Description, claimId, recipient);
                        }
                    }
                }
            }

        }

        public static void notifyUser2(string description, int claimid, string recipient)
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

            //Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, true);//user.isSSL ??


            string crsupportEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
            string crsupportEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
            Core.EmailHelper.sendEmail(crsupportEmail, recipients, null, subject, emailBody.ToString(), null, crsupportEmail, crsupportEmailPassword);
        }





        #endregion

        #endregion


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void CreateClaimIdSession(int claimID)
        {
            try
            {
                HttpContext.Current.Session["ClaimID"] = claimID;
            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }


        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static bool GetTutorialMode()
        {
            try
            {
                int userId = SessionHelper.getUserId();

                return SecUserManager.GetTutorialMode(userId);

            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

            return false;
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void SetTutorialMode(bool mode)
        {
            try
            {
                int userId = SessionHelper.getUserId();

                SecUserManager.SetTutorialMode(userId, mode);
            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }
        }

        [System.Web.Services.WebMethod]
        public static string SaveLossDetails(int policyID, string coverage, string type, string policyLimit, string deductible, string applyTo, string itv, string reserve, int acrossall, string catDeductible, string coInsuranceLimit)
        {
            string json = "";
            int limitID = 0;

            ClaimLimit claimLimit = null;
            PolicyLimit objPolicyLimit = null;

            Limit limits = null;
            Limit limits2 = null;
            int userID = SessionHelper.getUserId();
            ClaimManager objClaimManager = new ClaimManager();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    List<Claim> lstClaim = objClaimManager.GetPolicyClaim(policyID);

                    bool isTemplate = ClaimLimitManager.PolicyIsStaticTrue(policyID);

                    if (isTemplate)
                    {

                        PolicyLimitManager.IsDeleted(policyID);
                        //if coverage first time then delete all template data from all claimid
                        foreach (var claim in lstClaim)
                        {
                            int claimId = claim.ClaimID;
                            ClaimLimitManager.IsDeleted(claimId);
                        }

                    }


                    limits = new Limit();
                    limits.LimitLetter = coverage;
                    if (type == "Contacts" || type == "Personal Liability" || type == "Medical Payments")
                    {
                        limits.LimitType = 2;
                    }
                    else
                    {
                        limits.LimitType = 1;
                    }
                    limits.LimitDescription = type;
                    limits.IsStatic = false;
                    limits2 = LimitManager.Save(limits);

                    limitID = limits2.LimitID;

                    objPolicyLimit = new PolicyLimit();
                    objPolicyLimit.PolicyID = policyID;
                    objPolicyLimit.LimitID = limitID;
                    if (!string.IsNullOrEmpty(policyLimit))
                    {
                        objPolicyLimit.LimitAmount = Convert.ToDecimal(policyLimit);
                    }
                    else
                    {
                        objPolicyLimit.LimitAmount = 0;
                    }
                    if (!string.IsNullOrEmpty(deductible))
                    {
                        objPolicyLimit.LimitDeductible = Convert.ToDecimal(deductible);
                    }
                    else
                    {
                        objPolicyLimit.LimitDeductible = 0;
                    }
                    //new fields add
                    if (!string.IsNullOrEmpty(catDeductible))
                    {
                        objPolicyLimit.CATDeductible = catDeductible;
                    }
                    if (!string.IsNullOrEmpty(coInsuranceLimit))
                    {
                        objPolicyLimit.ConInsuranceLimit = Convert.ToDecimal(coInsuranceLimit);
                    }
                    else
                    {
                        objPolicyLimit.LimitDeductible = 0;
                    }

                    if (!string.IsNullOrEmpty(itv))
                    {
                        objPolicyLimit.ITV = Convert.ToDecimal(itv);
                    }
                    else
                    {
                        objPolicyLimit.ITV = 0;
                    }
                    if (!string.IsNullOrEmpty(reserve))
                    {
                        objPolicyLimit.Reserve = Convert.ToDecimal(reserve);
                    }
                    else
                    {
                        objPolicyLimit.Reserve = 0;
                    }
                    objPolicyLimit.IsDeleted = false;
                    if (acrossall == 1)
                    {
                        objPolicyLimit.ApplyAcrossAllCoverage = true;
                    }
                    else
                    {
                        objPolicyLimit.ApplyAcrossAllCoverage = false;
                    }
                    objPolicyLimit.ApplyTo = applyTo;

                    PolicyLimitManager.Save(objPolicyLimit);



                    // enter data for each claim
                    foreach (var claim in lstClaim)
                    {

                        claimLimit = new ClaimLimit();
                        claimLimit.ClaimID = claim.ClaimID;
                        claimLimit.LimitID = limitID;
                        claimLimit.LossAmountACV = 0;
                        claimLimit.LossAmountRCV = 0;
                        claimLimit.Depreciation = 0;
                        claimLimit.OverageAmount = 0;
                        claimLimit.NonRecoverableDepreciation = 0;
                        claimLimit.IsDeleted = false;
                        ClaimLimitManager.Save(claimLimit);


                    }


                    scope.Complete();
                }
                json = "Loss details successfully add";

            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

            return json;
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string SaveLossDetailsAdd(int policyID, string coverage, string type, string policyLimit, string deductible, string applyTo, string itv, string reserve, int acrossall, string catDeductible, string coInsuranceLimit)
        {
            string json = string.Empty;
            int limitId = 1;
            int policyLimitId = 1;
            int casulityPolicyLimitId = 1;
            try
            {
                ///////////////////
                if (HttpContext.Current.Session["Limit"] == null && HttpContext.Current.Session["PolicyLimit"] == null && HttpContext.Current.Session["tblCasulityPolicylimit"] == null)
                {
                    DataTable tbllimit = new DataTable();
                    tbllimit.Columns.Add("LimitID", typeof(int));
                    tbllimit.Columns.Add("LimitLetter", typeof(string));
                    tbllimit.Columns.Add("LimitType", typeof(int));
                    tbllimit.Columns.Add("LimitDescription", typeof(string));
                    tbllimit.Columns.Add("IsStatic", typeof(bool));


                    DataTable tblPolicylimit = new DataTable();
                    tblPolicylimit.Columns.Add("PolicyLimitID", typeof(int));
                    tblPolicylimit.Columns.Add("PolicyID", typeof(int));
                    tblPolicylimit.Columns.Add("LimitID", typeof(int));
                    tblPolicylimit.Columns.Add("LimitAmount", typeof(decimal));
                    tblPolicylimit.Columns.Add("LimitDeductible", typeof(decimal));
                    tblPolicylimit.Columns.Add("CATDeductible", typeof(string));
                    tblPolicylimit.Columns.Add("SettlementType", typeof(string));
                    tblPolicylimit.Columns.Add("ConInsuranceLimit", typeof(decimal));
                    tblPolicylimit.Columns.Add("ITV", typeof(decimal));
                    tblPolicylimit.Columns.Add("Reserve", typeof(decimal));
                    tblPolicylimit.Columns.Add("IsDeleted", typeof(bool));
                    tblPolicylimit.Columns.Add("ApplyTo", typeof(string));
                    tblPolicylimit.Columns.Add("ApplyAcrossAllCoverage", typeof(bool));


                    DataTable tblCasulityPolicylimit = new DataTable();
                    tblCasulityPolicylimit.Columns.Add("PolicyLimitID", typeof(int));
                    tblCasulityPolicylimit.Columns.Add("PolicyID", typeof(int));
                    tblCasulityPolicylimit.Columns.Add("LimitID", typeof(int));
                    tblCasulityPolicylimit.Columns.Add("LimitAmount", typeof(decimal));
                    tblCasulityPolicylimit.Columns.Add("LimitDeductible", typeof(decimal));
                    tblCasulityPolicylimit.Columns.Add("CATDeductible", typeof(string));
                    tblCasulityPolicylimit.Columns.Add("SettlementType", typeof(string));
                    tblCasulityPolicylimit.Columns.Add("ConInsuranceLimit", typeof(decimal));
                    tblCasulityPolicylimit.Columns.Add("ITV", typeof(decimal));
                    tblCasulityPolicylimit.Columns.Add("Reserve", typeof(decimal));
                    tblCasulityPolicylimit.Columns.Add("IsDeleted", typeof(bool));
                    tblCasulityPolicylimit.Columns.Add("ApplyTo", typeof(string));
                    tblCasulityPolicylimit.Columns.Add("ApplyAcrossAllCoverage", typeof(bool));

                    DataTable tblAllPolicylimit = new DataTable();
                    tblAllPolicylimit.Columns.Add("PolicyLimitID", typeof(int));
                    tblAllPolicylimit.Columns.Add("PolicyID", typeof(int));
                    tblAllPolicylimit.Columns.Add("LimitID", typeof(int));
                    tblAllPolicylimit.Columns.Add("LimitAmount", typeof(decimal));
                    tblAllPolicylimit.Columns.Add("LimitDeductible", typeof(decimal));
                    tblAllPolicylimit.Columns.Add("CATDeductible", typeof(string));
                    tblAllPolicylimit.Columns.Add("SettlementType", typeof(string));
                    tblAllPolicylimit.Columns.Add("ConInsuranceLimit", typeof(decimal));
                    tblAllPolicylimit.Columns.Add("ITV", typeof(decimal));
                    tblAllPolicylimit.Columns.Add("Reserve", typeof(decimal));
                    tblAllPolicylimit.Columns.Add("IsDeleted", typeof(bool));
                    tblAllPolicylimit.Columns.Add("ApplyTo", typeof(string));
                    tblAllPolicylimit.Columns.Add("ApplyAcrossAllCoverage", typeof(bool));






                    HttpContext.Current.Session["Limit"] = tbllimit;
                    HttpContext.Current.Session["PolicyLimit"] = tblPolicylimit;
                    HttpContext.Current.Session["tblCasulityPolicylimit"] = tblCasulityPolicylimit;
                    HttpContext.Current.Session["tblAllPolicylimit"] = tblAllPolicylimit;

                }

                //find table from session

                DataTable tbllimitGet = HttpContext.Current.Session["Limit"] as DataTable;
                DataTable tblPolicylimitGet = HttpContext.Current.Session["PolicyLimit"] as DataTable;
                DataTable tblCasulityPolicylimitGet = HttpContext.Current.Session["tblCasulityPolicylimit"] as DataTable;
                DataTable tblAllPolicylimitGet = HttpContext.Current.Session["tblAllPolicylimit"] as DataTable;

                //policylimit
                decimal decAmount = 0;
                decimal decDetuctible = 0;

                decimal decCoInsuranceLimit = 0;
                decimal decItv = 0;
                decimal decReserve = 0;
                bool bacrossall = false;

                if (!string.IsNullOrEmpty(policyLimit))
                {
                    decAmount = Convert.ToDecimal(policyLimit);
                }
                if (!string.IsNullOrEmpty(deductible))
                {
                    decDetuctible = Convert.ToDecimal(deductible);
                }
                //add new fields

                if (!string.IsNullOrEmpty(coInsuranceLimit))
                {
                    decCoInsuranceLimit = Convert.ToDecimal(coInsuranceLimit);
                }




                if (!string.IsNullOrEmpty(itv))
                {
                    decItv = Convert.ToDecimal(itv);
                }
                if (!string.IsNullOrEmpty(reserve))
                {
                    decReserve = Convert.ToDecimal(reserve);
                }
                if (acrossall == 1)
                {
                    bacrossall = true;
                }
                else
                {
                    bacrossall = false;
                }

                //set limitid and policylimitid
                if (tbllimitGet.Rows.Count == 0)
                {
                    limitId = 1;
                }
                else
                {
                    limitId = Convert.ToInt32(tbllimitGet.Rows[tbllimitGet.Rows.Count - 1]["LimitID"]) + 1;
                }
                if (tblPolicylimitGet.Rows.Count == 0)
                {
                    policyLimitId = 1;
                }
                else
                {
                    policyLimitId = Convert.ToInt32(tblPolicylimitGet.Rows[tblPolicylimitGet.Rows.Count - 1]["PolicyLimitID"]) + 1;
                }
                if (tblCasulityPolicylimitGet.Rows.Count == 0)
                {
                    casulityPolicyLimitId = 1;
                }
                else
                {
                    casulityPolicyLimitId = Convert.ToInt32(tblCasulityPolicylimitGet.Rows[tblCasulityPolicylimitGet.Rows.Count - 1]["PolicyLimitID"]) + 1;
                }
                /////
                if (type == "Contacts" || type == "Personal Liability" || type == "Medical Payments")
                {
                    tbllimitGet.Rows.Add(limitId, coverage, 2, type, false);

                    tblCasulityPolicylimitGet.Rows.Add(0, casulityPolicyLimitId, limitId, decAmount, decDetuctible, catDeductible, "", decCoInsuranceLimit, decItv, decReserve, false, applyTo, bacrossall);

                    tblAllPolicylimitGet.Rows.Add(0, policyLimitId, limitId, decAmount, decDetuctible, catDeductible, "", decCoInsuranceLimit, decItv, decReserve, false, applyTo, bacrossall);

                }
                else
                {
                    tbllimitGet.Rows.Add(limitId, coverage, 1, type, false);

                    tblPolicylimitGet.Rows.Add(0, policyLimitId, limitId, decAmount, decDetuctible, catDeductible, "", decCoInsuranceLimit, decItv, decReserve, false, applyTo, bacrossall);

                    tblAllPolicylimitGet.Rows.Add(0, policyLimitId, limitId, decAmount, decDetuctible, catDeductible, "", decCoInsuranceLimit, decItv, decReserve, false, applyTo, bacrossall);

                }


                HttpContext.Current.Session["Limit"] = tbllimitGet;
                HttpContext.Current.Session["PolicyLimit"] = tblPolicylimitGet;
                HttpContext.Current.Session["tblCasulityPolicylimit"] = tblCasulityPolicylimitGet;
                HttpContext.Current.Session["tblAllPolicylimit"] = tblAllPolicylimitGet;

                json = "Loss details successfully add";

            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

            return json;
        }


        [System.Web.Services.WebMethod]
        public static string[] GetLossDataAddEditProperty(int limitId)
        {
            string[] json = new string[12];

            DataTable tbllimitGet = HttpContext.Current.Session["Limit"] as DataTable;
            DataTable tblPolicylimitGet = HttpContext.Current.Session["PolicyLimit"] as DataTable;
            DataTable tblCasulityPolicylimitGet = HttpContext.Current.Session["tblCasulityPolicylimit"] as DataTable;
            DataTable tblAllPolicylimitGet = HttpContext.Current.Session["tblAllPolicylimit"] as DataTable;

            string limitFilter = "LimitID=" + limitId;
            DataRow[] limitRow = tbllimitGet.Select(limitFilter);

            foreach (DataRow row in limitRow)
            {

                json[0] = row["LimitLetter"] != null ? row["LimitLetter"].ToString() : "";

                json[1] = row["LimitType"] != null ? row["LimitType"].ToString() : "";

                json[2] = row["LimitDescription"] != null ? row["LimitDescription"].ToString() : "";
            }



            DataRow[] policyLimitRow = tblPolicylimitGet.Select(limitFilter);

            foreach (DataRow row in policyLimitRow)
            {
                json[3] = row["LimitAmount"] != null ? row["LimitAmount"].ToString() : "";
                json[4] = row["LimitDeductible"] != null ? row["LimitDeductible"].ToString() : "";
                json[5] = row["ITV"] != null ? row["ITV"].ToString() : "";
                json[6] = row["Reserve"] != null ? row["Reserve"].ToString() : "";
                json[7] = row["ApplyTo"] != null ? row["ApplyTo"].ToString() : "";

                json[10] = row["CATDeductible"] != null ? row["CATDeductible"].ToString() : "";
                json[11] = row["ConInsuranceLimit"] != null ? row["ConInsuranceLimit"].ToString() : "";

            }

            DataRow[] casulityPolicyLimitRow = tblCasulityPolicylimitGet.Select(limitFilter);

            foreach (DataRow row in casulityPolicyLimitRow)
            {
                json[3] = row["LimitAmount"] != null ? row["LimitAmount"].ToString() : "";
                json[4] = row["LimitDeductible"] != null ? row["LimitDeductible"].ToString() : "";
                json[5] = row["ITV"] != null ? row["ITV"].ToString() : "";
                json[6] = row["Reserve"] != null ? row["Reserve"].ToString() : "";
                json[7] = row["ApplyTo"] != null ? row["ApplyTo"].ToString() : "";

                json[10] = row["CATDeductible"] != null ? row["CATDeductible"].ToString() : "";
                json[11] = row["ConInsuranceLimit"] != null ? row["ConInsuranceLimit"].ToString() : "";
            }

            if (tblAllPolicylimitGet.Rows.Count > 0)
            {
                json[8] = tblAllPolicylimitGet.Rows[0]["ApplyAcrossAllCoverage"].ToString();
                json[9] = tblAllPolicylimitGet.Rows[0]["LimitID"].ToString();
            }
            return json;
        }



        //for coverage add mode
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string SaveLossDetailsAddEditProperty(string coverage, string type, string policyLimit, string deductible, string applyTo, string itv, string reserve, int acrossall, int limitId, string catDeductible, string coInsuranceLimit)
        {
            string json = string.Empty;
            try
            {
                //find table from session
                DataTable tbllimitGet = HttpContext.Current.Session["Limit"] as DataTable;
                DataTable tblPolicylimitGet = HttpContext.Current.Session["PolicyLimit"] as DataTable;
                DataTable tblCasulityPolicylimitGet = HttpContext.Current.Session["tblCasulityPolicylimit"] as DataTable;
                DataTable tblAllPolicylimitGet = HttpContext.Current.Session["tblAllPolicylimit"] as DataTable;

                //policylimit
                decimal decAmount = 0;
                decimal decDetuctible = 0;
                decimal decCoInsuranceLimit = 0;
                decimal decItv = 0;
                decimal decReserve = 0;
                bool bacrossall = false;
                if (!string.IsNullOrEmpty(policyLimit))
                {
                    decAmount = Convert.ToDecimal(policyLimit);
                }
                if (!string.IsNullOrEmpty(deductible))
                {
                    decDetuctible = Convert.ToDecimal(deductible);
                }
                //add new fields

                if (!string.IsNullOrEmpty(coInsuranceLimit))
                {
                    decCoInsuranceLimit = Convert.ToDecimal(coInsuranceLimit);
                }

                if (!string.IsNullOrEmpty(itv))
                {
                    decItv = Convert.ToDecimal(itv);
                }
                if (!string.IsNullOrEmpty(reserve))
                {
                    decReserve = Convert.ToDecimal(reserve);
                }
                if (acrossall == 1)
                {
                    bacrossall = true;
                }
                else
                {
                    bacrossall = false;
                }

                /////
                int limitType = 0;
                int oldLimitType = 0;
                if (type == "Contacts" || type == "Personal Liability" || type == "Medical Payments")
                {
                    limitType = 2;
                }
                else
                {
                    limitType = 1;
                }


                string limitFilter = "LimitID=" + limitId;
                DataRow[] limitRow = tbllimitGet.Select(limitFilter);

                foreach (DataRow row in limitRow)
                {
                    oldLimitType = Convert.ToInt32(row["LimitType"]);
                    row["LimitLetter"] = coverage;
                    row["LimitType"] = limitType.ToString();
                    row["LimitDescription"] = type;

                    tbllimitGet.AcceptChanges();
                }
                DataRow[] policyLimitRow = tblPolicylimitGet.Select(limitFilter);
                foreach (DataRow row in policyLimitRow)
                {
                    row["LimitAmount"] = decAmount;
                    row["LimitDeductible"] = decDetuctible;
                    row["CATDeductible"] = catDeductible;
                    row["ConInsuranceLimit"] = decCoInsuranceLimit;
                    row["ITV"] = decItv;
                    row["Reserve"] = decReserve;
                    row["ApplyTo"] = applyTo;
                    row["ApplyAcrossAllCoverage"] = bacrossall;

                    tblPolicylimitGet.AcceptChanges();
                }

                DataRow[] casulityPolicyLimitRow = tblCasulityPolicylimitGet.Select(limitFilter);

                foreach (DataRow row in casulityPolicyLimitRow)
                {
                    row["LimitAmount"] = decAmount;
                    row["LimitDeductible"] = decDetuctible;
                    row["CATDeductible"] = catDeductible;
                    row["ConInsuranceLimit"] = decCoInsuranceLimit;
                    row["ITV"] = decItv;
                    row["Reserve"] = decReserve;
                    row["ApplyTo"] = applyTo;
                    row["ApplyAcrossAllCoverage"] = bacrossall;
                    tblCasulityPolicylimitGet.AcceptChanges();
                }

                DataRow[] allPolicyLimitRow = tblAllPolicylimitGet.Select(limitFilter);

                foreach (DataRow row in allPolicyLimitRow)
                {
                    row["LimitAmount"] = decAmount;
                    row["LimitDeductible"] = decDetuctible;
                    row["CATDeductible"] = catDeductible;
                    row["ConInsuranceLimit"] = decCoInsuranceLimit;
                    row["ITV"] = decItv;
                    row["Reserve"] = decReserve;
                    row["ApplyTo"] = applyTo;
                    row["ApplyAcrossAllCoverage"] = bacrossall;
                    tblAllPolicylimitGet.AcceptChanges();
                }
                ///////////////////////////
                //if type change then move vice versa
                // 1 to 2

                if (limitType == 1 && oldLimitType == 2)
                {
                    foreach (DataRow row in casulityPolicyLimitRow)
                    {
                        tblPolicylimitGet.Rows.Add(0, Convert.ToInt32(row["PolicyID"]), Convert.ToInt32(row["LimitID"]), Convert.ToDecimal(row["LimitAmount"]), Convert.ToDecimal(row["LimitDeductible"]), row["CATDeductible"], "", Convert.ToDecimal(row["ConInsuranceLimit"]), Convert.ToDecimal(row["ITV"]), Convert.ToDecimal(row["Reserve"]), false, Convert.ToString(row["ApplyTo"]), Convert.ToBoolean(row["ApplyAcrossAllCoverage"]));
                        row.Delete();
                    }
                    tblCasulityPolicylimitGet.AcceptChanges();
                }


                if (limitType == 2 && oldLimitType == 1)
                {
                    foreach (DataRow row in policyLimitRow)
                    {
                        tblCasulityPolicylimitGet.Rows.Add(0, Convert.ToInt32(row["PolicyID"]), Convert.ToInt32(row["LimitID"]), Convert.ToDecimal(row["LimitAmount"]), Convert.ToDecimal(row["LimitDeductible"]), row["CATDeductible"], "", Convert.ToDecimal(row["ConInsuranceLimit"]), Convert.ToDecimal(row["ITV"]), Convert.ToDecimal(row["Reserve"]), false, Convert.ToString(row["ApplyTo"]), Convert.ToBoolean(row["ApplyAcrossAllCoverage"]));
                        row.Delete();
                    }
                    tblPolicylimitGet.AcceptChanges();
                }



                /////////////////////////////
                HttpContext.Current.Session["Limit"] = tbllimitGet;
                HttpContext.Current.Session["PolicyLimit"] = tblPolicylimitGet;
                HttpContext.Current.Session["tblCasulityPolicylimit"] = tblCasulityPolicylimitGet;
                HttpContext.Current.Session["tblAllPolicylimit"] = tblAllPolicylimitGet;

                json = "Loss details successfully add";


            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

            return json;
        }

        [System.Web.Services.WebMethod]
        public static string[] GetLossDataEditPropertyEdit(int limitId)
        {
            string[] json = new string[12];
            int policyId = 0;
            List<Limit> limit = null;
            List<PolicyLimit> policyLimit = null;


            limit = LimitManager.GetLimit(limitId);
            policyLimit = PolicyLimitManager.GetPolicyLimit(limitId);

            if (limit != null && limit.Count > 0)
            {
                json[0] = limit[0].LimitLetter;

                json[1] = limit[0].LimitType.ToString();

                json[2] = limit[0].LimitDescription;
            }
            if (policyLimit != null && policyLimit.Count > 0)
            {
                json[3] = policyLimit[0].LimitAmount == null ? "0" : policyLimit[0].LimitAmount.ToString();
                json[4] = policyLimit[0].LimitDeductible == null ? "0" : policyLimit[0].LimitDeductible.ToString();
                json[5] = policyLimit[0].ITV == null ? "0" : policyLimit[0].ITV.ToString();
                json[6] = policyLimit[0].Reserve == null ? "0" : policyLimit[0].Reserve.ToString();
                json[7] = policyLimit[0].ApplyTo.ToString();
                policyId = policyLimit[0].PolicyID;

                json[10] = policyLimit[0].CATDeductible == null ? "0" : policyLimit[0].CATDeductible.ToString();
                json[11] = policyLimit[0].ConInsuranceLimit == null ? "0" : policyLimit[0].ConInsuranceLimit.ToString();

            }
            policyLimit = null;
            policyLimit = PolicyLimitManager.GetAll(policyId);
            if (policyLimit != null && policyLimit.Count > 0)
            {
                json[8] = policyLimit[0].ApplyAcrossAllCoverage == null ? "0" : policyLimit[0].ApplyAcrossAllCoverage.ToString();
                json[9] = policyLimit[0].LimitID == null ? "0" : policyLimit[0].LimitID.ToString();

            }
            return json;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string SaveLossDetailsEditPropertyEdit(int policyID, string coverage, string type, string policyLimit, string deductible, string applyTo, string itv, string reserve, int acrossall, int limitId, string catDeductible, string coInsuranceLimit)
        {
            string json = string.Empty;
            try
            {
                Limit objLimit = null;
                PolicyLimit objPolicyLimit = null;
                decimal decAmount = 0;
                decimal decDetuctible = 0;
                decimal decCoInsuranceLimit = 0;
                decimal decItv = 0;
                decimal decReserve = 0;
                bool bacrossall = false;
                if (!string.IsNullOrEmpty(policyLimit))
                {
                    decAmount = Convert.ToDecimal(policyLimit);
                }
                if (!string.IsNullOrEmpty(deductible))
                {
                    decDetuctible = Convert.ToDecimal(deductible);
                }
                //add new fields
                if (!string.IsNullOrEmpty(coInsuranceLimit))
                {
                    decCoInsuranceLimit = Convert.ToDecimal(coInsuranceLimit);
                }

                if (!string.IsNullOrEmpty(itv))
                {
                    decItv = Convert.ToDecimal(itv);
                }
                if (!string.IsNullOrEmpty(reserve))
                {
                    decReserve = Convert.ToDecimal(reserve);
                }
                if (acrossall == 1)
                {
                    bacrossall = true;
                }
                else
                {
                    bacrossall = false;
                }

                /////
                int limitType = 0;

                if (type == "Contacts" || type == "Personal Liability" || type == "Medical Payments")
                {
                    limitType = 2;
                }
                else
                {
                    limitType = 1;
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    objLimit = new Limit();
                    objLimit.LimitID = limitId;
                    objLimit.LimitLetter = coverage;
                    objLimit.LimitType = limitType;
                    objLimit.LimitDescription = type;
                    LimitManager.UpdateLimit(objLimit);

                    objPolicyLimit = new PolicyLimit();
                    objPolicyLimit.PolicyID = policyID;
                    objPolicyLimit.LimitID = limitId;
                    objPolicyLimit.LimitAmount = decAmount;
                    objPolicyLimit.LimitDeductible = decDetuctible;
                    objPolicyLimit.CATDeductible = catDeductible;
                    objPolicyLimit.ConInsuranceLimit = decCoInsuranceLimit;
                    objPolicyLimit.ITV = decItv;
                    objPolicyLimit.Reserve = decReserve;
                    objPolicyLimit.ApplyAcrossAllCoverage = bacrossall;
                    objPolicyLimit.ApplyTo = applyTo;
                    PolicyLimitManager.UpdatePolicyLimit(objPolicyLimit);
                    scope.Complete();
                }
                json = "Loss details successfully add";
            }
            catch (Exception ex)
            {
                Core.EmailHelper.emailError(ex);
            }

            return json;
        }






    }
}