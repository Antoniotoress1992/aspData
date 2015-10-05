using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using CRM.Data;
using CRM.Data.Account;
using CRM.Core;
using CRM.Repository;
using CRM.RuleEngine;
using CRM.Data.Entities;

namespace CRM.RuleEngine.Service {
	class Program {
		static void Main(string[] args) {
			processClaimAssignmentReview();
		}

		static void processClaimAssignmentReview() {
			List<Claim> claims = null;
			List<BusinessRule> rules = null;
			RuleException ruleException = null;

			// get business rules for client/rule type id
			using (BusinessRuleManager repository = new BusinessRuleManager()) {
				rules = repository.GetBusinessRules(Globals.RuleType.ClaimAssingmentReview);
			}

			if (rules != null && rules.Count > 0) {
				foreach (BusinessRule rule in rules) {
					using (ClaimAssignmentReview ruleEngine = new ClaimAssignmentReview()) {
						claims = ruleEngine.TestRule(rule);


						if (claims != null && claims.Count > 0) {
							using (TransactionScope scope = new TransactionScope()) {
								try {
									foreach (Claim claim in claims) {
										// check exception already exists for this claim
										bool exceptionExists = ruleEngine.ExceptionExists((int)rule.ClientID, rule.BusinessRuleID, claim.ClaimID, (int)Globals.ObjectType.Claim);

										if (!exceptionExists) {
											// add exception to queue
											ruleException = new RuleException();

											ruleException.BusinessRuleID = rule.BusinessRuleID;

											ruleException.ClientID = rule.ClientID;

											ruleException.ObjectID = claim.ClaimID;

											ruleException.ObjectTypeID = (int)Globals.ObjectType.Claim;

											ruleException.UserID = null;

											ruleEngine.AddException(ruleException);
                                            //chetu code

                                            CheckSendMail(ruleException);


										}
									}

									// commit transaction
									scope.Complete();
								}
								catch (Exception ex) {
									Core.EmailHelper.emailError(ex);
								}
							}
						}			// if (claims != null && claims.Count > 0) 
					}				// using (ClaimAssignmentReview ruleEngine = new ClaimAssignmentReview()) 
				}	// foreach
			} // if
		}

        public static void CheckSendMail( RuleException ruleExp)
        {
            if (ruleExp!=null)
            {
            string adjusterEmail = string.Empty;
            string supervisorEmail = string.Empty;
            bool sendAdjuster=false;
            bool sendSupervisor=false;
            string recipient=string.Empty;
            int claimId=0;

            BusinessRuleManager objRuleManager = new BusinessRuleManager();
            BusinessRule objRule = new BusinessRule();
            CRM.Data.Entities.Claim objClaim=new CRM.Data.Entities.Claim(); 
            CRM.Data.Entities.SecUser objSecUser=new Data.Entities.SecUser();
            AdjusterMaster adjustermaster=new AdjusterMaster();
                
            int businessRuleID = ruleExp.BusinessRuleID ?? 0;
            objRule= objRuleManager.GetBusinessRule(businessRuleID);
            if (objRule!=null)
            {
            claimId=ruleExp.ObjectID??0;

            objClaim=objRuleManager.GetClaim(claimId);
            adjustermaster=  objRuleManager.GetAdjuster(objClaim.AdjusterID??0);
            objSecUser= objRuleManager.GetSupervisor(objClaim.SupervisorID??0);
            if (objSecUser!=null)
            {
            adjusterEmail=adjustermaster.email;
            supervisorEmail=objSecUser.Email;

            sendAdjuster=objRule.EmailAdjuster;
            sendSupervisor=objRule.EmailSupervisor;

            if(sendAdjuster==true && sendSupervisor==true)
            {
                recipient=adjusterEmail+","+supervisorEmail;
                notifyUser(objRule.Description, claimId, recipient);
            }
            else if(sendAdjuster==false && sendSupervisor==true)
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

        public static void notifyUser(string description, int claimid,string recipient)
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
                emailBody.Append(description + "with claim # "+claimid);  
                emailBody.Append("</div>");	// paneContentInner
                emailBody.Append("</div>");	// containerBox

                password = Core.SecurityManager.Decrypt(user.emailPassword);

                Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, true);//user.isSSL ??
            
        }


	}
}
