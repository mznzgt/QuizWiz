using System.Collections.Generic;

namespace QuizWiz.Application.SharedModel
{
    public class QuizResponse
    {
        public string Topic { get; set; }
        public List<Quiz> Quiz { get; set; }
    }

    public class Quiz
    {
        public string Question { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
