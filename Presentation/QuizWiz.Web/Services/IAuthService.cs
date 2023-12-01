using QuizWiz.Web.Model;

namespace QuizWiz.Web.Services
{
    public interface IAuthService
    {
        Task<LoginResult> Login(Login loginModel);
        Task Logout();
        Task<RegisterResult> Register(Register registerModel);
    }
}
