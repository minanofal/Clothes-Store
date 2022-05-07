using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }

        public char Gender { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public Type Type { get; set; }
        public int TypeId { get; set; }

        public IEnumerable< ProductImage> Images { get; set; }

        public IEnumerable<Product_Color_Size_Dto> Product_Color_Sizes { get; set; }

        public IEnumerable<Size> sizes { get; set; }

        public IEnumerable<Color>  colors { get; set; }

    }
}
