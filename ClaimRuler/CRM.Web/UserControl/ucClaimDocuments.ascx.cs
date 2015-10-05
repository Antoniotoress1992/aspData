using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;
using CRM.Data.Entities;
using CRM.Core;
using Microsoft.Exchange.WebServices.Data;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using CRM.Web.Protected.Admin;
using CRM.Web.Protected;

namespace CRM.Web.UserControl {
	public partial class ucClaimDocuments : System.Web.UI.UserControl {
		protected CRM.Web.Protected.ClaimRuler masterPage = null;
        static FindItemsResults<Item> instanceResults;
		private int claimID {
			get {
				return Session["ClaimID"] != null ? Convert.ToInt32(Session["ClaimID"]) : 0;
			}
			set {
				Session["ClaimID"] = value;
			}
		}
		private int policyID {
			get {
				return Session["policyID"] != null ? Convert.ToInt32(Session["policyID"]) : 0;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {

		}
		protected void Page_Init(object sender, EventArgs e) {
			// this is fired before Page_load
			masterPage = (CRM.Web.Protected.ClaimRuler)this.Page.Master.Master;
		}

        protected void btnDocumentsSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKeyword.Text))
            {
                lblErrorMSg.Visible = true;
            }
            else
            {
                lblErrorMSg.Visible = false;
                int userID = SessionHelper.getUserId();

                CRM.Data.Entities.SecUser secUser = SecUserManager.GetByUserId(userID);
                string email = secUser.Email;
                string password = SecurityManager.Decrypt(secUser.emailPassword);
                string url = "https://" + secUser.emailHost + "/EWS/Exchange.asmx";

                try
                {
                    ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                    service.Credentials = new WebCredentials(email, password);
                    service.Url = new Uri(url);

                    ItemView view = new ItemView(int.MaxValue);

                    Microsoft.Exchange.WebServices.Data.SearchFilter searchFilter = new Microsoft.Exchange.WebServices.Data.SearchFilter.ContainsSubstring(ItemSchema.Body, txtKeyword.Text);
                    instanceResults = service.FindItems(WellKnownFolderName.Inbox, searchFilter, view);

                    DataTable table = GetTable();

                    foreach (var item in instanceResults.Items)
                    {
                        item.Load();
                        //Console.Clear();
                        EmailMessage msg = item as EmailMessage;
                        DataRow row = table.NewRow();

                        string to = string.Empty;
                        foreach (var rec in msg.ToRecipients)
                            to = to + rec.Address + ";";

                        row["Subject"] = msg.Subject;
                        string body = ExtractHtmlInnerText(msg.Body);
                        if (body.Length > 2000)
                            body = body.Substring(0, 2000);
                        row["Body"] = "From :" + msg.From.Address + "\nTo :" + to + "\nSubject: " + msg.Subject + "\n" + body;
                        row["No.of Attachments"] = msg.Attachments.Count.ToString();
                        row["Date"] = msg.DateTimeReceived.Date.ToShortDateString();
                        table.Rows.Add(row);
                    }
                    gvSearchResult.DataSource = table;
                    gvSearchResult.DataBind();

                    pnlSearchResult.Visible = true;
                }
                catch (Exception ex)
                {
                    lblErrorMSg.Text = "Incorrect Email Settings";
                    lblErrorMSg.Visible = true;
                }

            }
        }

        private static string ExtractHtmlInnerText(string htmlText)
        {
            //Match any Html tag (opening or closing tags) 
            // followed by any successive whitespaces
            //consider the Html text as a single line

            Regex regex = new Regex("(<.*?>\\s*)+", RegexOptions.Singleline);

            // replace all html tags (and consequtive whitespaces) by spaces
            // trim the first and last space

            string resultText = regex.Replace(htmlText, " ").Trim();

            return resultText;
        }
        private DataTable GetTable()
        {
            // Here we create a DataTable with four columns.
            DataTable table = new DataTable();
            table.Columns.Add("Subject", typeof(string));
            table.Columns.Add("Body", typeof(string));
            table.Columns.Add("No.of Attachments", typeof(string));
            table.Columns.Add("Date", typeof(string));
            return table;
        }
        protected void btnDocumentsCancel_Click(object sender, EventArgs e)
        {
            pnlSearch.Visible = false;
            lblErrorMSg.Visible = false;
        }
		public void bindData(int claimID) {

           
			hf_claimID.Value = claimID.ToString();

			gvDocuments.DataSource = ClaimDocumentManager.GetAll(claimID);
			gvDocuments.DataBind();
            
			bindHistoricalDocuments();

			bindDocumentCategory(ddlDocumentCategory);
		}

