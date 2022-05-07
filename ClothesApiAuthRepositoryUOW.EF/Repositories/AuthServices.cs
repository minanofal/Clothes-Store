using AutoMapper;
using ClothesApiAuthRepositoryUOW.Core.Dtos;
using ClothesApiAuthRepositoryUOW.Core.Interfaces;
using ClothesApiAuthRepositoryUOW.Core.Models;
using ClothesApiAuthRepositoryUOW.Core.SecurityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClothesApiAuthRepositoryUOW.EF.Repositories
{
    public class AuthServices : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _usermanager;

        private readonly JWT _jwt;

        private readonly IMapper _mapper;

        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthServices(UserManager<ApplicationUser> userManger, IOptions<JWT> jwt , IMapper mapper , RoleManager<IdentityRole> roleManager)
        {
            _usermanager = userManger;
            _jwt = jwt.Value;
            _mapper = mapper;
            _roleManager = roleManager;

        }

        public async Task<string> AddRole(ChangeRoleDto model)
        {
            var user = await _usermanager.FindByEmailAsync(model.Email);
            if (user == null || !await _roleManager.RoleExistsAsync(model.Role))
                return "Email or Role is Incorrect";

            var roles = await _usermanager.GetRolesAsync(user);
            roles = roles.ToList();

            if (roles.Contains(model.Role))
                return "the user in this role";

            var ressult = await _usermanager.AddToRoleAsync(user,model.Role);


            return ressult.Succeeded ? String.Empty : "Something went Wrong";
        }

        public async Task<AuthModelDto> LoginAsync(LoginDto model)
        {
            var user = await _usermanager.FindByEmailAsync(model.Email);
            if (user == null || !await _usermanager.CheckPasswordAsync(user, model.Password))
                return new AuthModelDto { Message = "Email Or Passwor Is Incorrect" };

            var jwtsecuritytoken = await CreateJwtToken(user);
            var roles = await _usermanager.GetRolesAsync(user);

            return new AuthModelDto
            {
                Message = "Login Successfully",
                Email   = user.Email,
                IsAuthenticated = true,
                Roles = roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtsecuritytoken),
                ExpireOn = jwtsecuritytoken.ValidTo,
                UserName= user.UserName
                
            };


        }

        public async Task<AuthModelDto> RegisterAsync(RegisterModelDto model)
        {
            if (await _usermanager.FindByEmailAsync(model.Email) != null)
                return new AuthModelDto { Message = "Email is already registered" };
            
            if(await _usermanager.FindByNameAsync(model.UserName) !=null)
                return new AuthModelDto { Message = "User Name is already registered" };

            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _usermanager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                if(result.Errors.Any())
                    foreach (var error in result.Errors)
                        errors += $"{error.Description} ";
                return new AuthModelDto { Message = errors };
            }
            await _usermanager.AddToRoleAsync(user, "User");

            var jwtsecuritytoken =await CreateJwtToken(user);



            return new AuthModelDto {
                Email = user.Email,
                IsAuthenticated = true,
                Roles = new List<string> { "User"},
                UserName =  user.UserName,
                Message = "Registered Successfully",
                Token = new JwtSecurityTokenHandler().WriteToken(jwtsecuritytoken),
                ExpireOn = jwtsecuritytoken.ValidTo


            };

        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _usermanager.GetClaimsAsync(user);
            var roles = await _usermanager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

    }
}
