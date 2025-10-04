using Talabat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.DataAccess.Data.ConfigEntities
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // properties
            builder.Property(p => p.Name).HasMaxLength(100);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            // relationships
            builder.HasOne(p => p.ProductBrand).WithMany();
            builder.HasOne(p => p.ProductType).WithMany();
        }
    }
}
