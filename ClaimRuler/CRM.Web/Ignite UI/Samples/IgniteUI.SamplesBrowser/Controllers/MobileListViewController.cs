using IgniteUI.SamplesBrowser.Models.Repositories;
using Infragistics.Web.Mvc.Mobile;
using System.Linq;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class MobileListViewController : Controller
    {

        [ActionName("bind-collection")]
        public ActionResult BindCollection()
        {
            //var categories = RepositoryFactory.GetCategoryRepository(IncludeChildren.Products).Get();
            var categories = RepositoryFactory.GetCategoryRepository(7, IncludeChildren.Products).Get();
            return View("bind-collection", categories);            
        }

        [ActionName("load-on-demand")]
        public ActionResult LoadOnDemand()
        {
            var categories = RepositoryFactory.GetCategoryRepository().Get();
            return View("load-on-demand", categories);
        }

        [ActionName("categories-and-products-json")]
        public JsonResult CategoriesAndProductsJson()
        {
            var query = this.Request.QueryString;
            var path = query["path"];
            var layout = query["layout"];
            var page = query["page"];
            var pageSize = query["pageSize"];
            int id;
            int take = int.Parse(pageSize);
            int skip = int.Parse(page) * take;
            var categories = RepositoryFactory.GetCategoryRepository(IncludeChildren.Products).Get();

            if (layout == "Products")
            {
                id = int.Parse(path.TrimStart("ID:".ToCharArray()));

                var category = (from c in categories
                                where c.ID == id
                                select c).First();

                var products = from p in category.Products
                                select p;

                int totalCount = products.Count();

                var loadedProducts = products.Skip(skip).Take(take);

                dynamic result = new { Records = loadedProducts, TotalRecordsCount = totalCount };

                return Json(result , JsonRequestBehavior.AllowGet);
            }



            var loadedCategories = (from c in categories
                                    select c).Skip(skip).Take(take);

            return Json(loadedCategories, JsonRequestBehavior.AllowGet);
        }

    }
}
