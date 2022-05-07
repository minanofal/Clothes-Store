using ClothesApiAuthRepositoryUOW.Core.Models;
using ClothesApiAuthRepositoryUOW.EF.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = ClothesApiAuthRepositoryUOW.Core.Models.Type;

namespace ClothesApiAuthRepositoryUOW.EF
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //new ApplicationEntityTypeConfiguration().Configure(builder.Entity<ApplicationUser>());

            //builder.Entity<IdentityRole>().HasData(
            //    new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "User".ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() },
            //    new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "Admin".ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() }

            //    );
            builder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.ProviderKey, x.LoginProvider });
            builder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.RoleId, x.UserId });
            builder.Entity<IdentityUserToken<string>>().HasKey(x => new { x.LoginProvider, x.Name, x.UserId });

            new CategoryEntityTypeConfiguration().Configure(builder.Entity<Category>());
            new TypeEntityTypeConfiguration().Configure(builder.Entity<Type>());
            new ProductEntityTypeConfiguration().Configure(builder.Entity<Product>());
            new Product_Color_SizeEntityTypeConfiguration().Configure(builder.Entity<Product_Color_Size_Dto>());
            new ProductImagesEntityTypeConfiguration().Configure(builder.Entity<ProductImage>());
            new Type_CategoryEntityTypeConfiguration().Configure(builder.Entity<Type_Category>());
            new ColorEntityTypeConfiguration().Configure(builder.Entity<Color>());
            new SizeEntityTypeConfiguration().Configure(builder.Entity<Size>());

        }

        public DbSet<Category> categories {get; set; }  
        public DbSet<Type> types {get; set; }  
        public DbSet<Product> products {get; set; }  
        public DbSet<ProductImage> productImages {get; set; }  
        public DbSet<Product_Color_Size_Dto> product_Color_Sizes {get; set; }  
        public DbSet<Type_Category> Type_Categories {get; set; }

        public DbSet<Color> Colors {get; set; }
        public DbSet<Size>  sizes {get; set; }


    }
}
