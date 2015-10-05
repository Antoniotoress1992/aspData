using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqKit;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;

using Microsoft.Reporting.WebForms;
using CRM.Data.Entities;
using System.Reflection;

namespace CRM.Web.Protected.Reports.Payroll
{
    public partial class AdjusterPayout : System.Web.UI.Page
    {
        //List<AdjusterPayout> Adjuster = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            // check user permission
            Master.checkPermission();

            if (!Page.IsPostBack)
            {
                // bind adjuster data
                bindData();
            }
        }

        private void bindData()
        {
            // 2014-05-12 tortega
            // get client data
            int clientID = Core.SessionHelper.getClientId();

            List<AdjusterMaster> adjusters = null;

            // load adjusters branch with code
            List<AdjusterSettingsPayroll> adjusterSettingsPayrolls = new List<AdjusterSettingsPayroll>();

            adjusterSettingsPayrolls = AdjusterManager.GetGetAllAdjusterPayrollSetting(clientID).GroupBy(x => x.AdjusterBranch).Select(g => g.FirstOrDefault()).ToList();

            // bind adjusters branch and code to DDL 
           // CollectionManager.FillCollection(ddlBranch, "BranchCode", "AdjusterBranch", adjusterSettingsPayrolls);
            //  ddlBranch.Items.Insert(0, new ListItem("**All Branches**", "0"));

            // load adjusters for client
            //adjusters = AdjusterManager.GetAll(clientID).ToList();

            //// bind adjusters to DDL
            //CollectionManager.FillCollection(ddlAdjusters, "AdjusterId", "AdjusterName", adjusters);
        }
        private void generateReport()
        {

            //int adjusterId = 0;
            //string branchId = string.Empty;
            int clientID = Core.SessionHelper.getClientId();
            //Expression<Func<vw_AdjusterPayrollForAllBilled, bool>> predicate = null;
            //List<vw_AdjusterPayrollForAllBilled> reportData = null;

            //predicate = PredicateBuilder.True<CRM.Data.Entities.vw_AdjusterPayrollForAllBilled>();
            //predicate = predicate.And(x => x.ClientID == clientID);

            //// check for selected adjuster, if any
            //if (ddlBranch.SelectedIndex > 0)
            //{
            //    branchId = ddlBranch.SelectedValue;

            //    predicate = predicate.And(x => x.BranchCode == branchId);
            //}

            //// check for selected adjuster, if any
            //if (ddlAdjusters.SelectedIndex > 0) {
            //    adjusterId = Convert.ToInt32(ddlAdjusters.SelectedValue);

            //    predicate = predicate.And(x => x.AdjusterId == adjusterId);
            //}

            //// check date range
            //if (!string.IsNullOrEmpty(txtFromDate.Text)) {
            //    predicate = predicate.And(x => x.InvoiceDate >= txtFromDate.Date);
            //}

            //if (!string.IsNullOrEmpty(txtToDate.Text)) {
            //    predicate = predicate.And(x => x.InvoiceDate <= txtToDate.Date);
            //}

            //// check for All billed services and expenses 
            //if (rbtAllBilledServicesAndExpenses.Checked)
            //{
            //    predicate = predicate.And(x => x.isBillable == rbtAllBilledServicesAndExpenses.Checked);           
            //}           


            //tabPanelReport.Visible = true;

            ////int adjusterID = 0;

            //using (RerportManager report = new RerportManager()) {
            //    reportData = report.AdjusterPayrollAllBilled(predicate);
            //}

            ////Invoice = InvoiceManager.

            //// old report 
            //if (reportData != null)
            //{
            //    reportViewer.Reset();
            //    reportViewer.ProcessingMode = ProcessingMode.Local;

            //    reportViewer.LocalReport.DataSources.Clear();


            //    reportViewer.LocalReport.EnableExternalImages = true;
            //    ReportDataSource reportDataSource = new ReportDataSource();
            //    reportDataSource.Name = "AdjusterPayrollForAllBilled";
            //    reportDataSource.Value = reportData;
            //    reportViewer.LocalReport.DataSources.Add(reportDataSource);

            // check for  AdjusterPayrollForAllBilledWithoutReimbursable
            // check for IsIncluded Reimbursable
            if (rbtDetailedType.Checked)
            {

                GenerateAdjusterPayrollForAllBilledDetailed();

            }
            else
            {
                GenerateAdjusterPayrollSummary();
                //reportViewer.LocalReport.ReportPath = Server.MapPath(@"~/Protected/Reports/payroll/AdjusterPayrollForAllBilledWithoutReimbursable.rdlc");
            }

            reportViewer.LocalReport.Refresh();

            // if(String.IsNullOrWhiteSpace(txtFromDate.Text)&&string.IsNullOrWhiteSpace(txtToDate.Text))
            string dateReportParameter = string.Format("{0} - {1}", txtFromDate.Text.Trim(), txtToDate.Text.Trim());

            if (dateReportParameter == " - ")
            {
                dateReportParameter = " ";
            }

            reportViewer.LocalReport.SetParameters(new ReportParameter("Date", dateReportParameter));//string.Format("{0} - {1}", txtFromDate.Text.Trim(), txtToDate.Text.Trim())));
            // reportViewer.LocalReport.SetParameters(new ReportParameter("FromDate", txtToDate.Text));

            //clientID = SessionHelper.getClientId();

            Client client = ClientManager.Get(clientID);

            if (client != null)
            {
                reportViewer.LocalReport.SetParameters(new ReportParameter("CompanyName", client.BusinessName));
            }

            if (this.rbtDetailedType.Checked)
            {
                reportViewer.LocalReport.SetParameters(new ReportParameter("IsReimbursement", cbxIncludeReimbursable.Checked.ToString()));
            }
            // old report 
            //if (reportData != null) {
            //    reportViewer.Reset();
            //    reportViewer.ProcessingMode = ProcessingMode.Local;

            //    reportViewer.LocalReport.DataSources.Clear();

            //    reportViewer.LocalReport.EnableExternalImages = true;
            //    ReportDataSource reportDataSource = new ReportDataSource();
            //    reportDataSource.Name = "DataSet1";
            //    reportDataSource.Value = reportData;
            //    reportViewer.LocalReport.DataSources.Add(reportDataSource);
            //    reportViewer.LocalReport.ReportPath = Server.MapPath(@"~/Protected/Reports/payroll/AdjusterPayroll.rdlc");
            //    //reportViewer.LocalReport.ReportPath = Server.MapPath(@"~/Protected/Reports/payroll/AdjusterPayrout.rdlc");
            //    //add sub report
            //    //  this.reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subReportsEventHandler);
            //}
        }


