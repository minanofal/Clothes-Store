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
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Name).HasMaxLength(100);

            builder.HasMany(x => x.Types)
                .WithMany(x => x.categories)
                .UsingEntity<Type_Category>();
               
        }
    }
}
