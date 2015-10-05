using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;

using Infragistics.WebUI.WebHtmlEditor;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class InvoiceApproval : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			
			//checkPermissions();

			if (!Page.IsPostBack) {
				bindData();
			}			
		}

		public static void addReportToClaimDocument(string reportPath, int claimID, int invoiceNumber) {
			string claimDocumentPath = null;
			string destinationFilePath = null;

			// get application path
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			ClaimDocument claimDocument = new ClaimDocument();
			claimDocument.ClaimID = claimID;
			claimDocument.IsPrint = true;
			claimDocument.Description = "Invoice";
			claimDocument.DocumentDate = DateTime.Now;
			claimDocument.DocumentName = string.Format("Invoice_{0}.pdf", invoiceNumber);

			// report category: service bill
			claimDocument.DocumentCategoryID = 9;

			claimDocument = ClaimDocumentManager.Save(claimDocument);

			claimDocumentPath = string.Format("{0}/ClaimDocuments/{1}/{2}", appPath, claimID, claimDocument.ClaimDocumentID);

			if (!Directory.Exists(claimDocumentPath))
				Directory.CreateDirectory(claimDocumentPath);

			destinationFilePath = claimDocumentPath + string.Format("/Invoice_{0}.pdf", invoiceNumber);

			System.IO.File.Copy(reportPath, destinationFilePath, true);

			// delete temp file
			//File.Delete(reportPath);
		}

		[System.Web.Services.WebMethod]
		public static void approveInvoice(string sInvoiceID) {
			int invoiceID = Convert.ToInt32(Core.SecurityManager.DecryptQueryString(sInvoiceID));

			Invoice invoice = null;

			invoice = InvoiceManager.Get(invoiceID);
			if (invoice != null) {
				invoice.IsApprove = true;

				InvoiceManager.Save(invoice);

				string reportPath = Core.ReportHelper.generateInvoicePDF(invoiceID);

				addReportToClaimDocument(reportPath, invoice.ClaimID, (int)invoice.InvoiceNumber);
			}
		}

		private void batchApproveInvoices() {
			int invoiceID = 0;
			Invoice invoice = null;

			try {
				using (TransactionScope scope = new TransactionScope()) {
					foreach (GridViewRow row in gvInvoiceQ.Rows) {
						if (row.RowType == DataControlRowType.DataRow) {
							CheckBox cbxSelected = row.FindControl("cbxSelected") as CheckBox;

							if (cbxSelected.Checked) {
								invoiceID = (int)gvInvoiceQ.DataKeys[row.RowIndex].Values[0];

								invoice = InvoiceManager.Get(invoiceID);
								if (invoice != null) {
									invoice.IsApprove = true;

									InvoiceManager.Save(invoice);

									string reportPath = Core.ReportHelper.generateInvoicePDF(invoiceID);

									addReportToClaimDocument(reportPath, invoice.ClaimID, (int)invoice.InvoiceNumber);
								}
							}
						}
					}
					// complete transaction
					scope.Complete();
				}

				lblMessage.Text = "Selected invoices have been approved.";
				lblMessage.CssClass = "ok";
			}
			catch (Exception ex) {
				lblMessage.Text = "Selected invoices were not approved.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}
			finally {
				bindData();
			}

		}

		private void batchDeleteInvoices() {
			Claim claim = null;
			int invoiceID = 0;
			Invoice invoice = null;

			try {
				using (TransactionScope scope = new TransactionScope()) {
					foreach (GridViewRow row in gvInvoiceQ.Rows) {
						if (row.RowType == DataControlRowType.DataRow) {
							CheckBox cbxSelected = row.FindControl("cbxSelected") as CheckBox;

							if (cbxSelected.Checked) {
								invoiceID = (int)gvInvoiceQ.DataKeys[row.RowIndex].Values[0];

								invoice = InvoiceManager.Get(invoiceID);
								if (invoice != null) {
									// make invoice as rejected/void
									invoice.IsVoid = true;
									
									InvoiceManager.Save(invoice);

									// change claim progress status
									claim = ClaimsManager.GetByID(invoice.ClaimID);
									if (claim != null) {
										claim.ProgressStatusID = (int)Globals.ClaimProgressStatus.ClaimInvoiceRejectedRedo;

										ClaimsManager.Save(claim);
									}
								}
							}
						}
					}

					// complete transaction
					scope.Complete();
				}

				lblMessage.Text = "Selected invoices have been rejected.";
				lblMessage.CssClass = "ok";
			}
			catch (Exception ex) {
				lblMessage.Text = "Selected invoices were not rejected.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}
			finally {
				bindData();
			}
		}

		private void batchEmailInvoices() {
			string[] attachments = null;
			StringBuilder bodyText = new StringBuilder();
			int invoiceID = 0;
			int invoiceNumber = 0;
			List<string> invoicePaths = new List<string>();
			int port = 0;
			string password = null;
			string[] recipients = null;
			string reportPath = null;
			string subject = null;
            CRM.Data.Entities.SecUser user = null;

			int userID = Core.SessionHelper.getUserId();

			try {
				recipients = txtEmailTo.Text.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

				user = SecUserManager.GetByUserId(userID);

				if (user != null && recipients.Length > 0) {
					port = Convert.ToInt32(user.emailHostPort);

					password = Core.SecurityManager.Decrypt(user.emailPassword);

					bodyText.Append(txtEmailText.Text);

					// add user signature
					bodyText.Append("<br><br>" + user.emailSignature ?? "");

					foreach (GridViewRow row in gvInvoiceQ.Rows) {
						if (row.RowType == DataControlRowType.DataRow) {
							CheckBox cbxSelected = row.FindControl("cbxSelected") as CheckBox;

							if (cbxSelected.Checked) {
								invoiceID = (int)gvInvoiceQ.DataKeys[row.RowIndex].Values[0];
								invoiceNumber = (int)gvInvoiceQ.DataKeys[row.RowIndex].Values[1];

								reportPath = Core.ReportHelper.generateInvoicePDF(invoiceID);

								subject = string.Format("Invoice Number {0}", invoiceNumber);
								
								attachments = new string[] { reportPath };

								Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, bodyText.ToString(), attachments, user.emailHost, port, user.Email, password);

								cbxSelected.Checked = false;
							}
						}
					}
					
					lblMessage.Text = "Selected invoices have been emailed.";
					lblMessage.CssClass = "ok";
				}
				
			}
			catch (Exception ex) {
				lblMessage.Text = "Selected invoices were not emailed.";
				lblMessage.CssClass = "error";

				Core.EmailHelper.emailError(ex);
			}
		}
		
		private void bindData() {
			List<vw_InvoiceApprovalQueue> filterInvoices = null;

			filterInvoices = getInvoicesFromQueue();

			gvInvoiceQ.DataSource = filterInvoices;
			gvInvoiceQ.DataBind();
		}

		private string generateInvoicePDF(int invoiceID) {
			string reportPath = null;

			if (invoiceID > 0) {
				try {
					reportPath = Core.ReportHelper.generateInvoicePDF(invoiceID);			
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);				
				}				
			}

			return reportPath;
		}

		protected void btnEmailInvoice_Click(object sender, EventArgs e) {
			string decryptedInvoiceID = null;
			string reportPath = null;
			int invoiceID = 0;
			int port = 0;
			string password = null;
            CRM.Data.Entities.SecUser user = null;
			string subject = "Invoice";
			string bodyText = "Invoice is attached.";

			int userID = Core.SessionHelper.getUserId();


			decryptedInvoiceID = Core.SecurityManager.DecryptQueryString(hf_encryptedInvoiceID.Value);

			int.TryParse(decryptedInvoiceID, out invoiceID);

			if (invoiceID > 0) {
				try {
					reportPath = Core.ReportHelper.generateInvoicePDF(invoiceID);

					user = SecUserManager.GetByUserId(userID);

					if (user != null && !string.IsNullOrEmpty(reportPath)) {
						//invoice = InvoiceManager.Get(invoiceID);

						string[] recipients = txtEmailTo.Text.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

						string[] attachments = new string[] { reportPath };

						port = Convert.ToInt32(user.emailHostPort);

						password = Core.SecurityManager.Decrypt(user.emailPassword);

						Core.EmailHelper.sendEmail(user.Email, recipients, null, subject, bodyText, attachments, user.emailHost, port, user.Email, password);

						lblMessage.Text = "Invoice emailed successfully.";
						lblMessage.CssClass = "ok";
					}
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
					lblMessage.Text = "Unable to email invoice.";
					lblMessage.CssClass = "error";
				}
				finally {
					// reset
					hf_encryptedInvoiceID.Value = "0";
				}
			}
		}
		protected void btnRefreshgvInvoiceQ_Click(object sender, EventArgs e) {
			bindData();
		}
				
		protected void cbxSelectedAll_CheckedChanged(object sender, EventArgs e) {
			CheckBox cbxSelectedAll = sender as CheckBox;

			foreach (GridViewRow row in gvInvoiceQ.Rows) {
				CheckBox cbxSelected = row.FindControl("cbxSelected") as CheckBox;
				cbxSelected.Checked = cbxSelectedAll.Checked;
			}
		}

		private void checkPermissions() {
			int roleID = SessionHelper.getUserRoleId();

			Master.checkPermission();

			if (roleID != (int)UserRole.Client) {
				// check permission for each row action
				Infragistics.Web.UI.NavigationControls.DataMenuItem menuItemDelete = ContextMenu.Items.FindDataMenuItemByKey("delete");
				menuItemDelete.Visible = Master.hasDeletePermission;

				Infragistics.Web.UI.NavigationControls.DataMenuItem menuItemEdit = ContextMenu.Items.FindDataMenuItemByKey("edit");
				menuItemEdit.Visible = Master.hasEditPermission;

				// check permission for batch action
				Infragistics.Web.UI.NavigationControls.DataMenuItem menuItemDelete_batch = ContextMenu_Batch.Items.FindDataMenuItemByKey("delete");
				menuItemDelete_batch.Visible = Master.hasDeletePermission;			
			}
		}

		[System.Web.Services.WebMethod]
		public static void deleteInvoice(string sInvoiceID) {
			Invoice invoice = null;
			Claim claim = null;

			try {
				int invoiceID = Convert.ToInt32(Core.SecurityManager.DecryptQueryString(sInvoiceID));

				using (TransactionScope scope = new TransactionScope()) {
					invoice = InvoiceManager.Get(invoiceID);
					if (invoice != null) {
						invoice.IsVoid = true;

						InvoiceManager.Save(invoice);

						// change claim progress status
						claim = ClaimsManager.GetByID(invoice.ClaimID);
						if (claim != null) {
							claim.ProgressStatusID = (int)Globals.ClaimProgressStatus.ClaimInvoiceRejectedRedo;

							ClaimsManager.Save(claim);

							scope.Complete();
						}						
					}
				}
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
			}

		}

		private List<vw_InvoiceApprovalQueue> getInvoicesFromQueue() {
			int clientID = SessionHelper.getClientId();
			int roleID = SessionHelper.getUserRoleId();

			IQueryable<vw_InvoiceApprovalQueue> invoices = null;
			List<vw_InvoiceApprovalQueue> filterInvoices = null;
			List<SecRoleInvoiceApprovalPermission> rules = null;

			// load invoices
			invoices = InvoiceManager.GetInvoiceApprovalQueue(clientID);
            
            SecRoleModule accountingList = null;
            accountingList = SecRoleModuleManager.GetByRoleIdAccounting(roleID);//NEW OC 10/14/14 added to show invoices depending on role manager(not by client only)

			if (roleID == (int)UserRole.Client || accountingList.ViewPermission == true) 
            {
               //InvoiceType invoiceType = null;
               // foreach(vw_InvoiceApprovalQueue ipq in invoices)
               // {
               //     int typeID = Convert.ToInt32( ipq.InvoiceTypeID);
               //     invoiceType = InvoiceTypeManager.GetByID(typeID);
               //     string invoiceTypeDescription = invoiceType.InvoiceTypes;
                    
               //     Label lblInv = (Label)gvInvoiceQ.FindControl("lblInvoiceType") as Label;
               //     lblInv.Text = invoiceTypeDescription;
               //     //Session["invoiceTypeDescription"] = invoiceType.InvoiceTypes;
                        
               // }
				// load all invoices for client
				filterInvoices = invoices.ToList();
                      
                
			}
			else 
            {
				filterInvoices = new List<vw_InvoiceApprovalQueue>();
                
				// get invoice approval rules for role
				rules = InvoiceApprovalRuleManager.GetAll(roleID);

				// enforce approval rules
				if (rules != null && rules.Count > 0) {
					foreach (SecRoleInvoiceApprovalPermission rule in rules) {
						foreach (vw_InvoiceApprovalQueue invoiceq in invoices) {
							if (invoiceq.TotalAmount >= rule.AmountFrom && invoiceq.TotalAmount <= rule.AmountTo) {
								filterInvoices.Add(invoiceq);
							}
						}
					}
				}
			}

			return filterInvoices;
		}

		protected void gvInvoiceQ_Sorting(object sender, GridViewSortEventArgs e) {
			int clientID = SessionHelper.getClientId();
			string sortExpression = null;

			//IQueryable<vw_InvoiceApprovalQueue> invoices = null;
			List<vw_InvoiceApprovalQueue> invoices = null;

			//invoices = InvoiceManager.GetInvoiceApprovalQueue(clientID);			
			invoices = getInvoicesFromQueue();

			bool descending = false;

			if (ViewState[e.SortExpression] == null)
				descending = false;
			else
				descending = !(bool)ViewState[e.SortExpression];

			ViewState[e.SortExpression] = descending;

			sortExpression = e.SortExpression + (descending ? " desc" : " asc");

			gvInvoiceQ.DataSource = invoices.sort(sortExpression);

			gvInvoiceQ.DataBind();
		}

		protected void gvInvoiceQ_RowCommand(object sender, GridViewCommandEventArgs e) {

		}

		protected void gvInvoiceQ_RowDataBound(object sender, GridViewRowEventArgs e) {
			vw_InvoiceApprovalQueue q = null;
			string encryptedID = null;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				if (e.Row.DataItem != null) {
					q = e.Row.DataItem as vw_InvoiceApprovalQueue;

					HyperLink hlnkAction = e.Row.FindControl("hlnkAction") as HyperLink;

					encryptedID = Core.SecurityManager.EncryptQueryString(q.InvoiceID.ToString());

					//hlnkAction.Attributes["onclick"] = string.Format("$('#{0}').click(function(e){{showMenu(e,'{1}');}});", hlnkAction.ClientID, encryptedID);

					hlnkAction.Attributes["onclick"] = string.Format("javascript:return showMenu(event,'{0}');", encryptedID);
				}
			}
		}

		protected void btnBatchEmailInvoice_Click(object sender, EventArgs e) {
			batchEmailInvoices();			
		}

		protected void btnBatchApproveInvoice_Click(object sender, EventArgs e) {
			batchApproveInvoices();
		}

		protected void btnBatchDeleteInvoice_Click(object sender, EventArgs e) {
			batchDeleteInvoices();
		}

        protected void lbtnClaim_Click(object sender, EventArgs e)
        {
            Claim claim = null;
            string url = "~/Protected/ClaimEdit.aspx";
            string claimNumber = ((LinkButton)(sender)).ValidationGroup;

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

	}
}