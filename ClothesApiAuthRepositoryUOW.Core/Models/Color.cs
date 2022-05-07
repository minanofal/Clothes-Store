using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Models
{
    public class Color
    {
        
        public string color { get; set; }
        public Product Product { get; set; }

        public int ProductId { get; set; }
    }
}
