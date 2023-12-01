﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace QuizWiz.Application.SharedModel
{
    public class QuizResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        // We can use this as partitionKey
        public string SubjectDomain { get; set; } = "Computers";
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
