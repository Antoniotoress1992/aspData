using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class DialogWindowController : Controller
    {
        [ActionName("aspnet-mvc-helper")]
        public ActionResult BasicMvcHelper()
        {
            return View("aspnet-mvc-helper");
        }     
    }
}
