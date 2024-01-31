
using Book.Business.Contract;
using Book.Data.Contract.RepositoryInterfaces;
using Book.Data.Contract.ViewModels.Authentication;
using Book.Data.DBModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.Common.Helpers;

namespace Book.Business.Manager.Managers
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationRepository _iAuthenticationRepository;

        public AuthenticationManager(UserManager<ApplicationUser> userManager, IConfiguration configuration, IAuthenticationRepository iAuthenticationRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _iAuthenticationRepository = iAuthenticationRepository;
        }
        public async Task<string> CreateNewUser(ApplicationUserRequest request)
        {
            if (request == null)
            {
                return "Invalid request";
            }

            var objUser = await _userManager.FindByEmailAsync(request.Email);

            if (objUser != null)
            {
                return "User with the specified email already exists.";
            }

            ApplicationUser user = new()
            {
                SecurityStamp = Guid.NewGuid().ToString(),

                Name = request.Name,
               
                UserName = request.Email,
                Email = request.Email,
                Phoneno = request.Phoneno,
                Gender = request.Gender,
                Role = request.Role,

            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return $"Failed to create user. {result.Errors.FirstOrDefault()?.Description}";
            }

            return "User Created Succesfully";
        }

        public async Task<string> DeleteUser(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return "Invalid userId";
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return "User not found";
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return $"Failed to delete user. {result.Errors.FirstOrDefault()?.Description}";
            }

            return "User deleted successfully";
        }

        public async Task<LoginResponse> EditUser(ApplicationUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                return null;
            }

            user.Name = request.Name;
            user.Email = request.Email;
            user.Phoneno = request.Phoneno;
            user.Gender = request.Gender;
            user.Role = request.Role;


            await _userManager.UpdateAsync(user);

            var editedUser = new LoginResponse
            {
                UserId = user.Id,
                Name = $"{user.Name}",
                

            };

            return editedUser;
        }

        public async Task<UserVM> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            var userVM = new UserVM()
            {
                UserId = user.Id,
                Name = user.Name,
                Phoneno = user.Phoneno,
                Gender = user.Gender,
                Email = user.Email,
                Role = user.Role,
                

            };

            return userVM;
        }

        public async Task<IEnumerable<UserVM>> GetUserList()
        {
            var users = await _userManager.Users
               .Select(user => new UserVM
               {
                   UserId = user.Id,
                   Name = user.Name,
                   Phoneno = user.Phoneno,
                   Gender = user.Gender,
                   Email = user.Email,
                   Role = user.Role,
                  
                   
                  
               })
               .ToListAsync();

            return users;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (user != null && isValidPassword)
            {
                var authClaims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.UserName),
    new Claim(ClaimTypes.Role, user.Role),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    new Claim("userId", user.Id),
};

                JwtSecurityToken generatedToken = Helper.GetToken(authClaims, _configuration["JWT:Secret"], _configuration["JWT:ValidIssuer"], _configuration["JWT:ValidAudience"]);

                var loginResponse = new LoginResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(generatedToken),
                    UserId = user.Id,
                    Name = $"{user.Name} {user.Email}",
                };

                return loginResponse;
            }
            return null;
        }
    }
}
