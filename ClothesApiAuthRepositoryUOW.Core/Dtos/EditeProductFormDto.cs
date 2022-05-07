using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Dtos
{
    public class EditeProductFormDto
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public string Gender { get; set; }

        public decimal Price { get; set; }
        public int CategoryId { get; set; }


        public int TypeId { get; set; }

        public IEnumerable<IFormFile>? AddedImages { get; set; }

        public IEnumerable<int>? DeletedImages { get; set; }


        public IEnumerable<string> Sizes { get; set; }
        public IEnumerable<string> Colors { get; set; }

       
    }
}
