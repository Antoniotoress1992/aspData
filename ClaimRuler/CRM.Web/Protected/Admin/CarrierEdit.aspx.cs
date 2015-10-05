using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data.Account;
using CRM.Data;
using CRM.Core;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class CarrierEdit : System.Web.UI.Page {
		List<SecRoleModuleManager.secRoleModuleGet> resRolePermission = new List<SecRoleModuleManager.secRoleModuleGet>();

		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {

				//Master.checkPermission();
			}
		}

		[System.Web.Services.WebMethod]
		public static void saveFile(int carrierID, string filePath, string documentDescription) {
			string carrierDocumentPath = null;
			string destinationFilePath = null;
			CarrierDocument document = null;

			// get application path
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			// get file from temp folder
			string tempFilePath = HttpContext.Current.Server.MapPath(String.Format("~\\Temp\\{0}", filePath));

			if (carrierID > 0 && File.Exists(tempFilePath)) {
				try {
					using (TransactionScope scope = new TransactionScope()) {
						document = new CarrierDocument();
						document.CarrierID = carrierID;
						document.DocumentName = filePath;
						document.DocumentDescription = documentDescription;
						document.DocumentDate = DateTime.Now;
						CarrierDocumentManager.Save(document);

						carrierDocumentPath = string.Format("{0}/CarrierDocuments/{1}", appPath, carrierID);

						if (!Directory.Exists(carrierDocumentPath))
							Directory.CreateDirectory(carrierDocumentPath);

						destinationFilePath = string.Format("{0}/CarrierDocuments/{1}/{2}", appPath, carrierID, filePath);

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
	}
}