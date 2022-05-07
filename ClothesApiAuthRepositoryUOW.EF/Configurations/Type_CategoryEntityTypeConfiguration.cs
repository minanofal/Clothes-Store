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
    public class Type_CategoryEntityTypeConfiguration:IEntityTypeConfiguration<Type_Category>
    {
        public void Configure(EntityTypeBuilder<Type_Category> builder)
        {
            builder.HasKey(x => new { x.TypeId, x.CategoryId });
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Types_Categories)
                .HasForeignKey(x => x.CategoryId);
            builder.HasOne(x=>x.Type)
                .WithMany(x=>x.Types_Categories)
                .HasForeignKey(x=>x.TypeId);

        }
    }
}
