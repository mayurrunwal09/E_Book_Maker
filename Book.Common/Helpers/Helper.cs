using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Test.Common.Helpers
{
    public static class Helper
    {
        public static DateTime GetCurrentUTCDatetime()
        {
            return DateTime.UtcNow;
        }
        public static DateTime GetCurrentUTCDate()
        {
            return DateTime.UtcNow.Date;
        }
        public static JwtSecurityToken GetToken(List<Claim> authClaims, string secret, string jwtIssuer, string jwtAudience)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var token = new JwtSecurityToken(
               issuer: jwtIssuer,
                audience: jwtAudience,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }                     
    }
}
