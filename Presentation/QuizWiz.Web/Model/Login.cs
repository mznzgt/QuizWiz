﻿using System.ComponentModel.DataAnnotations;

namespace QuizWiz.Web.Model
{
    public class Login
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
