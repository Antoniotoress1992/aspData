using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace IgniteUI.SamplesBrowser.Application.Data
{
    public class EmployeeMap : EntityTypeConfiguration<EmployeeEntity>
    {
        public EmployeeMap()
        {
            // Primary Key
            this.HasKey(t => t.EmployeeID);

            // Properties
            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Title)
                .HasMaxLength(30);

            this.Property(t => t.TitleOfCourtesy)
                .HasMaxLength(25);

            this.Property(t => t.Address)
                .HasMaxLength(60);

            this.Property(t => t.City)
                .HasMaxLength(15);

            this.Property(t => t.Region)
                .HasMaxLength(15);

            this.Property(t => t.PostalCode)
                .HasMaxLength(10);

            this.Property(t => t.Country)
                .HasMaxLength(15);

            this.Property(t => t.HomePhone)
                .HasMaxLength(24);

            this.Property(t => t.Extension)
                .HasMaxLength(4);

            //this.Property(t => t.PhotoPath)
            //    .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Employees");
            this.Property(t => t.EmployeeID).HasColumnName("EmployeeID");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.TitleOfCourtesy).HasColumnName("TitleOfCourtesy");
            this.Property(t => t.BirthDate).HasColumnName("BirthDate");
            this.Property(t => t.HireDate).HasColumnName("HireDate");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Region).HasColumnName("Region");
            this.Property(t => t.PostalCode).HasColumnName("PostalCode");
            this.Property(t => t.Country).HasColumnName("Country");
            this.Property(t => t.HomePhone).HasColumnName("HomePhone");
            this.Property(t => t.Extension).HasColumnName("Extension");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.ReportsTo).HasColumnName("ReportsTo");

            // Relationships
            this.HasMany(t => t.Territories)
                .WithMany(t => t.Employees)
                .Map(m =>
                    {
                        m.ToTable("EmployeeTerritories");
                        m.MapLeftKey("EmployeeID");
                        m.MapRightKey("TerritoryID");
                    });

            this.HasOptional(t => t.Employee)
                .WithMany(t => t.Employees)
                .HasForeignKey(d => d.ReportsTo);

        }
    }
}
