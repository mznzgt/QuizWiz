using Microsoft.AspNetCore.Identity;

namespace QuizWiz.ApiService
{
    public class Participant : IdentityUser
    {
        public string Role { get; set; }
    }
}
