namespace QuizWiz.Domain.Models
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Error { get; set; }
    }
}
