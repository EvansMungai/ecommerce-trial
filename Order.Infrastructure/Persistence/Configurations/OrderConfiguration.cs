using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderDomain>
{
    public void Configure(EntityTypeBuilder<OrderDomain> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.OrderGuid)
            .HasColumnName("order_guid")
            .HasColumnType("uuid")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Phonenumber)
            .HasColumnName("phoneumber")
            .HasColumnType("text");

        builder.Property(o => o.OrderStatusId)
            .HasColumnName("order_status_id")
            .IsRequired();

        builder.Property(o => o.PaymentStatusId)
            .HasColumnName("payment_status_id")
            .IsRequired();

        builder.Property(o => o.Total)
            .HasColumnName("total")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(o => o.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(o => o.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(o => o.Deleted)
            .HasColumnName("deleted")
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasMany(x => x.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
