

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(ot => ot.Id);

            builder.Property(ot => ot.Id).HasConversion(
                orderItemId => orderItemId.Value,
                dbId => OrderItemId.Of(dbId)
                );

            builder.Property(ot => ot.Price).IsRequired();
            builder.Property(ot => ot.Quantity).IsRequired();

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(ot => ot.ProductId);
        }
    }
}
