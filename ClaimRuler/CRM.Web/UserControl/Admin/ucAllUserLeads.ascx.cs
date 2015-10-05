using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Core;
using CRM.Data.Account;
using CRM.Data;
using System.Transactions;
using LinqKit;
using System.Data;
using System.Runtime.Serialization;

using CRM.Repository;
using CRM.Data.Entities;
//using CRM.Data.Account;
using System.Text;

namespace CRM.Web.UserControl.Admin
{
    public partial class ucAllUserLeads : System.Web.UI.UserControl
    {
        int roleID = SessionHelper.getUserRoleId();

        protected Protected.ClaimRuler masterPage = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            masterPage = this.Page.Master as Protected.ClaimRuler;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
           
           
           
            int clientID = SessionHelper.getClientId();

            brnNewClient.Visible = masterPage.hasAddPermission;

            if (!IsPostBack)
            {
                // show open claims
                //rblStatus.SelectedValue = "2";

                loadInactivityPeriod();

                // get id for all status defined as "closed"
                hfSystemClosedStatuses.Value = StatusManager.GetClosedStatuses(clientID);

                // skip search for now
                //bindLeads();

                //hfRoleId.Value = Session["RoleId"].ToString();

                // clear any lead/claim open previous
                Session["LeadIds"] = null;

                bindData();
                bindServiceTypes();
                // default sort - ascending
                ViewState["lastSortExpression"] = "ClaimantLastName";
                ViewState["lastSortDirection"] = true;





                // // check for search request
                checkUserSearchFromMasterPage();
                bindDocumentCategory(ddlDocumentCategory);
                //string search = Convert.ToString(Request.QueryString["search"]);
                //if (search == "all") 
                //{
                //    btnSearch_Click(null, null);
                //}

                string search = Convert.ToString(Request.QueryString["s"]);
                if (search == null)
                {
                    btnSearch_Click(null, null);
                }
            }


        }

        List<LeadView> objLead = null;

        private int bindLeads()
        {
            //gonna have to add something here that says if user role is adjuster, do something different - OC9/4/14
            int resultCount = 0;
            int myClaimID = SessionHelper.getClaimID();  
            var predicate = buildPredicate();

            using (ClaimManager repository = new ClaimManager())
            {
                objLead = repository.Search(predicate);
            }
            
            if (objLead != null && objLead.Count > 0)
            {
                pnlSearchResult.Visible = true;

                gvUserLeads.DataSource = objLead;
                gvUserLeads.DataBind();

                resultCount = objLead.Count;
            }
            else
            {
                gvUserLeads.DataSource = null;
                gvUserLeads.DataBind();
            }

            lblSearchResult.Text = string.Format("{0} claims found.", resultCount);
            lblSearchResult.Visible = true;

            customizeFieldColumns(gvUserLeads);

            updateDashboard();

            return resultCount;
        }

        public List<LeadView> getLeadList()
        {
            List<LeadView> leadArr = new List<LeadView>();

            int myClaimID = SessionHelper.getClaimID();
            var predicate = buildPredicateList();

            using (ClaimManager repository = new ClaimManager())
            {
                leadArr = repository.Search(predicate);
            }



            return leadArr;
        }

        private Expression<Func<vw_Lead_Search, bool>> buildPredicateList()
        {
            AdjusterMaster adjuster = null;
            int adjustID = 0;
            Expression<Func<vw_Lead_Search, bool>> predicate = null;
            int carrierID = 0;
            int carrierIDFromDDL = 0;
            int clientID = 0;
            int policyTypeID = 0;
            int userID = 0;
            int[] statusMasterIDs = null;
            int statusID = 0;
            int subStatusID = 0;
            // int myClaimID = 0;
            CRM.Data.Entities.SecUser secUser = null;

            clientID = SessionHelper.getClientId();
            userID = SessionHelper.getUserId();
            // myClaimID = SessionHelper.getClaimID();

            predicate = PredicateBuilder.True<CRM.Data.Entities.vw_Lead_Search>();

            //var myCLaim = (from x in CRM.Data.Entities.Claim
            //               join p in CRM.Data.Entities.ProgressStatus on x.ProgressStatusID equals p.ProgressStatusID

            //               select new { p.ProgressDescription });
            // mandatory filters

            //predicate = predicate.And(x => x.IsActive == true);					// active claims
            predicate = predicate.And(x => x.ClientID == clientID);	// client leads
            predicate = predicate.And(x => x.Status != 0);			// active lead
            predicate = predicate.And(x => x.UserId != 0);			// user created lead
            // predicate = predicate.And(x => x.ClaimID == myClaimID);




            switch (roleID)
            {
                case (int)UserRole.Administrator:
                    break;

                case (int)UserRole.Client:
                case (int)UserRole.SiteAdministrator:
                    // 2014-03-11
                    //predicate = predicate.And(Lead => Lead.ClientID == clientID);
                    break;

                case (int)UserRole.Adjuster:
                    // get all leads assigned to adjuster
                    adjuster = AdjusterManager.GetAdjusterByUserID(userID);

                    if (adjuster != null)
                    {
                        adjustID = adjuster.AdjusterId;

                        predicate = predicate.And(x => x.AdjusterId == adjustID);
                    }
                    else
                    {
                        predicate = predicate.And(x => x.UserId == userID);
                    }
                    break;

                default:
                    // check account is related to carrier. if so, include those leads whose policy has been assinged this carrier
                    secUser = SecUserManager.GetById(userID);
                    int userRoleID = SessionHelper.getUserRoleId();
                    if (secUser != null && secUser.CarrierID != null)
                    {
                        carrierID = secUser.CarrierID ?? 0;
                        predicate = predicate.And(x => x.CarrierID == carrierID);
                    }
                    else if (secUser.isViewAllClaims ?? false)
                    {
                        // allow user to see all claims
                    }

                    //new, put in place to show claims assigned to the adjuster and ones created by the adjuster - OC 9/5/14
                    else if (secUser.SecRole.RoleName.ToString().Contains("Adjuster") || secUser.SecRole.RoleName.ToString().Contains("adjuster")) // there were too many roles with name adjuster so I set it to 38 across the board so everything is on the same page
                    {
                        adjuster = AdjusterManager.GetAdjusterByUserID(userID);

                        if (adjuster != null)
                        {
                            adjustID = adjuster.AdjusterId;

                            predicate = predicate.And(x => x.AdjusterId == adjustID || x.UserId == userID);
                            //predicate = predicate.And(x => x.UserId == userID);
                        }
                        else
                        {
                            predicate = predicate.And(x => x.UserId == userID);
                        }
                    }
                    //end new
                    else
                    {
                        // get all leads created by user		
                        predicate = predicate.And(x => x.UserId == userID);
                    }
                    break;
            }

            return predicate;


        }


        private void bindData()
        {
            int clientID = SessionHelper.getClientId();
            List<StatusMaster> statusMasters = null;
            List<SubStatusMaster> subStatusMasters = null;
            List<LeadPolicyType> policyTypes = null;

            statusMasters = StatusManager.GetAll(clientID);
            subStatusMasters = SubStatusManager.GetAll(clientID);

            CollectionManager.FillCollection(ddlStatus, "StatusId", "StatusName", statusMasters);
            CollectionManager.FillCollection(ddlSubStatus, "SubStatusId", "SubStatusName", subStatusMasters);

            // load carriers
            List<CarrierView> carriers = CarrierManager.GetAll(clientID);
            CollectionManager.FillCollection(ddlCarrier, "CarrierID", "CarrierName", carriers);

            // load policy type
            policyTypes = LeadPolicyTypeManager.GetAll();
            CollectionManager.FillCollection(ddlPolicyType, "LeadPolicyTypeID", "Description", policyTypes);
            fillClaimStatusReview(clientID);
        }

        private void bindServiceTypes()
        {
            int clientID = SessionHelper.getClientId();
            List<InvoiceServiceType> invoiceServiceTypes = null;

            using (Repository.InvoiceServiceTypeManager repository = new Repository.InvoiceServiceTypeManager())
            {
                invoiceServiceTypes = repository.GetAll(clientID).ToList();
            }

            Core.CollectionManager.FillCollection(ddlInvoiceServiceType, "ServiceTypeID", "ServiceDescription", invoiceServiceTypes);
        }

        protected void btnEdit_OnDataBinding(object sender, EventArgs e)
        {
            // register EditButton inside gridview to cause full postback so we
            // navigate to other pages properly

            // option 1
            //ImageButton ibtnEdit = sender as ImageButton;
            //ScriptManager sm = (ScriptManager)Page.Master.FindControl("ScriptManager1");
            //sm.RegisterPostBackControl(ibtnEdit);

            // option 2
            //AsyncPostBackTrigger trig = new AsyncPostBackTrigger();
            //trig.ControlID = ibtnEdit.UniqueID.ToString(); 
            //trig.EventName ="Click"; 
            //this.updatePanel1.Triggers.Add(trig);
            //ScriptManager sm = (ScriptManager)Page.Master.FindControl("ScriptManager1");
            //sm.RegisterAsyncPostBackControl(ibtnEdit);

        }

