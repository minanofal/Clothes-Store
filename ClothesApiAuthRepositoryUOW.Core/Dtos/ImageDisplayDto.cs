using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Dtos
{
    public class ImageDisplayDto
    {
        public int Id { get; set; }

        public byte[] Image { get; set; }
    }
}
