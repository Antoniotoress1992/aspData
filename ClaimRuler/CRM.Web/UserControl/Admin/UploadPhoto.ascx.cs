

namespace CRM.Web.UserControl.Admin {
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
	using System.IO;
	using System.Data;
	using System.Text;
	using System.Web.UI.HtmlControls;
    using CRM.Data.Entities;
	#endregion

	public partial class UploadPhoto : System.Web.UI.UserControl {
		string ErrorMessage = string.Empty;
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				if (Session["LeadIds"] != null) {

					hfLeadsId.Value = Session["LeadIds"].ToString();
					FillDocument(Convert.ToInt32(Session["LeadIds"]));
					FillDocumentList(Convert.ToInt32(Session["LeadIds"]));
					Session["LeadId"] = Session["LeadIds"].ToString();

					if (Session["View"] != null) {
						string view = Session["View"].ToString();
						if (view != null && view == "1") {
							dvEdit.Visible = false;
							lblheading.Text = "Documents";
							btnPhotos.Text = "Photos";


							//chkInsurancePolicy.Enabled = false;
							//chkSignedRetainer.Enabled = false;
							//chkDamageReport.Enabled = false;
							//chkDamagePhoto.Enabled = false;
							//chkCertifiedInsurancePolicy.Enabled = false;
							//chkOwnerContract.Enabled = false;
							//chkContentList.Enabled = false;
							//chkDamageEstimate.Enabled = false;


							//btnSave.Visible = false;
							hfView.Value = "1";
							gvDocuments.Columns[2].Visible = false;

						}
					}
				}
				Leads objLeadSubmitt = LeadsManager.GetByLeadId(Convert.ToInt32(hfLeadsId.Value));
				if (objLeadSubmitt != null && objLeadSubmitt.IsSubmitted == true)
					btnGenerateReport.Visible = true;
				else
					btnGenerateReport.Visible = false;

