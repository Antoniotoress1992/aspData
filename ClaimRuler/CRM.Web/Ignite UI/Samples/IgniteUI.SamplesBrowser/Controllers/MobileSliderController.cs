using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class MobileSliderController : Controller
    {
        [ActionName("basic-usage")]
        public ActionResult BasicUsage()
        {
            return View("basic-usage");
        }

        [ActionName("color-picker")]
        public ActionResult ColorPicker()
        {
            return View("color-picker");
        }

    }
}