        private void checkUserSearchFromMasterPage()
        {
            int adjustID = 0;
            AdjusterMaster adjuster = null;
            int carrierID = 0;
            int clientID = 0;
            int foundCount = 0;
            int userID = 0;
            string keyword = null;
            Expression<Func<vw_Lead_Search, bool>> predicate = null;
            CRM.Data.Entities.SecUser secUser = null;

            if (Request.Params["s"] != null)
            {
                keyword = Request.Params["s"].ToString();

                clientID = SessionHelper.getClientId();
                userID = SessionHelper.getUserId();

                predicate = PredicateBuilder.True<CRM.Data.Entities.vw_Lead_Search>();

                // mandatory filters

                //predicate = predicate.And(x => x.IsActive == true);		// active claims
                predicate = predicate.And(x => x.ClientID == clientID);	// client leads
                predicate = predicate.And(x => x.Status == 1);			// active lead
                //predicate = predicate.And(x => x.UserID != 0);			// user created lead

                predicate = predicate.And(x => x.AdjusterClaimNumber.Contains(keyword) ||
                                        x.ContractorName.Contains(keyword) ||
                                        x.AppraiserName.Contains(keyword) ||
                                        x.UmpireName.Contains(keyword) ||
                                        x.InsuredName.Contains(keyword) ||
                                        x.LossAddress.Contains(keyword) ||
                                        x.PolicyNumber.Contains(keyword) ||
                                        x.InsurerClaimNumber.Contains(keyword)||
                                        x.ProducerName.Contains(keyword));

                //if (DateTime.TryParse(keyword, out fromDate)) {
                //	//predicate = predicate.And(Lead => Lead.OriginalLeadDate >= txtDateFrom.Date);
                //	predicate = predicate.Or(x => x.LossDate >= fromDate);
                //}

                //if (DateTime.TryParse(keyword, out toDate)) {
                //	//predicate = predicate.And(Lead => Lead.OriginalLeadDate <= txtDateTo.Date);
                //	predicate = predicate.Or(x => x.LossDate <= toDate);
                //}





                switch (roleID)
                {
                    case (int)UserRole.Administrator:
                        break;

                    case (int)UserRole.Client:
                    case (int)UserRole.SiteAdministrator:
                        // 2014-03-11
                        //predicate = predicate.And(Lead => Lead.ClientID == clientID);
                        break;

                    case (int)UserRole.Adjuster:
                        // get all leads assigned to adjuster
                        adjuster = AdjusterManager.GetAdjusterByUserID(userID);

                        if (adjuster != null)
                        {
                            adjustID = adjuster.AdjusterId;

                            predicate = predicate.And(x => x.AdjusterId == adjustID);
                        }
                        else
                        {
                            predicate = predicate.And(x => x.UserId == userID);
                        }
                        break;

                    default:
                        // check account is related to carrier. if so, include those leads whose policy has been assinged this carrier
                        secUser = SecUserManager.GetById(userID);
                        if (secUser != null && secUser.CarrierID != null)
                        {
                            carrierID = secUser.CarrierID ?? 0;
                            predicate = predicate.And(x => x.CarrierID == carrierID);
                        }
                        else if (secUser.isViewAllClaims ?? false)
                        {
                            // allow user to see all claims
                        }
                        else
                        {
                            // get all leads created by user		
                            predicate = predicate.And(x => x.UserId == userID);
                        }
                        break;
                }

                // do search
                using (ClaimManager repository = new ClaimManager())
                {
                    objLead = repository.Search(predicate);
                }

                if (objLead != null && objLead.Count > 0)
                {
                    pnlSearchResult.Visible = true;

                    foundCount = objLead.Count;

                    customizeFieldColumns(gvUserLeads);
                }

                gvUserLeads.DataSource = objLead;
                gvUserLeads.DataBind();



                lblSearchResult.Text = string.Format("{0} claims found.", objLead.Count);
                lblSearchResult.Visible = true;

                //updateDashboard();
            }
        }

        protected void linkButton_OnDataBinding(object sender, EventArgs e)
        {
            // register EditButton inside gridview to cause full postback so we
            // navigate to other pages properly
            LinkButton lnkButton = sender as LinkButton;
            ScriptManager sm = (ScriptManager)Page.Master.FindControl("ScriptManager1");
            sm.RegisterPostBackControl(lnkButton);
        }

        private Expression<Func<vw_Lead_Search, bool>> buildPredicate()
        {
            AdjusterMaster adjuster = null;
            int adjustID = 0;
            Expression<Func<vw_Lead_Search, bool>> predicate = null;
            int carrierID = 0;
            int carrierIDFromDDL = 0;
            int clientID = 0;
            int policyTypeID = 0;
            int userID = 0;
            int[] statusMasterIDs = null;
            int statusID = 0;
            int subStatusID = 0;
           // int myClaimID = 0;
            CRM.Data.Entities.SecUser secUser = null;

            clientID = SessionHelper.getClientId();
            userID = SessionHelper.getUserId();
           // myClaimID = SessionHelper.getClaimID();

            predicate = PredicateBuilder.True<CRM.Data.Entities.vw_Lead_Search>();

            //var myCLaim = (from x in CRM.Data.Entities.Claim
            //               join p in CRM.Data.Entities.ProgressStatus on x.ProgressStatusID equals p.ProgressStatusID

            //               select new { p.ProgressDescription });
            // mandatory filters

            //predicate = predicate.And(x => x.IsActive == true);					// active claims
            predicate = predicate.And(x => x.ClientID == clientID);	// client leads
            predicate = predicate.And(x => x.Status != 0);			// active lead
            predicate = predicate.And(x => x.UserId != 0);			// user created lead
           // predicate = predicate.And(x => x.ClaimID == myClaimID);
            if (!String.IsNullOrEmpty(txtSearchClaimNumber.Text))
            {
                predicate = predicate.And(x => x.AdjusterClaimNumber.Contains(txtSearchClaimNumber.Text));
            }

            if (!String.IsNullOrEmpty(txtSearchContractor.Text))
            {
                predicate = predicate.And(x => x.ContractorName.Contains(txtSearchContractor.Text));
            }
            if (!String.IsNullOrEmpty(txtSearchAdjuster.Text))
            {
                predicate = predicate.And(x => x.AdjusterName.Contains(txtSearchAdjuster.Text));
            }
            if (!String.IsNullOrEmpty(txtSearchAppraiser.Text))
            {
                predicate = predicate.And(x => x.AppraiserName.Contains(txtSearchAppraiser.Text));
            }
            if (!String.IsNullOrEmpty(txtSearchUmpire.Text))
            {
                predicate = predicate.And(x => x.UmpireName.Contains(txtSearchUmpire.Text));
            }

            if (!String.IsNullOrEmpty(txtDateFrom.Text))
            {
                //predicate = predicate.And(Lead => Lead.OriginalLeadDate >= txtDateFrom.Date);
                predicate = predicate.And(x => x.LossDate >= txtDateFrom.Date);
            }
            if (!String.IsNullOrEmpty(txtDateTo.Text))
            {
                //predicate = predicate.And(Lead => Lead.OriginalLeadDate <= txtDateTo.Date);
                predicate = predicate.And(x => x.LossDate <= txtDateTo.Date);
            }
            if (!String.IsNullOrEmpty(txtClaimantName.Text))
            {
                predicate = predicate.And(x => x.ClaimantFirstName.Contains(txtClaimantName.Text) ||
                                            x.ClaimantLastName.Contains(txtClaimantName.Text) ||
                                            x.InsuredName.Contains(txtClaimantName.Text));
            }
            if (!String.IsNullOrWhiteSpace(txtClaimantAddress.Text))
            {
                predicate = predicate.And(x => x.LossAddress.Contains(txtClaimantAddress.Text));
            }
            if (!String.IsNullOrEmpty(txtInsurancePolicyNumber.Text))
            {
                predicate = predicate.And(x => x.PolicyNumber.Contains(txtInsurancePolicyNumber.Text));
            }

            if (!String.IsNullOrEmpty(txtSearchProducer.Text))
            {
                predicate = predicate.And(x => x.ProducerName.Contains(this.txtSearchProducer.Text));
            }

            // 2013-10-22
            if (ddlStatus.SelectedIndex > 0)
            {
                statusID = Convert.ToInt32(ddlStatus.SelectedValue);
                predicate = predicate.And(x => x.ClaimStatusID == statusID);
            }

            if (ddlSubStatus.SelectedIndex > 0)
            {
                subStatusID = Convert.ToInt32(ddlSubStatus.SelectedValue);
                predicate = predicate.And(x => x.ClaimSubStatusID == subStatusID);
            }

            // 2014-05-12 tortega
            if (ddlCarrier.SelectedIndex > 0)
            {
                carrierIDFromDDL = Convert.ToInt32(ddlCarrier.SelectedValue);
                predicate = predicate.And(x => x.CarrierID == carrierIDFromDDL);
            }

            // 2014-05-12 tortega
            if (this.ddlPolicyType.SelectedIndex > 0)
            {
                policyTypeID = Convert.ToInt32(ddlPolicyType.SelectedValue);
                predicate = predicate.And(x => x.policyTypeID == policyTypeID);
            }

           

            switch (ddlShowType.SelectedValue)
            {
                case "Open":
                    // show open claims 
                    statusMasterIDs = StatusManager.GetOpenStatusIDs(clientID);

                    // make sure client has defined statuses
                    if (statusMasterIDs != null && statusMasterIDs.Length > 0)
                        predicate = predicate.And(x => statusMasterIDs.Contains<int>(x.ClaimStatusID));
                    break;

                case "Closed":
                    // show closed claims
                    statusMasterIDs = StatusManager.GetClosedStatusIDs(clientID);

                    if (statusMasterIDs != null && statusMasterIDs.Length > 0)
                        predicate = predicate.And(x => statusMasterIDs.Contains<int>(x.ClaimSubStatusID));
                    break;

                default:
                    // all
                    break;
            }



            switch (roleID)
            {
                case (int)UserRole.Administrator:
                    break;

                case (int)UserRole.Client:
                case (int)UserRole.SiteAdministrator:
                    // 2014-03-11
                    //predicate = predicate.And(Lead => Lead.ClientID == clientID);
                    break;

                case (int)UserRole.Adjuster:
                    // get all leads assigned to adjuster
                    adjuster = AdjusterManager.GetAdjusterByUserID(userID);

                    if (adjuster != null)
                    {
                        adjustID = adjuster.AdjusterId;

                        predicate = predicate.And(x => x.AdjusterId == adjustID);
                    }
                    else
                    {
                        predicate = predicate.And(x => x.UserId == userID);
                    }
                    break;

                default:
                    // check account is related to carrier. if so, include those leads whose policy has been assinged this carrier
                    secUser = SecUserManager.GetById(userID);
                    int userRoleID = SessionHelper.getUserRoleId();
                    if (secUser != null && secUser.CarrierID != null)
                    {
                        carrierID = secUser.CarrierID ?? 0;
                        predicate = predicate.And(x => x.CarrierID == carrierID);
                    }
                    else if (secUser.isViewAllClaims ?? false)
                    {
                        // allow user to see all claims
                    }
                    
                    //new, put in place to show claims assigned to the adjuster and ones created by the adjuster - OC 9/5/14
                    else if (secUser.SecRole.RoleName.ToString().Contains("Adjuster") || secUser.SecRole.RoleName.ToString().Contains("adjuster")) // there were too many roles with name adjuster so I set it to 38 across the board so everything is on the same page
                    {
                        adjuster = AdjusterManager.GetAdjusterByUserID(userID);

                        if (adjuster != null)
                        {
                            adjustID = adjuster.AdjusterId;

                            predicate = predicate.And(x => x.AdjusterId == adjustID || x.UserId == userID);
                            //predicate = predicate.And(x => x.UserId == userID);
                        }
                        else
                        {
                            predicate = predicate.And(x => x.UserId == userID);
                        }
                    }
                      //end new
                    else
                    {
                        // get all leads created by user		
                        predicate = predicate.And(x => x.UserId == userID);
                    }
                    break;
            }

            return predicate;
        }

