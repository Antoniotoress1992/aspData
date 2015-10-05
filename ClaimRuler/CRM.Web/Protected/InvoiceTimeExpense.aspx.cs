using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;
using CRM.RuleEngine;

using Microsoft.Reporting.WebForms;
using Infragistics.Web.UI.EditorControls;
using CRM.Data.Entities;
using System.Text;
using CRM.Core;

namespace CRM.Web.Protected {
	public partial class InvoiceTimeExpense : System.Web.UI.Page {
		protected List<InvoiceView> invoiceReport = null;
		protected decimal invoiceTotalAmount = 0;
		protected void Page_Load(object sender, EventArgs e) {
			// access master page from nested master page
			Protected.ClaimRuler masterPage = Master.Master as Protected.ClaimRuler;

			// check user permission
			masterPage.checkPermission("LeadInvoiceLedger.aspx");

			if (!Page.IsPostBack) {
				bindData();
			}
		}

		protected void bindData() {
			int claimID = 0;
			int clientID = 0;
			int policyID = 0;
			Claim claim = null;

			Leads lead = null;

			// get id for current lead
			claimID = Core.SessionHelper.getClaimID();

			// get client id
			clientID = Core.SessionHelper.getClientId();

			// get current policy
			policyID = Core.SessionHelper.getPolicyID();

			using (ClaimManager repository = new ClaimManager()) {
				claim = repository.Get(claimID);
			}
			if (claim == null)
				return;

			// get lead/claim
			lead = claim.LeadPolicy.Leads;


			if (claim.LeadPolicy != null && claim.LeadPolicy.LeadPolicyType != null) {
				// get policy type
				lblPolicyType.Text = claim.LeadPolicy.LeadPolicyType.Description;
			}

			if (claim.LeadPolicy != null && claim.LeadPolicy != null) {
				// get policy number
				lblPolicyNumber.Text = claim.LeadPolicy.PolicyNumber;
			}

			// get insurer claim number
			lblInsurerClaimNumber.Text = claim.InsurerClaimNumber;

			// get policy information
			if (lead != null) {
				lblClient.Text = string.Format("<b>{0} {1}<br/>{2}<br/>{3}<br/>{4}, {5} {6}</b>",
						lead.ClaimantFirstName ?? "",		//0
						lead.ClaimantLastName ?? "",		//1
						lead.LossAddress ?? "",			//2
						lead.LossAddress2 ?? "",			//3
						lead.CityName ?? "",			//4
						lead.StateName ?? "",			//5
						lead.Zip ?? ""					//6
						);

			}


			bindTimeExpenseForInvoice(claimID);
		}

		private void bindTimeExpenseForInvoice(int claimID) {
			List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
			InvoiceDetail invoiceDetail = null;
			decimal serviceRate = 0;
			decimal servicePercentage = 0;
			decimal rate = 0;

			List<ClaimService> claimServices = null;
			List<ClaimExpense> claimExpenses = null;

			using (ClaimServiceManager repository = new ClaimServiceManager()) {
				claimServices = repository.GetAll(claimID);
			}
			using (ClaimExpenseManager repository = new ClaimExpenseManager()) {
				claimExpenses = repository.GetAll(claimID);
			}

			// add claim services to invoice
			if (claimServices != null) {
				foreach (ClaimService service in claimServices) {
					invoiceDetail = new InvoiceDetail();
					invoiceDetail.LineDate = service.ServiceDate;
					invoiceDetail.ServiceTypeID = service.ServiceTypeID;
					invoiceDetail.LineDescription = service.InvoiceServiceType.ServiceDescription;
					invoiceDetail.Qty = service.ServiceQty;
					invoiceDetail.Comments = service.ServiceDescription;

					serviceRate = service.InvoiceServiceType.ServiceRate ?? 0;
					servicePercentage = service.InvoiceServiceType.ServicePercentage ?? 0;

					if (serviceRate > 0) {
						rate = serviceRate;
					}
					else {
						rate = servicePercentage / 100;
					}

					invoiceDetail.LineAmount = invoiceDetail.Qty * rate;
					invoiceDetail.Rate = rate;

					// add service to invoicedetail list
					invoiceDetails.Add(invoiceDetail);
				}
			}
			// add claim expenses to invoice
			if (claimExpenses != null) {
				foreach (ClaimExpense expense in claimExpenses) {

					invoiceDetail = new InvoiceDetail();
					invoiceDetail.ExpenseTypeID = expense.ExpenseTypeID;
					invoiceDetail.LineDate = expense.ExpenseDate;
					invoiceDetail.LineDescription = expense.ExpenseDescription;
					invoiceDetail.Comments = expense.ExpenseDescription;
					invoiceDetail.LineAmount = expense.ExpenseAmount;
					invoiceDetail.Qty = 1;
					invoiceDetail.Rate = expense.ExpenseAmount;
					// add expense to invoicedetail list
					invoiceDetails.Add(invoiceDetail);


				}
			}

			// bind invoice detail lines
			gvTimeExpense.DataSource = invoiceDetails;
			gvTimeExpense.DataBind();
		}

