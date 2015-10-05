using IgniteUI.SamplesBrowser.Application.Model;
using Infragistics.IgniteUI.SamplesBrowser.Shared.Entities;
using Infragistics.IgniteUI.SamplesBrowser.Shared.Repositories;
using Infragistics.IgniteUI.SamplesBrowser.Shared.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Runtime.Caching;
using System.Web;
using System;
using Infragistics.IgniteUI.SamplesBrowser.Shared.Util;
using System.IO;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class HomeController : Controller
    {
        public ResourceStrings Resources;
        public Config Config;
        public FileSystemHelper FileSystem;
        public AppUrlHelper PathHelper;
        public TableOfContentsRepository TableOfContentsRepository;
        public ApplicationSampleRepository ApplicationSamplesRepository;
        public SampleRepository SampleRepository;
        public ControlRepository ControlRepository;
        public HelpContentRepository HelpContentRepository;
        public SampleSourceCodeRepository SampleSourceCodeRepository;
        public LocalizedSampleStringRepository LocalizedSampleStringRepository;

        public ViewResult Index()
        {
            IndexViewModel model = ViewModelFactory.CreateIndexViewModel(this.PathHelper, this.TableOfContentsRepository,
                this.ApplicationSamplesRepository, this.ControlRepository, this.SampleRepository, MemoryCache.Default);
            return View(model);
        }

        public ViewResult ApplicationSamples()
        {
            ShowcaseViewModel model = ViewModelFactory.CreateShowcaseViewModel(this.PathHelper, this.TableOfContentsRepository,
                this.ApplicationSamplesRepository, this.ControlRepository, this.SampleRepository, MemoryCache.Default);
            return View("ApplicationSamples", model);
        }

        public ViewResult GettingStarted(string filePath)
        {
            GettingStartedViewModel model = ViewModelFactory.CreateGettingStartedViewModel(this.PathHelper, this.TableOfContentsRepository, this.HelpContentRepository,
                this.ControlRepository, this.SampleRepository, filePath, MemoryCache.Default);
            return View("GettingStarted", model);
        }

        public ActionResult Sample(string samplePath, string controlPath)
        {
            Control control = this.ControlRepository.GetControl(controlPath);
            if (control == null)
                throw new HttpException(404, "");     

            Sample sample = ViewModelFactory.GetSampleFromTOC(this.TableOfContentsRepository, samplePath, control.PathID, MemoryCache.Default);

            if (sample == null)
                try
                {
                    sample = this.SampleRepository.GetSample(samplePath, control);
                }
                catch (Exception e)
                {
                    throw new HttpException(500, "", e);
                }
            if (sample == null)
                throw new HttpException(404, "");

            /* We don't think this is needed anymore and it is interfearing with the custom error pages.
            if (sample == null)
                return this.RedirectToRoute("aspnet", new { controller = controlPath, action = samplePath });
            */
            SampleViewModel model;
            try
            {
                model = ViewModelFactory.CreateSampleViewModel(this.PathHelper, this.TableOfContentsRepository, this.ControlRepository, this.SampleRepository, sample, this.SampleSourceCodeRepository, MemoryCache.Default, new SourceCodeParsingOptions(), this.ApplicationSamplesRepository);
            }
            catch (Exception e)
            {
                throw new HttpException(500, "", e);
            }
            
            SampleCode code = model.Sample.Code;
            if (code.IsFile)
            {
                var responseHeaders = code.FileResponseHeaders;
                var headerKeys = responseHeaders.Keys;
                foreach (string key in headerKeys)
                    this.Response.AddHeader(key, responseHeaders[key]);

                return File(code.File, "application/octect-stream");
            }
            var request = Config.GetHttpContext().Request;
            if (request.QueryString["mobileview"] != null && sample.Control.IsJQueryMobile)
                return View("MobileSample", model);

            return View(model);
        }

        public ActionResult Control(string controlPath)
        {
            Control control = this.ControlRepository.GetControl(controlPath);
            if (control == null)
                throw new HttpException(404, "");
            return RedirectToAction("Sample", new { controlPath = controlPath, samplePath="overview" });
        }

        public JavaScriptResult Data(string fileName)
        {
            string fullName = string.Format("{0}.js", fileName);
            string file = FileSystem.DataFileSourcePath(fullName, Resources.Culture);
            JavaScriptSourceBuilder jsBuilder = new JavaScriptSourceBuilder(this.Config);
            return JavaScript(jsBuilder.ReplaceResourceUrls(file));
        }

        public JavaScriptResult Js(string fileName)
        {
            string fullName = string.Format("{0}.js", fileName);
            string file = string.Format("{0}/{1}", FileSystem.JsDirectory, fullName);
            string culture = this.Resources.Culture;
            IDictionary<string, string> localizedStrings = this.LocalizedSampleStringRepository.GetLocalizedSampleStrings(this.FileSystem.JsMetadataFile);
            JavaScriptSourceBuilder jsBuilder = new JavaScriptSourceBuilder(this.Config);

            return JavaScript(jsBuilder.ProcessJavaScriptFile(file, localizedStrings));
        }

        /// <summary>
        /// Combines resources for requests to /js/all.js.
        /// </summary>
        /// <returns>Returns combined JavaScript for all.js.</returns>
        public JavaScriptResult AllJs()
        {
            string cacheKey = "alljs";
            MemoryCache cache = MemoryCache.Default;
            ResourceCombiner combiner = new ResourceCombiner(this.Config);
            
            string allScripts = combiner.GetCachedOuput(cacheKey, cache);

            if (string.IsNullOrEmpty(allScripts))
            {
                allScripts = combiner.GenerateCombinedResource(this.FileSystem.CombinedJsFiles, MemoryCache.Default, cacheKey);
            }

            return JavaScript(allScripts);
        }

        /// <summary>
        /// Combines resources for requests to /css/all.css.
        /// </summary>
        /// <returns>Returns combined test/css response for all.css.</returns>
        public ContentResult AllCss()
        {
            string cacheKey = "allcss";
            MemoryCache cache = MemoryCache.Default;
            ResourceCombiner combiner = new ResourceCombiner(this.Config);

            string allCss = combiner.GetCachedOuput(cacheKey, cache);

            if (string.IsNullOrEmpty(allCss))
            {
                allCss = combiner.GenerateCombinedResource(this.FileSystem.CombinedCssFiles, MemoryCache.Default, cacheKey);
            }

            return Content(allCss, "text/css");
        }

        public HomeController()
        {
            this.Config = new Config();
            this.Resources = new ResourceStrings(Config);
            this.FileSystem = new FileSystemHelper(Config);
            this.PathHelper = new AppUrlHelper(FileSystem, Resources);
            this.InitializeTableOfContentsRepository();
            this.ApplicationSamplesRepository = new ApplicationSampleRepository(Resources.Culture, this.FileSystem);
            this.SampleRepository = new SampleRepository(Resources.Culture, this.FileSystem);
            this.ControlRepository = new ControlRepository(Resources.Culture, this.FileSystem);
            this.SampleSourceCodeRepository = new SampleSourceCodeRepository(this.PathHelper);
            this.InitializeHelpContentRepository();
            this.LocalizedSampleStringRepository = new LocalizedSampleStringRepository(this.Resources.Culture);
        }

        protected virtual void InitializeTableOfContentsRepository()
        {
            this.TableOfContentsRepository = new TableOfContentsRepository(this.Resources, this.FileSystem);
        }

        protected virtual void InitializeHelpContentRepository()
        {
            this.HelpContentRepository = new HelpContentRepository(this.PathHelper, this.ControlRepository, this.SampleRepository, this.SampleSourceCodeRepository, this.TableOfContentsRepository, MemoryCache.Default, this.ApplicationSamplesRepository);            
        }

        // For testing purposes
        public HomeController(ResourceStrings resources, Config config, FileSystemHelper fileSystem, AppUrlHelper urlHelper,
            TableOfContentsRepository tocRepository, ApplicationSampleRepository appSampleRepository, SampleRepository sampleRepository,
            ControlRepository controlRepository, HelpContentRepository helpContentRepository, SampleSourceCodeRepository sampleSourceRepository)
        {
            this.Resources = resources;
            this.Config = config;
            this.FileSystem = fileSystem;
            this.PathHelper = urlHelper;
            this.TableOfContentsRepository = tocRepository;
            this.ApplicationSamplesRepository = appSampleRepository;
            this.SampleRepository = sampleRepository;
            this.ControlRepository = controlRepository;
            this.HelpContentRepository = helpContentRepository;
            this.SampleSourceCodeRepository = sampleSourceRepository;
        }
    }
}
