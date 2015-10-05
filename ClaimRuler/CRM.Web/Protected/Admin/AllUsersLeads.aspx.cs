using CRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.Web.Protected.Admin
{
	public partial class AllUsersLeads : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Ajax Methods
        [System.Web.Services.WebMethod]
        public static string saveFile(int claimID, string filePath, string documentDescription, int documentCategory)
        {
            string claimDocumentPath = null;
            string destinationFilePath = null;
            ClaimDocument document = null;
            string msg = string.Empty;
            // get application path
            string appPath = ConfigurationManager.AppSettings["appPath"].ToString();

            // get file from temp folder
            string tempFilePath = HttpContext.Current.Server.MapPath(String.Format("~\\Temp\\{0}", filePath));

            if (claimID > 0 )//&& File.Exists(tempFilePath)
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        document = new ClaimDocument();
                        document.ClaimID = claimID;
                        document.DocumentName = filePath;
                        document.Description = documentDescription;
                        document.DocumentDate = DateTime.Now;
                        document.IsPrint = true;
                        document.DocumentCategoryID = documentCategory;

                        CRM.Data.Account.ClaimDocumentManager.Save(document);

                        claimDocumentPath = string.Format("{0}/ClaimDocuments/{1}/{2}", appPath, claimID, document.ClaimDocumentID);

                        if (!Directory.Exists(claimDocumentPath))
                            Directory.CreateDirectory(claimDocumentPath);

                        destinationFilePath = claimDocumentPath + "/" + filePath;

                        System.IO.File.Copy(tempFilePath, destinationFilePath, true);

                        // delete temp file
                        File.Delete(tempFilePath);

                        scope.Complete();
                    }
                    msg = "Uploaded";
                }
                catch (Exception ex)
                {
                    Core.EmailHelper.emailError(ex);
                    msg = "Uploaded Fail";
                }
                finally
                {

                }
               
            }
            return msg;
        }


        #endregion
    }
}