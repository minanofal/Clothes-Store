using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Dtos
{
    public class Product_Color_Size
    {
      
        [MaxLength(100)]
        public string Color { get; set; }
        [MaxLength(10)]
        public string Size { get; set; }
      
    }
}
