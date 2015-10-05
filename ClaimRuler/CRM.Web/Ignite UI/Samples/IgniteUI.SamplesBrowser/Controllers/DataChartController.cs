using IgniteUI.SamplesBrowser.Models.Financial;
using System.Web.Mvc;
using System.Linq;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class DataChartController : Controller
    {
        //
        // GET: /DataChart/

        [ActionName("aspnet-mvc-helper")]
        public ActionResult AspNetMvcHelper()
        {
            StockMarketDataCollection model = new StockMarketDataCollection();
            return View("aspnet-mvc-helper", model);
        }

    }
}
