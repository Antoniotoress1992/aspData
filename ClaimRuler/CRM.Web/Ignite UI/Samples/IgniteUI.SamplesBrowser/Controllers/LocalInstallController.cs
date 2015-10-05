#region using
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IgniteUI.SamplesBrowser.Application;
using Infragistics.IgniteUI.SamplesBrowser.Shared.ViewModels;
using Infragistics.IgniteUI.SamplesBrowser.Shared.Util;
using Infragistics.IgniteUI.SamplesBrowser.Shared.Entities;
using Infragistics.IgniteUI.SamplesBrowser.Shared.Repositories;
using Infragistics.IgniteUI.SamplesBrowser.Shared.Contracts;
using System.Runtime.Caching;
#endregion

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class LocalInstallController : HomeController
    {
		#region Members        
        string appPath = "";
        string rootHref = "";
        string rootSrc = "";
        LocalInstallPageParser parser = null; 
        private string _rootPath;
		  #endregion

		  #region LocalInstallPageGeneration, IndexLocalInstall
        public ViewResult LocalInstallPageGeneration()
        {                        
            return View();
        }
        public string IndexLocalInstall(IndexViewModel model)
        {
            model.LocalZipInstall = true;  
            return ViewRenderer.RenderView("~/Views/Shared/Index.cshtml", model,
                                                     ControllerContext);
        }
		  #endregion

		  #region InitializeTableOfContentsRepository
        protected override void InitializeTableOfContentsRepository()
        {
            this.TableOfContentsRepository = new TableOfContentsRepository(this.Resources, this.FileSystem, true);
        }
		  #endregion

        protected override void InitializeHelpContentRepository()
        {
            this.HelpContentRepository = new HelpContentRepository(this.PathHelper, this.ControlRepository, this.SampleRepository, this.SampleSourceCodeRepository, 
                this.TableOfContentsRepository, MemoryCache.Default, this.ApplicationSamplesRepository, true);
        }
		  #region ShowcaseLocalInstall, GettingStartedLocalInstall
        public string ShowcaseLocalInstall()
        {
            ShowcaseViewModel model = ViewModelFactory.CreateShowcaseViewModel(this.PathHelper, this.TableOfContentsRepository,
                this.ApplicationSamplesRepository, this.ControlRepository, this.SampleRepository, MemoryCache.Default);
            model.LocalZipInstall = true;
            return ViewRenderer.RenderView("~/Views/Shared/ApplicationSamples.cshtml", model,
                                                     ControllerContext);
        }
        public string GettingStartedLocalInstall(string filePath)
        {
            GettingStartedViewModel model = ViewModelFactory.CreateGettingStartedViewModel(this.PathHelper, this.TableOfContentsRepository, this.HelpContentRepository,
                this.ControlRepository, this.SampleRepository, filePath, MemoryCache.Default);
            
            return ViewRenderer.RenderView("~/Views/Shared/GettingStarted.cshtml", model,
                                                     ControllerContext);
        }
        public void GettingStartedLocalInstallMobile(string outDir, string filePath)
        {
            GettingStartedViewModel model = ViewModelFactory.CreateGettingStartedViewModel(this.PathHelper, this.TableOfContentsRepository, this.HelpContentRepository,
                this.ControlRepository, this.SampleRepository, filePath, MemoryCache.Default);
            
            string sample =  ViewRenderer.RenderView("~/Views/Shared/GettingStarted.cshtml", model,
                                                     ControllerContext);

            string gettingStartedMobile = sample.Replace("href=\"~/", rootHref).Replace(rootHref, "href=\"./").Replace(rootSrc, "src=\"./").Replace("src=\"./../igniteui/", "src=\"./igniteui/");
            string gettingStartedMobileFileName = Path.Combine(outDir, "getting-started-mobile.html");
            List<string> iframeSrcs = new List<string>();
            foreach(Sample s in model.GettingStartedContent.Samples)
            {
                this.CreateMobileSamplePage(outDir, s.Control.PathID, s.PathID);
                iframeSrcs.Add(s.Control.PathID + "/" + s.PathID);
                
            }
            gettingStartedMobile = parser.AddFileExtentionToIFrames(gettingStartedMobile, iframeSrcs);                        
            parser.ChangeNavLinks(gettingStartedMobile, gettingStartedMobileFileName);
        }
		  #endregion

        public void CreateMobileSamplePage(string outDir, string controlPathID, string samplePathID)
        {
            string path = Path.Combine(this.FileSystem.HTMLSourceDirectory, controlPathID, samplePathID + ".html");
            // content of original html sample
            string samplePageIframe = this.GetFileContent(path);
           
            // adjust location of js files which hold data
            samplePageIframe = samplePageIframe.Replace("/data-files/", "/js-data/mobile/").Replace("href=\"../../", "href=\"./../").Replace("src=\"../../", "src=\"./../");;

            string dirPath = Path.Combine(outDir, controlPathID);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            string fileNameIframe = Path.Combine(dirPath, samplePathID + ".html");
            parser.ChangeNavLinks(samplePageIframe, fileNameIframe, true);           
        }

		  #region GenerateLocalInstallFiles
        [HttpPost]
        public ActionResult GenerateLocalInstallFiles()
        {            
            string filePath = this.GenerateZip();
            return File(filePath, "application/zip", System.IO.Path.GetFileName(filePath));
        }
		  #endregion

		#region GenerateZip
        public string GenerateZip()
        {
            // Writes the all.js and all.css files to disk
            ResourceCombiner combiner = new ResourceCombiner(this.Config);
            string allScripts = combiner.GenerateCombinedResource(this.FileSystem.CombinedJsFiles, MemoryCache.Default, null, this.FileSystem.JsDirectory + "\\all.js");
            string allCss = combiner.GenerateCombinedResource(this.FileSystem.CombinedCssFiles, MemoryCache.Default, null, this.FileSystem.CssDirectory + "\\all.css");

            parser = new LocalInstallPageParser(this.FileSystem.ProductSourcePath);
            appPath = Config.GetHttpContext().Request.ApplicationPath;
            if (!appPath.EndsWith("/"))
                appPath += "/";
            rootHref = "href=\"" + appPath;            
            rootSrc = "src=\"" + appPath;
            string outDir = this.GetTempDirName();
            if (Directory.Exists(outDir))
                Directory.Delete(outDir, true);
				string culture = this.Resources.Culture;
				Directory.CreateDirectory(outDir);
            this.DirectoryCopy(this.FileSystem.JsDirectory, Path.Combine(outDir, "js"), true);
            this.DirectoryCopy(this.FileSystem.CssDirectory, Path.Combine(outDir, "css"), true);
            //this.DirectoryCopy(this.FileSystem.IgniteUIDirectory, Path.Combine(outDir, "igniteui"), true);
            this.DirectoryCopy(this.FileSystem.ImagesDirectory, Path.Combine(outDir, "images"), true);
            this.DirectoryCopy(this.FileSystem.DataDirectory, Path.Combine(outDir, "js-data"), true, true);

            IndexViewModel model = ViewModelFactory.CreateIndexViewModel(this.PathHelper, this.TableOfContentsRepository,
               this.ApplicationSamplesRepository, this.ControlRepository, this.SampleRepository, MemoryCache.Default);
            
            string index = this.IndexLocalInstall(model).Replace(rootHref, "href=\"./").Replace(rootSrc, "src=\"./");
            string indexFileName = Path.Combine(outDir, "index.html");
            parser.ChangeNavLinks(index, indexFileName);            

            string showcase = this.ShowcaseLocalInstall().Replace(rootHref, "href=\"./").Replace(rootSrc, "src=\"./");
            string showcaseFileName = Path.Combine(outDir, "application-samples.html");
            parser.ChangeNavLinks(showcase, showcaseFileName);            

            string gettingStarted = this.GettingStartedLocalInstall("getting-started").Replace("href=\"~/", rootHref).Replace(rootHref, "href=\"./").Replace(rootSrc, "src=\"./").Replace("src=\"./../igniteui/","src=\"./igniteui/");
            string gettingStartedFileName = Path.Combine(outDir, "getting-started.html");
            parser.ChangeNavLinks(gettingStarted, gettingStartedFileName);

            this.GettingStartedLocalInstallMobile(outDir, "getting-started-mobile");            

            this.CreateSamplePages(outDir, model);            
            string zipFileName = this.WriteZipFile(outDir);
            Directory.Delete(outDir, true);
            return zipFileName;
        }
		  #endregion

		  #region GetZipFileName, GetTempDirName
		  private string GetRootPath()
		  {
            if (string.IsNullOrEmpty(this._rootPath))
			    this._rootPath = System.Web.HttpContext.Current.Server.MapPath("~\\");
			return this._rootPath;
		  }
		  private string GetZipFileName()
		  {
			string file = System.Configuration.ConfigurationManager.AppSettings["LocalInstallGenZipFileName"];
			if (string.IsNullOrEmpty(file))
				file = "LocalInstall.zip";
			if (file.IndexOf(":") < 0)
				file = Path.Combine(this.GetRootPath(), file);
			return file;
		  }
		  private string GetTempDirName()
		  {
			string path = System.Configuration.ConfigurationManager.AppSettings["LocalInstallGenTempDir"];
			if (string.IsNullOrEmpty(path))
				path = "TempLocalInstall";
			if (path.IndexOf(":") < 0)
				path = Path.Combine(this.GetRootPath(), path);
			return path;
		  }
		  #endregion

		  #region WriteZipFile, AddToZip
		  private string WriteZipFile(string dir)
		  {
			Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();
			this.AddToZip(dir, dir.Length + 1, zip);
			string zipFileName = this.GetZipFileName();
			zip.Save(zipFileName);
			return zipFileName;
		  }
		  private void AddToZip(string dir, int rootLength, Ionic.Zip.ZipFile zip)
		  {
			string[] files = System.IO.Directory.GetFiles(dir);
			int len = files.Length;
			while (len-- > 0)
			{
				string file = files[len], path = file;
				path = path.Substring(0, path.LastIndexOf(Path.DirectorySeparatorChar));
				zip.AddFile(file, path.Length > rootLength ? path.Substring(rootLength) : string.Empty);
			}
			string[] dirs = System.IO.Directory.GetDirectories(dir);
			len = dirs.Length;
			while (len-- > 0)
			{
				this.AddToZip(dirs[len], rootLength, zip);
			}
		  } 
		  #endregion

		  #region CreateSamplePages
        public void CreateSamplePages(string outDir, IndexViewModel model)
        {            
            foreach (ControlGroup controlGrp in model.IndexControlList)
            {
                foreach (Control control in controlGrp.Controls)
                {
                    if (!string.IsNullOrEmpty(control.OverviewUrl))                    
                        this.CreateSamplePage(outDir, control.PathID, "overview");
                    
                                        
                    foreach (string samplePath in control.SamplePathIDs)
                        this.CreateSamplePage(outDir, control.PathID, samplePath);
                    

                    foreach (SampleGroup sampleGrp in control.SampleGroups)
                    {
                        foreach (string samplePath in sampleGrp.SamplePathIDs)                        
                            this.CreateSamplePage(outDir, control.PathID, samplePath);                        
                    }
                    
                }
            }
        }
		  #endregion

		  #region CreateSamplePage, SamplePage
        private void CreateSamplePage(string outDir, string controlPathID, string samplePathID)
        {
            Dictionary<string, string> sampleData = this.SamplePage(samplePathID, controlPathID);
            string samplePage = sampleData["samplePage"];
            if (!string.IsNullOrEmpty(samplePage))
            {
                samplePage = samplePage.Replace("href=\"~/", rootHref).Replace(rootHref, "href=\"./../").Replace(rootSrc, "src=\"./../");
                string dirPath = Path.Combine(outDir, controlPathID);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                string fileName = Path.Combine(dirPath, samplePathID + ".html");
                samplePage = this.MobilePage(outDir, controlPathID, samplePathID, samplePage, (sampleData.Keys.Contains("redirectSourcePath") ? sampleData["redirectSourcePath"] : null));
                parser.ChangeNavLinks(samplePage, fileName, (controlPathID.IndexOf("mobile") == 0));
            }
        }
        private Dictionary<string, string> SamplePage(string samplePath, string componentPath)
        {
            Infragistics.IgniteUI.SamplesBrowser.Shared.Entities.Control control = this.ControlRepository.GetControl(componentPath);
            Dictionary<string, string> sampleData = new Dictionary<string, string>();
            sampleData["samplePage"] = "";
            if (control == null)
                return sampleData;

            Infragistics.IgniteUI.SamplesBrowser.Shared.Entities.Sample sample = this.SampleRepository.GetSample(samplePath, control);
            if (sample == null || sample.ServerRuntime == XmlSchema.Values.ServerRuntime.ASPNETMVC)
                return sampleData;

            SampleViewModel model = ViewModelFactory.CreateSampleViewModel(this.PathHelper, this.TableOfContentsRepository, this.ControlRepository, this.SampleRepository, sample, this.SampleSourceCodeRepository, MemoryCache.Default, new SourceCodeParsingOptions { IsLocalInstall = true }, this.ApplicationSamplesRepository);
            
            if (sample.ServerRuntime == XmlSchema.Values.ServerRuntime.Hosted)
            {
                model.RuntimeEnvLimitationMsg = Resources.ServerRuntimeMessageHosted;
                model.RuntimeEnvLimitationWrkUrl = this.LiveSampleUrl(sample.Url);
            }
            else if (sample.ServerRuntime == XmlSchema.Values.ServerRuntime.HostedRemote)
            {
                model.RuntimeEnvLimitationMsg = Resources.ServerRuntimeMessageHostedRemote;
                model.RuntimeEnvLimitationWrkUrl = this.LiveSampleUrl(sample.Url);
            }
            else if (sample.ServerRuntime == XmlSchema.Values.ServerRuntime.Remote)
                model.RuntimeEnvLimitationMsg = Resources.ServerRuntimeMessageRemote;

            var request = Config.GetHttpContext().Request;
           
            if (request.QueryString["mobileview"] != null && sample.Control.IsJQueryMobile)
            {
                sampleData["samplePage"] = ViewRenderer.RenderView("~/Views/Shared/MobileSample.cshtml", model,
                                                     ControllerContext);
                return sampleData;
            }
            sampleData["samplePage"] =  ViewRenderer.RenderView("~/Views/Shared/Sample.cshtml", model,
                                                     ControllerContext);            
            if (sample.Control.IsJQueryMobile && samplePath == "overview")            
                sampleData["redirectSourcePath"] = model.Sample.RedirectSourcePath;
                            
            return sampleData;
        }
        private string LiveSampleUrl(string url)
        {
            string website = Config.ZipInstallSampleBrowserUrl;
            if (!website.EndsWith("/"))
                website += "/";
            return website + url.TrimStart(new char[] {'~', '/'});
        }
		  #endregion

		  #region DirectoryCopy
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool fixImgPath = false)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            DirectoryInfo destDir = new DirectoryInfo(destDirName);
            destDir.Attributes &= ~FileAttributes.ReadOnly;

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                if (!fixImgPath)
                {
                    file.CopyTo(temppath, false);
                    FileInfo newFile = new FileInfo(temppath);
                    newFile.IsReadOnly = false;
                }
                else
                {                    
                    string text = System.IO.File.ReadAllText(file.FullName);
                    text = text.Replace("../../", "./../");
                    System.IO.File.WriteAllText(temppath, text, System.Text.Encoding.UTF8);
                    string mobileDataDir = Path.Combine(destDirName, "mobile");
                    if (!Directory.Exists(mobileDataDir))
                        Directory.CreateDirectory(mobileDataDir);
                    string tempMobilepath = Path.Combine(mobileDataDir, file.Name);
                    file.CopyTo(tempMobilepath, false);
                    FileInfo newFile = new FileInfo(tempMobilepath);
                    newFile.IsReadOnly = false;
                }
                
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }        
		  #endregion

		#region GetFileContent
		// returns content of file
		private string GetFileContent(string fileName)
		{
			try
			{
				fileName = fileName.Replace('/', '\\');
				if (!System.IO.File.Exists(fileName))
					return "";
				System.IO.StreamReader reader = new System.IO.StreamReader(fileName);
				string text = reader.ReadToEnd();
				reader.Close();
				return text;
			}
			catch(System.IO.IOException)
			{
			}
			return "";
		}
		#endregion

		#region MobilePage
		// create mobile page
		// NOTE: original html for mobile sample is used as content for iframe.
		// Sample swaps original html and makes it as a container for wrapper-sample with local iframe.
		// That method will add new directory with original html file: "/[samplePathID]/iframe/[samplePage].html"
		// and that file will be used as src for iframe
        private string MobilePage(string outDir, string controlPathID, string samplePathID, string samplePage, string redirectSourcePath)
        {
            if (controlPathID.IndexOf("mobile") != 0)
                return samplePage;
            if (!string.IsNullOrEmpty(redirectSourcePath))
            {
                List<string> iframeSrcs = new List<string>();
                List<string> iframeReplacementSrcs = new List<string>();
                iframeReplacementSrcs.Add(redirectSourcePath.Replace(controlPathID, controlPathID + "/" + "iframe"));
                iframeSrcs.Add(controlPathID + "/" + samplePathID);
                return parser.AddFileExtentionToIFrames(samplePage, iframeSrcs, iframeReplacementSrcs);
            }
            int iStart, iEnd;
            // location of original html sample
            string path = Path.Combine(this.FileSystem.HTMLSourceDirectory, controlPathID, samplePathID + ".html");
            // content of original html sample
            string samplePageIframe = this.GetFileContent(path);
            if (string.IsNullOrEmpty(samplePageIframe))
                return samplePage;
            // adjust location of js files which hold data
            samplePageIframe = samplePageIframe.Replace("/data-files/", "/js-data/mobile/");
            samplePage = samplePage.Replace("/data-files/", "/js-data/");
            // adjust references to background images used by local <style> in sample
            samplePage = samplePage.Replace("url(/images/", "url(../images/");
            // find <iframe>
            iStart = samplePage.IndexOf("<iframe ");
            if (iStart > 0)
            {
                iStart = samplePage.IndexOf(" src=\"", iStart);
                if (iStart > 0)
                {
                    // fix src of <iframe>, replace old
                    // src="aDir/[controlPathID]/[samplePathID]?xxx"
                    // by new
                    // src="aDir/[controlPathID]/iframe/[samplePathID].html"
                    iEnd = samplePage.IndexOf("\"", iStart + 8);
                    int iControl = samplePage.IndexOf("/" + controlPathID + "/", iStart);
                    iStart = samplePage.IndexOf("?", iStart + 8);
                    if (iEnd > iStart && iStart > 0 && iControl > 0 && iControl < iEnd)
                    {
                        samplePage = samplePage.Remove(iStart, iEnd - iStart).Insert(iStart, ".html");
                        samplePage = samplePage.Insert(iControl + controlPathID.Length + 2, "iframe/");

                        /* OK 9/19/2013 152482 - I will remove the popoutlink and the qr code with lync in the parse.ChangeNavLinks */
                        // remove popup link
                        //iEnd = samplePage.IndexOf("id=\"popoutLink\"");
                        //if (iEnd > 0)
                        //{
                        //    iStart = samplePage.LastIndexOf("<a ", iEnd);
                        //    if (iStart < iEnd && iStart + 20 > iEnd)
                        //    {
                        //        iEnd = samplePage.IndexOf("</a>", iEnd);
                        //        samplePage = samplePage.Remove(iStart, iEnd - iStart + 4);
                        //    }
                        //}

                        // copy original sample to controlName/iframe/sampleName location
                        string dirPath = Path.Combine(outDir, controlPathID, "iframe");
                        if (!Directory.Exists(dirPath))
                            Directory.CreateDirectory(dirPath);
                        string fileNameIframe = Path.Combine(dirPath, samplePathID + ".html");
                        parser.ChangeNavLinks(samplePageIframe, fileNameIframe, true);
                    }
                }
            }
            return samplePage;
        }		
		#endregion
    }
}
