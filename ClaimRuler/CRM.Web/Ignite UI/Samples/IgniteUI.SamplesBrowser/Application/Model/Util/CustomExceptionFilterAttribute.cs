using Elmah;
using Infragistics.IgniteUI.SamplesBrowser.Shared.Contracts;
using Infragistics.IgniteUI.SamplesBrowser.Shared.ViewModels;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Application.Model
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        private const int _defaultStatus = 500;
        private const string _defaultView = "Error";

        private int _status = -1;
        private string _view;        

        public int Status
        {
            get
            {
                return _status == -1 ? _defaultStatus : _status;
            }
            set
            {
                _status = value;
            }
        }

        public string View
        {
            get
            {
                return string.IsNullOrEmpty(_view) ? _defaultView : _view;
            }
            set
            {
                _view = value;
            }
        }

        public void OnException(ExceptionContext filterContext)
        {
            Config config = new Config();
            ResourceStrings resources = new ResourceStrings(config);

            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
                        
            if (filterContext.IsChildAction)
            {
                return;
            }

            if (filterContext != null && filterContext.Exception != null)
                ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);

            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {                
                return;
            }

            CustomErrorViewModelBase errorViewModel;

            HttpException httpException = filterContext.Exception as HttpException;
            
            if (httpException != null)
            {
                this.Status = httpException.GetHttpCode();
            }

            if (this.Status == 404)
            {
                errorViewModel = new Error404ViewModel(resources);
            }
            else if (config.IsLocalInstall && CheckForSqlException(filterContext.Exception, resources))
            {
                errorViewModel = new SqlErrorViewModel(resources);
            }
            else
            {
                errorViewModel = new Error500ViewModel(resources);
            }

            filterContext.Result = new ViewResult
            {
                ViewName = View,
                ViewData = new ViewDataDictionary<CustomErrorViewModelBase>(errorViewModel)
            };

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = this.Status;

            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        private bool CheckForSqlException(Exception exception, IResourceStrings resources)
        {
            if (exception == null)
                return false;

            if (exception.StackTrace.Contains("System.Data.SqlClient"))
            {
                return true;
            }

            // ASP.NET samples need an extra check because we actually create a web request internally
            // to get the rendered source of those samples so we have to check the response of the 
            // original web request of the sample
            WebException webEx = exception as WebException;

            if (webEx != null)
            {
                string content;
                using (StreamReader sr = new StreamReader(webEx.Response.GetResponseStream()))
                {
                    content = sr.ReadToEnd();
                }
                if (content.Contains(new SqlErrorViewModel(resources).Heading))
                {
                    return true;
                }
            }

            if (exception.InnerException != null)
            {
                return CheckForSqlException(exception.InnerException, resources);
            }

            return false;
        }
    }
}