using IgniteUI.SamplesBrowser.Models.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class PieChartController : Controller
    {
        //
        // GET: /PieChart/

        [ActionName("aspnet-mvc-helper")]
        public ActionResult AspNetMvcHelper()
        {
            FinancialDataCollection data = new FinancialDataCollection();
            return View("aspnet-mvc-helper", data);
        }

    }
}
