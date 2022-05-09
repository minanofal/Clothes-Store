using ClothesApiAuthRepositoryUOW.Core.Interfaces;
using ClothesApiAuthRepositoryUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.EF.Repositories
{
    public class ImageRepository : BaseRepository<ProductImage>, IImageRepository
    {
        private readonly ApplicationDbContext _context;
        public ImageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void CreateProductsImages(List<byte[]> images, int productID)
        {
            foreach (var image in images)
                _context.productImages.Add(new ProductImage { productId = productID, Image = image });
            _context.SaveChanges();
            
        }
    }
}
