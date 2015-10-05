
namespace CRM.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.Security;
    using CRM.Core;
    using LinqKit;

    using CRM.Data;
    using CRM.Data.Account;
    using CRM.Repository;
    using CRM.Data.Entities;
    using CRM.Web.UserControl.Admin;
    using System.Configuration;
    using System.Data.SqlClient;

    using System.Data;
    using System.Threading;
    using System.Xml.Linq;
    using CRM.RuleEngine;
    using System.Web.SessionState;
    using System.Text;
    using System.Net;
    using System.Net.Mail;



    public partial class Login : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ClaimRuler"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.SetFocus(txtUserName);

        }


        List<SecRoleModuleManager.secRoleModuleGet> resRolePermission = new List<SecRoleModuleManager.secRoleModuleGet>();
        Worker workerObject = new Worker();

        public void DoAuthenticate(CRM.Data.Entities.SecUser user)
        {
            string url = null;
            string userData = null;
            List<int> roleActions = null;

            Session["UserId"] = user.UserId;
            Session["UserName"] = user.UserName;
            Session["RoleId"] = user.SecRole.RoleId.ToString();

            // 201307-29
            Session["ClientId"] = user.ClientID;
            if (user.Client.Count > 0)
            {
                Session["ClientShowTask"] = user.Client == null ? true : (user.Client.FirstOrDefault().isShowTasks ?? true);
            }
            else
            {
                Session["ClientShowTask"] = true;
            }

            userData = string.Format("{0}|{1}|{2} {3}|{4}", user.SecRole.RoleName, user.SecRole.RoleId, user.FirstName, user.LastName, user.Email);

            var ticket = new FormsAuthenticationTicket
                (
                   1,
                   user.UserId.ToString(),
                   DateTime.Now,
                   DateTime.Now.AddMinutes(120),
                   true,
                   userData,	//(user.SecRole.RoleName + "|" + user.SecRole.RoleId.ToString()),
                   FormsAuthentication.FormsCookiePath
                );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
            Session["rolePermission"] = resRolePermission = SecRoleModuleManager.getRolePermission(user.SecRole.RoleId).ToList();

            // get role actions
            if (user.RoleId != (int)UserRole.Administrator)
            {
                using (ActionManager repository = new ActionManager())
                {
                    roleActions = repository.GetActions((int)user.ClientID, (int)user.RoleId);
                    Session["roleActions"] = roleActions;
                }
            }

            //if (user.SecRole.RoleId == (int)UserRole.Adjuster)
            //	url = "~/Protected/Intake/form.aspx";
            //else
            if (Request.QueryString["url"] != null)
            {
                url = Request.QueryString["url"].ToString();
            }
            else
            {
                url = FormsAuthentication.DefaultUrl;
                Session["Count"] = null;


                formatException();
                setRulexception();

                setGlobalSession();



                Thread thread = new Thread(delegate()
                {
                    workerObject.DoWork();
                });
                //ThreadPool.QueueUserWorkItem(new WaitCallback(workerObject.DoWork), leadView, testse);

                // workerThread = new Thread(workerObject.DoWork);

                thread.Start();
            }

            Response.Redirect(url);
        }
        List<LoginTrials> resLoginTrails = new List<LoginTrials>();

        public void setGlobalSession()
        {
            Globals gv = Globals.Instance();

            //int[] claimInt;
            ucAllUserLeads userGetClaim = new ucAllUserLeads();
            List<LeadView> lead = new List<LeadView>();
            lead = userGetClaim.getLeadList();

            int[] claimInt = new int[lead.Count];
            for (var i = 0; i < lead.Count; i++)
            {
                claimInt[i] = lead[i].ClaimID;
            }


            gv.setCliamList(claimInt);
            //gv.setClaimId(Session["ClaimID"].ToString());
            gv.setUserId(Session["UserId"].ToString());
            gv.setUserName(Session["UserName"].ToString());
            //gv.setClaimId(Session["RoleId"].ToString());
            gv.setClientId(Session["ClientId"].ToString());
            //gv.setClaimId(Session["ClientShowTask"].ToString());
            //gv.setClaimId(Session["rolePermission"].ToString());
            //gv.setClaimId(Session["roleActions"].ToString());
            //gv.setClaimId(Session["Count"].ToString());
            gv.setclaimCount(lead.Count);


        }




        public void formatException()
        {
            ucAllUserLeads userGetClaim = new ucAllUserLeads();
            List<LeadView> leadView = new List<LeadView>();
            leadView = userGetClaim.getLeadList();
            int clientID = Convert.ToInt32(Session["ClientId"]);
            int userID = Convert.ToInt32(Session["UserId"]);
            int claimId = 0;

            for (int i = 0; i < leadView.Count; i++)
            {
                claimId = leadView[i].ClaimID;
                formatExceptions(clientID, userID, claimId);
            }

        }


        public void formatExceptions(int clientID, int userID, int claimId)
        {
            RuleExceptionManager RuleExceptionManagerObj = new RuleExceptionManager();
            RuleExceptionManagerObj.format(clientID, userID, claimId);

        }
        public void setRulexception()
        {
            ucAllUserLeads userGetClaim = new ucAllUserLeads();
            List<LeadView> leadView = new List<LeadView>();
            leadView = userGetClaim.getLeadList();
            int clientID = Convert.ToInt32(Session["ClientId"]);
            int userID = Convert.ToInt32(Session["UserId"]);
            int claimId;

            for (int i = 0; i < leadView.Count; i++)
            {
                claimId = leadView[i].ClaimID;


                if (claimId != 0)
                {
                    int progressId = getProgressId(claimId);
                    DateTime progressDate = progressClaimRecievedData(claimId);
                    if (progressId != 0)
                    {

                        List<int> ruleId = getRuleId(progressId);
                        for (var j = 0; j < ruleId.Count; j++)
                        {
                            if (ruleId.Count != 0)
                            {
                                List<BusinessRule> businessRuleArr = new List<BusinessRule>();
                                businessRuleArr = getBusinessRuleId(clientID, ruleId[j]);
                                for (var k = 0; k < businessRuleArr.Count; k++)
                                {
                                    int timeInterval = getTimeFlag(businessRuleArr[k].RuleXML, businessRuleArr[k].Description);
                                    TimeSpan timeDiff = DateTime.Now - progressDate;
                                    if ((timeDiff.Days * 24 + timeDiff.Hours) >= timeInterval)
                                    {
                                        int checkstate = checkInsertable(businessRuleArr[k].BusinessRuleID, clientID, userID, claimId);
                                        if (checkstate == 0)
                                        {
                                            insertRuleException(businessRuleArr[k].BusinessRuleID, clientID, userID, claimId);
                                        }
                                        if (checkstate == 1)
                                        {
                                            updateRuleException(businessRuleArr[k].BusinessRuleID, clientID, userID, claimId);
                                        }
                                    }
                                }

                            }

                        }
                    }
                }
            }

        }

        public int checkInsertable(int businessruleId, int clientId, int userId, int claimId)
        {
            RuleExceptionManager RuleExceptionManagerObj = new RuleExceptionManager();
            RuleException RuleExceptionObj = new RuleException();
            int insertState = 0;

            RuleExceptionObj = RuleExceptionManagerObj.GetRuleException(businessruleId, clientId, userId, claimId);

            try
            {
                bool active = Convert.ToBoolean(RuleExceptionObj.IsActive);
                if (active == false) { insertState = 1; } else { insertState = 2; }

            }
            catch (Exception e)
            {

                insertState = 0;
            }




            return insertState;


        }


        public int getTimeFlag(string ruleXml, string value)
        {
            int timeInterval = 0;
            int rulexml = Convert.ToInt32(ruleXml);
            int timeValue = Convert.ToInt32(value);

            if (rulexml == 1)
            {
                timeInterval = timeValue;
            }
            if (rulexml == 2)
            {
                timeInterval = timeValue * 24;
            }
            return timeInterval;

        }
        public int getProgressId(int claimId)
        {
            int progress = 0;
            ClaimManager ClaimManagerobj = new ClaimManager();
            Claim claimObj = new Claim();
            claimObj = ClaimManagerobj.Get(claimId);

            if (claimObj.ProgressStatusID != null)
            {
                progress = Convert.ToInt32(claimObj.ProgressStatusID);

            }
            else
            {
                progress = 0;

            }



            return progress;
        }

        public List<int> getRuleId(int progressId)
        {

            List<int> ruleIds = new List<int>();
            if (checkRuleId1(progressId) == 1) { ruleIds.Add((int)Globals.RuleType.NewClaimAttention); }
            if (checkRuleId2(progressId) == 1) { ruleIds.Add((int)Globals.RuleType.ClaimAssingmentReview); }
            if (checkRuleId3(progressId) == 1) { ruleIds.Add((int)Globals.RuleType.ContactWithInsured); }
            if (checkRuleId4(progressId) == 1) { ruleIds.Add((int)Globals.RuleType.FirstReport); }
            if (checkRuleId5(progressId) == 1) { ruleIds.Add((int)Globals.RuleType.InterimOrFinalReportOrClaimClosing); }
            return ruleIds;
        }
        public DateTime progressClaimRecievedData(int claimId)
        {
            DateTime progressDate = new DateTime();
            ClaimManager ClaimManagerobj = new ClaimManager();
            Claim claimObj = new Claim();
            claimObj = ClaimManagerobj.Get(claimId);

            if (claimObj.ProgressStatusID != null)
            {

                progressDate = Convert.ToDateTime(claimObj.DateOpenedReported);


            }
            else
            {
                progressDate = DateTime.MinValue;
            }

            return progressDate;
        }
        public int checkRuleId1(int progressId)
        {
            int result = 0;
            if (progressId == 16) { result = 1; }
            if (progressId == 0) { result = 1; }
            return result;
        }
        public int checkRuleId2(int progressId)
        {
            int result = 0;
            if (progressId == 16) { result = 1; }
            if (progressId == 1) { result = 1; }
            if (progressId == 0) { result = 1; }
            return result;
        }
        public int checkRuleId3(int progressId)
        {
            int result = 0;
            if (progressId == 16) { result = 1; }
            if (progressId == 1) { result = 1; }
            if (progressId == 2) { result = 1; }
            if (progressId == 0) { result = 1; }
            return result;
        }
        public int checkRuleId4(int progressId)
        {
            int result = 0;
            if (progressId == 16) { result = 1; }
            if (progressId == 1) { result = 1; }
            if (progressId == 2) { result = 1; }
            if (progressId == 3) { result = 1; }
            if (progressId == 4) { result = 1; }
            if (progressId == 5) { result = 1; }
            if (progressId == 0) { result = 1; }
            return result;
        }
        public int checkRuleId5(int progressId)
        {
            int result = 0;
            if (progressId == 16) { result = 1; }
            if (progressId == 1) { result = 1; }
            if (progressId == 2) { result = 1; }
            if (progressId == 3) { result = 1; }
            if (progressId == 4) { result = 1; }
            if (progressId == 5) { result = 1; }
            if (progressId == 6) { result = 1; }
            if (progressId == 0) { result = 1; }
            return result;
        }


        public List<BusinessRule> getBusinessRuleId(int clientID, int ruleId)
        {
            List<BusinessRule> businessRuleArr = new List<BusinessRule>();
            BusinessRuleManager businessRuleManager = new BusinessRuleManager();

            businessRuleArr = businessRuleManager.GetBusinessRuleThread(clientID, ruleId);
            for (var i = 0; i < businessRuleArr.Count; i++)
            {
                XElement ruleXML = XElement.Parse(businessRuleArr[i].RuleXML.ToString());
                using (RuleHelper ruleHelper = new RuleHelper())
                {
                    XElement conditionLapseTime = ruleHelper.GetElement(ruleXML, "LapseTime");
                    businessRuleArr[i].Description = conditionLapseTime.Element("value").Value;

                    XElement conditionLapseTimeType = ruleHelper.GetElement(ruleXML, "LapseTimeType");
                    businessRuleArr[i].RuleXML = conditionLapseTimeType.Element("value").Value;
                }



            }

            return businessRuleArr;
        }

        public void insertRuleException(int BusinessRuleId, int clientID, int userID, int claimId)
        {

            RuleExceptionManager RuleExceptionManagerObj = new RuleExceptionManager();
            RuleException RuleExceptionObj = new RuleException();

            RuleExceptionObj.BusinessRuleID = BusinessRuleId;
            RuleExceptionObj.ClientID = clientID;
            RuleExceptionObj.UserID = userID;
            RuleExceptionObj.ObjectID = claimId;
            RuleExceptionObj.ObjectTypeID = 1;

            RuleExceptionManagerObj.Save(RuleExceptionObj);


        }
        public void updateRuleException(int BusinessRuleId, int clientID, int userID, int claimId)
        {

            RuleExceptionManager RuleExceptionManagerObj = new RuleExceptionManager();
            RuleExceptionManagerObj.update(BusinessRuleId, clientID, userID, claimId);

        }
        protected void btnSubmit_click(object sender, EventArgs e)
        {

            CRM.Data.Entities.SecUser user = SecUserManager.GetByUserName(txtUserName.Text.Trim());
            if (user.UserName != null && user.Password != null)
            {
                string password = SecurityManager.Decrypt(user.Password);

                if (user.UserId > 0 && password.Equals(txtPassword.Text.Trim()))
                {
                    lblError.Visible = false;


                    DoAuthenticate(user);

                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Invalid User Name or Password!";
                    createLoginLog(user.UserId, false);

                }
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Invalid User Name or Password!";
                return;
            }

        }


        private void checkforLoginAttempt()
        {

        }

        private void createLoginLog(int UserId, bool sucess)
        {


            SecLoginLog loginlog = new SecLoginLog();
            loginlog.UserId = UserId;
            loginlog.LoginId = txtUserName.Text.Trim();
            loginlog.LoginTime = DateTime.Now;
            loginlog.LogoutTime = DateTime.Now;
            if (sucess == true)
            {
                loginlog.Sucess = true;
            }
            else
            {
                loginlog.Sucess = false;
            }
            loginlog.CreatedMachineIP = Request.ServerVariables["remote_addr"].ToString();
            SecLoginLogManager.Save(loginlog);
        }



    }
    public class Worker
    {
        public int k = 0;

        bool firstLogin = false;

        public void DoWork()
        {

            int claimId = 0;
            Globals gvGet = Globals.Instance();
            int clientID = Convert.ToInt32(gvGet.getClientId());
            int userID = Convert.ToInt32(gvGet.getUserId());
            int claimCount = gvGet.getclaimCount();
            int[] claimList = new int[claimCount];
            claimList = gvGet.getCliamList();

            //string str_tmp = gv.getTestValue();
            while (!_shouldStop)
            {


                for (int i = 0; i < claimCount; i++)
                {
                    claimId = claimList[i];
                    formatExceptions(clientID, userID, claimId);
                }
                for (int i = 0; i < claimCount; i++)
                {
                    claimId = claimList[i];


                    if (claimId != 0)
                    {
                        int progressId = getProgressId(claimId);
                        DateTime progressDate = progressClaimRecievedData(claimId);
                        if (progressId != 0)
                        {

                            List<int> ruleId = getRuleId(progressId);
                            for (var j = 0; j < ruleId.Count; j++)
                            {
                                if (ruleId.Count != 0)
                                {
                                    List<BusinessRule> businessRuleArr = new List<BusinessRule>();
                                    businessRuleArr = getBusinessRuleId(clientID, ruleId[j]);
                                    for (var k = 0; k < businessRuleArr.Count; k++)
                                    {
                                        int timeInterval = getTimeFlag(businessRuleArr[k].RuleXML, businessRuleArr[k].Description);
                                        TimeSpan timeDiff = DateTime.Now - progressDate;
                                        if ((timeDiff.Days * 24 + timeDiff.Hours) >= timeInterval)
                                        {
                                            int checkstate = checkInsertable(businessRuleArr[k].BusinessRuleID, clientID, userID, claimId);

                                            if (checkstate == 0)
                                            {
                                                insertRuleException(businessRuleArr[k].BusinessRuleID, clientID, userID, claimId);
                                            }
                                            if (checkstate == 1)
                                            {
                                                updateRuleException(businessRuleArr[k].BusinessRuleID, clientID, userID, claimId);
                                            }
                                        }
                                    }

                                }

                            }

                        }
                    }
                }
                setAdjusterSupervisorList();
                sendEmailThread();
                if (firstLogin == false)
                {
                    firstLogin = true;
                    setAdjusterSupervisorList();
                }
                else {
                    sendEmailThread();
                }
                
                
               
                Thread.Sleep(1000 * 60 * 1);
           }


        }
        public void sendEmailThread()
        {

            var adjusterCount = 0;
            var supervisorCount = 0;

            RuleExceptionManager RuleExceptionManagerObj = new RuleExceptionManager();
            RuleException RuleExceptionObj = new RuleException();
            List<RuleException> RuleExceptionArr = new List<RuleException>();
            Globals gvGet = Globals.Instance();
            int clientID = Convert.ToInt32(gvGet.getClientId());
            RuleExceptionArr = RuleExceptionManagerObj.GetAllException(clientID);




            for (var i = 0; i < RuleExceptionArr.Count; i++)
            {
                int businessRuleId = Convert.ToInt32(RuleExceptionArr[i].BusinessRuleID);
                if (checkAdjusterSendMail(businessRuleId))
                {
                    adjusterCount = adjusterCount + 1;
                }
                if (checkSupervisorSendMail(businessRuleId))
                {
                    supervisorCount = supervisorCount + 1;
                }

            }

            int[] exceptionListAdjuster = new int[adjusterCount];
            int[] formerExceptionListAdjuster = gvGet.getExceptionListAdjuster();

            int[] exceptionListSupervisor = new int[supervisorCount];
            int[] formerExceptionSupervisor = gvGet.getExceptionListSupervisor();

            var j = 0; var k = 0;

            for (var i = 0; i < RuleExceptionArr.Count; i++)
            {
                int businessRuleId = Convert.ToInt32(RuleExceptionArr[i].BusinessRuleID);
                int userId = Convert.ToInt32(RuleExceptionArr[i].UserID);
                int claimId = Convert.ToInt32(RuleExceptionArr[i].ObjectID);

                if (checkAdjusterSendMail(businessRuleId))
                {
                    exceptionListAdjuster[j] = RuleExceptionArr[i].RuleExceptionID;
                    bool canAdd = true;

                    for (var q = 0; q < formerExceptionListAdjuster.Length; q++)
                    {
                        if (RuleExceptionArr[i].RuleExceptionID == formerExceptionListAdjuster[q])
                        {
                            canAdd = false;
                        }

                    }
                    if (canAdd)
                    {

                        adjusterSendMail(businessRuleId, clientID, userId, claimId);


                    }
                    j = j + 1;
                }

                if (checkSupervisorSendMail(businessRuleId))
                {
                    exceptionListSupervisor[k] = RuleExceptionArr[i].RuleExceptionID;
                    bool canAdd = true;

                    for (var q = 0; q < formerExceptionSupervisor.Length; q++)
                    {
                        if (RuleExceptionArr[i].RuleExceptionID == formerExceptionSupervisor[q])
                        {
                            canAdd = false;
                        }

                    }
                    if (canAdd)
                    {

                        supervisorSendMail(businessRuleId, clientID, userId, claimId);



                    }

                    k = k + 1;
                }

            }

            gvGet.setExceptionListAdjuster(exceptionListAdjuster);
            gvGet.setExceptionListSupervisor(exceptionListSupervisor);


        
        }

        public void setAdjusterSupervisorList()
        {
            var adjusterCount = 0;
            var supervisorCount = 0;

            RuleExceptionManager RuleExceptionManagerObj = new RuleExceptionManager();
            RuleException RuleExceptionObj = new RuleException();
            List<RuleException> RuleExceptionArr = new List<RuleException>();
            Globals gvGet = Globals.Instance();
            int clientID = Convert.ToInt32(gvGet.getClientId());
            RuleExceptionArr = RuleExceptionManagerObj.GetAllException(clientID);




            for (var i = 0; i < RuleExceptionArr.Count; i++)
            {
                int businessRuleId = Convert.ToInt32(RuleExceptionArr[i].BusinessRuleID);
                if (checkAdjusterSendMail(businessRuleId))
                {
                    adjusterCount = adjusterCount + 1;
                }
                if (checkSupervisorSendMail(businessRuleId))
                {
                    supervisorCount = supervisorCount + 1;
                }

            }

            int[] exceptionListAdjuster = new int[adjusterCount];
            int[] formerExceptionListAdjuster = gvGet.getExceptionListAdjuster();

            int[] exceptionListSupervisor = new int[supervisorCount];
            int[] formerExceptionSupervisor = gvGet.getExceptionListSupervisor();

            var j = 0; var k = 0;

            for (var i = 0; i < RuleExceptionArr.Count; i++)
            {
                int businessRuleId = Convert.ToInt32(RuleExceptionArr[i].BusinessRuleID);
                int userId = Convert.ToInt32(RuleExceptionArr[i].UserID);
                int claimId = Convert.ToInt32(RuleExceptionArr[i].ObjectID);

                if (checkAdjusterSendMail(businessRuleId))
                {
                    
                    bool canAdd = true;

                    for (var q = 0; q < formerExceptionListAdjuster.Length; q++)
                    {
                        if (RuleExceptionArr[i].RuleExceptionID == formerExceptionListAdjuster[q])
                        {
                            canAdd = false;
                        }
                      
                           
                       

                    }
                    if (canAdd == true) {
                        exceptionListAdjuster[j] = RuleExceptionArr[i].RuleExceptionID;
                    }
                   
                    j = j + 1;
                }

                if (checkSupervisorSendMail(businessRuleId))
                {
                    
                    bool canAdd = true;

                    for (var q = 0; q < formerExceptionSupervisor.Length; q++)
                    {
                        if (RuleExceptionArr[i].RuleExceptionID == formerExceptionSupervisor[q])
                        {
                            canAdd = false;
                        }
                        
                           
                    
                    }
                    if (canAdd == true) {

                        exceptionListSupervisor[k] = RuleExceptionArr[i].RuleExceptionID;
                    }
                    k = k + 1;
                }

            }

            gvGet.setExceptionListAdjuster(exceptionListAdjuster);
            gvGet.setExceptionListSupervisor(exceptionListSupervisor);


        }



        public void RequestStop()
        {
            _shouldStop = true;
        }
        private volatile bool _shouldStop = false;

        public bool checkAdjusterSendMail(int businessRuleId)
        {
            BusinessRuleManager businessRuleManagerObj = new BusinessRuleManager();
            BusinessRule businessRuleObj = new BusinessRule();
            businessRuleObj = businessRuleManagerObj.GetBusinessRule(businessRuleId);
            if (businessRuleObj.EmailAdjuster && businessRuleObj.IsActive == true)
            {

                return true;
            }
            else
            {
                return false;
            }


        }

        public bool checkSupervisorSendMail(int businessRuleId)
        {
            BusinessRuleManager businessRuleManagerObj = new BusinessRuleManager();
            BusinessRule businessRuleObj = new BusinessRule();
            businessRuleObj = businessRuleManagerObj.GetBusinessRule(businessRuleId);
            if (businessRuleObj.EmailSupervisor && businessRuleObj.IsActive == true)
            {

                return true;
            }
            else
            {
                return false;
            }


        }

        public void adjusterSendMail(int BusinessRuleID, int clientId, int userId, int claimId)
        {
            BusinessRuleManager businessRuleManagerObj = new BusinessRuleManager();
            BusinessRule businessRuleObj = new BusinessRule();
            businessRuleObj = businessRuleManagerObj.GetBusinessRule(BusinessRuleID);

            List<string> emailData = new List<string>();
            emailData = getEmailData(claimId, BusinessRuleID);

            string adjusterEmail = emailData[0];
            string adjusterName = emailData[1];
            string superVisorEmail = emailData[2];
            string superVisorName = emailData[3];
            string adjusterClaimNumber = emailData[4];
            string dateRecieved = emailData[5];
            string businessDescription = emailData[6];
            string redFlagName = emailData[7];
            string insuredName = emailData[8];
            string encryptedClaimNumber = emailData[9];
            StringBuilder mailContent = mail_content(adjusterEmail, adjusterName, superVisorEmail, superVisorName, adjusterClaimNumber, dateRecieved, businessDescription, redFlagName, insuredName, encryptedClaimNumber);
            string subjectString = get_mail_content(adjusterEmail, adjusterName, superVisorEmail, superVisorName, adjusterClaimNumber, dateRecieved, businessDescription, redFlagName, insuredName, encryptedClaimNumber);
        
            if (adjusterEmail != "")
            {
                sendMail(adjusterEmail, adjusterName, mailContent, subjectString);
            }

        }
        public void supervisorSendMail(int BusinessRuleID, int clientId, int userId, int claimId)
        {
            BusinessRuleManager businessRuleManagerObj = new BusinessRuleManager();
            BusinessRule businessRuleObj = new BusinessRule();
            businessRuleObj = businessRuleManagerObj.GetBusinessRule(BusinessRuleID);

            List<string> emailData = new List<string>();
            emailData = getEmailData(claimId, BusinessRuleID);

            string adjusterEmail = emailData[0];
            string adjusterName = emailData[1];
            string superVisorEmail = emailData[2];
            string superVisorName = emailData[3];
            string adjusterClaimNumber = emailData[4];
            string dateRecieved = emailData[5];
            string businessDescription = emailData[6];
            string redFlagName = emailData[7];
            string insuredName = emailData[8];
            string encryptedClaimNumber = emailData[9];
            StringBuilder mailContent = mail_content(superVisorEmail, superVisorName, adjusterEmail, adjusterName, adjusterClaimNumber, dateRecieved, businessDescription, redFlagName, insuredName, encryptedClaimNumber);
            string subjectString = get_mail_content(superVisorEmail, superVisorName, adjusterEmail, adjusterName, adjusterClaimNumber, dateRecieved, businessDescription, redFlagName, insuredName, encryptedClaimNumber);
        
            if (superVisorEmail != "")
            {
                sendMail(superVisorEmail, superVisorName, mailContent, subjectString);

            }


        }
        public void sendMail(string adjusterEmail, string adjusterName, StringBuilder mailContent , string subject)
        {

            var fromAddress = new MailAddress("postmaster@itstrategiesgroup.com", "");
            //var toAddress = new MailAddress(adjusterEmail, adjusterName);
            var toAddress = new MailAddress(adjusterEmail, adjusterName);
            const string fromPassword = "380vq9fgejy4";
            var smtp = new SmtpClient
            {
                Host = "smtp.mailgun.org",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = mailContent.ToString(),
                IsBodyHtml = true
            })
            {
                try { smtp.Send(message); }
                catch (Exception e) { }

            }

        }

        public StringBuilder mail_content(string adjusterEmail, string adjusterName, string superVisorEmail, string superVisorName, string adjusterClaimNumber, string dateRecieved, string businessDescription, string redFlagName, string insuredName, string encryptedClaimNumber)
        {


            StringBuilder emailBody = new StringBuilder();
            emailBody.Append("<div style=\"margin:auto;width:600px;border: 1px solid #DDDDE4; margin: auto;font-family: Tahoma, Arial,sans-serif;font-size: 12px;color: #808080;\">");

            string appUrl = ConfigurationManager.AppSettings["appUrl"].ToString();

            // .paneContentInner
            emailBody.Append("<div style=\"margin: 20px;\">");
            //emailBody.Append("<span style=\"font-weight:bold;\">" + "From:</span> " + "postmaster@itstrategiesgroup.com [mailto:postmaster@itstrategiesgroup.com] " + "<br><br>");
            //emailBody.Append("<span style=\"font-weight:bold;\">" + "Sent:</span> " + Convert.ToString(DateTime.Now) + "<br><br>");
            //emailBody.Append("<span style=\"font-weight:bold;\">" + "To:</span> " + adjusterName + "<br><br>");
            emailBody.Append(insuredName + ",<br><br>");
            emailBody.Append("The following Red Flag was just triggered and requires immediate attention:<br><br>");
            emailBody.Append(redFlagName + "<br><br>");
            emailBody.Append(businessDescription + "<br><br>");
            emailBody.Append(string.Format("<p><a href=\"{0}/login.aspx?url=~/protected/claimedit.aspx?id={1}\">Please click here to access claim.</a></p>", appUrl, encryptedClaimNumber));
            emailBody.Append("Claim #:   " + adjusterClaimNumber + "<br><br>");
            emailBody.Append("Insured Name: " + insuredName + "<br><br>");
            emailBody.Append("Date/Time Received:" + dateRecieved + "<br><br>");
            emailBody.Append("Adjuster:" + adjusterName + "<br><br>");
            emailBody.Append("Supervisor:" + superVisorName + "<br><br>");

            emailBody.Append("</div>");	// containerBox



            return emailBody;
        }

        public string get_mail_content(string adjusterEmail, string adjusterName, string superVisorEmail, string superVisorName, string adjusterClaimNumber, string dateRecieved, string businessDescription, string redFlagName, string insuredName, string encryptedClaimNumber)
        {
            return  "Subject:" + "Claim #" + adjusterClaimNumber + ", Red Flag Triggered, " + redFlagName + ":" + insuredName ;
           
        }

        public List<string> getEmailData(int claimId, int BusinessRuleID)
        {

            List<string> emailData = new List<string>();
            string adjusterEmail = "";

            int adjusterId = 0;
            int superVisorId = 0;
            string adjusterClaimNumber = "";
            string dateRecieved = "";
            int ruleId = 0;
            string businessDescription = "";
            string redFlagName = "";
            string insuredName = "";
            string adjusterName = "";
            ClaimManager ClaimManagerObj = new ClaimManager();
            Claim claimObj = new Claim();
            claimObj = ClaimManagerObj.Get(claimId);
            if (claimObj != null && claimObj.AdjusterID != null)
            {
                adjusterId = Convert.ToInt32(claimObj.AdjusterID);
            }

            if (claimObj != null && claimObj.InsurerClaimNumber != null)
            {
                adjusterClaimNumber = claimObj.InsurerClaimNumber;
                

            }

            if (claimObj != null && claimObj.DateOpenedReported != null)
            {
                dateRecieved = Convert.ToString(claimObj.DateOpenedReported);
            }

            AdjusterMaster adjusterMasterObj = new AdjusterMaster();



            if (claimObj != null) { adjusterMasterObj = claimObj.AdjusterMaster; }
            if (adjusterMasterObj != null && adjusterMasterObj.email != null)
            {
                adjusterEmail = Convert.ToString(adjusterMasterObj.email);
            }

            if (adjusterMasterObj != null && adjusterMasterObj.adjusterName != null)
            {
                adjusterName = Convert.ToString(adjusterMasterObj.adjusterName);
            }




            if (adjusterMasterObj != null && adjusterMasterObj.SupervisorID != null)
            {
                superVisorId = Convert.ToInt32(adjusterMasterObj.SupervisorID);
            }

            CRM.Data.Entities.SecUser secUserObj = new CRM.Data.Entities.SecUser();

            secUserObj = getSecUser(superVisorId);
            insuredName = claimObj.LeadPolicy.Leads.insuredName;

            BusinessRuleManager BusinessRuleManagerObj = new BusinessRuleManager();
            BusinessRule BusinessRuleObj = new BusinessRule();
            BusinessRuleObj = BusinessRuleManagerObj.GetBusinessRule(BusinessRuleID);
            if (BusinessRuleObj.RuleID != null)
            {
                ruleId = Convert.ToInt32(BusinessRuleObj.RuleID);
            };
            businessDescription = BusinessRuleObj.Description;

            CRM.Data.Entities.Rule ruleObj = new CRM.Data.Entities.Rule();
            ruleObj = BusinessRuleManagerObj.GetRule(ruleId);
            redFlagName = ruleObj.RuleName;
            string encryptedClaimNumber = Core.SecurityManager.EncryptQueryString(claimId.ToString());

            emailData.Add(adjusterEmail);
            emailData.Add(adjusterName);
            emailData.Add(secUserObj.Email);
            emailData.Add(secUserObj.UserName);
            emailData.Add(adjusterClaimNumber);
            emailData.Add(dateRecieved);
            emailData.Add(businessDescription);
            emailData.Add(redFlagName);
            emailData.Add(insuredName);
            emailData.Add(encryptedClaimNumber);
            return emailData;
        }

        public CRM.Data.Entities.SecUser getSecUser(int superVisorId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ClaimRuler"].ConnectionString;
            string str_query = @"SELECT * FROM SecUser WHERE UserId = @userId";
            CRM.Data.Entities.SecUser SecUserObj = new CRM.Data.Entities.SecUser();
            using (SqlConnection conn = new SqlConnection(connectionString))

            using (SqlCommand cmd = new SqlCommand(str_query, conn))
            {
                cmd.Parameters.AddWithValue("@userId", superVisorId);


                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["Email"] != null) { SecUserObj.Email = Convert.ToString(reader["Email"]); }
                    if (reader["UserName"] != null) { SecUserObj.UserName = Convert.ToString(reader["UserName"]); }

                }
                conn.Close();
            }
            return SecUserObj;
        }



        public void formatExceptions(int clientID, int userID, int claimId)
        {
            RuleExceptionManager RuleExceptionManagerObj = new RuleExceptionManager();
            RuleExceptionManagerObj.format(clientID, userID, claimId);

        }


        public int checkInsertable(int businessruleId, int clientId, int userId, int claimId)
        {
            RuleExceptionManager RuleExceptionManagerObj = new RuleExceptionManager();
            RuleException RuleExceptionObj = new RuleException();
            int insertState = 0;

            RuleExceptionObj = RuleExceptionManagerObj.GetRuleException(businessruleId, clientId, userId, claimId);

            try
            {
                bool active = Convert.ToBoolean(RuleExceptionObj.IsActive);
                if (active == false) { insertState = 1; } else { insertState = 2; }

            }
            catch (Exception e)
            {

                insertState = 0;
            }




            return insertState;


        }


        public int getTimeFlag(string ruleXml, string value)
        {
            int timeInterval = 0;
            int rulexml = Convert.ToInt32(ruleXml);
            int timeValue = Convert.ToInt32(value);

            if (rulexml == 1)
            {
                timeInterval = timeValue;
            }
            if (rulexml == 2)
            {
                timeInterval = timeValue * 24;
            }
            return timeInterval;

        }
        public int getProgressId(int claimId)
        {
            int progress = 0;
            ClaimManager ClaimManagerobj = new ClaimManager();
            Claim claimObj = new Claim();
            claimObj = ClaimManagerobj.Get(claimId);

            if (claimObj.ProgressStatusID != null)
            {
                progress = Convert.ToInt32(claimObj.ProgressStatusID);

            }
            else
            {
                progress = 0;

            }



            return progress;
        }

        public List<int> getRuleId(int progressId)
        {

            List<int> ruleIds = new List<int>();
            if (checkRuleId1(progressId) == 1) { ruleIds.Add((int)Globals.RuleType.NewClaimAttention); }
            if (checkRuleId2(progressId) == 1) { ruleIds.Add((int)Globals.RuleType.ClaimAssingmentReview); }
            if (checkRuleId3(progressId) == 1) { ruleIds.Add((int)Globals.RuleType.ContactWithInsured); }
            if (checkRuleId4(progressId) == 1) { ruleIds.Add((int)Globals.RuleType.FirstReport); }
            if (checkRuleId5(progressId) == 1) { ruleIds.Add((int)Globals.RuleType.InterimOrFinalReportOrClaimClosing); }
            return ruleIds;
        }
        public DateTime progressClaimRecievedData(int claimId)
        {
            DateTime progressDate = new DateTime();
            ClaimManager ClaimManagerobj = new ClaimManager();
            Claim claimObj = new Claim();
            claimObj = ClaimManagerobj.Get(claimId);

            if (claimObj.ProgressStatusID != null)
            {

                progressDate = Convert.ToDateTime(claimObj.DateOpenedReported);


            }
            else
            {
                progressDate = DateTime.MinValue;
            }

            return progressDate;
        }
        public int checkRuleId1(int progressId)
        {
            int result = 0;
            if (progressId == 16) { result = 1; }
            if (progressId == 0) { result = 1; }
            return result;
        }
        public int checkRuleId2(int progressId)
        {
            int result = 0;
            if (progressId == 16) { result = 1; }
            if (progressId == 1) { result = 1; }
            if (progressId == 0) { result = 1; }
            return result;
        }
        public int checkRuleId3(int progressId)
        {
            int result = 0;
            if (progressId == 16) { result = 1; }
            if (progressId == 1) { result = 1; }
            if (progressId == 2) { result = 1; }
            if (progressId == 0) { result = 1; }
            return result;
        }
        public int checkRuleId4(int progressId)
        {
            int result = 0;
            if (progressId == 16) { result = 1; }
            if (progressId == 1) { result = 1; }
            if (progressId == 2) { result = 1; }
            if (progressId == 3) { result = 1; }
            if (progressId == 4) { result = 1; }
            if (progressId == 5) { result = 1; }
            if (progressId == 0) { result = 1; }
            return result;
        }
        public int checkRuleId5(int progressId)
        {
            int result = 0;
            if (progressId == 16) { result = 1; }
            if (progressId == 1) { result = 1; }
            if (progressId == 2) { result = 1; }
            if (progressId == 3) { result = 1; }
            if (progressId == 4) { result = 1; }
            if (progressId == 5) { result = 1; }
            if (progressId == 6) { result = 1; }
            if (progressId == 0) { result = 1; }
            return result;
        }


        public List<BusinessRule> getBusinessRuleId(int clientID, int ruleId)
        {
            List<BusinessRule> businessRuleArr = new List<BusinessRule>();
            BusinessRuleManager businessRuleManager = new BusinessRuleManager();

            businessRuleArr = businessRuleManager.GetBusinessRuleThread(clientID, ruleId);
            for (var i = 0; i < businessRuleArr.Count; i++)
            {
                XElement ruleXML = XElement.Parse(businessRuleArr[i].RuleXML.ToString());
                using (RuleHelper ruleHelper = new RuleHelper())
                {
                    XElement conditionLapseTime = ruleHelper.GetElement(ruleXML, "LapseTime");
                    businessRuleArr[i].Description = conditionLapseTime.Element("value").Value;

                    XElement conditionLapseTimeType = ruleHelper.GetElement(ruleXML, "LapseTimeType");
                    businessRuleArr[i].RuleXML = conditionLapseTimeType.Element("value").Value;
                }



            }

            return businessRuleArr;
        }

        public void insertRuleException(int BusinessRuleId, int clientID, int userID, int claimId)
        {

            RuleExceptionManager RuleExceptionManagerObj = new RuleExceptionManager();
            RuleException RuleExceptionObj = new RuleException();

            RuleExceptionObj.BusinessRuleID = BusinessRuleId;
            RuleExceptionObj.ClientID = clientID;
            RuleExceptionObj.UserID = userID;
            RuleExceptionObj.ObjectID = claimId;
            RuleExceptionObj.ObjectTypeID = 1;


            RuleExceptionManagerObj.Save(RuleExceptionObj);


        }
        public void updateRuleException(int BusinessRuleId, int clientID, int userID, int claimId)
        {

            RuleExceptionManager RuleExceptionManagerObj = new RuleExceptionManager();
            RuleExceptionManagerObj.update(BusinessRuleId, clientID, userID, claimId);

        }





    }
    public class LoginTrials
    {
        public int LoginId { get; set; }
        public int Trial { get; set; }
        public DateTime TrailTime { get; set; }
    }

}