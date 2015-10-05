using AutoMapper;
using IgniteUI.SamplesBrowser.Application.Data;
using IgniteUI.SamplesBrowser.Application.Model;
using IgniteUI.SamplesBrowser.Models.Northwind;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace IgniteUI.SamplesBrowser.Models.Repositories
{
    public static class RepositoryFactory
    {        
        #region Northwind
        public static ItemRepository<Product> GetProductRepository()
        {
            GetRepositoryMapping().MapProducts();

            IEnumerable<Product> products;
            string sessionKey = GetSessionKey(typeof(Product));
            var session = HttpContext.Current.Session[sessionKey];

            if (session != null)
                products = session as IEnumerable<Product>;
            else
                products = Mapper.Map<IEnumerable<ProductEntity>, IEnumerable<Product>>(
                    new NorthwindContext().Products.AsEnumerable());

            return new ItemRepository<Product>(products, sessionKey);
        }

        public static ItemRepository<Employee> GetEmployeeRepository(IncludeChildren includeChildren = new IncludeChildren())
        {
            GetRepositoryMapping().MapEmployees(includeChildren);

            IEnumerable<Employee> employees;
            string sessionKey = GetSessionKey(typeof(Employee), includeChildren);
            var session = HttpContext.Current.Session[sessionKey];

            if (session != null)
                employees = session as IEnumerable<Employee>;
            else
                employees = Mapper.Map<IEnumerable<EmployeeEntity>, IEnumerable<Employee>>(
                    new NorthwindContext().Employees.AsEnumerable());

            return new ItemRepository<Employee>(employees, sessionKey);
        }

        public static ItemRepository<Customer> GetCustomerRepository(IncludeChildren includeChildren = new IncludeChildren())
        {
            GetRepositoryMapping().MapCustomers(includeChildren);

            IEnumerable<Customer> customers;
            string sessionKey = GetSessionKey(typeof(Customer), includeChildren);
            var session = HttpContext.Current.Session[sessionKey];

            if (session != null)
                customers = session as IEnumerable<Customer>;
            else
                customers = Mapper.Map<IEnumerable<CustomerEntity>, IEnumerable<Customer>>(
                    new NorthwindContext().Customers.AsEnumerable());

            return new ItemRepository<Customer>(customers, sessionKey);
        }
        public static ItemRepository<Category> GetCategoryRepository(int count, IncludeChildren includeChildren = new IncludeChildren())
        {
            GetRepositoryMapping().MapCategories(includeChildren);

            IEnumerable<Category> categories;
            string sessionKey = GetSessionKey(typeof(Category), includeChildren);
            var session = HttpContext.Current.Session[sessionKey];

            if (session != null)
                categories = session as IEnumerable<Category>;
            else
                categories = Mapper.Map<IEnumerable<CategoryEntity>, IEnumerable<Category>>(
                new NorthwindContext().Categories.Take(count).AsEnumerable());

            return new ItemRepository<Category>(categories, sessionKey);
        }
        public static ItemRepository<Category> GetCategoryRepository(IncludeChildren includeChildren = new IncludeChildren())
        {
            GetRepositoryMapping().MapCategories(includeChildren);

            IEnumerable<Category> categories;
            string sessionKey = GetSessionKey(typeof(Category), includeChildren);
            var session = HttpContext.Current.Session[sessionKey];

            if (session != null)
                categories = session as IEnumerable<Category>;
            else
                categories = Mapper.Map<IEnumerable<CategoryEntity>, IEnumerable<Category>>(
                new NorthwindContext().Categories.AsEnumerable());

            return new ItemRepository<Category>(categories, sessionKey);
        }

        public static ItemRepository<Invoice> GetInvoiceRepository()
        {
            GetRepositoryMapping().MapInvoices();

            IEnumerable<Invoice> invoices;
            string sessionKey = GetSessionKey(typeof(Invoice));
            var session = HttpContext.Current.Session[sessionKey];

            if (session != null)
                invoices = session as IEnumerable<Invoice>;
            else
                invoices = Mapper.Map<IEnumerable<InvoiceEntity>, IEnumerable<Invoice>>(
                   new NorthwindContext().Invoices.AsEnumerable());

            return new ItemRepository<Invoice>(invoices, sessionKey);
        }

        public static ItemRepository<Order> GetOrderRepository()
        {
            GetRepositoryMapping().MapOrders();

            IEnumerable<Order> orders;
            string sessionKey = GetSessionKey(typeof(Order));
            var session = HttpContext.Current.Session[sessionKey];

            if (session != null)
                orders = session as IEnumerable<Order>;
            else
                orders = Mapper.Map<IEnumerable<OrderEntity>, IEnumerable<Order>>(
                    new NorthwindContext().Orders.AsEnumerable());

            return new ItemRepository<Order>(orders, sessionKey);
        }
        #endregion Northwind

        #region Helper Methods
        private static string GetSessionKey(Type type)
        {
            return type.AssemblyQualifiedName;
        }

        private static string GetSessionKey(Type type, IncludeChildren includeChildren)
        {
            return type.AssemblyQualifiedName + includeChildren.ToString();
        }

        public static RepositoryMapping GetRepositoryMapping()
        {
            return new RepositoryMapping();
        }
        #endregion
    }
}