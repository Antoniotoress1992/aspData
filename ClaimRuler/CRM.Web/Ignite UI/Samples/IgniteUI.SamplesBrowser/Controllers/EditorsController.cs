using IgniteUI.SamplesBrowser.Models.DataAnnotations;
using IgniteUI.SamplesBrowser.Models.Northwind;
using IgniteUI.SamplesBrowser.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class EditorsController : Controller
    {
        //
        // GET: /Editors/

        [ActionName("data-annotation-validation")]
        public ActionResult DataAnnotationValidation()
        {
            ValidatedOrder model = new ValidatedOrder();
            model.ShipMethodList = this.GetShipMethods();
            return View("data-annotation-validation", model);
        }

        [HttpPost]
        [ActionName("data-annotation-validation")]
        public ActionResult DataAnnotationValidation(ValidatedOrder model)
        {
            ViewData["submittedModel"] = model;
            model = new ValidatedOrder();
            model.ShipMethodList = this.GetShipMethods();
            return View("data-annotation-validation", model);
        }

        [ActionName("load-and-save-form-values")]
        public ActionResult EditProduct()
        {
            Product product = RepositoryFactory.GetProductRepository().Get().First();
            ViewData["message"] = String.Empty;
            return View("load-and-save-form-values", product);
        }

        [HttpPost]
        [ActionName("load-and-save-form-values")]
        public ActionResult EditProduct(Product updatedProduct)
        {
            Product model;

            ItemRepository<Product> products = RepositoryFactory.GetProductRepository();
            Product existingProduct = products.Get(p => p.ID == updatedProduct.ID);

            if (existingProduct != null)
            {
                existingProduct.ProductName = updatedProduct.ProductName;
                existingProduct.UnitsInStock = updatedProduct.UnitsInStock;
                existingProduct.QuantityPerUnit = updatedProduct.QuantityPerUnit;
                existingProduct.UnitsOnOrder = updatedProduct.UnitsOnOrder;
                existingProduct.UnitPrice = updatedProduct.UnitPrice;
                existingProduct.ReorderLevel = updatedProduct.ReorderLevel;

                products.Update(existingProduct, p => p.ID == existingProduct.ID);
                products.Save();

                ViewData["message"] = "Product saved successfully";

                model = existingProduct;
            }
            else
            {
                model = updatedProduct;
            }

            return View("load-and-save-form-values", model);
        }


        public List<SelectListItem> GetShipMethods()
        {
            List<SelectListItem> selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Standard", Value = "Standard" },
                new SelectListItem { Text = "One Day", Value = "OneDay" },
                new SelectListItem { Text = "Two Day", Value = "TwoDay" },
                new SelectListItem { Text = "International", Value = "International" }
            };
            return selectList;
        }

    }
}
