using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class RadialMenuController : Controller
    {
        // GET: /RadialMenu/

        [ActionName("mvc-initialization")]
        public ActionResult RenderMenu()
        {
            return View("mvc-initialization");
        }
    }
}