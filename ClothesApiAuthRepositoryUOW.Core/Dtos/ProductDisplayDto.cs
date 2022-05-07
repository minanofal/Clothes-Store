using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Dtos
{
    public class ProductDisplayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public char Gender { get; set; }
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int TypeId { get; set; }
        public string TypeName { get; set; }

        public IEnumerable<string> Colors { get; set; }
        public IEnumerable<string> sizes { get; set; }

        public IEnumerable<Product_Color_Size>? product_Color_Sizes { get; set; }

        //public IEnumerable<byte[]> Images { get; set; }

        public IEnumerable<ImageDisplayDto> Images { get; set; }

      

        public string? Message { get; set; }
    }
}
