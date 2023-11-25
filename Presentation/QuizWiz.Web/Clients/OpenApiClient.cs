using QuizWiz.Application.SharedModel;
using System.Net.Http.Json;

namespace QuizWiz.Web.Clients
{
    public class OpenApiClient(HttpClient httpClient)
    {
        public async Task<QuizResponse> GetQuizAsync(string userInput)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(160));
            var response = await httpClient.PostAsJsonAsync("/api/OpenAI/quiz/create", userInput, cts.Token);
            return await response.Content.ReadFromJsonAsync<QuizResponse>();
        }
    }
}
