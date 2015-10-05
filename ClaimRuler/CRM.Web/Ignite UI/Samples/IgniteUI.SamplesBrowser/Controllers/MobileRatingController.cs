using IgniteUI.SamplesBrowser.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class MobileRatingController : Controller
    {
        //
        // GET: /MobileRating/
        [ActionName("aspnet-mvc-helper")]
        public ActionResult Index()
        {
            var product = RepositoryFactory.GetProductRepository().Get().First();
            return View("aspnet-mvc-helper", product);
        }

    }
}
