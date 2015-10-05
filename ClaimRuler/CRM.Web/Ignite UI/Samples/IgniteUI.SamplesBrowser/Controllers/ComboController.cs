using IgniteUI.SamplesBrowser.Models.Repositories;
using Infragistics.Web.Mvc;
using System.Web.Mvc;
using System.Linq;
using IgniteUI.SamplesBrowser.Models.Northwind;
using System.Collections.Generic;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class ComboController : Controller
    {
        //
        // GET: /Combo/

        [ComboDataSourceAction]
        [ActionName("employee-combo-data")]
        public ActionResult ComboData()
        {
            IEnumerable<Employee> employees = RepositoryFactory.GetEmployeeRepository().Get();
            return View(employees);
        }

        [ActionName("aspnet-mvc-helper")]
        public ActionResult UsingAspNetMvcHelper()
        {
            Order order = RepositoryFactory.GetOrderRepository().Get().First();
            return View("aspnet-mvc-helper", order);
        }

        [HttpPost]
        [ActionName("aspnet-mvc-helper")]
        public ActionResult UsingAspNetMvcHelper(Order updatedOrder)
        {
            ItemRepository<Order> orderRepository = RepositoryFactory.GetOrderRepository();
            ItemRepository<Employee> employeeRepository = RepositoryFactory.GetEmployeeRepository();

            Order existingOrder = orderRepository.Get(o => o.OrderID == updatedOrder.OrderID);
            Employee newEmployee = employeeRepository.Get(e => e.ID == updatedOrder.EmployeeID);

            if (existingOrder != null && newEmployee != null)
            {
                existingOrder.EmployeeID = newEmployee.ID;
                existingOrder.EmployeeName = newEmployee.Name;

                orderRepository.Update(existingOrder, o => o.OrderID == existingOrder.OrderID);
                orderRepository.Save();
            }

            return View("aspnet-mvc-helper", existingOrder);
        }

    }
}
