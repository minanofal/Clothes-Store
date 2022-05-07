using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public ICollection<Type> Types { get; set; }

        public List<Type_Category> Types_Categories { get; set; }

    }
}
