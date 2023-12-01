using QuizWiz.Infrastructure.OpenAI;
using QuizWiz.Persistence.BlobStorage.Settings;
using QuizWiz.Persistence.Cosmos.Settings;
namespace QuizWiz.ApiService.Settings
{
    public class ApiServiceSettings
    {
        public const string SectionName = "ApiServiceSettings";
        public BlobStorageSettings BlobStorageSettings { get; set; }
        public OpenAIServiceSettings OpenAIServiceSettings { get; set; }
        public CosmosSettings CosmosSettings { get; set; }
    }
}