		private void bindHistoricalDocuments() {
			int leadID = Core.SessionHelper.getLeadId();

			// load legacy documents
			List<LeadsDocument> documents = LeadsUploadManager.getLeadsDocumentByLeadID(leadID);

			gvHistoricalDocuments.DataSource = documents;
			gvHistoricalDocuments.DataBind();

		}
		
		protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e) {
			int documentID = Convert.ToInt32(e.CommandArgument);
			ClaimDocument claimDocument = null;

			if (e.CommandName == "DoDelete") {
				try {
					ClaimDocumentManager.Delete(documentID);										
					
					bindData(this.claimID);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}
			else if (e.CommandName == "DoEdit") {
				ViewState["ClaimDocumentID"] = e.CommandArgument.ToString();

				claimDocument = ClaimDocumentManager.Get(documentID);
				if (claimDocument != null) {
					pnlEditClaimDocument.Visible = true;
					pnlGridPanel.Visible = false;

					txtDcoumentDescription.Text = claimDocument.Description;

					bindDocumentCategory(ddlDocumentCategoryEdit);

					ddlDocumentCategoryEdit.SelectedValue = (claimDocument.DocumentCategoryID ?? 0).ToString();
				}
			}
		}

		private void bindDocumentCategory(DropDownList ddl) {
			List<DocumentCategory> documentCategories = null;

			using (DocumentCategoryManager repository = new DocumentCategoryManager()) {
				documentCategories = repository.GetAll();
			}

			Core.CollectionManager.FillCollection(ddl, "DocumentCategoryID", "CategoryName", documentCategories);
		}

		protected void cbxPrint_CheckedChanged(object sender, EventArgs e) {
			ClaimDocument document = null;
			
			CheckBox cbx = sender as CheckBox;
			
			GridViewRow row = cbx.NamingContainer as GridViewRow;

			try {
				int documentID = (int)gvDocuments.DataKeys[row.RowIndex].Value;

				document = ClaimDocumentManager.Get(documentID);
				if (document != null) {
					document.IsPrint = cbx.Checked;

					document = ClaimDocumentManager.Save(document);

					bindData(this.claimID);
				}
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);
			}
		}

		protected void gvDocuments_RowDataBound(object sender, GridViewRowEventArgs e) {
			string appUrl = ConfigurationManager.AppSettings["appURL"].ToString();
			
			int leadId = Core.SessionHelper.getLeadId();

			ClaimDocument document = e.Row.DataItem as ClaimDocument;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				HyperLink hlnkDocument = e.Row.FindControl("hlnkDocument") as HyperLink;

				hlnkDocument.NavigateUrl = string.Format("{0}/ClaimDocuments/{1}/{2}/{3}", appUrl, document.ClaimID, document.ClaimDocumentID, document.DocumentName);
				hlnkDocument.Text = document.DocumentName;				
			}
		}

		protected void btnRefresh_Click(object sender, EventArgs e) {
			bindData(this.claimID);
		}

		protected void btnSaveDocumentEdit_Click(object sender, EventArgs e) {
			int claimDocumentID = Convert.ToInt32(ViewState["ClaimDocumentID"]);
			ClaimDocument claimDocument = null;

			Page.Validate("document");
			if (!Page.IsValid)
				return;

			claimDocument = ClaimDocumentManager.Get(claimDocumentID);
			
			if (claimDocument != null) {
				claimDocument.Description = txtDcoumentDescription.Text.Trim();

				if (ddlDocumentCategoryEdit.SelectedIndex > 0)
					claimDocument.DocumentCategoryID = Convert.ToInt32(ddlDocumentCategoryEdit.SelectedValue);

				ClaimDocumentManager.Save(claimDocument);
			}

			pnlEditClaimDocument.Visible = false;
			pnlGridPanel.Visible = true;

			gvDocuments.DataSource = ClaimDocumentManager.GetAll(this.claimID);
			gvDocuments.DataBind();
		}

		protected void btnCanelDocumentEdit_Click(object sender, EventArgs e) {
			pnlEditClaimDocument.Visible = false;
			pnlGridPanel.Visible = true;

			bindData(this.claimID);
		}

		protected void gvHistoricalDocuments_RowDataBound(object sender, GridViewRowEventArgs e) {
			string appUrl = ConfigurationManager.AppSettings["appURL"].ToString();

			int leadId = Core.SessionHelper.getLeadId();

			LeadsDocument document = e.Row.DataItem as LeadsDocument;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				HyperLink hlnkDocument = e.Row.FindControl("hlnkDocument") as HyperLink;

				hlnkDocument.NavigateUrl = string.Format("{0}/LeadsDocument/{1}/{2}/{3}", appUrl, document.LeadId, document.LeadDocumentId, document.DocumentName);
				hlnkDocument.Text = document.DocumentName;
			}
		}

        protected void linkSearch_Click(object sender, EventArgs e)
        {
            pnlSearch.Visible = true;
        }

        protected void gvSearchResult_DataBound(object sender, EventArgs e)
        {
            //gvSearchResult.Columns[1].ItemStyle.Width = Convert.ToInt32(gvSearchResult.Width) / 3;
            //gvSearchResult.Columns[2].ItemStyle.Width = Convert.ToInt32(gvSearchResult.Width) / 2;
            //gvSearchResult.Columns[3].ItemStyle.Width = 100;
            //gvSearchResult.Columns[4].ItemStyle.Width = 100;
        }

        protected void btnDocumentsSave_Click(object sender, EventArgs e)
        {
            for (int rowCount = 0; rowCount < gvSearchResult.Rows.Count; rowCount++)
            {
                CheckBox box = (CheckBox)gvSearchResult.Rows[rowCount].Cells[0].Controls[1];
                if (box.Checked)
                {
                    int commentID = 0;
                    ClaimComment comment = null;

                    comment = new ClaimComment();
                    comment.ClaimID = this.claimID;
                    comment.IsActive = true;
                    comment.UserId = Core.SessionHelper.getUserId();
                    comment.CommentDate = DateTime.Now;

                    string serviceUrl = ConfigurationManager.AppSettings["importServiceUrl"];
                    serviceUrl = serviceUrl + "/" + "ExportNotes?commentId=" + commentID;
                    Common.SendRequest(serviceUrl);
                    if (comment != null && comment.ClaimID > 0)
                    {
                        comment.CommentText = gvSearchResult.Rows[rowCount].Cells[2].Text;

                        try
                        {
                            ClaimCommentManager.Save(comment);
                            EmailMessage msg = instanceResults.Items[rowCount] as EmailMessage;
                            if (msg.HasAttachments)
                            {
                                foreach (Attachment attachment in msg.Attachments)
                                {
                                    if (attachment is FileAttachment)
                                    {
                                        FileAttachment fileAttachment = attachment as FileAttachment;

                                        // Load the attachment into a file.
                                        // This call results in a GetAttachment call to EWS.
                                        string tempName = HttpContext.Current.Server.MapPath(String.Format("~\\Temp\\{0}", fileAttachment.Name));
                                        fileAttachment.Load(tempName);
                                        ClaimEdit.saveFile(claimID, fileAttachment.Name, "Attachment from email " + msg.Subject, 6);
                                        File.Delete(tempName);
                                    }
                                }
                            }
                            lblErrorMSg.Text = "Email(s) Saved successfully";
                            lblErrorMSg.Visible = true;
                        }
                        catch (Exception ex)
                        {
                            lblErrorMSg.Text = "Mail not saved!";
                            lblErrorMSg.CssClass = "error";
                            Core.EmailHelper.emailError(ex);
                        }
                    }
                }
            }
        }

        protected void btnDocumentsClear_Click(object sender, EventArgs e)
        {
            gvSearchResult.DataSource = null;
            gvSearchResult.DataBind();
        }

	}
}