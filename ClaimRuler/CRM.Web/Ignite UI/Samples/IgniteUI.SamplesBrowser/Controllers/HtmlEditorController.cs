using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class HtmlEditorController : Controller
    {
        [ActionName("aspnet-mvc-helper")]
        public ActionResult BasicMvcHelper()
        {
            return View("aspnet-mvc-helper");
        }     
    }
}
