using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Models
{
    public class Type_Category
    {
        public Type Type { get; set; }
        public int TypeId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; } 
    }
}
