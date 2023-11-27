using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuizWiz.Infrastructure.OpenAI;

public static class OpenAIDependencyInjection
{
    public static IServiceCollection AddOpenAIServices(
        this IServiceCollection services,
        string proxyUrl,
        string apiKey,
        string githubAlias)
    {
        services.Configure<OpenAIServiceSettings>(options =>
        {
            options.ProxyUrl = proxyUrl;
            options.ApiKey = apiKey;
            options.GitHubAlias = githubAlias;
        });

        services.AddScoped<IOpenAIService, OpenAIService>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<OpenAIService>>();
            var openAIOptions = provider.GetRequiredService<IOptions<OpenAIServiceSettings>>();
            return new OpenAIService(logger, openAIOptions);
        });

        return services;
    }
}
