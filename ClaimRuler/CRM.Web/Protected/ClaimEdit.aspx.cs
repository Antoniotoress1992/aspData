using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Transactions;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Core;
using CRM.Data;
using CRM.Data.Account;
using CRM.Repository;
using CRM.Web.Utilities;

using CRM.RuleEngine;
using CRM.Data.Entities;

using CRM.Web.UserControl.Admin;
using System.Configuration;
using System.Data.SqlClient;

using System.Data;

namespace CRM.Web.Protected {
	public partial class ClaimEdit : System.Web.UI.Page {
        string connectionString = ConfigurationManager.ConnectionStrings["ClaimRuler"].ConnectionString; 
		public int claimID {
			get {
				string decryptedClaimNumber = null;
				int id = 0;
				// check user specified claim id on query string
				if (Request.QueryString["id"] != null) {
					decryptedClaimNumber = Core.SecurityManager.DecryptQueryString(Request.QueryString["id"].ToString());
					int.TryParse(decryptedClaimNumber, out id);
				}
				else
					id = Session["ClaimID"] != null ? Convert.ToInt32(Session["ClaimID"]) : 0;

				return id;
			}
			set {
				Session["ClaimID"] = value;
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			string dencryptedID = null;
			bool isPrimeSessionFields = false;



			// access master page from nested master page
			Protected.ClaimRuler masterPage = Master.Master as Protected.ClaimRuler;

			// check user permission
			bool hasEditPermission = Core.PermissionHelper.checkEditPermission("UsersLeads.aspx");
			//lbtnSave.Visible = hasEditPermission;
			//btnLetter.Visible = hasEditPermission;
			btnEmail.Visible = hasEditPermission;

			Master.showTopMenu(hasEditPermission);
			//btnAutoInvoice.Visible = Core.PermissionHelper.checkAction((int)Globals.Actions.GenerateInvoice); -OC 9/2/14 


			//masterPage.checkPermission();


			Form.DefaultButton = btnSave_hidden.UniqueID;

			// hide return to claim button
			Master.enableBackToClaimButton = false;

			if (!Page.IsPostBack) {
				if (Request.Params["q"] != null) {
					dencryptedID = Core.SecurityManager.DecryptQueryString(Request.Params["q"].ToString());
					this.claimID = Convert.ToInt32(dencryptedID);
				}
				else
					isPrimeSessionFields = Request.QueryString["id"] != null;

				 claimEdit.bindData(this.claimID, isPrimeSessionFields);

			}
		}

		protected void btnSave_Click(object sender, EventArgs e) {
			Page.Validate();

			//if (Page.IsValid) {
				// save claim
				claimEdit.saveForm();
                Login login = new Login();
                login.formatException();
                login.setRulexception();
                
			//}

		}

		#region Ajax Methods
		[System.Web.Services.WebMethod]
		public static void saveFile(int claimID, string filePath, string documentDescription, int documentCategory) {
			string claimDocumentPath = null;
			string destinationFilePath = null;
			ClaimDocument document = null;

			// get application path
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			// get file from temp folder
			string tempFilePath = HttpContext.Current.Server.MapPath(String.Format("~\\Temp\\{0}", filePath));

			if (claimID > 0 && File.Exists(tempFilePath)) {
				try {
					using (TransactionScope scope = new TransactionScope()) {
						document = new ClaimDocument();
						document.ClaimID = claimID;
						document.DocumentName = filePath;
						document.Description = documentDescription;
						document.DocumentDate = DateTime.Now;
						document.IsPrint = true;
						document.DocumentCategoryID = documentCategory;

						ClaimDocumentManager.Save(document);

						claimDocumentPath = string.Format("{0}/ClaimDocuments/{1}/{2}", appPath, claimID, document.ClaimDocumentID);

						if (!Directory.Exists(claimDocumentPath))
							Directory.CreateDirectory(claimDocumentPath);

						destinationFilePath = claimDocumentPath + "/" + filePath;

						System.IO.File.Copy(tempFilePath, destinationFilePath, true);

						// delete temp file
						File.Delete(tempFilePath);

						scope.Complete();
					}
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
				finally {

				}
			}
		}


		#endregion

		protected void btnGenerateInvoice_hidden_Click(object sender, EventArgs e) {
			claimEdit.generateInvoice();

		}

	}
}