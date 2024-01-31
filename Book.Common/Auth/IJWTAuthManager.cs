

namespace WebAPI.Middleware.Auth
{
    public interface IJWTAuthManager
    {
        string GenerateJWT(Book.Data.DBModel.ApplicationUser user, List<string> roles);
    }
}
