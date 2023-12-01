using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;
using QuizWiz.ApiService.Settings;
using QuizWiz.Application.QuizGenerator;
using QuizWiz.Persistence.BlobStorage;
using QuizWiz.Persistence.Cosmos;
using System.Reflection;

namespace QuizWiz.ApiService
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            // Add dependencies here
            var settings = configuration.GetSection(ApiServiceSettings.SectionName).Get<ApiServiceSettings>();

            services.AddSingleton(x => new BlobServiceClient(settings.BlobStorageSettings.ConnectionString));
            services.AddSingleton(x => new CosmosClient(configuration.GetConnectionString("CosmosConnectionName")));

            services.AddSingleton(settings.BlobStorageSettings);
            services.AddSingleton(settings.CosmosSettings);

            // Persistence
            // Register BlobServices
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<ICosmosService, CosmosService>();

            // Application
            services.AddQuizGeneratorModule();
            return services;
        }
    }
}
