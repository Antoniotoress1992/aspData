using IgniteUI.SamplesBrowser.Models.Northwind;
using IgniteUI.SamplesBrowser.Models.Repositories;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Query;
using System.Web.Http.OData;
using IgniteUI.SamplesBrowser.Application.Model;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace IgniteUI.SamplesBrowser.Controllers
{
    public class DataController : ApiController
    {
        [ActionName("products")]
        [CustomQueryable(AllowedArithmeticOperators = AllowedArithmeticOperators.None, AllowedQueryOptions = AllowedQueryOptions.All, AllowedFunctions = AllowedFunctions.All)] 
        public IQueryable<Product> GetProducts()
        {
            var products = RepositoryFactory.GetProductRepository().Get();
            return products.AsQueryable();
        }

        [ActionName("products")]
        public Product GetProduct(int id)
        {
            return RepositoryFactory.GetProductRepository().Get(p => p.ID == id);
        }

        [ActionName("employees")]
        [CustomQueryable(AllowedArithmeticOperators = AllowedArithmeticOperators.None, AllowedQueryOptions = AllowedQueryOptions.All)] 
        public IQueryable<Employee> GetEmployees()
        {
            var employees = RepositoryFactory.GetEmployeeRepository().Get();
            return employees.AsQueryable();
        }

        [ActionName("customers")]
        [Queryable(AllowedArithmeticOperators = AllowedArithmeticOperators.None)]
        public IQueryable<Customer> GetCustomers()
        {
            var customers = RepositoryFactory.GetCustomerRepository().Get();
            return customers.AsQueryable();
        }

        [ActionName("categories")]
        [CustomQueryable(AllowedArithmeticOperators = AllowedArithmeticOperators.None, AllowedQueryOptions = AllowedQueryOptions.All, AllowedFunctions = AllowedFunctions.All)]
        public IQueryable<Category> GetCategories()
        {
            var data = RepositoryFactory.GetCategoryRepository(IncludeChildren.Products);
            return data.Get().OrderBy(category => category.CategoryName).AsQueryable();
        }

        [ActionName("invoices")]
        [CustomQueryable(AllowedArithmeticOperators = AllowedArithmeticOperators.None, AllowedQueryOptions = AllowedQueryOptions.All, AllowedFunctions = AllowedFunctions.All)] 
        public IQueryable<Invoice> GetInvoices()
        {
            var invoices = RepositoryFactory.GetInvoiceRepository().Get();
            return invoices.AsQueryable();
        }

        [ActionName("customers-with-orders")]
        [Queryable(AllowedArithmeticOperators = AllowedArithmeticOperators.None)]
        public IQueryable<Customer> GetCustomersAndOrders()
        {
            var customers = RepositoryFactory.GetCustomerRepository(IncludeChildren.Orders).Get();
            return customers.AsQueryable();
        }

        [ActionName("orders")]
        [CustomQueryable(AllowedArithmeticOperators = AllowedArithmeticOperators.None, AllowedQueryOptions = AllowedQueryOptions.All, AllowedFunctions = AllowedFunctions.All)]
        public IQueryable<Order> GetOrders()
        {
            var data = RepositoryFactory.GetOrderRepository();
            return data.Get().AsQueryable();
        }

        [ActionName("orders")]
        public HttpResponseMessage PutOrder(int id, Order order)
        {
            var orders = RepositoryFactory.GetOrderRepository();
            if (!orders.Update(order, o => o.OrderID == id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [ActionName("orders")]
        public HttpResponseMessage PostOrder(Order order)
        {
            var orders = RepositoryFactory.GetOrderRepository();
            orders.Add(order);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [ActionName("orders")]
        public HttpResponseMessage DeleteOrder(int id)
        {
            var orders = RepositoryFactory.GetOrderRepository();
            if (orders.Delete(o => o.OrderID == id) == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
