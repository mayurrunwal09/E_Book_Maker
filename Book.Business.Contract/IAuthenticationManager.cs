using Book.Data.Contract.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Business.Contract
{
    public interface IAuthenticationManager
    {
        Task<string> CreateNewUser(ApplicationUserRequest request);
        Task<LoginResponse> EditUser(ApplicationUserRequest request);
        Task<UserVM> GetUserById(string userId);
        Task<IEnumerable<UserVM>> GetUserList();
        Task<LoginResponse> Login(LoginRequest request);
        Task<string> DeleteUser(string? userId);

    }
}
