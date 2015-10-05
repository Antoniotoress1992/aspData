using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.SessionState;
using CRM.Core;
using CRM.Data.Account;
using CRM.Data;
using System.Transactions;
using LinqKit;
using CRM.Data.Entities;
namespace CRM.Web {
	/// <summary>
	/// Summary description for FileUploaderHandler
	/// </summary>
	public class FileUploaderHandler : IHttpHandler, IReadOnlySessionState {
		public bool IsReusable { get { return true; } }

		public void ProcessRequest(HttpContext context) {
			string LeadId = "";
			int policyTypeID = 0;

			if (context.Session["LeadIds"] != null) {
				LeadId = Convert.ToString(context.Session["LeadIds"]);
			}
			HttpPostedFile fileToUpload = context.Request.Files["Filedata"];


			string ext = System.IO.Path.GetExtension(fileToUpload.FileName);
			string ActualFileName = fileToUpload.FileName.Substring(fileToUpload.FileName.LastIndexOf(@"\") + 1);
			string FileNameWithoutExt = ActualFileName.Replace(ext, "");
			if (ext != ".doc" && ext != ".exl" && ext != ".pdf") {
				
                
				LeadsImage objLeadImage = new LeadsImage();
				objLeadImage.LeadId = Convert.ToInt32(LeadId);
				objLeadImage.ImageName = ActualFileName;
				objLeadImage.Status = 1;
				
				if (context.Session["policyTypeID"] != null)
					policyTypeID = (int) context.Session["policyTypeID"];

				objLeadImage.policyTypeID = policyTypeID;

				// 2013-09-24 tortega
				objLeadImage.isPrint = true;

				objLeadImage = LeadsUploadManager.SaveImage(objLeadImage);

				if (Convert.ToInt32(LeadId) > 0) {
					if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/LeadsImage/" + Convert.ToInt32(LeadId) + "/" + objLeadImage.LeadImageId)))
						Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/LeadsImage/" + Convert.ToInt32(LeadId) + "/" + objLeadImage.LeadImageId));

					fileToUpload.SaveAs(HttpContext.Current.Server.MapPath("~/LeadsImage/" + Convert.ToInt32(LeadId) + "/" + objLeadImage.LeadImageId + "/" + ActualFileName));

				}
			}

			//string pathToSave = HttpContext.Current.Server.MapPath("~/TempLeadsImage/") + LeadId + "/" +fileToUpload.FileName;


			//if (!Directory.Exists(HttpContext.Current.Server.MapPath("~\\TempLeadsImage\\" + LeadId)))
			//    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~\\TempLeadsImage\\" + LeadId));

			//fileToUpload.SaveAs(pathToSave);

		}

	}
}