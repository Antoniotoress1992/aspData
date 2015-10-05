using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class RadialGaugeController : Controller
    {
        //
        // GET: /RadialGauge/

        [ActionName("mvc-initialization")]
        public ActionResult RenderGauge()
        {
            return View("mvc-initialization");
        }

    }
}
