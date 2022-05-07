using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Consts
{
    public static class ImageConstants
    {
        public static readonly List<string> ImageExtention = new List<string>() { ".png", ".jpg" };

        public const long MaxLength = 1048576;
    }
}
