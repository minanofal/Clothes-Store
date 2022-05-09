using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Consts
{
    public static class SizesConstants
    {
        public static readonly List<string> SizesList = new List<string>() { "S", "M","L","XL","XXL","XXXL","XXXXL","XXXXXL" };


        public  static string CheckSize(  IEnumerable<string> Sizes)
        {
            string Message = string.Empty;
            foreach (var _size in Sizes.Distinct())
            {

                if (!SizesList.Contains(_size.ToUpper()))
                {
                    var sizes = "";
                    foreach (var size in SizesList) { sizes = sizes + " " + size; }
                    return Message = $"only sizes {sizes}";
                }


                }
            return null;
            }
        }
    }

