using System.Web.Mvc;
using IgniteUI.SamplesBrowser.Models.Financial;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class DoughnutChartController : Controller
    {
        [ActionName("bind-to-collection")]
        public ActionResult BindCollection()
        {
            var invoices = new FinancialDataCollection();
            return View("bind-to-collection", invoices);
        }        

    }
}
