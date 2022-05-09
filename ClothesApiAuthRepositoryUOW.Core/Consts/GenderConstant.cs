using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Consts
{
    public static class GenderConstant
    {
        public static readonly List<string> GendersList = new List<string>() { "M" ,"F" , "MALE" ,"FEMALE" };


        public static string CheckGender(string Gender)
        {
            string Message = "Gender IS ";
            if (!GendersList.Contains(Gender.ToUpper()))
            {
                foreach (var gender in GendersList)
                {
                    Message += gender + " ";
                }
                return Message;
                 
            }

            return null;
        }
    }
}
