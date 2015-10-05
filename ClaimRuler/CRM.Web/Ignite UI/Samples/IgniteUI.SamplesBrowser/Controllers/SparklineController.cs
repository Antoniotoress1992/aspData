using IgniteUI.SamplesBrowser.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class SparklineController : Controller
    {
        //
        // GET: /Sparkline/

        [ActionName("bind-collection")]
        public ActionResult BindCollection()
        {
            var invoices = RepositoryFactory.GetInvoiceRepository().Get().OrderBy(i => i.OrderDate).Take(80);
            return View("bind-collection", invoices.AsQueryable());
        }

    }
}
