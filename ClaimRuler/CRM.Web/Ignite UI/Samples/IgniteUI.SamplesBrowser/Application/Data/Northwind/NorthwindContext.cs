using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web;

namespace IgniteUI.SamplesBrowser.Application.Data
{
    public class NorthwindContext : DbContext
    {
        static NorthwindContext()
        {
            Database.SetInitializer<NorthwindContext>(null);
        }

        public NorthwindContext()
            : base(string.Format("{0}-{1}", "Name=NorthwindConnection",
            HttpContext.GetGlobalResourceObject("Main", "Request_Culture").ToString()))
        {
        }

        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<OrderDetailEntity> Order_Details { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<RegionEntity> Regions { get; set; }
        public DbSet<ShipperEntity> Shippers { get; set; }
        public DbSet<SupplierEntity> Suppliers { get; set; }
        public DbSet<TerritoryEntity> Territories { get; set; }
        public DbSet<InvoiceEntity> Invoices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new EmployeeMap());
            modelBuilder.Configurations.Add(new OrderDetailMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new ShipperMap());
            modelBuilder.Configurations.Add(new SupplierMap());
            modelBuilder.Configurations.Add(new TerritoryMap());
            modelBuilder.Configurations.Add(new InvoiceMap());
        }
    }
}
