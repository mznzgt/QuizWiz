using QuizWiz.Web.Components.Pages;
using QuizWiz.Web.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace QuizWiz.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public Task<LoginResult> Login(Model.Login loginModel)
        {
            throw new NotImplementedException();
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<RegisterResult> Register(Register register)
        {
            var httpClient = _httpClientFactory.CreateClient("Auth");

            using StringContent registerContent = new(System.Text.Json.JsonSerializer.Serialize(register),Encoding.UTF8,"application/json");
            using HttpResponseMessage response = await httpClient.PostAsync("/account/register", registerContent);
            
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Response>(jsonResponse);

            return new RegisterResult
            {
                Successful = response.IsSuccessStatusCode,
                Response = res
            };
        }
    }
}
