using QuizWiz.Application.SharedModel;
using System.Net.Http.Headers;

namespace QuizWiz.Web.Services
{
    public interface IOpenAIService
    {
        Task<QuizResponse> GetQuizAsync(string userInput, string token);
    }

    public class OpenAIService : IOpenAIService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OpenAIService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<QuizResponse> GetQuizAsync(string userInput, string token)
        {
            var httpClient = _httpClientFactory.CreateClient("OpenAI");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(160));
            var response = await httpClient.PostAsJsonAsync("/api/OpenAI/quiz/create", userInput, cts.Token);

            return await response.Content.ReadFromJsonAsync<QuizResponse>();
        }
    }
}
