using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace QuizWiz.Application.SharedModel
{
    public class QuizResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SubjectDomain { get; set; } = "Quiz";
        public string Email { get; set; }
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

    public class CosmosEmailQueryResponse
    {
        public Guid Id { get; set; }
        public string Topic { get; set; }
        public string Email { get; set; }
    }
}
