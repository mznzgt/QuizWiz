namespace QuizWiz.Web.Services
{
    public interface IJwtService
    {
        string GetRoleFromToken(string token);
    }
}
