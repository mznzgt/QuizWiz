using System.ComponentModel.DataAnnotations;

namespace QuizWiz.Web.Model
{
    public class Register
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
