using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Models
{
    public class Type
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Product> products { get; set; }

        public ICollection<Category> categories { get; set; }

        public List<Type_Category> Types_Categories { get; set; } 

    }
}
