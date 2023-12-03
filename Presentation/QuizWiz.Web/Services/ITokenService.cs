namespace QuizWiz.Web.Services
{
    public interface ITokenService
    {
        Task ClearTokenAsync();
        Task<string> GetTokenAsync();
        Task StoreTokenAsync(string token);
    }
}
