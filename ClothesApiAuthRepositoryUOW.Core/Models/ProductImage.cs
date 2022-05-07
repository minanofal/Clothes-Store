using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int productId { get; set; }
        public byte[] Image{ get; set; }
    }
}
