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
    public class Product_Color_SizeEntityTypeConfiguration : IEntityTypeConfiguration<Product_Color_Size_Dto>
    {
        public void Configure(EntityTypeBuilder<Product_Color_Size_Dto> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.Size, x.Color });
            builder.Property(x=>x.Color).HasMaxLength(50);
            builder.Property(x=>x.Size).HasMaxLength(10);

        }
    }
}
