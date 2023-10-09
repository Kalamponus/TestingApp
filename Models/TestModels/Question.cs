
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace TestingApp.Models.TestModels
{
    internal class Question
    {

        public int? QuestionNumber { get; set; }
        public string? QuestionText { get; set; }
        public string? QuestionImage { get; set; }
        public QuestionType QuestionType { get; set; }
        public List<string>? CorrectAnswers { get; set; }
        public ObservableCollection<Answer>? Answers { get; set; }

    }
}
