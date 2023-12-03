using Newtonsoft.Json.Linq;
using QuizWiz.Application.SharedModel;
using System.Net.Http.Headers;

namespace QuizWiz.Web.Services
{
    public interface IStudentService
    {
        Task<QuizResponse> GetQuizAsync(string token);
    }

    public class StudentService : IStudentService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StudentService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<QuizResponse> GetQuizAsync(string token)
        {
            var httpClient = _httpClientFactory.CreateClient("Student");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = "/api/student/get/quiz";
            var itemId = "a09e8b52-fdc6-4e60-b4f3-ccb2d6d84423";

            var response = await httpClient.GetAsync($"{url}?itemId={itemId}");

            return await response.Content.ReadFromJsonAsync<QuizResponse>();
        }
    }
}