		private void bindInvoiceDetailLines(int invoiceID) {
			List<InvoiceDetail> invoiceDetails = null;

			invoiceDetails = InvoiceDetailManager.GetInvoiceDetails(invoiceID);

			gvTimeExpense.DataSource = invoiceDetails;
			gvTimeExpense.DataBind();

		}
		protected void btnEmailInvoice_Click(object sender, EventArgs e) {
			string reportPath = null;
			int invoiceID = 0;
			Invoice invoice = null;
			int port = 0;
			string password = null;
			CRM.Data.Entities.SecUser user = null;
			string subject = "Invoice";
			int userID = Core.SessionHelper.getUserId();
			string bodyText = "Invoiced is attached.";

			//Page.Validate("emailInvoice");
			//if (!Page.IsValid)
			//	return;

			try {
				invoiceID = Convert.ToInt32(ViewState["InvoiceID"].ToString());

				reportPath = generateInvoicePDF(invoiceID);

				user = SecUserManager.GetByUserId(userID);

				if (user != null && !string.IsNullOrEmpty(reportPath)) {
					invoice = InvoiceManager.Get(invoiceID);

					string[] recipients = txtEmailTo.Text.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

					string[] attachments = new string[] { reportPath };

					port = Convert.ToInt32(user.emailHostPort);

					password = Core.SecurityManager.Decrypt(user.emailPassword);

					Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, bodyText, attachments, user.emailHost, port, user.Email, password);

					// add copy of invoice to documet list
					updateDocumentLog(reportPath, invoice.ClaimID);

					lblMessage.Text = "Invoice mailed successfully.";
					lblMessage.CssClass = "ok";
				}
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
				lblMessage.Text = "Unable to email invoice.";
				lblMessage.CssClass = "error";
			}
		}

		protected void btnPrint_Click(object sender, EventArgs e) {
			int invoiceID = 0;
			int.TryParse(ViewState["InvoiceID"].ToString(), out invoiceID);

			string encryptedID = Core.SecurityManager.EncryptQueryString(ViewState["InvoiceID"].ToString());

			if (invoiceID > 0) {
				string js = string.Format("window.open('../Content/PrintInvoice.aspx?q={0}', 'invoice', 'status=0, toolbar=0, width=800, height=800');", encryptedID);

				ScriptManager.RegisterStartupScript(Page, typeof(Page), "Print_Invoice", js, true);
			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			Page.Validate("invoice");
			if (!Page.IsValid)
				return;

			saveInvoice();
		}

		protected string generateInvoicePDF(int invoiceID) {
			string reportPath = null;

			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			invoiceReport = InvoiceManager.GetInvoiceForReport(invoiceID);

			if (invoiceReport != null) {
				reportViewer.Reset();

				reportViewer.LocalReport.DataSources.Clear();

				reportViewer.LocalReport.EnableExternalImages = true;

				ReportDataSource reportDataSource = new ReportDataSource();

				reportDataSource.Name = "DataSet1";

				reportDataSource.Value = invoiceReport;

				reportViewer.LocalReport.DataSources.Add(reportDataSource);

				reportViewer.LocalReport.ReportPath = Server.MapPath("~/Content/Invoice.rdlc");

				reportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subReportsEventHandler);

				reportPath = string.Format("{0}/Temp/Invoice_{1}.pdf", appPath, invoiceID);

				Core.ReportHelper.savePDFFromLocalReport(reportViewer.LocalReport, reportPath);
			}

			return reportPath;
		}