        protected void gvUserLeads_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblError.Visible = false;
            lblError.Text = string.Empty;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;

            if (e.CommandName == "DoView")
            {
                this.Context.Items.Add("selectedleadid", e.CommandArgument.ToString());
                this.Context.Items.Add("view", "A");
                Server.Transfer("~/protected/newlead.aspx");
            }
            if (e.CommandName == "DoEdit")
            {
                //this.Context.Items.Add("selectedleadid", e.CommandArgument.ToString());
                //this.Context.Items.Add("view", "");
                //Server.Transfer("~/protected/admin/newlead.aspx");

                // because of update panels
                // int a = Convert.ToInt32(e.CommandArgument);
                //PUT IN PLACE TO GRAB THE CLAIM ID WHEN THE USER HITS EDIT TO SET THE SESSION VARIABLE TO BE USED IN THE POLLICY GRID
                //TO TAKE CARE OF CLAIM LIMIT STUFF: OC 9/16/14
                Claim claim = null;
                GridViewRow gvRow = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                LinkButton lb = (LinkButton)gvRow.FindControl("lbtnClaim");
              
                string claimNumber = lb.Text;

                if (!string.IsNullOrEmpty(claimNumber))
                {
                    claim = ClaimsManager.Get(claimNumber);

                    if (claim != null)
                    {
                        Session["ClaimID"] = claim.ClaimID;

                    }
                }
                Response.Redirect("~/protected/newlead.aspx?q=" + Core.SecurityManager.EncryptQueryString(e.CommandArgument.ToString()));
            }
            //ADD NOTE
            if(e.CommandName =="AddNotes")
            {
                Claim claim = null;
                int myClaimID = Convert.ToInt32(e.CommandArgument);
                claim = ClaimsManager.Get(myClaimID);

                //Carrier Invoice type for new window
                CarrierInvoiceProfile cip = null;
                int myCIPID = Convert.ToInt32(claim.CarrierInvoiceProfileID);
                cip = CarrierInvoiceProfileManager.Get(myCIPID);
                Session["CarrierType"] = cip.ProfileName;
                
                //Insured Name for new window
                GridViewRow gvRow = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                Label lbl = (Label)gvRow.FindControl("lblInsuredName");
                Session["InsuredName"] = lbl.Text;
                //Leads lead = LeadsManager.GetByLeadId(LeadId);

                //CLient for new window
                GridViewRow gvRow2 = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                Label lblClient = (Label)gvRow.FindControl("lblClient");
                Session["myClient"] = lblClient.Text;

                //Insurer/Branch for new window
                GridViewRow gvRow3 = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                Label lblBranch = (Label)gvRow.FindControl("lblBranch");
                Session["InsurerBranch"] = lblBranch.Text;

                //Net Claim Payable for new window
                decimal d = Convert.ToDecimal(claim.NetClaimPayable);
                Session["ClaimPayable"] = "$" + Math.Round(d, 2);


                if (myClaimID == 0)
                {
                    popUpService.Show();
                    //pnlService.Visible = true;
                }
                else
                {
                    Session["CarrierInvoiceID"] = claim.CarrierInvoiceProfileID;
                    Session["ClaimID"] = e.CommandArgument;
                    Session["ComingFromAllClaims"] = 1;
                    
                    var claimID = Core.SecurityManager.EncryptQueryString(e.CommandArgument.ToString());
                    // Response.Redirect("~/protected/ClaimTimeExpense.aspx?q=" + Core.SecurityManager.EncryptQueryString(e.CommandArgument.ToString()));
                    ScriptManager.RegisterStartupScript(Page, typeof(string), "popup", "window.open('../ClaimTimeExpense.aspx?q=" + claimID + " ' , '_blank');", true);
                    // ScriptManager.RegisterStartupScript(this, typeof(string), "PersonalData", "window.open( 'StudentManager.aspx?id=" + studentid + " ', null,  );", true);
                }
            }
            //ADD EXPENSE
            if (e.CommandName == "AddExpense")
            {
                Claim claim = null;
                int myClaimID = Convert.ToInt32(e.CommandArgument);
                claim = ClaimsManager.Get(myClaimID);
                
                CarrierInvoiceProfile cip = null;
                int myCIPID = Convert.ToInt32(claim.CarrierInvoiceProfileID);
                cip = CarrierInvoiceProfileManager.Get(myCIPID);
                Session["CarrierType"] = cip.ProfileName;

                //Insured Name for new window
                GridViewRow gvRow = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                Label lbl = (Label)gvRow.FindControl("lblInsuredName");
                Session["InsuredName"] = lbl.Text;
                //Leads lead = LeadsManager.GetByLeadId(LeadId);
               
                //CLient for new window
                GridViewRow gvRow2 = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                Label lblClient = (Label)gvRow.FindControl("lblClient");
                Session["myClient"] = lblClient.Text;

                //Insurer/Branch for new window
                GridViewRow gvRow3 = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                Label lblBranch = (Label)gvRow.FindControl("lblBranch");
                Session["InsurerBranch"] = lblBranch.Text;

                //Net Claim Payable for new window
                decimal d = Convert.ToDecimal(claim.NetClaimPayable);
                Session["ClaimPayable"] = "$" + Math.Round(d, 2);

                if (myClaimID == 0)
                {
                    popUpService.Show();
                    //pnlService.Visible = true;
                }
                else
                {
                    Session["CarrierInvoiceID"] = claim.CarrierInvoiceProfileID;
                    Session["ClaimID"] = e.CommandArgument;
                    Session["ComingFromAllClaimsExpense"] = 1;

                    var claimID = Core.SecurityManager.EncryptQueryString(e.CommandArgument.ToString());
                    // Response.Redirect("~/protected/ClaimTimeExpense.aspx?q=" + Core.SecurityManager.EncryptQueryString(e.CommandArgument.ToString()));
                    ScriptManager.RegisterStartupScript(Page, typeof(string), "popup", "window.open('../ClaimTimeExpense.aspx?q=" + claimID + " ' , '_blank');", true);
                    // ScriptManager.RegisterStartupScript(this, typeof(string), "PersonalData", "window.open( 'StudentManager.aspx?id=" + studentid + " ', null,  );", true);
                }
            }


