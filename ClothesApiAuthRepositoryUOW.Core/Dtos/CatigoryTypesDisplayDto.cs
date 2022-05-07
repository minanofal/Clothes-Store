using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Dtos
{
    public class CatigoryTypesDisplayDto
    {
        public int CategoryId { get; set; }
        [MaxLength(100)]
        public string CategoryName { get; set; }
        public IEnumerable< TypeDisplayDto> Types { get; set; }
    }
}
