using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class MobileToggleSwitchController : Controller
    {

        [ActionName("basic-usage")]
        public ActionResult BasicUsage()
        {
            return View("basic-usage");
        }

        [ActionName("device-manager")]
        public ActionResult DeviceManager()
        {
            return View("device-manager");
        }
    }
}
