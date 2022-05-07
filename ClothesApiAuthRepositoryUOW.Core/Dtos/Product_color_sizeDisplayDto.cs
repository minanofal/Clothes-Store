using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Dtos
{
    public class Product_color_sizeDisplayDto
    {
        public string? Message { get; set; }

       
        public IEnumerable<Product_Color_Size> product_Color_Sizes { get; set; }
        
    }
}
