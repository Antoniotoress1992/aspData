using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Infragistics.Web.Mvc;
using IgniteUI.SamplesBrowser.Controllers;

namespace IgniteUI.SamplesBrowser
{
    public class UploadConfig
    {
        public static void RegisterHandlers()
        {
            UploadProgressManager upm = UploadProgressManager.Instance;
            upm.AddFinishingUploadEventHandler("serverID1", UploadConfig.DeleteFile);
            upm.AddFinishingUploadEventHandler("serverID2", UploadConfig.DeleteFile);
            upm.AddStartingUploadEventHandler("serverID3", FileUploadController.HandlerUploadStarting);
            upm.AddFinishingUploadEventHandler("serverID3", FileUploadController.HandlerUploadFinishing);
            upm.AddFinishingUploadEventHandler("serverID3", UploadConfig.DeleteFile);
        }

        protected static void DeleteFile(object sender, UploadFinishingEventArgs e)
        {
            string filePath = String.Format("{0}{1}", e.FolderPath, e.TemporaryFileName);
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception)
                {

                }
            }
        }
    }
}