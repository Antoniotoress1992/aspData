using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class MobileButtonController : Controller
    {
        //
        // GET: /Button/
        [ActionName("basic-usage")]
        public ActionResult Index()
        {
            return View("basic-usage");
        }

    }
}
