using ClothesApiAuthRepositoryUOW.Core.Dtos;
using ClothesApiAuthRepositoryUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Interfaces
{
    public interface IProductRepository 
    {
        Task<ProductDisplayDto> CreateProduct(ProductFormDto dto);

        Task<Product_color_sizeDisplayDto> CreatePrduct_Size_color (Product_Size_Color_formDto prduct_size);
        Task<Product_color_sizeDisplayDto> UpatePrduct_Size_color(IEnumerable<Product_Color_Size> prduct_size, int id);

        Task<IEnumerable<ProductDisplayDto>> GetAllProducts(Expression<Func<ProductDisplayDto, bool>> match = null);
        Task<DeleteDisplay> DeleteProduct(int id);
        Task<ProductDisplayDto> UpdateProduct(EditeProductFormDto dto , int id);

        

    }
}
