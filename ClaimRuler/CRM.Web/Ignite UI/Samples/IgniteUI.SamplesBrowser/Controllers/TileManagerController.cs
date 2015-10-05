using System.Web.Mvc;
using IgniteUI.SamplesBrowser.Models.Northwind;
using IgniteUI.SamplesBrowser.Models.Repositories;
using System.Collections.Generic;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class TileManagerController : Controller
    {
        [ActionName("aspnet-mvc-helper")]
        public ActionResult BasicMvcHelper()
        {
            IEnumerable<Category> categories = RepositoryFactory.GetCategoryRepository(IncludeChildren.Products).Get();
            return View("aspnet-mvc-helper", categories);
        }     
    }
}
