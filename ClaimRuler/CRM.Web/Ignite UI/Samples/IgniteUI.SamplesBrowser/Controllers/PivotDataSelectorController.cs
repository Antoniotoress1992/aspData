using System.Web.Mvc;
using Infragistics.Web.Mvc;
using IgniteUI.SamplesBrowser.Application.Data;
using System.Data;
using System.Data.SqlClient;
using IgniteUI.SamplesBrowser.Models.Repositories;
using Newtonsoft.Json.Converters;
using System.Linq;
using System;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class PivotDataSelectorController : Controller
    {
        [ActionName("using-the-asp-net-mvc-helper-with-flat-data-source")]
        public ActionResult UsingAspNetMvcHelperWithFlatDataSource()
        {
            var invoices = RepositoryFactory.GetInvoiceRepository().Get().Take(200).Select(invoice => new
            {
                Address = invoice.Address,
                City = invoice.City,
                CustomerName = invoice.CustomerName,
                Discount = invoice.Discount,
                Freight = invoice.Freight,
                OrderDate = invoice.OrderDate == null ? null : invoice.OrderDate.Value.AddYears(16).ToShortDateString(),
                ProductName = invoice.ProductName,
                Quantity = invoice.Quantity,
                RequiredDate = invoice.RequiredDate == null ? null : invoice.RequiredDate.Value.AddYears(16).ToShortDateString(),
                Salesperson = invoice.Salesperson,
                ShipCountry = invoice.ShipCountry,
                ShippedDate = invoice.ShippedDate == null ? null : invoice.ShippedDate.Value.AddYears(16).ToShortDateString(),
                ShipperName = invoice.ShipperName
            });
            return View("using-the-asp-net-mvc-helper-with-flat-data-source", invoices);
        }

        [ActionName("using-the-asp-net-mvc-helper-with-xmla-data-source")]
        public ActionResult UsingAspNetMvcHelperWithXmlaDataSource()
        {
            return View("using-the-asp-net-mvc-helper-with-xmla-data-source");
        }
    }
}
