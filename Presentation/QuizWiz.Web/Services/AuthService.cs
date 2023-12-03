using QuizWiz.Web.Model;
using System.Text;
using Newtonsoft.Json;
using QuizWiz.Domain.Models;

namespace QuizWiz.Web.Services
{
    public interface IAuthService
    {
        Task<LoginResponseModel> LoginAsync(UserLoginModel userLoginModel);
        Task LogoutAsync();
        Task<string> RegisterAsync(Register registerModel);
    }

    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<LoginResponseModel> LoginAsync(UserLoginModel userLoginModel)
        {
            var httpClient = _httpClientFactory.CreateClient("Auth");
            using StringContent loginContent = new(System.Text.Json.JsonSerializer.Serialize(userLoginModel),Encoding.UTF8,"application/json");

            var response = await httpClient.PostAsync("/login", loginContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(responseContent);

            return loginResponse;
        }

        public async Task LogoutAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("Auth");
            await httpClient.PostAsync("/logout", null);


        }

        public async Task<string> RegisterAsync(Register register)
        {
            var httpClient = _httpClientFactory.CreateClient("Auth");

            using StringContent registerContent = new(System.Text.Json.JsonSerializer.Serialize(register),Encoding.UTF8,"application/json");
            using HttpResponseMessage response = await httpClient.PostAsync("/register", registerContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                throw new Exception($"Registration failed: {response.StatusCode}");
            }
        }
    }
}
