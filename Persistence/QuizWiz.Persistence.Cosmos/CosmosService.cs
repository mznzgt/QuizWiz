using Microsoft.Azure.Cosmos;
using QuizWiz.Application.SharedModel;
using QuizWiz.Persistence.Cosmos.Settings;

namespace QuizWiz.Persistence.Cosmos
{
    public interface ICosmosService
    {
        Task<QuizResponse> CreateItemAsync(QuizResponse item);
        Task<QuizResponse> GetItemAsync(string itemId);
    }
    public class CosmosService : ICosmosService
    {
        private readonly Container _container;
        public CosmosService(CosmosClient cosmosClient, CosmosSettings settings)
        {
            var database = cosmosClient.GetDatabase(settings.CosmosDatabase);
            _container = database.GetContainer(settings.CosmosContainer);
        }
        public async Task<QuizResponse> CreateItemAsync(QuizResponse item)
        {
            try
            {
                var response = await _container.CreateItemAsync(item, new PartitionKey(item.Topic));
                return response.Resource;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception($"Error creating item: {ex.Message}");
            }
        }

        public async Task<QuizResponse> GetItemAsync(string itemId)
        {
            try
            {
                var response = await _container.ReadItemAsync<QuizResponse>(itemId, new PartitionKey("USA"));
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return null;
                }
                return response.Resource;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error connecting to cosmos. Exception {ex.Message}");
            }
        }
    }
}
