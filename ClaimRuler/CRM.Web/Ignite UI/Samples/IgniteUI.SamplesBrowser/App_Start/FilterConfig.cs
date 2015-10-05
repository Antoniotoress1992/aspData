using IgniteUI.SamplesBrowser.Application.Model;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomExceptionFilterAttribute());
        }
    }
}