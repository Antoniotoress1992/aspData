using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class ErrorController : Controller
    {      
        public ActionResult HttpError404()
        {
            return View("NotFound");
        }
        public ActionResult HttpError500()
        {
            return View("ServerError");
        }        

        // Shhh .. secret test method .. ooOOooOooOOOooohhhhhhhh
        public ActionResult ThrowError()
        {
            throw new NotImplementedException("Pew ^ Pew");
        }


    }
}
