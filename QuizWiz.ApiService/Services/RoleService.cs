using Microsoft.AspNetCore.Identity;
using QuizWiz.Data.Model;

namespace QuizWiz.ApiService.Services
{
    public static class RoleService
    {
        public static async Task<WebApplication> CreateRolesAsync(this WebApplication app, IConfiguration configuration)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));
            var roles = configuration.GetSection("Roles").Get<List<string>>();
            var adminRole = "Admin";

            roles.Add(adminRole);

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            return app;
        }
    }
}
