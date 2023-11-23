using Microsoft.Extensions.DependencyInjection;

namespace QuizWiz.Application.QuizGenerator
{
    public static class QuizGeneratorDependencyInjection
    {
        public static IServiceCollection AddQuizGeneratorModule(this IServiceCollection services)
        {
            services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(QuizGeneratorDependencyInjection).Assembly));

            return services;

        }
    }
}
