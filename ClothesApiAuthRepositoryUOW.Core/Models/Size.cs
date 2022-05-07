using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Models
{
    public class Size
    {
        public string size { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

    }
}
