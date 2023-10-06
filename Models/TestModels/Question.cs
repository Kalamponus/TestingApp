
using System.Collections.Generic;
using System.Collections.Specialized;

namespace TestingApp.Models.TestModels
{
    internal abstract class Question
    {

        public string? Number { get; set; }
        public string? QuestionText { get; set; }
        public string? QuestionType { get; set; }
        public string? RightAnswer { get; set; }

        public List<Answer>? Answers { get; set; }

    }
}
