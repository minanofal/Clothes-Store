using ClothesApiAuthRepositoryUOW.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.Core.Interfaces
{
    public interface IAuthServices
    {
        Task<AuthModelDto> RegisterAsync(RegisterModelDto model);

        Task<AuthModelDto> LoginAsync(LoginDto model);

        Task<string> AddRole(ChangeRoleDto model);


    }
}
