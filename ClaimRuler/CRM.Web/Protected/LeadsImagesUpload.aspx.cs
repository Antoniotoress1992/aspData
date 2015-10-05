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
using CRM.Repository;

using Infragistics.Web.UI.EditorControls;
using ComputerBeacon.Json;
using CRM.Data.Entities;

namespace CRM.Web.Protected.Admin {
	public class ClaimImageView {
		public string location { get; set; }
		public string description { get; set; }
	}

	public partial class LeadsImagesUpload : System.Web.UI.Page {

		int leadID = 0;
			
		public int claimID {
			get { return Session["ClaimID"] == null ? 0 : Convert.ToInt32(Session["ClaimID"]); }
		}
		public int userID {
			get { return Session["UserId"] == null ? 0 : Convert.ToInt32(Session["UserId"]); }
		}

		protected void Page_Load(object sender, EventArgs e) {

			// access master page from nested master page
			Protected.ClaimRuler masterPage = Master.Master as Protected.ClaimRuler;

			// check user permission
			// masterPage.checkPermission();


			leadID = Core.SessionHelper.getLeadId();			
					

			if (!Page.IsPostBack) {
				legacyPhotos.bindData(leadID);

				claimPhotos.bindData(claimID);
			}
		}

		#region AJAX Methods
		[System.Web.Services.WebMethod]
		public static string getClaimPhoto(int claimImageID) {
			string result = null;

			ClaimImage claimImage = ClaimImageManager.Get(claimImageID);
			ClaimImageView claimView = null;

			if (claimImage != null) {
				claimView = new ClaimImageView {
					location = claimImage.Location,
					description = claimImage.Description 
				};				
			}
			result = ComputerBeacon.Json.Serializer.Serialize(claimView);
			return result;
		}

		[System.Web.Services.WebMethod]
		public static string getLeadPhoto(int leadImageID) {
			string result = null;
			LeadsImage leadImage = LeadsUploadManager.GetLeadsImageById(leadImageID);		
			ClaimImageView claimView = null;

			if (leadImage != null) {
				claimView = new ClaimImageView {
					location = leadImage.Location,
					description = leadImage.Description
				};
			}
			result = ComputerBeacon.Json.Serializer.Serialize(claimView);
			return result;
		}
		[System.Web.Services.WebMethod]
		public static void togglePrintFlag(bool isPrint, int leadImageID) {
			LeadsImage leadImage = LeadsUploadManager.GetLeadsImageById(leadImageID);

			if (leadImage != null) {
				leadImage.isPrint = isPrint;

				try {
					LeadsUploadManager.SaveImage(leadImage);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}

			}
		}
		[System.Web.Services.WebMethod]
		public static void saveLeadPhotoDescription(int leadImageID, string photoLocation, string photoDescription) {
			LeadsImage leadImage = LeadsUploadManager.GetLeadsImageById(leadImageID);

			if (leadImage != null) {
				leadImage.Location = photoLocation.Trim();
				leadImage.Description = photoDescription.Trim();

				try {
					LeadsUploadManager.SaveImage(leadImage);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}

			}
		}
		[System.Web.Services.WebMethod]
		public static void saveClaimPhotoDescription(int claimImageID, string photoLocation, string photoDescription) {

            
            

			ClaimImage claimImage = ClaimImageManager.Get(claimImageID);

			if (claimImage != null) {
				claimImage.Location = photoLocation.Trim();
				claimImage.Description = photoDescription.Trim();

				try {
					ClaimImageManager.Save(claimImage);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}

			}
		}
		#endregion

		protected void webUpload1_OnUploadFinishing(object sender, UploadFinishingEventArgs e) {
			string imageFolderPath = null;
			string destinationFilePath = null;									
			
			// get application path
			string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

			// get uploaded file
			string tempFilePath = String.Format("{0}{1}", e.FolderPath, e.TemporaryFileName);

			if (this.claimID > 0 && File.Exists(tempFilePath)) {
				try {
					ClaimImage claimImage = new ClaimImage();
					claimImage.ClaimID = claimID;
					claimImage.ImageName = e.FileName;
					claimImage.IsActive = true;
					claimImage.UserID = userID;

					claimImage.IsPrint = true;

					claimImage = ClaimImageManager.Save(claimImage);

					imageFolderPath = string.Format("{0}/ClaimImage/{1}/{2}", appPath, this.claimID, claimImage.ClaimImageID);

					if (!Directory.Exists(imageFolderPath))
						Directory.CreateDirectory(imageFolderPath);

					destinationFilePath = imageFolderPath + "/" + e.FileName;

					System.IO.File.Copy(tempFilePath, destinationFilePath);

					// delete temp file
					File.Delete(tempFilePath);

					// refresh photos
					claimPhotos.bindData(claimID);
				}
				catch (Exception ex) {
					Core.EmailHelper.emailError(ex);
				}
			}
		}

	}
}