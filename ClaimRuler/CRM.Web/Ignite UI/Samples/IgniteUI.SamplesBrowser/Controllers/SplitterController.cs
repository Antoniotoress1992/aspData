using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class SplitterController : Controller
    {
        [ActionName("aspnet-mvc-helper-splitter")]
        public ActionResult BasicMvcHelper()
        {
            return View("aspnet-mvc-helper-splitter");
        }     
    }
}
