using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CRM.Data;
using CRM.Data.Account;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public partial class AdjusterEdit : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			//Master.checkPermission();
		}

		[System.Web.Services.WebMethod]
		public static void saveLicenseFile(int adjusterID, int stateID, string filePath) {
			string imageFolderPath = null;
			string destinationFilePath = null;

			// get application path
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			// get uploaded file
			string tempFilePath = HttpContext.Current.Server.MapPath(String.Format("~\\Temp\\{0}", filePath));

			if (adjusterID > 0 && File.Exists(tempFilePath)) {
				try {

					imageFolderPath = string.Format("{0}/Adjusters/{1}/License", appPath, adjusterID);

					if (!Directory.Exists(imageFolderPath))
						Directory.CreateDirectory(imageFolderPath);


					//destinationFilePath = HttpContext.Current.Server.MapPath("~/LeadsImage/" + LeadId.ToString() + "/" + objLeadImage.LeadImageId.ToString() + "/" + e.FileName);
					destinationFilePath = string.Format("{0}/{1}{2}", imageFolderPath, stateID, Path.GetExtension(filePath));

					System.IO.File.Copy(tempFilePath, destinationFilePath, true);

					// delete temp file
					File.Delete(tempFilePath);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}				
			}
		}
		[System.Web.Services.WebMethod]
		public static void saveAdjusterPhoto(int adjusterID, string filePath) {			
			AdjusterMaster adjuster = null;
			string photoFileName = null;

			string imageFolderPath = null;
			string destinationFilePath = null;

			// get application path
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			// get uploaded file
			string tempFilePath = HttpContext.Current.Server.MapPath(String.Format("~\\Temp\\{0}", filePath));

			adjuster = AdjusterManager.GetAdjusterId(adjusterID);

			if (adjuster != null && File.Exists(tempFilePath)) {
				try {
					imageFolderPath = string.Format("{0}/Adjusters/{1}/Photo", appPath, adjuster.AdjusterId);

					if (!Directory.Exists(imageFolderPath))
						Directory.CreateDirectory(imageFolderPath);

					photoFileName = adjuster.AdjusterId.ToString() + Path.GetExtension(filePath);

					destinationFilePath = string.Format("{0}/{1}", imageFolderPath, photoFileName);

					System.IO.File.Copy(tempFilePath, destinationFilePath, true);

					// delete temp file
					File.Delete(tempFilePath);
					
					adjuster.PhotoFileName = photoFileName;

					AdjusterManager.Save(adjuster);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}				
			}
		}
	}
}