		protected InvoiceDetail getInvoiceDetailLine(GridViewRow row) {
			decimal rate = 0;
			decimal totalAmount = 0;
			int? serviceTypeID = 0;
			DateTime date = DateTime.MaxValue;

			InvoiceDetail invoiceDetailLine = null;

			CheckBox cbxInclude = row.FindControl("cbxInclude") as CheckBox;

			if (cbxInclude.Checked) {
				invoiceDetailLine = new InvoiceDetail();

				// invoiceLineID
				invoiceDetailLine.InvoiceLineID = (int)gvTimeExpense.DataKeys[row.RowIndex].Values[0]; 

				// service date
				Label lblLineDate = row.FindControl("lblLineDate") as Label;
				if (!string.IsNullOrEmpty(lblLineDate.Text))
					invoiceDetailLine.LineDate = Convert.ToDateTime(lblLineDate.Text);


				// service description
				Label lblLineDescription = row.FindControl("lblLineDescription") as Label;

				// service type
				serviceTypeID = (int?)gvTimeExpense.DataKeys[row.RowIndex].Values[1];

				invoiceDetailLine.ServiceTypeID = serviceTypeID;

				// expense type id 
				invoiceDetailLine.ExpenseTypeID = (int?)gvTimeExpense.DataKeys[row.RowIndex].Values[2]; 


				invoiceDetailLine.LineDescription = lblLineDescription.Text;

				// quantity
				Label lblQty = row.FindControl("lblQty") as Label;
				invoiceDetailLine.Qty = string.IsNullOrEmpty(lblQty.Text) ? 0 : Convert.ToDecimal(lblQty.Text);

				// rate
				Label lblRate = row.FindControl("lblRate") as Label;

				rate = string.IsNullOrEmpty(lblRate.Text) ? 0 : Convert.ToDecimal(lblRate.Text);

				invoiceDetailLine.Rate = rate;


				// total amount
				// quantity
				Label lblLineTotal = row.FindControl("lblLineTotal") as Label;

				decimal.TryParse(lblLineTotal.Text.Trim().Replace(",", "").Replace("$", ""), out totalAmount);
				invoiceDetailLine.LineAmount = totalAmount;


				// comments
				Label lblComments = row.FindControl("lblComments") as Label;
				invoiceDetailLine.Comments = lblComments.Text.Trim();

				invoiceDetailLine.isBillable = true;
			}

			return invoiceDetailLine;
		}

		protected void subReportsEventHandler(object sender, SubreportProcessingEventArgs e) {
			e.DataSources.Add(new ReportDataSource("DataSet1", invoiceReport[0].invoiceLines));
		}

