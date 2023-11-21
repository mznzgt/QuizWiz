//using Microsoft.Extensions.DependencyInjection;

//namespace QuizWiz.Infrastructure.OpenAI;

//public static class OpenAIModule
//{
//    public static IServiceCollection AddOpenAIServices(this IServiceCollection services, string proxyUrl, string apiKey, string githubAlias)
//    {
//        services.AddSingleton<IOpenAIService, OpenAIService>(provider =>
//        {
//            return new OpenAIService(proxyUrl, apiKey, githubAlias);
//        });

//        return services;
//    }
//}
