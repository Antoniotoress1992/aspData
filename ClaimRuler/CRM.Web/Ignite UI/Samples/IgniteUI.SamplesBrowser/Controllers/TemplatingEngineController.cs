using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class TemplatingEngineController : Controller
    {
        [ActionName("aspnet-mvc-usage")]
        public ActionResult AspnetMvcUsage()
        {
            // This is a placeholder action method. To see the 
            // sample source, see this action in the file:
            // /Controllers/PieChartController.cs
            return View("aspnet-mvc-usage");
        }
    }
}
