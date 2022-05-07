using ClothesApiAuthRepositoryUOW.Core.Dtos;
using ClothesApiAuthRepositoryUOW.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothesApiAuthRepositoryUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthsController(IAuthServices authServices)
        {
            _authServices = authServices;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModelDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authServices.RegisterAsync(model);
            if(!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authServices.LoginAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] ChangeRoleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authServices.AddRole(model);
            if(!string.IsNullOrEmpty(result))
                return BadRequest(result);
            return Ok(model);
        }



    }
}
