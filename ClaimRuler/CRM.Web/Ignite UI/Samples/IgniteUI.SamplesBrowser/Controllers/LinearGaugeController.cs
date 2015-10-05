using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class LinearGaugeController : Controller
    {
        [ActionName("mvc-initialization")]
        public ActionResult MVCInitialization()
        {
            return View("mvc-initialization");
        }
    }
}
