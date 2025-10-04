using Talabat.Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.DataAccess.Data.ConfigEntities
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, shippingAddress => shippingAddress.WithOwner());

            builder.Property(o => o.Status)
                .HasConversion(
                // store in db
                s => s.ToString(),
                // return
                s => (OrderStatus)Enum.Parse(typeof(OrderStatus), s)
                );

            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");

            builder.HasMany(o => o.Items).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o=> o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);
        }
    }
}
