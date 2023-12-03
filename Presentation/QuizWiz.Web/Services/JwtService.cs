using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QuizWiz.Web.Services
{
    public interface IJwtService
    {
        string GetRoleFromToken(string token);
    }

    public class JwtService : IJwtService
    {
        public string GetRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var roleClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
            return roleClaim;
        }
    }
}
