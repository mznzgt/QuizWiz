using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWix.Application.Auth
{
    public static class AuthDependencyInjection
    {
        public static IServiceCollection AddAuthApplicationModule(this IServiceCollection services)
        {
            services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(AuthDependencyInjection).Assembly));

            return services;

        }
    }
}
