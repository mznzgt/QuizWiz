﻿using Azure.Storage.Blobs;
using QuizWiz.ApiService.Settings;
using QuizWiz.Application.QuizGenerator;
using QuizWiz.Persistence.BlobStorage;
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

            services.AddSingleton(settings.BlobStorageSettings);

            // Persistence
            // Register BlobServices
            services.AddScoped<IBlobService, BlobService>();

            // Application
            services.AddQuizGeneratorModule();
            return services;
        }
    }
}