using Microsoft.AspNetCore.Http;
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

        public static List<byte[]> ImageToByteArray(IEnumerable<IFormFile> ImageFiles)
        {
            var images = new List<byte[]>();
            using var datastream = new MemoryStream();
            foreach (IFormFile file in ImageFiles)
            {
                file.CopyTo(datastream);
                images.Add(datastream.ToArray());
            }
            
            return images;
        }
        public static string CheckImageValid(IEnumerable<IFormFile> ImageFiles)
        {
            string message = null;
            foreach (IFormFile file in ImageFiles)
            {
                if (!ImageExtention.Contains(Path.GetExtension(file.FileName).ToLower()))
                {
                    message = $"image {file.FileName} must be has one extention of ";
                    foreach (var ext in ImageExtention)
                    {
                        message += ext + " or ";
                    }
                }
                if (MaxLength < file.Length)
                {
                    message = $"image {file.FileName} must has a max size {MaxLength / 1048576}  MG ";
                }
            }
            return message;
        }
    }
}
