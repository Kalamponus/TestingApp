using System;
using System.Windows.Input;
using TestingApp.Infrastructure.Commands;
using TestingApp.Infrastructure.Services;
using TestingApp.ViewModels.Base;
using TestingApp.Models.TestModels;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics.Eventing.Reader;
using System.Collections.ObjectModel;

namespace TestingApp.ViewModels
{
    internal class TestingPageViewModel : ViewModel
    {
        #region Поля
        public Test CurrentTest { get; set; }
        public string SomeTest { get; set; }
        public double FinalResult { get; set; }
        public int MaxResult { get; set; }
        #endregion

        Question question1 = new Question();
        Question question2 = new Question();
        Question question3 = new Question();
        Question question4 = new Question();

        #region Комманды       
        public ICommand NavigateMainMenuPageCommand { get; }  
        public ICommand NavigateFinalResultCommand { get; }
        public ICommand FinishTest { get; }
        
        private bool CanFinishTestCommandExecuted(object p) => true;

        private void OnFinishTestCommandExecuted(object p)
        {
            FinalResult = 0;
            foreach (Question question in CurrentTest.Questions)
            {
                int selectedCorrectAnswers = 0;
                foreach(Answer answer in question.Answers)
                {
                    if (!string.IsNullOrEmpty(answer.AnswerText))
                    {
                        if (question.QuestionType == QuestionType.TextQuestion && question.CorrectAnswers.Contains(answer.AnswerText.Trim().ToLower()))
                        {
                            FinalResult++;
                        }
                        else if (answer.IsSelected == true)
                        {
                            if (question.CorrectAnswers.Contains(answer.AnswerText))
                            {
                                selectedCorrectAnswers++;
                            }
                            else if (selectedCorrectAnswers > 0)
                            {
                                selectedCorrectAnswers--;
                            }
                        }
                    }                   
                }
                if (selectedCorrectAnswers > 0) FinalResult += (double)1 / question.CorrectAnswers.Count * selectedCorrectAnswers;
            }

            NavigateFinalResultCommand.Execute(p);
        }
        #endregion

        public TestingPageViewModel(NavigationStore navigationStore)
        {

            CurrentTest = new Test();

            #region Данные для захардкоженного теста

            question1.QuestionText = "Test1 fghddfghfhdgdfgh \n fghfghdfghhdfgdfghfdgh \n  fdghhdfgghdfhdgf\n fdhghdgfghfdhdfgfghdfghfghfhgfhgdfghdfhdgdfhfdfghfdghfg";
            question1.QuestionType = QuestionType.TextQuestion;
            question1.QuestionNumber = 1;
            question1.Answers = new ObservableCollection<Answer>() { new Answer()};
            question1.CorrectAnswers = new List<string>() { "answ1" };

            question2.QuestionText = "Test2";
            question2.QuestionType = QuestionType.TextQuestion;
            question2.QuestionNumber = 2;
            question2.Answers = new ObservableCollection<Answer>() { new Answer() };
            question2.CorrectAnswers = new List<string> (){ "answ1", "answ2" };

            question3.QuestionText = "Test3";
            question3.QuestionType = QuestionType.ImageQuestion;
            question3.QuestionNumber = 3;
            //question3.Answers = new List<Answer>() { new Answer() { Id = 1, AnswerText = new Uri("Data/Images/kolbasa.jpg") }, new Answer() { Id = 2, AnswerText = System.AppDomain.CurrentDomain + "Data/Images/mem.jpg" } };

            question4.QuestionText = "Test4";
            question4.QuestionType = QuestionType.MultiChoiseQuestion;
            question4.QuestionNumber = 4;
            question4.Answers = new ObservableCollection<Answer>() { new Answer() { AnswerText = "Hey3" }, new Answer() { AnswerText = "Hey4" }, new Answer() { AnswerText = "Hey6" }, new Answer() { AnswerText = "Hey7" }, new Answer() { AnswerText = "Hey8" } };
            question4.CorrectAnswers = new List<string>() { "Hey3", "Hey4", "Hey7" };

            #endregion
            CurrentTest.Questions = new ObservableCollection<Question>() { question1, question2, question4 };
            MaxResult = CurrentTest.Questions.Count;

            NavigateMainMenuPageCommand = new NavigateCommand<MainMenuPageViewModel>(navigationStore, () => new MainMenuPageViewModel(navigationStore));
            NavigateFinalResultCommand = new NavigateCommand<TestResultViewModel>(navigationStore, () => new TestResultViewModel(navigationStore, FinalResult, MaxResult));
            FinishTest = new LambdaCommand(OnFinishTestCommandExecuted, CanFinishTestCommandExecuted);
        }

    }

}