				if (Session["LeadIds"] != null) {


					hfLeadsId.Value = Session["LeadIds"].ToString();

					FillDocument(Convert.ToInt32(Session["LeadIds"]));


					if (Session["Submitted"] != null) {
						string submitted = Session["submitted"].ToString();
						if (submitted != null && submitted == "1")
							dvEdit.Visible = false;
					}
				}
			}


		}

		private void FillDocument(int LeadId) {

			List<LeadsDocument> doc = LeadsUploadManager.getLeadsDocumentByLeadID(LeadId);
			if (doc != null && doc.Count > 0) {
				gvDocuments.DataSource = doc;
				gvDocuments.DataBind();
			}
			else {
				gvDocuments.DataSource = null;
				gvDocuments.DataBind();
			}


		}

		protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e) {
			if (e.CommandName == "DoDelete") {
				try {
					ImageButton imsource = (ImageButton)e.CommandSource;
					GridViewRow currentrow = null;
					if (imsource != null)
						currentrow = (GridViewRow)imsource.NamingContainer;
					HiddenField LeadId = (HiddenField)currentrow.FindControl("hfLeadId");

					using (TransactionScope scope = new TransactionScope()) {
						LeadsDocument doc = LeadsUploadManager.GetLeadsDocumentById(Convert.ToInt32(e.CommandArgument));
						doc.Status = 0;
						LeadsUploadManager.SaveDocument(doc);
						scope.Complete();
					}
					FillDocument(Convert.ToInt32(LeadId.Value));
					lblSave.Visible = true;
					lblSave.Text = "Record Deleted !!!";
				}
				catch (Exception ex) {
					lblError.Visible = true;
					lblError.Text = "Record Not Deleted !!!";
				}
			}
			if (e.CommandName == "DoUpdate") {


			}
		}

		protected void lvData_ItemCommand(object sender, ListViewCommandEventArgs e) {

		}

		protected void btnUploadDoc_Click(object sender, EventArgs e) {
			string ext;
			string ActualFileName = "", SavedFileName = string.Empty;
			string FileNameWithoutExt = "";
			string OccasionId = string.Empty;
			string Occasion = string.Empty;
			try {
				LeadsDocument objLeadDoc = new LeadsDocument();
				if (FileUpload2.HasFile) {
					int LeadID = 0;
					if (hfLeadsId.Value != null && Convert.ToInt32(hfLeadsId.Value) > 0) {
						LeadID = Convert.ToInt32(hfLeadsId.Value);
					}
					else {
						lblError.Text = string.Empty;
						lblError.Text = "There is a problem to upload.";
						lblError.Visible = true;
						return;
					}
					string Location = "";
					ext = System.IO.Path.GetExtension(FileUpload2.PostedFile.FileName);
					ActualFileName = FileUpload2.PostedFile.FileName.Substring(FileUpload2.PostedFile.FileName.LastIndexOf(@"\") + 1);
					FileNameWithoutExt = ActualFileName.Replace(ext, "");
					objLeadDoc.LeadId = LeadID;
					objLeadDoc.Description = txtDescriptionDoc.Text;
					objLeadDoc.DocumentName = ActualFileName;
					objLeadDoc.Status = 1;
					LeadsDocument objld = LeadsUploadManager.SaveDocument(objLeadDoc);
					if (objld.LeadDocumentId > 0) {
						if (!Directory.Exists(Server.MapPath("~/LeadsDocument/" + LeadID + "/" + objld.LeadDocumentId))) {
							Directory.CreateDirectory(Server.MapPath("~/LeadsDocument/" + LeadID + "/" + objld.LeadDocumentId));
						}

						FileUpload2.PostedFile.SaveAs(Server.MapPath("~/LeadsDocument/" + LeadID + "/" + objld.LeadDocumentId + "/" + ActualFileName));


						lblSave.Text = string.Empty;
						lblSave.Text = "Document Uploaded Successfully";
						lblSave.Visible = true;
						txtDescriptionDoc.Text = string.Empty;
						FillDocument(LeadID);


					}
				}
			}
			catch (Exception ex) {
				lblError.Text = string.Empty;
				lblError.Text = "There is a problem to upload.";
				lblError.Visible = true;
			}

		}

		protected void btnGenerateReport_Click(object sender, EventArgs e) {
			int LeadId = 0;

			if (hfLeadsId.Value != null && Convert.ToInt32(hfLeadsId.Value) > 0) {
				try {

					LeadId = Convert.ToInt32(hfLeadsId.Value);
					string filename1 = CreatePDF.CreateAndGetPDF(LeadId, Request.PhysicalApplicationPath + "PDF\\", out ErrorMessage);


					LeadReportGenerateLog objLeadReportGenerateLog = new LeadReportGenerateLog();
					objLeadReportGenerateLog.LeadId = Convert.ToInt32(hfLeadsId.Value);
					objLeadReportGenerateLog.GenerateDate = DateTime.Now;
					objLeadReportGenerateLog.Generatedby = Convert.ToInt32(Session["UserId"]);
					LeadReportLogManager.Save(objLeadReportGenerateLog);
					
				}
				catch (Exception ex) {
					lblError.Text = "Report Not Generated. " + ex.Message;
					lblError.Visible = true;

					return;
				}

				OpenNewWindow(LeadId);
			}
		}
		
		public void OpenNewWindow(int LeadId) {
			string url = Request.PhysicalApplicationPath + "PDF/" + LeadId + ".pdf";
			if (File.Exists(url)) {

				string FileName = LeadId + ".pdf";
				Response.ContentType = "Application/pdf";
				Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName + " ");
				Response.TransmitFile(url);
				Response.End();
			}
		}

		private void FillDocumentList(int LeadId) {

			Leads objLead = LeadsManager.GetByLeadId(LeadId);

			//if (objLead.LeadId > 0 && objLead.LeadId != null) {

			//     chkInsurancePolicy.Checked = objLead.hasInsurancePolicy == null ? false : objLead.hasInsurancePolicy == false ? false : true;
			//     chkSignedRetainer.Checked = objLead.hasSignedRetainer == null ? false : objLead.hasSignedRetainer == false ? false : true;
			//     chkDamageReport.Checked = objLead.hasDamageReport == null ? false : objLead.hasDamageReport == false ? false : true;
			//     chkDamagePhoto.Checked = objLead.hasDamagePhoto == null ? false : objLead.hasDamagePhoto == false ? false : true;
			//     chkCertifiedInsurancePolicy.Checked = objLead.hasCertifiedInsurancePolicy == null ? false : objLead.hasCertifiedInsurancePolicy == false ? false : true;
			//     chkOwnerContract.Checked = objLead.hasOwnerContract == null ? false : objLead.hasOwnerContract == false ? false : true;
			//     chkContentList.Checked = objLead.hasContentList == null ? false : objLead.hasContentList == false ? false : true;
			//     chkDamageEstimate.Checked = objLead.hasDamageEstimate == null ? false : objLead.hasDamageEstimate == false ? false : true;
			//}
		}
		
		protected void btnSave_Click(object sender, EventArgs e) {
			lblError.Text = string.Empty;
			lblError.Visible = false;
			lblSave.Text = string.Empty;
			lblSave.Visible = false;
			lblMessage.Visible = false;
			lblMessage.Text = string.Empty;
			try {
				bool isnew = false;
				Leads objLead = new Leads();
				if (hfLeadsId.Value == "0") {
					isnew = true;
				}
				else {
					objLead = LeadsManager.GetByLeadId(Convert.ToInt32(hfLeadsId.Value));
				}
				if (isnew) {
				}
				else {
					/*New Fields*/
					//objLead.hasInsurancePolicy = chkInsurancePolicy.Checked == true ? true : false;
					//objLead.hasSignedRetainer = chkSignedRetainer.Checked == true ? true : false;
					//objLead.hasDamageReport = chkDamageReport.Checked == true ? true : false;
					//objLead.hasDamagePhoto = chkDamagePhoto.Checked == true ? true : false;
					//objLead.hasCertifiedInsurancePolicy = chkCertifiedInsurancePolicy.Checked == true ? true : false;
					//objLead.hasOwnerContract = chkOwnerContract.Checked == true ? true : false;
					//objLead.hasContentList = chkContentList.Checked == true ? true : false;
					//objLead.hasDamageEstimate = chkDamageEstimate.Checked == true ? true : false;
					//if (chkInsurancePolicy.Checked == true && chkSignedRetainer.Checked == true && chkDamageReport.Checked == true && chkDamagePhoto.Checked == true &&
					//    chkCertifiedInsurancePolicy.Checked == true && chkOwnerContract.Checked == true && chkContentList.Checked == true && chkDamageEstimate.Checked == true) {
					//     objLead.AllDocumentsOnFile = true;
					//}
					//else {
					//     objLead.AllDocumentsOnFile = false;
					//}
					/**/
					Leads ins = LeadsManager.Save(objLead);

					//if (ins.LeadId > 0) {
					//	LeadLog objLeadlog = new LeadLog();
					//	//objLeadlog.hasInsurancePolicy = chkInsurancePolicy.Checked == true ? true : false;
					//	//objLeadlog.hasSignedRetainer = chkSignedRetainer.Checked == true ? true : false;
					//	//objLeadlog.hasDamageReport = chkDamageReport.Checked == true ? true : false;
					//	//objLeadlog.hasDamagePhoto = chkDamagePhoto.Checked == true ? true : false;
					//	//objLeadlog.hasCertifiedInsurancePolicy = chkCertifiedInsurancePolicy.Checked == true ? true : false;
					//	//objLeadlog.hasOwnerContract = chkOwnerContract.Checked == true ? true : false;
					//	//objLeadlog.hasContentList = chkContentList.Checked == true ? true : false;
					//	//objLeadlog.hasDamageEstimate = chkDamageEstimate.Checked == true ? true : false;
					//	//if (chkInsurancePolicy.Checked == true && chkSignedRetainer.Checked == true && chkDamageReport.Checked == true && chkDamagePhoto.Checked == true &&
					//	//    chkCertifiedInsurancePolicy.Checked == true && chkOwnerContract.Checked == true && chkContentList.Checked == true && chkDamageEstimate.Checked == true) {
					//	//     objLeadlog.AllDocumentsOnFile = true;
					//	//}
					//	//else {
					//	//     objLeadlog.AllDocumentsOnFile = false;
					//	//}

					//	/**/
					//	LeadLog insLog = LeadsManager.SaveLeadLog(objLeadlog);
					//	FillDocumentList(Convert.ToInt32(hfLeadsId.Value));
					//	lblSave.Text = "Data Saved Sucessfully.";
					//	lblSave.Visible = true;
					//}
				}
			}
			catch (Exception ex) {
				lblError.Text = "Data Not Saved.";
				lblError.Visible = true;
			}
		}

		protected void btnCancelSave_Click(object sender, EventArgs e) {

		}

		protected void btnPhotos_Click(object sender, EventArgs e) {
			if (hfView.Value == "1")
				Session["View"] = 1;
			Session["pageIndex"] = 0;
			var url = "~/protected/LeadsImagesUpload.aspx";
			Response.Redirect(url);
		}

		protected void btnGenerateRepLetter_Click(object sender, EventArgs e) {
			int LeadId = Convert.ToInt32(hfLeadsId.Value);

			if (LeadId > 0) {				
				var _leads = LeadsManager.GetByLeadId(LeadId);

				if (!validateForm((Leads)_leads)) {
					lblError.Text = "All or some of the Insurance Information is missing. Please verify information.";
					lblError.Visible = true;
					return;
				}

				string InsurancecompanyName = null;// _leads.InsuranceCompanyName == null ? "" : _leads.InsuranceCompanyName.ToString();
				string InsuranceContactName = null;	// _leads.InsuranceContactName == null ? "" : _leads.InsuranceContactName.ToString();
				string InsuranceAddress = null;// _leads.InsuranceAddress == null ? "" : _leads.InsuranceAddress.ToString();
				string iCity = _leads.CityMaster == null ? "" : _leads.CityMaster.CityName;

				string iState = "";
				if (_leads.StateMaster != null) {
					iState = _leads.StateMaster.StateCode == null ? "" : _leads.StateMaster.StateCode.ToString();
				}
				string zip = _leads.Zip == null ? "" : _leads.Zip.ToString();
				string izip = null;// leads.InsuranceZipCode == null ? "" : _leads.InsuranceZipCode;
				string insfulladdress = iState + " " + zip;

				string _firstName = _leads.ClaimantFirstName == null ? "" : _leads.ClaimantFirstName.ToString();
				string _lastName = _leads.ClaimantLastName == null ? "" : _leads.ClaimantLastName.ToString();
				string Claimant_Name = _firstName + ' ' + _lastName;
				string BusinessName = _leads.BusinessName == null ? "" : _leads.BusinessName.ToString();
				string ClaimantAddress = _leads.LossAddress == null ? "" : _leads.LossAddress.ToString();

				string City = _leads.CityMaster == null ? "" : _leads.CityMaster.CityName;
				string State = "";
				if (_leads.StateMaster != null) {
					State = _leads.StateMaster.StateCode == null ? "" : _leads.StateMaster.StateCode;
				}

				string fulladdress = State + " " + izip;

				string policy = null;// _leads.InsurancePolicyNumber == null ? "" : _leads.InsurancePolicyNumber.ToString();



				StringBuilder sb = new StringBuilder();

				string url = string.Empty;
				url = "http://demo.claimruler.com/images/claim_ruler_logo.jpg";
				//url = "http://csg.sandyclaims.org/images/lg.jpg";
				//url = Request.PhysicalApplicationPath + "Images/lg.jpg";
				
				
				
				//string url = string.Empty;
				//string domain1;
				//Uri urls = HttpContext.Current.Request.Url;
				//domain1 = urls.AbsoluteUri.Replace(urls.PathAndQuery, string.Empty);
				//url = domain1 + "/Images/";

				sb.Append("<html>");
				sb.Append("<head>");
				//sb.Append("<style type='text/css'>");
				//sb.Append("body {margin:5%;width:90%;}");
				//sb.Append("</style>");
				sb.Append("</head>");
				sb.Append("<body  >");
				sb.Append("<form>");
				sb.Append("<div align='center'>");
				//sb.Append("<p style='width: 20px; height: 20px;'><img src='" + url + "lg.jpg' style='height: 20px; width: 20px' /></p>");
				sb.Append("<p style='width: 20px; height: 20px;'><img src='" + url + "' style='height: 20px; width: 20px' /></p>");
				sb.Append("<p style='font-family: Times New Roman; font-size: 14px;'>");
				sb.Append("401 East Las Olas Blvd #130-356<br />");
				sb.Append(" Fort Lauderdale, FL 33301");
				sb.Append("</p>");
				sb.Append("</div>");
				sb.Append("<div style='height: 25px'>");
				sb.Append(" &nbsp;</div>");
				sb.Append("<div style='text-align: center;'>");
				sb.Append("<p style='font-family: Times New Roman; font-size: 16px; font-weight: bold;'>");
				sb.Append("Notice of Representation and Lien on Claim</p>");
				sb.Append("</div>");
				sb.Append("<div style='font-family: Times New Roman; font-size: 16px;'>");
				sb.Append("" + DateTime.Now.Date.ToString("MMMM dd, yyyy") + "<br />");
				sb.Append("<br />");
				sb.Append("" + InsurancecompanyName + "<br />");
				sb.Append("" + InsuranceAddress + "<br />");
				sb.Append("" + City + ", " + fulladdress + "<br />");
				//sb.Append(""+insfulladdress+"<br />");
				//sb.Append("" + fulladdress + "<br />");

				// 2013-03-11 tortega
				sb.Append("<p>Insurance Company Contact:<br/>");
				//sb.Append(_leads.InsuranceContactName + "<br/>");
				//sb.Append(_leads.InsuranceContactPhone + "<br/>");
				//sb.Append(_leads.InsuranceContactEmail + "<br/>");
				sb.Append("</p>");

				sb.Append("</div>");
				sb.Append("<div style='height: 30px;'>");
				sb.Append("&nbsp;</div>");
				sb.Append("<div style='font-family: Times New Roman; margin-left: 35px; font-size: 16px;'>");
				sb.Append("<label style='font-weight: bold;'>");
				sb.Append("Re:&nbsp;&nbsp;</label>" + Claimant_Name + "<br/>");
				sb.Append(ClaimantAddress + "<br />");
				
				if (!string.IsNullOrEmpty(_leads.LossAddress2))
					sb.Append(_leads.LossAddress2 + "<br/>");

				sb.Append(iCity + ", " + insfulladdress + "<br />");
				//sb.Append("" + fulladdress + "");
				//sb.Append("" + insfulladdress + "");

				sb.Append("</div>");
				sb.Append("<div style='font-family: Times New Roman; font-size: 16px;'>");
				sb.Append("<br />");
				sb.Append("<label style='font-weight: bold;'>");
				sb.Append("Insurance Co:&nbsp;&nbsp;</label>" + InsurancecompanyName);
				sb.Append("<br />");
				sb.Append("<label style='font-weight: bold;'>Policy #:&nbsp;&nbsp;</label>" + policy + "");
				sb.Append("<br />");
				sb.Append("<label style='font-weight: bold; margin-left:187px;'>Claim #:&nbsp;&nbsp;</label>" + _leads.ClaimsNumber);
				sb.Append("</div>");
				sb.Append("<div style='font-family: Times New Roman; font-size: 16px;'>");
				sb.Append("<p>");
				sb.Append("Please be advised that we represent the above named client with regard to damages and claim<br />");
				sb.Append("arising from Superstorm Sandy.  Attached is a copy of our Contract For Services.");
				sb.Append("</p>");

				// 2013-03-11 tortega
				sb.Append("<p>");
				sb.Append("Please be advised that Claims Strategies Group has a lien to the extent of its fees on all<br/>");
				sb.Append("proceeds and agreement with the client that it be named as a party on all payments.<br/>");
				sb.Append("The correct title of the company to be named is: <b>Claim Ruler Example Public Adjsuter</b>");
				sb.Append("</p>");

				sb.Append("<p>");
				sb.Append("At this time we have not completed our review of or estimate of the client’s claim.<br />");
				sb.Append("We understand that your adjuster has attended the property.  Please provide the following<br />");
				sb.Append("information at the earliest time:");
				sb.Append("</p>");
				sb.Append("<p style='margin-left:20px;'>");
				sb.Append("1&nbsp;&nbsp;&nbsp;A complete copy of the policy of insurance.<br />");
				sb.Append("2&nbsp;&nbsp;&nbsp;A complete copy of the application for insurance.<br />");
				sb.Append("3&nbsp;&nbsp;&nbsp;A complete copy of any estimates completed and photographs taken.<br />");
				sb.Append("4&nbsp;&nbsp;&nbsp;The name and contact information for your adjuster who will be appointed for any<br />");
				sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;re-inspection necessary or the handling adjuster so we may correspond further.");

				sb.Append("</p>");
				sb.Append("<p>");
				sb.Append("If you have any questions, please contact us:<br />");
				sb.Append("<br />");
				sb.Append("Thank you,");

				sb.Append("</p>");
				sb.Append("</div>");
				sb.Append("<div style='height:25px;'></div>");
				sb.Append("<div style='font-family: Times New Roman; font-size: 16px;'>");
				sb.Append("Jane Doe<br />");
				sb.Append("Claims Processor<br />");
				sb.Append("Office: +1 (732) 555-1212<br />");
				sb.Append("Fax +1 (866) 555-1212<br />");
				sb.Append("E-mail:<a href='#' >processor@publicadjusterexample.com</a><br />");
				sb.Append("Web: <a href='#' > http://www.examplepublicadjuster.com</a><br />");
				sb.Append("</div>");
				sb.Append("</form>");
				sb.Append("</body>");
				sb.Append("</html>");




				Response.AppendHeader("Content-Type", "application/msword");
				Response.AppendHeader("Content-disposition", "attachment; filename=Rep Letter .doc");
				Response.Write(sb);
			}

		}

		protected void btnReturnToClaim_Click(object sender, EventArgs e) {
			if (Session["LeadIds"] != null) {
				Response.Redirect("~/protected/newlead.aspx?id=" + Session["LeadIds"].ToString());
			}
		}

		protected void btnComments_Click(object sender, EventArgs e) {
			if (hfView.Value == "1")
				Session["View"] = 1;
			var url = "~/protected/admin/LeadComments.aspx";
			Response.Redirect(url);
		}

		protected bool validateForm(Leads lead) {
			bool isValid = true;

			//if (	string.IsNullOrEmpty(lead.InsuranceCompanyName) ||
			//	string.IsNullOrEmpty(lead.InsuranceContactName) ||
			//	string.IsNullOrEmpty(lead.InsuranceContactPhone) ||
			//	string.IsNullOrEmpty(lead.InsuranceContactEmail) ||
			//	string.IsNullOrEmpty(lead.InsuranceAddress) ||
			//	(lead.InsuranceCity ?? 0) == 0 ||
			//	(lead.InsuranceState ?? 0) == 0 ||
			//	string.IsNullOrEmpty(lead.InsuranceZipCode) ||
			//	string.IsNullOrEmpty(lead.InsurancePolicyNumber)
			//	)
			//	isValid = false;

			return isValid;

		}

	}
}