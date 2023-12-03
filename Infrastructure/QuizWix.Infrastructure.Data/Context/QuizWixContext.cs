using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizWiz.Domain.Entities;

namespace QuizWiz.Data.Context
{
    public class QuizWixContext : IdentityDbContext<User>
    {
        public QuizWixContext(DbContextOptions<QuizWixContext> options) : base(options)
        {
            
        }
    }
}
