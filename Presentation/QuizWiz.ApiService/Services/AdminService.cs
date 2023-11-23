using Microsoft.AspNetCore.Identity;
using QuizWiz.Domain;

namespace QuizWiz.ApiService.Services
{
    public static class AdminService
    {
        public static async Task<WebApplication> AddAdminAsync(this WebApplication app, IConfiguration configuration)
        {
            using var scope = app.Services.CreateScope();
            var userManager = (UserManager<User>)scope.ServiceProvider.GetService(typeof(UserManager<User>));

            var adminRole = "Admin";

            var adminConfig = configuration.GetSection("Admin").Get<AdminUser>();

            var adminUser = await userManager.FindByEmailAsync(adminConfig.Email);
            if(adminUser == null)
            {
                var newAdmin = new User { Email = adminConfig.Email, UserName = adminConfig.Email };

                var result = await userManager.CreateAsync(newAdmin, adminConfig.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, adminRole);
                }
            }

            return app;
        }

        internal class AdminUser
        {
            public string Email { get; set; }
            public string Password { get; set; }    
        }
    }
}
