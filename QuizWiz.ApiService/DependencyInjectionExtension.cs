using Azure.Storage.Blobs;
using QuizWiz.ApiService.Settings;
using QuizWiz.Persistence.BlobStorage;

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

            services.AddSingleton(settings.BlobStorageSettings);

            // Register BlobServices

            services.AddScoped<IBlobService, BlobService>();

            return services;
        }
    }
}
