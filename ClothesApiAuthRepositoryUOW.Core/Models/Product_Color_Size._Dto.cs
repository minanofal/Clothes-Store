using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Models
{
    public class Product_Color_Size_Dto
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string Color { get; set; }

        public string Size { get; set; }
    }
}
