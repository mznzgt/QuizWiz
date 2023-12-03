using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace QuizWiz.Web.Services
{
    public class TokenService : ITokenService
    {
        private readonly ProtectedSessionStorage _sessionStorage;

        public TokenService(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task StoreTokenAsync(string token)
        {
            await _sessionStorage.SetAsync("authToken", token);
        }

        public async Task<string> GetTokenAsync()
        {
            var result = await _sessionStorage.GetAsync<string>("authToken");
            return result.Success ? result.Value : null;
        }

        public async Task ClearTokenAsync()
        {
            await _sessionStorage.DeleteAsync("authToken");
        }
    }
}
