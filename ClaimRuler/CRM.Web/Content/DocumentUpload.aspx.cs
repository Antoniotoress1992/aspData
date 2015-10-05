using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.Content {
	public partial class DocumentUpload : System.Web.UI.Page {
		string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

		protected void Page_Load(object sender, EventArgs e) {
			// 2013-08-06 tortega
			if (Session["UserId"] == null) {
				FormsAuthentication.SignOut();
				FormsAuthentication.RedirectToLoginPage();
			}
				
		}
		protected void btnUploadDoc_Click(object sender, EventArgs e) {
			string ext;
			string ActualFileName = "", SavedFileName = string.Empty;
			string FileNameWithoutExt = "";
			DateTime fileDate = DateTime.MinValue;
			string OccasionId = string.Empty;
			string Occasion = string.Empty;
			int LeadID = Convert.ToInt32(Session["LeadIds"]);

			Page.Validate("DocUpload");
			if (!Page.IsValid)
				return;

			string policyType = Page.Request.Params["t"].ToString();
			int policyTypeID = (int)Enum.Parse(typeof(PolicyType), policyType);

			try {
				if (FileUpload2.HasFile) {
					LeadsDocument objLeadDoc = new LeadsDocument();

					ext = System.IO.Path.GetExtension(FileUpload2.PostedFile.FileName);
					ActualFileName = FileUpload2.PostedFile.FileName.Substring(FileUpload2.PostedFile.FileName.LastIndexOf(@"\") + 1);
					FileNameWithoutExt = ActualFileName.Replace(ext, "");
					objLeadDoc.LeadId = LeadID;
					objLeadDoc.Description = txtDescriptionDoc.Text;
					objLeadDoc.DocumentName = ActualFileName;
					objLeadDoc.Status = 1;

					DateTime.TryParse(hf_lastModifiedDate.Value, out fileDate);

					objLeadDoc.InsertDate = fileDate;

					objLeadDoc.PolicyTypeID = policyTypeID;

					LeadsDocument objld = LeadsUploadManager.SaveDocument(objLeadDoc);

					if (objld.LeadDocumentId > 0) {
						if (!Directory.Exists(appPath + "/LeadsDocument/" + LeadID + "/" + objld.LeadDocumentId)) {
							Directory.CreateDirectory(appPath + "/LeadsDocument/" + LeadID + "/" + objld.LeadDocumentId);
						}

						FileUpload2.PostedFile.SaveAs(appPath + "/LeadsDocument/" + LeadID + "/" + objld.LeadDocumentId + "/" + ActualFileName);

						//lblSave.Text = string.Empty;
						//lblSave.Text = "Document Uploaded Successfully";
						//lblSave.Visible = true;
						txtDescriptionDoc.Text = string.Empty;
						//bindDocuments(LeadID);

						string js = "<script type='text/javascript'>closeRefresh();</script>";

						ClientScript.RegisterClientScriptBlock(typeof(Page), "commentk", js);
					}
				}
			}
			catch (Exception ex) {
				Core.EmailHelper.emailError(ex);

				//lblError.Text = string.Empty;
				//lblError.Text = "There is a problem to upload.";
				//lblError.Visible = true;
			}

		}

	}
}