		private void updateDocumentLog(string reportPath, int claimID) {
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();
			string documentPath = null;

			ClaimDocument objLeadDoc = new ClaimDocument();

			string ActualFileName = Path.GetFileName(reportPath);

			string FileNameWithoutExt = Path.GetFileNameWithoutExtension(reportPath);

			objLeadDoc.ClaimID = claimID;

			objLeadDoc.Description = "Invoice";

			objLeadDoc.DocumentName = ActualFileName;
			objLeadDoc.IsPrint = false;

			objLeadDoc.DocumentDate = DateTime.Now;

			objLeadDoc = ClaimDocumentManager.Save(objLeadDoc);

			if (objLeadDoc.ClaimDocumentID > 0) {

				documentPath = string.Format("{0}/ClaimDocuments/{1}/{2}", appPath, claimID, objLeadDoc.ClaimDocumentID);
				if (!Directory.Exists(documentPath)) {
					Directory.CreateDirectory(documentPath);
				}

				File.Copy(reportPath, documentPath + "/" + ActualFileName, true);
			}
		}

		protected void addInvoiceToClaimDiary(Invoice invoice, InvoiceDetail invoiceDetail) {
			ClaimComment comment = null;

			// retrieve comment by reference
			//comment = ClaimCommentManager.GetLeadCommentByReferenceId(invoiceDetail.InvoiceLineID);

			if (comment == null)
				comment = new ClaimComment();

			comment.UserId = Core.SessionHelper.getUserId();
			comment.CommentDate = DateTime.Now;
			comment.ClaimID = invoice.ClaimID;
			//comment.ReferenceID = invoiceDetail.InvoiceLineID;
			comment.IsActive = true;	// active
			comment.CommentText = string.Format("<div>Invoice # {0} - {1:MM-dd-yyyy} for {2} Qty:{3:N2} Rate:{4:N2} Item Total:{5:N2}</div><div>{6}</div>",
				invoice.InvoiceNumber,
				invoiceDetail.LineDate,
				invoiceDetail.LineDescription ?? "",
				invoiceDetail.Qty ?? 0,
				invoiceDetail.Rate ?? 0,
				invoiceDetail.LineAmount ?? 0,
				invoiceDetail.Comments ?? ""
				);

			ClaimCommentManager.Save(comment);
		}

		protected void gvTimeExpense_RowDataBound(object sender, GridViewRowEventArgs e) {
			if (e.Row.RowType == DataControlRowType.DataRow) {
				if (e.Row.DataItem != null) {
					InvoiceDetail invoiceDetail = e.Row.DataItem as InvoiceDetail;
					this.invoiceTotalAmount += invoiceDetail.LineAmount ?? 0;
				}

			}
			else if (e.Row.RowType == DataControlRowType.Footer) {
				Label lblInvoiceTotal = (Label)e.Row.FindControl("lblInvoiceTotal");
				lblInvoiceTotal.Text = invoiceTotalAmount.ToString("c");
			}
		}
		
		private void showToolbarButtons() {
			btnPrint.Visible = true;

			lbtnEmail.Visible = true;
		}

