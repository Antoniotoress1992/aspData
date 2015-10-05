using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Linq;
using Infragistics.Web.Mvc;
using IgniteUI.SamplesBrowser.Models.Repositories;
using IgniteUI.SamplesBrowser.Application.Data;
using IgniteUI.SamplesBrowser.Models.Northwind;
using System.Collections.Generic;
using System;

namespace IgniteUI.SamplesBrowser.Controllers
{
   
    public class GridController : Controller
    {
        [GridDataSourceAction]
        [ActionName("datatable-binding")]
        public ActionResult BasicMvcHelper()
        {
            DataTable customers = GetCustomerDataTable();  
            return View("datatable-binding", customers );
        }

        [ActionName("aspnet-mvc-helper")]
        public ActionResult AspnetMvcHelper()
        {
            return View("aspnet-mvc-helper");
        }

        [ActionName("sorting-remote")]
        public ActionResult SortingRemote()
        {
            return View("sorting-remote");
        }

        [ActionName("summaries-remote")]
        public ActionResult SummariesRemote()
        {
            return View("summaries-remote");
        }

        [ActionName("bind-web-api")]
        public ActionResult BindToWebAPI()
        {
            return View("bind-web-api");
        }

        [ActionName("basic-editing")]
        public ActionResult BasicEditing()
        {
            ViewBag.Customers = RepositoryFactory.GetCustomerRepository().Get().AsQueryable();
            return View("basic-editing");
        }

        [ActionName("rest-editing")]
        public ActionResult RestEditing()
        {
            return View("rest-editing");
        }

        public ActionResult OrdersSaveData()
        {
            GridModel gridModel = new GridModel();
            List<Transaction<Order>> transactions = gridModel.LoadTransactions<Order>(HttpContext.Request.Form["ig_transactions"]);
            var orders = RepositoryFactory.GetOrderRepository();
            foreach (Transaction<Order> t in transactions)
            {
                if (t.type == "newrow")
                {
                    orders.Add(t.row);
                }
                else if (t.type == "deleterow")
                {
                    orders.Delete(o => o.OrderID == Int32.Parse(t.rowId));
                }
                else if (t.type == "row")
                {
                    var order = (from o in orders.Get()
                                   where o.OrderID == Int32.Parse(t.rowId)
                                   select o).Single();
                    if (t.row.OrderDate != null)
                    {
                        order.OrderDate = t.row.OrderDate;
                    }
                    if (t.row.TotalPrice != null)
                    {
                        order.TotalPrice = t.row.TotalPrice;
                    }
                    if (t.row.TotalItems != null)
                    {
                        order.TotalItems = t.row.TotalItems;
                    }
                    if (t.row.CustomerID != null)
                    {
                        order.CustomerID = t.row.CustomerID;
                    }
                    if (t.row.ShipAddress != null)
                    {
                        order.ShipAddress = t.row.ShipAddress;
                    }
                    orders.Update(order, o => o.OrderID == Int32.Parse(t.rowId));
                }
            }
            orders.Save();
            JsonResult result = new JsonResult();
            Dictionary<string, bool> response = new Dictionary<string, bool>();
            response.Add("Success", true);
            result.Data = response;
            return result;
        }

        [GridDataSourceAction]
        public ActionResult ChainingGetData()
        {
            var employees = RepositoryFactory.GetEmployeeRepository().Get();
            return View(employees);
        }

        [GridDataSourceAction]
        public ActionResult GetEmployees()
        {
            var employees = RepositoryFactory.GetEmployeeRepository().Get();
            return View(employees);
        }

        [GridDataSourceAction]
        public ActionResult GetProducts()
        {
            var products = RepositoryFactory.GetProductRepository().Get();
            return View(products);
        }

        [GridDataSourceAction]
        public ActionResult GetCategories()
        {
            var categories = RepositoryFactory.GetCategoryRepository().Get();
            return View(categories);
        }

        [GridDataSourceAction]
        public ActionResult GetOrders()
        {
            var orders = RepositoryFactory.GetOrderRepository().Get();
            return View(orders);
        }

        private DataTable GetCustomerDataTable()
        {
            NorthwindContext ctx =new NorthwindContext();
            SqlConnection conn = (SqlConnection)ctx.Database.Connection;
            DataTable dt = new DataTable();
            using (SqlConnection con = conn)
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM CUSTOMERS", con))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }        
    }
}
