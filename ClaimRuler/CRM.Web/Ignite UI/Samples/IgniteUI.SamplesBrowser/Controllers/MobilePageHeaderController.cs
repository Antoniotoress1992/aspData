using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class MobilePageHeaderController : Controller
    {
        [ActionName("basic-usage")]
        public ActionResult BasicUsage()
        {
            // This is a placeholder action method. To see the 
            // sample source, see this action in the file:
            // /Controllers/MobilePageController.cs
            return View("basic-usage");
        }
    }
}
