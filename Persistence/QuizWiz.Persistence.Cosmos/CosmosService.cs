using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using QuizWiz.Application.SharedModel;
using QuizWiz.Persistence.Cosmos.Settings;

namespace QuizWiz.Persistence.Cosmos
{
    public interface ICosmosService
    {
        Task<QuizResponse> CreateItemAsync(QuizResponse item);
        Task<QuizResponse> GetItemAsync(string itemId, string partitionKey);
        Task<IEnumerable<CosmosEmailQueryResponse>> GetDocumentsByPartitionKeyAsync(string email);
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
                var response = await _container.CreateItemAsync(item, new PartitionKey(item.Email));
                return response.Resource;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception($"Error creating item: {ex.Message}");
            }
        }

        public async Task<QuizResponse> GetItemAsync(string itemId, string partitionKey)
        {
            try
            {
                var response = await _container.ReadItemAsync<QuizResponse>(itemId, new PartitionKey(partitionKey));
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

        public async Task<IEnumerable<CosmosEmailQueryResponse>> GetDocumentsByPartitionKeyAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException($"{nameof(email)} cannot be null or empty");
            }

            var linqQueryable = _container.GetItemLinqQueryable<QuizResponse>(requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(email)
            });

            var iterator = linqQueryable
                .Where(q => q.Email == email)
                .Select(q => new CosmosEmailQueryResponse
                {
                    Id = q.Id,
                    Topic = q.Topic,
                    Email = q.Email
                })
                .ToFeedIterator();

            var results = new List<CosmosEmailQueryResponse>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }
    }
}
