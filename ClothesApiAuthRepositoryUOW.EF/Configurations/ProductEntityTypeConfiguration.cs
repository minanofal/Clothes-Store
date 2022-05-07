using ClothesApiAuthRepositoryUOW.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.EF.Configurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(150);
            builder.Property(x => x.Gender).HasMaxLength(1);

            builder.HasOne(x=>x.Category)
                .WithMany(x=>x.Products)
                .HasPrincipalKey(x=> x.Id)
                .HasForeignKey(x=>x.CategoryId);

            builder.HasOne(x => x.Type)
                .WithMany(x => x.products)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x=>x.TypeId);

            builder.HasMany(x => x.Images)
                .WithOne(x => x.Product)
                .HasPrincipalKey(x => x.Id)
                .HasForeignKey(x => x.productId);

            builder.HasMany(x=>x.Product_Color_Sizes)
            .WithOne(x=>x.Product)
            .HasPrincipalKey(x=>x.Id)
            .HasForeignKey(x=>x.ProductId);

        }
    }
}
