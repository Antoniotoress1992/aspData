using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infragistics.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class FileUploadController : Controller
    {
        private static Dictionary<String, Dictionary<string, string>> UploadedFiles
        {
            get
            {
                if (System.Web.HttpContext.Current.Cache["UploadedFiles"] == null)
                {
                    System.Web.HttpContext.Current.Cache["UploadedFiles"] = new Dictionary<String, Dictionary<string, string>>();
                }
                return (Dictionary<String, Dictionary<string, string>>)System.Web.HttpContext.Current.Cache["UploadedFiles"];
            }
        }

        [ActionName("aspnet-mvc-helper")]
        public ActionResult AspNetMvcHelper()
        {
            return View("aspnet-mvc-helper");
        }

        [ActionName("upload-progress-manager")]
        public ActionResult UploadProgrManager()
        {
            /* Code for registering the UploadProgressManager event handlers in the Global.asax file
               NOTE: igUpload event handlers should be registered only once
                protected void Application_Start()
                {
                    UploadProgressManager.Instance.AddFinishingUploadEventHandler("serverID3", FileUploadController.HandlerUploadFinishing);
                    UploadProgressManager.Instance.AddStartingUploadEventHandler("serverID3", FileUploadController.HandlerUploadStarting);
                }
            */
            return View("upload-progress-manager");
        }

        // NOTE: igUpload handlers are registered in the Global.asax file
        internal static void HandlerUploadStarting(object sender, UploadStartingEventArgs args)
        {
            Dictionary<String, Dictionary<string, string>> uploadedFiles = FileUploadController.UploadedFiles;
            string fileName = args.FileName;

            if (uploadedFiles.ContainsKey(fileName))
            {
                Dictionary<string, string> fileInfo = null;
                uploadedFiles.TryGetValue(fileName, out fileInfo);
                args.ServerMessage = String.Format("File already exists on server: {0}, {1}, {2}", fileInfo["fileName"], fileInfo["fileSize"], fileInfo["mimeType"]);
                args.Cancel = true;
            }
        }

        // NOTE: igUpload handlers are registered in the Global.asax file
        internal static void HandlerUploadFinishing(object sender, UploadFinishingEventArgs args)
        {
            string filePath = String.Format("{0}{1}", args.FolderPath, args.TemporaryFileName);
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    string folderPath = args.FolderPath;
                    string fileName = args.FileName;
                    string mimeType = args.MimeType;
                    long fileSize = args.FileSize;

                    Dictionary<string, string> fileInfo = new Dictionary<string, string>();
                    fileInfo.Add("fileName", fileName);
                    fileInfo.Add("folderPath", folderPath);
                    fileInfo.Add("mimeType", mimeType);
                    fileInfo.Add("fileSize", fileSize.ToString());

                    Dictionary<String, Dictionary<string, string>> uploadedFiles = FileUploadController.UploadedFiles;
                    uploadedFiles.Add(args.FileName, fileInfo);
                }
                catch (Exception ex)
                {
                    args.ServerMessage = ex.Message;
                }

                args.Cancel = false;
            }
        }
    }
}
