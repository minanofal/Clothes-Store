﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Dtos
{
    public class AuthModelDto
    {
        public string? Message { get; set; }

        public string? UserName { get; set; }

        public bool IsAuthenticated { get; set; }

        public string? Email { get; set; }

        public string? Token { get; set; }

        public List<string> Roles { get; set; }

        public DateTime? ExpireOn { get; set; }
    }
}