		private void saveInvoice() {
			int clientID = 0;
			int invoiceID = 0;
			int InvoiceLineID = 0;
			Invoice invoice = null;
			InvoiceDetail invoiceDetailLine = null;
			InvoiceDetail invoiceDetail = null;
		

			int nextInvoiceNumber = 0;
			decimal taxAmount = 0;


			clientID = Core.SessionHelper.getClientId();

			invoiceID = ViewState["InvoiceID"] == null ? 0 : Convert.ToInt32(ViewState["InvoiceID"].ToString());


			if (invoiceID == 0) {
				invoice = new Invoice();

				// get id for current lead
				invoice.ClaimID = Core.SessionHelper.getClaimID();

				invoice.IsVoid = false;
			}
			else {
				invoice = InvoiceManager.Get(invoiceID);

			}


			//invoice.PolicyID = policy.PolicyType;

			invoice.InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
			invoice.DueDate = Convert.ToDateTime(txtDueDate.Text);
			invoice.BillToName = txtBillTo.Text.Trim();
			invoice.BillToAddress1 = txtBillToAddress1.Text.Trim();
			invoice.BillToAddress2 = txtBillToAddress2.Text.Trim();
			invoice.BillToAddress3 = txtBillToAddress3.Text.Trim();


			try {
				using (TransactionScope scope = new TransactionScope()) {
					if (invoiceID == 0) {
						// assign next invoice number to new invoice
						nextInvoiceNumber = InvoiceManager.GetNextInvoiceNumber(clientID);

						invoice.InvoiceNumber = nextInvoiceNumber;
					}

					invoiceID = InvoiceManager.Save(invoice);

					// save newly generated invoice id
					ViewState["InvoiceID"] = invoiceID.ToString();
					
					#region get detail line from gridview
					foreach (GridViewRow row in gvTimeExpense.Rows) {
						// get detail line from grid
						invoiceDetailLine = getInvoiceDetailLine(row);

						if (invoiceDetailLine != null) {
							if (invoiceDetailLine.InvoiceLineID > 0)
								invoiceDetail = InvoiceDetailManager.Get(InvoiceLineID);
							else
								invoiceDetail = new InvoiceDetail();

							// update fields
							invoiceDetail.InvoiceID = invoiceID;
							invoiceDetail.ServiceTypeID = invoiceDetailLine.ServiceTypeID;
							invoiceDetail.ExpenseTypeID = invoiceDetailLine.ExpenseTypeID;
							invoiceDetail.Comments = invoiceDetailLine.Comments;
							invoiceDetail.isBillable = invoiceDetailLine.isBillable;
							invoiceDetail.LineAmount = invoiceDetailLine.LineAmount;
							invoiceDetail.LineDate = invoiceDetailLine.LineDate;
							invoiceDetail.LineDescription = invoiceDetailLine.LineDescription;
							invoiceDetail.Qty = invoiceDetailLine.Qty;
							invoiceDetail.Rate = invoiceDetailLine.Rate;
							invoiceDetail.UnitDescription = invoiceDetailLine.UnitDescription;
							invoiceDetail.Total = invoiceDetailLine.Total;

							// save invoice detail
							InvoiceDetailManager.Save(invoiceDetail);												
						}
					} // foreach
					#endregion

					// update invoice total after adding 
					invoice = InvoiceManager.Get(invoiceID);

					invoice.TotalAmount = invoice.InvoiceDetail.Where(x => x.isBillable == true).Sum(x => x.LineAmount);

					taxAmount = (invoice.TotalAmount ?? 0) * (invoice.TaxRate / 100);

					InvoiceManager.Save(invoice);

					// add invoice to claim diary comment
					addInvoiceToClaimDiary(invoice, invoiceDetail);

					// 2014-05-02 apply rule
					using (SpecificExpenseTypePerCarrier ruleEngine = new SpecificExpenseTypePerCarrier()) {
						RuleException ruleException = ruleEngine.TestRule(clientID, invoice);

						if (ruleException != null) {
							ruleException.UserID = Core.SessionHelper.getUserId();
							ruleEngine.AddException(ruleException);
                            CheckSendMail(ruleException);
						}
					}
				

					// complete transaction
					scope.Complete();

				} // using


				// refresh invoice number on UI
				txtInvoiceNumber.Text = (invoice.InvoiceNumber ?? 0).ToString();


				showToolbarButtons();

				lblMessage.Text = "Invoice was generated successfully.";
				lblMessage.CssClass = "ok";
			}
			catch (Exception ex) {
				lblMessage.Text = "Error while saving invoice.";
				lblMessage.CssClass = "error";
				Core.EmailHelper.emailError(ex);
			}
			
		}

		protected void cbxAll_CheckedChanged(object sender, EventArgs e) {
			CheckBox cbxAll = null;

			foreach (GridViewRow row in gvTimeExpense.Rows) {
				if (gvTimeExpense.HeaderRow != null) {
					cbxAll = gvTimeExpense.HeaderRow.FindControl("cbxAll") as CheckBox;
				}
				
				if (row.RowType == DataControlRowType.DataRow) {
					CheckBox cbxInclude = row.FindControl("cbxInclude") as CheckBox;
					cbxInclude.Checked = cbxAll.Checked;
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




	}
}