using ClothesApiAuthRepositoryUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Interfaces
{
    public interface IImageRepository : IBaseRepository<ProductImage>
    {
        void CreateProductsImages(List<byte[]> images , int productId);

    }
}
