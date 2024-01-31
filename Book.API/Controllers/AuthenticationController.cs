using Book.Business.Contract;
using Book.Data.Contract.ViewModels.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationManager _iauthenticationManager;

        public AuthenticationController(IAuthenticationManager iauthenticationManager)
        {
            _iauthenticationManager = iauthenticationManager;
        }

        [HttpPost(nameof(Login))]
        [AllowAnonymous]
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var result = await _iauthenticationManager.Login(request);
            return result;
        }

      
        [HttpPost(nameof(UserRegistration))]
        [AllowAnonymous]
        public async Task<string> UserRegistration(ApplicationUserRequest model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var result = await _iauthenticationManager.CreateNewUser(model);
            return result;
        }
        [HttpPost(nameof(EditUser))]
        public async Task<LoginResponse> EditUser([FromForm] ApplicationUserRequest model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var result = await _iauthenticationManager.EditUser(model);
            return result;
        }

        [HttpGet(nameof(DeleteUser))]
        public async Task<string> DeleteUser(string? userId)
        {
            var result = await _iauthenticationManager.DeleteUser(userId);
            return result;
        }

        [HttpGet(nameof(GetUserById))]
        public async Task<UserVM> GetUserById(string userId)
        {
            var result = await _iauthenticationManager.GetUserById(userId);
            return result;
        }

        [HttpGet(nameof(GetUserList))]
        public async Task<IEnumerable<UserVM>> GetUserList()
        {
            var result = await _iauthenticationManager.GetUserList();
            return result;
        }
    }
}
