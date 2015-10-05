using IgniteUI.SamplesBrowser.Models.Northwind;
using IgniteUI.SamplesBrowser.Models.Repositories;
using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class TreeController : Controller
    {
        //
        // GET: /Tree/

        [ActionName("aspnet-mvc-helper")]
        public ActionResult AspNetMvcHelper()
        {
            IEnumerable<Category> categories = RepositoryFactory.GetCategoryRepository(IncludeChildren.Products).Get();
            return View("aspnet-mvc-helper", categories);
        }

        [ActionName("aspnet-mvc-load-on-demand")]
        public ActionResult AspNetLoadOnDemand()
        {
            IEnumerable<Category> categories = RepositoryFactory.GetCategoryRepository().Get();
            return View("aspnet-mvc-load-on-demand", categories);
        }

        [ActionName("tree-data-on-demand")]
        public JsonResult TreeDataOnDemand(string path, string binding, int depth)
        {
            TreeModel model = new TreeModel();

            switch (depth)
            {
                case 0:
                    model.DataSource = RepositoryFactory.GetCategoryRepository(IncludeChildren.Products).Get();
                    break;
                default:
                    model.DataSource = RepositoryFactory.GetCategoryRepository().Get();
                    break;
            }

            return model.GetData(path, binding);
        }

    }
}
