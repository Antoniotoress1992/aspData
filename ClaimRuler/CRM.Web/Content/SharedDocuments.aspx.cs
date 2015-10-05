using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.Content {
	public partial class SharedDocuments : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			if (Request.Params["q"] != null)
				bindData();

			
		}
		private void bindData() {
			int leadID = 0;
			int claimID = 0;
			Leads lead = null;
			Claim claim = null;
			List<LeadsDocument> leadDocuments = null;
			List<ClaimDocument> claimDocuments = null;
			List<DocumentList> allDocuments = new List<DocumentList>();

			int.TryParse(Core.SecurityManager.DecryptQueryString(Request.Params["q"].ToString()), out claimID);

			if (claimID > 0) {
				claim = ClaimsManager.Get(claimID);
				if (claim != null) {
					lead = claim.LeadPolicy.Leads;

					lblName.Text = lead.insuredName;
					
					// lead documents
					leadDocuments = LeadsUploadManager.getLeadsDocumentForExportByLeadID(leadID);
					if (leadDocuments != null && leadDocuments.Count > 0) {
						foreach (LeadsDocument leadDocument in leadDocuments) {
							DocumentList doc = new DocumentList();
							doc.DocumentName = leadDocument.DocumentName;
							doc.Description = leadDocument.Description;

							doc.DocumentPath = string.Format("~/LeadsDocument/{0}/{1}/{2}",
												leadDocument.LeadId,
												leadDocument.LeadDocumentId,		// document id
												leadDocument.DocumentName);		// document file name

							allDocuments.Add(doc);
						}						
					}

					// claim documents
					claimDocuments = ClaimDocumentManager.GetAll(claimID);
					if (claimDocuments != null && claimDocuments.Count > 0) {
						foreach (ClaimDocument claimDocument in claimDocuments) {
							DocumentList doc = new DocumentList();
							doc.DocumentName = claimDocument.DocumentName;
							doc.Description = claimDocument.Description;

							doc.DocumentPath = string.Format("~/ClaimDocuments/{0}/{1}/{2}",
												claimDocument.ClaimID,
												claimDocument.ClaimDocumentID,		// document id
												claimDocument.DocumentName);		// document file name
							allDocuments.Add(doc);
						}
					}

					gvDocument.DataSource = allDocuments;
					gvDocument.DataBind();
				}
			}
		}

		protected void gvDocument_RowDataBound(object sender, GridViewRowEventArgs e) {
			DocumentList document = null;
			string fileExtension = null;

			if (e.Row.RowType == DataControlRowType.DataRow) {
				if (e.Row.DataItem != null) {
					document = e.Row.DataItem as DocumentList;

					fileExtension = Path.GetExtension(document.DocumentName);

					Image img = e.Row.FindControl("imgDocumentType") as Image;
					switch (fileExtension) {
						case ".pdf":
							img.ImageUrl = "~/Images/pdf.png";
							break;

						case ".doc":
						case ".docx":
							img.ImageUrl = "~/Images/word.png";
							break;

						case ".xlsx":
						case ".csv":
							img.ImageUrl = "~/Images/excel.png";
							break;

						case ".jpg":
						case ".png":
						case ".bmp":
						case ".gif":
							img.ImageUrl = "~/Images/picture.gif";
							break;

						default:
							break;
					}

					HyperLink lnk = e.Row.FindControl("lnkDocument") as HyperLink;

					if (lnk != null) {

						lnk.Text = document.DocumentName;
						lnk.NavigateUrl = document.DocumentPath;
					}
				}
			}
		}
	}
}