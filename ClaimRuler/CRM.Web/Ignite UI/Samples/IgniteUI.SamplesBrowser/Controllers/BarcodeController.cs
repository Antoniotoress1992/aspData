using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class BarcodeController : Controller
    {
        [ActionName("mvc-initialization")]
        public ActionResult Index()
        {
            return View("mvc-initialization");
        }

    }
}
