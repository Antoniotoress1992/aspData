using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;
using CRM.Data.Entities;
using CRM.Web.UserControl.Admin;


using Microsoft.Reporting.WebForms;

namespace CRM.Web.Protected.Reports.Claim
{
    public partial class claimListing : System.Web.UI.Page
    {
        List<Carrier> carriers = null;
        ClaimManager ClaimManagerObj = new ClaimManager();
        PolicyLimit PolicyLimitObj = new PolicyLimit();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                int clientID = SessionHelper.getClientId();
                carriers = CarrierManager.GetCarriers(clientID).ToList();
                CollectionManager.FillCollection(ddlCarrier, "CarrierID", "CarrierName", carriers);
                if (carriers != null)
                {
                    int[] carrierID = new int[carriers.Count];
                    for ( var i = 0;i<carriers.Count;i++){
                        carrierID[i]  = carriers[i].CarrierID; 
                    }
                    

                    List<CarrierLocation> CarrierLocationArr = new List<CarrierLocation>();

                    CarrierLocationArr = CarrierLocationManager.GetAllList(carrierID);

                    CollectionManager.FillCollection(gvLocation, "CarrierLocationID", "LocationName", CarrierLocationArr);

                }
                ddlCarrier.Items[0].Text = "Select All";
                gvLocation.Items[0].Text = "Select All";
                ddlCarrier.SelectedIndex = 0;
                gvLocation.SelectedIndex = 0;
            }

        }
        public void btnGenerateReport_Click(object sender, EventArgs e)
        {
            bindReport();

        }
        int CoverageALimitId = 0;
        int CoverageBLimitId = 0;
        int CoverageCLimitId = 0;
        int CoverageDLimitId = 0;
        int CoverageELimitId = 0;
        int OtherCoverageLimitId = 0;


        decimal coverageA = 0;
        decimal coverageB = 0;
        decimal coverageC = 0;
        decimal coverageD = 0;
        decimal coverageE = 0;
        decimal otherCoverage = 0;

        private void bindReport()
        {
            int clientID = Core.SessionHelper.getClientId();


           

            reportViewer.Reset();
            reportViewer.ProcessingMode = ProcessingMode.Local;

            reportViewer.LocalReport.DataSources.Clear();

            reportViewer.LocalReport.EnableExternalImages = true;

            ReportDataSource reportDataSourceDataSet1 = new ReportDataSource();
            ReportDataSource reportDataSourceDataSet2 = new ReportDataSource();

            

            List<ClaimReport> reportData = null;

            DateTime txtFromDateTime = new DateTime();
            txtFromDateTime.Equals(DateTime.MinValue);



            DateTime txtToDateTime = new DateTime();
          

            //search condition
            if (txtFromDate.Text != "")
            {
                txtFromDateTime = Convert.ToDateTime(txtFromDate.Text);
            }
            if (txtToDate.Text != "")
            {
                txtToDateTime = Convert.ToDateTime(txtToDate.Text);
            }

            int ddlCarrierListCount = 0;
            int gvLocationListCount = 0;
            if (ddlCarrier.SelectedIndex == 0)
            {
                ddlCarrierListCount = ddlCarrier.Items.Count;
            }
            else {
                for (int i = 0; i < ddlCarrier.Items.Count; i++)
                {

                    if (ddlCarrier.Items[i].Selected == true)
                    {

                        ddlCarrierListCount = ddlCarrierListCount + 1;
                    }
                }
            
            }
            if (gvLocation.SelectedIndex == 0)
            {

                gvLocationListCount = gvLocation.Items.Count;
            }
            else {
                for (int i = 0; i < gvLocation.Items.Count; i++)
                {


                    if (gvLocation.Items[i].Selected == true)
                    {
                        gvLocationListCount = gvLocationListCount + 1;
                    }


                }
            
            }

            

            int[] ddlCarrierList = new int[ddlCarrierListCount];
            string[] gvLocationList = new string[gvLocationListCount];

            var j = 0;
            
            if (ddlCarrier.SelectedIndex == 0)
            {
                for (int i = 1; i < ddlCarrier.Items.Count; i++)
                {

                    ddlCarrierList[j] = Convert.ToInt32(ddlCarrier.Items[i].Value);
                    j = j + 1;



                }

            }
            else {
                for (int i = 0; i < ddlCarrier.Items.Count; i++)
                {


                    if (ddlCarrier.Items[i].Selected == true)
                    {
                        ddlCarrierList[j] = Convert.ToInt32(ddlCarrier.Items[i].Value);
                        j = j + 1;
                    }


                }
            
            }
            j = 0;
            if (gvLocation.SelectedIndex == 0) {
                for (int i = 1; i < gvLocation.Items.Count; i++)
                {


                   
                        gvLocationList[j] = gvLocation.Items[i].Text;
                        j = j + 1;
                   

                }
            
            
            }else{
                for (int i = 0; i < gvLocation.Items.Count; i++)
                {


                    if (gvLocation.Items[i].Selected == true)
                    {
                        gvLocationList[j] = gvLocation.Items[i].Text;
                        j = j + 1;
                    }


                }
            
            
            }
            

            //PolicyLimit PolicyLimitObj = new PolicyLimit();

          


            //get the claimList

            ucAllUserLeads userGetClaim = new ucAllUserLeads();
            List<LeadView> lead = new List<LeadView>();
            lead = userGetClaim.getLeadList();
            
            if (lead != null) { 
                int[] claimList = new int[lead.Count];
                for (var i = 0; i < lead.Count; i++) {
                    claimList[i] = lead[i].ClaimID;
                }
                reportData = ClaimManagerObj.GetClaimsByClientId(clientID, claimList);

                ClaimReportView ClaimReportViewObj = new ClaimReportView();

                ClaimReportViewObj = getClaimListingReport(reportData, txtFromDateTime, txtToDateTime, ddlCarrierList, gvLocationList);



                reportDataSourceDataSet1.Name = "DataSet1";
                reportDataSourceDataSet1.Value = ClaimReportViewObj.claimReportArr;

                reportDataSourceDataSet2.Name = "DataSet2";
                reportDataSourceDataSet2.Value = ClaimReportViewObj.ClaimReportAverageObj;

                reportViewer.LocalReport.DataSources.Add(reportDataSourceDataSet1);
                reportViewer.LocalReport.DataSources.Add(reportDataSourceDataSet2);

                reportViewer.LocalReport.ReportPath = Server.MapPath("~/Protected/Reports/Claim/ClaimReport.rdlc");
            
            }
            
           
        }
        public string getInsureBranch(int claimId) {
            ucAllUserLeads userGetClaim = new ucAllUserLeads();
            List<LeadView> lead = new List<LeadView>();
            lead = userGetClaim.getLeadList();
            string insureBranch = "";
            for (var i = 0; i < lead.Count; i++) {
                if (lead[i].ClaimID == claimId) {
                    insureBranch = lead[i].LocationName;
                    break;
                }
            }
            return insureBranch;
        }
        public string getInsuredName(int claimId) {
            ucAllUserLeads userGetClaim = new ucAllUserLeads();
            List<LeadView> lead = new List<LeadView>();
            lead = userGetClaim.getLeadList();
            string insuredName = "";
            for (var i = 0; i < lead.Count; i++)
            {
                if (lead[i].ClaimID == claimId)
                {
                   
                    insuredName = lead[i].InsuredName;
                    break;
                }
            }
            return insuredName;
        
        }
        public string getExaminerName(int examinerId){
            string examiner = "";
            Contact contactObj = new Contact();
            contactObj = ContactManager.Get(examinerId);
            if(contactObj != null){
                examiner = contactObj.ContactName;
                
            }
            return examiner;

        }
        private ClaimReportView getClaimListingReport(List<ClaimReport> reportData, DateTime txtFromDateTime, DateTime txtToDateTime, int[] ddlCarrierList, string[] gvLocationList)
        {
            List<ClaimReport> ClaimReportArr = new List<ClaimReport>();
            int clientID = SessionHelper.getClientId();
            ClaimReportView ClaimReportViewObj = new ClaimReportView();
            List<ClaimReportAverage> ClaimReportAverageArr = new List<ClaimReportAverage>();
            ClaimReportAverage ClaimReportAverageObj = new ClaimReportAverage();

            decimal totalCoverageA = 0;
            decimal totalCoverageB = 0;
            decimal totalCoverageC = 0;
            decimal totalCoverageD = 0;
            decimal totalCoverageE = 0;
            decimal totalOtherCoverage = 0;
            decimal totalOurInvoice = 0;
            decimal totalQty = 0;

            for (var i = 0; i < reportData.Count; i++) {
                string insureBranch = getInsureBranch(reportData[i].claimId);
                string insuredName = getInsuredName(reportData[i].claimId);
                string examinerName = "";
                if(reportData[i].ExaminerId != null){
                    int examinerId = Convert.ToInt32(reportData[i].ExaminerId);
                    
                     examinerName = getExaminerName(examinerId);
                }
                

                if (((reportData[i].DateReceived >= txtFromDateTime && reportData[i].DateClosed <= txtToDateTime) || txtToDateTime == DateTime.MinValue) && gvLocationList.Contains(insureBranch) && ddlCarrierList.Contains(Convert.ToInt32(reportData[i].CarrierId)))
                {
                        ClaimReport ClaimReportObj = new ClaimReport();
                        ClaimReportObj.AdjusterClaimNumber = reportData[i].AdjusterClaimNumber;
                        ClaimReportObj.InsurerClaim = reportData[i].InsurerClaim ;
                        ClaimReportObj.InsuredName = insuredName;
                        TimeSpan span = Convert.ToDateTime(reportData[i].DateClosed) - Convert.ToDateTime(reportData[i].DateReceived);
                        ClaimReportObj.DaystoClose = span.Days;
                        
                        ClaimReportObj.DateReceived = reportData[i].DateReceived;
                        ClaimReportObj.DateClosed = reportData[i].DateClosed ;
                        ClaimReportObj.InsureBranch = reportData[i].InsureBranch;
                        ClaimReportObj.OurAdjuster = reportData[i].OurAdjuster ;
                        ClaimReportObj.claimId = reportData[i].claimId;
                        ClaimReportObj.CarrierExaminer = examinerName;  


                        int policyId = reportData[i].policyId;
                        
                        ClaimReportObj.CoverageA = 0;
                        ClaimReportObj.CoverageB = 0;
                        ClaimReportObj.CoverageC = 0;
                        ClaimReportObj.CoverageD = 0;
                        ClaimReportObj.CoverageE = 0;
                        ClaimReportObj.OtherCoverage = 0;
                        ClaimReportObj.CoverageA = PolicyLimitManager.getPolicyLimitAmount("A", policyId);
                        ClaimReportObj.CoverageB = PolicyLimitManager.getPolicyLimitAmount("B", policyId);
                        ClaimReportObj.CoverageC = PolicyLimitManager.getPolicyLimitAmount("C", policyId);
                        ClaimReportObj.CoverageD = PolicyLimitManager.getPolicyLimitAmount("D", policyId);
                        ClaimReportObj.CoverageE = PolicyLimitManager.getPolicyLimitAmount("E", policyId);
                        ClaimReportObj.OtherCoverage = PolicyLimitManager.getPolicyLimitAmountOther(policyId);
                        ClaimReportObj.OurInvoice = InvoiceDetailManager.getTotalInvoice(reportData[i].claimId);
                        ClaimReportObj.Miles = InvoiceDetailManager.getTotalMiles(reportData[i].claimId, clientID);

                      
                       

                        ClaimReportObj.CoverageA = Decimal.Round(Convert.ToDecimal(ClaimReportObj.CoverageA), 2);
                        ClaimReportObj.CoverageB = Decimal.Round(Convert.ToDecimal(ClaimReportObj.CoverageB), 2);
                        ClaimReportObj.CoverageC = Decimal.Round(Convert.ToDecimal(ClaimReportObj.CoverageC), 2);
                        ClaimReportObj.CoverageD = Decimal.Round(Convert.ToDecimal(ClaimReportObj.CoverageD), 2);
                        ClaimReportObj.CoverageE = Decimal.Round(Convert.ToDecimal(ClaimReportObj.CoverageE), 2);
                        ClaimReportObj.OtherCoverage = Decimal.Round(Convert.ToDecimal(ClaimReportObj.OtherCoverage), 2);
                        ClaimReportObj.OurInvoice = Decimal.Round(Convert.ToDecimal(ClaimReportObj.OurInvoice), 2);
                        ClaimReportObj.Miles = Decimal.Round(Convert.ToDecimal(ClaimReportObj.Miles), 2);


                        if (ClaimReportObj.CoverageA != null) { totalCoverageA = Decimal.Round(totalCoverageA + Convert.ToDecimal(ClaimReportObj.CoverageA),2); }
                        if (ClaimReportObj.CoverageB != null) { totalCoverageB = Decimal.Round(totalCoverageB + Convert.ToDecimal(ClaimReportObj.CoverageB),2); }
                        if (ClaimReportObj.CoverageC != null) { totalCoverageC = Decimal.Round(totalCoverageC + Convert.ToDecimal(ClaimReportObj.CoverageC),2); }
                        if (ClaimReportObj.CoverageD != null) { totalCoverageD = Decimal.Round(totalCoverageD + Convert.ToDecimal(ClaimReportObj.CoverageD),2); }
                        if (ClaimReportObj.CoverageE != null) { totalCoverageE = Decimal.Round(totalCoverageE + Convert.ToDecimal(ClaimReportObj.CoverageE),2); }
                        if (ClaimReportObj.OtherCoverage != null) { totalOtherCoverage = Decimal.Round(totalOtherCoverage + Convert.ToDecimal(ClaimReportObj.OtherCoverage),2); }
                        if (ClaimReportObj.OurInvoice != null) { totalOurInvoice = Decimal.Round(totalOurInvoice + Convert.ToDecimal(ClaimReportObj.OurInvoice), 2); }
                        if (ClaimReportObj.Miles != null) { totalQty = Decimal.Round(totalQty + Convert.ToDecimal(ClaimReportObj.Miles), 2); }    

                        ClaimReportArr.Add(ClaimReportObj);
                      

                    }

            }
            if (reportData != null && reportData.Count != 0) {
                ClaimReportViewObj.claimReportArr = ClaimReportArr;
                ClaimReportAverageObj.totalCoverageA = totalCoverageA;
                ClaimReportAverageObj.totalCoverageB = totalCoverageB;
                ClaimReportAverageObj.totalCoverageC = totalCoverageC;
                ClaimReportAverageObj.totalCoverageD = totalCoverageD;
                ClaimReportAverageObj.totalCoverageE = totalCoverageE;
                ClaimReportAverageObj.totalOtherCoverage = totalOtherCoverage;
                ClaimReportAverageObj.totalOurInvoice = totalOurInvoice;
                ClaimReportAverageObj.totalMiles = totalQty;
                
            }
           

            if (reportData != null && reportData.Count != 0)
            {
                ClaimReportAverageObj.avgCoverageA = Decimal.Round(totalCoverageA / reportData.Count,2);
                ClaimReportAverageObj.avgCoverageB = Decimal.Round(totalCoverageB / reportData.Count,2);
                ClaimReportAverageObj.avgCoverageC = Decimal.Round(totalCoverageC / reportData.Count,2);
                ClaimReportAverageObj.avgCoverageD = Decimal.Round(totalCoverageD / reportData.Count,2);
                ClaimReportAverageObj.avgCoverageE = Decimal.Round(totalCoverageE / reportData.Count,2);
                ClaimReportAverageObj.avgOtherCoverage = Decimal.Round(totalOtherCoverage / reportData.Count,2);
                ClaimReportAverageObj.avgOurInvoice = Decimal.Round(totalOurInvoice / reportData.Count,2);
                ClaimReportAverageObj.avgMiles = Decimal.Round(totalQty / reportData.Count,2);
            }
            ClaimReportAverageArr.Add(ClaimReportAverageObj);
            ClaimReportViewObj.ClaimReportAverageObj = ClaimReportAverageArr;
            return ClaimReportViewObj;

        }



    }

    
}
    