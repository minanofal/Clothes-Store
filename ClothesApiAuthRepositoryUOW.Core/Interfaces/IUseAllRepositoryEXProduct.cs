using ClothesApiAuthRepositoryUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Interfaces
{
    public interface IUseAllRepositoryEXProduct
    {
        ICategoryRepository Categories { get; }
        IBaseRepository<Models.Type> Types { get; }
        IBaseRepository<Type_Category> Type_Category { get; }
        IBaseRepository<Product_Color_Size_Dto> Poroduct_Color_sizes { get; }
        IBaseRepository<Color> Color { get; }
        IBaseRepository<Size> Size { get; }
        IImageRepository Images { get; }

       
    }
}