        /// <summary>
        /// Generate report for AdjusterPayrollForAllBilledDetailed
        /// </summary>
        private void GenerateAdjusterPayrollForAllBilledDetailed()
        {
            int adjusterId = 0;
            string branchId = string.Empty;
            int clientID = Core.SessionHelper.getClientId();
            Expression<Func<vw_AdjusterPayrollForAllBilledDetailed, bool>> predicate = null;
            List<vw_AdjusterPayrollForAllBilledDetailed> reportData = null;

            predicate = PredicateBuilder.True<CRM.Data.Entities.vw_AdjusterPayrollForAllBilledDetailed>();
            predicate = predicate.And(x => x.ClientID == clientID);

            // check for selected adjuster, if any
            //if (ddlBranch.SelectedIndex > 0)
           // {
             //   branchId = ddlBranch.SelectedValue;

              //  predicate = predicate.And(x => x.BranchCode == branchId);
           // }

            // check for selected adjuster, if any
            if (ddlAdjusters.SelectedIndex > 0)
            {
                adjusterId = Convert.ToInt32(ddlAdjusters.SelectedValue);

                predicate = predicate.And(x => x.AdjusterId == adjusterId);
            }

            // check date range
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                predicate = predicate.And(x => x.InvoiceDate >= txtFromDate.Date);
            }

            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                predicate = predicate.And(x => x.InvoiceDate <= txtToDate.Date);
            }

            // check for All billed services and expenses 
            if (rbtAllBilledServicesAndExpenses.Checked)
            {
                predicate = predicate.And(x => x.isBillable == rbtAllBilledServicesAndExpenses.Checked);
            }


            tabPanelReport.Visible = true;

            //int adjusterID = 0;

            using (RerportManager report = new RerportManager())
            {
                reportData = report.AdjusterPayrollAllBilledDetailed(predicate);
            }

            //Invoice = InvoiceManager.

            // old report 
            if (reportData != null)
            {
                reportViewer.Reset();
                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.DataSources.Clear();


                reportViewer.LocalReport.EnableExternalImages = true;
                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "AdjusterPayrollForAllBilledDetailed";
                reportDataSource.Value = reportData;
                reportViewer.LocalReport.DataSources.Add(reportDataSource);

                // reportViewer.LocalReport.Refresh();

                // if (cbxIncludeReimbursable.Checked)
                //if(true)
                //{
                using (RerportManager report = new RerportManager())
                {
                    //reportData =

                    ReportDataSource reportDataSource1 = new ReportDataSource();
                    reportDataSource1.Name = "AdjusterNames";
                    reportDataSource1.Value = report.AdjusterName();
                    reportViewer.LocalReport.DataSources.Add(reportDataSource1);

                    reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

                    //  reportViewer.LocalReport.Refresh();
                    //reportViewer.LocalReport.SetParameters(new ReportParameter("AdjusterName", string.Format("{0} - {1}", txtFromDate.Text.Trim(), txtToDate.Text.Trim())));                      

                }

                // this report contains subreport named payroll.rdlc
                reportViewer.LocalReport.ReportPath = Server.MapPath(@"~/Protected/Reports/payroll/AdjusterPayrollDetails.rdlc");
                // reportViewer.LocalReport.ReportPath = Server.MapPath(@"~/Protected/Reports/payroll/AdjusterPayrollForAllBilledDetailed.rdlc");
                //}
                //else 
                //{
                //    reportViewer.LocalReport.ReportPath = Server.MapPath(@"~/Protected/Reports/payroll/AdjusterPayrollForAllBilledDetailedWithoutReimbursable.rdlc");
                //}                
            }
        }

        // subreport viewer 
        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            // report code
            int adjusterId = 0;
            string branchId = string.Empty;
            int clientID = Core.SessionHelper.getClientId();
            Expression<Func<vw_AdjusterPayrollForAllBilledDetailed, bool>> predicate = null;
            List<vw_AdjusterPayrollForAllBilledDetailed> reportData = null;

            predicate = PredicateBuilder.True<CRM.Data.Entities.vw_AdjusterPayrollForAllBilledDetailed>();
            predicate = predicate.And(x => x.ClientID == clientID);

            // check for selected adjuster, if any
           // if (ddlBranch.SelectedIndex > 0)
           // {
            //    branchId = ddlBranch.SelectedValue;

              //  predicate = predicate.And(x => x.BranchCode == branchId);
          //  }

            // check for selected adjuster, if any
            if (ddlAdjusters.SelectedIndex > 0)
            {
                adjusterId = Convert.ToInt32(ddlAdjusters.SelectedValue);

                predicate = predicate.And(x => x.AdjusterId == adjusterId);
            }

            // check date range
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                predicate = predicate.And(x => x.InvoiceDate >= txtFromDate.Date);
            }

            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                predicate = predicate.And(x => x.InvoiceDate <= txtToDate.Date);
            }

            // check for All billed services and expenses 
            if (rbtAllBilledServicesAndExpenses.Checked)
            {
                predicate = predicate.And(x => x.isBillable == rbtAllBilledServicesAndExpenses.Checked);
            }


            tabPanelReport.Visible = true;

            //int adjusterID = 0;

            using (RerportManager report = new RerportManager())
            {
                reportData = report.AdjusterPayrollAllBilledDetailed(predicate);
            }


            // get adjusterName from the parameters
            string adjusterName = e.Parameters[0].Values[0];

            // remove all previously attached Datasources.
            e.DataSources.Clear();
            //Expression<Func<vw_AdjusterPayrollForAllBilledDetailed, bool>> predicate = null;

            //predicate = PredicateBuilder.True<CRM.Data.Entities.vw_AdjusterPayrollForAllBilledDetailed>();
            //predicate.And(x => x.AdjusterName == adjusterName);

            using (RerportManager report = new RerportManager())
            {
                // var reportData = report.AdjusterPayrollAllBilledDetailed(predicate);


                // Retrieve payrollData list based on adjusterName
                var payrollData = from m in reportData
                                  where m.AdjusterName == adjusterName
                                  select m;

                // add retrieved dataset or you can call it list to data source
                e.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource()
                {
                    Name = "AdjusterPayrollForAllBilledDetailed",
                    Value = payrollData

                });
            }
        }


        /// <summary>
        /// Generate report for Adjuster Payroll For All Billed Summary
        /// </summary>
        private void GenerateAdjusterPayrollSummary()
        {
            int adjusterId = 0;
            string branchId = string.Empty;
            int clientID = Core.SessionHelper.getClientId();
            Expression<Func<vw_AdjusterPayrollForAllBilled, bool>> predicate = null;
            List<vw_AdjusterPayrollForAllBilled> reportData = null;

            predicate = PredicateBuilder.True<CRM.Data.Entities.vw_AdjusterPayrollForAllBilled>();
            predicate = predicate.And(x => x.ClientID == clientID);

            // check for selected adjuster, if any
          //  if (ddlBranch.SelectedIndex > 0)
           // {
              //  branchId = ddlBranch.SelectedValue;

               // predicate = predicate.And(x => x.BranchCode == branchId);
           // }

            // check for selected adjuster, if any
            if (ddlAdjusters.SelectedIndex > 0)
            {
                adjusterId = Convert.ToInt32(ddlAdjusters.SelectedValue);

                predicate = predicate.And(x => x.AdjusterId == adjusterId);
            }

            // check date range
            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                predicate = predicate.And(x => x.InvoiceDate >= txtFromDate.Date);
            }

            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                predicate = predicate.And(x => x.InvoiceDate <= txtToDate.Date);
            }

            // check for All billed services and expenses 
            if (rbtAllBilledServicesAndExpenses.Checked)
            {
                predicate = predicate.And(x => x.isBillable == rbtAllBilledServicesAndExpenses.Checked);
            }


            tabPanelReport.Visible = true;

            //int adjusterID = 0;

            using (RerportManager report = new RerportManager())
            {
                reportData = report.AdjusterPayrollAllBilled(predicate);
            }

            //Invoice = InvoiceManager.

            // old report 
            if (reportData != null)
            {
                reportViewer.Reset();
                reportViewer.ProcessingMode = ProcessingMode.Local;

                reportViewer.LocalReport.DataSources.Clear();


                reportViewer.LocalReport.EnableExternalImages = true;
                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "AdjusterPayrollForAllBilled";
                reportDataSource.Value = reportData;
                reportViewer.LocalReport.DataSources.Add(reportDataSource);

                // check for  AdjusterPayrollForAllBilledWithoutReimbursable
                // check for IsIncluded Reimbursable
                if (cbxIncludeReimbursable.Checked)
                {
                    reportViewer.LocalReport.ReportPath = Server.MapPath(@"~/Protected/Reports/payroll/AdjusterPayrollForAllBilled.rdlc");
                }
                else
                {
                    reportViewer.LocalReport.ReportPath = Server.MapPath(@"~/Protected/Reports/payroll/AdjusterPayrollForAllBilledWithoutReimbursable.rdlc");
                }
            }
        }


        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            // show report tab
            tabContainerReport.ActiveTabIndex = 1;


            generateReport();
            // DisableUnwantedExportFormat(reportViewer, "Excel");
            // DisableUnwantedExportFormat(reportViewer, "WORD");

        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get client data
            int clientID = Core.SessionHelper.getClientId();

            List<AdjusterMaster> adjusters = null;

            //if (ddlBranch.SelectedIndex > 0)
            //{
                // load adjusters for client
              //  adjusters = AdjusterManager.GetAllByBranch(clientID, ddlBranch.SelectedValue).ToList();

                // bind adjusters to DDL
             //   CollectionManager.FillCollection(ddlAdjusters, "AdjusterId", "AdjusterName", adjusters);
           // }
        }

        //public void DisableUnwantedExportFormat(ReportViewer ReportViewerID, string strFormatName)
        //{
        //    FieldInfo info;

        //    foreach (RenderingExtension extension in ReportViewerID.LocalReport.ListRenderingExtensions())
        //    {
        //        if (extension.Name == strFormatName)
        //        {
        //            info = extension.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
        //            info.SetValue(extension, false);
        //        }
        //    }
        //}

        //protected void subReportsEventHandler(object sender, SubreportProcessingEventArgs e)
        //{
        //    e.DataSources.Add(new ReportDataSource("DataSet1", invoice[0].invoiceLines));
        //}

    }


}