#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Core;
using CRM.Data.Account;
using CRM.Data;
using System.Transactions;
using LinqKit;
using System.Data;
using System.Reflection;
using CrystalDecisions.CrystalReports.Engine;
using CRM.Data.Entities;
#endregion

namespace CRM.Web.Protected.Reports {
	
	public partial class AllUserLeadsReport : System.Web.UI.Page {
		protected void Page_Init(Object sender, EventArgs e) {
			if (IsPostBack)
				bindReport();
		}
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				bindReport();
			}
		}
		protected void bindReport() {
			

			List<Leads> objLead = null;
			var predicate = PredicateBuilder.True<CRM.Data.Entities.Leads>();
		
			if (Cache["FromDate"] != null && Cache["FromDate"].ToString() != "") {
				DateTime sDate = new DateTime(Convert.ToInt32(Cache["FromDate"].ToString().Trim().Substring(6, 4)), Convert.ToInt32(Cache["FromDate"].ToString().Trim().Substring(3, 2)), Convert.ToInt32(Cache["FromDate"].ToString().Trim().Substring(0, 2)));

				var datefrom = Convert.ToDateTime(sDate);
				predicate = predicate.And(Lead => Lead.DateSubmitted >= datefrom
								    );
			}
			if (Cache["ToDate"] != null && Cache["ToDate"].ToString() != "") {
				DateTime sDate = new DateTime(Convert.ToInt32(Cache["ToDate"].ToString().Trim().Substring(6, 4)), Convert.ToInt32(Cache["ToDate"].ToString().Trim().Substring(3, 2)), Convert.ToInt32(Cache["ToDate"].ToString().Trim().Substring(0, 2)));

				var dateto = Convert.ToDateTime(sDate);
				predicate = predicate.And(Lead => Lead.DateSubmitted <= dateto
								    );
			}
			if (Cache["Criteria"] != null && Cache["Criteria"].ToString() != "") {
				var user = Cache["Criteria"].ToString();
				predicate = predicate.And(Lead => Lead.SecUser.UserName == user);

			}
			objLead = LeadsManager.GetPredicate(predicate);

			List<AllUserLeads> resAllUserLeads = new List<AllUserLeads>();
			AllUserLeads objAllUserLeadsGet = null;

			foreach (var get in objLead) {
				objAllUserLeadsGet = new AllUserLeads();
				

				objAllUserLeadsGet.LeadId = get.LeadId;
				objAllUserLeadsGet.UserId = (int)get.UserId;
				objAllUserLeadsGet.UserName = get.SecUser.UserName;
				//objAllUserLeadsGet.DateSubmitted = (DateTime?)get.DateSubmitted;
				objAllUserLeadsGet.DateSubmitted = get.DateSubmitted == null ? "" : Convert.ToDateTime(get.DateSubmitted).ToString("dd/MMM/yy");
				objAllUserLeadsGet.LFUUID = get.LFUUID;
				//objAllUserLeadsGet.OriginalLeadDate = (DateTime)get.OriginalLeadDate;
				objAllUserLeadsGet.OriginalLeadDate = get.OriginalLeadDate == null ? "" : Convert.ToDateTime(get.OriginalLeadDate).ToString("dd/MMM/yy");
				objAllUserLeadsGet.ClaimsNumber = get.ClaimsNumber == null ? 0 : (int)get.ClaimsNumber;
				//objAllUserLeadsGet.ClaimantName = get.ClaimantName;
				objAllUserLeadsGet.ClaimantName = get.ClaimantFirstName + get.ClaimantLastName;

				//if (get.LeadSourceMaster != null)
				//	objAllUserLeadsGet.LeadStatus = get.LeadSourceMaster.LeadSourceName == null ? "" : get.LeadSourceMaster.LeadSourceName;

				objAllUserLeadsGet.EmailAddress = get.EmailAddress;

				//if (get.AdjusterMaster != null)
				//	objAllUserLeadsGet.Adjuster = get.AdjusterMaster.AdjusterName == null ? "" : get.AdjusterMaster.AdjusterName;
				////objAllUserLeadsGet.LeadSource = get.LeadSource;

				if (get.LeadSourceMaster != null)
					objAllUserLeadsGet.LeadSource = get.LeadSourceMaster.LeadSourceName == null ? "" : get.LeadSourceMaster.LeadSourceName;

				if (get.PrimaryProducerId != null)
					objAllUserLeadsGet.PrimaryProducerId = (int)get.PrimaryProducerId;

				if (get.ProducerMaster != null)
					objAllUserLeadsGet.PrimaryProducerName = get.ProducerMaster.ProducerName;

				objAllUserLeadsGet.SecondaryProducerId = (int)(get.SecondaryProducerId ?? 0);
				//objAllUserLeadsGet.SecondaryProducerName = get.ProducerMaster1.ProducerName;

				if (get.SecondaryProducerMaster != null)
					objAllUserLeadsGet.SecondaryProducerName = get.SecondaryProducerMaster.SecondaryProduceName == null ? "" : get.SecondaryProducerMaster.SecondaryProduceName;

				objAllUserLeadsGet.EmailAddress = get.EmailAddress;

				if (get.InspectorMaster != null)
					objAllUserLeadsGet.InspectorName = get.InspectorMaster.InspectorName == null ? "" : get.InspectorMaster.InspectorName;

				objAllUserLeadsGet.InspectorCell = get.InspectorCell;
				objAllUserLeadsGet.InspectorEmail = get.InspectorEmail;
				objAllUserLeadsGet.PhoneNumber = get.PhoneNumber;

				//if (get.WebformSourceMaster != null)
				//	objAllUserLeadsGet.WebformSource = get.WebformSourceMaster.WebformSource == null ? "" : get.WebformSourceMaster.WebformSource;

				objAllUserLeadsGet.ClaimantComments = get.ClaimantComments;
				//objAllUserLeadsGet.TypeOfDamage = get.TypeOfDamageMaster.TypeOfDamage == null ? "" : get.TypeOfDamageMaster.TypeOfDamage;
				objAllUserLeadsGet.TypeOfDamage = get.TypeofDamageText == null ? "" : get.TypeofDamageText;
				//objAllUserLeadsGet.TypeOfProperty = (int)get.TypeOfProperty;
				//objAllUserLeadsGet.TypeOfProperty = get.TypeOfProperty == 1 ? "Home" : "Hotel";

				if (get.TypeOfPropertyMaster != null)
					objAllUserLeadsGet.TypeOfProperty = get.TypeOfPropertyMaster.TypeOfProperty == null ? "" : get.TypeOfPropertyMaster.TypeOfProperty;

				//objAllUserLeadsGet.ReporterToInsurer = get.ReporterToInsurer == null ? "NO" : get.ReporterToInsurer.ToString() == "Y" ? "YES" : "NO";
				if (get.ReportedToInsurerMaster != null)
					objAllUserLeadsGet.ReporterToInsurer = get.ReportedToInsurerMaster.ReportedToInsurer == null ? "" : get.ReportedToInsurerMaster.ReportedToInsurer;

				resAllUserLeads.Add(objAllUserLeadsGet);
			}

		

			ListtoDataTableConverter converter = new ListtoDataTableConverter();
			DataTable dt = converter.ToDataTable(resAllUserLeads);
			string xmlpath = Request.MapPath("~/Protected/Reports") + "/AllUserLeads.xml";
			dt.WriteXml(xmlpath);

			
			if (dt != null && dt.Rows.Count > 0) {
				
				ReportDocument RptDoc = new ReportDocument();
				string ReportFolderPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"].ToString();
				RptDoc.Load(Request.MapPath(ReportFolderPath + "crptAllUserLeads1.rpt"));//CrystalReport1.rpt
				RptDoc.SetDataSource(dt);
				CrystalReportViewer1.ReportSource = RptDoc;
				CrystalReportViewer1.DataBind();
			}
			else {
				CrystalReportViewer1.ReportSource = null;
				CrystalReportViewer1.DataBind();
			}
		}
		public class ListtoDataTableConverter {
			public DataTable ToDataTable<T>(List<T> items) {
				DataTable dataTable = new DataTable(typeof(T).Name);
				//Get all the properties
				PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
				foreach (PropertyInfo prop in Props) {
					//Setting column names as Property names
					dataTable.Columns.Add(prop.Name);
				}
				foreach (T item in items) {
					var values = new object[Props.Length];
					for (int i = 0; i < Props.Length; i++) {
						//inserting property values to datatable rows
						values[i] = Props[i].GetValue(item, null);
					}
					dataTable.Rows.Add(values);
				}
				//put a breakpoint here and check datatable
				return dataTable;
			}
		}

		protected void btnSearch_Click(object sender, EventArgs e) {
			hfToDate.Value = txtDateTo.Text.Trim();
			hfFromDate.Value = txtDateFrom.Text.Trim();
			hfCriteria.Value = txtSearch.Text.Trim();

			Cache["ToDate"] = txtDateTo.Text.Trim();
			Cache["FromDate"] = txtDateFrom.Text.Trim();
			Cache["Criteria"] = txtSearch.Text.Trim();



			bindReport();

		}

		protected void btnReset_Click(object sender, EventArgs e) {
			hfToDate.Value = "";
			txtDateTo.Text = string.Empty;
			hfFromDate.Value = "";
			txtDateFrom.Text = string.Empty;

			txtSearch.Text = string.Empty;
			hfCriteria.Value = "";
			Cache["ToDate"] = "";
			Cache["FromDate"] = "";
			Cache["Criteria"] = "";

			bindReport();

		}
	}
	public class AllUserLeads {
		

		public int? LeadId { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		//public DateTime? DateSubmitted { get; set; }
		public string DateSubmitted { get; set; }
		public string LFUUID { get; set; }
		//public DateTime OriginalLeadDate { get; set; }
		public string OriginalLeadDate { get; set; }
		public int ClaimsNumber { get; set; }
		public string ClaimantName { get; set; }
		//public string LeadStatus { get; set; }
		public string EmailAddress { get; set; }
		public string Adjuster { get; set; }
		public string LeadSource { get; set; }
		public int PrimaryProducerId { get; set; }
		public string PrimaryProducerName { get; set; }
		public int SecondaryProducerId { get; set; }
		public string SecondaryProducerName { get; set; }
		public string InspectorName { get; set; }
		public string InspectorCell { get; set; }
		public string InspectorEmail { get; set; }
		public string PhoneNumber { get; set; }
		public string WebformSource { get; set; }
		public string ClaimantComments { get; set; }
		public string TypeOfDamage { get; set; }
		//public int TypeOfProperty { get; set; }//MarketValue
		public string TypeOfProperty { get; set; }
		public string ReporterToInsurer { get; set; }
	}
}