            if (e.CommandName == "DoDelete")
            {
                int LeadId = Convert.ToInt32(e.CommandArgument);
                try
                {
                    var list = LeadsManager.GetByLeadId(LeadId);
                    list.Status = 0;
                    LeadsManager.Save(list);


                    lblSave.Text = "Record Deleted Successfully.";
                    lblSave.Visible = true;
                    bindLeads();
                }
                catch (Exception ex)
                {
                    lblError.Text = "Record Not Deleted.";
                    lblError.Visible = true;
                }
            }
            if (e.CommandName == "DoCopy")
            {
                int LeadId = Convert.ToInt32(e.CommandArgument);
                try
                {

                    Leads lead = LeadsManager.GetByLeadId(LeadId);

                    var list = LeadsManager.GetByLeadId(LeadId);
                    Leads objlead = new Leads();// CRM.Web.Utilities.cln.Clone(list);

                    objlead.ClientID = SessionHelper.getClientId();

                    //objlead.AllDocumentsOnFile = list.AllDocumentsOnFile;
                    objlead.InsuredName = list.InsuredName;
                    objlead.BusinessName = list.BusinessName;
                    objlead.CityId = list.CityId;
                    objlead.ClaimantAddress = list.ClaimantAddress;
                    objlead.ClaimantComments = list.ClaimantComments;
                    objlead.ClaimantFirstName = list.ClaimantFirstName;
                    objlead.ClaimantLastName = list.ClaimantLastName;
                    objlead.ClaimsNumber = list.ClaimsNumber;
                    objlead.DateSubmitted = list.DateSubmitted;
                    objlead.EmailAddress = list.EmailAddress;
                    //objlead.Habitable = list.Habitable;
                    objlead.hasCertifiedInsurancePolicy = list.hasCertifiedInsurancePolicy;
                    objlead.hasContentList = list.hasContentList;
                    objlead.hasDamageEstimate = list.hasDamageEstimate;
                    objlead.hasDamagePhoto = list.hasDamagePhoto;
                    objlead.hasDamageReport = list.hasDamageReport;
                    objlead.hasInsurancePolicy = list.hasInsurancePolicy;
                    objlead.hasOwnerContract = list.hasOwnerContract;
                    objlead.hasSignedRetainer = list.hasSignedRetainer;
                    objlead.HearAboutUsDetail = list.HearAboutUsDetail;
                    objlead.InspectorCell = list.InspectorCell;
                    objlead.InspectorEmail = list.InspectorEmail;
                    objlead.InspectorName = list.InspectorName;
                    //objlead.InsuranceAddress = list.InsuranceAddress;
                    //objlead.InsuranceCity = list.InsuranceCity;
                    //objlead.InsuranceCompanyName = list.InsuranceCompanyName;
                    //objlead.InsuranceContactEmail = list.InsuranceContactEmail;
                    //objlead.InsuranceContactName = list.InsuranceContactName;
                    //objlead.InsuranceContactPhone = list.InsuranceContactPhone;
                    //objlead.InsurancePolicyNumber = list.InsurancePolicyNumber;
                    //objlead.InsuranceState = list.InsuranceState;
                    //objlead.InsuranceZipCode = list.InsuranceZipCode;
                    objlead.IsSubmitted = list.IsSubmitted;
                    objlead.LastContactDate = list.LastContactDate;
                    objlead.LeadSource = list.LeadSource;
                    //objlead.LeadStatus = list.LeadStatus;
                    objlead.LossAddress = list.LossAddress;
                    objlead.MarketValue = list.MarketValue;
                    objlead.OriginalLeadDate = list.OriginalLeadDate;
                    objlead.OtherSource = list.OtherSource;
                    objlead.OwnerFirstName = list.OwnerFirstName;
                    objlead.OwnerLastName = list.OwnerLastName;
                    objlead.OwnerPhone = list.OwnerPhone;
                    objlead.OwnerSame = list.OwnerSame;
                    objlead.PhoneNumber = list.PhoneNumber;
                    objlead.PrimaryProducerId = list.PrimaryProducerId;
                    objlead.PropertyDamageEstimate = list.PropertyDamageEstimate;
                    objlead.ReporterToInsurer = list.ReporterToInsurer;
                    objlead.SecondaryEmail = list.SecondaryEmail;
                    objlead.SecondaryPhone = list.SecondaryPhone;
                    objlead.SecondaryProducerId = list.SecondaryProducerId;
                    objlead.SiteInspectionCompleted = list.SiteInspectionCompleted;
                    objlead.SiteLocation = list.SiteLocation;
                    objlead.SiteSurveyDate = list.SiteSurveyDate;
                    objlead.StateId = list.StateId;
                    objlead.Status = list.Status;
                    //objlead.SubStatus = list.SubStatus;
                    objlead.TypeOfDamage = list.TypeOfDamage;
                    objlead.TypeofDamageText = list.TypeofDamageText;
                    objlead.TypeOfProperty = list.TypeOfProperty;
                    objlead.UserId = list.UserId;
                    objlead.WebformSource = list.WebformSource;
                    objlead.Zip = list.Zip;

                    objlead.LossAddress2 = list.LossAddress2;
                    objlead.LossDate = list.LossDate;
                    objlead.LossLocation = list.LossLocation;

                    objlead.StateName = list.StateName;
                    objlead.CityName = list.CityName;

                    objlead = LeadsManager.Save(objlead);


                    copyPolicy(LeadId, objlead.LeadId);

                    copyInvoices(LeadId, objlead.LeadId);

                    copyLienholders(LeadId, objlead.LeadId);

                    copyComments(LeadId, objlead.LeadId);

                    copyContacts(LeadId, objlead.LeadId);

                    lblSave.Text = "Record Copied Successfully.";
                    lblSave.Visible = true;
                    bindLeads();
                }
                catch (Exception ex)
                {
                    lblError.Text = "Record Not Copied.";
                    lblError.Visible = true;
                }
            }
        }

        protected void copyInvoices(int sourceLeadID, int targetLeadID)
        {
            int clientID = Core.SessionHelper.getClientId();

            List<Invoice> invoices = InvoiceManager.GetAll(sourceLeadID).ToList();

            //if (invoices != null && invoices.Count > 0) {
            //	foreach (Invoice invoice in invoices) {
            //		Invoice newInvoice = new Invoice();
            //		newInvoice.LeadId = targetLeadID;
            //		newInvoice.PolicyTypeID = invoice.PolicyTypeID;

            //		newInvoice.InvoiceNumber = InvoiceManager.GetNextInvoiceNumber(clientID);
            //		newInvoice.AdjusterID = invoice.AdjusterID;
            //		newInvoice.AdjusterInvoiceNumber = invoice.AdjusterInvoiceNumber;
            //		newInvoice.BillToAddress1 = invoice.BillToAddress1;
            //		newInvoice.BillToAddress2 = invoice.BillToAddress2;
            //		newInvoice.BillToAddress3 = invoice.BillToAddress3;
            //		newInvoice.BillToName = invoice.BillToName;
            //		newInvoice.ClientID = invoice.ClientID;
            //		newInvoice.Comments = invoice.Comments;
            //		newInvoice.DueDate = invoice.DueDate;
            //		newInvoice.InvoiceDate = invoice.InvoiceDate;
            //		newInvoice.isVoid = invoice.isVoid;
            //		newInvoice.TaxRate = invoice.TaxRate;
            //		newInvoice.TotalAmount = invoice.TotalAmount;

            //		int invoiceID = InvoiceManager.Save(newInvoice);

            //		if (invoice.LeadInvoiceDetails != null && invoice.LeadInvoiceDetails.Count > 0) {
            //			foreach (LeadInvoiceDetail detail in invoice.LeadInvoiceDetails) {
            //				LeadInvoiceDetail newDetail = new LeadInvoiceDetail();
            //				newDetail.InvoiceID = invoiceID;
            //				newDetail.Comments = detail.Comments;
            //				newDetail.isBillable = detail.isBillable;
            //				newDetail.LineAmount = detail.LineAmount;
            //				newDetail.LineDate = detail.LineDate;
            //				newDetail.LineDescription = detail.LineDescription;
            //				newDetail.LineItemNo = detail.LineItemNo;
            //				newDetail.Qty = detail.Qty;
            //				newDetail.Rate = detail.Rate;
            //				newDetail.ServiceTypeID = detail.ServiceTypeID;
            //				newDetail.Total = detail.Total;
            //				newDetail.UnitDescription = detail.UnitDescription;
            //				newDetail.UnitID = detail.UnitID;

            //				LeadInvoiceDetailManager.Save(newDetail);

            //			}
            //		}
            //	}
            //}
        }

        protected void copyPolicy(int sourceLeadID, int targetLeadID)
        {
            List<CRM.Data.Entities.LeadPolicy> policies = LeadPolicyManager.GetPolicies(sourceLeadID);
            if (policies != null && policies.Count > 0)
            {
                foreach (CRM.Data.Entities.LeadPolicy policy in policies)
                {
                    CRM.Data.Entities.LeadPolicy newPolicy = new CRM.Data.Entities.LeadPolicy();
                    newPolicy.LeadId = targetLeadID;

                    newPolicy.AdjusterID = policy.AdjusterID;
                    newPolicy.CarrierID = policy.CarrierID;
                    newPolicy.ClaimNumber = policy.ClaimNumber;
                    newPolicy.FaxNumber = policy.FaxNumber;
                    newPolicy.InsuranceAddress = policy.InsuranceAddress;
                    newPolicy.InsuranceCity = policy.InsuranceCity;
                    newPolicy.InsuranceCompanyName = policy.InsuranceCompanyName;
                    newPolicy.InsuranceState = policy.InsuranceState;
                    newPolicy.InsuranceZipCode = policy.InsuranceZipCode;
                    newPolicy.InsurerFileNo = policy.InsurerFileNo;
                    newPolicy.IsActive = policy.IsActive;
                    newPolicy.isAllDocumentUploaded = policy.isAllDocumentUploaded;
                    newPolicy.LastStatusUpdate = policy.LastStatusUpdate;
                    newPolicy.LeadStatus = policy.LeadStatus;
                    newPolicy.LoanNumber = policy.LoanNumber;
                    newPolicy.LossDate = policy.LossDate;
                    newPolicy.LossLocation = policy.LossLocation;
                    newPolicy.MortgageeID = policy.MortgageeID;
                    newPolicy.PhoneNumber = policy.PhoneNumber;
                    newPolicy.PolicyNumber = policy.PolicyNumber;
                    newPolicy.PolicyPeriod = policy.PolicyPeriod;
                    newPolicy.PolicyType = policy.PolicyType;
                    newPolicy.SiteInspectionCompleted = policy.SiteInspectionCompleted;
                    newPolicy.SiteSurveyDate = policy.SiteSurveyDate;
                    newPolicy.SubStatus = policy.SubStatus;

                    int newPolicyID = LeadPolicyManager.Save(newPolicy);

                    //copyCoverages(newPolicyID, policy);					
                }

            }

        }



        protected void copyLienholders(int sourceLeadID, int targetLeadID)
        {
            //List<LeadPolicyLienholder> lienholders = LienholderManager.GetAll(sourceLeadID);
            //if (lienholders != null && lienholders.Count > 0) {
            //	foreach (LeadPolicyLienholder lienholder in lienholders) {
            //		LeadPolicyLienholder newLienHolder = new LeadPolicyLienholder();
            //		newLienHolder.LeadID = targetLeadID;
            //		newLienHolder.MortgageeID = lienholder.MortgageeID;

            //		LienholderManager.Save(newLienHolder);
            //	}
            //}

        }

        //protected void copyCoverages(int targetPolicyID, LeadPolicy policy) {
        //	if (policy.LeadPolicyCoverages != null && policy.LeadPolicyCoverages.Count > 0) {
        //		foreach (LeadPolicyCoverage coverage in policy.LeadPolicyCoverages) {
        //			LeadPolicyCoverage newCoverage = new LeadPolicyCoverage();
        //			newCoverage.LeadPolicyID = targetPolicyID;
        //			newCoverage.Deductible = coverage.Deductible;
        //			newCoverage.Description = coverage.Description;
        //			newCoverage.Limit = coverage.Limit;

        //			LeadPolicyCoverageManager.Save(newCoverage);
        //		}
        //	}
        //}

        protected void copyComments(int sourceLeadID, int targetLeadID)
        {
            List<LeadComment> comments = LeadCommentManager.getLeadCommentByLeadID(sourceLeadID);
            if (comments != null && comments.Count > 0)
            {
                foreach (LeadComment comment in comments)
                {
                    LeadComment newComment = new LeadComment();
                    newComment.CommentText = comment.CommentText;
                    newComment.InsertBy = comment.InsertBy;
                    newComment.InsertDate = comment.InsertDate;
                    newComment.LeadId = targetLeadID;
                    newComment.PolicyType = comment.PolicyType;
                    newComment.UserId = comment.UserId;
                    newComment.Status = comment.Status;
                    newComment.ReferenceID = comment.ReferenceID;

                    LeadCommentManager.Save(newComment);
                }
            }
        }

        protected void copyContacts(int sourceLeadID, int targetLeadID)
        {
            List<LeadContact> contacts = LeadContactManager.GetContactByLeadID(sourceLeadID);
            if (contacts != null && contacts.Count > 0)
            {
                foreach (LeadContact contact in contacts)
                {
                    LeadContact newContact = new LeadContact();
                    newContact.ContactName = contact.ContactName;
                    newContact.CityID = contact.CityID;
                    newContact.CompanyAddress = contact.CompanyAddress;
                    newContact.CompanyName = contact.ContactName;
                    newContact.ContactTypeID = contact.ContactTypeID;
                    newContact.DateCreated = contact.DateCreated;
                    newContact.Email = contact.Email;
                    newContact.InsuranceTypeID = contact.InsuranceTypeID;
                    newContact.isActive = contact.isActive;
                    newContact.LeadID = targetLeadID;
                    newContact.Mobile = contact.Mobile;
                    newContact.Phone = contact.Phone;
                    newContact.PolicyTypeID = contact.PolicyTypeID;
                    newContact.StateID = contact.StateID;
                    newContact.ZipCode = contact.ZipCode;

                    LeadContactManager.Save(newContact);
                }
            }
        }

        protected void gvUserLeads_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            List<LeadView> results = null;

            gvUserLeads.PageIndex = e.NewPageIndex;

            string sortExpression = ViewState["lastSortExpression"] == null ? "ClaimantLastName" : ViewState["lastSortExpression"].ToString();
            bool descending = ViewState["lastSortDirection"] == null ? false : (bool)ViewState["lastSortDirection"];

            Expression<Func<vw_Lead_Search, bool>> predicate = buildPredicate();

            using (ClaimManager repository = new ClaimManager())
            {
                results = repository.Search(predicate, sortExpression, descending);
            }

            gvUserLeads.DataSource = results;
            gvUserLeads.DataBind();
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
            lblMessage.Text = string.Empty;
            lblMessage.Visible = false;
            lblSave.Text = string.Empty;
            lblSave.Visible = false;

            //hfToDate.Value = txtDateTo.Text.Trim();
            //hfFromDate.Value = txtDateFrom.Text.Trim();
            //hfCriteria.Value = txtSearch.Text.Trim();
            //hfClaimantName.Value = txtClaimantName.Text.Trim();
            //hfClaimantAddress.Value = txtClaimantAddress.Text.Trim();
            //hfPolicyNumber.Value = txtInsurancePolicyNumber.Text.Trim();
            //hfSearchClaimNumber.Value = txtSearchClaimNumber.Text.Trim();

            int resultCount = bindLeads();
            if (resultCount > 0)
            {
                pnlSearchResult.Visible = true;
                pnlSearch.Visible = false;
            }
            lblSearchResult.Text = string.Format("{0} claims found.", resultCount);
            lblSearchResult.Visible = true;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //hfToDate.Value = "";
            txtDateTo.Text = string.Empty;
            //hfFromDate.Value = "";
            txtDateFrom.Text = string.Empty;
            txtClaimantAddress.Text = string.Empty;
            txtClaimantName.Text = string.Empty;
            txtInsurancePolicyNumber.Text = string.Empty;
            //hfClaimantAddress.Value = "";
            //hfClaimantName.Value = "";
            //hfPolicyNumber.Value = "";
            //txtSearch.Text = string.Empty;
            //hfCriteria.Value = "";

            ddlStatus.SelectedIndex = -1;
            ddlSubStatus.SelectedIndex = -1;

            bindLeads();

        }

        private bool checkForClosedStatus(string claimStatus)
        {
            bool isClosed = false;
            string[] systemClosedStatuses = null;
            string[] claimStatuses = null;

            if (!string.IsNullOrEmpty(hfSystemClosedStatuses.Value) && !string.IsNullOrEmpty(claimStatus))
            {
                systemClosedStatuses = hfSystemClosedStatuses.Value.Split(',');

                claimStatuses = claimStatus.Split(',');

                if (claimStatuses.Length > 0)
                {
                    foreach (string status in claimStatuses)
                        if (systemClosedStatuses.Contains(status))
                        {
                            isClosed = true;
                            break;
                        }
                }
            }

            return isClosed;
        }

        protected void gvUserLeads_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            string[] claimNumbers = null;
            TimeSpan inactivityDays;
            bool isClosed = false;
            LeadView leadView = null;
            int roleID = 0;


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                leadView = (LeadView)e.Row.DataItem;

                // determine lead exceeds inactivity period
                if (leadView != null && leadView.LastActivityDate != null && !string.IsNullOrEmpty(hfInactivityPeriod.Value))
                {
                    inactivityDays = DateTime.Now - (DateTime)leadView.LastActivityDate;

                    // check inactivity period was exceeded	
                    if (inactivityDays.TotalDays >= Convert.ToInt32(hfInactivityPeriod.Value))
                    {
                        // get lead current policy statuses
                        HiddenField hfClaimStatusCodes = e.Row.FindControl("hfClaimStatusCodes") as HiddenField;

                        // check claim status is closed
                        if (!string.IsNullOrEmpty(hfClaimStatusCodes.Value))
                            isClosed = checkForClosedStatus(hfClaimStatusCodes.Value);

                        Image imgInactivityFlag = e.Row.FindControl("imgInactivityFlag") as Image;

                        // show inactivity flag when claim is not closed and inactivity period has been exceeeded
                        if (imgInactivityFlag != null && !isClosed)
                            imgInactivityFlag.Visible = true;

                    }
                }

                // moved role management
                //LinkButton lnkDelete = e.Row.FindControl("lnkDelete") as LinkButton;

                //if (lnkDelete != null) {
                //	roleID = SessionHelper.getUserRoleId();

                //	if (roleID == (int)UserRole.Administrator)
                //		lnkDelete.Visible = true;
                //	else if ((roleID == (int)UserRole.Client || roleID == (int)UserRole.SiteAdministrator) && Session["ClientId"] != null)
                //		lnkDelete.Visible = true;
                //}

                // link to claim page
                if (!string.IsNullOrEmpty(leadView.ClaimNumber))
                {
                    LinkButton lbtnClaim = e.Row.FindControl("lbtnClaim") as LinkButton;
                    lbtnClaim.Text = leadView.ClaimNumber;
                }

                Label lblCauseOfLoss = e.Row.FindControl("lblCauseOfLoss") as Label;
                if (lblCauseOfLoss != null && !string.IsNullOrEmpty(leadView.TypeofDamageText))
                {
                    string[] damageDescriptions = TypeofDamageManager.GetDescriptions(leadView.TypeofDamageText);
                    lblCauseOfLoss.Text = string.Join("<br>", damageDescriptions);
                }

                //// loss date
                //Repeater rptLossDate = e.Row.FindControl("rptLossDate") as Repeater;
                //var lossDates = (from x in leadView.LossDates select new { LossDate = x }).ToArray();
                //rptLossDate.DataSource = lossDates;
                //rptLossDate.DataBind();



            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }

        protected void gvUserLeads_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<LeadView> results = null;
            bool descending = false;

            if (ViewState[e.SortExpression] == null)
                descending = false;
            else
                descending = !(bool)ViewState[e.SortExpression];

            ViewState[e.SortExpression] = descending;

            ViewState["lastSortExpression"] = e.SortExpression;
            ViewState["lastSortDirection"] = descending;

            Expression<Func<vw_Lead_Search, bool>> predicate = buildPredicate();

            using (ClaimManager repository = new ClaimManager())
            {
                results = repository.Search(predicate, e.SortExpression, descending);
            }

            gvUserLeads.DataSource = results;
            gvUserLeads.DataBind();

        }

        protected void gvTasks_onSorting(object sender, GridViewSortEventArgs e)
        {
            System.Web.UI.WebControls.GridView gridView = sender as System.Web.UI.WebControls.GridView;
            int userID = 0;

            IQueryable<LeadTask> tasks = null;

            int roleID = SessionHelper.getUserRoleId();

            int clientID = SessionHelper.getClientId();

            DateTime fromDate = DateTime.Today;
            DateTime toDate = DateTime.Today;

            if (roleID == (int)UserRole.Administrator)
            {
                // load tasks for user "Admin". Admin has not clientid associated with it.
                tasks = TasksManager.GetLeadTask(fromDate, toDate);
            }
            else if ((roleID == (int)UserRole.Client || roleID == (int)UserRole.SiteAdministrator) && clientID > 0)
            {
                // load all tasks for client (sort of admin)				
                tasks = TasksManager.GetLeadTask(clientID, fromDate, toDate);
            }
            else
            {
                userID = SessionHelper.getUserId();
                tasks = TasksManager.GetLeadTaskByUserID(userID, fromDate, toDate);
            }

            bool descending = false;

            if (ViewState[e.SortExpression] == null)
                descending = false;
            else
                descending = !(bool)ViewState[e.SortExpression];

            ViewState[e.SortExpression] = descending;

            gridView.DataSource = tasks.orderByExtension(e.SortExpression, descending);

            gridView.DataBind();

        }

        protected void customizeFieldColumns(System.Web.UI.WebControls.GridView gridView)
        {
            DataControlField gridColumn = null;

            int clientID = SessionHelper.getClientId();

            List<vw_FieldColumn> columns = FieldColumnManager.GetAll(clientID);

            if (columns != null && columns.Count > 0)
            {
                foreach (vw_FieldColumn column in columns)
                {
                    gridColumn = (DataControlField)gridView.Columns
                                .Cast<DataControlField>()
                                .Where(fld => (fld.HeaderText == column.ColumnName))
                                .SingleOrDefault();

                    if (gridColumn != null)
                        gridColumn.Visible = column.isVisible;
                }
            }
        }


        protected void loadInactivityPeriod()
        {
            int clientID = SessionHelper.getClientId();

            Client client = ClientManager.Get(clientID);
            if (client != null)
            {
                hfInactivityPeriod.Value = client.InactivityDays == null ? "0" : client.InactivityDays.ToString();
            }
        }

        public SortDirection SortDirection
        {
            get
            {
                if (ViewState["SortDirection"] == null)
                {
                    ViewState["SortDirection"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["SortDirection"];
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        protected void btnRefreshTasks_Click(object sender, EventArgs e)
        {
            DateTime fromDate = DateTime.Now;

            DateTime.TryParse(hf_taskDate.Value, out fromDate);

            //bindTasks(fromDate.Date, fromDate.Date);
        }

        protected void updateDashboard()
        {
            int clientID = SessionHelper.getClientId();
            int openCount = 0;
            int closeCount = 0;

            if (clientID > 0)
            {
                openCount = LeadsManager.GetOpenLeadCount(clientID);

                closeCount = LeadsManager.GetCloseLeadCount(clientID);

                lblOpenLeadCount.Text = string.Format("{0:n0}", openCount);

                lblCloseLeadCount.Text = string.Format("{0:n0}", closeCount);
            }
        }

        protected void brnNewClient_Click(object sender, EventArgs e)
        {
            Session.Remove("LeadIds");

            Response.Redirect("~/Protected/NewLead.aspx");
        }

        protected void rptClaim_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string claimNumber = null;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbtnClaim = e.Item.FindControl("lbtnClaim") as LinkButton;
                claimNumber = e.Item.DataItem as string;

                lbtnClaim.Text = claimNumber;
                lbtnClaim.CssClass = "link";

            }
        }

        protected void lbtnClaim_Click(object sender, EventArgs e)
        {
            Claim claim = null;
            string url = "~/Protected/ClaimEdit.aspx";
            string claimNumber = ((LinkButton)(sender)).Text;
            Session["ComingFromAllClaims"] = null; //clear out the session variable for the "addNotes"
            Session["ComingFromAllClaimsExpense"] = null;
            if (!string.IsNullOrEmpty(claimNumber))
            {
                claim = ClaimsManager.Get(claimNumber);

                if (claim != null)
                {
                    Session["LeadIds"] = claim.LeadPolicy.LeadId;
                    Session["policyID"] = claim.LeadPolicy.Id;
                    Session["ClaimID"] = claim.ClaimID;
                    
                    Response.Redirect(url);
                }
            }
        }
        //NEW OC 9/19/2014: for insurer claim number field to pull up correect claim when clicked
        protected void lbtnClaim_Click2(object sender, EventArgs e)
        {
            Claim claim = null;
            string url = "~/Protected/ClaimEdit.aspx";
            string InsurerClaimNumber = ((LinkButton)(sender)).Text;
            Session["ComingFromAllClaims"] = null; //clear out the session variable for the "addNotes" OC 10202014
            Session["ComingFromAllClaimsExpense"] = null;
            if (!string.IsNullOrEmpty(InsurerClaimNumber))
            {
                claim = ClaimsManager.Get2(InsurerClaimNumber);

                if (claim != null)
                {
                    Session["LeadIds"] = claim.LeadPolicy.LeadId;
                    Session["policyID"] = claim.LeadPolicy.Id;
                    Session["ClaimID"] = claim.ClaimID;

                    Response.Redirect(url);
                }
            }
        }

        protected void lbtrnSearchPanel_Click(object sender, EventArgs e)
        {
            pnlSearch.Visible = true;
            pnlSearchResult.Visible = false;
        }

        protected void lbtnClientDelete_Click(object sender, EventArgs e)
        {
            int delCountTrue = 0;
            int delCountFalse = 0;
            foreach (GridViewRow row in gvUserLeads.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSelect = row.FindControl("chkSelectLeads") as CheckBox;
                    if (chkSelect.Checked)
                    {
                        HiddenField hdnField = row.FindControl("hdnSelectLeads") as HiddenField;
                        int LeadId = Convert.ToInt32(hdnField.Value);
                        try
                        {
                            var list = LeadsManager.GetByLeadId(LeadId);
                            list.Status = 0;
                            LeadsManager.Save(list);
                            delCountTrue = delCountTrue + 1;
                        }
                        catch (Exception ex)
                        {
                            delCountFalse = delCountFalse + 1;
                        }
                    }
                }
            }

            if (delCountTrue == 0 && delCountFalse == 0)
            {
                lblSave.Visible = true;
                lblSave.Text = "Please Select Any Record";
            }
           else if (delCountTrue > 0)
            {
                lblSave.Visible = true;
                lblSave.Text = delCountTrue + " Records Deleted Successfully.";
            }

            if (delCountFalse > 0)
            {
                lblError.Visible = true;
                lblError.Text = delCountFalse + " Record Not Deleted.";

            }
           
            bindLeads();
        }


        protected void fillClaimStatusReview(int clientd)
        {
            ClaimManager objClaimManager = new ClaimManager();

            List<Carrier> objCarrier = new List<Carrier>();
            objCarrier = objClaimManager.GetAllCarrier(clientd);
            

            List<ContactList> objContactlist = new List<ContactList>();

            List<Contact> listContact = ContactManager.GetAll(clientd).ToList();

            List<StatusMaster> statusMasters = null;

            List<ExpenseType> expenseTypes = null;

            ContactList objcontact1;
            foreach (Contact data in listContact)
            {
                objcontact1 = new ContactList();
                objcontact1.ContactID = data.ContactID;
                objcontact1.FirstName = data.FirstName;
                objcontact1.LastName = data.LastName;
                objcontact1.Email = data.Email;
                objcontact1.CompanyName = data.CompanyName;
                objcontact1.IdOf = "c";
                objContactlist.Add(objcontact1);
            }
            List<AdjusterMaster> listAdjuster = CRM.Data.Account.AdjusterManager.GetAll(clientd).ToList();
            foreach (AdjusterMaster data in listAdjuster)
            {
                objcontact1 = new ContactList();
                objcontact1.ContactID = data.AdjusterId;
                objcontact1.FirstName = data.FirstName;
                objcontact1.LastName = data.LastName;
                objcontact1.Email = data.email;
                objcontact1.CompanyName = data.CompanyName;
                objcontact1.IdOf = "a";
                objContactlist.Add(objcontact1);
            }
            gvSelectRecipients.DataSource = objContactlist.AsQueryable();
            gvSelectRecipients.DataBind();


            gvSelectRecipientsStatus.DataSource = objContactlist.AsQueryable();
            gvSelectRecipientsStatus.DataBind();

            gvSelectRecipientsExpense.DataSource = objContactlist.AsQueryable();
            gvSelectRecipientsExpense.DataBind();


            statusMasters = StatusManager.GetList(clientd);
            if (statusMasters!=null)
            {
            CollectionManager.FillCollection(ddlClaimStatusReview, "StatusId", "StatusName", statusMasters);
            }
            if (objCarrier!=null)
            {
            CollectionManager.FillCollection(ddlClaimCarrier, "CarrierID", "CarrierName", objCarrier);
            CollectionManager.FillCollection(ddlExpenseClaimCarrier, "CarrierID", "CarrierName", objCarrier);
            }

            Client objClient = ClaimsManager.GetClientByUserId(SessionHelper.getUserId());
            if (objClient!=null)
            {
                txtAdjusterComapnyName.Text = objClient.BusinessName;
                txtExpenseAdjusterComapnyName.Text = objClient.BusinessName;
            }

            using (ExpenseTypeManager repository = new ExpenseTypeManager())
            {
                expenseTypes = repository.GetAll(clientd).ToList();
            }
            if (expenseTypes!=null)
            {
            Core.CollectionManager.FillCollection(ddlExpenseType, "ExpenseTypeID", "ExpenseName", expenseTypes);
            }




        }

        private void bindDocumentCategory(DropDownList ddl)
        {
            List<DocumentCategory> documentCategories = null;

            using (DocumentCategoryManager repository = new DocumentCategoryManager())
            {
                documentCategories = repository.GetAll();
            }

            Core.CollectionManager.FillCollection(ddl, "DocumentCategoryID", "CategoryName", documentCategories);
        }

        protected void imgbtnNotes_Click(object sender, ImageClickEventArgs e)
        {
            //NEW CODE FOR REDIRECT TO T&E PAGE
           // Response.Redirect("~/Protected/ClaimTimeExpense.aspx");
            //var claimID = 
            //string url = "~/ClaimTimeExpense.aspx";
            //ScriptManager.RegisterStartupScript(Page, typeof(string), "popup", "window.open('../ClaimTimeExpense.aspx', '_blank')", true);
             //ScriptManager.RegisterStartupScript(this, typeof(string), "popup", "window.open('ClaimTimeExpense.aspx','_blank')", true);
           
            //var studentid = e.CommandArgument.ToString();
           // ScriptManager.RegisterStartupScript(this, typeof(string), "PersonalData", "window.open( 'StudentManager.aspx?id=" + studentid + " ', null, 'height=700,width=1200,status=yes,toolbar=yes,menubar=yes,location=yes,resizable=yes,scrollbars=yes' );", true);
        }

        //public static void notifyUser(int claimStatus, string InsurerClaimId, string InsurerName, int ClaimAdjusterId, string AdjusterComapnyName, string Updatedby, string CommentNote, string EmailTo, int carrierID, int claimid, string recipient, string ClaimAdjuster, string claimStatusName, string carrier, string[] recipId, string[] IdofTable)
        //{

        //    StringBuilder emailBody = new StringBuilder();
        //    string password = null;
        //    string[] recipients = null;
        //    string smtpHost = null;
        //    int smtpPort = 0;
        //    int supervisorId = 0;
        //    AdjusterMaster objAdjusterMaster = null;
        //    CRM.Data.Entities.SecUser objSecUser = null;
        //    //string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
        //    string subject = "Please Read Note, Claim Ruler Status Change to: " + claimStatusName + " on Claim # " + InsurerClaimId;
        //    CRM.Data.Entities.SecUser user = null;

        //    // get logged in user
        //    int userID = SessionHelper.getUserId();
        //    // get logged in user email info
        //    user = SecUserManager.GetByUserId(userID);
        //    // load email credentials
        //    smtpHost = user.emailHost;
        //    string appurl = ConfigurationManager.AppSettings["appURL"].ToString();

        //    int.TryParse(user.emailHostPort, out smtpPort);

        //    recipient = EmailTo != string.Empty && recipient != string.Empty ? EmailTo + "," + recipient : recipient;
        //    recipient = EmailTo != string.Empty && recipient == string.Empty ? EmailTo : recipient;

        //    objAdjusterMaster = new AdjusterMaster();
        //    objAdjusterMaster = AdjusterManager.GetAdjusterId(ClaimAdjusterId);
        //    // code for add adjuster and supervisor email for add in recipients
        //    if (user.Email != string.Empty)
        //    {
        //        recipient = recipient + "," + user.Email;
        //    }
        //    if (!string.IsNullOrEmpty(objAdjusterMaster.email))
        //    {
        //        recipient = recipient + "," + objAdjusterMaster.email;
        //    }
        //    if (!string.IsNullOrEmpty(Convert.ToString(objAdjusterMaster.SupervisorID)))
        //    {
        //        SecUserManager.GetByUserId(objAdjusterMaster.SupervisorID ?? 0);
        //        objSecUser = new Data.Entities.SecUser();
        //        if (!string.IsNullOrEmpty(objSecUser.Email))
        //        {
        //            recipient = recipient + "," + objSecUser.Email;
        //        }
        //    }

        //    // recipients
        //    recipients = new string[] { };
        //    recipients = recipient.Split(',');
        //    // build email body
        //    // .containerBox
        //    emailBody.Append("<div>");
        //    emailBody.Append("<div>");
        //    emailBody.Append("Hi " + ClaimAdjuster + ",<br><br>");

        //    emailBody.Append("<table>");
        //    emailBody.Append("<tr><td style='width:100px;'></td>");
        //    emailBody.Append("<td>");
        //    emailBody.Append("<b>Update Claim Status/Review</b><br/>");
        //    emailBody.Append("</td>");
        //    emailBody.Append("</tr>");
        //    emailBody.Append("</table>");

        //    emailBody.Append("<table>");

        //    emailBody.Append("<tr>");
        //    emailBody.Append("<td style='width:200px;'>");
        //    emailBody.Append("Update Status To:");
        //    emailBody.Append("</td>");
        //    emailBody.Append("<td>");
        //    emailBody.Append(claimStatusName);
        //    emailBody.Append("</td>");
        //    emailBody.Append("</tr>");

        //    emailBody.Append("<tr>");
        //    emailBody.Append("<td>");
        //    emailBody.Append("Insurer Claim #:");
        //    emailBody.Append("</td>");
        //    emailBody.Append("<td><a href='" + appurl + "/protected/Admin/UsersLeads.aspx?s=" + InsurerClaimId + "'>");
        //    emailBody.Append(InsurerClaimId);
        //    emailBody.Append("</a></td>");
        //    emailBody.Append("</tr>");

        //    emailBody.Append("<tr>");
        //    emailBody.Append("<td>");
        //    emailBody.Append("Insured Name:");
        //    emailBody.Append("</td>");
        //    emailBody.Append("<td>");
        //    emailBody.Append(InsurerName);
        //    emailBody.Append("</td>");
        //    emailBody.Append("</tr>");

        //    emailBody.Append("<tr>");
        //    emailBody.Append("<td>");
        //    emailBody.Append("Carrier:");
        //    emailBody.Append("</td>");
        //    emailBody.Append("<td>");
        //    emailBody.Append(carrier);
        //    emailBody.Append("</td>");
        //    emailBody.Append("</tr>");

        //    emailBody.Append("<tr>");
        //    emailBody.Append("<td>");
        //    emailBody.Append("Adjuster:");
        //    emailBody.Append("</td>");
        //    emailBody.Append("<td>");
        //    emailBody.Append(ClaimAdjuster);
        //    emailBody.Append("</td>");
        //    emailBody.Append("</tr>");

        //    emailBody.Append("<tr>");
        //    emailBody.Append("<td>");
        //    emailBody.Append("Adjuster Company Name:");
        //    emailBody.Append("</td>");
        //    emailBody.Append("<td>");
        //    emailBody.Append(AdjusterComapnyName);
        //    emailBody.Append("</td>");
        //    emailBody.Append("</tr>");


        //    emailBody.Append("<tr>");
        //    emailBody.Append("<td>");
        //    emailBody.Append("Updated By:");
        //    emailBody.Append("</td>");
        //    emailBody.Append("<td>");
        //    emailBody.Append(user.UserName);
        //    emailBody.Append("</td>");
        //    emailBody.Append("</tr>");

        //    emailBody.Append("<tr>");
        //    emailBody.Append("<td>");
        //    emailBody.Append("Comment/Note:");
        //    emailBody.Append("</td>");
        //    emailBody.Append("<td>");
        //    emailBody.Append(CommentNote);
        //    emailBody.Append("</td>");
        //    emailBody.Append("</tr>");

        //    emailBody.Append("<tr>");
        //    emailBody.Append("<td>");
        //    emailBody.Append("Email To:");
        //    emailBody.Append("</td>");
        //    emailBody.Append("<td>");
        //    emailBody.Append(EmailTo);
        //    emailBody.Append("</td>");
        //    emailBody.Append("</tr>");

        //    if (recipId.Length > 0)
        //    {
        //        emailBody.Append("<tr>");
        //        emailBody.Append("<td style='vertical-align:top'>");
        //        emailBody.Append("Selected Recipients");
        //        emailBody.Append("</td>");
        //        emailBody.Append("<td>");

        //        //////

        //        emailBody.Append("<table style='border:1px solid;'>");
        //        emailBody.Append("<tr>");
        //        emailBody.Append("<td>");
        //        emailBody.Append("First Name");
        //        emailBody.Append("</td>");
        //        emailBody.Append("<td>");
        //        emailBody.Append("Last Name");
        //        emailBody.Append("</td>");
        //        emailBody.Append("<td>");
        //        emailBody.Append("Company Name");
        //        emailBody.Append("</td>");
        //        emailBody.Append("<td>");
        //        emailBody.Append("Email");
        //        emailBody.Append("</td>");
        //        emailBody.Append("<td>");
        //        emailBody.Append("Contact Title");
        //        emailBody.Append("</td>");
        //        emailBody.Append("</tr>");
        //        int index2 = 0;
        //        for (int index = 0; index < recipId.Length; index++)
        //        {
        //            index2 = 0;
        //            int.TryParse(recipId[index], out index2);
        //            if (IdofTable[index] == "c")
        //            {

        //                Contact objContact = ContactManager.Get(index2);
        //                emailBody.Append("<tr>");

        //                emailBody.Append("<td>");
        //                emailBody.Append(objContact.FirstName);
        //                emailBody.Append("</td>");
        //                emailBody.Append("<td>");
        //                emailBody.Append(objContact.LastName);
        //                emailBody.Append("</td>");
        //                emailBody.Append("<td>");
        //                emailBody.Append(objContact.CompanyName);
        //                emailBody.Append("</td>");
        //                emailBody.Append("<td>");
        //                emailBody.Append(objContact.Email);
        //                emailBody.Append("</td>");
        //                emailBody.Append("<td>");
        //                emailBody.Append(objContact.ContactTitle);
        //                emailBody.Append("</td>");
        //                emailBody.Append("</tr>");

        //            }
        //            else
        //            {
        //                AdjusterMaster objAdjuster = AdjusterManager.GetAdjusterId(index2);

        //                emailBody.Append("<tr>");

        //                emailBody.Append("<td>");
        //                emailBody.Append(objAdjuster.FirstName);
        //                emailBody.Append("</td>");
        //                emailBody.Append("<td>");
        //                emailBody.Append(objAdjuster.LastName);
        //                emailBody.Append("</td>");
        //                emailBody.Append("<td>");
        //                emailBody.Append(objAdjuster.CompanyName);
        //                emailBody.Append("</td>");
        //                emailBody.Append("<td>");
        //                emailBody.Append(objAdjuster.email);
        //                emailBody.Append("</td>");
        //                emailBody.Append("<td>");
        //                emailBody.Append("");
        //                emailBody.Append("</td>");
        //                emailBody.Append("</tr>");
        //            }
        //        }


        //        emailBody.Append("</table>");
        //        emailBody.Append("</td>");
        //        emailBody.Append("</tr>");
        //    }

        //    emailBody.Append("</table>");
        //    emailBody.Append("</div>");	// paneContentInner
        //    emailBody.Append("</div>");	// containerBox

        //    password = Core.SecurityManager.Decrypt(user.emailPassword);

        //    // Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, emailBody.ToString(), null, user.emailHost, smtpPort, user.Email, password, true);//user.isSSL ??
        //    string crsupportEmail = ConfigurationManager.AppSettings["smtpEmail"].ToString();
        //    string crsupportEmailPassword = ConfigurationManager.AppSettings["smtpPassword"].ToString();
        //    Core.EmailHelper.sendEmail(crsupportEmail, recipients, null, subject, emailBody.ToString(), null, crsupportEmail, crsupportEmailPassword);


        //}

    }
}