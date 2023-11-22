using Azure.AI.OpenAI;
using System.Threading.Tasks;

namespace QuizWiz.Infrastructure.OpenAI
{
    public interface IOpenAIService
    {
        Task<ChatCompletions> GetChatCompletionsAsync(ChatCompletionsOptions options);
    }
}
