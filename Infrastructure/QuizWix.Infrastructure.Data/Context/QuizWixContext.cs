using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizWiz.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWiz.Data.Context
{
    public class QuizWixContext : IdentityDbContext<User>
    {
        public QuizWixContext(DbContextOptions<QuizWixContext> options) : base(options)
        {
            
        }
    }
}
