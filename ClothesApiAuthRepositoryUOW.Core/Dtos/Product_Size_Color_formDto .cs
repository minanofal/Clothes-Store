using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Dtos
{
    public class Product_Size_Color_formDto
    {
        public int ProductID { get; set; }
        public IEnumerable<Product_Color_Size> ProductColorSizes { get; set; }
    }
}
