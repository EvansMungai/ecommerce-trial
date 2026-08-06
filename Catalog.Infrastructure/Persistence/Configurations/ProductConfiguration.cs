using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography.X509Certificates;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.ShowOnHomePage)
            .HasColumnName("show_on_home_page")
            .HasDefaultValue(true);

        builder.Property(x => x.Published)
            .HasColumnName("published")
            .HasDefaultValue(true);

        builder.Property(x => x.Sku)
            .HasColumnName("sku")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.StockQuantity)
            .HasColumnName("stock_quantity");

        builder.Property(x => x.MinStockQuantity)
            .HasColumnName("min_stock_quantity");

        builder.Property(x => x.LowStockActivityId)
            .HasColumnName("low_stock_activity_id");

        builder.Property(x => x.OrderMinimumQuantity)
            .HasColumnName("order_minimum_quantity");

        builder.Property(x => x.OrderMaximumQuantity)
            .HasColumnName("order_maximum_quantity");

        builder.Property(c => c.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .HasColumnType("timestamp with time zone")
            .IsRequired();
    }
}
