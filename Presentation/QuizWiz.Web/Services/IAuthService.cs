using QuizWiz.Domain.Models;
using QuizWiz.Web.Model;

namespace QuizWiz.Web.Services
{
    public interface IAuthService
    {
        Task<LoginResponseModel> LoginAsync(UserLoginModel userLoginModel);
        Task LogoutAsync();
        Task<string> RegisterAsync(Register registerModel);
    }
}
