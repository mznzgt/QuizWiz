using QuizWiz.Application.SharedModel;
using System.Net.Http.Headers;

namespace QuizWiz.Web.Services
{
    public interface IStudentService
    {
        Task<QuizResponse> GetQuizAsync(string token, string itemId, string email);
        Task<IEnumerable<CosmosEmailQueryResponse>> GetQuizByEmailAsync(string token, string email);
    }

    public class StudentService : IStudentService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StudentService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<QuizResponse> GetQuizAsync(string token, string itemId, string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(itemId))
            {
                throw new ArgumentNullException($"{nameof(email)} or {nameof(itemId)} cannot be null or empty");
            }

            var httpClient = _httpClientFactory.CreateClient("Student");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = "/api/student/get/quiz/id";

            var response = await httpClient.GetAsync($"{url}?itemId={itemId}&partitionKey={email}");

            return await response.Content.ReadFromJsonAsync<QuizResponse>();
        }

        public async Task<IEnumerable<CosmosEmailQueryResponse>> GetQuizByEmailAsync(string token, string email)
        {
            var httpClient = _httpClientFactory.CreateClient("Student");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = "/api/student/get/quiz/email";
            var response = await httpClient.GetAsync($"{url}?email={email}");

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<CosmosEmailQueryResponse>>();

            return result;
        }
    }
}
