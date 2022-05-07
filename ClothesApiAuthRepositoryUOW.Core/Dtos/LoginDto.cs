using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Dtos
{
    public class LoginDto
    {
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
