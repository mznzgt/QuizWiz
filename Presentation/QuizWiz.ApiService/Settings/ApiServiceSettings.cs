using QuizWiz.Infrastructure.OpenAI;
using QuizWiz.Persistence.BlobStorage.Settings;
namespace QuizWiz.ApiService.Settings
{
    public class ApiServiceSettings
    {
        public const string SectionName = "ApiServiceSettings";
        public BlobStorageSettings BlobStorageSettings { get; set; }
        public OpenAIServiceSettings OpenAIServiceSettings { get; set; }
    }
}
