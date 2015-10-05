using IgniteUI.SamplesBrowser.Models.Northwind;
using IgniteUI.SamplesBrowser.Models.Repositories;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class LoaderController : Controller
    {
        //
        // GET: /Loader/

        [ActionName("aspnet-mvc-helper")]
        public ActionResult AspNetMvcHelper()
        {
            IEnumerable<Product> products = RepositoryFactory.GetProductRepository().Get();
            return View("aspnet-mvc-helper", products.AsQueryable());
        }

    }
}
